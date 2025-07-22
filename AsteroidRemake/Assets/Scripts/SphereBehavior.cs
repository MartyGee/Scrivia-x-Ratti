using Unity.VisualScripting;
using UnityEngine;

public class SphereBehaviour : MonoBehaviour
{
    [Header("Velocità")]
    [SerializeField] float speed = 10f;
    [Header("Input")]
    float horizontalInput;
    float verticalInput;

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(horizontalInput, verticalInput) * speed * Time.deltaTime;
        transform.Translate(movement);
    }
}
