# MugenAITool

## Description:

A tool for mugen AIs. It is produced not only for AI authors but also for the players who don't know about character AI.  
There are 3 modules in it, Replace Helper, Atk Storage, and Automatic AI Maker. Automatic AI Maker is not ready yet.

## Components

### Replace Helper
A tool for Mugen AI authors to program AI. Give a template of Mugen code and replace the specific strings into the strings you want.

### Atk Storage
A tool for Mugen AI authors to find out the range of all atk skills. It reads an DEF files of a character as input, and records many details about different attack states into CSV file.

### Automatic AI Maker
A tool for all Mugen players to make an AI for specific character automatically. Not finished yet.

## Release notes

### 2017.07.30:
- Atk Storage: Added comments. Changed fields and helper functions from public to private. Created AtkStorageMake for AS_B1_Click event, renamed RH_Replace.  

### 2017.08.02:
- Atk Storage: ReadSt() finished.

### 2017.11.12:
- Updated version number.
- Atk Storage: ReadCns(), ReadAir() and CreateCsvFile() finished, first draft of Atk Storage finished. Created StringHelpers.cs to handle strings, added RegularExpressions.cs but not used yet.
- Mugen Template: Created new 2 Neutral templates.

### 2017.12.10:
- Atk Storage: Added StateType into CSV file.
- Mugen Template: Updated Neutral templates.

### 2017.12.23:
- Atk Storage: Added stateControllerType to identify the state crontroller, added Guardflag, Hit / Guard Pausetime into CSV file.

### 2017.12.31:
- Main: Added menu bar (Language and Help submenu) and new tabs (Mainpage, AI Switch, InvincibleCounter, Punish, Stuffing, Pressuring, Combo, Neutral, Guard and Movement). Renamed components.
- AI Switch Manager: First draft.
- Guard Manager: First draft.
- Mugen Template: Created new AI Switch and 2 Guard templates.

### 2018.01.13:
- Main: Created main page and renamed variables.

### 2018.01.24:
- Main: Renamed components and created new class CharFilesInfo to record the file information.
- Atk Storage: Updated with CharFilesInfo class.

### 2018.02.11:
- Main: First step of "one button create AI". Created new files and folder for AI. Try to link Atk Storage, AI Switch Manager and Guard Manager together.
- CharFilesInfo: Bug fix, clear names of ST files to avoid when we use ReadDef function.
- Atk Storage: Record down all triggers except for those contain "command"s when we read CMD file.
- Guard Manager: Check "CanAirGuard".

### 2018.02.17:
- Main, Settings: First draft of multi-language, added Simplified Chinese.


