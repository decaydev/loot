
<img src="https://i.ibb.co/2dGJm44/logo.png" alt="decay.dev" width="255"/><img src="https://i.ibb.co/vvRfnRb/loot.png" alt="decay.dev/loot" width="255"/>

## A simple/fast loot system. 

Why another loot system? Because all of the existing loot systems didn't do what exactly what we wanted and because the behavior of the plugins was too complicated.

## Requirement

We wanted the following attributese:

- unique loot in crates without intense looping/retrying
  - arrays where sensible/possible
- stacked loot
- fixed scrap amounts
- random versus probable
  - random number of loot items within range
  - random min/max stacking within range
    - min/max respected (if min is 5 and max is 5, put 5 damn pieces of loot in the crate)
  - no duplicates
- we do not attempt to generate any config code
- we do not account for prefabs (crates), but we provide a generator tool that does.
- we do not rely on or use Rust/Facepunch DLL's outside of what `oxide` uses
  - less chances of breakage when API's change (and they do)
- accurate counting and logging
- We are faster than **BetterLoot** and we do it in less then 150 lines of code versus 600+
  - to be fair see scroll up - we do not attempt to generate any config code


## Requirements

- rust/oxide server
- generate your `DecayLoot.json` config first and move it to the `oxide/config/` directory in oxide
- move `DecayLoot.cs`  into `oxide/plugins/` directory
- reload plugin
- containers drop and pick up new loot

## Todo
- skinnable loot
- rarity indexing
  - key exists for future work
- blueprint boolean
  - we may implement a random bool if true, else no blueprints

### LootTable Example:
```json
[
  {
    "blueprints": true,
    "items": [
      {
        "shortname": "barricade.stone.blueprint",
        "min": 1,
        "max": 1,
        "rar": 0
      },
      {
        "shortname": "barricade.wood",
        "min": 1,
        "max": 1,
        "rar": 0
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

# BetterLoot

If you're currently using BetterLoot we provide a tool for converting your `JSON` configuration to the DecayLoot format. 
