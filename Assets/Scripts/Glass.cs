using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Glass : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shatter()
    {
        GetComponent<BoxCollider>().enabled = false;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            Rigidbody rb = child.AddComponent<Rigidbody>();
            rb.AddForce(100f * new Vector3(Random.value, Random.value, Random.value));
        }
    }
}
