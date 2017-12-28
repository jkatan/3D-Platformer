using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour {

	public Transform target;
	public float lookSmooth = 0.09f;
	public Vector3 offSetFromTarget = new Vector3(0, 2 ,-4);

	private Vector3 destination = Vector3.zero;
	Player characterController;
	float rotateVel = 0;

	void Start() {

		setCameraTarget (target);
	}

	public void setCameraTarget(Transform target) {

		this.target = target;

		if (this.target != null) {
			if (this.target.GetComponent<Player> ())
				characterController = this.target.GetComponent<Player> ();
		} else
			Debug.LogError ("La cámara necesita un target.");
	}

	void LateUpdate() {
		MoveToTarget ();
		LookAtTarget ();
	}

	void MoveToTarget() {
		destination = characterController.CurrentRotation * offSetFromTarget;
		destination += target.position;
		transform.position = destination;
	}

	void LookAtTarget() {
		float eulerYAngle = Mathf.SmoothDampAngle (transform.eulerAngles.y, target.eulerAngles.y, ref rotateVel, lookSmooth);
		transform.rotation = Quaternion.Euler (transform.eulerAngles.x, eulerYAngle, 0);
	}
}
