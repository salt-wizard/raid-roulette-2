using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Numerics;
using System.Net;
using log4net.Config;
using log4net;
using System.Configuration;
using System.Data.SqlClient;

#if EXTERNAL_EDITOR
public class CPHInline : CPHInlineBase
#else
public class CPHInline
#endif
{
    private readonly string raidVersion = "2.0.1";
    private readonly string dbFile = @"RaidRoulette/raidroulette.db";
    private readonly string log4netCfg = @"RaidRoulette/log4net.xml";
    private readonly static ILog _logger = LogManager.GetLogger("RaidRoulette");
    private static readonly HttpClient _httpClient = new HttpClient();


    // Conditions for submitting suggestions; configurable
    private bool followerOnly;
    private int followerOnlyAge;
    private bool subscriberOnly;


    public void Init()
    {
        CPH.LogInfo($"Initializing Raid Roulette v{raidVersion}...");
        
        // Initalize the logger
        XmlConfigurator.ConfigureAndWatch(new FileInfo(log4netCfg));
        _logger.Info($"***********************************************************************************************");
        _logger.Info($"* Raid Roulette v{raidVersion}");
        _logger.Info($"* Start Time: {DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt")}");
        _logger.Info($"* Author: salt_wizard");
        _logger.Info($"***********************************************************************************************");
        
        // Initialize the DB
        InitRaidDB();

        CPH.LogInfo("Raid Roulette Initialized.");
    }

	public bool Execute()
    {
        PrintArgsVerbose();
        _logger.Info("testing");
        _logger.Trace("TRACE");
        _logger.Debug("DEBUG");
        _logger.Info("INFO");
        _logger.Warn("WARN");
        _logger.Error("ERROR");
        // your main code goes here
        return true;
	}

    public void Dispose(){
    }

    public void BuildConnection()
    {
        string path = "";
        using (SQLiteConnection connection = new SQLiteConnection(path))
        {
            
        }
    }





    /******************************************************************************************************************
     * DATABASE FUNCTIONS
     ******************************************************************************************************************/
    /// <summary>
    /// Initialize the Raid Roulette SQLite DB. If the database does not exist, it will be created. Likewise, tables
    /// and indexes will be created if they don't exist already.
    /// </summary>
    public void InitRaidDB()
    {
        _logger.Trace($"ENTER InitRaidDB");

        _logger.Info("Verifying if database exists...");
        if (!File.Exists(dbFile))
        {
            _logger.Info("Creating raid roulette database...");
            SQLiteConnection.CreateFile(dbFile);
            _logger.Info("Raid roulette database created.");
        } else
        {
            _logger.Info("Raid roulette database found.");
        }

        string connStr = $"Data Source={dbFile};";
        using (var conn = new SQLiteConnection(connStr))
        {
            conn.Open();
            _logger.Info("Creating tables if they don't exist...");

            // Create raid_targets table
            string sql = "";
            sql = $@"CREATE TABLE IF NOT EXISTS raid_targets (
                    userId INT NOT NULL, 
                    userLogin VARCHAR(30) NOT NULL,
                    userName VARCHAR(30) NOT NULL,
                    userPfp VARCHAR(255) NOT NULL,
                    broadcasterType VARCHAR(255) NOT NULL,
                    isBlacklisted VARCHAR(5) NOT NULL,
                    raidCount INT NOT NULL, 
                    lastRaidDate DATE, 
                    raidedByCount INT NOT NULL,
                    lastRaidedByDate DATE
                );";
            using (var command = new SQLiteCommand(sql, conn))
            {
                _logger.Debug($"Executing the following statement :: {sql}");
                command.ExecuteNonQuery();
            }

            // Create twitch_users table
            sql = $@"CREATE TABLE IF NOT EXISTS twitch_users (
                    userId INT NOT NULL, 
                    userLogin VARCHAR(30) NOT NULL,
                    userName VARCHAR(30) NOT NULL,
                    userPfp VARCHAR(255) NOT NULL,
                    isBlacklisted VARCHAR(5) NOT NULL
                );";
            using (var command = new SQLiteCommand(sql, conn))
            {
                _logger.Debug($"Executing the following statement :: {sql}");
                command.ExecuteNonQuery();
            }

            // Create twitch_users table
            sql = $@"CREATE TABLE IF NOT EXISTS user_auditing (
                    suggesterId INT NOT NULL, 
                    targetId INT NOT NULL,
                    dateSuggested DATE NOT NULL
                );";
            using (var command = new SQLiteCommand(sql, conn))
            {
                _logger.Debug($"Executing the following statement :: {sql}");
                command.ExecuteNonQuery();
            }

            _logger.Info("Creating indexes if they don't exist...");

            // Create indexes
            // raid_targets
            sql = $@"CREATE UNIQUE INDEX IF NOT EXISTS 
                    userIdInd ON raid_targets (userId);";
            using (var command = new SQLiteCommand(sql, conn))
            {
                _logger.Debug($"Executing the following statement :: {sql}");
                command.ExecuteNonQuery();
            }

            // twitch_users
            sql = $@"CREATE UNIQUE INDEX IF NOT EXISTS 
                    userIdInd ON twitch_users (userId);";
            using (var command = new SQLiteCommand(sql, conn))
            {
                _logger.Debug($"Executing the following statement :: {sql}");
                command.ExecuteNonQuery();
            }

            // user_auditing
            sql = $@"CREATE UNIQUE INDEX IF NOT EXISTS 
                    suggesterIdInd ON user_auditing (suggesterId);";
            using (var command = new SQLiteCommand(sql, conn))
            {
                _logger.Debug($"Executing the following statement :: {sql}");
                command.ExecuteNonQuery();
            }
            sql = $@"CREATE UNIQUE INDEX IF NOT EXISTS 
                    targetIdInd ON user_auditing (targetId);";
            using (var command = new SQLiteCommand(sql, conn))
            {
                _logger.Debug($"Executing the following statement :: {sql}");
                command.ExecuteNonQuery();
            }

            conn.Close();
        }

        _logger.Info("Database initialization complete.");

        _logger.Trace($"EXIT InitRaidDB");
    }
    
    public bool LoadRaidInit()
    {
        string connStr = $"Data Source={dbFile};";
        using (var conn = new SQLiteConnection(connStr))
        {
            string sql = "";
            conn.Open();

            sql = $@"SELECT * FROM raid_targets";
            using (var command = new SQLiteCommand(sql, conn))
            {
                SQLiteDataReader reader = command.ExecuteReader();

                JArray jarray = new JArray();
                if (reader.HasRows)
                {
                    _logger.Trace("Rows were returned.");
                    while (reader.Read())
                    {
                        int ind = 0;
                        int userId = reader.GetInt32(ind++);
                        string userLogin = reader.GetString(ind++);
                        string userName = reader.GetString(ind++);
                        string userPfp = reader.GetString(ind++);
                        string broadcasterType = reader.GetString(ind++);
                        bool isBlacklisted = bool.Parse(reader.GetString(ind++));
                        int raidCount = reader.GetInt32(ind++);
                        string lastRaidDate = SafeGetString(reader, ind++);
                        int raidedByCount = reader.GetInt32(ind++);
                        string lastRaidedByDate = SafeGetString(reader, ind++);

                        // Additional details about the raid target need to be gathered from Twitch
                        RaidRouletteUserDetails user = PopulateRaidTargetDetails(userId, userLogin, userName, userPfp,
                                                                                    broadcasterType, isBlacklisted, 
                                                                                    raidCount, lastRaidDate, 
                                                                                    raidedByCount, lastRaidedByDate);

                        JObject jobj = new JObject(
                            new JProperty("userId", user.userId),
                            new JProperty("userLogin", userLogin),
                            new JProperty("userName", userName),
                            new JProperty("userPfp", userPfp),
                            new JProperty("broadcasterType", broadcasterType),
                            new JProperty("isBlacklisted", isBlacklisted),
                            new JProperty("raidCount", raidCount),
                            new JProperty("lastRaidDate", lastRaidDate),
                            new JProperty("raidedByCount", raidedByCount),
                            new JProperty("lastRaidedByDate", lastRaidedByDate),
                            new JProperty("isOnline", user.isOnline),
                            new JProperty("viewCount", user.viewCount),
                            new JProperty("gameId", user.gameId),
                            new JProperty("gameName", user.gameName),
                            new JProperty("streamTitle", user.streamTitle),
                            new JProperty("streamPreview", user.streamPreview),
                            new JProperty("tags", user.tags),
                            new JProperty("isMature", user.isMature),
                            new JProperty("streamStart", user.streamStart),
                            new JProperty("slowMode", user.slowMode),
                            new JProperty("followerMode", user.followerMode),
                            new JProperty("subscriberMode", user.subscriberMode),
                            new JProperty("emoteMode", user.emoteMode),
                            new JProperty("uniqueChatMode", user.uniqueChatMode)
                        );
                        jarray.Add(jobj);
                    }
                }

                _logger.Trace($"Final result from DB :: {JsonConvert.SerializeObject(jarray)}");
                CPH.SetGlobalVar("raidTargets", JsonConvert.SerializeObject(jarray), true);
            }
            conn.Close();
        }

        return true;
    }

    // Populates the full details of a raid target
    private RaidRouletteUserDetails PopulateRaidTargetDetails(int userId, string userLogin, string userName, 
        string userPfp, string broadcasterType, bool isBlacklisted, int raidCount, string lastRaidDate, 
        int raidedByCount, string lastRaidedByDate)
    {
        RaidRouletteUserDetails user = new RaidRouletteUserDetails(userId, userLogin, userName, userPfp, 
                                                                    broadcasterType, isBlacklisted, raidCount, 
                                                                    lastRaidDate, raidedByCount, lastRaidedByDate);

        // Pull stream details for the user
        JObject? streamDetails = GetTwitchUserStreamDetails(user.userName);
        _logger.Trace($"Stream details for user {userLogin} :: {streamDetails}");
        
        if(!streamDetails["data"].HasValues)
        {
            _logger.Trace("This user is not online; skipping the rest of the values");
            user.isOnline = false;

            user.gameId = 0;
            user.gameName = "";
            user.streamTitle = "";
            user.streamPreview = "";
            user.tags = Array.Empty<string>();
            user.isMature = false;
            user.streamStart = "";
            user.slowMode = false;
            user.followerMode = false;
            user.subscriberMode = false;
            user.emoteMode = false;
            user.uniqueChatMode = false;
        } else {
            user.isOnline = true;
            
            JToken streamToken = streamDetails["data"]![0];
            user.gameId = (int)streamToken["game_id"];
            user.gameName = (string)streamToken["game_name"];
            user.streamTitle = (string)streamToken["title"];
            user.viewCount = (int)streamToken["viewer_count"];
            user.streamStart = (string)streamToken["started_at"];
            user.streamPreview = ((string)streamToken["thumbnail_url"]).Replace(
                "{width}x{height}",
                "440x248"
            );
            user.tags = streamToken["tags"]?.ToObject<string[]>() ?? Array.Empty<string>();
            user.isMature = (bool)streamToken["is_mature"];

            // Pull chat permissions for the user
            JObject? chatPerms = CheckChatPerms(userId);
            _logger.Trace($"Chat perms for user {userLogin} :: {chatPerms}");
            JToken chatToken = chatPerms["data"]![0];
            user.slowMode = (bool)chatToken["slow_mode"];
            user.followerMode = (bool)chatToken["follower_mode"];
            user.subscriberMode = (bool)chatToken["subscriber_mode"];
            user.emoteMode = (bool)chatToken["emote_mode"];;
            user.uniqueChatMode = (bool)chatToken["unique_chat_mode"];
        }
        
        return user;
    }

    public bool AuditUserSuggestion()
    {
        _logger.Trace($"ENTER AuditUserSuggestion");
        if(args["userId"] != null)
        {
            string userId = (string)args["userId"];
            TwitchUserInfo userInfo = CPH.TwitchGetExtendedUserInfoById(userId);

            _logger.Debug($"User details retrieved :: {ObjToString(userInfo)}");
        } else
        {
            _logger.Error("userId was missing from arguments! Exiting audit!");
        }

        _logger.Trace($"EXIT AuditUserSuggestion");
        return true;
    }

    /// <summary>
    /// Adds a raid target to the list of suggestions if they haven't been suggested already. Users who can add
    /// suggestions are also based on criteria provided by the streamer.
    /// </summary>
    /// <returns></returns>
    public bool SuggestRaidTarget()
    {
        _logger.Trace("ENTER SuggestRaidTarget");
        PrintArgsVerbose();

        if(args["input0"] == null){
            _logger.Trace("No input was detected, exiting.");
            _logger.Trace("EXIT SuggestRaidTarget");
            return false;
        }
        string suggestion = (string)args["input0"];

        // TODO - VERIFY USER HERE (THE ONE WHO MADE THE SUGGESTION)

        // Validate that the user does not exist in the local cache
        _logger.Debug($"Checking if suggestion {suggestion} is in the local cache...");
        string raidTargetsStr = CPH.GetGlobalVar<string>("raidTargets", true);
        _logger.Trace($"Returned JSON string :: {raidTargetsStr}");
        _logger.Trace($"Parsing string into JArray object...");
        JArray raidTargets = JArray.Parse(raidTargetsStr); 
        bool suggestionFound = false;
        foreach(JObject item in raidTargets)
        {
            string value = item.GetValue("userName").ToString();
            
            if(String.Equals(suggestion, value, StringComparison.OrdinalIgnoreCase))
            {
                suggestionFound = true;
            }
        }
        if (suggestionFound)
        {
            _logger.Debug($"Suggestion {suggestion} already exists in the local cache.");
            _logger.Trace("EXIT SuggestRaidTarget");
            return false;
        }

        // TODO - CHECK THE DATABASE TO MAKE SURE THE USER HAS NOT BEEN SUGGESTED BEFORE EITHER (CORNER CASE)

        // Assuming the target does not exist, we need the streamer details
        JObject? userDetails = GetTwitchUserDetails(suggestion);
        if(userDetails == null){
            _logger.Trace("Details returned back from Twitch are null; exiting early");
            _logger.Trace("EXIT SuggestRaidTarget");
            return false;
        }

        // Only the first data point matters since we're only calling the user one at a time
        JToken userToken = userDetails["data"]![0];
        int userId = (int)userToken["id"];
        string userLogin = (string)userToken["login"];
        string userName = (string)userToken["display_name"];
        string userPfp = (string)userToken["profile_image_url"];
        string broadcasterType = (string)userToken["broadcaster_type"];

        RaidRouletteUserDetails user = PopulateRaidTargetDetails(userId, userLogin, userName, userPfp, 
                                                                    broadcasterType, false, 0, "", 0, "");

        JObject jobj = JObject.Parse(JsonConvert.SerializeObject(user));
        _logger.Trace($"User Object Created :: {jobj}");

        _logger.Info($"Adding suggestion {suggestion} to the list!");
        raidTargets.Add(jobj);
        CPH.SetGlobalVar("raidTargets", JsonConvert.SerializeObject(raidTargets), true);

        // Add user to the database if they have not been added already
        InsertRaidTarget(user);

        _logger.Trace("EXIT SuggestRaidTarget");
        return true;
    }
    
    /******************************************************************************************************************
     * DATABASE FUNCTIONS
     ******************************************************************************************************************/
    private void InsertRaidTarget(RaidRouletteUserDetails user)
    {
        string connStr = $"Data Source={dbFile};";
        using (var conn = new SQLiteConnection(connStr))
        {
            conn.Open();
            _logger.Info($"Inserting user {user.userName} into database...");

            // Create raid_targets table
            string sql = "";
            sql = $@"INSERT INTO raid_targets VALUES (
                    @userId,
                    @userLogin,
                    @userName,
                    @userPfp,
                    @broadcasterType,
                    @isBlacklisted,
                    @raidCount,
                    @lastRaidDate,
                    @raidedByCount,
                    @lastRaidedByDate
                );";
            using (var command = new SQLiteCommand(sql, conn))
            {
                command.Parameters.Add(new SQLiteParameter("@userId", user.userId));
                command.Parameters.Add(new SQLiteParameter("@userLogin", user.userLogin));
                command.Parameters.Add(new SQLiteParameter("@userName", user.userName));
                command.Parameters.Add(new SQLiteParameter("@userPfp", user.userPfp));
                command.Parameters.Add(new SQLiteParameter("@broadcasterType", user.broadcasterType));

                // TODO - IS BLACKLISTED IS AN INTEGER INSTEAD OF JUST A BOOLEAN
                command.Parameters.Add(new SQLiteParameter("@isBlacklisted", user.isBlacklisted.ToString()));
                command.Parameters.Add(new SQLiteParameter("@raidCount", user.raidCount));
                command.Parameters.Add(new SQLiteParameter("@lastRaidDate", user.lastRaidDate));
                command.Parameters.Add(new SQLiteParameter("@raidedByCount", user.raidedByCount));
                command.Parameters.Add(new SQLiteParameter("@lastRaidedByDate", user.lastRaidedByDate));

                _logger.Debug($"Executing the following statement :: {sql}");
                command.ExecuteNonQuery();
            }
        }
    }

    // TODO - Implement functionality to return back existing raid targets
    private void ReturnRaidTarget()
    {
        
    }

    /******************************************************************************************************************
	 * TWITCH FUNCTIONS
	 ******************************************************************************************************************/

    
    /// <summary>
    /// Return back the details for a single Twitch user
    /// </summary>
    private JObject? GetTwitchUserDetails(string username)
    {
        string apiUrl = $"https://api.twitch.tv/helix/users?login={username}";
        JObject? userDetails = ExecuteHelixGetCall(apiUrl);
        return userDetails;
    }

    /// <summary>
    /// Return back the details for multiple Twitch users
    /// </summary>
    private JObject? GetTwitchUsersDetails(string[] usernames)
    {
        string apiUrl = $"https://api.twitch.tv/helix/users?login={string.Join(",", usernames)}";
        JObject? userDetails = ExecuteHelixGetCall(apiUrl);
        return userDetails;
    }
    
    /// <summary>
    /// Return back the stream details of a single Twitch user. If there is no live stream the JObject should be empty.
    /// </summary>
    private JObject? GetTwitchUserStreamDetails(string username)
    {
        string apiUrl = $"https://api.twitch.tv/helix/streams?user_login={username}";
        JObject? streamDetails = ExecuteHelixGetCall(apiUrl);
        return streamDetails;
    }

    /// <summary>
    /// Return back the stream details for multiple Twitch users. If there are no live stream the JObject should be 
    /// empty.
    /// </summary>
    private JObject? GetTwitchUsersStreamDetails(string usernames)
    {
        string apiUrl = $"https://api.twitch.tv/helix/streams?user_login={string.Join(",", usernames)}";
        JObject? streamDetails = ExecuteHelixGetCall(apiUrl);
        return streamDetails;
    }

    /// <summary>
    /// Return back the chat permissions for a single Twitch streamer. This API endpoint cannot supply multiple 
    /// usernames.
    /// </summary>
    private JObject? CheckChatPerms(int broadcaster_id)
    {
        string apiUrl = $"https://api.twitch.tv/helix/chat/settings?broadcaster_id={broadcaster_id}";
        JObject? chatPerms = ExecuteHelixGetCall(apiUrl);
        return chatPerms;
    }

    /// <summary>
    /// Make a GET API call to Twitch to receive streamer / user information
    /// </summary>
    /// <param name="apiUrl"></param>
    /// <returns></returns>
    private JObject? ExecuteHelixGetCall(string apiUrl)
    {
        try
        {
            string clientId = CPH.TwitchClientId;
            string accessToken = CPH.TwitchOAuthToken;

            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(accessToken))
            {
                _logger.Error("No crendentials provided, unable to execute API call.");
                return null;
            }
            using (var request = new HttpRequestMessage(HttpMethod.Get, apiUrl))
            {
                request.Headers.Add("Client-ID", clientId);
                request.Headers.Add("Authorization", $"Bearer {accessToken}");

                var response = _httpClient.SendAsync(request).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    
                    _logger.Trace($"JSON Response :: {jsonResponse}");
                    return JObject.Parse(jsonResponse);
                } else
                {
                    _logger.Error($"Twitch API error for call '{apiUrl}': {response.StatusCode}");
                    return null;
                }
            }
        } catch (Exception ex)
        {
            _logger.Error($"Unable to execute call '{apiUrl}':{ex.Message}");
            return null;
        }
    }

    /******************************************************************************************************************
	 * HELPER FUNCTIONS
	 ******************************************************************************************************************/
    /// <summary>
    /// Prints Streamer.bot arguments to a log file.
    /// </summary>
    public void PrintArgsVerbose()
    {
        _logger.Verbose($"Arguments being passed in...");
        foreach (var arg in args)
        {
            _logger.Verbose($"{arg.Key} :: {arg.Value}");
        }
    }


    private bool VerifyFollowConditions(Dictionary<string, object> args)
    {
        _logger.Trace("ENTER VerifyFollowConditions");
        bool verifiedFollower = false;
        // Must confirm that the arguments exist, otherwise there will be issues
        if(args["isFollowing"] != null && args["followAgeSeconds"] != null)
        {
            
        } else
        {
            _logger.Error("Unable to find proper arguments, exiting early.");
        }
        _logger.Trace("EXIT VerifyFollowConditions");
        return verifiedFollower;
    }

    /// <summary>
    /// Convert any object type into a string value.
    /// </summary>
    public string ObjToString(object obj)
    {
        return JsonConvert.SerializeObject(obj, Formatting.Indented,
        [new StringEnumConverter()]);
    }


    /// <summary>
    /// Return back either a string or an empty string when reading from a table.
    /// </summary>
    public static string SafeGetString(SQLiteDataReader reader, int colIndex)
    {
        if (!reader.IsDBNull(colIndex))
            return reader.GetString(colIndex);
        return string.Empty;
    }
}

/// <summary>
/// Enable verbose/trace logging in log4net using extension methods. Taken from: https://stackoverflow.com/a/3461437
/// </summary>
public static class ILogExtentions
{
    public static void Trace(this ILog logger, string message, 
                                Exception? exception)
    {
        logger.Logger.Log(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, 
            log4net.Core.Level.Trace, message, exception);
    }

    public static void Trace(this ILog logger, string message)
    {
        logger.Trace(message, null);
    }

    public static void Verbose(this ILog logger, string message, 
                                Exception? exception)
    {
        logger.Logger.Log(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, 
            log4net.Core.Level.Verbose, message, exception);
    }

    public static void Verbose(this ILog logger, string message)
    {
        logger.Verbose(message, null);
    }
}

/// <summary>
/// Format of users pulled from Twitch
/// </summary>
public class RaidRouletteUserDetails
{
    public int userId { get; set; }
    public string? userLogin { get; set; }
    public string? userName { get; set; }
    public string? userPfp { get; set; }
    public string? broadcasterType { get; set; }
    public bool isBlacklisted { get; set; }
    public int raidCount { get; set; }
    public string? lastRaidDate { get; set; }
    public int raidedByCount { get; set; }
    public string? lastRaidedByDate { get; set; }
    public bool isOnline { get; set; }
    public int viewCount { get; set; }
    public int gameId { get; set; }
    public string? gameName { get; set; }
    public string? streamTitle { get; set; }
    public string? streamPreview { get; set; }
    public string[]? tags { get; set; }
    public bool isMature { get; set; }
    public string? streamStart { get; set; }
    public bool slowMode { get; set; }
    public bool followerMode { get; set; }
    public bool subscriberMode { get; set; }
    public bool emoteMode { get; set; }
    public bool uniqueChatMode { get; set; }
    public RaidRouletteUserDetails(int userId, string userLogin, string userName, string userPfp, 
                                    string broadcasterType, bool isBlacklisted, int raidCount, string lastRaidDate, 
                                    int raidedByCount, string lastRaidedByDate)
    {
        this.userId = userId;
        this.userLogin = userLogin;
        this.userName = userName;
        this.userPfp = userPfp;
        this.broadcasterType = broadcasterType;
        this.isBlacklisted = isBlacklisted;
        this.raidCount = raidCount;
        this.lastRaidDate = lastRaidDate;
        this.raidedByCount = raidedByCount;
        this.lastRaidedByDate = lastRaidedByDate;
    }
}