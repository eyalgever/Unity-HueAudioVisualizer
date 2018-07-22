using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.CustomEvents
{
    public interface ILightSelectedHandler : IEventSystemHandler
    {
        void SingleSelect(GameObject lightGameObject);
        void ClearSelection();
    }
}
