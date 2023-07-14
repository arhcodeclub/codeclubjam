using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueStartExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<DSDialogue>().StartDialogue();   
    }

}
