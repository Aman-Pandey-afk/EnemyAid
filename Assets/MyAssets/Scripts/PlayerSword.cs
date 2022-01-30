using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private float attackRange;
    [SerializeField] private int swordDamage = 4;

    private int enemyCount = SkeletonSpawner.totalSkeletonCount;
    [SerializeField] private LevelLoader levelLoader;

    private void Start()
    {
        SkeletonEffect.OnDestroyed += EnemyDestroyed;
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
        Collider2D[] enemyHitInfo = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in enemyHitInfo)
        {
            enemy.GetComponent<SkeletonEffect>().TakeDamage(swordDamage);
        }
    }

    private void EnemyDestroyed()
    {
        enemyCount -= 1;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
