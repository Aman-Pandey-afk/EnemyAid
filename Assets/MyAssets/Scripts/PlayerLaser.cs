using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser: MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private float laserDamage = 0.08f;

    public static bool isLaserFiring = false;
    [SerializeField] private LineRenderer attackLaser;

    [SerializeField] private float ammo=20;

    [SerializeField] private LevelLoader levelLoader;
    private int enemyCount = SpaceshipSpawner.totalSpaceshipCount;

    AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        SpaceshipEffect.OnDestroyed += EnemyDestroyed;
        CharacterController.OnLaserAttack += LaserAttack;
        attackLaser.enabled = false;
    }

    private void Update()
    {
        if (enemyCount <= 0)
        {
            levelLoader.LoadNextLevel();
        }
        if (!isLaserFiring) attackLaser.enabled = false;
    }

    private void EnemyDestroyed()
    {
        enemyCount -= 1;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("DEADUFO") && Input.GetKeyDown(KeyCode.E))
        {
            ammo += 6;
            Destroy(collision.gameObject);
        }
    }

    private void LaserAttack()
    {
        if (ammo > 0)
        {
            isLaserFiring = true;
            if (audioManager != null) audioManager.Play("PlayerLaser");
            int dir;
            if (CharacterController.isFacingRight) dir = 1;
            else dir = -1;

            Vector2 attackOrigin = attackPoint.position;
            RaycastHit2D hitInf = Physics2D.Raycast(attackOrigin, new Vector2(attackOrigin.x + dir, attackOrigin.y + 1) - attackOrigin);

            if (hitInf)
            {
                SpaceshipEffect enemy = hitInf.transform.GetComponent<SpaceshipEffect>();

                if (enemy != null) enemy.TakeDamage(laserDamage);

                attackLaser.SetPosition(0, attackPoint.position);
                attackLaser.SetPosition(1, hitInf.point);
            }
            else
            {
                attackLaser.SetPosition(0, attackPoint.position);
                attackLaser.SetPosition(1, new Vector2(attackOrigin.x + dir * 100, attackOrigin.y + 100));
            }
            attackLaser.enabled = true;
            ammo -= 0.02f;
        }
        else
        {
            print("NO AMMO!");
            attackLaser.enabled = false;
        }
    }
}
