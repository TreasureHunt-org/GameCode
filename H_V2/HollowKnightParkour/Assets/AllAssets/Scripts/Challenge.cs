using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Challenge : MonoBehaviour
{
    public static Challenge Instance { get; private set; }
    private List<char> selectedLetters = new List<char>();
    public GameManager gameManager;

    public TextMeshProUGUI formingWordText;  
    private Dictionary<int, string> correctWords = new Dictionary<int, string>(); // Stores past correct words

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Add a letter to the word
    public void AddLetter(char letter)
    {
        if (selectedLetters.Count < 7)
        {
            selectedLetters.Add(letter);
            UpdateFormingWordText();  // Update the text when a new letter is added
        }
    }

    public void UpdateFormingWordText()
    {
        string displayText = "";

        // Loop through each round to display its status
        for (int i = 1; i <= gameManager.currentRound; i++)  // include the current round as well
        {
            if (correctWords.ContainsKey(i))
            {
                displayText += $"Round {i}: Completed\n";  // Mark the round as completed
            }
            else
            {
                // Show the current word being formed for incomplete rounds
                displayText += $"Round {i}: {GetWord()}\n";
            }
        }

        formingWordText.text = displayText;
    }


    // Get the current word as a string
    public string GetWord()
    {
        return new string(selectedLetters.ToArray());
    }

    // Reset the word (for next round or re-entry)
    public void ResetWord()
    {
        selectedLetters.Clear();
        UpdateFormingWordText();  // Reset the text when the word is cleared
    }

    // Check if the word is a palindrome
    public bool IsPalindrome()
    {
        string word = GetWord();
        int len = word.Length;
        for (int i = 0; i < len / 2; i++)
        {
            if (word[i] != word[len - i - 1])
            {
                return false;
            }
        }
        return true;
    }

    // Stores the correct word and moves to the next round
    public void SaveCorrectWord()
    {
        correctWords[gameManager.currentRound] = GetWord();
    }
}
