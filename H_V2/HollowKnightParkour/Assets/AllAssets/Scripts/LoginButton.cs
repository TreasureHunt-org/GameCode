using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections.Generic;


[System.Serializable]
public class LoginResponse
{
    public bool success;
    public string message;
    public List<UserData> data;
    public List<string> errors;
    public int errorCode;
    public long timestamp;
}

[System.Serializable]
public class UserData
{
    public string id;
    public string username;
    public string email;
    public List<string> roles;
    public string refreshToken;
    public string accessToken;
}

public class LoginButton : MonoBehaviour
{
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    public CanvasGroup Panel;
    public TextMeshProUGUI textMeshPro;
    public AudioSource AudioSource;
    public AudioClip clip;

    private string loginURL = "https://f16f-92-253-50-80.ngrok-free.app/api/v1/treasure-hunt/auth/signin";

    public void OnPlayButtonClicked()
    {
        string username = usernameField.text.Trim();
        string password = passwordField.text.Trim();
        Debug.Log(username);
        Debug.Log(password);
       
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            textMeshPro.text = "Please enter your username and password";
        }
        else
        {
            StartCoroutine(ValidateLogin(username, password));
        }
    }

    IEnumerator ValidateLogin(string username, string password)
    {
        string jsonPayload = $"{{\"email\":\"{username}\",\"password\":\"{password}\"}}";
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonPayload);

        UnityWebRequest request = new UnityWebRequest(loginURL, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
        {
            string responseBody = request.downloadHandler.text;
            Debug.Log($"Response: {responseBody}");

            // Deserialize JSON
            LoginResponse loginData = JsonUtility.FromJson<LoginResponse>(responseBody);

            if (loginData.success && loginData.data.Count > 0)
            {
                UserData user = loginData.data[0];
                Debug.Log($"Username: {user.username}");
                Debug.Log($"Email: {user.email}");
                Debug.Log($"Roles: {string.Join(", ", user.roles)}");
                Debug.Log($"Access Token: {user.accessToken}"); ///////////////////////////

            
                PlayerPrefs.SetString("AuthToken", user.accessToken);
                PlayerPrefs.SetString("RefreshToken", user.refreshToken);
                PlayerPrefs.Save();

                StartCoroutine(FadeAndLoadScene("Cinematic"));
            }
        }
        else
        {
            string errorResponse = request.downloadHandler.text;
            Debug.Log($"Error Response: {errorResponse}");
            textMeshPro.text = "Invalid username or password";
        }
    }
    IEnumerator FadeAndLoadScene(string sceneName)
    {
        if (Panel != null)
        {
            Panel.gameObject.SetActive(true);
            float duration = 1.5f;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                Panel.alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
                yield return null;
            }
        }

        SceneManager.LoadScene(sceneName);
    }
}