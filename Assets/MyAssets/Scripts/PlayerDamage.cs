using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] private float playerHealth;
    [SerializeField] private float skeletalDamage;
    [SerializeField] private float continousDamage;
    [SerializeField] private float dragonDamage;

    private float shieldTime = 0;
    private string state = "NOSHIELD";

    private void Start()
    {
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
                StartCoroutine(ShieldTiming());
            }
        }
    }

    IEnumerator ShieldTiming()
    {
        yield return new WaitForSeconds(shieldTime);
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
