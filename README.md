
![](https://avatars2.githubusercontent.com/u/52544889?s=400&u=b9887976a97a065b507224a1b51c70865a1e40ab&v=4)

## A simple/fast loot system.

Why another loot system? Because all of the existing loot systems didn't do what we wanted, were 3-4 times as large from a code base perspective, and painfully slow which can have serious impact on user and server performance. 

We wanted the following attributes to ensure performance:

- unique loot in crates without intense looping/retrying
- stacked loot
- fixed scrap amounts
- random versus probable
  - random number of loot items within range
  - random min/max stacking within range
    - min/max respected (if min is 12 and max is 12, put 12 pieces of looot in crate)
  - no duplicates
- we do not attempt to generate any config code
- we do not account for prefabs (crates), but we provide a generator tool that does.
- we do not rely on or use Rust/Facepunch DLL's outside of what `oxide` uses
  - less chances of breakage when API's change (and they do)

## Requirements

- rust/oxide server
- generate your `DecayLoot.json` config first and move it to the `oxide/config/` directory in oxide
- move `DecayLoot.cs`  into `plugins/` directory
- reload plugin
- containers drop and pick up new loot

## Todo
- skinnable loot
- rarity indexing
  - key for this exists, just need to implement
- blueprint boolean (give blueprints?)
  - we can give a blue print or not
    - we may implement a dice roll (TBD)

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

## Why not use BetterLoot?

We started here but quickly learned that the plugin wasn't doing exactly what we wanted. As engineers we decided to take a peek under the covers. We found a lot of useless code, innaccurate data being displayed to the user/logs, too many looping constucts, and high cyclomatic complexity between functions/methods. Last but not least, we also noticed the BetterLoot plugin caused the game engine to chirp/halt on a 4 Core, 16GB server with no users. 670 lines of code versus 150 lines of code, you do the math.
