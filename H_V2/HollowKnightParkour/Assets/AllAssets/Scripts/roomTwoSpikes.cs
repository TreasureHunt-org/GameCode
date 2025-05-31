using Cinemachine;
using System.Collections;
using UnityEngine;

public class roomTwoSpikes : MonoBehaviour
{
    public GameObject[] arrows;
    public float delayBetweenArrows = 0.3f; 
    public float arrowSpeed = 10f; 
    public AudioSource audio;
    public AudioClip clip;
    private bool hasActivated = false; 
    private Transform player;
    public CinemachineVirtualCamera virtualCamera; 
    public Transform cameraTargetPosition; 
    public float cameraFollowSpeed = 2f; 
    public Collider2D collider;
    public Collider2D collider2;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasActivated) 
        {
            hasActivated = true; // Mark as activated
            StartCoroutine(ActivateArrowsOneByOne());
        }
    }
    IEnumerator ActivateArrowsOneByOne()
    {
        // Start the arrow falling sequence
        audio.PlayOneShot(clip); // Play sound once when triggered
        StartCoroutine(FollowTargetPositionWhileShooting()); // Start following target position while shooting arrows

        foreach (GameObject arrow in arrows)
        {
            audio.PlayOneShot(clip);
            arrow.SetActive(true); // Activate the arrow
            StartCoroutine(FallTowardLockedPosition(arrow)); // Make it fall toward locked position

            yield return new WaitForSeconds(delayBetweenArrows); // Wait before next arrow drops
        }

        // After all arrows are done, return the camera to follow the player again
        yield return new WaitForSeconds(0.5f); // Small delay after arrows finish
        virtualCamera.Follow = player; // Re-enable camera follow
        collider.enabled = false;
        collider2.enabled = false;
        GetComponent<Collider2D>().enabled = false; // Disable trigger collider after activation
    }

    // Coroutine to smoothly make the camera follow the target position (while arrows are falling)
    IEnumerator FollowTargetPositionWhileShooting()
    {
        virtualCamera.Follow = cameraTargetPosition; // Camera starts following the target position
        collider.enabled = true;
        collider2.enabled = true;
        while (hasActivated && AnyArrowsActive())
        {
            // Smoothly follow the target position
            virtualCamera.transform.position = Vector3.Lerp(virtualCamera.transform.position,
                cameraTargetPosition.position, cameraFollowSpeed * Time.deltaTime);

            yield return null; // Wait until the next frame
        }

        virtualCamera.transform.position = cameraTargetPosition.position; // Ensure the camera is at the target position
    }

    // Check if any arrows are still active (falling)
    bool AnyArrowsActive()
    {
        foreach (GameObject arrow in arrows)
        {
            if (arrow.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator FallTowardLockedPosition(GameObject arrow)
    {
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();

        if (rb != null && player != null)
        {
            Vector2 lockedTarget = player.position; // Capture player's position at the moment of arrow spawn
            Vector2 direction = (lockedTarget - (Vector2)arrow.transform.position).normalized; // Get direction

            // Move the arrow
            rb.velocity = direction * arrowSpeed;

            // **Rotate the arrow to face the player properly**
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        yield return null;
    }


}
