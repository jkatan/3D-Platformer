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

	public MoveSettings moveSettings = new MoveSettings();
	public LineRenderer hookRenderer;

	private IPlayerState playerState;
	private Rigidbody rigidBody;
	private ConfigurableJoint hook;

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
	}

	public void SwingFront() {
		rigidBody.AddForce (transform.forward * 3, ForceMode.Impulse);
	}

	public void SwingBack() {
		rigidBody.AddForce (-transform.forward * 3, ForceMode.Impulse);
	}

	//Devuelve True si se enganchó el hook, False en caso contrario
	public bool ShootHook() {
		
		Vector2 mousePos = Input.mousePosition;
		Vector3 target = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, 5));
		RaycastHit hit = new RaycastHit (); //En hit se guarda la información del collider con el que colisiona el raycast

		//Se verifica si el gancho chocó contra algo o no
		if (Physics.Raycast (transform.position, target, out hit, 5)) {

			//Se renderiza el hook
			hookRenderer.SetPosition (0, transform.position);
			hookRenderer.SetPosition (1, hit.point);
			hookRenderer.enabled = true;

			//Se conecta el hook con el punto de colisión
			hook.connectedAnchor = hit.point;
			SoftJointLimit newLimit = new SoftJointLimit ();
			newLimit.limit = Vector3.Distance(transform.position, hit.point);
			hook.linearLimit = newLimit;

			//Se configura el hook para limitar correctamente el movimiento del personaje
			hook.xMotion = ConfigurableJointMotion.Limited;
			hook.yMotion = ConfigurableJointMotion.Limited;
			hook.zMotion = ConfigurableJointMotion.Limited;

			return true;
		}

		return false;
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
