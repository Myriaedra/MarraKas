using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using Cinemachine;
using Vuforia;
using UnityEditor;

public class PlayerController : MonoBehaviour {

	public static PlayerController pc;
	public static bool controlsAble = true;

    public  CinemachineFreeLook freeLookCM;
	public SkinnedMeshRenderer meshRenderer;
	public PostProcessingProfile postProcess;
	public BarkManagement barkManagement;
	public Animator anim;

	public Transform spawnerSeaPart;
	public GameObject seaPartBurst;
	public ParticleSystem seaPartOverTime;
	public ParticleSystem seaPartOverTimeSprint;
	bool inWater = false;

	[HideInInspector]
	public GameObject beingTalkedTo;

	Camera cam;
	CamController camController;
	Rigidbody rb;
	[HideInInspector]
	public bool landed = false;

    string feedbackCoroutineRunning = "Nothing";

	public AudioSource aS;
	AudioClip usedStep;
	public AudioClip groundStep;
	public AudioClip waterStep;
	public AudioClip splashSFX;

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

    [Header("Feedbacks Values : ")]
    public float vignetteDefaultValue;
    public float vignetteSprintValue;
    public float fovDefaultValue;
    public float fovSprintValue;

    [Header("OtherValues : ")]
    public float jumpForce;

	// Use this for initialization
	void Start ()
	{
		seaPartOverTimeSprint.Stop();		
		cam = Camera.main;
		camController = cam.GetComponent<CamController>();
		rb = GetComponent<Rigidbody>();
		pc = GetComponent<PlayerController> ();
		usedStep = groundStep;
	}
    
	void Update()//JUMP MANAGEMENT--------------------------------------------------------------------------------------------------------------------------
	{
        //JUMP MANAGEMENT
        if (Input.GetButtonDown("Jump") && IsGrounded() && controlsAble)
        {
			anim.SetTrigger ("JumpTrigger");
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }	
	}

	void FixedUpdate ()//AIR OR GROUND MOVEMENT --> Check ControlsAble------------------------------------------------------------------------------------
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
	} //PERSONAL DRAG

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
			anim.speed = 1.4f;
		}
		else
		{
			Move(groundSprintAcceleration);
			LimitVelocity(groundSprintMaxSpeed);
			OrientCharacter(0.2f);
			FeedbacksManagement(true);
			Drag(groundSprintDragValue);
			anim.speed = 2;
		}
		anim.SetFloat ("Speed", rb.velocity.magnitude);


		if(inWater){
			if (!seaPartOverTime.isPlaying && rb.velocity.magnitude > 0.5f)
				seaPartOverTime.Play ();
			else if (seaPartOverTime.isPlaying && rb.velocity.magnitude < 0.5f)
				seaPartOverTime.Stop ();
			if (Input.GetAxisRaw ("Sprint") < 0.2f) {
				seaPartOverTimeSprint.Stop ();
			}
			else{
				seaPartOverTimeSprint.Play ();				
			}
		}
		else if(!inWater && seaPartOverTime.isPlaying){
			seaPartOverTime.Stop ();
			seaPartOverTimeSprint.Stop ();
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
            FeedbacksManagement(false);
        }
        else
        {
            Move(airSprintAcceleration);
            GravityMod(3.0f);
            LimitVelocity(airSprintMaxSpeed);
            OrientCharacter(0.05f);
            Drag(airSprintDragValue);
            FeedbacksManagement(true);
		}
		anim.SetFloat ("Speed", rb.velocity.magnitude);
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

		Debug.DrawRay (position1+transform.right*0.3f, -transform.up);
		Debug.DrawRay (position2+transform.right*0.3f, -transform.up);
		Debug.DrawRay (position3+transform.right*0.3f, -transform.up);
		Debug.DrawRay (position1-transform.right*0.3f, -transform.up);
		Debug.DrawRay (position2-transform.right*0.3f, -transform.up);
		Debug.DrawRay (position3-transform.right*0.3f, -transform.up);
		//Raycasts !
		if (Physics.Raycast(position1+transform.right*0.3f, -transform.up, 1.2f, layerMask) 
			|| Physics.Raycast(position2+transform.right*0.3f, -transform.up, 1.2f, layerMask) 
			|| Physics.Raycast(position3+transform.right*0.3f, -transform.up, 1.2f, layerMask)
			|| Physics.Raycast(position1-transform.right*0.3f, -transform.up, 1.2f, layerMask)
			|| Physics.Raycast(position2-transform.right*0.3f, -transform.up, 1.2f, layerMask)
			|| Physics.Raycast(position3-transform.right*0.3f, -transform.up, 1.2f, layerMask))
		{
            if (!landed)
			{
				anim.SetTrigger ("JumpOverTrigger");
				landed = true;
				//rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
				//rb.velocity = new Vector3(rb.velocity.x/4, 0, rb.velocity.z/4); //Avoid slipping
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
		//float wantedVignetteValue;
		if (sprintOrNot && feedbackCoroutineRunning!="Sprint")
		{
            StopAllCoroutines();
            if (freeLookCM.m_Lens.FieldOfView != fovSprintValue)
            {
                StartCoroutine(feedbacksSprint(true));
            }
		}
		else if(!sprintOrNot && feedbackCoroutineRunning!="Default")
		{
            StopAllCoroutines();
            if(freeLookCM.m_Lens.FieldOfView!= fovDefaultValue)
            {
                StartCoroutine(feedbacksSprint(false));
            }
		}

		//vignette management---------------------------------------
		//var vignette = postProcess.vignette.settings;
		//vignette.intensity = Mathf.Lerp(vignette.intensity, wantedVignetteValue, 0.05f);
		//postProcess.vignette.settings = vignette;

    }

    IEnumerator feedbacksSprint(bool sprint)
    {
        var vignette = postProcess.vignette.settings;
        float startVignette = vignette.intensity;

        float startFov = freeLookCM.m_Lens.FieldOfView;

        if (sprint)
        {
            feedbackCoroutineRunning = "Sprint";
            for (int i = 0; i < 50; i++)
            {
                float newLerpValue = i * 0.02f + 0.02f;

                freeLookCM.m_Lens.FieldOfView = Mathf.Lerp(startFov, fovSprintValue, newLerpValue);
                vignette.intensity = Mathf.Lerp(startVignette, vignetteSprintValue, newLerpValue);
                postProcess.vignette.settings = vignette;

                if (i == 49)
                {
                    feedbackCoroutineRunning = "Nothing";
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            feedbackCoroutineRunning = "Default";
            for (int i = 0; i < 50; i++)
            {
                float newLerpValue = i * 0.02f + 0.02f;

                freeLookCM.m_Lens.FieldOfView = Mathf.Lerp(startFov, fovDefaultValue, newLerpValue);
                vignette.intensity = Mathf.Lerp(startVignette, vignetteDefaultValue, newLerpValue);
                postProcess.vignette.settings = vignette;

                if (i == 49)
                {
                    feedbackCoroutineRunning = "Nothing";
                }

                yield return new WaitForSeconds(0.01f);
            }
        }
    }

	public void SetRenderer(bool value)
	{
		meshRenderer.enabled = value;
	}

	void OnCollisionEnter(Collision other){
		if(other.collider.tag == "Water"){
			GameObject waterBurst = Instantiate (seaPartBurst, spawnerSeaPart.position, Quaternion.Euler (-90, 0, 0));
			aS.PlayOneShot (splashSFX);
			Destroy (waterBurst, 2);
			inWater = true;
			usedStep = waterStep;
		}
	}

	void OnCollisionExit(Collision other){
		if(other.collider.tag == "Water"){
			GameObject waterBurst = Instantiate (seaPartBurst, spawnerSeaPart.position, Quaternion.Euler (-90, 0, 0));
			Destroy (waterBurst, 2);
			inWater = false;
			usedStep = groundStep;
		}
	}
		
	public void Step()
	{
		aS.pitch = Random.Range (0.95f, 1.05f);
		aS.PlayOneShot (usedStep);
	}
}
