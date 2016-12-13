using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

namespace SurvivalTest {
	public class CNatureObjectController : CNeutralObjectController {

		protected override void OnRegisterFSM ()
		{
			base.OnRegisterFSM ();

			var idleState = new FSMNatureObjectIdleState (this);
			var inativeState = new FSMNatureObjectInactiveState (this);

			m_FSMManager.RegisterState ("NatureObjectIdleState", idleState);
			m_FSMManager.RegisterState ("NatureObjectInactiveState", inativeState);
		}

		public override void ApplyDamage (IBattlable attacker, int damage, CEnum.EElementType damageType)
		{
			if (this.GetOtherInteractive () == false)
				return;
			m_BattleComponent.ApplyDamage (1, CEnum.EElementType.Pure);
		}

		protected virtual void OnNatureObjectInactive(string value) {
			
		}

		public override void SpawnResources ()
		{
			base.SpawnResources ();
			var objectSpawned = this.m_ObjectManager.GetObject("Rock_Item") as CNeutralObjectController;
			objectSpawned.Init ();
			objectSpawned.SetActive (true);
			objectSpawned.SetStartPosition (this.GetPosition());
			objectSpawned.SetPosition (this.GetPosition());
		}

		public override void SetActive (bool value)
		{
//			base.SetActive (value);
			m_Active = value;
		}
	}
}
