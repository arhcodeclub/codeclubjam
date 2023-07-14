using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent OnInteract;
    public float range = 3f;
    GameObject player;
    public Transform currentTransform;

    public GameObject text;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentTransform = transform;
    }

    private void FixedUpdate() {

        float distance1 = Vector2.Distance(currentTransform.position, player.transform.position);

        if (distance1 < range) {
            if (Input.GetKeyDown(KeyCode.E)) {
                Interact();
            }
            if (text != null)
            {
                text.SetActive(true);
            }
        } else {
            if (text != null)
            {
                text.SetActive(false);
            }
        }
    }

    public virtual void Interact()
    {
        OnInteract.Invoke();

        Debug.Log("Interacted with " + gameObject.name);

    }
}