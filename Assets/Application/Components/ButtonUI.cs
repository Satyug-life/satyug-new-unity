using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] Image[] images;
    [SerializeField] Sprite selectedSprite;
    [SerializeField] Sprite unselectedSprite;

    [SerializeField] AudioClip mouseClick;

    [SerializeField] UnityEvent onClickEvent;

    private void Start()
    {
        foreach (Image image in images) { image.sprite = unselectedSprite; }
    }

    public void SelectImage(int index)
    {
        AudioSource.PlayClipAtPoint(mouseClick,Camera.main.transform.position);

        for (int i = 0; i < images.Length; i++)
        {
            images[i].sprite = unselectedSprite;
            if(i == index) images[index].sprite = selectedSprite;
        }      
        
        onClickEvent?.Invoke();
    }
}