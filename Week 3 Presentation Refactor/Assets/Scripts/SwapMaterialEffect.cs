using UnityEngine;
using System.Collections;

public class SwapMaterialEffect : MonoBehaviour
{

    // Ghost material
    public Material ghostM;
    public float EffectTime = 5f;
    public GameObject colorChangeHitEffect;

    public bool isActive;

    // Attached to obj
    private Material initialMat;
    private Shader initialShader;
    private Renderer rend;

    // Switch back and forth
    private float timeSinceLast;
    private float elapsedTime;
    private float switchTime = .1f;
    private bool swap;

    void Start()
    {
        rend = GetComponent<Renderer>();
        initialMat = rend.material;
        isActive = false;
    }

    void Update()
    {
        if (!isActive) {
            return;
        }

        colorChangeHitEffect.GetComponent<ColorEffectOnScreen>().enabled = true;

        if (elapsedTime >= EffectTime) {
            colorChangeHitEffect.GetComponent<ColorEffectOnScreen>().End();

            rend.material = initialMat; // Revert back to initial material
            elapsedTime = 0;
            isActive = false;
            return;
        }

        elapsedTime += Time.deltaTime; // counter for effect

        // Swap back and borth between ghostM and the initial material
        if (timeSinceLast >= switchTime)
        {
            if (swap) {
                rend.material = ghostM;
            } else {
                rend.material = initialMat;
            }
            swap = !swap;
            timeSinceLast = 0;
        }
        else
            timeSinceLast += Time.deltaTime;
        
    }
}
