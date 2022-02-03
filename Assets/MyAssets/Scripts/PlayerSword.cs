using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private float attackRange;
    [SerializeField] private int swordDamage = 3;

    private int enemyCount = SkeletonSpawner.totalSkeletonCount*2;
    [SerializeField] private LevelLoader levelLoader;

    AudioManager audioManager;
    private bool disabled=false;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        SkeletonEffect.OnDestroyed += EnemyDestroyed;
        PlayerDamage.OnPlayerDie += Disable;
    }

    private void Update()
    {
        if(enemyCount<=0)
        {
            levelLoader.LoadNextLevel();
        }
    }

    void SwordAttack()
    {
        if (!disabled)
        {
            if (audioManager != null) audioManager.Play("BladeSwing");
            Collider2D[] enemyHitInfo = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
            foreach (Collider2D enemy in enemyHitInfo)
            {
                enemy.GetComponent<SkeletonEffect>().TakeDamage(swordDamage);
            }
        }
    }

    private void EnemyDestroyed()
    {
        enemyCount -= 1;
    }

    private void Disable()
    {
        disabled = true;
    }

    private void OnDestroy()
    {
        SkeletonEffect.OnDestroyed -= EnemyDestroyed;
        PlayerDamage.OnPlayerDie -= Disable;
    }
}
