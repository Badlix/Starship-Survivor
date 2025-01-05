using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    string m_sceneName = "Game";
    public GameObject startButton;
    public GameObject quitButton;

    private Color hoverColor = new Color(0.6f, 0.6f, 0.6f, 1.0f);
    private Color notHoverColor = new Color(0.9f, 0.9f, 0.9f, 1.0f);

    public void changeScene()
    {
        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(m_sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void setStartButtonIsHover()
    {
        Image buttonImage = startButton.GetComponent<Image>();
        buttonImage.color = hoverColor;
    }

    public void setStartButtonIsNotHover()
    {
        Image buttonImage = startButton.GetComponent<Image>();
        buttonImage.color = notHoverColor;
    }

    public void setQuitButtonIsHover()
    {
        Image buttonImage = quitButton.GetComponent<Image>();
        buttonImage.color = hoverColor;
    }

    public void setQuitButtonIsNotHover()
    {
        Image buttonImage = quitButton.GetComponent<Image>();
        buttonImage.color = notHoverColor;
    }
}
