using UnityEngine;

public class MouseBasedRotation : MonoBehaviour
{
    [SerializeField] private float rotationAmount = 0.5f; // Controls sensitivity of the effect
    private Quaternion initialRotation;
    private Vector2 screenCenter;

    void Start()
    {
        initialRotation = transform.rotation;
        screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
    }

    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        mousePosition.x = Mathf.Clamp(mousePosition.x, 0, Screen.width);
        mousePosition.y = Mathf.Clamp(mousePosition.y, 0, Screen.height);

        float offsetX = (mousePosition.x - screenCenter.x) / screenCenter.x;
        float offsetY = (mousePosition.y - screenCenter.y) / screenCenter.y;

        Quaternion targetRotation = initialRotation * Quaternion.Euler(
            -offsetY * rotationAmount,
            offsetX * rotationAmount,
            0f
        );

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
}