using System.Collections;
using UnityEngine;

public class FloorCollapse : MonoBehaviour
{
    public GameObject floor;        
    public ParticleSystem dustParticles; 
    public float shakeDuration = 1.5f;  
    public float shakeMagnitude = 0.1f; 
    public float collapseDelay = 3f;    

    private Vector3 originalPosition;   

    void Start()
    {
        originalPosition = floor.transform.position;
    }

    public void StartCollapseSequence()
    {
        StartCoroutine(WarningShake());
    }

    private IEnumerator WarningShake()
    {
        // Play particles
        if (dustParticles != null)
        {
            dustParticles.Play();
        }

        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            // Shake the floor slightly up & down
            Vector3 randomShake = new Vector3(Random.Range(-shakeMagnitude, shakeMagnitude), Random.Range(-shakeMagnitude, shakeMagnitude), 0);
            floor.transform.position = originalPosition + randomShake;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset floor position
        floor.transform.position = originalPosition;

        // Wait before collapsing the floor
        yield return new WaitForSeconds(collapseDelay);

        // Collapse the floor
        CollapseFloor();
    }

    private void CollapseFloor()
    {

        Rigidbody2D rb = floor.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic; // Make the floor fall
        }

        // Stop particles
        if (dustParticles != null)
        {
            dustParticles.Stop();
        }
    }
}
