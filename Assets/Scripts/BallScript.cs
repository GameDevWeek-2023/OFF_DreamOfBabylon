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
