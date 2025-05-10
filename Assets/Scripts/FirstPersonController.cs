using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]

public class FirstPersonController : MonoBehaviour
{
    private float moveSpeed = 7.5f;
    private float jumpHeight = 3f;
    private float gravity = -45f;
    public float mouseSensitivity = 2f;
    public Transform cameraTransform;
    private float spikeDetectionRadius = 2f;
    public LayerMask hiddenSpikeMask;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;
    private bool isGrounded;
    private Vector3 startPosition;
    private GameObject rotatingPlatforms;

    public float groundCheckDistance = 0.4f;
    public LayerMask groundMask;
    private Transform groundCheck;

    public delegate void PlayerDeathEvent();
    public static event PlayerDeathEvent OnPlayerDeath;
    public bool isInputEnabled = true;


    private GameObject spikeWall;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        startPosition = transform.position;
        rotatingPlatforms = GameObject.FindGameObjectWithTag("RotatingPlatform");
        spikeWall = GameObject.FindGameObjectWithTag("SpikeWall");
        _Start();
    }

    void _Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        startPosition = transform.position;

        GameObject groundCheckObj = new GameObject("GroundCheck");
        groundCheckObj.transform.SetParent(transform);
        groundCheckObj.transform.localPosition = new Vector3(0, -1f, 0);
        groundCheck = groundCheckObj.transform;

        if (rotatingPlatforms != null)
        {
            rotatingPlatforms.GetComponent<RotatingSpikes>().Deactivate();
        }

        if (spikeWall != null)
        {
            spikeWall.GetComponent<SpikeWall>().Deactivate();
        }

        GameObject[] hiddenSpikes = GameObject.FindGameObjectsWithTag("HiddenSpike");
        foreach (GameObject spike in hiddenSpikes)
        {
            MeshRenderer renderer = spike.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.enabled = false;
            }
        }

    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        CheckHiddenSpikes();
        CheckSpikeWall();
        if (transform.position.y < -15f)
        {
            Die();
        }
        checkWetherPlayerHasPassedTheLevelOrHeIsStillPlayingThisStupidGame();
    }

    void checkWetherPlayerHasPassedTheLevelOrHeIsStillPlayingThisStupidGame()
    {
        if (transform.position.z >= 21.5f && transform.position.y >= 4.5f && rotatingPlatforms != null)
        {
            SceneManager.LoadScene("Level2");
        }
        if (transform.position.z >= 28.5f && transform.position.y >= -0.5f && spikeWall != null)
        {
            SceneManager.LoadScene("Level3");
        }
        if (transform.position.x >= -5 && transform.position.x <= -3.5f && transform.position.y >= -0.5f && transform.position.z <= 0.5f && spikeWall == null && rotatingPlatforms == null)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    void CheckSpikeWall()
    {
        if (spikeWall == null)
            return;

        if (transform.position.z > 21)
        {
            spikeWall.GetComponent<SpikeWall>().Activate();
        }
    }

    void HandleMouseLook()
    {
        if (!isInputEnabled) return;
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        if (!isInputEnabled) return;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void CheckHiddenSpikes()
    {
        if (Physics.CheckSphere(transform.position, spikeDetectionRadius, hiddenSpikeMask))
        {
            GameObject[] hiddenSpikes = GameObject.FindGameObjectsWithTag("HiddenSpike");
            foreach (GameObject spike in hiddenSpikes)
            {
                MeshRenderer renderer = spike.GetComponent<MeshRenderer>();
                if (renderer != null && !renderer.enabled)
                {
                    float distanceToSpike = Vector3.Distance(transform.position, spike.transform.position);
                    if (distanceToSpike <= 2 * spikeDetectionRadius)
                    {
                        renderer.enabled = true;
                        if (rotatingPlatforms != null)
                        {
                            rotatingPlatforms.GetComponent<RotatingSpikes>().Activate();
                        }
                    }
                }
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("Spike") ||
            hit.gameObject.layer == LayerMask.NameToLayer("HiddenSpike"))
        {
            Die();
        }
    }

    void Die()
    {
        controller.enabled = false;
        transform.position = startPosition;
        velocity = Vector3.zero;
        controller.enabled = true;

        _Start();

        OnPlayerDeath?.Invoke();
    }
}
