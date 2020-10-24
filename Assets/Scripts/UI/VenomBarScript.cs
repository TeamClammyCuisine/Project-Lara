using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VenomBarScript : MonoBehaviour
{
    public Slider slider;

    public void SetMaxVenom(float venom)
    {
        slider.maxValue = venom;
    }

    public void SetVenom(float venom)
    {
        slider.value = venom;
    }
}
