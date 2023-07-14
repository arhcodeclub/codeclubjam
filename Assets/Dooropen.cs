using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dooropen : MonoBehaviour
{
    public Animator anim;
    public void openDoor()
    {
        anim.SetTrigger("Open");
    }
}
