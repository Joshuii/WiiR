using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    GameObject EnemeyPrefab;

    [SerializeField]
    int TotalSpawns;

    [SerializeField]
    float SpawnIntervalSeconds;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        for (int i = 0; i < TotalSpawns; i++)
        {
            Object.Instantiate(EnemeyPrefab, transform.position, Quaternion.FromToRotation(transform.position, Vector3.zero));
            yield return new WaitForSeconds(SpawnIntervalSeconds);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
