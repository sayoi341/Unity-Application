using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Auto : MonoBehaviour
{
    GameObject Master;
    Script script;
    
    public Text text;
    void Start()
    {
        Master = GameObject.Find("MasterOBJ");
        script = Master.GetComponent<Script>();
    }

    // Update is called once per frame
    void Update() {
        if (script.auto)
        {
            text.text = "on";
        }
        else
        {
            text.text = "off";
        }
    }
}
