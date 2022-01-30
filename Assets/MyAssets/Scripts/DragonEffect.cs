using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Experimental.Rendering.Universal;

public class DragonEffect : MonoBehaviour
{
    public static event Action OnDealDamage;
    private string state = "ATTACK";

    [SerializeField] private float health = 10;
    private float currHealth;

    private bool canHeal=true;
    [SerializeField] private float healRange;

    [SerializeField] private GameObject Fire;
    [SerializeField] private GameObject HealPsystem;
    [SerializeField] private Transform Player;

    [SerializeField] private Light2D healLight;

    private void Start()
    {
        currHealth = health;
        healLight.enabled = false;
        HealPsystem.GetComponent<ParticleSystem>().Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == "ATTACK")
        {
            OnDealDamage?.Invoke();
            state = "IDLE";
            Fire.GetComponent<ParticleSystem>().Play();
            StartCoroutine(StateManager());
        }
        if(currHealth<5 && canHeal)
        {
            state = "HEAL";
            HealPsystem.GetComponent<ParticleSystem>().Play();
            healLight.enabled = true;
            canHeal = false;
            StartCoroutine(StateManager());
        }
        if(state == "HEAL")
        {
            currHealth += 0.01f;
            if(Mathf.Abs(Player.position.x-transform.position.x)<6)
            {
                Player.GetComponent<PlayerDamage>().Heal(0.01f);
            }
        }
    }

    IEnumerator StateManager()
    {
        if (state == "IDLE")
        {
            yield return new WaitForSeconds(2);
            state = "ATTACK";
        }
        else if(state=="HEAL")
        {
            yield return new WaitForSeconds(3);
            HealPsystem.GetComponent<ParticleSystem>().Stop();
            healLight.enabled = false;
            state = "ATTACK";
        }
    }

    public void TakeDamage(float damage)
    {
        currHealth -= damage;
        if (currHealth <= 0) Destroy(gameObject);
    }
}
