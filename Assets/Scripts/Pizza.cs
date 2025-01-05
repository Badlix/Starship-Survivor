using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : MonoBehaviour
{
    public GameObject endGame;

    void Update()
    {
        transform.Rotate(0, 1, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        GameStateManager endGameScript = endGame.GetComponent<GameStateManager>();
        endGameScript.gameEnd = true;
        Destroy(gameObject);
    }
}
