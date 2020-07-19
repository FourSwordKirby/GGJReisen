using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusText : MonoBehaviour
{
    public Color startingColor;
    public TextMeshPro text;

    public float duration = 0.4f;
    public float speed;

    private void Awake()
    {
        text.color = startingColor;
    }

    float timer = 0.0f;
    // Update is called once per frame
    void Update()
    {
        if (timer < duration)
        {
            this.transform.position += Vector3.up * speed * Time.deltaTime;
            text.color = Color.Lerp(startingColor, Color.clear, timer / duration);
            timer += Time.deltaTime;
        }
        else
            Destroy(this.gameObject);
    }
}
