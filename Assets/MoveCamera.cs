using UnityEngine;
using UnityEngine.UI;

public class MoveCamera : MonoBehaviour
{
    public Material material, text;
    public Image sliderFill;
    float xMove, yMove, speed = 30f, intensity, factor;
    Color color;
    Vector3 previousPosition, direction;
    public Camera cam;

    private void Start()
    {
        InvokeRepeating("RandomSpin", 0, 5);
    }

    void Update()
    {
       
        if (Input.GetMouseButtonDown(0))
        {
            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            direction = previousPosition - cam.ScreenToViewportPoint(Input.mousePosition);
        }
        MoveCam();
        previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        color = new Vector4(Mathf.Clamp((1f / 360f) * Mathf.Abs(transform.rotation.eulerAngles.x), 0.00000000000001f, 1),
                            Mathf.Clamp((1f / 360f) * Mathf.Abs(transform.rotation.eulerAngles.y), 0.00000000000001f, 1),
                            Mathf.Clamp((1f / 360f) * Mathf.Abs(transform.rotation.eulerAngles.z), 0.00000000000001f, 1), 0f);
        intensity = (color.r + color.g + color.b) / 3f;
        factor = 2f / intensity;
        color = new Color(color.r * factor, color.g * factor, color.b * factor);
        material.SetColor("GridColor", color);
        text.SetColor("_GlowColor", color);
        sliderFill.color = color;
    }

    private void MoveCam()
    {
        transform.Rotate(new Vector3(1, 0, 0), direction.y * 180 * Time.deltaTime * speed + yMove);
        transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180 * Time.deltaTime * speed + xMove);
    }

    private void RandomSpin()
    {
        xMove = .1f / Random.Range(1, 4);
        yMove = .1f / Random.Range(1, 4);
    }
}
