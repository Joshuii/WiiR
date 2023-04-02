using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent Agent;
    private GameObject Player;

    [SerializeField]
    GameObject BulletPrefab;

    [SerializeField]
    float ShootDelaySeconds;

    [SerializeField]
    float ShootAttemptSeconds;

    [SerializeField]
    float ShootAngleError;

    // Start is called before the first frame update
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(UpdatePositionLoop());
        StartCoroutine(ShootLoop());
    }

    IEnumerator UpdatePositionLoop()
    {
        while (true)
        {
            Agent.destination = Player.transform.position;
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator ShootLoop()
    {
        float timeSinceLastShot = Mathf.Infinity;

        while (true)
        {
            if (ShootDelaySeconds <= timeSinceLastShot)
            {
                Vector3 direction = (Player.transform.position - (transform.position + new Vector3(0, 1.5f, 0))).normalized;
                Vector3 startPos = transform.position + new Vector3(0, 1.5f, 0) + (direction * 0.75f);
                if (Physics.Raycast(startPos, direction, out RaycastHit hit, 1000f))
                {
                    Debug.Log(hit.collider.gameObject.name);
                    if (hit.collider == null || hit.collider.gameObject == Player)
                    {
                        //Fire!
                        GameObject bullet = Object.Instantiate(BulletPrefab, startPos, Quaternion.LookRotation(direction));
                        timeSinceLastShot = 0;
                    }
                }
            }
            yield return new WaitForSeconds(ShootAttemptSeconds);
            timeSinceLastShot += ShootAttemptSeconds;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
