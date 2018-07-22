using System;
using Assets.Scripts.AudioReactiveEffects.Base;
using Assets.Scripts.Effects;
using Assets.Scripts.Extensions;
using Q42.HueApi.Streaming.Models;
using Q42.HueApi.Streaming.Extensions;
using UnityEngine;

namespace Assets.Scripts.AudioReactiveEffects
{
    public class HueReactiveLight : VisualizationEffectBase
    {
        private SpriteRenderer _spriteRenderer;
        private ColorRotator _colorRotator;

        public StreamingLight StreamingLight { get; set; }
        public bool IsSelected;
        public bool DoHueVisualization;
        public bool DoColorRotate;
        public GameObject SelectionGameObject;
        public Color UnityBaseColor = Color.grey;
        public Color Color;
        public float UnityIntensityModifier = 2.0f;
        public float UnityEmissionModifier = 2.0f;
        public float HueMinBrightness;
        public float HueBrightnessScale;
        public float HueUpdateIntervalSeconds;
        public float OverallBrightnessModifier;
	    public int HueSetStateMs;

        public override void Start()
        {
            base.Start();

            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _colorRotator = new ColorRotator();

            InvokeRepeating("UpdateHueLight", 0.0f, HueUpdateIntervalSeconds);
        }

        public void Update()
        {
            if (DoColorRotate)
            {
                _colorRotator.Rotate();
                Color = _colorRotator.Color;
            }

            float audioData = GetAudioData();
            float intensityAmount = HueMinBrightness + audioData * HueBrightnessScale * UnityIntensityModifier * OverallBrightnessModifier;
            var color = Color.Lerp(UnityBaseColor, Color, intensityAmount);

            _spriteRenderer.color = color;
        }

	    public void UpdateHueLight()
        {
            if (DoHueVisualization)
            {
                float audioData = GetAudioData();
                float brightnessAmount = HueMinBrightness + audioData * HueBrightnessScale * OverallBrightnessModifier;
                StreamingLight.SetState(Color.ToHueColor(), brightnessAmount, TimeSpan.FromMilliseconds(HueSetStateMs));
            }

            SelectionGameObject.SetActive(IsSelected);
        }
    }
}