using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffectOverTime : MonoBehaviour {

    private Color color1;
    public Color color2;
    [SerializeField] private float duration = 10.0f;
    private float t = 0;

    private Image background;
    private AudioSource aud;
    private List<AudioSource> usedAudios = new List<AudioSource>();
    void Start()
    {
        OnEnable();
    }

	private void OnEnable()
	{
        foreach (AudioSource a in FindObjectsOfType<AudioSource>() as AudioSource[]) {
            usedAudios.Add(a);
            a.volume = .2f;
        }
        aud = GetComponent<AudioSource>();
        aud.volume = 1f;
        aud.Play();
        background = GetComponent<Image>();
        background.enabled = true;
        color1 = background.color;

	}

	void Update()
    {
        // lerp color
        background.color = Color.Lerp(color1, color2, t);
        if (t < 1)
        { // while t below the end limit...
          // increment it at the desired rate every update:
            t += Time.deltaTime / duration;
        }
        if (background.color == color2)
            End();
    }
    public void End() {
        
        foreach (AudioSource a in usedAudios) {
            if (a)
                a.volume = 1; // restore volume
        }
        t = 0;
        background.color = color1;
        background.enabled = false;
        this.enabled = false;
    }
}
