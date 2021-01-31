using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractTextScript : MonoBehaviour
{
    [SerializeField]
    private Animator toogleAnimation;

    [SerializeField]
    private GridHolderScript ghs;

    public void TurnOnExtract() // turn on extract mode and toggle button
    {
        ghs = GameObject.Find("GridHolder").GetComponent<GridHolderScript>();
        ghs.ExtractSelected();
        ExtractAnimationToggle();
    }

    private void ExtractAnimationToggle()
    {
        toogleAnimation.ResetTrigger("Scan");
        toogleAnimation.SetTrigger("Extract");
    }
}