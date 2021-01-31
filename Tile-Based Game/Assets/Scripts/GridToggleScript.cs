using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridToggleScript : MonoBehaviour
{
    [SerializeField]
    Animator canvasAnimation;

    [SerializeField]
    Canvas grid;

    [SerializeField]
    GameObject gridHolder;

    bool isShowing = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) // When player press G
        {
            if (isShowing) // If it's ON
            {
                TurnOFF();               // Turn OFF
            }
            else                         // Else, turn it on
            {
                TurnON();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Table")) // If we get close to the table
        {
            TurnON(); // Turn the grid ON
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Table")) // If we get far from the table
        {
            TurnOFF(); // Turn the grid OFF
        }
    }

    private void TurnON()
    {
        if (!isShowing)
        {
            gridHolder.GetComponent<GridHolderScript>().ReGenerate();
            //grid.enabled = true; //Turn ON
            canvasAnimation.ResetTrigger("Hide");
            canvasAnimation.SetTrigger("Show");
            isShowing = true;
            Cursor.visible = true; // set cursor visible
        }
    }

    private void TurnOFF()
    {
        if (isShowing)
        {
            gridHolder.GetComponent<GridHolderScript>().DestroyGrid();
            //grid.enabled = false; // TURN OFF
            canvasAnimation.ResetTrigger("Show");
            canvasAnimation.SetTrigger("Hide");
            isShowing = false;
            Cursor.visible = false; // set cursor NOT visible
        }
    }
}
