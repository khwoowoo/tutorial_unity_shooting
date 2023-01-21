using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshairs : MonoBehaviour
{
    public LayerMask targetMask;
    public SpriteRenderer dot;
    public Color dotHighlightColor;
    Color originColor;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        originColor = dot.color;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * 40 * Time.deltaTime);
    }

    public void DetecTargets(Ray ray)
    {
        if(Physics.Raycast(ray, 100f, targetMask))
        {
            dot.color = dotHighlightColor;
        }
        else
        {
            dot.color = originColor;
        }
    }
}
