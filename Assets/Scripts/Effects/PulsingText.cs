using TMPro;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    public class PulsingText : MonoBehaviour
    {
        private TextMeshProUGUI _textMeshPro;

        public float PulseSpeed = 2.0f;

        public void Start()
        {
            _textMeshPro = GetComponent<TextMeshProUGUI>();
        }

        public void Update()
        {
            var color = (Color)_textMeshPro.faceColor;
            color.a = Mathf.PingPong(Time.time * PulseSpeed, 1.0f);
            _textMeshPro.faceColor = color;
        }
    }
}
