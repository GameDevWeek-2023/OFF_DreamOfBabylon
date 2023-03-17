using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public static CheckPoint instance;
    [SerializeField] private int numberOfCheckPoint = -1;
    [SerializeField] private float newCheckpointEffectTime = 1f;

    public int NumberOfCheckPoint { get => numberOfCheckPoint;}

    private Vector3 respornPosition;
    private ParticleSystem ps;
    private ParticleSystem psa;

    // Start is called before the first frame update
    void Start()
    {
        if(numberOfCheckPoint == -1)
        {
            Debug.LogWarning("Checkpoint has no number assigned");
        }
        respornPosition = gameObject.transform.GetChild(0).position;
        ps = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        psa = gameObject.transform.GetChild(2).GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(CheckPoint.instance == null || CheckPoint.instance.NumberOfCheckPoint < numberOfCheckPoint)
        {
            CheckPoint.instance?.ps.gameObject.SetActive(false);
            CheckPoint.instance = this;
            Debug.Log("New Checkpoint is CheckPoint " + numberOfCheckPoint + ".");
            psa.gameObject.SetActive(true);
            StartCoroutine(NewCheckpointEffect());

        }
    }
    private IEnumerator NewCheckpointEffect()
    {
        yield return new WaitForSeconds(0.5f);
        ps.gameObject.SetActive(true);
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
