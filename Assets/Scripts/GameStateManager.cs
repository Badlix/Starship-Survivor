using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public bool isDead = false;
    public bool gameEnd = false;
    private float startTime;
    private float elapsedTime = 0;

    [SerializeField]
    public GameObject panel;
    public TMPro.TextMeshProUGUI gameStatusText;
    public TMPro.TextMeshProUGUI timeText;

    void Start()
    {
        gameStatusText.text = "";
        timeText.text = "";
        startTime = Time.time;
        panel.SetActive(false);
    }

    void Update()
    {
        if (gameEnd && elapsedTime == 0)
        {
            // On affiche le menu de fin de jeu
            panel.SetActive(true);
            // on libère le curseur pour pouvoir cliquer sur les boutons
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (isDead)
            {
                gameStatusText.text = "Vous êtes mort";
            }
            else
            {
                gameStatusText.text = "Vous avez survecu";
                elapsedTime = Time.time - startTime;
                timeText.text = "Temps : " + formatTime(elapsedTime);
            }
        }
    }

    private string formatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
