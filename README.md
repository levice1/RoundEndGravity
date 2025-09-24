# RoundEndGravity
Plugin for CounterStrikeSharp framework for Counter-Strike 2. Change gravity on end round.

Made at the request of players on my server in AWP Only mode.
When someone wins (the round ends), the gravity changes from the standard 800 to 200, and it stays that way until the start of the next round. During warmup, gravity remains standard.

Copy the `RoundEndGravity` folder with its files into: `csgo/addons/counterstrikesharp/plugins`

The configuration file will be generated automatically at:
`csgo/addons/counterstrikesharp/configs/plugins/RoundEndGravity/RoundEndGravity.json`
In this file you can change the gravity value applied at the end of the round (default is 200).
If your server uses a custom gravity value for normal rounds, you can also adjust it here (default is 800).
