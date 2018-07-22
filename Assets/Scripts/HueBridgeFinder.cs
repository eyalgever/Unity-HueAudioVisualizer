using System;
using System.Collections;
using System.Linq;
using System.Net;
using Assets.Scripts.CustomEvents;
using Q42.HueApi;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models.Bridge;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class HueBridgeFinder : MonoBehaviour
    {
        public int PrefindSeconds = 1;
        public int TimeoutSeconds = 5;
        public GameObject SearchingGameObject;
        public GameObject RetryGameObject;

        public void Start()
        {
            if (ApplicationState.Instance.ActiveScreen == ScreenType.BridgeFinder)
            {
                StartCoroutine(searchForBridge());
            }
        }

        public void SearchForBridge()
        {
            StartCoroutine(searchForBridge());
        }

        private IEnumerator searchForBridge()
        {
            SearchingGameObject.SetActive(true);
            RetryGameObject.SetActive(false);

            Debug.Log("Searching for bridge...");

            yield return new WaitForSeconds(PrefindSeconds);

            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;

            LocatedBridge bridge = null;

            try
            {
                IBridgeLocator locator = new HttpBridgeLocator();
                var bridges = locator.LocateBridgesAsync(TimeSpan.FromSeconds(TimeoutSeconds)).Result;
                bridge = bridges.FirstOrDefault();
            }
            catch
            {
                // Swallow exception
            }

            Debug.Log(bridge == null ? "No bridge found" : $"Bridge found: {bridge.IpAddress}");

            if (bridge != null)
            {
                // Bridge found, move to app registration
                if (!string.IsNullOrEmpty(bridge.IpAddress))
                {
                    // Set IP in application state
                    ApplicationState.Instance.HueBridgeIp = bridge.IpAddress;

                    // Move to app registration screen
                    ExecuteEvents.ExecuteHierarchy<ISwitchScreenHandler>(gameObject, null,
                        (x, y) => x.SwitchScreen(ScreenType.AppRegistration));
                }
            }
            else
            {
                // Show retry UI
                SearchingGameObject.SetActive(false);
                RetryGameObject.SetActive(true);
            }
        }
    }
}
