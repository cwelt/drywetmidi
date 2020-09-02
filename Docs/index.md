![DryWetMIDI Logo](https://github.com/melanchall/drywetmidi/blob/develop/Resources/Images/dwm-logo.png?raw=true)

# Overview

DryWetMIDI is the .NET library to work with MIDI files and MIDI devices. With the DryWetMIDI you can:

* Read, write and create [Standard MIDI Files (SMF)](https://www.midi.org/specifications/category/smf-specifications). It is also possible to read [RMID](https://www.loc.gov/preservation/digital/formats/fdd/fdd000120.shtml) files where SMF wrapped to RIFF chunk.
* [Send](https://melanchall.github.io/drywetmidi/articles/devices/Output-device.html) MIDI events to/[receive](https://melanchall.github.io/drywetmidi/articles/devices/Input-device.html) them from MIDI devices, [play](https://melanchall.github.io/drywetmidi/articles/playback/Overview.html) MIDI data and [record](https://melanchall.github.io/drywetmidi/articles/recording/Overview.html) it.
* Finely adjust process of reading and writing. It allows, for example, to read corrupted files and repair them, or build MIDI file validators.
* Implement [custom meta events](https://melanchall.github.io/drywetmidi/articles/custom-data-structures/Custom-meta-events.html) and [custom chunks](https://melanchall.github.io/drywetmidi/articles/custom-data-structures/Custom-chunks.html) that can be written to and read from MIDI files.
* Easily catch specific error when reading or writing MIDI file since all possible errors in a MIDI file are presented as separate exception classes.
* Manage content of a MIDI file either with low-level objects, like event, or high-level ones, like note (read the **High-level data managing** section of the library docs).
* Build musical compositions (see [Pattern](https://melanchall.github.io/drywetmidi/articles/composing/Pattern.html) page of the library docs).
* Perform complex tasks like quantizing, notes splitting or converting MIDI file to CSV representation (see [Tools](https://melanchall.github.io/drywetmidi/articles/tools/Overview.html) page of the library docs).

Useful links:

* [Documentation](https://melanchall.github.io/drywetmidi)
* [NuGet package page](https://www.nuget.org/packages/Melanchall.DryWetMidi)

# Status

## Version

Current version is **5.1.1**.

## CI

|   |Windows (.NET Framework)|Windows (.NET Core)|macOS (.NET Core)|Linux (.NET Core)|
|---|---|---|---|---|
|**Core**|[![Build Status](https://dev.azure.com/Melanchall/DryWetMIDI/_apis/build/status/Windows/%5BWindows%5D%20Test%20core%20part%20on%20.NET%20Framework?branchName=develop)](https://dev.azure.com/Melanchall/DryWetMIDI/_build/latest?definitionId=1&branchName=develop)|[![Build Status](https://dev.azure.com/Melanchall/DryWetMIDI/_apis/build/status/Windows/%5BWindows%5D%20Test%20core%20part%20on%20.NET%20Core?branchName=develop)](https://dev.azure.com/Melanchall/DryWetMIDI/_build/latest?definitionId=13&branchName=develop)|[![Build Status](https://dev.azure.com/Melanchall/DryWetMIDI/_apis/build/status/macOS/%5BmacOS%5D%20Test%20core%20part%20on%20.NET%20Core?branchName=develop)](https://dev.azure.com/Melanchall/DryWetMIDI/_build/latest?definitionId=11&branchName=develop)|[![Build Status](https://dev.azure.com/Melanchall/DryWetMIDI/_apis/build/status/Linux/%5BLinux%5D%20Test%20core%20part%20on%20.NET%20Core?branchName=develop)](https://dev.azure.com/Melanchall/DryWetMIDI/_build/latest?definitionId=12&branchName=develop)|
|**Devices**|[![Build Status](https://dev.azure.com/Melanchall/DryWetMIDI/_apis/build/status/Windows/%5BWindows%5D%20Test%20devices%20part%20on%20.NET%20Framework?branchName=develop)](https://dev.azure.com/Melanchall/DryWetMIDI/_build/latest?definitionId=10&branchName=develop)|[![Build Status](https://dev.azure.com/Melanchall/DryWetMIDI/_apis/build/status/Windows/%5BWindows%5D%20Test%20devices%20part%20on%20.NET%20Core?branchName=develop)](https://dev.azure.com/Melanchall/DryWetMIDI/_build/latest?definitionId=14&branchName=develop)|Not supported|Not supported|

## Obsolete API

Here the table of current API that is obsolete and thus will be removed from the library by a next release.

$OBSOLETE_API$
