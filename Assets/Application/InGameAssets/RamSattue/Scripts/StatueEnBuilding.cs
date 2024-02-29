using UnityEngine;

public class StatueEnBuilding : MonoBehaviour
{
    [SerializeField] private GameObject[] statueParts;
    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        foreach (GameObject part in statueParts) 
        {
            part.SetActive(false);
        }
    }

    public void ShowAccessories()
    {
        foreach (GameObject part in statueParts)
        {
            part.SetActive(true);
        }
    }

    public void SetGlowOn()
    {
        audioSource.Play();
        for (int i = 0; i < statueParts.Length; i++)
        {
            if (statueParts[i].TryGetComponent(out Interactible interactible))
            {
                if(interactible.accesory == StatueManager.StatueManagerInstance.GetSelectedAccessoryName())
                {
                    interactible.GetComponent<Animator>().SetBool("Glow", true);
                }
            }
        }
    }

    public void SetGlowOff()
    {
        audioSource.Stop();

        foreach (var parts in statueParts)
        {
            Animator animator = parts.GetComponent<Animator>();
            if (animator.GetBool("Glow"))
                animator.SetBool("Glow", false);

            if (animator.GetBool("Fade"))
                animator.SetBool("Fade", false);
        }
    }

    public void AfterAttemptComplete()
    {
        audioSource.Play();

        for (int i = 0; i < statueParts.Length; i++)
        {
            if (statueParts[i].TryGetComponent(out Interactible interactible))
            {
                if (interactible.accesory == StatueManager.StatueManagerInstance.GetSelectedAccessoryName())
                {
                    interactible.GetComponent<Animator>().SetBool("Glow", true);
                }
                else
                {
                    interactible.GetComponent<Animator>().SetBool("Fade", true);
                }
            }
        }
    }
}
