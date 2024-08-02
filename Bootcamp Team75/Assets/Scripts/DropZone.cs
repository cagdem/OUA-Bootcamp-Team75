using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;

        DraggableItem item = eventData.pointerDrag.GetComponent<DraggableItem>();
        item.parentAfterDrag = transform;
        
    }
}
