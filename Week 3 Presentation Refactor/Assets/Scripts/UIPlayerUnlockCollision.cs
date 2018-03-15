using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerUnlockCollision : MonoBehaviour {

    public Text soulsText;
    public string soulsNeededText;
    private bool isEnabled = true;

	void Start () {
	}
	
	void Update () {
		
	}

	private void OnTriggerEnter(Collider collision)
	{
        if (!isEnabled)
            return;
        if (collision.gameObject.CompareTag("Player")) {
            soulsText.enabled = true;
            soulsText.text = soulsNeededText;
        }
	}

    private void OnTriggerExit(Collider collision)
    {
        if (!isEnabled)
            return;
        if (collision.gameObject.CompareTag("Player"))
        {
            soulsText.enabled = false;
        }
    }
	public void Disable()
	{
        isEnabled = false;
        Destroy(this.gameObject);
	}
}
