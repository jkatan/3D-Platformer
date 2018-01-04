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

		if (Input.GetKey(KeyCode.A)) {
			player.turnLeft();
		}

		if (Input.GetKey(KeyCode.D)) {
			player.turnRight();
		}

		if (Input.GetKey (KeyCode.Q)) {
			player.Wake ();
			player.HookUp ();
		}

		if (Input.GetKey (KeyCode.E)) {
			player.Wake ();
			player.HookDown ();
		}

		if (Input.GetKeyDown (KeyCode.W))
			player.SwingFront ();

		if (Input.GetKeyDown (KeyCode.S))
			player.SwingBack ();

		if (Input.GetButtonUp ("Fire1")) {
			player.Wake ();
			player.FreeHook ();
			player.State = StandingState.Instance;
		}
	}
}
