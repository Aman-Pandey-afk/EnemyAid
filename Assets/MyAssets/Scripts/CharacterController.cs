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
    bool disabled;

    private Rect cameraRect;
    Vector3 bottomLeft;
    Vector3 topRight;

    AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        PlayerDamage.OnPlayerDie += Disable;

        bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight));

        cameraRect = new Rect(
        bottomLeft.x,
        bottomLeft.y,
        topRight.x - bottomLeft.x,
        topRight.y - bottomLeft.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.isGamePaused) return;

        //Motion
        if (!disabled)
        {
            float movHor = Input.GetAxisRaw("Horizontal") * speed;

            if (movHor > 0)
            {
                if(transform.position.x < cameraRect.xMax) transform.position += Vector3.right * movHor;
                isFacingRight = true;
                if (audioManager != null) audioManager.Play("Walk");
            }

            else if (movHor < 0)
            {
                if(transform.position.x > cameraRect.xMin) transform.position += Vector3.right * movHor;
                isFacingRight = false;
                if (audioManager != null) audioManager.Play("Walk");
            }

            anim.SetFloat("checkSpeed", Mathf.Abs(movHor));

            if (isFacingRight) transform.rotation = Quaternion.Euler(0, 180, 0);
            else transform.rotation = Quaternion.Euler(0, 0, 0);

            //Jump
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                anim.SetBool("isJump", true);
            }

            //Attack
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0)) anim.SetTrigger("isAttacking");
            }
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    OnLaserAttack?.Invoke();
                }
                else PlayerLaser.isLaserFiring = false;
            }
        }
    }

    private void Disable()
    {
        disabled = true;
    }

    public void OnLand()
    {
        anim.SetBool("isJump", false);
    }

    private void OnDestroy()
    {
        PlayerDamage.OnPlayerDie -= Disable;
    }
}
