using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int currentRound = 1;
    private int[] targetLengths = { 3, 5, 7 };

   
    public TextMeshProUGUI timerText;  

    public float duration = 10f;
    public GameObject door; 
    public Vector3 doorOpenPosition;
    private Vector3 doorClosedPosition; 

    public AudioSource audio;
    public AudioClip clip;
    public float currentTimer;
    private Coroutine timerCoroutine;  
    public ChallengeDialougeManager challengeDialougeManager;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (door != null)
        {
            doorClosedPosition = door.transform.position;
            challengeDialougeManager.image11.gameObject.SetActive(false);
            challengeDialougeManager.image12.gameObject.SetActive(false);
            challengeDialougeManager.image13.gameObject.SetActive(false);
            challengeDialougeManager.t1.gameObject.SetActive(false);
            challengeDialougeManager.t2.gameObject.SetActive(false);
        }

        //StartRound();  
    }

   
    public void StartRound()
    {
        SetTimerForRound();  
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);  
        }
        timerCoroutine = StartCoroutine(RoundTimer());  
    }

   
    public void UpdateRoundText()
    {
        //roundText.text = "Round " + currentRound;
    }

    
    public int GetCurrentTargetLength()
    {
        return targetLengths[currentRound - 1];
    }

   
    private void SetTimerForRound()
    {
        if (currentRound == 1)
        {
            currentTimer = 30f;
        }
        else if (currentRound == 2)
        {
            currentTimer = 45f;
        }
        else if (currentRound == 3)
        {
            currentTimer = 75f;
        }
    }

 
    public IEnumerator RoundTimer()
    {
        while (currentTimer > 0f)
        {
            currentTimer -= Time.deltaTime; 
            timerText.text = Mathf.Ceil(currentTimer).ToString();  
            yield return null;
        }

        
        Challenge.Instance.ResetWord();
        Debug.Log("Time's up! Please re-enter the word.");

      
        SetTimerForRound();
        StartCoroutine(RoundTimer());
    }


    public void NextRound()
    {
        if (currentRound < 3)
        {
            currentRound++;
            UpdateRoundText();  
            Challenge.Instance.ResetWord();  
            StartRound();  
        }
        else
        {
            if (door != null)
            {
                StartCoroutine(MoveDoor(doorClosedPosition, doorOpenPosition, duration));

            }
        }

        // Move the door
        IEnumerator MoveDoor(Vector3 startPos, Vector3 endPos, float time)
        {
            float elapsedTime = 0f;
            audio.PlayOneShot(clip);
            while (elapsedTime < time)
            {
                float t = elapsedTime / time;
                door.transform.position = Vector3.Lerp(startPos, endPos, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            door.transform.position = endPos;
        }
    }
}
