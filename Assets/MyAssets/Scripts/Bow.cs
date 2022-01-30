using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private float launchForce;
    [SerializeField] private Transform firePoint;

    [SerializeField] private DragonEffect drgEffect;

    [SerializeField] private GameObject pathPoint;
    GameObject[] pathPoints;
    [SerializeField] private int noOfPoints;
    [SerializeField] private float spaceBetweenPoints;

    private Vector2 dir;

    private void Start()
    {
        pathPoints = new GameObject[noOfPoints];
        for(int i=0; i<noOfPoints; i++)
        {
            pathPoints[i] = Instantiate(pathPoint, firePoint.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 bowPosition = transform.position;
        dir = mousePosition - bowPosition;
        transform.right = dir;

        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        for (int i = 0; i < noOfPoints; i++)
        {
            pathPoints[i].transform.position = PointPosition(i * spaceBetweenPoints);
        }
    }

    private void Shoot()
    {
        GameObject newArrow = Instantiate(arrow, firePoint.position, firePoint.rotation);
        newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;
        newArrow.GetComponent<Arrow>().dragonEffect = drgEffect;
    }

    Vector2 PointPosition(float t)
    {
        Vector2 posi = (Vector2)firePoint.position + (dir.normalized * launchForce * t) + (0.5f*Physics2D.gravity*t*t);
        return posi;
    }
}
