using UnityEngine;
using UnityEngine.EventSystems;

public enum ShapeType { Box, Triangle, Diamond }

public class Shape : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public ShapeType shapeType;

    [Header("Floats")]
    public float yMax = 3.9f;
    public float yMin = -.1f;
    public float xMax = 3.5f;
    public float xMin = -3.5f;
    public float sensitivity = 0.008f;
    public float maxSpeed = 1f;

    public RectTransform m_transform = null;
    public bool isShapeSpecific = false;

    private bool dragging;

    private Vector2 posToMove;

    void Start() => m_transform = GetComponent<RectTransform>();

    private void FixedUpdate()
    {
        m_transform.anchoredPosition += posToMove;
        posToMove = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragging || eventData.delta.y > 0 && m_transform.anchoredPosition.y >= yMax / m_transform.localScale.y
            || eventData.delta.y < 0 && m_transform.anchoredPosition.y <= yMin / m_transform.localScale.y
            || eventData.delta.x > 0 && m_transform.anchoredPosition.x >= xMax / m_transform.localScale.x
            || eventData.delta.x < 0 && m_transform.anchoredPosition.x <= xMin / m_transform.localScale.x)
            return;

        posToMove = new Vector2(Mathf.Clamp(eventData.delta.x * sensitivity, -maxSpeed, maxSpeed), Mathf.Clamp(eventData.delta.y * sensitivity, -maxSpeed, maxSpeed));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetSiblingIndex(transform.parent.childCount - 1);
        dragging = true;
    }

    public void OnEndDrag(PointerEventData eventData) => dragging = false;
}
