using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Interactible : MonoBehaviour
{
    public Accessories accesory;
    public Material material;
    private MeshRenderer m_MeshRenderer;
    public string ID;
    [SerializeField] UnityEvent disableGlowEvent;
    private void Awake()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();
    }

    public void Interaction()
    {
        if (!StatueManager.StatueManagerInstance.isGameComplete)
        {

            if (StatueManager.StatueManagerInstance.GetSelectedAccessoryName() == accesory)
            {
                StatueManager.StatueManagerInstance.onSuccessfullyPlaced?.Invoke();
                StatueManager.StatueManagerInstance.isGameComplete = true;
                //StartCoroutine(startProcess());
            }
            else
            {
                StatueManager.StatueManagerInstance.onFailurefullyPlaced?.Invoke();
                StatueManager.StatueManagerInstance.CheckAttempts();
            }
        }
    }

    private WaitForSeconds m_waitForSpawnElements = new WaitForSeconds(1f);

    IEnumerator startProcess()
    {
        StatueManager.StatueManagerInstance.isGameComplete = true;
        yield return m_waitForSpawnElements;
        StatueManager.StatueManagerInstance.onCompleteEvent?.Invoke();
    }

    internal void StartBuilding()
    {
        m_MeshRenderer.material = material;
        this.GetComponent<DissolveController>().enabled = true;
    }
}
