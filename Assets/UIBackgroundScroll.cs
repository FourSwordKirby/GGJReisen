using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBackgroundScroll : MonoBehaviour
{
    public float x_animationDistance;
    public float y_animationDistance;

    private void Update()
    {
        ScrollImage(x_animationDistance, y_animationDistance);    
    }

    float current_x_offset = 0;
    float current_y_offset = 0;
    public void ScrollImage(float x, float y)
    {
        current_x_offset += x;
        current_y_offset += y;
        Vector2 offset = new Vector2(current_x_offset, current_y_offset);
        this.GetComponent<RawImage>().material.SetTextureOffset("_MainTex", offset);
    }
}
