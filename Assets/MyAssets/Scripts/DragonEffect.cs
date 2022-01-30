using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class DragonEffect : MonoBehaviour
{
    public static event Action OnDealDamage;
    private string state = "ATTACK";

    [SerializeField] private float health = 150;
    private float currHealth;

    private bool canHeal=true;
    [SerializeField] private float healRange;

    [SerializeField] private GameObject Fire;
    [SerializeField] private GameObject HealPsystem;
    [SerializeField] private Transform Player;

    public GameObject gameWinUI;

    [SerializeField] private Light2D healLight;

    public Image image;

    AudioManager audioManager;

    private void Start()
    {
        gameWinUI.SetActive(false);
        audioManager = FindObjectOfType<AudioManager>();
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
            if (audioManager != null) audioManager.Play("DragonGrawl");
            if (audioManager != null) audioManager.Play("Fire");
            StartCoroutine(StateManager());
        }
        if(currHealth<50 && canHeal)
        {
            state = "HEAL";
            HealPsystem.GetComponent<ParticleSystem>().Play();
            if (audioManager != null) audioManager.Play("Heal");
            healLight.enabled = true;
            canHeal = false;
            StartCoroutine(StateManager());
        }
        if(state == "HEAL")
        {
            currHealth += 0.1f;
            if(Mathf.Abs(Player.position.x-transform.position.x)<6)
            {
                Player.GetComponent<PlayerDamage>().Heal(0.1f);
            }
        }
        image.GetComponent<Image>().fillAmount = currHealth / health;
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
        if (currHealth <= 0)
        {
            Destroy(gameObject);
            gameWinUI.SetActive(true);
        }
    }
}
