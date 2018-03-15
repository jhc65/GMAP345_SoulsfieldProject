using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorEffectOnScreen : MonoBehaviour {

    public Color color1 = Color.black;
    public Color color2 = Color.white;
    public float duration = 10.0F;

    private float t;

    private Image background;

    void Awake()
    {
        background = GetComponent<Image>();

    }

    void Update()
    {

        // ping pong color
        float t = Mathf.PingPong(Time.time, duration) / duration;
        background.color = Color.Lerp(color1, color2, t);

    }
    public void End() {
        background.color = Color.Lerp(color2, color1, 5);
        enabled = false;
    }
}
