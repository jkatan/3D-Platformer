using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookHeadScript : MonoBehaviour {

	private Player player;

	void OnCollisionEnter(Collision collision) {
		Rigidbody rbody = GetComponent<Rigidbody> ();
		rbody.velocity = Vector3.zero;
		rbody.constraints = RigidbodyConstraints.FreezeAll;
		transform.parent = collision.gameObject.transform;
		player.setHookHead ();
	}

	public Player Player {
		set { player = value;} 
	}
}
