using UnityEngine;
using UnityEngine.InputSystem;

public class Crosshair : MonoBehaviour
{
    private RectTransform rectTransform;

    void Start()
    {
        Cursor.visible = false;
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        rectTransform.position = mousePos;
    }

    void OnDestroy()
    {
        Cursor.visible = true;
    }
}