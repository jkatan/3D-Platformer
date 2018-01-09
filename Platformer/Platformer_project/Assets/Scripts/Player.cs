using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[System.Serializable]
	public class MoveSettings {
		public float rotateVelocity = 100;
		public float distanceToGround = 0.1f;
		public LayerMask ground;
	}

	[System.Serializable]
	public class PhysicsSettings {
		public float hookVelocity = 3.0f;
		public float limitVelocity = 4.5f;
	}


	public MoveSettings moveSettings = new MoveSettings();
	public PhysicsSettings physicsSettings = new PhysicsSettings();
	public LineRenderer hookRenderer;
	public GameObject hookHead;

	private IPlayerState playerState;
	private Rigidbody rigidBody;
	private ConfigurableJoint hook;
	private int layerMask = ~ (1 << 8); //Bitmask para que el hook no colisione con el personaje
	private GameObject cloneHookHead;

	public Quaternion CurrentRotation {
		get { return transform.rotation; }
	}

	public IPlayerState State {
		get { return playerState;}
		set { playerState = value;}
	}

	void Start () {
		playerState = StandingState.Instance;
		rigidBody = GetComponent<Rigidbody> ();
		hookRenderer.enabled = false;
		hook = GetComponent<ConfigurableJoint> ();
	}

	void Update () {
		playerState.handleInput (this);
	}

	//Se actualiza el renderizado de la posición del origen del hook acorde al movimiento del jugador
	public void UpdateHookRenderer() {
		hookRenderer.SetPosition (0, transform.position);
		hookRenderer.SetPosition (1, cloneHookHead.transform.position);
	}

	//Se achica la longitud del hook
	public void HookUp() {
		SoftJointLimit n = new SoftJointLimit ();
		n.limit = hook.linearLimit.limit - Time.deltaTime * 5;
		hook.linearLimit = n;
	}

	//Se alarga la longitud del hook
	public void HookDown() {
		SoftJointLimit n = new SoftJointLimit ();
		n.limit = hook.linearLimit.limit + Time.deltaTime * 5;
		hook.linearLimit = n;
	}

	//Se libera el hook
	public void FreeHook() {
		hook.xMotion = ConfigurableJointMotion.Free;
		hook.yMotion = ConfigurableJointMotion.Free;
		hook.zMotion = ConfigurableJointMotion.Free;
		hookRenderer.enabled = false;
		Destroy (cloneHookHead);
		Wake ();
	}

	public void SwingFront() {
		if(rigidBody.velocity.magnitude < physicsSettings.limitVelocity)
			rigidBody.AddForce (transform.forward, ForceMode.Impulse);
	}

	public void SwingBack() {
		if(rigidBody.velocity.magnitude < physicsSettings.limitVelocity)
			rigidBody.AddForce (-transform.forward, ForceMode.Impulse);
	}

	//Devuelve True si se enganchó el hook, False en caso contrario
	public bool ShootHook() {

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit = new RaycastHit (); //En hit se guarda la información del collider con el que colisiona el raycast

		//Se verifica si el gancho chocó contra algo o no
		if (Physics.Raycast (ray, out hit, 100, layerMask)) {

			Vector3 hookDirection = (hit.point - transform.position).normalized;
			cloneHookHead = Instantiate (hookHead, transform.position, transform.rotation);
			cloneHookHead.GetComponent<Rigidbody> ().velocity = hookDirection * physicsSettings.hookVelocity;
			cloneHookHead.GetComponent<HookHeadScript> ().Player = this;
			
			//Se renderiza el hook
			hookRenderer.SetPosition (0, transform.position);
			hookRenderer.SetPosition (1, cloneHookHead.transform.position);
			hookRenderer.enabled = true;

			return true;
		}
		return false;
	}

	public void setHookHead() {
	
		//Se conecta el hook con el punto de colisión
		hook.connectedBody = cloneHookHead.GetComponent<Rigidbody>();
		SoftJointLimit newLimit = new SoftJointLimit ();
		newLimit.limit = Vector3.Distance (transform.position, cloneHookHead.transform.position);
		hook.linearLimit = newLimit;

		//Se configura el hook para limitar correctamente el movimiento del personaje
		hook.xMotion = ConfigurableJointMotion.Limited;
		hook.yMotion = ConfigurableJointMotion.Limited;
		hook.zMotion = ConfigurableJointMotion.Limited;
	}

	public void Wake() {
		rigidBody.WakeUp ();
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
}
