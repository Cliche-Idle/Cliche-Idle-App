# CHANGELOG

This is a list of changes between the JS and C# version, among other things.

### Stats
* Changes from 3 letter abbreviations to full words.
  * This is to avoid a naming collision between `INT(elligence)` and the Integer type.
  * Renamed `int` to `intelligence`.
  * Renamed `dex` to `dexterity`.
  * Renamed `vit` to `vitality`.
  * Renamed `str` to `strength`.

### Account
* Removed `hasCharacter` property.
  * This should be implicitly handled by checking for character name.
* Renamed `createdOn` to `registeredOn`.
* Renamed `lastWrite` to `lastSync`.
* Moved `userName` and the above two properties to a new subclass `IdentifiableAccountData`.

### General
* Renamed `hp` to `health`.
* Renamed `class` to `classSpecName`.
  * This is to avoid a naming collision with native type declaration `class`.

### Inventory
##### Weapons
* Renamed `atk` to `attack`.
* Removed `id` property.
  * `variantID` already contains this information.

##### Armour
* Structure copied from Weapons as there was no clear plan here.
* Renamed `def` to `defense`.
* Removed `id` property.
  * `variantID` already contains this information.

### Activities
##### Queue
* Changed timed activity queue item volatile property `end` type from string to double.
  * Why was this even a thing?? Potentially an older schema version but keep an eye out for this, in case it appears in other places too.