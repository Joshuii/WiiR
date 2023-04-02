using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    // Update is called once per frame
    void Update()
    {
        GetComponent<Renderer>().material.SetFloat("distance", gameManager.Distance);
    }
}
