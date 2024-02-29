using UnityEngine;
using UnityEngine.UI;

namespace Satyug.HardMode
{

    [RequireComponent(typeof(Button))]
    public class TabData : MonoBehaviour
    {
        public string id;
        public RawImage rawImage;
        private Button btn;
        
        private void Awake()
        {
            btn = GetComponent<Button>();

            btn.onClick.AddListener(() => 
            {
                StatueManager.StatueManagerInstance.collectionID = id; 
            });
        }
    }
}
