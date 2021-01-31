using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractTextScript : MonoBehaviour
{
    [SerializeField]
    Animator toogleAnimation;

    [SerializeField]
    GridHolderScript ghs;

    public void TurnOnExtract() // turn on extract mode and toggle button
    {
        ghs = GameObject.Find("GridHolder").GetComponent<GridHolderScript>();
        ghs.ExtractSelected();
        toogleAnimation.ResetTrigger("Scan");
        toogleAnimation.SetTrigger("Extract");
    }
}