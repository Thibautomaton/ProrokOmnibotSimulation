using UnityEngine;

namespace Ressources.Scripts
{
    public class UI_Manager : MonoBehaviour
    {
        public void ChangeState()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(!child.gameObject.activeSelf);
            }
        }
    }
}