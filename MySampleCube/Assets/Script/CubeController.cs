using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public float MoveSpeed = 10f;
    public float HorizontalSpeed = 10f;
    public float JumpForce = 10f;
    // private bool canMove = true;
    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // ButtonRestart1.onClick.AddListener(RestartGame);
        // ButtonRestart2.onClick.AddListener(RestartGame);
    }

    // public Button ButtonRestart1, ButtonRestart2;

    // void RestartGame()
    // {
    //     SceneManager.LoadScene("SampleScene");
    // }

    // Update is called once per frame
    void Update()
    {
        // if (!canMove) return; // Cegah gerakan setelah game over
        
        // rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, MoveSpeed);
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, MoveSpeed);

        float moveHorizontal = 0;

        if (Input.GetKey(KeyCode.A))
        {
            moveHorizontal = -HorizontalSpeed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveHorizontal = HorizontalSpeed;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }

        rb.linearVelocity = new Vector3(moveHorizontal, rb.linearVelocity.y, MoveSpeed);
        // rb.linearVelocity = new Vector3(moveHorizontal, rb.linearVelocity.y, MoveSpeed);
    }

    // public GameObject PanelWin, PanelLose;  

    // public void StopMoving()
    // {
    //     canMove = false;
    // }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // PanelLose.SetActive(true);
            Debug.LogError("WADUHHHH");
            // StopMoving(); // Nonaktifkan gerakan
        }

        if (collision.gameObject.CompareTag("FinishLine"))
        {
            // StopMoving(); // Nonaktifkan gerakan
            // PanelWin.SetActive(true);
            Debug.LogError("HOREEEEE");
        }
    }
}
