using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class State : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject Master;
    Script script;
    
    public InputField inputField;
    public Text text;

    void Start() {
        Master = GameObject.Find("MasterOBJ");
        script = Master.GetComponent<Script>();
    }

    void Update() {
        inputField.text = script._state;
    }
    
    public void InputText(){
        text.text = inputField.text;
        
    }
}
