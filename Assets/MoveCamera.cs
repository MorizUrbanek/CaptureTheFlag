using UnityEngine;
using UnityEngine.UI;

public class MoveCamera : MonoBehaviour
{
    public ColorScript color;
    float xMove, yMove, speed = 30f;
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
        color.ChangeColor();
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
