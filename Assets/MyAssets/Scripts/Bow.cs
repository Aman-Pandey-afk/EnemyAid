using UnityEngine;
using TMPro;

public class Bow : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private Vector2 launchForceRange;
    private float launchForce;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform bowGraphic;

    [SerializeField] private DragonEffect drgEffect;

    [SerializeField] private GameObject pathPoint;
    GameObject[] pathPoints;
    [SerializeField] private int noOfPoints;
    [SerializeField] private float spaceBetweenPoints;
    [SerializeField] private TextMeshProUGUI text;

    int arrowNo = 25;

    private Vector2 dir;
    AudioManager audioManager;

    bool disabled;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        launchForce = Random.Range(launchForceRange.x, launchForceRange.y);
        pathPoints = new GameObject[noOfPoints];
        for (int i = 0; i < noOfPoints; i++)
        {
            pathPoints[i] = Instantiate(pathPoint, firePoint.position, Quaternion.identity);
        }

        PlayerDamage.OnPlayerDie += Disable;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.isGamePaused) return;
        if (!disabled)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 bowPosition = transform.position;

            dir = mousePosition - bowPosition;
            transform.right = dir;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (arrowNo > 0)
                {
                    Shoot();
                    launchForce = Random.Range(launchForceRange.x, launchForceRange.y);
                    arrowNo--;
                    text.text = arrowNo.ToString();
                }
            }
            for (int i = 0; i < noOfPoints; i++)
            {
                pathPoints[i].transform.position = PointPosition(i * spaceBetweenPoints);
            }
        }
    }

    private void Shoot()
    {
        if (audioManager != null) audioManager.Play("Bow");
        GameObject newArrow = Instantiate(arrow, firePoint.position, firePoint.rotation);
        newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;
        newArrow.GetComponent<Arrow>().dragonEffect = drgEffect;
    }

    private void Disable()
    {
        disabled = true;
    }

    Vector2 PointPosition(float t)
    {
        Vector2 posi = (Vector2)firePoint.position + (dir.normalized * launchForce * t) + (0.5f * t * t * Physics2D.gravity);
        return posi;
    }

    private void OnDestroy()
    {
        PlayerDamage.OnPlayerDie -= Disable;
    }
}
