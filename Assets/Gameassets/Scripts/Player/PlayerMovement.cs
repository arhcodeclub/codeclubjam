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

    private void OnLook(InputValue inputValue)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider.gameObject.tag == "Interactable") {
            // todo: draw text above object
        }
    }

    private void OnFire(InputValue inputValue)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider.gameObject.tag == "Interactable") {
            // todo: interact with object
        }
    }
}
