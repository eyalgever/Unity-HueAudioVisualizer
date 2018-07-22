using UnityEngine;


namespace Assets.Scripts
{
    public class Shutdown : MonoBehaviour
    {
        public void OnApplicationQuit()
        {
            ApplicationState.Instance.Shutdown();
            Debug.Log("OnApplicationQuit");
        }
    }
}
