using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Models;
using Newtonsoft.Json;
using Q42.HueApi.Models.Groups;
using Q42.HueApi.Streaming;
using Q42.HueApi.Streaming.Models;
using UnityEngine;

namespace Assets.Scripts
{
    public class ApplicationState
    {
        public static readonly ApplicationState Instance = new ApplicationState();

        private const string ProfileFilename = "HueVisuzalizationProfile.json";

        private bool _isConnected;

        public HueProfile Profile { get; }
        public ScreenType ActiveScreen { get; set; }
        public string HueBridgeIp { get; set; }
        public StreamingHueClient Client { get; private set; }
        public List<Group> AvailableEntertainmentGroups { get; private set; }
        public StreamingGroup SelectedEntertainmentGroup { get; private set; }
        public HueGroupSetting SelectedGroupSetting { get; private set; }
        public HueLightProfile SelectedLightProfile { get; private set; }
        public Action<HueGroupSetting> EntertainmentGroupSelected { get; set; }
        public Action<HueLightProfile> LightProfileSelected { get; set; }

        private ApplicationState()
        {
            // Load saved data
            if (File.Exists(ProfileFilename))
            {
                Profile = JsonConvert.DeserializeObject<HueProfile>(File.ReadAllText(ProfileFilename));
            }
            else
            {
                Profile = HueProfile.GetDefaultProfile();
                SaveProfile();
            }
        }

        public void InitializeClient()
        {
            // Initialize streaming client
            Client = new StreamingHueClient(HueBridgeIp, Profile.HueKey, Profile.HueStreamingKey);

            // Get the entertainment group
            var all = Client.LocalHueClient.GetBridgeAsync().Result;
            AvailableEntertainmentGroups = all.Groups.Where(x => x.Type == GroupType.Entertainment).ToList();
        }

        public async Task InitializeEntertainmentGroup(Group group, int autoUpdateFrequency)
        {
            // Create a streaming group
            SelectedEntertainmentGroup = new StreamingGroup(group.Locations);
            SelectedEntertainmentGroup.IsForSimulator = true;

            bool doProfileSave = false;

            // If there are no group settings, initialize list
            if (Profile.GroupSettings == null)
            {
                Profile.GroupSettings = new List<HueGroupSetting>();
                doProfileSave = true;
            }

            // If there's no profile for this group, grab a default
            if (Profile.GroupSettings.FirstOrDefault(g => g.GroupName == group.Name) == null)
            {
                Profile.GroupSettings.Add(HueGroupSetting.GetDefault(group.Id, group.Name));
                doProfileSave = true;
            }

            if(doProfileSave)
            {
                SaveProfile();
            }

            // Connect to the streaming group
            await connectToHueBridge(group.Id);

            _isConnected = true;

            // Start auto updating this entertainment group
            Client.AutoUpdate(SelectedEntertainmentGroup, autoUpdateFrequency);

            // Send an event indicating group has been selected
            SelectedGroupSetting = Profile.GroupSettings.First(g => g.GroupId == group.Id);

            // Default to picking first light profile
            SelectedLightProfile = SelectedGroupSetting.LightProfiles[0];
        }

        public void FireSelectionNotifications()
        {
            EntertainmentGroupSelected?.Invoke(SelectedGroupSetting);
            LightProfileSelected?.Invoke(SelectedLightProfile);
        }

        public void Shutdown()
        {
            if (_isConnected)
            {
                Client?.Close();
            }

            _isConnected = false;
        }

        public void SaveProfile()
        {
            File.WriteAllText(ProfileFilename, JsonConvert.SerializeObject(Profile, Formatting.Indented));
        }

        private async Task connectToHueBridge(string groupId)
        {
            await Client.Connect(groupId);

            Debug.Log("Connected.");

            var bridgeInfo = Client.LocalHueClient.GetBridgeAsync().Result;
            Debug.Log(bridgeInfo.IsStreamingActive ? "Streaming is active." : "Streaming is not active.");
        }
    }
}
