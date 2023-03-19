using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class BallScript : MonoBehaviour, IResetable
{
    [SerializeField] AudioSource roll;
    private Vector2 startPos;
    public void ResetToStart()
    {
        gameObject.transform.position = startPos;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (gameObject.GetComponent<Rigidbody2D>().velocity.x > 0.5f || gameObject.GetComponent<Rigidbody2D>().velocity.x < -0.5f)
        {
            if (!((bool)(roll.isPlaying)))
            {
                roll.Play();
            }
        }
        else
        {
            roll.Stop();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        roll.Stop();
    }

    // Start is called before the first frame update
    void Start()
    {
        startPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
