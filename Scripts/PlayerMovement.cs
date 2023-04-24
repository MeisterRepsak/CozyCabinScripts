using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public GameObject body;

    Rigidbody rb;
    InputManager input;

    Transform cam;
    [SerializeField] float speed;
    [SerializeField] Vector3 moveDirection;

    [Header("Bodies")]
    public GameObject normalBody;
    public GameObject fishBody;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        input = GetComponent<InputManager>();
        cam = Camera.main.transform;
    }

    private void Update()
    {
        if (input.isMoving)
        {
            HandleRotation();
        }
    }
    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        moveDirection = input.xAxis * cam.right + input.zAxis * cam.forward;
        moveDirection.y = 0;
        moveDirection.Normalize();
        moveDirection *= speed;
        moveDirection.y += rb.velocity.y;

        rb.velocity = moveDirection;
    }

    void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = input.xAxis * cam.right + input.zAxis * cam.forward;
        targetDirection.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        body.transform.rotation = Quaternion.Slerp(body.transform.localRotation, targetRotation, 10 * Time.deltaTime);
    }

    public void SwitchBody(GameObject body)
    {
        if (body == this.body)
            return;
        
        body.SetActive(true);
        body.transform.localRotation = this.body.transform.localRotation;
        this.body.SetActive(false);
        this.body = body;
    }
}
