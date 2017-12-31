using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookedState : IPlayerState {

	private static HookedState instance = null;

	public static HookedState Instance {
		get {
			if (instance == null) 
				instance = new HookedState ();

			return instance;
		}
	}

	public void handleInput(Player player) {

		player.UpdateHookRenderer ();

		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.S)) {
			player.Wake ();
			player.ModifyHookLength ();
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			player.Wake ();
			player.FreeHook ();
			player.State = StandingState.Instance;
		}

		if (Input.GetButtonDown ("Fire1")) {
			player.FreeHook ();
			if (!player.ShootHook ())
				player.State = StandingState.Instance;
		}
	}
}
