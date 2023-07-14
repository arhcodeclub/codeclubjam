using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorInteractable : MonoBehaviour
{
    public UnityEvent OnInteract;
    public GameObject text;
    public GameObject player;
    private AudioSource audio;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    private void FixedUpdate() {
        Vector3 point1 = transform.GetChild(1).position;
        Vector3 point2 = transform.GetChild(2).position;

        float distance1 = Vector2.Distance(point1, player.transform.position);
        float distance2 = Vector2.Distance(point2, player.transform.position);

        if (distance1 < 1.5f || distance2 < 1.5f) {
            Debug.Log("In range");
            transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
            text.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E)) {
                Debug.Log("Interacted");
                audio.Play();
                Interact();
            }
        } else {
            text.SetActive(false);
            transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public virtual void Interact()
    {
        OnInteract.Invoke();

        Debug.Log("Interacted with " + gameObject.name);

        // Get the positions of point1 and point2 relative to the player. The point1 and point2 are children of the door.

        Vector3 point1 = transform.GetChild(1).position;
        Vector3 point2 = transform.GetChild(2).position;

        // Get the distance between the points and the player.
        float distance1 = Vector2.Distance(point1, player.transform.position);
        float distance2 = Vector2.Distance(point2, player.transform.position);

        if (distance1 < distance2) {
            player.transform.position = point2;
        } else {
            player.transform.position = point1;
        }
    }
}