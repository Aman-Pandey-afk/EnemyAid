using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SkeletonSpawner : MonoBehaviour
{
    [SerializeField] private GameObject skeleton;
    [SerializeField] private Transform PlayerTransform;
    [SerializeField] private Vector2 speedRange;

    private int skeletonCount=0;
    public static int totalSkeletonCount = 5;

    private void Start()
    {
       StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        while (skeletonCount < totalSkeletonCount)
        {
            float speed = Random.Range(speedRange.x, speedRange.y);
            GameObject newSkeleton = Instantiate(skeleton, transform.position, Quaternion.identity);
            newSkeleton.GetComponent<AIDestinationSetter>().target = PlayerTransform;
            newSkeleton.GetComponent<AIPath>().maxSpeed = speed;
            skeletonCount++;
            yield return new WaitForSeconds(1.25f);
        }
    }
}
