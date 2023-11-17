using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    [SerializeField] private Transform[] coverPositions;

    public Transform GetNearestCover(Vector3 pos)
    {
        float lowestMag = Mathf.Infinity;
        Transform nearestCover = null;

        for (int i = 0; i < coverPositions.Length; i++)
        {
            if (nearestCover == null)
            {
                nearestCover = coverPositions[i];
                lowestMag = (nearestCover.position - pos).magnitude;
            }
            else
            {
                if ((coverPositions[i].position - pos).magnitude < lowestMag)
                {
                    nearestCover = coverPositions[i];
                    lowestMag = (nearestCover.position - pos).magnitude;
                }
            }
        }

        return nearestCover;
    }
}
