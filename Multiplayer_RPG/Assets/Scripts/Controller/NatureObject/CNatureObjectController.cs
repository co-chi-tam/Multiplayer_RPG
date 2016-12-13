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

		public override void SpawnResources ()
		{
			if (this.GetOtherInteractive () == false)
				return;
			base.SpawnResources ();
			for (int i = 0; i < m_Data.inventoryItems.Length; i++) {
				var itemData = m_Data.inventoryItems [i];
				var objectSpawned = this.m_ObjectManager.GetObject(itemData.name) as CNeutralObjectController;
				var randomAround = UnityEngine.Random.insideUnitCircle * this.GetSize ();
				var randomPosition = new Vector3 (randomAround.x, 0f, randomAround.y) + this.GetPosition();
				objectSpawned.Init ();
				objectSpawned.SetActive (true);
				objectSpawned.SetStartPosition (this.GetPosition());
				objectSpawned.SetPosition (randomPosition);
				objectSpawned.SetCurrentAmount (itemData.currentAmount);
			}
		}

		public override void SetActive (bool value)
		{
//			base.SetActive (value);
			m_Active = value;
		}
	}
}
