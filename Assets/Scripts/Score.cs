using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI text;

    [SerializeField]
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        gameManager.OnStopGame += (s, e) =>
        {
            text.text = "Score: " + (int)gameManager.Distance;
            text.enabled = true;
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
