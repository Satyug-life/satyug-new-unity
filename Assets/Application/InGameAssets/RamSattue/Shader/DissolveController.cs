using UnityEngine;

public class DissolveController : MonoBehaviour
{
    public float dissolveDuration = 30f;
    public float targetDissolveValue = 0f;

    public Material dissolveMaterial;
    public bool isDissolving = false;
    private bool hasStarted = false; // New variable to track if dissolve has started
    private float dissolveStartTime;
    private float initialDissolveValue;
    
    //variable used for check is active by user or not
    
    void Start()
    {
        // Get the material with the dissolve shader attached
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            dissolveMaterial = renderer.material;
            if (dissolveMaterial.HasProperty("_Dissolve"))
            {
                initialDissolveValue = dissolveMaterial.GetFloat("_Dissolve");
            }
        }
        StartDissolve();

    }

    void Update()
    {
        // Check if we need to start the dissolve process
        if (isDissolving && hasStarted)
        {
            float timeSinceStart = Time.time - dissolveStartTime;
            float t = timeSinceStart / dissolveDuration;

            // Calculate the new dissolve value based on the lerp
            float newDissolveValue = Mathf.Lerp(initialDissolveValue, targetDissolveValue, t);

            // Apply the new dissolve value to the material
            dissolveMaterial.SetFloat("_Dissolve", newDissolveValue);

            // Check if the dissolve process is complete
            if (t >= 1f)
            {
                isDissolving = false;
            }
        }
    }


    // Call this method to start the dissolve process
    public void StartDissolve()
    {
        dissolveStartTime = Time.time;
        isDissolving = true;
        hasStarted = true;
    }

    // Call this method to stop the dissolve process
    public void StopDissolve()
    {
        hasStarted = false;
    }
}
