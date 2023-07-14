using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D body;
    private Vector2 input;

    public float speed = 5.0f; 

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        body.velocity = input * speed;
    }

    private void OnMove(InputValue inputValue)
    {
        input = inputValue.Get<Vector2>();
    }

    private void OnFire(InputValue inputValue)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        Debug.Log(hit.collider);
        Debug.Log(hit);
        if (hit.collider != null) {
            Debug.Log(hit.collider.gameObject.tag);
        }
    }
}
