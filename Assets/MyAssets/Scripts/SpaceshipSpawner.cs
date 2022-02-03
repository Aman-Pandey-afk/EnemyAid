using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SpaceshipSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spaceShip;
    [SerializeField] private Transform PlayerTransform;
    [SerializeField] private Vector2 speedRange;
    [SerializeField] private Vector2 spawnRange;

    private int spaceshipCount = 0;
    public static int totalSpaceshipCount=5;

    private void Start()
    {
        StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        while (spaceshipCount < totalSpaceshipCount)
        {
            float speed = Random.Range(speedRange.x, speedRange.y);
            float spawnCor = Random.Range(spawnRange.x, spawnRange.y);
            GameObject newSpaceship = Instantiate(spaceShip, transform.position + new Vector3(spawnCor,0), Quaternion.identity);
            newSpaceship.GetComponent<AIDestinationSetter>().target = PlayerTransform;
            newSpaceship.GetComponent<AIPath>().maxSpeed = speed;
            spaceshipCount++;
            yield return new WaitForSeconds(2);
        }
    }
}
