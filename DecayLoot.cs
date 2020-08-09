using System.Collections.Generic;
using System.Linq;
using Random = System.Random;


namespace Oxide.Plugins
{
	[Info("DLoot", "decay.dev", "0.0.1")]
	[Description("add loot to tables")]

	public class DLoot : RustPlugin
	{

		bool initialized = false;

		private ConfigData configData;

		void OnServerInitialized()
		{
			ItemManager.Initialize();
			UpdateInternals();
		}

		void UpdateInternals()
		{
			int populatedContainers = 0;
			NextTick(() => {
				foreach (var container in BaseNetworkable.serverEntities.Where(p => p != null && p.GetComponent<BaseEntity>() != null && p is LootContainer).Cast<LootContainer>().ToList())
				{
					foreach (ConfigData.Prefab prefab in configData.Prefabs)
					{
						if (container.PrefabName.Contains(prefab.id))
						{
							PopulateContainer(container, prefab);
							populatedContainers++;
							break;
						}
					}
				}
				Puts($"Populated {populatedContainers} supported containers.");
				initialized = true;
			});
		}

		bool PopulateContainer(LootContainer container, ConfigData.Prefab prefab)
		{
			if (container == null || prefab == null) return false;
			int itemsCount;
			var rng = new Random();
			if (prefab.item_range[0] == prefab.item_range[1]) itemsCount = prefab.item_range[1];
			else itemsCount = rng.Next(prefab.item_range[0], prefab.item_range[1]);
			container.inventory.Clear();
			ItemManager.DoRemoves();
			container.scrapAmount = prefab.scrap;
			container.inventory.capacity = itemsCount;
			var sample = new List<ConfigData.Prefab.Item>(prefab.items);

			for (var i = 0; i < (itemsCount > 0 ? itemsCount : 1) - 1; i++)
			{
				ConfigData.Prefab.Item itemSpec = sample.GetRandom();
				if (!sample.Remove(itemSpec)) PrintWarning("error removing item from sample: {0}", itemSpec.shortname);
				var itemDef = ItemManager.FindItemDefinition(itemSpec.shortname.Replace(".blueprint", ""));
				var itemAmount = rng.Next(itemSpec.min, itemSpec.max);
				var ranged = new ItemAmountRanged(itemDef, itemAmount > 1 ? itemAmount : 1f);
				Item item;
				if (ranged.itemDef.spawnAsBlueprint)
				{
					ItemDefinition blueprintBaseDef = ItemManager.FindItemDefinition("blueprintbase");
					if (blueprintBaseDef == null)
					{
						PrintWarning("blueprintbase is null for item: {0}", itemSpec.shortname);
						continue;
					}
					item = ItemManager.Create(blueprintBaseDef, 1, 0uL);
					item.blueprintTarget = ranged.itemDef.itemid;
				}
				else
				{
					item = ItemManager.CreateByItemID(ranged.itemid, (int)ranged.GetAmount(), 0uL);
				}
				if (item == null)
				{
					continue;
				}
				bool allowStack = !new[] {
					ItemCategory.Weapon,
				}.Contains(itemDef.category);
				if (!item.MoveToContainer(container.inventory, -1, allowStack))
				{
					if ((bool)container.inventory.playerOwner)
					{
						item.Drop(container.inventory.playerOwner.GetDropPosition(), container.inventory.playerOwner.GetDropVelocity());
					}
					else
					{
						item.Remove();
					}
				}
			}
			container.GenerateScrap();
			container.SendNetworkUpdate();
			return true;
		}

		object OnLootSpawn(LootContainer container)
		{
			if (!initialized || container == null) return null;
			foreach (ConfigData.Prefab prefab in configData.Prefabs)
			{
				if (container.PrefabName.Contains(prefab.id))
				{
					if (PopulateContainer(container, prefab))
					{
						ItemManager.DoRemoves();
						return true;
					}
					else return null;
				}
			}
			return null;
		}

		class ConfigData
		{
			public Prefab[] Prefabs
			{
				get;
				set;
			}
			public class Prefab
			{
				public string id;
				public int blueprints;
				public List<Item> items;
				public int[] item_range;
				public int scrap;

				public class Item
				{
					public string shortname;
					public int min;
					public int max;
					public int rar;
				}
			}
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
	}
}
