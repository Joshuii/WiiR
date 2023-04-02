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

    public void Shatter(Collision collision)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                child.gameObject.SetActive(true);
                Debug.Log(child.name);
                Rigidbody rb = child.AddComponent<Rigidbody>();
                rb.AddForce(100f * new Vector3(Random.value, Random.value, Random.value));
                Destroy(child.gameObject, 1f);
            }

        }
    }
}
