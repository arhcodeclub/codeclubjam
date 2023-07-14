using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayScript : MonoBehaviour
{
    public Button startButton;

	void Start () {
		Button btn = startButton.GetComponent<Button>();
		btn.onClick.AddListener(ButtonClicked);
	}

    void ButtonClicked() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }
}
