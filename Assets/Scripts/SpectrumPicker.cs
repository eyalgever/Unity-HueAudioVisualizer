using Assets.Scripts.AudioReactiveEffects;
using Assets.Scripts.AudioReactiveEffects.Base;
using UnityEngine;

namespace Assets.Scripts
{
    public class SpectrumPicker : MonoBehaviour
    {
        private bool _isMouseDown;

        public int SelectedSpectrumIndex { get; private set; }

        public GameObject InvisibleGripGameObject;
        public GameObject SelectedLightGameObject;

        public void Start()
        {

        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hitInfo = new RaycastHit();
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) && hitInfo.collider.gameObject == gameObject)
                {
                    _isMouseDown = true;
                    InvisibleGripGameObject.transform.position = gameObject.transform.position;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isMouseDown = false;
            }

            if (_isMouseDown)
            {
                InvisibleGripGameObject.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, gameObject.transform.position.y, gameObject.transform.position.z);

                // Find the spectrum bar under the picker
                RaycastHit hitInfo = new RaycastHit();
                if (Physics.Raycast(new Ray(InvisibleGripGameObject.transform.position, Vector3.down), out hitInfo) && hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Spectrum Bars"))
                {
                    var barSpectrum = hitInfo.transform.gameObject.GetComponentInParent<VisualizationEffectBase>();
                    SelectedSpectrumIndex = barSpectrum.AudioSampleIndex;

                    // Snap to the bar spectrum x
                    gameObject.transform.position = new Vector3(barSpectrum.gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);

                    // Set the actual value
                    SelectedLightGameObject.GetComponent<HueReactiveLight>().AudioSampleIndex = SelectedSpectrumIndex;
                }
            }
        }

        public void SetSelectedSpectrumIndex(GameObject barSpectrum)
        {
            gameObject.transform.position = new Vector3(barSpectrum.gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }
}
