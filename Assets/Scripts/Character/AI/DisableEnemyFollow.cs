using UnityEngine;
using System.Collections;
using BehaviourMachine;

public class DisableEnemyFollow : ActionNode {

	private EnemyFollow enemyFollow;

	public override void Start(){
		enemyFollow = self.GetComponent<EnemyFollow> ();
		enemyFollow.targetType = EnemyFollow.TargetType.Null;
	}

}