using UnityEngine;

public class SquareScriptMovement : MonoBehaviour
{
    float horizontalInput;
    float verticalInput;
    [Header("Speed")]
    [SerializeField] float speed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        transform.position += new Vector3(horizontalInput, verticalInput, 0) * speed * Time.deltaTime;
    }
}
