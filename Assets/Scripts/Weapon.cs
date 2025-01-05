using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public Transform playerHand;
    public GameObject player;
    public Vector3 rotationOffset;
    private bool isPickedUp = false;
    private KeyInput playerScript;

    void Start()
    {
        playerScript = player.GetComponent<KeyInput>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Pickup();
        }
    }

    void Pickup()
    {
        if (!isPickedUp)
        {
            playerScript.hasWeapon = true;
            isPickedUp = true;

            // Attacher l'arme à la main du joueur
            transform.SetParent(playerHand);

            // Repositionner l'arme pour qu'elle soit dans le bon sens
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(rotationOffset); ;
        }
    }
}
