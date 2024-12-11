# TARP | Discord Rich Presence for [Totally Accurate Battlegrounds](https://store.steampowered.com/app/823130/Totally_Accurate_Battlegrounds/)
![presence1](https://github.com/PikachuPenial/TotallyAccurateRichPresence/assets/62630906/f505964f-e47d-41e5-b13d-c649e2016808)
![presence2](https://github.com/PikachuPenial/TotallyAccurateRichPresence/assets/62630906/94b321f5-84ad-4248-9299-69438901105a)
![presence3](https://github.com/PikachuPenial/TotallyAccurateRichPresence/assets/62630906/72674043-d1e7-43bb-b182-9eb2caf64292)

## DISCLAIMER
- Use this mod at your own risk, bugs and/or crashes can occur.

## Installation
*For this installation, I will be rerferring to my game directory as `TotallyAccurateBattlegroundsModded`*.
1. If you do not already have BepInEx installed, create a backup of your vanilla game files, follow **[these instructions](https://docs.bepinex.dev/articles/user_guide/installation/index.html)** to install BepInEx, and launch the game once.
2. Download the latest `TARP.dll` file from the [TARP Releases page](https://github.com/PikachuPenial/TotallyAccurateRichPresence/releases).
3. Put the `TARP.dll` file into the `TotallyAccurateBattlegroundsModded\BepInEx\plugins` folder of your game directory.
4. Download the latest `discord-rpc-win.zip` file from the [discord-rps Releases page](https://github.com/discord/discord-rpc/releases).
5. Put the `discord-rpc\win64-dynamic\bin\discord-rpc.dll` file into the `TotallyAccurateBattlegroundsModded\TotallyAccurateBattlegrounds_Data\Plugins` folder of your game directory.

## Patch Notes

<details>
<summary>Version 1.1</summary>
<br>

Released on <i>9/17/2023</i>

Hovering over a presence image now shows the players LVL and their progress as a percentage to the next LVL

Refactored the process for presence updates

Fixed a case of presence desync when returning to the Main Menu

Removed custom logging system and switch logger to BepInEx (previously Unity)

---
</details>

<details>
<summary>Version 1.0</summary>
<br>

Released on <i>9/15/2023</i>

Initial release

---
</details>

## License
MIT License

Some of the Discord callbacks were taken from [eai04191/craftopia-rpc](https://github.com/eai04191/craftopia-rpc). Be sure to check out their projects!

*Be sure to report any issues that you may come across, either through the repositories [Issues page](https://github.com/PikachuPenial/TotallyAccurateRichPresence/issues), or by reaching out to me on Discord (penial).*

*I am not responsible for any damage that this mod may cause to your game account and/or game files. If you install this mod properly, neither of these things should be affected.*
