//by EvolveGames
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

namespace EvolveGames
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("PlayerController")]
        [SerializeField] public GameObject camOBJ;
        [SerializeField] public Transform Camera;
        [SerializeField] public ItemChange Items;
        [SerializeField, Range(1, 10)] float walkingSpeed = 3.0f;
        [Range(0.1f, 5)] public float CroughSpeed = 1.0f;
        [SerializeField, Range(2, 20)] float RuningSpeed = 4.0f;
        [SerializeField, Range(0, 20)] float jumpSpeed = 6.0f;
        [SerializeField, Range(0.5f, 10)] float lookSpeed = 2.0f;
        [SerializeField, Range(10, 120)] float lookXLimit = 80.0f;
        [Space(20)]
        [Header("Advance")]
        [SerializeField] float RunningFOV = 65.0f;
        [SerializeField] float SpeedToFOV = 4.0f;
        [SerializeField] float CroughHeight = 1.0f;
        [SerializeField] float gravity = 20.0f;
        [SerializeField] float timeToRunning = 2.0f;
        [HideInInspector] public bool canMove = true;
        [HideInInspector] public bool CanRunning = true;
        [SerializeField] private bool useFootsteps = true;

        [Header("Footsteps")]
        [SerializeField] private float baseStepSpeed = 0.4f;
        [SerializeField] private float croughStepSpeed = 1f;
        [SerializeField] private float runStepSpeed = 0.6f;
        [SerializeField] private AudioSource footstepAudio = default;
        [SerializeField] private AudioClip[] rockStep = default;
        [SerializeField] private AudioClip[] grassStep = default;
        [SerializeField] private AudioClip[] dirtStep = default;
        [SerializeField] private AudioClip[] metalStep = default;
        [SerializeField] private AudioClip[] woodStep = default;
        [SerializeField] private AudioClip[] jumpSound = default;
        private float footTimer = 0;
        private float GetCurrentOffset => isCrough ? baseStepSpeed * croughStepSpeed : isRunning ? baseStepSpeed * runStepSpeed : baseStepSpeed;

        [Space(20)]
        [Header("Climbing")]
        [SerializeField] bool CanClimbing = true;
        [SerializeField, Range(1, 25)] float Speed = 2f;
        bool isClimbing = false;

        [Space(20)]
        [Header("HandsHide")]
        [SerializeField] bool CanHideDistanceWall = true;
        [SerializeField, Range(0.1f, 5)] float HideDistance = 1.5f;
        [SerializeField] int LayerMaskInt = 1;

        [Space(20)]
        [Header("Input")]
        [SerializeField] KeyCode CroughKey = KeyCode.LeftControl;


        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Vector3 moveDirection = Vector3.zero;
        [SerializeField] private TMP_Text textP;
        bool isCrough = false;
        float InstallCroughHeight;
        float rotationX = 0;
        [HideInInspector] public bool isRunning = false;
        Vector3 InstallCameraMovement;
        float InstallFOV;
        Camera cam;
        [HideInInspector] public bool Moving;
        [HideInInspector] public float vertical;
        [HideInInspector] public float horizontal;
        [HideInInspector] public float Lookvertical;
        [HideInInspector] public float Lookhorizontal;
        float RunningValue;
        float installGravity;
        bool WallDistance;
        private Vector2 currentInput;
        [HideInInspector] public float WalkingValue;

        [Header("Interactable")]
        //[SerializeField] private GameObject crosshair;
        [SerializeField] private GameObject crosshair2;

        void Start()
        {
            characterController = GetComponent<CharacterController>();
            if (Items == null && GetComponent<ItemChange>()) Items = GetComponent<ItemChange>();
            cam = GetComponentInChildren<Camera>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            InstallCroughHeight = characterController.height;
            InstallCameraMovement = Camera.localPosition;
            InstallFOV = cam.fieldOfView;
            RunningValue = RuningSpeed;
            installGravity = gravity;
            WalkingValue = walkingSpeed;
        }

        void Update()
        {
            RaycastHit hit;
            if (Physics.Raycast(camOBJ.transform.position, camOBJ.transform.forward, out hit, 2))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Interactable"))
                    {
                        if (hit.collider.TryGetComponent(out IShowable showText))
                        {
                            textP.text = showText.value;
                            //crosshair.SetActive(false);
                            crosshair2.SetActive(true);
                        }
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            if (hit.collider.TryGetComponent(out IInteractable interactObject))
                            {
                                interactObject.Interact();
                                Debug.Log("work!");
                            }
                        }

                    }
                }
            }
            else
            {
                //crosshair.SetActive(true);
                crosshair2.SetActive(false);
                textP.text = "";
            }


            RaycastHit CroughCheck;
            RaycastHit ObjectCheck;

            if (useFootsteps)
            {
                footSteps();
            }


            if (!characterController.isGrounded && !isClimbing)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            isRunning = !isCrough ? CanRunning ? Input.GetKey(KeyCode.LeftShift) : false : false;
            currentInput = new Vector2((isRunning ? RunningValue : WalkingValue) * Input.GetAxis("Vertical"), (isRunning ? RunningValue : WalkingValue) * Input.GetAxis("Horizontal"));
            vertical = canMove ? (isRunning ? RunningValue : WalkingValue) * Input.GetAxis("Vertical") : 0;
            horizontal = canMove ? (isRunning ? RunningValue : WalkingValue) * Input.GetAxis("Horizontal") : 0;
            if (isRunning) RunningValue = Mathf.Lerp(RunningValue, RuningSpeed, timeToRunning * Time.deltaTime);
            else RunningValue = WalkingValue;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * vertical) + (right * horizontal);

            if (Input.GetButton("Jump") && canMove && characterController.isGrounded && !isClimbing)
            {
                moveDirection.y = jumpSpeed;
                footstepAudio.PlayOneShot(jumpSound[Random.Range(0, jumpSound.Length - 1)]);
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }
            characterController.Move(moveDirection * Time.deltaTime);
            Moving = horizontal < 0 || vertical < 0 || horizontal > 0 || vertical > 0 ? true : false;

            if (Cursor.lockState == CursorLockMode.Locked && canMove)
            {
                Lookvertical = -Input.GetAxis("Mouse Y");
                Lookhorizontal = Input.GetAxis("Mouse X");

                rotationX += Lookvertical * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                Camera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Lookhorizontal * lookSpeed, 0);

                if (isRunning && Moving) cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, RunningFOV, SpeedToFOV * Time.deltaTime);
                else cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, InstallFOV, SpeedToFOV * Time.deltaTime);
            }

            if (Input.GetKey(CroughKey))
            {
                isCrough = true;
                float Height = Mathf.Lerp(characterController.height, CroughHeight, 5 * Time.deltaTime);
                characterController.height = Height;
                WalkingValue = Mathf.Lerp(WalkingValue, CroughSpeed, 6 * Time.deltaTime);

            }
            else if (!Physics.Raycast(GetComponentInChildren<Camera>().transform.position, transform.TransformDirection(Vector3.up), out CroughCheck, 0.8f, 1))
            {
                if (characterController.height != InstallCroughHeight)
                {
                    isCrough = false;
                    float Height = Mathf.Lerp(characterController.height, InstallCroughHeight, 6 * Time.deltaTime);
                    characterController.height = Height;
                    WalkingValue = Mathf.Lerp(WalkingValue, walkingSpeed, 4 * Time.deltaTime);
                }
            }

            if (WallDistance != Physics.Raycast(GetComponentInChildren<Camera>().transform.position, transform.TransformDirection(Vector3.forward), out ObjectCheck, HideDistance, LayerMaskInt) && CanHideDistanceWall)
            {
                WallDistance = Physics.Raycast(GetComponentInChildren<Camera>().transform.position, transform.TransformDirection(Vector3.forward), out ObjectCheck, HideDistance, LayerMaskInt);
                Items.ani.SetBool("Hide", WallDistance);
                Items.DefiniteHide = WallDistance;
            }
        }



        private void footSteps()
        {
            if (!characterController.isGrounded) return;
            if (currentInput == Vector2.zero) return;

            footTimer -= Time.deltaTime;

            if (footTimer <= 0)
            {
                //footstepAudio.pitch = Random.Range(.9f, 1.2f);
                if (Physics.Raycast(characterController.transform.position, Vector3.down, out RaycastHit hit, 2))
                {
                    switch (hit.collider.tag)
                    {
                        case "Footstep/Rock":
                            footstepAudio.PlayOneShot(rockStep[Random.Range(0, rockStep.Length - 1)]);
                            break;
                        case "Footstep/GRASS":
                            footstepAudio.PlayOneShot(grassStep[Random.Range(0, grassStep.Length - 1)]);
                            break;
                        case "Footstep/DIRTY":
                            footstepAudio.PlayOneShot(dirtStep[Random.Range(0, dirtStep.Length - 1)]);
                            break;
                        case "Footstep/METAL":
                            footstepAudio.PlayOneShot(metalStep[Random.Range(0, metalStep.Length - 1)]);
                            break;
                        case "Footstep/WOOD":
                            footstepAudio.PlayOneShot(woodStep[Random.Range(0, woodStep.Length - 1)]);
                            break;
                        default:
                            footstepAudio.PlayOneShot(rockStep[Random.Range(0, rockStep.Length - 1)]);
                            break;
                    }
                }

                footTimer = GetCurrentOffset;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Ladder" && CanClimbing)
            {
                CanRunning = false;
                isClimbing = true;
                WalkingValue /= 2;
                Items.Hide(true);
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Ladder" && CanClimbing)
            {
                moveDirection = new Vector3(0, Input.GetAxis("Vertical") * Speed * (-Camera.localRotation.x / 1.7f), 0);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Ladder" && CanClimbing)
            {
                CanRunning = true;
                isClimbing = false;
                WalkingValue *= 2;
                Items.ani.SetBool("Hide", false);
                Items.Hide(false);
            }
        }

    }
}