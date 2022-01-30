using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    public static event Action OnLaserAttack;

    [SerializeField] private Animator anim;
    [SerializeField] private float speed=0.5f;

    public static bool isFacingRight=false;

    AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Motion
        float movHor = Input.GetAxisRaw("Horizontal") * speed;
        transform.position += Vector3.right*movHor;
        anim.SetFloat("checkSpeed", Mathf.Abs(movHor));
        if (movHor > 0)
        {
            isFacingRight = true;
            transform.rotation = Quaternion.Euler(0, 180, 0);
            if(audioManager!=null) audioManager.Play("Walk");
        }

        else if (movHor < 0)
        {
            isFacingRight = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            if (audioManager != null) audioManager.Play("Walk");
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            anim.SetBool("isJump", true);
        }

        //Attack
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0)) anim.SetTrigger("isAttacking");
        }
        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                OnLaserAttack?.Invoke();
            }
            else PlayerLaser.isLaserFiring = false;
        }
    }
    public void OnLand()
    {
        anim.SetBool("isJump", false);
    }
}
