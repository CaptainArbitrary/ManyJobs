## Release 1.6.1

### Bug Fixes

**Change patchop to PatchOperationReplace** ([PR #113](https://github.com/CaptainArbitrary/ManyJobs/pull/113))

- Change value from `<li>MJobs_PriorityHauling</li>` to `<workType>MJobs_PriorityHauling</workType>`

## Release 1.6.0

### New Features

**Added support for Vanilla Ideology Expanded - Memes and Structures** ([PR #93](https://github.com/CaptainArbitrary/ManyJobs/pull/93))

uninstall ancient junk: was hauling, is now deconstructing

**Added support for Alpha Biomes** ([PR #95](https://github.com/CaptainArbitrary/ManyJobs/pull/95))

drill at core sample drill → drilling
operate exotic smelter → smelting

**Added support for Alpha Genes** ([PR #97](https://github.com/CaptainArbitrary/ManyJobs/pull/97))

- administer metal to patients is now a caregiver job
- deliver metal to prisoners is now a caregiver job

**Added support for Biomes! Caverns** ([PR #98](https://github.com/CaptainArbitrary/ManyJobs/pull/98))

Filling and emptying mushroom fermenting barrels are now priority hauling jobs

**Added support for Gene Extractor Tiers** ([PR #99](https://github.com/CaptainArbitrary/ManyJobs/pull/99))

Haul to/Carry to gene extractor vat → priority haul

**Added support for Tiered Mechs** ([PR #101](https://github.com/CaptainArbitrary/ManyJobs/pull/101))

"Plus" and "Ultra" mech variants can now do the same jobs as their Biotech counterparts

**Added support for Gene Ripper** ([PR #102](https://github.com/CaptainArbitrary/ManyJobs/pull/102))

Carry to Gene Ripper is now a priority hauling job

**Added support for Vanilla Brewing Expanded and the Coffee and Tea add-on** ([PR #103](https://github.com/CaptainArbitrary/ManyJobs/pull/103))

Mixing drinks at the bar and making drinks at the espresso machine are both brewing jobs now

**Added support for Vanilla Factions Expanded - Insectoids** ([PR #104](https://github.com/CaptainArbitrary/ManyJobs/pull/104))

Inserting genomes is now a priority hauling job

**Added support for Vanilla Plants Expanded - More Plants** ([PR #106](https://github.com/CaptainArbitrary/ManyJobs/pull/106))

If Vanilla Plants Expanded - More Plants is active _and_ Vanilla Cooking Expanded is active, then the "extract milk from plants" job gets reassigned to the brewing category.

**Added support for Vanilla Races Expanded - Android** ([PR #107](https://github.com/CaptainArbitrary/ManyJobs/pull/107))

- Repairing and operating on androids are now fabrication jobs
- Carrying/hauling to the subcore polyanalyzer are priority hauling jobs
- Carrying to the android behaviorist station is a priority hauling job

**Added support for Vanilla Races Expanded - Sanguophage** ([PR #108](https://github.com/CaptainArbitrary/ManyJobs/pull/108))

Refilling drain caskets is now a priority hauling job

**Added support for Vehicle Framework** ([PR #109](https://github.com/CaptainArbitrary/ManyJobs/pull/109))

Refueling vehicles, rearming turrets and loading upgrade materials are all priority hauling jobs. Packing vehicles (in the caravan sense) is a loading job.

### Enhancements

**Updated the workTypes of some Biotech work givers** ([PR #76](https://github.com/CaptainArbitrary/ManyJobs/pull/76))

Change DeliverHemogenToPrisoner to MJobs_Caring.
Change CarryToSubcoreScanner to MJobs_PriorityHauling.
Change HaulToCarrier to MJobs_PriorityHauling.
Change HaulToSubcoreScanner to MJobs_PriorityHauling.

**Improved support for Vanilla Factions Expanded - Mechanoids** ([PR #105](https://github.com/CaptainArbitrary/ManyJobs/pull/105))

Refueling rocket silos is now a priority hauling job

### Bug Fixes

**Removed CarryToGeneExtractor from MJobs_Mechanitor** ([PR #75](https://github.com/CaptainArbitrary/ManyJobs/pull/75))

## Release 1.5.0

### New Features

**Added mod options menu to enable or disable mod-added work types** ([PR #67](https://github.com/CaptainArbitrary/ManyJobs/pull/67))

Mod options are accessible in the usual way, from the main menu's "Options" button, then the "Mod Options" button on the bottom left side of the window.

Note that any changes to the mod's options require a restart to be applied. You'll be prompted to restart your game if you make any changes to the mod options.

**Added a compatibility patch for WVC - Xenotypes and Genes** ([PR #69](https://github.com/CaptainArbitrary/ManyJobs/pull/69))

Golems are now empowered to do Many Jobs work types.

### Enhancements

**Lifter mechanoids (Biotech) and Hauler robots (Misc. Robots/Misc. Robots++) can now do undertaking jobs** ([PR #71](https://github.com/CaptainArbitrary/ManyJobs/pull/71))

That's right, you can now delegate the somber and serious task of handling the deceased to mindless automata, you monsters.

## Release 1.4.0

### New Features

**Added support for Vanilla Factions Expanded - Mechanoids** ([PR #60](https://github.com/CaptainArbitrary/ManyJobs/pull/60))

The following mechanoids have had their work types adjusted:
- VFE_Mechanoids_Autocleaner
  - added MJobs_PriorityCleaning
- VFE_Mechanoids_Autohauler
  - added MJobs_PriorityHauling, MJobs_Delivering, MJobs_Loading
- VFE_Mechanoids_Autominer
  - added MJobs_Drilling
- VFE_Mechanoids_AutoMedic
  - added MJobs_Rescuing, MJobs_Operating, MJobs_Caring

