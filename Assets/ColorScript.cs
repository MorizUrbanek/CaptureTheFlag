using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorScript : MonoBehaviour
{
    public Material cubeMaterial, text;
    public Image sensitivitySliderFill, volumeSliderFill;
    float intensity, factor;
    Color color;

   public void ChangeColor()
   {
        color = new Vector4(Mathf.Clamp((1f / 360f) * Mathf.Abs(transform.rotation.eulerAngles.x), 0.00000000000001f, 1),
                            Mathf.Clamp((1f / 360f) * Mathf.Abs(transform.rotation.eulerAngles.y), 0.00000000000001f, 1),
                            Mathf.Clamp((1f / 360f) * Mathf.Abs(transform.rotation.eulerAngles.z), 0.00000000000001f, 1), 0f);
        intensity = (color.r + color.g + color.b) / 3f;
        factor = 2f / intensity;
        color = new Color(color.r * factor, color.g * factor, color.b * factor);
        cubeMaterial.SetColor("GridColor", color);
        text.SetColor("_GlowColor", color);
        sensitivitySliderFill.color = color;
        volumeSliderFill.color = color;
    }
}
