using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : MonoBehaviour
{
    public GameObject endGame;
    public AudioClip itemSound;
    public AudioSource source;

    void Update()
    {
        transform.Rotate(0, 1, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        GameStateManager endGameScript = endGame.GetComponent<GameStateManager>();
        endGameScript.gameEnd = true;
        source.PlayOneShot(itemSound);
        Destroy(gameObject);
    }
}
