using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingScript : MonoBehaviour
{
    private DragonBones.UnityArmatureComponent armatureComponent;
    private CubeMovementTest cubeMovement;
    private bool isDead;
    [SerializeField] AudioSource death;
    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        armatureComponent = GetComponentInChildren<DragonBones.UnityArmatureComponent>();//kann uns um die Ohren fliegen, weil CubeMovementTest und DyingScript beide auf die AmatureComponent zugreifen.
        cubeMovement = gameObject.GetComponent<CubeMovementTest>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Deadly"))
        {
            cubeMovement.PauseInputs(true);
            armatureComponent.animation.Play("Death", 1);
            death.Play();
            isDead = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Deadly"))
        {
            cubeMovement.PauseInputs(true);
            armatureComponent.animation.Play("Death", 1);
            armatureComponent.animation.timeScale = 0.8f;
            death.Play();
            isDead = true;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead && armatureComponent.animation.isCompleted)
        {
            RespornPlayer();
        }
    }

    private void RespornPlayer()
    {
        //gameObject.transform.position = CheckPoint.instance.GetRespornPosition();
        isDead = false;
        cubeMovement.PauseInputs(false);
        cubeMovement.Horizontal = 0;
        cubeMovement.ResetLevel();
    }
}
