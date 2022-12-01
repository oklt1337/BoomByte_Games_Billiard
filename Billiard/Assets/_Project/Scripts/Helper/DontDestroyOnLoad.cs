using UnityEngine;

namespace _Project.Scripts.Helper
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}
