using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    public float Speed;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 0, Speed));
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
