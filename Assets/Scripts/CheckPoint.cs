using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public static CheckPoint instance;
    [SerializeField] private int numberOfCheckPoint = -1;

    public int NumberOfCheckPoint { get => numberOfCheckPoint;}

    private Vector3 respornPosition;

    // Start is called before the first frame update
    void Start()
    {
        if(numberOfCheckPoint == -1)
        {
            Debug.LogWarning("Checkpoint has no number assigned");
        }
        respornPosition = gameObject.transform.GetChild(0).position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(CheckPoint.instance == null || CheckPoint.instance.NumberOfCheckPoint < numberOfCheckPoint)
        {
            CheckPoint.instance = this;
            Debug.Log("New Checkpoint is CheckPoint " + numberOfCheckPoint + ".");
        }
    }

    public Vector3 GetRespornPosition()
    {
        return respornPosition;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
