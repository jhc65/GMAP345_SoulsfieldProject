using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
	void Start()
	{

	}

	void Update()
	{
			
	}

    public void OnStart() {
        SceneManager.LoadScene("Scenes/Final");
    }

	public void OnQuit()
	{
        Application.Quit();
	}

    public void OnMenu() {
        SceneManager.LoadScene("Scenes/StartScreen");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
