using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class WiimoteConnectionManager : MonoBehaviour
{
    public Wiimote HeadWiimote;
    public Wiimote HandWiimote;

    void Awake()
    {
        WiimoteManager.FindWiimotes();
        HeadWiimote = SetupWiimote(0, true, false, false, true);
        HandWiimote = SetupWiimote(1, false, true, true, false);
    }

    private Wiimote SetupWiimote(int index, bool led1, bool led2, bool led3, bool led4)
    {
        if (WiimoteManager.Wiimotes.Count <= index)
        {
            Debug.LogWarning("Not enough Wiimotes");
            return null;
        }

        Wiimote wiimote = WiimoteManager.Wiimotes[index];
        wiimote.SetupIRCamera(IRDataType.EXTENDED);
        wiimote.SendPlayerLED(led1, led2, led3, led4);
        return wiimote;
    }

    void Update()
    {
        UpdateWiimote(HeadWiimote);
        UpdateWiimote(HandWiimote);
    }

    private void UpdateWiimote(Wiimote wiimote)
    {
        if (wiimote != null)
        {
            int ret;
            do
            {
                ret = wiimote.ReadWiimoteData();
            } while (ret > 0);
        }
    }

    private void OnDestroy()
    {
        if (HeadWiimote != null) WiimoteManager.Cleanup(HeadWiimote);
        if (HandWiimote != null) WiimoteManager.Cleanup(HandWiimote);
    }
}
