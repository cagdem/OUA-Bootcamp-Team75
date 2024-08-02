using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Image image;
    public Transform parentAfterDrag;
    Vector2? startDiff;
    public void Update()
    {
        image = GetComponent<Image>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (startDiff == null)
        {
            startDiff = eventData.position - (Vector2)transform.position;
        }
        transform.position = eventData.position - startDiff.GetValueOrDefault();    
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        startDiff = null;
        image.raycastTarget = true;
    }
}
