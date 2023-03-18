using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour, IResetable
{
    Vector2 startPos;
    public void Reset()
    {
        gameObject.transform.parent = null;
        gameObject.transform.position = startPos;
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
