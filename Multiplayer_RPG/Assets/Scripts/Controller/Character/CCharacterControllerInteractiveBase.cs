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

		public override void InteractAnObject () {	
			if (this.GetOtherInteractive() == false)
				return;
			base.InteractAnObject ();
			if (m_TargetInteract != null) {
				if (m_TargetInteract.GetObjectType() != this.GetObjectType ()) {
					if (this.GetObjectType () == CEnum.EObjectType.Survivaler) {
						this.SetAnimation (this.GetCurrentSkill());
					} else {
						var random = (int)(Mathf.PerlinNoise (Time.time, Time.time) * 4);
						this.SetActiveSkill ((int)CEnum.EAnimation.Attack_3);
					}
				}
			}
		}

		public override void UpdateInteractiveAnObject () {
			base.UpdateInteractiveAnObject ();

		}

		public override void FindTargetInteract() {
			if (this.GetOtherInteractive () == false)
				return;
			base.FindTargetInteract ();
			// FIND ENEMY BASE OBJECT TYPE AND INRANGE
			this.SetTargetInteract (null);
			var colliders = Physics.OverlapSphere (this.GetPosition (), this.GetSeekRadius (), m_ObjPlayerMask);
			if (colliders.Length > 0) {
				for (int i = colliders.Length - 1; i >= 0; i--) {
					var objCtrl = colliders [i].GetComponent<CObjectController> ();
					if (objCtrl != null && objCtrl != this) {
						if (objCtrl.GetObjectType () != this.GetObjectType ()) {
							var direction = objCtrl.GetPosition () - this.GetPosition ();
							var targetPosition = objCtrl.GetPosition () - (direction.normalized * (objCtrl.GetSize () + this.GetAttackRange() - 0.2f)); 
							this.SetTargetInteract (objCtrl);
							this.SetMovePosition (targetPosition);
							break;
						}
					}
				}
			}
		}

		public override void ApplyDamage(IBattlable attacker, int damage, CEnum.EElementType damageType) {
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
