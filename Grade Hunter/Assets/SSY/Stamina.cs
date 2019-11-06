using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Stamina : MonoBehaviour
{
    public Slider sb;
    public static int StaminaMax;
    public static int StaminaCurrent;
    // Start is called before the first frame update
    void Start()
    {
        sb.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
       sb.value = StaminaCurrent / StaminaMax;
    }
}