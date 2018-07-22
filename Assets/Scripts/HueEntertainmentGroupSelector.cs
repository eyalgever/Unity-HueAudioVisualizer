using Assets.Scripts.CustomEvents;
using Q42.HueApi.Models.Groups;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class HueEntertainmentGroupSelector : MonoBehaviour
    {
        public Group Group { get; set; }

        public void SelectGroup()
        {
            Debug.Log($"{Group.Name} selected. Firing event");
            ExecuteEvents.ExecuteHierarchy<IGroupSelectedHandler>(gameObject, null, (x, y) => x.SelectGroup(Group));
        }
    }
}
