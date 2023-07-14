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
            transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
            text.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E)) {
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

        Vector3 point1 = transform.GetChild(1).position;
        Vector3 point2 = transform.GetChild(2).position;

        float distance1 = Vector2.Distance(point1, player.transform.position);
        float distance2 = Vector2.Distance(point2, player.transform.position);

        if (distance1 < distance2) {
            player.transform.position = point2;
        } else {
            player.transform.position = point1;
        }

        Camera.main.GetComponent<CameraShake>().Shake(0.1f, 0.1f);
    }
}