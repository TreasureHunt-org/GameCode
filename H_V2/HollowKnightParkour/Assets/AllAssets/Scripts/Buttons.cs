using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Buttons : MonoBehaviour
{
    HealthManager healthManager;
    public CanvasGroup fadePanel;
    public AudioSource AudioSource;
    public AudioClip clip;
    TestingMovement movement;
    private void Start()
    {
        healthManager = FindAnyObjectByType<HealthManager>();
        movement = FindAnyObjectByType<TestingMovement>();
    }
    public void LoadSampleScene()
    {
        StartCoroutine(FadeAndLoadScene("Cinematic"));
        AudioSource.PlayOneShot(clip);
    }
   
    public void TryAgain()
    {
        if (movement.PreviousScene == "SampleScene")
        {
            StartCoroutine(FadeAndLoadScene("SampleScene"));
            healthManager.currentHealth = 5;
            AudioSource.PlayOneShot(clip);
        }
        else if (movement.PreviousScene == "CityOfTears")
        {
            StartCoroutine(FadeAndLoadScene("CityOfTears"));
            healthManager.currentHealth = 5;
            AudioSource.PlayOneShot(clip);
        }
        else if (movement.PreviousScene == "Abyss")
        {

            StartCoroutine(FadeAndLoadScene("Abyss"));
            healthManager.currentHealth = 5;
            AudioSource.PlayOneShot(clip);
            movement.doubleJumpForce = 0;
            movement.isAllowedToSideHit = false;

        }
        else if (movement.PreviousScene == "WhitePalace")
        {
            
            StartCoroutine(FadeAndLoadScene("WhitePalace"));
            healthManager.currentHealth = 5;
            AudioSource.PlayOneShot(clip);
        }

    }
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the editor
#else
        AudioSource.PlayOneShot(clip);
        Application.Quit(); // Quit the game in a built application
#endif
    }
    IEnumerator FadeAndLoadScene(string sceneName)
    {
        if (fadePanel != null)
        {
            fadePanel.gameObject.SetActive(true);
            float duration = 1.5f;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                fadePanel.alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
                yield return null;
            }
        }

        SceneManager.LoadScene(sceneName);
    }
    public void RestartLevel()
    {
        if (movement.currentScene == "SampleScene")
        {
            StartCoroutine(FadeAndLoadScene("SampleScene"));
            healthManager.currentHealth = 5;
            AudioSource.PlayOneShot(clip);
        }
        else if (movement.currentScene == "CityOfTears")
        {
            StartCoroutine(FadeAndLoadScene("CityOfTears"));
            healthManager.currentHealth = 5;
            AudioSource.PlayOneShot(clip);
        }
        else if (movement.currentScene == "Abyss")
        {

            StartCoroutine(FadeAndLoadScene("Abyss"));
            healthManager.currentHealth = 5;
            AudioSource.PlayOneShot(clip);
            movement.doubleJumpForce = 0;
            movement.isAllowedToSideHit = false;

        }
        else if (movement.currentScene == "WhitePalace")
        {
            StartCoroutine(FadeAndLoadScene("WhitePalace"));
            healthManager.currentHealth = 5;
            AudioSource.PlayOneShot(clip);
        }
    }
    public void RestartGame()
    {
        StartCoroutine(FadeAndLoadScene("CityOfTears"));
        healthManager.currentHealth = 5;
        AudioSource.PlayOneShot(clip);
        movement.doubleJumpForce = 0;
        movement.isAllowedToSideHit = false;
    }
}
