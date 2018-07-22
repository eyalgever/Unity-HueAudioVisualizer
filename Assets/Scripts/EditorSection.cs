using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.AudioReactiveEffects;
using Assets.Scripts.CustomEvents;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class EditorSection : MonoBehaviour, ILightSelectedHandler
    {
        private List<ObjectScaleReactiveEffect> _spectrumBars;
        private SpectrumPicker _spectrumPicker;

        public GameObject HueVisualizationGameObject;
        public GameObject PickerGameObject;
        public GameObject SelectLightGameObject;
        public GameObject EditorGameObject;
        public GameObject MinBrightnessSliderGameObject;
        public GameObject ReactivitySliderGameObject;
        public GameObject BarSpectrumGameObject;
        public GameObject SelectedLight { get; private set; }

        public void Start()
        {
            if (ApplicationState.Instance.ActiveScreen == ScreenType.Main)
            {
                _spectrumPicker = PickerGameObject.GetComponent<SpectrumPicker>();
                setEditorPanel();
            }
        }

        public void Update()
        {
            if (_spectrumBars == null)
            {
                _spectrumBars = BarSpectrumGameObject.GetComponentsInChildren<ObjectScaleReactiveEffect>().ToList();
            }
        }

        public void MinBrightnessChanged(float value)
        {
            SelectedLight.GetComponent<HueReactiveLight>().HueMinBrightness = value;
        }

        public void ReactivityChanged(float value)
        {
            SelectedLight.GetComponent<HueReactiveLight>().HueBrightnessScale = value;
        }

        public void SingleSelect(GameObject lightGameObject)
        {
            SelectedLight = lightGameObject;
            MinBrightnessSliderGameObject.GetComponent<Slider>().value = SelectedLight.GetComponent<HueReactiveLight>().HueMinBrightness;
            ReactivitySliderGameObject.GetComponent<Slider>().value = SelectedLight.GetComponent<HueReactiveLight>().HueBrightnessScale;
            var associatedSpectrumBar = _spectrumBars.First(bar => bar.AudioSampleIndex == SelectedLight.GetComponent<HueReactiveLight>().AudioSampleIndex);
            _spectrumPicker.SelectedLightGameObject = lightGameObject;
            _spectrumPicker.SetSelectedSpectrumIndex(associatedSpectrumBar.gameObject);
            setEditorPanel();
        }

        public void ClearSelection()
        {
            SelectedLight = null;
            setEditorPanel();
        }

        private void setEditorPanel()
        {
            if (SelectedLight == null)
            {
                SelectLightGameObject.SetActive(true);
                EditorGameObject.SetActive(false);
            }
            else
            {
                SelectLightGameObject.SetActive(false);
                EditorGameObject.SetActive(true);
            }
        }
    }
}
