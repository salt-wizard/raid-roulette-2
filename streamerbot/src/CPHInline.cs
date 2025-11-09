using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Numerics;
using System.Net;

#if EXTERNAL_EDITOR
public class CPHInline : CPHInlineBase
#else
public class CPHInline
#endif
{
    readonly private string raidRouletteVersion = "2.0.0";

    public void Init(){
    }

	public bool Execute()
	{
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





    /**************************************************************************
     * DATABASE FUNCTIONS
     **************************************************************************/
    public bool InitRaidDB()
    {
        LogVerbose("=============================");
        LogVerbose($"ENTER InitRaidDB");
        LogVerbose("=============================");

        LogInfo("Initializing database...");

        string dbFile = @"./raidroulette/raidroulette.db";
        if (!File.Exists(dbFile))
        {
            SQLiteConnection.CreateFile(dbFile);
            LogInfo("Raid roulette database created...");
        }

        string connStr = $"Data Source={dbFile};Version=3;";
        using (var conn = new SQLiteConnection(connStr))
        {
            conn.Open();

            LogInfo("Creating tables if they don't exist...");

            // Create raid_targets table
            string sql = $@"CREATE TABLE IF NOT EXISTS raid_targets (
                            userId VARCHAR(20) NOT NULL, 
                            userLogin VARCHAR(30) NOT NULL,
                            userName VARCHAR(30) NOT NULL,
                            userPfp VARCHAR(255) NOT NULL,
                            isBlacklisted BOOLEAN NOT NULL, 
                            isOnline BOOLEAN NOT NULL, 
                            isVeto BOOLEAN NOT NULL,
                            raidCount INT NOT NULL, 
                            lastRaidDate DATE, 
                            raidedByCount INT NOT NULL, 
                            lastRaidedDate DATE
                        );";
            using (var command = new SQLiteCommand(sql, conn))
            {
                command.ExecuteNonQuery();
            }

            // Create indexes
            sql = $@"CREATE UNIQUE INDEX IF NOT EXISTS 
                    userIdInd ON raid_targets (userId);";
            using (var command = new SQLiteCommand(sql, conn))
            {
                command.ExecuteNonQuery();
            }

            // Set all isVeto to False
            sql = $"UPDATE raid_targets SET isVeto = false";
            using (var command = new SQLiteCommand(sql, conn))
            {
                command.ExecuteNonQuery();
            }

            conn.Close();
        }

        LogVerbose("=============================");
        LogVerbose($"EXIT InitRaidDB");
        LogVerbose("=============================");
        return true;
    }

    /***************************************************************************
	 * HELPER FUNCTIONS BELOW
	 **************************************************************************/
    /// <summary>
    /// Creates the directory for the logs, and creates a log file for the 
    /// current Streamer.bot session
    /// </summary>
    public bool CreateLogs()
    {
        string logDir = @"./raidroulette/logs";
        try
        {
            Directory.CreateDirectory(logDir);
        }
        catch (Exception e)
        {
            CPH.LogError($"The following error occurred :: {e.Message}");
        }

        // Create the log file and set the global variable
        DateTime now = DateTime.Now;
        string dateTime = now.ToString("yyyyMMdd-HHmmss");
        string raidLogFile = @$"{logDir}/raid-roulette-{dateTime}.txt";
        CPH.SetGlobalVar("raidLogFile", raidLogFile, true);

        if (!File.Exists(raidLogFile))
        {
            File.Create(raidLogFile).Close();
        }

        LogInfo($"Initializing Raid Roulette v{raidRouletteVersion}");

        return true;
    }
    
    private void LogVerbose(string msg)
    {
        string newline = Environment.NewLine;
        string raidLogFile = CPH.GetGlobalVar<String>("raidLogFile", true);
        DateTime now = DateTime.Now;
        string nowStr = now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        File.AppendAllText(raidLogFile, $"[{nowStr} VRB] {msg}" + newline);
    }

    private void LogDebug(string msg){
        string newline = Environment.NewLine;
        string raidLogFile = CPH.GetGlobalVar<String>("raidLogFile", true);
        DateTime now = DateTime.Now;
        string nowStr = now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        File.AppendAllText(raidLogFile, $"[{nowStr} DBG] {msg}" + newline);
    }

    private void LogInfo(string msg){
        string newline = Environment.NewLine;
        string raidLogFile = CPH.GetGlobalVar<String>("raidLogFile", true);
        DateTime now = DateTime.Now;
        string nowStr = now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        File.AppendAllText(raidLogFile, $"[{nowStr} INF] {msg}" + newline);
    }

    private void LogWarn(string msg){
        string newline = Environment.NewLine;
        string raidLogFile = CPH.GetGlobalVar<String>("raidLogFile", true);
        DateTime now = DateTime.Now;
        string nowStr = now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        File.AppendAllText(raidLogFile, $"[{nowStr} WRN] {msg}" + newline);
    }

    private void LogError(string msg){
        string newline = Environment.NewLine;
        string raidLogFile = CPH.GetGlobalVar<String>("raidLogFile", true);
        DateTime now = DateTime.Now;
        string nowStr = now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        File.AppendAllText(raidLogFile, $"[{nowStr} ERR] {msg}" + newline);
    }

    public void PrintArgsVerbose()
    {
        //LogVerbose($"Arguments being passed in...");
        foreach (var arg in args)
        {
            //LogVerbose($"{arg.Key} :: {arg.Value}");
        }
    }
    
    	public string ObjToString(object obj)
    {
        return JsonConvert.SerializeObject(obj, Formatting.Indented,
        [new StringEnumConverter()]);
    }


    /**
     *
     */
    public static string SafeGetString(SQLiteDataReader reader, int colIndex)
    {
        if (!reader.IsDBNull(colIndex))
            return reader.GetString(colIndex);
        return string.Empty;
    }


}