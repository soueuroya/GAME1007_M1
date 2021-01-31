using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public int x, y;
    public GridHolderScript ghs;
    public Text value;

    public void OnPointerEnter(PointerEventData eventData) // On mouse over
    {
        transform.localScale = (Vector2.right + Vector2.up) * 1.09f; // Scale square up a bit
    }

    public void OnPointerExit(PointerEventData eventData) // On mouse off
    {
        transform.localScale = (Vector2.right + Vector2.up); // rescale back to original size
    }

    public void OnPointerDown(PointerEventData eventData) // On mouse down
    {
        ghs.TileSelected(x, y); // Call tile selected function
    }
}
