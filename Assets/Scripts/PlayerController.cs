using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public enum Animal
    {
        Dog, Cat
    }

    public Animal animal;

    public Transform cam;
    public ParticleSystem runEffect;
    public GameObject heldObject;
    public Transform mouthPosition;
    [SerializeField] private LayerMask pickUpLayerMask;
    public float grabDistance = 1;
    public float releaseDistance = 1;
    
    [Header("Player Movement")]
    public float speed = 10f;
    public float jumpHeight = 10;
    public float fallAcceleration = -0.05f;
    public float terminalVelocity = -40;
    public float turnSmoothTime = 0.1f;

    [Space(5)]
    [Header("Dog Stuff")]
    public DogBark dogBarkPrefab;
    public float dogBarkWindUpTime = 0.33f;
    public float dogBarkRecoveryTime = 0.7f;
    public float dogBarkSize = 5;
    public float dogBarkDuration = 1.5f;

    [Space(5)]
    [Header("Cat Stuff")]
    public float parabolicTurnSpeed = 5;
    public float parabolicWindUpDuration = 0.5f;

    [HideInInspector]public CharacterController cc;
    [HideInInspector]public Transform parabolicJumpTarget;
    [HideInInspector]public float parabolicJumpHeight;
    [HideInInspector]public float parabolicJumpSpeed;
    [HideInInspector] public PickableObject pickedObjectRef;
    [HideInInspector] public bool isHoldingItem = false;
    [HideInInspector]public bool coroutinePause;


    float turnSmoothVelocity;
    float airSpeed;
    float particleTimer = 0;

    bool heldObjectColliderEnabled;
    bool heldObjectIsKinematic;

    PlayerController partner;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();

        PlayerController[] playerControllers = GameObject.FindObjectsOfType<PlayerController>();

        foreach (PlayerController pc in playerControllers)
        {
            if ((animal == Animal.Dog && pc.animal == Animal.Cat) || (animal == Animal.Cat && pc.animal == Animal.Dog))
            {
                partner = pc;
                break;
            }
        }
    }

    private void Update()
    {
        if (!coroutinePause)
        {
            AirPhysics();
            TryGrab();
            switch (animal)
            {
                case Animal.Dog:
                    Debug.DrawRay(mouthPosition.position, mouthPosition.forward, Color.green);
                    DogBark();
                    break;

                case Animal.Cat:
                    CatJump();
                    break;
            }
            Movement();
        }
    }

    private void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            SpawnParticle();
            cc.Move(new Vector3(moveDir.x * speed * Time.deltaTime, airSpeed, moveDir.z * speed * Time.deltaTime));
        }

        else
            cc.Move(new Vector3(0, airSpeed, 0));
    }

    private void SpawnParticle()
    {
        particleTimer += Time.deltaTime;
        if (particleTimer > 0.1f && cc.isGrounded)
        {
            Instantiate(runEffect, new Vector3(transform.position.x, transform.position.y - GetComponent<Collider>().bounds.size.y/2, transform.position.z), transform.rotation);
            particleTimer = 0;
        }
    }

    private void AirPhysics()
    {
        if (cc.isGrounded)
        {
            airSpeed = -0.01f;
            
        }

        else
        {
            airSpeed += fallAcceleration * Time.deltaTime;
            if (airSpeed < terminalVelocity * Time.deltaTime)
                airSpeed = terminalVelocity * Time.deltaTime;
        }
    }

    void TryGrab()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isHoldingItem == false)
            {
                float pickUpDistance = 2f;
                bool hashit = Physics.Raycast(mouthPosition.position, mouthPosition.forward, out RaycastHit hit, grabDistance);
                if (hashit)
                {
                    if (hit.transform.TryGetComponent(out pickedObjectRef))
                    {
                        heldObject = hit.transform.gameObject;
                        pickedObjectRef.isHeld = true;
                        isHoldingItem = true;
                        HoldObject(heldObject, new Vector3(0, 0, 0));
                    }
                }
            }
            else
            {
                RemoveObject(2);
                isHoldingItem = false;
                pickedObjectRef.isHeld = false;
            }
        }
    }

    public void HoldObject(GameObject obj, Vector3 rotationOffset)
    {
        Collider heldObjectCollider = obj.GetComponent<Collider>() ? obj.GetComponent<Collider>() : null;
        Rigidbody heldObjectRigidbody = obj.GetComponent<Rigidbody>() ? obj.GetComponent<Rigidbody>() : null;

        if (heldObjectCollider)
        {
            heldObjectColliderEnabled = heldObjectCollider.enabled == true ? true : false;
            heldObjectCollider.enabled = false;
        }

        if (heldObjectRigidbody)
        {
            heldObjectIsKinematic = heldObjectRigidbody.isKinematic == true ? true : false;
            heldObjectRigidbody.isKinematic = true;
        }

        obj.transform.SetParent(mouthPosition.transform, true);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.eulerAngles = transform.eulerAngles + rotationOffset;
        
        heldObject = obj;
    }

    public void RemoveObject(float removeDistance)
    {
        if (heldObject != null)
        {
            Collider heldObjectCollider = heldObject.GetComponent<Collider>();
            Rigidbody heldObjectRigidbody = heldObject.GetComponent<Rigidbody>();

            if (heldObjectCollider)
                heldObjectCollider.enabled = heldObjectColliderEnabled;

            if (heldObjectRigidbody)
                heldObjectRigidbody.isKinematic = heldObjectIsKinematic;

            heldObject.transform.SetParent(null, true);
            heldObject.transform.position = transform.position + transform.forward * removeDistance;
            heldObject = null;
        }
    }

    private void DogBark()
    {
        if (Input.GetButtonDown("Jump"))
        {
            StartCoroutine(Bark());
        }
    }

    private void CatJump()
    {
        if (Input.GetButtonDown("Jump") && cc.isGrounded)
        {
            if (parabolicJumpTarget != null)
                StartCoroutine(ParabolicCatJump());

            else
                airSpeed = jumpHeight / 600; //Time.deltaTime produced random Jump Heights. This is to keep it consistent while being close to the value Time.deltaTime would produce on average
        }
    }

    IEnumerator Bark()
    {
        coroutinePause = true;

        dogBarkPrefab.GetComponent<SphereCollider>().radius = dogBarkSize;
        dogBarkPrefab.GetComponent<DestroySelf>().time = dogBarkDuration;

        yield return new WaitForSeconds(dogBarkWindUpTime);

        Instantiate(dogBarkPrefab, transform.position, transform.rotation);
        //Animation also goes here
        
        yield return new WaitForSeconds(dogBarkRecoveryTime);

        coroutinePause = false;
        yield return null;
    }

    IEnumerator ParabolicCatJump()
    {
        coroutinePause = true;

        float timer = 0;

        while(timer < parabolicWindUpDuration)
        {
            timer += Time.deltaTime;

            // Determine which direction to rotate towards
            Vector3 targetDirection = parabolicJumpTarget.position - transform.position;
            targetDirection.y = 0;

            // The step size is equal to speed times frame time.
            float singleStep = parabolicTurnSpeed * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            // Draw a ray pointing at our target in
            Debug.DrawRay(transform.position, newDirection, Color.red);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);
            yield return new WaitForFixedUpdate();
        }

        Vector3 startPos = transform.position;
        Vector3 endPos = parabolicJumpTarget.position;
        float parabolaTimeline = 0;

        while (parabolaTimeline < 1)
        {
            transform.position = MathParabola.Parabola(startPos, endPos, parabolicJumpHeight, parabolaTimeline);
            parabolaTimeline += Time.deltaTime * parabolicJumpSpeed;
            Debug.Log(parabolaTimeline);
            yield return new WaitForFixedUpdate();
        }

        coroutinePause = false;
        yield return null;
    }
}