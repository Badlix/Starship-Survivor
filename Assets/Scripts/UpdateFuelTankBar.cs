using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateFuelTankBar : MonoBehaviour
{

    private int nbFuelTank = 0;

    [SerializeField]
    public KeyInput character_script;
    public TMPro.TextMeshProUGUI fuelTankText;
    public GameObject bar_1;
    public GameObject bar_2;
    public GameObject bar_3;
    public GameObject bar_4;
    public GameObject bar_5;
    public GameObject key;

    void Start()
    {
        bar_1.SetActive(false);
        bar_2.SetActive(false);
        bar_3.SetActive(false);
        bar_4.SetActive(false);
        bar_5.SetActive(false);
        key.SetActive(false);
        fuelTankText.text = nbFuelTank.ToString() + "/5";
    }

    void Update()
    {
        GameObject go = GameObject.Find("Astronaut");
        KeyInput d = go.GetComponent<KeyInput>();
        if (nbFuelTank != character_script.numberOfFuelTank)
        {
            nbFuelTank = character_script.numberOfFuelTank;
            fuelTankText.text = nbFuelTank.ToString() + "/5";
            if (nbFuelTank == 1)
            {
                bar_1.SetActive(true);
            }
            else if (nbFuelTank == 2)
            {
                bar_2.SetActive(true);
            }
            else if (nbFuelTank == 3)
            {
                bar_3.SetActive(true);
            }
            else if (nbFuelTank == 4)
            {
                bar_4.SetActive(true);
            }
            else if (nbFuelTank == 5)
            {
                bar_5.SetActive(true);
            }
        }
        if (character_script.hasKey)
        {
            key.SetActive(true);
        }
    }
}
