using UnityEngine;
using System.Collections;

public class ArrowTrigger : MonoBehaviour
{
    public GameObject[] arrows; // Assign arrow GameObjects in the Inspector
    public Transform[] floors; // Assign the two floor objects here
    public float shakeDuration = 1f; // Duration of the earthquake effect
    public float shakeStrength = 0.1f; // How much the floors shake
    public string playerTag = "Player"; // The tag of the player

    private void Start()
    {
        // Make sure all arrows start as inactive
        foreach (GameObject arrow in arrows)
        {
            arrow.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            // Activate all arrows
            foreach (GameObject arrow in arrows)
            {
                arrow.SetActive(true);
                arrow.GetComponent<AbyssArrowsGoUp>().ActivateArrow(); // Call the ActivateArrow method for each arrow
            }

            // Start shaking floors
            foreach (Transform floor in floors)
            {
                StartCoroutine(ShakeFloor(floor));
            }
        }
    }

    IEnumerator ShakeFloor(Transform floor)
    {
        Vector3 originalPos = floor.position;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float offsetX = Random.Range(-shakeStrength, shakeStrength);
            float offsetY = Random.Range(-shakeStrength, shakeStrength);

            floor.position = originalPos + new Vector3(offsetX, offsetY, 0);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        floor.position = originalPos; // Reset position after shaking ends
    }
}
