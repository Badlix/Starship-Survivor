using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CoinsScript : MonoBehaviour
{

    public AudioClip coinSound;
    public AudioSource source;
    private KeyInput playerScript;

    void Start()
    {
        GameObject player = GameObject.Find("Astronaut");
        playerScript = player.GetComponent<KeyInput>();
    }

    void Update()
    {
        transform.Rotate(0, 1, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        playerScript.numberOfFuelTank++;
        source.PlayOneShot(coinSound);
        Destroy(gameObject);
    }
}
