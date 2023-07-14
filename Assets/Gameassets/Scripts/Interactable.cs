using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent OnInteract;
    public GameObject player;

    private void FixedUpdate() {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < 3f) {
            showInteract();
        } else {
            hideInteract();
        }
    }

    private void showInteract() {
        if (transform.childCount > 0) {
            return;
        }
        GameObject text = new GameObject();
        text.transform.SetParent(this.transform);
        text.transform.localPosition = Vector3.zero + new Vector3(0, 2, 0);
        text.transform.localScale = Vector3.one;
        TextMesh textMesh = text.AddComponent<TextMesh>();
        textMesh.text = "E";
        textMesh.color = Color.red;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.fontSize = 10;
    }

    private void hideInteract() {
        if (transform.childCount > 0) {
            Destroy(transform.GetChild(0).gameObject);
        }
    }

    public virtual void Interact()
    {
        OnInteract.Invoke();
    }
}