using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapMoveController : MonoBehaviour
{
    private Vector3 move;
    private float moveTilePanel = 8.0f;
    private Rigidbody2D rb;
    Vector2 velocity;

    void Start() {
        TryGetComponent(out rb);    
    }

    public void OnInputMove(InputAction.CallbackContext context) {
        move = context.ReadValue<Vector2>();

        move = move.normalized;
        Move();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //InputMove();
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    public void OnJump(InputAction.CallbackContext context) {

    }

    private void Move() {
        //transform.Translate(move * 0.5f);
        //rb.velocity = move * moveTilePanel;

        velocity = move * moveTilePanel;

        //rb.MovePosition(rb.position + velocity );
    }
}
