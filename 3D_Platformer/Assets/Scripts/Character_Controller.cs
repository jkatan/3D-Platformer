using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller : MonoBehaviour {

	public float inputDelay = 0.1f;		//Delay para que el personaje se mueva un poco después de presionar la tecla
	public float forwardVelocity = 12;
	public float rotateVelocity = 100;

	private Quaternion currentRotation;
	private Rigidbody rigidBody;
	private float forwardInput;
	private float turnInput;

	public Quaternion CurrentRotation {
		get { return currentRotation; }
	}

	void Start() {
		currentRotation = transform.rotation;
		if (GetComponent<Rigidbody> ())
			rigidBody = GetComponent<Rigidbody> ();
		else
			Debug.LogError ("El personaje necesita un rigidbody.");

		forwardInput = 0;
		turnInput = 0;
	}

	void GetInput() {
		forwardInput = Input.GetAxis ("Vertical");
		turnInput = Input.GetAxis ("Horizontal");
	}

	void Update() {
		GetInput ();
		Run ();
		Turn ();
	}

	void Run() {
		if (Mathf.Abs (forwardInput) > inputDelay) {
			rigidBody.velocity = transform.forward * forwardInput * forwardVelocity;
		} else
			rigidBody.velocity = Vector3.zero;
	}

	void Turn() {
		if (Mathf.Abs (turnInput) > inputDelay) {
			currentRotation *= Quaternion.AngleAxis (rotateVelocity * turnInput * Time.deltaTime, Vector3.up); 
		}

		transform.rotation = currentRotation;
	}
}
