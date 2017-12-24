using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller : MonoBehaviour {

	[System.Serializable]
	public class MoveSettings {
		public float forwardVelocity = 8;
		public float rotateVelocity = 100;
		public float jumpVelocity = 8;
		public float distanceToGround = 0.5f;
		public LayerMask ground;
	}

	[System.Serializable]
	public class PhysicsSettings {
		public float downAcceleration = 0.75f;
	}

	[System.Serializable]
	public class InputSettings {
		public float inputDelay = 0.1f;		//Delay para que el personaje se mueva un poco después de presionar la tecla
		public string FORWARD_AXIS = "Vertical";
		public string TURN_AXIS = "Horizontal";
		public string JUMP_AXIS = "Jump";
	}

	public MoveSettings moveSettings = new MoveSettings();
	public PhysicsSettings physicsSettings = new PhysicsSettings();
	public InputSettings inputSettings = new InputSettings();

	private Vector3 velocity = Vector3.zero;
	private float forwardInput;
	private float turnInput;
	private float jumpInput;
	private Quaternion currentRotation;
	private Rigidbody rigidBody;

	public Quaternion CurrentRotation {
		get { return currentRotation; }
	}

	bool Grounded() {
		return Physics.Raycast (transform.position, Vector3.down, moveSettings.distanceToGround, moveSettings.ground);
	}

	void Start() {
		currentRotation = transform.rotation;
		if (GetComponent<Rigidbody> ())
			rigidBody = GetComponent<Rigidbody> ();
		else
			Debug.LogError ("El personaje necesita un rigidbody.");

		forwardInput = 0;
		turnInput = 0;
		jumpInput = 0;
	}

	void GetInput() {
		forwardInput = Input.GetAxis (inputSettings.FORWARD_AXIS);
		turnInput = Input.GetAxis (inputSettings.TURN_AXIS);
		jumpInput = Input.GetAxisRaw (inputSettings.JUMP_AXIS);
	}

	void Update() {
		GetInput ();
		Turn ();
	}

	void FixedUpdate() {
		Run ();
		Jump ();
		rigidBody.velocity = transform.TransformDirection(velocity);
	}

	void Run() {
		if (Mathf.Abs (forwardInput) > inputSettings.inputDelay) {
			velocity.z = forwardInput * moveSettings.forwardVelocity;
		} else
			velocity.z = 0;
	}

	void Turn() {
		if (Mathf.Abs (turnInput) > inputSettings.inputDelay) {
			currentRotation *= Quaternion.AngleAxis (moveSettings.rotateVelocity * turnInput * Time.deltaTime, Vector3.up); 
		}

		transform.rotation = currentRotation;
	}

	void Jump() {

		if ((jumpInput > 0 && Grounded ()) || (jumpInput > 0 && !Grounded ())) {
			
			velocity.y = moveSettings.jumpVelocity;

		} else if (jumpInput == 0 && Grounded ()) {
			
			velocity.y = 0;

		} else {

			velocity.y -= physicsSettings.downAcceleration;
		}
	}
}
