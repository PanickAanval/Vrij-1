using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{
    public TMP_Text MyText;
    public int amountScrews = 0;
    public int amountCogs = 0;
    public UnityEvent Genoeg;
    // Start is called before the first frame update
    void Start()
    {
        MyText.text = "Schroeven:";
    }
    // Update is called once per frame
    void Update()
    {
        MyText.text = "Schroeven:" + amountScrews;
        if (amountScrews == 10)
        {
            Genoeg.Invoke();
        }
    }

    public void IncreaseCounterScrew()
    {
        amountScrews++;
        print("Schroef Opgepakt");
    }

    public void IncreaseCounterCog()
    {
        amountCogs++;
        print("Wiel Opgepakt");
    }
}
