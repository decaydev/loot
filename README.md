<img src="https://raw.githubusercontent.com/decaydev/art/master/decay.png" width=192><img src="https://raw.githubusercontent.com/decaydev/art/master/plugins/loot.png" width=192>

# DecayLoot - NOW WITH PR!!!!!

## Plugin Attributes

- unique loot in crates without intense looping/retrying
  - arrays where sensible/possible
    - int over float
    - min/max range types
  - no duplicate items
  - no empty slots in crates (less calculations)
- guaranteed scrap ranges  
- stacked loot
- blueprints support for individual items 
- fixed scrap amounts
- random versus probable
  - random number of loot items within range
  - random min/max stacking within range
    - min/max respected (if min is 5 and max is 5, put 5 pieces of loot in the crate)
- does not attempt to generate any base config code
- does not account for prefabs (crates), but we provide a nice generator tool that does [https://decay.dev/loot](https://decay.dev/loot).
- does not rely on or use Unity/Rust/Facepunch `DLL`'s outside of what `oxide` uses
```
  using Newtonsoft.Json;
  using System;
  using System.Collections.Generic;
  using System.Linq;
```

## Caveats

- If you use `scrap_range`, and a max items of `1`, **scrap will always take precedence as the single item** regardless of the items you added to the table.
- If you choose a guaranteed scrap range, and also choose to add an additional scrap resource to the crate as an item, scrap will be stacked.
- We don't account for rarity of items

## Install/Requirements

- rust/oxide server
- generate your `DecayLoot.json` config **FIRST** and move it to the `oxide/config/` directory in oxide
- move `DecayLoot.cs`  into `oxide/plugins/` directory
- reload plugin

## DecayLoot Table Generator

You can run this [yourself](https://github.com/decaydev/lootapp) or use the latest version here [https://decay.dev/loot](https://decay.dev/loot)

![](https://i.ibb.co/zn3QjF7/screen-shot-2020-08-23-at-1-24-43-pm.png)

### DecayLoot JSON Schema
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
