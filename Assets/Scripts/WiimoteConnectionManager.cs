using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class WiimoteConnectionManager : MonoBehaviour
{
    public Wiimote HeadWiimote;

    // Start is called before the first frame update
    void Awake()
    {
        WiimoteManager.FindWiimotes();
        if (WiimoteManager.Wiimotes.Count == 0)
        {
            Debug.LogWarning("No wiimotes found");
            return;
        }
        HeadWiimote = WiimoteManager.Wiimotes[0];
        HeadWiimote.SetupIRCamera(IRDataType.EXTENDED);
        HeadWiimote.SendPlayerLED(true, false, false, true);
    }

    // Update is called once per frame
    void Update()
    {
        int ret = 0;
        do
        {
            HeadWiimote.ReadWiimoteData();
        } while (ret > 0);
    }

    private void OnDestroy()
    {
        WiimoteManager.Cleanup(HeadWiimote);
    }
}
