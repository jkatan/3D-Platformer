using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : IPlayerState {

	private static JumpingState instance = null;
	private bool doubleJump = false;

	public static JumpingState Instance {
		get {
			if (instance == null) 
				instance = new JumpingState ();

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

		/*if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.S)) {
			player.Run ();
		}*/

		if (Input.GetKeyDown (KeyCode.Space) && !doubleJump) {
			player.Jump ();
			doubleJump = true;
		}

		if (player.Grounded ()) {
			player.changeState (StandingState.Instance);
			doubleJump = false;
		}
	}
}
