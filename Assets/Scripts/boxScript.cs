using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxScript : MonoBehaviour
{
    [SerializeField]
    public Animator boxAnimator;
    public AudioSource audioSource;
    public AudioClip chestOpen;
    public AudioClip chestFailOpen;
    public Transform lightSaber;
    public Vector3 newPos;
    private bool isAlreadyOpen = false;
    private float delay = 0.8f;
    private bool lightSaberAnimationPlaying = false;

    void Update()
    {
        if (isAlreadyOpen && lightSaberAnimationPlaying)
            lightSaber.position += new Vector3(0, 0.01f, 0);
        if (lightSaber.position.y >= 2)
        {
            lightSaberAnimationPlaying = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isAlreadyOpen == false)
        {
            GameObject go = GameObject.Find("Astronaut");
            KeyInput d = go.GetComponent<KeyInput>();
            if (d.hasKey)
            {
                boxAnimator.SetTrigger("OpenTrigger");
                Invoke("PlayChestOpenWithDelay", delay);
                isAlreadyOpen = true;
                Invoke("PlayLightSaberAnimation", delay);
            }
            else
            {
                audioSource.PlayOneShot(chestFailOpen);
            }
        }
    }

    void PlayLightSaberAnimation()
    {
        lightSaberAnimationPlaying = true;
    }

    void PlayChestOpenWithDelay()
    {
        audioSource.PlayOneShot(chestOpen);
    }
}
