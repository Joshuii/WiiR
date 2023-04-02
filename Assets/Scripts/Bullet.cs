using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 0, speed));
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit");
        if (collision.collider.TryGetComponent(out Glass glass))
        {
            Debug.Log("glass");
            glass.Shatter();
        }
        Destroy(gameObject);
    }
}
