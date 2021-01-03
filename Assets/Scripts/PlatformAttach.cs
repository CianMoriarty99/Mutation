using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttach : MonoBehaviour
{
    public GameObject Player;

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject == Player)
        {
            Player.transform.parent = transform;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject == Player)
        {
            Player.transform.parent = null;
        }
    }
}
