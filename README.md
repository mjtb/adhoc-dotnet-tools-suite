# Michael Trenholm-Boyle’s Ad Hoc .NET Tools Suite

This is my personal collection of tools I wrote to support various aspects of Windows software
development. The tools are largely written in C# using MSBuild as a build engine.

The tools include:

*	`calendar` — displays a monthy calendar
*	`colour` — perform arithmetic on RGB and HSL colours
*	`colorpicker` & `colormatcher` — lookup colours
*	`enc` — various text encoding utilities
*	`ico2png` — extract .png files from a .ico file
*	`jtime` — print date/time stamps in various Internet formats
*	`mkicon` — create a .ico file from one or more .png files
*	`paste` — copy text from the clipboard to a file
*	`pbkdf2` — hash a password using the PBKDF2 algorithm
*	`pwgen` — generate a random password using various encoding strategies
*	`respeel` — extract .ico and other resources from .dll and .exe files
*	`sudo` — runs a program in a UAC-elevated command prompt window
*	`wrap` — hard-wrap a text file

**THESE TOOLS COME WITH NO WARRANTY, EXPRESSED OR IMPLIED. USE AT YOUR OWN RISK.**

The source code in this repository is licensed under a permissive MIT License. Please see the
LICENSE file for details.


# Prerequisites

Microsoft .NET Framework 4 SDK


# Building

Use MSBuild. The project file is `build.msproj`. The default target is `Build`, which will build all
of the tools.

There are two MSBuild properties that you can override:

*	`/p:OutDir` — where the built .exe files are placed \[default: .\\bin\]
*	`/p:BuildDir` — where temporary build files are placed \[deafult: .\\obj\]


An additional target `/t:Clean` may be invoked to remove `$(OutDir)` and `$(BuildDir)`.


# Issues

If you like these tools and use them and find an issue you'd like me to consider fixing, please
create an issue using GitHub.


# Pull Requests

Pull requests for bug fixes and other improvements gratefully accepted.
