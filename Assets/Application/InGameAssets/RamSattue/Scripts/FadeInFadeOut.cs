using System.Collections;
using UnityEngine;

public class FadeInFadeOut : MonoBehaviour
{
    [SerializeField] CanvasGroup m_CanvasGroup;
    float lerpTime;
    private void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
        lerpTime = 0.9f;
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {
        while(m_CanvasGroup.alpha < 0.9f)
        {
            m_CanvasGroup.alpha = Mathf.Lerp(m_CanvasGroup.alpha,1,lerpTime * Time.deltaTime);
            yield return null;
        }

        m_CanvasGroup.alpha = 1;
    }
}
