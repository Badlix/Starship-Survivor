using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public AudioClip itemSound;
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
        Debug.Log("Collide");
        playerScript.hasKey = true;
        source.PlayOneShot(itemSound);
        Destroy(gameObject);
    }
}
