using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[System.Serializable]
	public class MoveSettings {
		public float forwardVelocity = 0.1f;
		public float rotateVelocity = 100;
		public float jumpVelocity = 4;
		public float distanceToGround = 0.1f;
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

	private IPlayerState playerState;
	private Rigidbody rigidBody;

	public Quaternion CurrentRotation {
		get { return transform.rotation; }
	}

	// Use this for initialization
	void Start () {
		playerState = StandingState.Instance;
		if (GetComponent<Rigidbody> ())
			rigidBody = GetComponent<Rigidbody> ();
		else
			Debug.LogError ("El personaje necesita un rigidbody.");
	}

	// Update is called once per frame
	void Update () {
		playerState.handleInput (this);
		Debug.Log (playerState);
	}

	public void changeState(IPlayerState newState) {
		playerState = newState;
	}

	public void Run() {
		Vector3 currVel = transform.InverseTransformDirection(rigidBody.velocity);
		currVel.z = Input.GetAxis (inputSettings.FORWARD_AXIS) * moveSettings.forwardVelocity;
		rigidBody.velocity = transform.TransformDirection(currVel);
	}

	public void turnRight() {
		transform.Rotate (Vector3.up * Time.deltaTime * moveSettings.rotateVelocity);
	}

	public void turnLeft() {
		transform.Rotate (Vector3.up * Time.deltaTime * -1 * moveSettings.rotateVelocity);
	}

	public bool Grounded() {
		return Physics.Raycast (transform.position, Vector3.down, moveSettings.distanceToGround, moveSettings.ground);
	}

	public void Jump() {
		Vector3 currVel = transform.InverseTransformDirection(rigidBody.velocity);
		currVel.y = moveSettings.jumpVelocity;
		rigidBody.velocity = transform.TransformDirection(currVel);
	}

	public void Fall() {
		Vector3 currVel = transform.InverseTransformDirection(rigidBody.velocity);
		currVel.y -= physicsSettings.downAcceleration;
		rigidBody.velocity = transform.TransformDirection(currVel);
	}

	public void StopFalling() {
		rigidBody.velocity = transform.up * 0;
	}

	public void Stop() {
		rigidBody.velocity = Vector3.zero;
	}
}
