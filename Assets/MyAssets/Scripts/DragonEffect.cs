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

    [SerializeField] private Light2D healLight;

    [SerializeField] private GameObject GameWinUI;

    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image image;

    bool disabled;

    AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        currHealth = health;
        healLight.enabled = false;
        HealPsystem.GetComponent<ParticleSystem>().Stop();

        PlayerDamage.OnPlayerDie += Disable;
    }

    // Update is called once per frame
    void Update()
    {
        if (!disabled)
        {
            if (state == "ATTACK")
            {
                state = "IDLE";
                Fire.GetComponent<ParticleSystem>().Play();
                OnDealDamage?.Invoke();
                if (audioManager != null) audioManager.Play("DragonGrawl");
                if (audioManager != null) audioManager.Play("Fire");
                StartCoroutine(StateManager());
            }
            if (currHealth < 50 && canHeal)
            {
                state = "HEAL";
                HealPsystem.GetComponent<ParticleSystem>().Play();
                if (audioManager != null) audioManager.Play("Heal");
                healLight.enabled = true;
                canHeal = false;
                StartCoroutine(StateManager());
            }
            if (state == "HEAL")
            {
                currHealth += 0.35f;
                if (Mathf.Abs(Player.position.x - transform.position.x) < healRange)
                {
                    Player.GetComponent<PlayerDamage>().Heal(0.35f);
                }
            }

            slider.value = currHealth;
            image.color = gradient.Evaluate(slider.normalizedValue);
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
            yield return new WaitForSeconds(5);
            currHealth += 10;
            if (Mathf.Abs(Player.position.x - transform.position.x) < healRange) Player.GetComponent<PlayerDamage>().Heal(10f);
            HealPsystem.GetComponent<ParticleSystem>().Stop();
            healLight.enabled = false;
            state = "ATTACK";
        }
    }

    private void Disable()
    {
        disabled = true;
    }

    public void TakeDamage(float damage)
    {
        currHealth -= damage;
        if (currHealth <= 0)
        {
            GameWinUI.SetActive(true);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        PlayerDamage.OnPlayerDie -= Disable;
    }
}
