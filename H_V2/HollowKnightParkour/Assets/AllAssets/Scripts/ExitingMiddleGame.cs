using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ExitingMiddleGame : MonoBehaviour
{
    public Image image1, image2, image3;
    public Button exitButton,RestartButton,RestartGame,BugFix;
    private bool HasExit = false; 
    void Start()
    {
      
        image1.gameObject.SetActive(false);
        image2.gameObject.SetActive(false);
        image3.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        RestartButton.gameObject.SetActive(false);
        RestartGame.gameObject.SetActive(false);
        BugFix.gameObject.SetActive(false);
    }

    public void Exit()
    {
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#else
        Application.Quit(); 
#endif
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HasExit = !HasExit;  

            if (HasExit)
            {
                image1.gameObject.SetActive(true);
                image2.gameObject.SetActive(true);
                image3.gameObject.SetActive(true);
                exitButton.gameObject.SetActive(true);
                RestartButton.gameObject.SetActive(true);
                RestartGame.gameObject.SetActive(true);
                BugFix.gameObject.SetActive(true);
            }
            else
            {
                image1.gameObject.SetActive(false);
                image2.gameObject.SetActive(false);
                image3.gameObject.SetActive(false);
                exitButton.gameObject.SetActive(false);
                RestartButton.gameObject.SetActive(false);
                RestartGame.gameObject.SetActive(false);
                BugFix.gameObject.SetActive(false);
            }
        }
    }
}
