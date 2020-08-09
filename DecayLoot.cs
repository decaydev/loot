using System;
using System.Collections.Generic;
using System.Linq;

namespace Oxide.Plugins
{
    [Info("DecayLoot", "decay.dev", "0.0.1")]
    [Description("manage loot")]
    public class DLoot : RustPlugin
    {
        private ConfigData configData;

        private bool initialized;

        private void OnServerInitialized()
        {
            ItemManager.Initialize();
            UpdateInternals();
        }

        private void UpdateInternals()
        {
            var populatedContainers = 0;
            NextTick(() =>
            {
                foreach (var container in BaseNetworkable.serverEntities
                    .Where(p => p != null && p.GetComponent<BaseEntity>() != null && p is LootContainer)
                    .Cast<LootContainer>().ToList())
                foreach (var prefab in configData.Prefabs)
                    if (container.PrefabName.Contains(prefab.id))
                    {
                        PopulateContainer(container, prefab);
                        populatedContainers++;
                        break;
                    }
                Puts($"[DecayLoot]: {populatedContainers} containers uptdate.");
                initialized = true;
            });
        }

        private bool PopulateContainer(LootContainer container, ConfigData.Prefab prefab)
        {
            if (container == null || prefab == null) return false;
            int itemsCount;
            var rng = new Random();
            itemsCount = prefab.item_range[0] == prefab.item_range[1] ? prefab.item_range[1] : rng.Next(prefab.item_range[0], prefab.item_range[1]);
            container.inventory.Clear();
            ItemManager.DoRemoves();
            container.scrapAmount = prefab.scrap;
            container.inventory.capacity = itemsCount;
            var sample = new List<ConfigData.Prefab.Item>(prefab.items);

            for (var i = 0; i < (itemsCount > 0 ? itemsCount : 1) - 1; i++)
            {
                var itemSpec = sample.GetRandom();
                sample.Remove(itemSpec);
                var itemDef = ItemManager.FindItemDefinition(itemSpec.shortname.Replace(".blueprint", ""));
                var itemAmount = rng.Next(itemSpec.min, itemSpec.max);
                var ranged = new ItemAmountRanged(itemDef, itemAmount > 1 ? itemAmount : 1f);
                Item item;
                if (ranged.itemDef.spawnAsBlueprint)
                {
                    var blueprintBaseDef = ItemManager.FindItemDefinition("blueprintbase");
                    if (blueprintBaseDef == null)
                    {
                        PrintWarning("blueprintbase is null for item: {0}", itemSpec.shortname);
                        continue;
                    }

                    item = ItemManager.Create(blueprintBaseDef);
                    item.blueprintTarget = ranged.itemDef.itemid;
                }
                else
                {
                    item = ItemManager.CreateByItemID(ranged.itemid, (int) ranged.GetAmount());
                }

                if (item == null) continue;
                var allowStack = !new[]
                {
                    ItemCategory.Weapon
                }.Contains(itemDef.category);
                if (!item.MoveToContainer(container.inventory, -1, allowStack))
                {
                    if ((bool) container.inventory.playerOwner)
                        item.Drop(container.inventory.playerOwner.GetDropPosition(),
                            container.inventory.playerOwner.GetDropVelocity());
                    else
                        item.Remove();
                }
            }

            container.GenerateScrap();
            container.SendNetworkUpdate();
            return true;
        }

        private object OnLootSpawn(LootContainer container)
        {
            if (!initialized || container == null) return null;
            foreach (var prefab in configData.Prefabs)
                if (container.PrefabName.Contains(prefab.id))
                {
                    if (PopulateContainer(container, prefab))
                    {
                        ItemManager.DoRemoves();
                        return true;
                    }

                    return null;
                }

            return null;
        }

        protected override void LoadConfig()
        {
            base.LoadConfig();
            configData = Config.ReadObject<ConfigData>();
            Config.WriteObject(configData, true);
        }

        protected override void LoadDefaultConfig()
        {
            LoadConfig();
        }

        private class ConfigData
        {
            public Prefab[] Prefabs { get; set; }

            public class Prefab
            {
                public int blueprints;
                public string id;
                public int[] item_range;
                public List<Item> items;
                public int scrap;

                public class Item
                {
                    public int max;
                    public int min;
                    public int rar;
                    public string shortname;
                }
            }
        }
    }
}
