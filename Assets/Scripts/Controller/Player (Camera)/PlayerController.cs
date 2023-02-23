/*********************************************************************************************
* Project: Singularity
* File   : PlayerController
* Date   : 09.02.2022
* Author : Marcel Klein
*
* Controls the player take the input properties from the input manager and uses them for movement and interaction methods.
* 
* History:
*    09.02.2022    MK    Created
*********************************************************************************************/


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameData data = null;

    private CharacterController playerController = null;

    private Camera playerCamera = null;
    
    private GameObject playerId = null;

    [Header("Important Player Values")]
    [SerializeField]
    private float jumpHeight = 2f;
    [SerializeField]
    private float playerSpeed = 10.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField, Range(0f, 0.5f)]
    private float moveSmoothTime = 0.5f;
    [SerializeField]
    [Range(0.1f, 0.3f)]
    private float mouseSmoothTime = 0.2f;
    [SerializeField]
    [Range(0.1f, 1.0f)]
    private float mouseSensitivity = 0.5f;
    [SerializeField]
    private Slider m_mouseSensitivitySlider = null;

    [Header("List For Audio Zones")]
    [SerializeField]
    private List<AudioSource> audioSources = new List<AudioSource>();

    private Vector2 currentDirectionVelocity = Vector2.zero;    
    private Vector2 currentDirection = Vector2.zero;
    private Vector2 currentMouseDelta = Vector2.zero;
    private Vector2 currentMouseDeltaVelocity = Vector2.zero;
    private Vector3 upVector = Vector3.zero;
    private Vector3 movementVelocity = Vector3.zero;
    private float cameraPitch = 0.0f;
    private float velocityY = 0.0f;
    private float timeToDestroyKey = 2.5f;
    private float timeToDestroyGate = 3.5f;
    private float cameraPitchClampUp = 90f;
    private float cameraPitchClampDown = -90f;
    private float jumpMultiplyer = -1.5f;
    private bool groundedPlayer = false;
    private bool terraintToSteep = false;

    private Vector2 moveInput = Vector2.zero;
    public Vector2 MoveInput { get { return moveInput; } }

    private bool lockCursor = true;
    public bool LockCursor { get { return lockCursor; } set { lockCursor = value; } }

    private bool collisionWithKey = false;
    public bool CollisionWithKey { get { return collisionWithKey; } }

    private bool collisionWithGate = false;
    public bool CollisionWithGate { get { return collisionWithGate; } }

    private bool collisionWithGateError = false;
    public bool CollisionWithGateError { get { return collisionWithGateError; } }

    private int singularitiesUsed = 0;
    public int SingularitiesUsed { get => singularitiesUsed; set => singularitiesUsed = value; }


    private void Awake()
    {
        playerController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        collisionWithKey = false;
        collisionWithGate = false;
        playerId = this.gameObject;
        SetMouseSensitivity();
    }
    public void Update()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    private void FixedUpdate()
    {
        UpdateGroundPlayer();
        UpdateJumpPlayer();
        UpdateMouseLook();
        UpdateMovement();
    }

    public void ChangeMouseSensitivity(float _value)
    {
        PlayerPrefs.SetFloat("PlayerMouseSensitivity", _value);
        PlayerPrefs.Save();
        mouseSensitivity = PlayerPrefs.GetFloat("PlayerMouseSensitivity", 0.5f);
    }
    public void SetMouseSensitivity()
    {
        m_mouseSensitivitySlider.value = PlayerPrefs.GetFloat("PlayerMouseSensitivity", 0.5f);
    }

    //Collision with keys and gates
    public void OnTriggerStay(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Key":
                collisionWithKey = true;
                if (InputManager.Instance.Use == 1 && data.KeysGathered < LevelManager.Instance.KeysToCollect)
                {
                    other.gameObject.GetComponent<KeyController>().Tag = "UsedKey";
                    data.KeysGathered++;
                    StartCoroutine(WaitForSecondsAfterKey(timeToDestroyKey, other.gameObject));
                    collisionWithKey = false;
                }
                break;
            case "Gate":
                collisionWithGate = true;
                if (InputManager.Instance.Use == 1 && data.KeysGathered >= LevelManager.Instance.KeysToCollect)
                {
                    other.gameObject.GetComponent<GateController>().Tag = "UsedGate";
                    singularitiesUsed++;
                    collisionWithGate = false;
                    StartCoroutine(WaitForSecondsAfterGate(timeToDestroyGate, other.gameObject));
                }
                if (InputManager.Instance.Use == 1 && data.KeysGathered < LevelManager.Instance.KeysToCollect)
                {
                    collisionWithGateError = true;
                    collisionWithGate = false;
                }
                break;
            default:
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Gate":
                collisionWithGateError = false;
                collisionWithGate = false;
                break;
            default:
                break;
        }
    }


    //Coroutines for destroying keys and gates after time.
    private IEnumerator WaitForSecondsAfterKey(float time, GameObject objectToDestroy)
    {
        yield return new WaitForSeconds(time);
        Destroy(objectToDestroy);
    }
    private IEnumerator WaitForSecondsAfterGate(float time, GameObject objectToDestroy)
    {
        yield return new WaitForSeconds(time);
        Destroy(objectToDestroy);
        LevelManager.Instance.CheckForPlayer(playerId);
    }


    //Check for collision with steep terrain and AI.
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Steepness
        if ((Vector3.Angle(Vector3.up, hit.normal) > playerController.slopeLimit))
        {
            terraintToSteep = true;
        }
        else
        {
            terraintToSteep = false;
        }

        //AI
        /*
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null)
        {
            if (hit.gameObject.tag == "AI" && body != null || !body.isKinematic)
            {
                body.velocity += playerController.velocity;
            }
            else
            {
                return;
            }
        }
        */
    }

    //Respawn
    public void OnTriggerEnter(Collider _other)
    {
        switch (_other.gameObject.tag)
        {
            case "Water":
                //Checking for currently playing audio source and pausing it
                for (int i = 0; i < audioSources.Count; i++)
                {
                    switch (audioSources[i].name)
                    {
                        case "Audio Zone Center":
                            if (audioSources[i].isPlaying)
                            {
                                audioSources[i].Pause();
                            }
                            break;
                        case "Audio Zone Forrest":
                            if (audioSources[i].isPlaying)
                            {
                                audioSources[i].Pause();
                            }
                            break;
                        case "Audio Zone Swamp":
                            if (audioSources[i].isPlaying)
                            {
                                audioSources[i].Pause();
                            }
                            break;
                        case "Audio Zone Canyon":
                            if (audioSources[i].isPlaying)
                            {
                                audioSources[i].Pause();
                            }
                            break;
                        case "Audio Zone Sea":
                            if (audioSources[i].isPlaying)
                            {
                                audioSources[i].Pause();
                            }
                            break;
                        case "Audio Zone Mountains":
                            if (audioSources[i].isPlaying)
                            {
                                audioSources[i].Pause();
                            }
                            break;
                        default:
                            break;
                    }
                }
                playerController.enabled = false;
                transform.position = LevelManager.Instance.FindRandomCharSP().position; 
                playerController.enabled = true;
                break;
            default:
                break;
        }
    }

    //Physic methods
    private void UpdateGroundPlayer()
    {
        groundedPlayer = playerController.isGrounded;
        if (groundedPlayer && upVector.y < 0)
        {
            upVector.y = -0.1f;
        }
    }
    private void UpdateJumpPlayer()
    {

        if (InputManager.Instance.Jumped && groundedPlayer)
        {
            if (!terraintToSteep)
            {
                upVector.y += Mathf.Sqrt(jumpHeight * jumpMultiplyer * gravityValue);
            }
            else if (terraintToSteep)
            {
                upVector.y = 0;
            }
            else
            {
                upVector.y += Mathf.Sqrt(jumpHeight * jumpMultiplyer * gravityValue);
            }
        }
        upVector.y += gravityValue * Time.deltaTime;
        playerController.Move(upVector * Time.deltaTime);
        
    }
    private void UpdateMouseLook()
    {
        Vector2 lookInput = InputManager.Instance.Look;
        Vector2 targetMouseDelta = new Vector2(lookInput.x, lookInput.y);
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, cameraPitchClampDown, cameraPitchClampUp);
        playerCamera.transform.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);   
    }
    private void UpdateMovement()
    {
        moveInput = InputManager.Instance.Move;
        Vector2 targetDirection = new Vector2(moveInput.x, moveInput.y).normalized;
        currentDirection = Vector2.SmoothDamp(currentDirection, targetDirection, ref currentDirectionVelocity, moveSmoothTime);
        if (playerController.isGrounded)
        {
            velocityY = 0f;
            velocityY += gravityValue * Time.deltaTime;
        }
        movementVelocity = (transform.forward * currentDirection.y + transform.right * currentDirection.x) * playerSpeed + Vector3.up * velocityY;
        playerController.Move(movementVelocity * Time.deltaTime);
    }

}
