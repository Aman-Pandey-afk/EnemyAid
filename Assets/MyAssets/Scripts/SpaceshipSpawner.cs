using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SpaceshipSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spaceShip;
    [SerializeField] private Transform PlayerTransform;
    private int spaceshipCount = 0;
    public static int totalSpaceshipCount = 2;

    private void Start()
    {
        StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        while (spaceshipCount < totalSpaceshipCount)
        {
            GameObject newSkeleton = Instantiate(spaceShip, transform.position, Quaternion.identity);
            newSkeleton.GetComponent<AIDestinationSetter>().target = PlayerTransform;
            spaceshipCount++;
            yield return new WaitForSeconds(2);
        }
    }
}
