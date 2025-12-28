using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotateSpeed = 10f;
    public float moveSpeed = 10f;
    public float zoomSpeed = 10f;
    private int _mult = 1;

    private void Update()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
        float rotate = 0f;
        if (Input.GetKey(KeyCode.Q)) rotate = 1f;
        else if (Input.GetKey(KeyCode.E)) rotate = -1f;
        if (Input.GetKey(KeyCode.LeftShift)) _mult = 2;
        else _mult = 1;
        transform.Rotate(Vector3.up * -rotateSpeed * Time.deltaTime * rotate * _mult, Space.World);
        transform.Translate(new Vector3(hInput, 0, vInput)*Time.deltaTime*moveSpeed*_mult, Space.Self);
        transform.position += transform.up * zoomSpeed * Time.deltaTime * Input.GetAxis("Mouse ScrollWheel");
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -20f, 30f), transform.position.z);
    }
}
