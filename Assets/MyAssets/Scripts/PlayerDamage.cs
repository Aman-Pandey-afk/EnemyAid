using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour
{
    private float maxHealth = 100;
    public static float playerHealth=100;
    [SerializeField] private float skeletalDamage=3;
    [SerializeField] private float continousDamage=0.05f;
    [SerializeField] private float dragonDamage=7;

    private float shieldTime = 0;
    private string state = "NOSHIELD";

    public Image image;

    [SerializeField] private GameObject shield;

    AudioManager audioManager;

    public GameObject GameLoseUI;

    private void Start()
    {
        maxHealth = playerHealth;
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
        image.GetComponent<Image>().fillAmount = playerHealth / maxHealth;
        if(playerHealth<0)
        {
            GameLoseUI.SetActive(true);
            Destroy(gameObject);
        }
    }

    IEnumerator ShieldTiming()
    {
        yield return new WaitForSeconds(shieldTime);
        shield.SetActive(false);
        shieldTime = 0;
        state = "NOSHIELD";
    }

    private void ManageShield()
    {
        shieldTime += 0.5f;
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
}
