using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Collectable : MonoBehaviour
{
    public int amountScrews = 0;
    public int amountCogs = 0;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
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
