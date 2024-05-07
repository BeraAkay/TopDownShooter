using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[DefaultExecutionOrder(1)]
public class PlayerController : MonoBehaviour
{ 
    //Vector3 movementInput;
    //Vector2 mousePosition;

    public float crosshairMaxDistance;

    Rigidbody2D playerRB;

    public GameObject crosshair, crosshairSS;

    public float crossHairDefaultDist;
    public float mouseSensitivity;

    Vector3 cameraOffset;
    Vector3 cameraTarget;
    public float camFollowSpeed;

    InputActionMap actionMap;
    PlayerInput playerInput;


    public Transform rotParts;

    public static PlayerController instance;

    public PlayerInteractions playerInteractions;


    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        Initialize();

        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        CameraFollow();

        AimAndLook();

        //WeaponSwap();

        if (playerInput.actions["Shoot"].ReadValue<float>() != 0)
        {
            playerInteractions.AttackCall(playerInteractions.mainWeapon);
        }
        else if(playerInput.actions["ShootSecondary"].ReadValue<float>() != 0)
        {
            playerInteractions.AttackCall(playerInteractions.secondaryWeapon);
        }

        Initialize();
    }

    void FixedUpdate()
    {
        Movement();
    }

    public void Initialize()
    {
        playerRB = GetComponent<Rigidbody2D>();
        float cameraOffsetZ = Camera.main.transform.position.z - transform.position.z;
        cameraOffset = Vector3.zero;
        cameraOffset.z = cameraOffsetZ;

        playerInteractions = GetComponent<PlayerInteractions>();

        SetUpInputs();
    }

    void SetUpInputs()
    {
        playerInput = GetComponent<PlayerInput>();

        actionMap = playerInput.currentActionMap;
        if(actionMap.name == "Classic")
        {

            //playerInput.actions["ChangeWeapon"].performed += ctx => playerInteractions.ChangeWeapon((int)ctx.ReadValue<float>());
            playerInput.actions["Shoot"].performed += ctx => playerInteractions.AttackCall(playerInteractions.mainWeapon);
            playerInput.actions["ShootSecondary"].performed += ctx => playerInteractions.AttackCall(playerInteractions.secondaryWeapon);

            playerInput.actions["Escape"].performed += ctx => OpenMenu();
        }

    }

    private void OnEnable()
    {
        ToggleInputs(true);
    }

    private void OnDisable()
    {
        ToggleInputs(false);
    }

    void ToggleInputs(bool flag)
    {
        if (flag)
        {
            playerInput.actions.Enable();
        }
        else
        {
            playerInput.actions.Disable();
        }
    }

    void OpenMenu()
    {
        MenuManager.instance.GoToMenu(MenuManager.State.PauseMenu);
        GameManager.instance.Pause(true);
    }

    
    void Movement()
    {
        Vector2 movementInput;
        movementInput = playerInput.actions["Move"].ReadValue<Vector2>();
        if (movementInput.magnitude > 0.1)
        {
            playerRB.velocity = movementInput.normalized * playerInteractions.stats[Stats.Types.MovementSpeed];
        }
        else
        {
            playerRB.velocity = (playerRB.velocity / 2).magnitude > 0.01f ? playerRB.velocity / 2 : Vector2.zero;
        }
        
    }
    void AimAndLook()
    {
        /*
        //RectTransform crosshairRect = crosshairSS.GetComponent<RectTransform>();
        //Vector3 newPos;

        //Vector2 lookInput;

        //FIX THIS MESS PLEASE
        
        {
        lookInput = playerInput.actions["Look"].ReadValue<Vector2>();

        //Vector2 clamp = Camera.main.ViewportToWorldPoint(new Vector3(0.9f, 0.9f, 0));

        //Debug.Log(clamp);
        //float newX = Mathf.Clamp(crosshairRect.anchoredPosition.x + (lookInput.x * mouseSensitivity), -850, 850);// - , + (1920/2 - padvalue(90 pad))
        //float newY = Mathf.Clamp(crosshairRect.anchoredPosition.y + (lookInput.y * mouseSensitivity), -430, 430);// -, + (1080/2 - padvalue(90 pad))

        float crosshairMaxDist = crossHairDefaultDist * 2;

        float newX = Mathf.Clamp(crosshair.transform.localPosition.x + (lookInput.x * mouseSensitivity), -crosshairMaxDist * 1.6f , crosshairMaxDist * 1.6f);
        float newY = Mathf.Clamp(crosshair.transform.localPosition.y + (lookInput.y * mouseSensitivity), -crosshairMaxDist * .9f, crosshairMaxDist * .9f);

        newPos = new Vector2(newX, newY);

        newPos = Vector3.ClampMagnitude(newPos,crosshairMaxDist * 1.6f);//when using mouse the default dist should be more
        }
        */

        Vector3 newPos = Camera.main.ScreenToWorldPoint(playerInput.actions["MousePosition"].ReadValue<Vector2>());

        newPos.z = crosshair.transform.localPosition.z;
        crosshair.transform.localPosition = newPos;
        

        float angle = Vector3.SignedAngle(Vector3.up, crosshair.transform.position - transform.position, new Vector3(0, 0, 1));//Mathf.Atan2(crosshairDelta.y, crosshairDelta.x) * Mathf.Rad2Deg;
        
        Vector3 rotVec = Vector3.zero;
        rotVec.z = angle;
        rotParts.rotation = Quaternion.identity;
        rotParts.Rotate(rotVec);

    }

    void CameraFollow()
    {
        cameraTarget = transform.position + cameraOffset;

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraTarget, Time.deltaTime * camFollowSpeed);
    }


    
}
