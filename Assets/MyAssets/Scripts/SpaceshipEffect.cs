using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class SpaceshipEffect : MonoBehaviour
{
    private Transform targetPlayer;

    public static event Action OnDealDamage;
    public static event Action OnDestroyed;

    [SerializeField] private float attackRange;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LineRenderer enemyLaser;

    [SerializeField] private float health = 10;
    private float currHealth;

    [SerializeField] private GameObject DeadUFO;

    AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        targetPlayer = gameObject.GetComponent<AIDestinationSetter>().target;
        enemyLaser.enabled = false;
        currHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.x - targetPlayer.position.x) < attackRange)
        {
            Shoot();
        }
        else enemyLaser.enabled = false;
    }
    private void Shoot()
    {
        Vector2 targetDir = targetPlayer.position - attackPoint.position;
        if (audioManager != null)  audioManager.Play("SpaceshipLaser");
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, targetDir);
        if (hitInfo)
        {
            enemyLaser.enabled = true;
            OnDealDamage?.Invoke();
            enemyLaser.SetPosition(0, attackPoint.position);
            enemyLaser.SetPosition(1, hitInfo.point);
        }
    }

    public void TakeDamage(float damage)
    {
        currHealth -= damage;
        if (currHealth <= 0)
        {
            if (audioManager != null) audioManager.Play("SpaceshipDie");
            Instantiate(DeadUFO, transform.position, Quaternion.Euler(0,0,145));
            Destroy(gameObject);
            OnDestroyed?.Invoke();
        }
    }
}
