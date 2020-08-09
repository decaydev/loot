# DecayLoot

## A simple/fast loot system.

Why another loot system? Because all of the existing loot systems didn't do what we wanted, were 3-4 times as large from a code base perspective, and painfully slow which can have serious impact on user and server performance. We wanted the following attributes:

- unique loot in crates without intense looping/retrying
- stacked loot
- random versus probable
  - random number of loot items within range
  - random min/max stacking within range
    - min/max respected (if the min is 12 and the max is 12, generate 12 pieces of loot)
  - no duplicates
- we do not attempt to generate any config code
  - this means much less code
- we do not account for prefabs (crates), but we provide a generator tool that does.
- does not rely on or use Rust/Facepunch DLL's
  - less chances of breakage when API's change (and they do)

## Todo
- skinnable loot
- account for rarity
- blueprint boolean (give blueprints?)

### LootTable Example (not compatible with BetterLoot):
```json
[
   {
      "id":"assets/bundled/prefabs/radtown/loot_barrel_2.prefab",
      "blueprints":true,
      "items":[
         {
            "shortname":"barricade.stone.blueprint",
            "min":1,
            "max":1,
            "rar":0
         },
         {
            "shortname":"barricade.wood",
            "min":1,
            "max":1,
            "rar":0
         }
      ],
      "item_range":[
         1,
         2
      ],
      "scrap":100
   }
]
```
