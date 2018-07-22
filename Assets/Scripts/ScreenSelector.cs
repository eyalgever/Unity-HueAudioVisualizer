using Assets.Scripts.CustomEvents;
using UnityEngine;

namespace Assets.Scripts
{
    public class ScreenSelector : MonoBehaviour, ISwitchScreenHandler
    {
        public GameObject BridgeFinder;
        public GameObject AppRegistration;
        public GameObject EntertainGroupSelection;
        public GameObject Main;

        public void Awake()
        {
            // Bridge finder always loads on start
            SwitchScreen(ScreenType.BridgeFinder);
        }

        public void SwitchScreen(ScreenType screenType)
        {
            Debug.Log($"Switching screen to {screenType}");

            switch (screenType)
            {
                case ScreenType.BridgeFinder:
                    BridgeFinder.SetActive(true);
                    AppRegistration.SetActive(false);
                    EntertainGroupSelection.SetActive(false);
                    Main.SetActive(false);
                    break;
                case ScreenType.AppRegistration:
                    BridgeFinder.SetActive(false);
                    AppRegistration.SetActive(true);
                    EntertainGroupSelection.SetActive(false);
                    Main.SetActive(false);
                    break;
                case ScreenType.EntertainmentGroupSelection:
                    BridgeFinder.SetActive(false);
                    AppRegistration.SetActive(false);
                    EntertainGroupSelection.SetActive(true);
                    Main.SetActive(false);
                    break;
                case ScreenType.Main:
                    BridgeFinder.SetActive(false);
                    AppRegistration.SetActive(false);
                    EntertainGroupSelection.SetActive(false);
                    Main.SetActive(true);
                    break;
            }

            ApplicationState.Instance.ActiveScreen = screenType;
        }
    }
}
