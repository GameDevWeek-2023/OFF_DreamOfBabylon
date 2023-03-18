using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHitGround : MonoBehaviour
{
    void OnCollisionEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            AudioManager.instance?.Play("Drop");
        }
    }
}
