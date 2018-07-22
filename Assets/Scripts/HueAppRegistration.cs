using System;
using Assets.Scripts.CustomEvents;
using Q42.HueApi;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class HueAppRegistration : MonoBehaviour
    {
        public string ApplicationName = "HueRealtimeAudio";
        public string DeviceName = "WindowsPC";
        public float TryRegisterIntervalSeconds = 1.0f;

        public void Start()
        {
            if (ApplicationState.Instance.ActiveScreen == ScreenType.AppRegistration)
            {
                if (string.IsNullOrEmpty(ApplicationState.Instance.Profile.HueKey) ||
                    string.IsNullOrEmpty(ApplicationState.Instance.Profile.HueStreamingKey))
                {
                    // We're missing keys - do registration
                    InvokeRepeating("TryRegister", 0.0f, TryRegisterIntervalSeconds);
                }
                else
                {
                    // We have keys - already registered - move to entertainment group selection
                    Debug.Log("App already registered. Moving to entertainment group selection.");

                    ExecuteEvents.ExecuteHierarchy<ISwitchScreenHandler>(gameObject, null, (x, y) => x.SwitchScreen(ScreenType.EntertainmentGroupSelection));
                }
            }
        }

        public void TryRegister()
        {
            Debug.Log("Trying to register");

            try
            {
                var result = LocalHueClient.RegisterAsync(ApplicationState.Instance.HueBridgeIp, ApplicationName, DeviceName, true).Result;

                ApplicationState.Instance.Profile.HueKey = result.Username;
                ApplicationState.Instance.Profile.HueStreamingKey = result.StreamingClientKey;

                CancelInvoke("TryRegister");

                // Save registration keys to profile
                ApplicationState.Instance.SaveProfile();

                Debug.Log($"Registered: user - {result.Username}, streaming key - {result.StreamingClientKey}");

                // Move to entertainment group selection
                ExecuteEvents.ExecuteHierarchy<ISwitchScreenHandler>(gameObject, null, (x, y) => x.SwitchScreen(ScreenType.EntertainmentGroupSelection));
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }
    }
}
