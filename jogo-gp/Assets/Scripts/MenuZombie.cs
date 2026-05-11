using UnityEngine;

public class MenuZombie : MonoBehaviour
{
    public float speed = 2f;

    void Update()
    {
        // Anda para a esquerda
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Destroi quando sair da tela
        if (transform.position.x < -12f)
            Destroy(gameObject);
    }
}