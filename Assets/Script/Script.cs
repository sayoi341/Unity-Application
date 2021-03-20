using UnityEngine;
using System;
using System.Text;
using Discord;
using System.Threading.Tasks;



public class Script : MonoBehaviour {
    private string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\approvers\\youtube_status\\status.ini";

    string active;
    string multi;
    bool active_tab;
    string title;
    string url;


    public string _details;
    public string _state;

    public bool auto = false;
    
    Discord.Discord discord;

    private Discord.Activity rpc;

    private bool old_active = false;

    void Start() {
        discord = new Discord.Discord(820934372634132530, (System.UInt64) Discord.CreateFlags.Default);
        
        actStart();
        UpdateRPC();
    }

    void Update() {
        ReadINI();
        discord.RunCallbacks();
    }
    
    void ReadINI() {
        IniFileReader ini = new IniFileReader(path, Encoding.GetEncoding("shift_jis"));
        ini.SetSection("YouTube_Status");
        active = ini.GetValue("active");
        multi = ini.GetValue("multi");
        title= ini.GetValue("title");
        url = ini.GetValue("url");
    }

    private void OnApplicationQuit() {
        var activityManager = discord.GetActivityManager();
        activityManager.ClearActivity((res) => {
            if (res == Discord.Result.Ok) {
                Debug.LogError("Clear");
            }
        });
        discord.RunCallbacks();
        discord.Dispose();
    }

    void actStart() {
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        int cur_time = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
        rpc = new Activity() {
            Timestamps = new ActivityTimestamps() {
                Start = cur_time,
            },
            Assets = new ActivityAssets() {
                LargeImage = "icon",
                LargeText = "YouTube Status"
            }
        };
    }


    public void UpdateRPC() {
        var activityManager = discord.GetActivityManager();
        
        if (active == "true") {

            if (old_active) {
                actStart();
            }
            
            if (int.Parse(multi) > 1) {
                _details = "複数の動画を視聴中";
                _state = "";
            } else {
                _details = "YouTubeを視聴中";
                _state = title;
            }
            
            rpc.Details = _details; 
            rpc.State = _state;
            
            activityManager.UpdateActivity(rpc, (res) => {
                if (res == Discord.Result.Ok) {
                    Debug.LogError("Everything is fine");
                }
            });

            old_active = false;
        } else
        {
            _details = "";
            _state = "";
            activityManager.ClearActivity((res) => {
                if (res == Discord.Result.Ok) {
                    Debug.LogError("Clear");
                }
            });

            old_active = true;
        }
    }

    public void on() {
        if (auto) {
            auto = false;
        } else
        {
            auto = true;
            everUp();
        }
    }

    async void everUp() {
        while (auto) {
            await Task.Delay(1000);
            UpdateRPC();
        }
    }
}
