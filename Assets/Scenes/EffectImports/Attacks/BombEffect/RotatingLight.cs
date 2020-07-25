using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingLight : MonoBehaviour {

    public Renderer selfRenderer;
    public float rate = 0.2f;
    private float currentTime = 0;
    private void Update()
    {
        currentTime += Time.deltaTime;
        //renderer.material.mainTextureOffset += Vector2.left * Time.deltaTime;
        selfRenderer.material.SetVector("_Offset", Vector2.left * rate * currentTime);
    }
}
