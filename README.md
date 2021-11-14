# Kaboom - tiling window manager for windows
You might ask yourself why 'Kaboom', that doesn't seem to have anything to do with tiling window managers?
Well I am not very creative with names, so...

Anyhow, Kaboom is a tiling window manager for windows.
It tiles windows. It tiles horizontally by default, a vertical arrangement is available too.

The shortcuts are no longer hardcoded. Wow amazing...
You should find the config file at %appdata%/Kaboom/settings.conf after running it once.
I used this config file parser: https://github.com/salaros/config-parser

The default shortcuts are as follows:<br>

Move Window: Alt + Arrow Key<br>
Move Selection: Ctrl + Arrow Key<br>

Wrap selected Window with horizontal Arrangement: Alt + H<br>
Wrap selected Window with vertical Arrangement: Alt + V<br>
Unwrap parent of selected Window: Alt + U<br>

Exit Kaboom: Alt + Q<br>
<br>

Note that it is rather hard to decide which window to catch and which not.
Some Windows might not be catched while other invisible windows such as overlays might.
Also some Applications have little helper windows (Paint.net for example) that might get tiled and are then unusable.

If you have any suggestions feel free to open an issue.
