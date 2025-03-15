# Lumen.Modules.SteamPartner

Store followers, wishlists and package sales for games on Steam, in a database, later used for reporting/stats.

**Followers:** It query and stores followers amount for every game listed in the "SteamGames" table. This task runs once a day at 06:00 UTC.  
**Wishlists & Sales:** It removes the most recent 35 days of wishlists/sales stored in database then query everything missing from the database from the Steam partner backend. This happens only when calling the `/SteamData/queryDataFromSteam` route with the Steam partners session cookie. To prevent doing this manually, a tampermonkey script exists to call this for you.  

*Note for unreleased games:* If you don't specify the `packageId` column in the "SteamGames" table, it will only query followers and wishlists, not sales. It's how you are supposed to get data for unreleased games.
