﻿using UnityEngine;
using System.Collections;
using BehaviourMachine;

public class SlashPlayer : ConditionNode {

	private Attack attack;

	public override void Start () {
		attack = self.GetComponent<Attack> ();
	}

	public override Status Update () {

		// Attack Abe
		if (Random.value > 0.35) {
			attack.LightAttack();
		} else {
			attack.HeavyAttack();
		}
		if (onSuccess.id != 0)
			owner.root.SendEvent (onSuccess.id);
		return Status.Success;
	}
}
