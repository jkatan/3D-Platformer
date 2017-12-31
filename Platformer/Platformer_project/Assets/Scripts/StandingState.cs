using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingState : IPlayerState {

	private static StandingState instance = null;

	public static StandingState Instance {
		get {
			if (instance == null) 
				instance = new StandingState ();

			return instance;
		}
	}

	public void handleInput(Player player) {

		if (Input.GetKey(KeyCode.A)) {
			player.turnLeft();
		}

		if (Input.GetKey(KeyCode.D)) {
			player.turnRight();
		}

		if (Input.GetButtonDown ("Fire1")) {

			if (player.ShootHook ())
				player.State = HookedState.Instance;
		}
	}
}
