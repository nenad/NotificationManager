using UnityEngine;
using System.Collections;

public class MessageTrigger : MonoBehaviour {

    
    public string message;
    public float duration;

    public NM.Symbol symbol;
    new public NM.Animation animation;

    public bool destroyOnHit;
    

    void OnTriggerEnter()
    {
        NM.NotificationManager.ShowMessage(message, symbol, duration, animation);
        if (destroyOnHit)
        {
            Destroy(gameObject);
        }
    }

}
