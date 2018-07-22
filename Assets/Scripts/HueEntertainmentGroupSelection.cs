using Assets.Scripts.CustomEvents;
using Q42.HueApi.Models.Groups;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class HueEntertainmentGroupSelection : MonoBehaviour, IGroupSelectedHandler
    {
        public GameObject EntertainmentGroupContainer;
        public GameObject EntertainmentGroupPrefab;
        public GameObject RetryGameObject;
        public float VerticalSpacing = 10.0f;
        public int HueAutoUpdateFrequency = 50;

        public void Awake()
        {
            EntertainmentGroupContainer.SetActive(false);
            RetryGameObject.SetActive(false);
        }

        public void Start()
        {
            if (ApplicationState.Instance.ActiveScreen == ScreenType.EntertainmentGroupSelection)
            {
                GetEntertainmentGroups();
            }
        }

        public void GetEntertainmentGroups()
        {
            EntertainmentGroupContainer.SetActive(false);
            RetryGameObject.SetActive(false);

            // Initializing the client will make Entertainment Groups available
            ApplicationState.Instance.InitializeClient();

            if (ApplicationState.Instance.AvailableEntertainmentGroups != null &&
                ApplicationState.Instance.AvailableEntertainmentGroups.Count > 0)
            {
                EntertainmentGroupContainer.SetActive(true);

                float yOffset = 0.0f;

                // Create a button for each group
                foreach (Group group in ApplicationState.Instance.AvailableEntertainmentGroups)
                {
                    var newGameObject = Instantiate(EntertainmentGroupPrefab, EntertainmentGroupContainer.transform);
                    newGameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, yOffset);
                    newGameObject.GetComponent<HueEntertainmentGroupSelector>().Group = group;
                    newGameObject.name = $"{newGameObject.name} ({group.Name})";
                    newGameObject.GetComponentInChildren<TextMeshProUGUI>().SetText(group.Name);

                    yOffset -= VerticalSpacing + newGameObject.GetComponent<RectTransform>().sizeDelta.y;
                }
            }
            else
            {
                RetryGameObject.SetActive(true);
            }
        }

        public async void SelectGroup(Group group)
        {
            Debug.Log($"Receiving event - group selected: {group.Name} ({group.Id}). Initializing entertainment group...");
            // Group selected, start group and navigate to main screen
            await ApplicationState.Instance.InitializeEntertainmentGroup(group, HueAutoUpdateFrequency);
            ExecuteEvents.ExecuteHierarchy<ISwitchScreenHandler>(gameObject, null, (x, y) => x.SwitchScreen(ScreenType.Main));

            ApplicationState.Instance.FireSelectionNotifications();
        }
    }
}
