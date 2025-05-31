using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI buttonText; 
    public Image image1; 
    public Image image2; 
    public Color hoverColor = Color.black;

    private Color originalTextColor;
    private Color originalImage1Color;
    private Color originalImage2Color;

    public AudioSource hoverSound;

    void Start()
    {
        if (buttonText == null)
            buttonText = GetComponentInChildren<TextMeshProUGUI>(); // Get the button's text automatically

        if (buttonText != null)
            originalTextColor = buttonText.color;
        else
            Debug.LogError("No Text component found on button!");

        if (image1 != null)
            originalImage1Color = image1.color;
        if (image2 != null)
            originalImage2Color = image2.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonText != null)
            buttonText.color = hoverColor; 

        if (image1 != null)
            image1.color = hoverColor; 

        if (image2 != null)
            image2.color = hoverColor; 

        if (hoverSound != null && !hoverSound.isPlaying)
        {
            hoverSound.Play();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonText != null)
            buttonText.color = originalTextColor; 

        if (image1 != null)
            image1.color = originalImage1Color; 

        if (image2 != null)
            image2.color = originalImage2Color;
    }
}
