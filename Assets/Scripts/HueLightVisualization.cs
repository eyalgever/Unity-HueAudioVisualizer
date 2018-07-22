using System.Collections.Generic;
using Assets.Scripts.AudioReactiveEffects;
using Assets.Scripts.CustomEvents;
using Assets.Scripts.Models;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class HueLightVisualization : MonoBehaviour
    {
        private HueLightProfile _selectedProfile;

        public bool DoHueVisualization = true;
        public GameObject HueLightPrefab;
        public GameObject EditorSectionGameObject;
        public Vector2 HueLightPositionSpacing = new Vector2(3.0f, 2.0f);
        public int HueSetStateMs = 50;
        public float HueUpdateIntervalSeconds = 0.1f;
        public List<GameObject> LightGameObjects = new List<GameObject>();

        public void Awake()
        {
            ApplicationState.Instance.EntertainmentGroupSelected += entertainmentGroupSelected;
            ApplicationState.Instance.LightProfileSelected += lightProfileSelected;
        }

        public void Start()
        {

        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                bool hit2d = false;

                // Light selection / deselection - 2D Raycast to sprites
                var hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject.transform.parent.gameObject.layer == gameObject.layer)
                {
                    hit2d = true;
                    var reactiveLight = hit.collider.gameObject.transform.parent.gameObject.GetComponent<HueReactiveLight>();

                    if (!reactiveLight.IsSelected)
                    {
                        LightGameObjects.ForEach(l => l.GetComponent<HueReactiveLight>().IsSelected = false);
                        reactiveLight.IsSelected = true;
                        ExecuteEvents.ExecuteHierarchy<ILightSelectedHandler>(EditorSectionGameObject, null, (x, y) => x.SingleSelect(hit.collider.gameObject.transform.parent.gameObject));

                    }
                    else
                    {
                        reactiveLight.IsSelected = false;
                        ExecuteEvents.ExecuteHierarchy<ILightSelectedHandler>(EditorSectionGameObject, null, (x, y) => x.ClearSelection());
                    }
                }

                if (!hit2d)
                {
                    // Deselect zone needs 3D Raycast
                    RaycastHit hitInfo = new RaycastHit();
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) && hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Deselect Zone"))
                    {
                        LightGameObjects.ForEach(l => l.GetComponent<HueReactiveLight>().IsSelected = false);
                        ExecuteEvents.ExecuteHierarchy<ILightSelectedHandler>(EditorSectionGameObject, null, (x, y) => x.ClearSelection());
                    }
                }
            }
        }

        public void OverallBrightnesChanged(float value)
        {
            Debug.Log(value);
            foreach (GameObject lightGameObject in LightGameObjects)
            {
                var reactiveLight = lightGameObject.GetComponent<HueReactiveLight>();
                reactiveLight.OverallBrightnessModifier = value;
            }
        }

        public void SaveLightProfile()
        {
            // save light profile - try to find matching light
            foreach (GameObject lightGameObject in LightGameObjects)
            {
                var reactiveLight = lightGameObject.GetComponent<HueReactiveLight>();
                var lightSetting = _selectedProfile.LightSettings.FirstOrDefault(s => s.LightId == reactiveLight.StreamingLight.Id);

                if (lightSetting != null)
                {
                    // Set properties from profile
                    lightSetting.SpectrumBand = reactiveLight.AudioSampleIndex;
                    lightSetting.MinBrightness = reactiveLight.HueMinBrightness;
                    lightSetting.Reactivity = reactiveLight.HueBrightnessScale;
                    lightSetting.Color = reactiveLight.Color;
                    lightSetting.AutoColor = reactiveLight.DoColorRotate;
                }
            }

            ApplicationState.Instance.SaveProfile();
        }

        private void entertainmentGroupSelected(HueGroupSetting groupSetting)
        {
            // Create light object for every light in the group
            for (int i = 0; i < ApplicationState.Instance.SelectedEntertainmentGroup.Count; i++)
            {
                var streamingLight = ApplicationState.Instance.SelectedEntertainmentGroup[i];

                // Create a gameobject for each hue light
                var newVisualGameObject = Instantiate(HueLightPrefab, transform);
                newVisualGameObject.layer = gameObject.layer;
                
                // Get a reference to th Hue StreamingLight - this will be used to identify each light
                var reactiveLight = newVisualGameObject.GetComponent<HueReactiveLight>();
                reactiveLight.StreamingLight = streamingLight;

                LightGameObjects.Add(newVisualGameObject);

                // Position light based on info from hue bridge
                newVisualGameObject.transform.localPosition = new Vector3((float)streamingLight.LightLocation.X * HueLightPositionSpacing.x,
                    (float)streamingLight.LightLocation.Y * HueLightPositionSpacing.y, 0.0f);

                newVisualGameObject.name = $"{newVisualGameObject.name} - {streamingLight.Id}";

                // Set properties on the Unity representation taken from this behavior
                reactiveLight.DoHueVisualization = DoHueVisualization;
                reactiveLight.HueUpdateIntervalSeconds = HueUpdateIntervalSeconds;
                reactiveLight.HueSetStateMs = HueSetStateMs;
            }
        }

        private void lightProfileSelected(HueLightProfile hueLightProfile)
        {
            _selectedProfile = hueLightProfile;

            // load light profile - try to find matching light
            foreach (GameObject lightGameObject in LightGameObjects)
            {
                var reactiveLight = lightGameObject.GetComponent<HueReactiveLight>();
                var lightSetting = hueLightProfile.LightSettings.FirstOrDefault(s => s.LightId == reactiveLight.StreamingLight.Id);

                if (lightSetting != null)
                {
                    // Set properties from profile
                    reactiveLight.AudioSampleIndex = lightSetting.SpectrumBand;
                    reactiveLight.HueMinBrightness = lightSetting.MinBrightness;
                    reactiveLight.HueBrightnessScale = lightSetting.Reactivity;
                    reactiveLight.Color = lightSetting.Color;
                    reactiveLight.DoColorRotate = lightSetting.AutoColor;
                }
            }
        }
    }
}
