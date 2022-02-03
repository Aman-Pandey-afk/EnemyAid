using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using TMPro;

public class PlayerDamage : MonoBehaviour
{
    // Variables

    [SerializeField] private float maxHealth = 100;
    private float playerHealth;

    [SerializeField] private float skeletalDamage=3;
    [SerializeField] private float continousDamage=0.05f;
    [SerializeField] private float dragonDamage=7;

    private float shieldTime = 0;
    private string state = "NOSHIELD";

    public static event Action OnPlayerDie;

    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image image;

    [SerializeField] private GameObject shield;

    AudioManager audioManager;

    public GameObject GameLoseUI;
    public TextMeshProUGUI text;


    // Start & Update

    private void Start()
    {
        playerHealth = maxHealth;

        GameLoseUI.SetActive(false);
        if(shield!=null) shield.SetActive(false);

        audioManager = FindObjectOfType<AudioManager>();

        SkeletonEffect.OnDestroyed += ManageShield;
        SkeletonEffect.OnDealDamage += TakeSkeletalDamage;

        SpaceshipEffect.OnDealDamage += TakeSpaceshipDamage;

        DragonEffect.OnDealDamage += TakeDragonDamage;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && state=="NOSHIELD" && SceneManager.GetActiveScene().buildIndex == 1)
        {
            if(shieldTime>0)
            {
                state = "SHIELD";
                if (audioManager != null) audioManager.Play("Shield");
                shield.SetActive(true);
                StartCoroutine(ShieldTiming());
            }
        }
        
        if(playerHealth<=0)
        {
            GameLoseUI.SetActive(true);
            OnPlayerDie();
        }

        if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            if (transform.position.x < 4) dragonDamage = 6.6f;
            else dragonDamage = 3.2f;
        }

        slider.value = playerHealth;
        image.color = gradient.Evaluate(slider.normalizedValue);
    }


    // Methods

    IEnumerator ShieldTiming()
    {
        text.text = "Shield in Use";
        yield return new WaitForSeconds(shieldTime);
        shield.SetActive(false);
        shieldTime = 0;
        state = "NOSHIELD";
    }

    private void ManageShield()
    {
        shieldTime += 0.65f;
        text.text = shieldTime.ToString() + "s";
    }

    private void TakeSkeletalDamage()
    {
        if(state=="NOSHIELD") playerHealth -= skeletalDamage;
    }
    private void TakeSpaceshipDamage()
    {
        playerHealth -= continousDamage;
    }
    private void TakeDragonDamage()
    {
        playerHealth -= dragonDamage;
    }
    public void Heal(float healAmount)
    {
        playerHealth += healAmount;
    }

    private void OnDestroy()
    {
        SkeletonEffect.OnDestroyed -= ManageShield;
        SkeletonEffect.OnDealDamage -= TakeSkeletalDamage;

        SpaceshipEffect.OnDealDamage -= TakeSpaceshipDamage;

        DragonEffect.OnDealDamage -= TakeDragonDamage;
    }
}
