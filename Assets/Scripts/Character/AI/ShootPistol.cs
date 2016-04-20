using UnityEngine;
using System.Collections;
using BehaviourMachine;

public class ShootPistol : ConditionNode
{
	private PistolAttack pistolAttack;

	public override void OnEnable()
	{
		pistolAttack = self.GetComponent<PistolAttack>();
		pistolAttack.StartLightAttack ();
	}

	public override Status Update()
	{
		if (pistolAttack.state == BaseAttack.State.Null)
		{
			if (onSuccess.id != 0)
				owner.root.SendEvent(onSuccess.id);
			return Status.Success;
		}

		return Status.Running;
	}
}
