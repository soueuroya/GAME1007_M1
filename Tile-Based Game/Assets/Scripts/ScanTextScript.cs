using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanTextScript : MonoBehaviour
{
    [SerializeField]
    Animator toogleAnimation;

    public GridHolderScript ghs;

    public void TurnOnScan() // turn on scan mode and toggle button
    {
        ghs = GameObject.Find("GridHolder").GetComponent<GridHolderScript>();
        ghs.ScanSelected();
        toogleAnimation.ResetTrigger("Extract");
        toogleAnimation.SetTrigger("Scan");
    }
}
