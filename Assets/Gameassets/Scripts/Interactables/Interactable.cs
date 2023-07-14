using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent OnInteract;
    public float range = 1.5f;
    GameObject player;

    public GameObject text;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate() {

        float distance1 = Vector2.Distance(transform.position, player.transform.position);

        if (distance1 < range) {
            text.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E)) {
                Interact();
            }
        } else {
            text.SetActive(false);
        }
    }

    public virtual void Interact()
    {
        OnInteract.Invoke();

        Debug.Log("Interacted with " + gameObject.name);

    }
}