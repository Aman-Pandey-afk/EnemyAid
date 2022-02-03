using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerLaser: MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private float laserDamage = 0.08f;

    public static bool isLaserFiring = false;
    [SerializeField] private LineRenderer attackLaser;

    [SerializeField] private float ammo=18;
    public TextMeshProUGUI text;
    [SerializeField] private Color textColor;

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
            if (ammo < 0) text.color = textColor;

            ammo += 4f;
            Destroy(collision.gameObject);
            text.text = Mathf.RoundToInt(ammo).ToString();
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
            RaycastHit2D hitInf = Physics2D.Raycast(attackOrigin, new Vector2(attackOrigin.x + dir, attackOrigin.y + 1.2f) - attackOrigin);

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
            text.text = Mathf.RoundToInt(ammo).ToString();
        }
        else
        {
            text.color = Color.red;
            text.text = "No ammo";
            attackLaser.enabled = false;
        }
    }

    private void OnDestroy()
    {
        SpaceshipEffect.OnDestroyed -= EnemyDestroyed;
        CharacterController.OnLaserAttack -= LaserAttack;
    }
}
