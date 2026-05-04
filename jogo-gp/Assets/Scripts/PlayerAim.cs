using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    public Transform gunPoint; // arraste o GunPoint aqui

    void Update()
    {
        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(mouseScreen);
        mousePos.z = 0f;

        // Só o GunPoint rotaciona, o corpo do player fica parado
        Vector3 direction = mousePos - gunPoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gunPoint.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}