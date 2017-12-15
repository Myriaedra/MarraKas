using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PlayerController : MonoBehaviour {

    Camera cam;
    CamController camController;
    Rigidbody rb;

    public float groundAcceleration;
    public float sprintAcceleration;
    public float airAcceleration;
    public float glideAcceleration;
    public float sprintSpeed;
    public float maxSpeed;
    public float glideSpeed;
    public float climbAcceleration;
    public float climbMaxSpeed;
	public float jumpForce;

    bool landed = false;
    bool gliding = true;
    bool climbing = false;

    Vector3 startPosition;

    public PostProcessingProfile postProcess;
    public ParticleSystem speedParticleSystem;

    public BarkManagement barkManagement;

	// Use this for initialization
	void Start ()
    {
        cam = Camera.main;
        camController = cam.GetComponent<CamController>();
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }
	
    void Update()//--------------------------------------------------------------------------------------------------------------------------
    {
        //DEBUG RESTART POSITION----------------
        if (Input.GetKeyDown(KeyCode.A))
            transform.position = startPosition;

        //JUMP MANAGEMENT
		if (Input.GetButtonDown ("Jump") && IsGrounded ()) //Normal jump
			rb.velocity = new Vector3 (rb.velocity.x, jumpForce, rb.velocity.z);
		/*else if (Input.GetButtonDown ("Jump") && climbing && !IsGrounded()) //Wall Jump
		{
			climbing = false;
			print (transform.forward);
			rb.velocity = new Vector3 (-transform.forward.x*15, jumpForce, -transform.forward.z*15); //Opposite direction
			transform.Rotate(new Vector3 (0, 180, 0));
			print (rb.velocity);
			camController.Invoke("SnapBack", 0.5f);
		}*/

        /*NOTES TRUC A FAIRE-------------------------------------------------------------------
            Régler le feel de l'orientation quand on est immobile --> on devrait être capable de tourner sur soi-même pour s'orienter comme on veut
        */
    }

	void FixedUpdate ()//-------------------------------------------------------------------------------------------------------------------
    {
        if (IsGrounded())//MOUVEMENT AU SOL---------------------------------------------------
        {
            if (gliding)
                gliding = false;

            GroundMovement();
        }
        else//MOUVEMENT AERIEN---------------------------------------------------------------------
        {
            AirMovement();
        }
    }

    void OnTriggerStay(Collider other)//TRIGGER STAY------------------------------------------------------
    {
        if (other.tag == "Wall")//CHECK POUR LE WALLJUMP---------------------   À TESTER
        {
			if (Input.GetButtonDown ("Jump") && climbing && !IsGrounded()) //Wall Jump
			{
				climbing = false;
				print (transform.forward);
				rb.velocity = new Vector3 (-transform.forward.x*15, jumpForce, -transform.forward.z*15); //Opposite direction
				transform.Rotate(new Vector3 (0, 180, 0));
				print (rb.velocity);
				camController.Invoke("SnapBack", 0.5f);
			}
        }//-----------------------------------------------------------------
    }

	void OnTriggerEnter(Collider other)//TRIGGER ENTER------------------------------------------------------
	{
		if (other.tag == "Collectable")//CHECK POUR LES COLLECTABLES---------------------------------
		{
			Collectable col = other.GetComponent<Collectable> ();
			if (!col.collected) 
			{
				col.StartCoroutine ("Collected");
			}
		}//-----------------------------------------------------------------------------------
	} 

	void GroundMovement() //FONCTION MVT SOL
    {
        //move
        if (Input.GetAxisRaw("Sprint") < 0.2f)
        {
            Move(groundAcceleration);
            LimitVelocity(maxSpeed);
            OrientCharacter(0.2f);
            FeedbacksManagement(false);
        }
        else
        {
            Move(sprintAcceleration);
            LimitVelocity(sprintSpeed);
            OrientCharacter(0.2f);
            FeedbacksManagement(true);
        }

        if (Input.GetButtonDown("Bark"))
        {
            barkManagement.Bark();
        }
    }

    void AirMovement()//FONCTION MVT AIR
    {

        Move(airAcceleration);

        GravityMod(3.0f);
        LimitVelocity(maxSpeed);
        OrientCharacter(0.05f);

        if(rb.velocity.y<=0 && Input.GetButton("Jump"))
        {
            //gliding = true;
            //rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
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
            float wantedAngle = transform.eulerAngles.y; //Declare
            //Checks which side to turn (shortest way)
            float angle = Vector3.Angle(transform.forward, rb.velocity.normalized);
            Vector3 cross = Vector3.Cross(transform.forward, rb.velocity.normalized);
            if (cross.y < 0)
                angle = -angle;
            wantedAngle = transform.eulerAngles.y + angle;
            //Rotate SMOOTHLY to the right direction
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, wantedAngle, 0), lerpValue);
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
        if (Physics.Raycast(transform.position, -transform.up, 1.2f, layerMask))
        {
            if (!landed)
            {
                landed = true;
                //rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
				rb.velocity = new Vector3(0, 0, 0); //Avoid slipping
            }     
            return true;
        }
        else
        {

            if (landed)
                landed = false;
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
            if (rb.velocity.magnitude > 4.0f)
                speedParticleSystem.enableEmission = true;
        }
        else
        {
            wantedVignetteValue = 0;
            wantedFieldOfView = 60;
            speedParticleSystem.enableEmission = false;
        }
        
        //vignette management---------------------------------------
        var vignette = postProcess.vignette.settings;
        vignette.intensity = Mathf.Lerp(vignette.intensity, wantedVignetteValue, 0.05f);
        postProcess.vignette.settings = vignette;

        //field of view management----------------------------------
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wantedFieldOfView, 0.05f);
    }
}
