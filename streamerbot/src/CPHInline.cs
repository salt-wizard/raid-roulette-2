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
    public void Init(){
    }

	public bool Execute()
	{
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

    /************************************************************************
	 * HELPER FUNCTIONS BELOW
	 ************************************************************************/
    private void LogVerbose(string msg)
    {
        CPH.LogVerbose($"{ACTION_GROUP} :: {msg}");
    }

    private void LogDebug(string msg){
        CPH.LogDebug($"{ACTION_GROUP} :: {msg}");
    }

    private void LogInfo(string msg){
        CPH.LogInfo($"{ACTION_GROUP} :: {msg}");
    }

    private void LogWarn(string msg){
        CPH.LogWarn($"{ACTION_GROUP} :: {msg}");
    }

    private void LogError(string msg){
        CPH.LogError($"{ACTION_GROUP} :: {msg}");
    }

	public void PrintArgsVerbose()
    {
        LogVerbose($"Arguments being passed in...");
        foreach (var arg in args)
        {
            LogVerbose($"{arg.Key} :: {arg.Value}");
        }
    }

	public string ObjToString(object obj)
    {
        return JsonConvert.SerializeObject(obj, Formatting.Indented, [new StringEnumConverter()]);
    }


    /**
     *
     */
    public static string SafeGetString(SQLiteDataReader reader, int colIndex)
    {
    if(!reader.IsDBNull(colIndex))
        return reader.GetString(colIndex);
        return string.Empty;
    }
}