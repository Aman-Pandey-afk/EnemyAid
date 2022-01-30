using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkeletonEffect : MonoBehaviour
{
    [SerializeField] private int health=10;
    private int currHealth;

    public static event Action OnDealDamage;
    public static event Action OnDestroyed;

    private string state="ATTACK";

    AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        currHealth = health;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && state=="ATTACK")
        {
            state = "IDLE";
            OnDealDamage?.Invoke();
            StartCoroutine(StateManager());
        }
    }

    public void TakeDamage(int damage)
    {
        currHealth -= damage;
        if (currHealth <= 0)
        {
            Destroy(gameObject);
            if (audioManager != null) audioManager.Play("SkeletonDie");
            OnDestroyed?.Invoke();
        }
    }

    IEnumerator StateManager()
    {
        yield return new WaitForSeconds(2);
        state = "ATTACK";
    }
}
