using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler
{
    private void Update()
    {
        if (transform.childCount > 0)
        {
            GetComponent<Image>().enabled = false;
        }else
        {
            GetComponent<Image>().enabled = true;
        }
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem item = eventData.pointerDrag.GetComponent<DraggableItem>();
        item.parentAfterDrag = transform;
    }
}
