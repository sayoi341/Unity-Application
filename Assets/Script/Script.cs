using UnityEngine;
using System;
using System.Text;
using Discord;

public class Script : MonoBehaviour
{
    private string path = "C:/Users/sayoi/AppData/Roaming/approvers/youtube_status/status.ini";
        
    public string active;
    public string multi;
    public bool active_tab;
    public string title;
    public string url;

    public Discord.Discord discord;

    private Discord.Activity rpc;

    void Start() {
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        int cur_time = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
        
        discord = new Discord.Discord(820934372634132530, (System.UInt64) Discord.CreateFlags.Default);
        
        rpc = new Activity() {
            Timestamps = new ActivityTimestamps() {
                Start = cur_time,
            },
            Assets = new ActivityAssets() {
                LargeImage = "icon",
                LargeText = "YouTube Status"
            }
        };
        
        var activityManager = discord.GetActivityManager();
        activityManager.UpdateActivity(rpc, (res) => {
            if (res == Discord.Result.Ok)
            {
                Debug.LogError("Everything is fine");
            }
        });
    }

    void Update() {
        ReadINI();
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        int cur_time = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
        
        discord.RunCallbacks();
        
        if (active == "true")
        {
            rpc.Details = "Youtubeを視聴中";
            rpc.State = title;
            UpdateRPC();
        } else if (int.Parse(multi) > 1) {
            rpc.Details = "複数の動画を視聴中";
            rpc.State = "";
            UpdateRPC();
        } else {
            discord.Dispose();
        }
    }


    void UpdateRPC() {
        var activityManager = discord.GetActivityManager();
        activityManager.UpdateActivity(rpc, (res) => {
            if (res == Discord.Result.Ok)
            {
                Debug.LogError("Everything is fine");
            }
        });
    }

    void ReadINI() {
        IniFileReader ini = new IniFileReader(path, Encoding.GetEncoding("shift_jis"));
        ini.SetSection("YouTube_Status");
        
        active = ini.GetValue("active");
        multi = ini.GetValue("multi");
        title= ini.GetValue("title");
        url = ini.GetValue("url");
    }

    private void OnApplicationQuit()
    {
        var activityManager = discord.GetActivityManager();
        activityManager.ClearActivity((res) =>
        {
            if (res == Discord.Result.Ok)
            {
                Debug.LogError("Clear");
            }
        });
        discord.RunCallbacks();
        discord.Dispose();
    }
}
