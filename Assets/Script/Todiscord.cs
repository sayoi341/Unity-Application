using UnityEngine;
using DiscordPresence;
public class Todiscord : MonoBehaviour
{

    private string now;
    void Update()
    {
        now = Time.time.ToString();
        
        PresenceManager.UpdatePresence(detail:"test4", state:now);
    }
}
