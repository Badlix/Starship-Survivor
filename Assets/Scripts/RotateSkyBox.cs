using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkyBox : MonoBehaviour
{

    public int speed;
    public int start;
    private float newRotation;

    // Start is called before the first frame update
    void Start()
    {
        newRotation = start;
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", newRotation);
        newRotation = newRotation + speed * 0.1f;
    }
}
