using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D body;
    private Vector2 input;

    public GameObject cam;

    public float speed = 5.0f;  

    public Sprite right;
    public Sprite idle;

    private int sinceLastIdle = 0;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        body.velocity = input * speed;

        if (body.velocity.x > 0.5) {
            GetComponent<SpriteRenderer>().sprite = right;
            GetComponent<SpriteRenderer>().flipX = false;
        } else if (body.velocity.x < -0.5) {
            GetComponent<SpriteRenderer>().sprite = right;
            GetComponent<SpriteRenderer>().flipX = true;
        } else {
            GetComponent<SpriteRenderer>().sprite = idle;
            sinceLastIdle += 1;
            if (sinceLastIdle > 50) {
                GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
                sinceLastIdle = 0;
            }
        }
        
        cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(transform.position.x, transform.position.y, -10), 0.1f);
    }

    private void OnMove(InputValue inputValue)
    {
        input = inputValue.Get<Vector2>();
    }

    private void OnFire(InputValue inputValue)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider && hit.collider.gameObject.tag == "Interactable") {
            // todo: interact with object
            Debug.Log("Interacted");
        }
    }
}
