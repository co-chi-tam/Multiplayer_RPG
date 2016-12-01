using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

namespace SurvivalTest {

	public partial class CCharacterController {

		#region Properties

		protected CBattlableComponent m_BattleComponent;

		#endregion

		#region Main methods

		public override void FindAttackObject() {
			if (this.GetOtherInteractive () == false)
				return;
			base.FindAttackObject ();
			// FIND ENEMY BASE OBJECT TYPE AND INRANGE
			this.SetTargetAttack (null);
			var colliders = Physics.OverlapSphere (this.GetPosition (), this.GetSeekRadius (), m_ObjPlayerMask);
			if (colliders.Length > 0) {
				for (int i = colliders.Length - 1; i >= 0; i--) {
					var objCtrl = colliders [i].GetComponent<CObjectController> ();
					if (objCtrl != null && objCtrl != this) {
						if (objCtrl.GetObjectType () != this.GetObjectType ()) {
							var direction = objCtrl.GetPosition () - this.GetPosition ();
							var targetPosition = objCtrl.GetPosition () - (direction.normalized * (objCtrl.GetSize () + this.GetAttackRange() - 0.2f)); 
							this.SetTargetAttack (objCtrl);
							this.SetMovePosition (targetPosition);
							break;
						}
					}
				}
			}
		}

		public override void ApplyDamage(IBattlable attacker, int damage, CEnum.EAttackType damageType) {
			return;
			if (this.GetOtherInteractive () == false)
				return;
			base.ApplyDamage (attacker, damage, damageType);
			m_BattleComponent.ApplyDamage (damage, damageType);
			var health = 0;
			if (m_BattleComponent.CalculateHealth (this.GetCurrentHealth (), out health)) {
				this.SetCurrentHealth (health);
			}
		}

		public override void ApplyBuff (IBattlable buffer, int buff, CEnum.EStatusType statusType) {
			return;
			if (this.GetOtherInteractive () == false)
				return;
			base.ApplyBuff (buffer, buff, statusType);
			m_BattleComponent.ApplyBuff (buff, statusType);
			var health = 0;
			if (m_BattleComponent.CalculateHealth (this.GetCurrentHealth (), out health)) {
				this.SetCurrentHealth (health);
			}
		}

		#endregion
	
	}
}
