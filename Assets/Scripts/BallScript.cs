using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour, IResetable
{
    private Vector2 startPos;
    public void Reset()
    {
        gameObject.transform.position = startPos;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (gameObject.GetComponent<Rigidbody2D>().velocity.x > 0.5f || gameObject.GetComponent<Rigidbody2D>().velocity.x < -0.5f)
        {
            if (!((bool)(AudioManager.instance.FindMusic("Roll")?.source.isPlaying)))
            {
                AudioManager.instance?.Play("Roll");
            }
        }
        else
        {
            AudioManager.instance?.Stop("Roll");
        }
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
