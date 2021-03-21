using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using UnityEngine.PlayerLoop;


public class ExtensionMS : MonoBehaviour
{

    public GameObject MasterObj;
    public GameObject BUpdate;
    public GameObject BAuto;
    public GameObject Details;
    public GameObject State;

    public InputField thisI;
    public Text thisT;

    string path = Directory.GetCurrentDirectory();
    
    [System.Serializable]
    public class hostJSONTYPE {
        public string name;
        public string description;
        public string path;
        public string type;
        public string[] allowed_origins;
    }

    private string Flag = "false";

    void Start() {
        ReadINI();     
        Debug.Log(path);
        
        if (Flag == "true") {
            Destroy(this.gameObject);
        } else {
            MasterObj.SetActive(false);
            BUpdate.SetActive(false);
            BAuto.SetActive(false);
            Details.SetActive(false);
            State.SetActive(false);
        }
        
        
    }

    void Update()
    {
        
    }

    void ReadINI() {
        IniFileReader ini = new IniFileReader(path + "\\host\\config.ini", Encoding.GetEncoding("shift_jis"));
        ini.SetSection("YouTube_Status");
        Flag = ini.GetValue("initial_settings");
    }

    public void set() {
        hostJSONTYPE hostJSON = new hostJSONTYPE();
        hostJSON.name = "youtube_status";
        hostJSON.description = "When you watching YouTube, Discord show your watching activity by this app.";
        hostJSON.path = path + "\\host\\main.py";
        hostJSON.type = "stdio";
        hostJSON.allowed_origins = new []{"chrome-extension://" + thisI.text + "/"};

        savePlayerData(hostJSON);
        
        MasterObj.SetActive(true);
        BUpdate.SetActive(true);
        BAuto.SetActive(true);
        Details.SetActive(true);
        State.SetActive(true);
        Destroy(this.gameObject);
        
    }

    public void UpdateText() {
        thisT.text = thisI.text;
    }
    
    public void savePlayerData(hostJSONTYPE json) {
        StreamWriter writer;

        string jsonstr = JsonUtility.ToJson (json);

        DirectoryUtils.SafeCreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\approvers\\youtube_status");

        writer = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\approvers\\youtube_status\\host.json", false);
        writer.Write (jsonstr);
        writer.Flush ();
        writer.Close ();
    }
}
