# MugenAITool

## Description:

A tool for mugen AIs. It is produced not only for AI authors but also the players who don't know about character AI.  
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
- Mugen Template: Created new 2 templates.
