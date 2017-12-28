using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : IPlayerState {

	private static WalkingState instance = null;

	public static WalkingState Instance {
		get {
			if (instance == null) 
				instance = new WalkingState ();

			return instance;
		}
	}

	public void handleInput(Player player) {

		player.Run ();

		if (!player.Grounded ())
			player.Fall ();

		if (Input.GetKey(KeyCode.A)) {
			player.turnLeft();
		}

		if (Input.GetKey(KeyCode.D)) {
			player.turnRight();
		}

		if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S)) {
			player.changeState (StandingState.Instance);
			player.Stop ();
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			player.Jump ();
			player.changeState (JumpingState.Instance);
		}
	}
}
