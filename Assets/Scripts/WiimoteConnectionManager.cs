using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class WiimoteConnectionManager : MonoBehaviour
{
    public Wiimote HeadWiimote;

    // Start is called before the first frame update
    void Start()
    {
        WiimoteManager.FindWiimotes();
        if (WiimoteManager.Wiimotes.Count == 0)
        {
            Debug.LogWarning("No wiimotes found");
            return;
        }
        HeadWiimote = WiimoteManager.Wiimotes[0];
        HeadWiimote.SetupIRCamera(IRDataType.EXTENDED);
    }

    // Update is called once per frame
    void Update()
    {
        int ret = 0;
        do
        {
            HeadWiimote.ReadWiimoteData();
        } while (ret > 0);
        Debug.Log(HeadWiimote.Button.a);
    }

    private void OnDestroy()
    {
        WiimoteManager.Cleanup(HeadWiimote);
    }
}
