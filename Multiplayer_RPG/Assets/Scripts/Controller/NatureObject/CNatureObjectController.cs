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

		public override void SpawnResourceMaterials ()
		{
			if (this.GetOtherInteractive () == false)
				return;
			base.SpawnResourceMaterials ();
			if (m_Data == null || m_Data.resourceMaterials.Length == 0)
				return;
			for (int i = 0; i < m_Data.resourceMaterials.Length; i++) {
				var itemData = m_Data.resourceMaterials [i];
				var randomCircle = UnityEngine.Random.insideUnitCircle; 
				var randomAround = randomCircle * this.GetSize ();
				var randomPosition = new Vector3 (randomAround.x, 0f, randomAround.y) + this.GetPosition();
				if ((Mathf.Abs(randomCircle.x) * 100f) > itemData.rate) {
					continue;
				}
				this.m_ObjectManager.GetObjectModified (itemData.name, (obj) => {
					var objectSpawned = obj as CNeutralObjectController;
					objectSpawned.Init ();
					objectSpawned.SetActive (true);
					objectSpawned.SetEnable (true);
					objectSpawned.SetStartPosition (randomPosition);
					objectSpawned.SetPosition (randomPosition);
					objectSpawned.SetCurrentAmount (itemData.currentAmount);
					return objectSpawned;
				});
			}
		}

		public override void SetActive (bool value)
		{
//			base.SetActive (value);
			m_Active = value;
		}
	}
}
