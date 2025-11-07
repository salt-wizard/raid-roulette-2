# raid-roulette
Provides functionality for the chat to suggest other streamers to raid, and have the application chose on random.

Streamer.Bot is required to handle the logic between the custom UI and Twitch. It can be downloaded from [here](https://streamer.bot/downloads).

## Streamer.Bot
### Compiling Code
All contents in the file CPHInline.cs needs to be copied into the Streamer.Bot application and compiled from within there.

The following files need to be added as references for Streamer.Bot to compile successfully:
- System.Data.SQLite.dll

### Importing Project into Streamer.Bot
TBD

## OBS
The UI can be imported as a custom dock into OBS. It only requires the dist files.

In OBS, go to Docks -> Custom Browser Docks, and add the following:

    Dock Name: Raid Roulette
    URL: http://absolute/{PATH TO THE INDEX.HTML FILE}

You may need to refresh the dock from time to time to address possible issues.