// Player3DController.cs - MovementState Entegrasyonlu Versiyon
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player3DController : MonoBehaviour
{
    public enum MovementState
    {
        Idle,
        Walking,
        Running,
        Crouching,
        Jumping,
        Falling
    }

    [Header("Movement Settings")]
    public float speed = 5.0f;
    public float sprintSpeed = 7f;
    public float crouchSpeed = 2.5f;
    public float jumpForce = 5.0f;
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;

    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float stamina = 100f;
    public float staminaDrain = 20f;
    public float staminaGain = 10f;
    public Image staminaImage;

    [Header("Camera & Animator")]
    public Transform playerCam;

    [Header("UI & Map")]
    public GameObject miniMap;
    public GameObject bigMap;
    public GameObject escMenu;
    public TextMeshProUGUI timerText;
    public Animator timerTextPanel;

    private Rigidbody rb;
    private bool isGrounded = true;
    private bool isCrouching = false;
    private bool bigMapActive = false;
    private bool isTimerRunning = false;
    private float timeElapsed = 0f;
    private Coroutine staminaRegenTimer;
    private PlatfromScript currentPlatform = null;
    private GameData data;
    private bool wasSprinting = false;
    private float coyoteTimer = 0f;
    private float jumpBufferTimer = 0f;

    public bool canMove = true;

    [SerializeField] private MovementState currentState = MovementState.Idle;
    public MovementState CurrentState => currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        data = DataManager.Instance.LoadData();
    }

    void Update()
    {
        UpdateStaminaBar();
        HandleESC();
        HandleMapToggle();
        UpdateMovementState();
        if (!canMove) return;

        HandleMovement();
        HandleJump();
        HandleCrouch();
        HandleSprint();

        

        if (isTimerRunning)
        {
            timeElapsed += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    void HandleMovement()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        Vector3 move = transform.right * input.x + transform.forward * input.z;
        rb.MovePosition(rb.position + move * speed * Time.deltaTime);

        if (currentPlatform != null)
            rb.MovePosition(rb.position + currentPlatform.deltaPos);
    }

    void HandleJump()
    {
        // Timer güncellemeleri
        if (isGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;

        if (Input.GetButtonDown("Jump"))
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= Time.deltaTime;

        // Zýplama koþulu
        if (jumpBufferTimer > 0 && coyoteTimer > 0 && !isCrouching && stamina >= 20f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            if (stamina > 30)
                stamina -= 30f;
            else
                stamina -= stamina;
            isGrounded = false;
            coyoteTimer = 0;
            jumpBufferTimer = 0;

            StopAllCoroutines();
            staminaRegenTimer = null;
            StartStaminaRegen();
        }
    }


    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = true;
            speed = crouchSpeed;
            jumpForce = 0f;
            playerCam.localPosition += Vector3.down * 0.65f;
            GetComponent<CapsuleCollider>().height = 1f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouching = false;
            speed = 5f;
            jumpForce = 5f;
            playerCam.localPosition += Vector3.up * 0.65f;
            GetComponent<CapsuleCollider>().height = 1.8f;
        }
    }

    void HandleSprint()
    {
        bool sprinting = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && stamina > 0 && !isCrouching;

        if (sprinting)
        {
            speed = sprintSpeed;
            stamina -= staminaDrain * Time.deltaTime;

            if (staminaRegenTimer != null)
            {
                StopCoroutine(staminaRegenTimer);
                staminaRegenTimer = null;
            }
        }
        else
        {
            speed = 5f;

            if (wasSprinting && stamina < maxStamina && staminaRegenTimer == null)
            {
                staminaRegenTimer = StartCoroutine(RegenerateStamina());
            }
        }

        wasSprinting = sprinting;
    }

    void StartStaminaRegen()
    {
        if (staminaRegenTimer == null)
            staminaRegenTimer = StartCoroutine(RegenerateStamina());
    }

    IEnumerator RegenerateStamina()
    {
        yield return new WaitForSeconds(1f);
        while (stamina < maxStamina)
        {
            stamina += staminaGain * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0, maxStamina);
            yield return null;
        }
        staminaRegenTimer = null;
    }

    void HandleMapToggle()
    {
        if (GameManager.Instance.GetCurrentLevel() == "MazeLevelEasy" || GameManager.Instance.GetCurrentLevel() == "MazeLevelMid" || GameManager.Instance.GetCurrentLevel() == "MazeLevelHard")
            miniMap.SetActive(!bigMapActive);
        else
            miniMap.SetActive(false);

        if (isTimerRunning && bigMapActive)
        {
            timeElapsed += Time.deltaTime;
            UpdateTimerDisplay();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            bigMapActive = true;
            bigMap.SetActive(true);
            miniMap.SetActive(false);
            canMove = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else if (bigMapActive && Input.anyKeyDown)
        {
            bigMapActive = false;
            bigMap.SetActive(false);
            miniMap.SetActive(true);
            canMove = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void HandleESC()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canMove)
        {
            bool menuOpen = escMenu.activeSelf;
            escMenu.SetActive(!menuOpen);
            canMove = menuOpen;
            Cursor.lockState = menuOpen ? CursorLockMode.Locked : CursorLockMode.Confined;
            GameManager.Instance.gameState = menuOpen ? GameManager.GameState.InGame : GameManager.GameState.Paused;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;

        if (collision.gameObject.TryGetComponent(out PlatfromScript platform))
            currentPlatform = platform;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlatfromScript>() != null)
            currentPlatform = null;
    }

    void UpdateMovementState()
    {
        if (!isGrounded)
        {
            currentState = rb.velocity.y > 0.1f ? MovementState.Jumping : MovementState.Falling;
        }
        else if (isCrouching)
        {
            currentState = MovementState.Crouching;
        }
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            currentState = MovementState.Running;
        }
        else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            currentState = MovementState.Walking;
        }
        else
        {
            currentState = MovementState.Idle;
        }
    }

    void UpdateStaminaBar()
    {
        staminaImage.fillAmount = stamina / maxStamina;
    }

    public void OnTeleport()
    {
        timerTextPanel.SetBool("isActive", true);
        isTimerRunning = true;
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void ExitLevel()
    {
        isTimerRunning = false;
        timerTextPanel.SetBool("isActive", false);
        timerText.SetText("00:00");

        if (GameManager.Instance.GetCurrentLevel() == "MazeLevelEasy" && (data.MazeLevelEasy> timeElapsed || data.MazeLevelEasy == 0))
            data.MazeLevelEasy = timeElapsed;

        if (GameManager.Instance.GetCurrentLevel() == "MazeLevelMid" && (data.MazeLevelMid > timeElapsed || data.MazeLevelMid == 0))
            data.MazeLevelMid = timeElapsed;

        if (GameManager.Instance.GetCurrentLevel() == "MazeLevelHard" && (data.MazeLevelHard > timeElapsed || data.MazeLevelHard == 0))
            data.MazeLevelHard = timeElapsed;

        if (GameManager.Instance.GetCurrentLevel() == "PlatformLevelEasy" && (data.PlatformEasyLevel == 0 || data.PlatformEasyLevel > timeElapsed))
            data.PlatformEasyLevel = timeElapsed;

        if (GameManager.Instance.GetCurrentLevel() == "PlatformLevelHard" && (data.PlatformHardLevel == 0 || data.PlatformHardLevel > timeElapsed))
            data.PlatformHardLevel = timeElapsed;

        timeElapsed = 0f;
        GameManager.Instance.SetCurrentLevel(0);
        DataManager.Instance.SaveData(data);
    }

    public void OnDeath() => timeElapsed = 0;

    public void MainMenu()
    {
        DataManager.Instance.SaveData(data);
        GameManager.Instance.MainMenu();
    }

    public void ExitGame()
    {
        DataManager.Instance.SaveData(data);
        Application.Quit();
    }

    public void ResumeGame()
    {
        GameManager.Instance.gameState = GameManager.GameState.InGame;
        canMove = true;
        escMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Settings()
    {
        GameManager.Instance.SettingsMenu();
    }

    public void RespawnOnCheckpoint(Transform checkpoint)
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        transform.position = checkpoint.position;
    }

    public void SetCanMove(bool input) => canMove = input;

    public void SetMenuActive(bool input)
    {
        miniMap.SetActive(input);
    }
}
