using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Look at player
public class GameOverCam : MonoBehaviour
{
    public Vector3 target;
    public Text YouDied;
    public Text Restart;
    public Text BackToMenu;
    public MenuController menu;
    void Start()
	{
        YouDied.enabled = true;
        StartCoroutine(ShowRestart(5));
	}

	void Update()
	{
        transform.LookAt(target);
        if (Input.GetKeyDown(KeyCode.R)) {
            menu.OnStart();
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            menu.OnMenu();
        }
	}

    IEnumerator ShowRestart(float time) {
        yield return new WaitForSeconds(time);
        Restart.enabled = true;
        BackToMenu.enabled = true;
    }
}


