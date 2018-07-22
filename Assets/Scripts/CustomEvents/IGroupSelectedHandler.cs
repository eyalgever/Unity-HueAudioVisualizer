using UnityEngine.EventSystems;

namespace Assets.Scripts.CustomEvents
{
    public interface ISwitchScreenHandler : IEventSystemHandler
    {
        void SwitchScreen(ScreenType screenType);
    }
}
