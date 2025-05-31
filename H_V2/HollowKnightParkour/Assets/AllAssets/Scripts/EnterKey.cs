using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class EnterKey : MonoBehaviour
{
    public float moveDistance = -2f;  
    public float rotationAngle = 90f; 
    public float duration = 10f; 
    public float returnDelay = 2f; 

    private bool isMoving = false; 
    private Vector3 startPosition;
    private Quaternion startRotation;
    public GameManager manager;
    public AudioSource audio;
    public AudioClip clip;

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }
    public void OnEnterPressed()
    {
        string currentWord = Challenge.Instance.GetWord().ToLower(); // Convert to lowercase for case-insensitivity
        Debug.Log("Entered Word: " + currentWord);

        if (currentWord.Length == 3 || currentWord.Length == 5 || currentWord.Length == 7)
        {
            if (Challenge.Instance.IsPalindrome() && realPalindromes.Contains(currentWord))
            {
                Debug.Log("Correct! " + currentWord + " is a real palindrome.");
                Challenge.Instance.SaveCorrectWord();  // Save the word as correct
                GameManager.Instance.NextRound();  // Move to the next round
                Challenge.Instance.UpdateFormingWordText();  // Update the UI text
            }
            else if (!realPalindromes.Contains(currentWord))
            {
                Debug.Log("Incorrect! " + currentWord + " is not a real word.");
                Challenge.Instance.ResetWord();  // Make the player re-enter
                manager.currentTimer = 0;
            }
            else
            {
                manager.currentTimer = 0;
                Debug.Log("Incorrect! " + currentWord + " is not a palindrome.");
                Challenge.Instance.ResetWord();  // Make the player re-enter
            }
        }
        else
        {
            Debug.Log("Invalid word length! Please form a " + GameManager.Instance.GetCurrentTargetLength() + "-letter palindrome.");
            Challenge.Instance.ResetWord();  // Make the player re-enter
            manager.currentTimer = 0;
        }
    }

    private List<string> realPalindromes = new List<string>()
{
    "madam", "racecar", "level", "radar", "refer", "deified", "civic", "rotor", "kayak", "noon",
    "bob", "dad", "did", "eve", "gig", "gag", "mam", "nan", "nun", "pep", "pip", "pop", "tat", "tot", "wow",
    "dewed", "sagas", "solos", "tenet", "stats", "revive", "minim", "repaper", "kajak", "reifier",
    "redder", "redivider", "detartrated", "reviver", "rotator", "detartrated", "solosol", "racecar", "redivider",
    "repaper", "revive", "redder", "reviver", "rotator", "rotator", "tenetot", "wowowow", "sagas", "tacit", "mom"
};


    public void RotateStick()
    {
        if (!isMoving) // Only trigger if it's not already moving
        {

            StartCoroutine(RotateAndMove());
            OnEnterPressed();
        }
    }

    IEnumerator RotateAndMove()
    {
        audio.PlayOneShot(clip);
        isMoving = true; // Prevent multiple hits

        Vector3 targetPos = startPosition + new Vector3(-moveDistance, 0, 0); // Move 2 units on X-axis
        Quaternion targetRotation = Quaternion.Euler(0, 0, -rotationAngle); // Rotate on Z

        // Start moving the door at the same time as the stick


        // Move stick to target position
        yield return MoveAndRotate(startPosition, targetPos, startRotation, targetRotation, duration);

        // Wait before returning
        yield return new WaitForSeconds(returnDelay);



        // Move stick back to original position
        yield return MoveAndRotate(targetPos, startPosition, targetRotation, startRotation, duration);

        isMoving = false; // Allow the player to hit it again
    }

    IEnumerator MoveAndRotate(Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            float t = elapsedTime / time; // Normalize time (0 to 1)
            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.rotation = Quaternion.Lerp(startRot, endRot, t);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        // Ensure final values are set correctly
        transform.position = endPos;
        transform.rotation = endRot;
    }

}
