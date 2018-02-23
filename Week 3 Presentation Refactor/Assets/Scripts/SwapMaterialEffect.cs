using UnityEngine;
using System.Collections;

public class SwapMaterialEffect : MonoBehaviour
{

    // Ghost material
    public Material ghostM;
    public float EffectTime = 5f; 

    public bool enabled;

    // Attached to obj
    private Material initialMat;
    private Shader initialShader;
    private Renderer rend;

    // Switch back and forth
    private float timeSinceLast;
    private float elapsedTime;
    private float switchTime = .3f;
    private bool swap;


    void Start()
    {
        rend = GetComponent<Renderer>();
        initialMat = rend.material;
        enabled = false;
    }

    void Update()
    {
        if (!enabled)
            return;

        if (elapsedTime >= EffectTime) {
            rend.material = initialMat; // Revert back to initial material
            elapsedTime = 0;
            enabled = false;
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
