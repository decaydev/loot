<img src="https://i.ibb.co/93mSYZ4/decay.png" alt="decay.dev" width="255"/><img src="https://i.ibb.co/zbkjYkV/loot.png" alt="decay.dev/loot" width="255"/>

# DecayLoot

## Plugin Attributes

- unique loot in crates without intense looping/retrying
  - arrays where sensible/possible
    - int over float
  - no duplicate items
- stacked loot
- blueprints support for individual items
- fixed scrap amounts
- random versus probable
  - random number of loot items within range
  - random min/max stacking within range
    - min/max respected (if min is 5 and max is 5, put 5 pieces of loot in the crate)
- we do not attempt to generate any base config code
- we do not account for prefabs (crates), but we provide a nice generator tool that does [https://decay.dev/loot](https://decay.dev/loot).
- we do not rely on or use Rust/Facepunch DLL's outside of what `oxide` uses
```
  using System;
  using System.Collections.Generic;
  using System.Linq;
```

## Install/Requirements (important!!!)

- rust/oxide server
- generate your `DecayLoot.json` config **FIRST** and move it to the `oxide/config/` directory in oxide
- move `DecayLoot.cs`  into `oxide/plugins/` directory
- reload plugin

## Loot Table Generator

You can run this (yourself)[https://github.com/decaydev/lootapp] or use the latest version here [https://decay.dev/loot](https://decay.dev/loot)

![](https://i.ibb.co/zn3QjF7/screen-shot-2020-08-23-at-1-24-43-pm.png)

### LootTable JSON Schema
```json
{
  "prefabs": [
    {
      "id": "assets/bundled/prefabs/autospawn/resource/loot/loot-barrel-1.prefab",
      "scrap_range": [
        0,
        0
      ],
      "item_range": [
        0,
        0
      ],
      "items": [
        {
          "shortname": "riflebody",
          "range": [
            0,
            0
          ]
        },
        {
          "shortname": "ammo.shotgun",
          "range": [
            0,
            1
          ]
        }
      ]
    }
  ]
}
```
