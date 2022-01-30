using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SkeletonSpawner : MonoBehaviour
{
    [SerializeField] private GameObject skeleton;
    [SerializeField] private Transform PlayerTransform;
    private int skeletonCount=0;
    public static int totalSkeletonCount = 8;

    private void Start()
    {
       StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        while (skeletonCount < totalSkeletonCount)
        {
            GameObject newSkeleton = Instantiate(skeleton, transform.position, Quaternion.identity);
            newSkeleton.GetComponent<AIDestinationSetter>().target = PlayerTransform;
            skeletonCount++;
            yield return new WaitForSeconds(2);
        }
    }
}
