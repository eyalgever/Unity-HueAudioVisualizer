using Q42.HueApi.Models.Groups;
using UnityEngine.EventSystems;

namespace Assets.Scripts.CustomEvents
{
    public interface IGroupSelectedHandler : IEventSystemHandler
    {
        void SelectGroup(Group group);
    }
}
