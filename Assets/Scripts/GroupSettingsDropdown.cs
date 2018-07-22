using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class GroupSettingsDropdown : MonoBehaviour
    {
        private TMP_Dropdown _dropdown;

        public void Awake()
        {
            _dropdown = GetComponent<TMP_Dropdown>();
            ApplicationState.Instance.EntertainmentGroupSelected += entertainmentGroupSelected;
        }

        public void Start()
        {
            
        }

        private void entertainmentGroupSelected(HueGroupSetting groupSetting)
        {
            var optionData = new List<TMP_Dropdown.OptionData>();

            foreach (var lightProfile in groupSetting.LightProfiles)
            {
                optionData.Add(new TMP_Dropdown.OptionData
                {
                    text = lightProfile.Name
                });
            }

            _dropdown.ClearOptions();
            _dropdown.AddOptions(optionData);
        }
    }
}
