<img src="https://i.ibb.co/93mSYZ4/decay.png" alt="decay.dev" width="255"/><img src="https://i.ibb.co/zbkjYkV/loot.png" alt="decay.dev/loot" width="255"/>

## DecayLoot



## Requirements/Attributes

- unique loot in crates without intense looping/retrying
  - arrays where sensible/possible
  - no duplicate items
- stacked loot
- fixed scrap amounts
- random versus probable
  - random number of loot items within range
  - random min/max stacking within range
    - min/max respected (if min is 5 and max is 5, put 5 pieces of loot in the crate)
- we do not attempt to generate any base config code
- we do not account for prefabs (crates), but we provide a generator tool that does.
- we do not rely on or use Rust/Facepunch DLL's outside of what `oxide` uses
- accurate counting and logging

## Requirements

- rust/oxide server
- generate your `DecayLoot.json` config first and move it to the `oxide/config/` directory in oxide
- move `DecayLoot.cs`  into `oxide/plugins/` directory
- reload plugin
- containers drop and pick up new loot

## Todo
- skinnable loot

### LootTable Example:
```json
[
  {
    "blueprints": 0,
    "items": [
      {
        "shortname": "barricade.stone.blueprint",
        "range": [
          0,
          1
        ]
      },
      {
        "shortname": "barricade.wood",
        "range": [
          0,
          1
        ]
      }
    ],
    "item_range": [
      1,
      2
    ],
    "scrap_range": [
      1,
      100
    ]
  }
]
```
