using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private WiimoteConnectionManager connectionManager;

    private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        gameManager.OnStopGame += (s, e) =>
        {
            canvas.enabled = true;
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.IsStarted && connectionManager.HandWiimote != null && connectionManager.HandWiimote.Button.a)
        { 
            gameManager.StartGame();
            canvas.enabled = false;
        }
    }
}
