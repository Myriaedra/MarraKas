using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PlayerController : MonoBehaviour {

	Camera cam;
	CamController camController;
	Rigidbody rb;
    public static bool controlsAble = true;

    [Header("GroundValues : ")]
    public float groundAcceleration;
    public float groundMaxSpeed;
    public float groundDragValue;

    [Header("GroundSprintValues : ")]
    public float groundSprintAcceleration;
    public float groundSprintMaxSpeed;
    public float groundSprintDragValue;

    [Header("AirValues : ")]
    public float airAcceleration;
    public float airMaxSpeed;
    public float airDragValue;

    [Header("AirSprintValues : ")]
    public float airSprintAcceleration;
    public float airSprintMaxSpeed;
    public float airSprintDragValue;

    [Header("OtherValues : ")]
    public float jumpForce;

	bool landed = false;
	public PostProcessingProfile postProcess;
	public BarkManagement barkManagement;

	// Use this for initialization
	void Start ()
	{
		cam = Camera.main;
		camController = cam.GetComponent<CamController>();
		rb = GetComponent<Rigidbody>();
	}

	void Update()//--------------------------------------------------------------------------------------------------------------------------
	{
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (controlsAble)
                controlsAble = false;
            else
                controlsAble = true;
        }

        //JUMP MANAGEMENT
        if (Input.GetButtonDown("Jump") && IsGrounded() && controlsAble)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
	}

	void FixedUpdate ()//-------------------------------------------------------------------------------------------------------------------
	{
        if (controlsAble)
        {
            if (IsGrounded())//MOUVEMENT AU SOL---------------------------------------------------
            {
                GroundMovement();
            }
            else//MOUVEMENT AERIEN---------------------------------------------------------------------
            {
                AirMovement();
            }
        }
        else
            rb.velocity = Vector3.zero;
	}

	void Drag(float drag){
		Vector3 vel = rb.velocity;
		vel.x *= 1 - drag;
		vel.z *= 1 - drag;
		rb.velocity = vel;
	}

	void GroundMovement() //FONCTION MVT SOL
	{
		//move
		if (Input.GetAxisRaw("Sprint") < 0.2f)
		{
			Move(groundAcceleration);
			LimitVelocity(groundMaxSpeed);
			OrientCharacter(0.2f);
			FeedbacksManagement(false);
            Drag(groundDragValue);
        }
		else
		{
			Move(groundSprintAcceleration);
			LimitVelocity(groundSprintMaxSpeed);
			OrientCharacter(0.2f);
			FeedbacksManagement(true);
            Drag(groundSprintDragValue);
        }
	}

	void AirMovement()//FONCTION MVT AIR
	{
        if (Input.GetAxisRaw("Sprint") < 0.2f)
        {
            Move(airAcceleration);
            GravityMod(3.0f);
            LimitVelocity(airMaxSpeed);
            OrientCharacter(0.05f);
            Drag(airDragValue);
        }
        else
        {
            Move(airSprintAcceleration);
            GravityMod(3.0f);
            LimitVelocity(airSprintMaxSpeed);
            OrientCharacter(0.05f);
            Drag(airSprintDragValue);
        }
	}

	void LimitVelocity(float speedLimit)//FONCTION MAXSPEED
	{
		Vector2 xzVel = new Vector2(rb.velocity.x, rb.velocity.z);
		if (xzVel.magnitude > speedLimit)
		{
			xzVel = xzVel.normalized * speedLimit;
			rb.velocity = new Vector3(xzVel.x, rb.velocity.y, xzVel.y);
		}
	}

	void OrientCharacter(float lerpValue)//FONCTION ORIENTATION PERSO
	{
		//Check joystick input
		float xSpeed = Input.GetAxis("Horizontal");
		float zSpeed = Input.GetAxis("Vertical");
		Vector3 velocityAxis = new Vector3(xSpeed, 0, zSpeed);
		//Convert direction depending on the camera
		velocityAxis = Quaternion.AngleAxis(cam.transform.eulerAngles.y, Vector3.up) * velocityAxis;
		//Actual velocity
		Vector3 hVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

		//If the character is moving
		if (hVelocity.magnitude > 1 && velocityAxis.magnitude > 0)
		{
			Vector3 newVelocity = new Vector3 (rb.velocity.x, 0, rb.velocity.z);
			float wantedAngle = transform.eulerAngles.y; //Declare
			//Checks which side to turn (shortest way)
			float angle = Vector3.Angle(transform.forward, newVelocity.normalized);
			Vector3 cross = Vector3.Cross(transform.forward, newVelocity.normalized);
			if (cross.y < 0)
				angle = -angle;
			wantedAngle = transform.eulerAngles.y + angle;
			//Rotate SMOOTHLY to the right direction
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler (0, wantedAngle, 0), lerpValue);
		}
	}

	void GravityMod(float mod)//FONCTION EXTRA/LESS GRAVITY
	{
		Vector3 GravityForce = (Physics.gravity * mod) - Physics.gravity;
		rb.AddForce(GravityForce);
	}

	void Move(float acceleration)//BASIC MOVEMENT (USED IN OTHER FUNCTIONS)
	{
		float xSpeed = Input.GetAxis("Horizontal");
		float zSpeed = Input.GetAxis("Vertical");
		Vector3 velocityAxis = new Vector3(xSpeed, 0, zSpeed);
		velocityAxis = Quaternion.AngleAxis(cam.transform.eulerAngles.y, Vector3.up) * velocityAxis;
		rb.AddForce(velocityAxis.normalized * acceleration);
	}

	bool IsGrounded()//VERIFIER SI ON EST AU SOL
	{
		//Get PlayerMask
		int layerMask = 1 << 8;
		//On inverse et donc --> get tout sauf playerMask
		layerMask = ~layerMask;
        //Set des différentes positions
        Vector3 position1 = transform.position - transform.forward;
        Vector3 position2 = transform.position;
        Vector3 position3 = transform.position + transform.forward;
        //Raycasts !
        if (Physics.Raycast(position1, -transform.up, 1.0f, layerMask) 
        || Physics.Raycast(position2, -transform.up, 1.0f, layerMask) 
        || Physics.Raycast(position3, -transform.up, 1.0f, layerMask))
		{
            if (!landed)
			{
				landed = true;
				//rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
				rb.velocity = new Vector3(rb.velocity.x/4, 0, rb.velocity.z/4); //Avoid slipping
			}     
			return true;
		}
		else
		{
            if (landed){
				landed = false;
			}
			return false;
		}
	}

	void FeedbacksManagement(bool sprintOrNot)
	{
		float wantedVignetteValue;
		float wantedFieldOfView;
		if (sprintOrNot)
		{
			wantedVignetteValue = 0.45f;
			wantedFieldOfView = 80;
		}
		else
		{
			wantedVignetteValue = 0;
			wantedFieldOfView = 60;
		}

		//vignette management---------------------------------------
		var vignette = postProcess.vignette.settings;
		vignette.intensity = Mathf.Lerp(vignette.intensity, wantedVignetteValue, 0.05f);
		postProcess.vignette.settings = vignette;

		//field of view management----------------------------------
		cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wantedFieldOfView, 0.05f);
	}

	public void PlayerControl( bool value)
	{
		controlsAble = value;
	}
}
