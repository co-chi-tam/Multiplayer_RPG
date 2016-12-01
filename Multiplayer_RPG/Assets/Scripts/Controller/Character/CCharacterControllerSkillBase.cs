using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

namespace SurvivalTest {
	
	public partial class CCharacterController {

		#region Properties



		#endregion

		#region Main methods

		protected override void OnRegisterAnimation() {
			base.OnRegisterComponent ();
			m_AnimatorController.RegisterAnimation ("Attack", AttackToTarget);
			m_AnimatorController.RegisterAnimation ("AttackPureDamage", AttackPureDamage);
			m_AnimatorController.RegisterAnimation ("AttackPhysicDamage", AttackPhysicDamage);
			m_AnimatorController.RegisterAnimation ("AttackMagicDamage", AttackMagicDamage);

			m_AnimatorController.RegisterAnimation ("BuffHealth", BuffHealth);
			m_AnimatorController.RegisterAnimation ("BuffAllyHealth", BuffAllyHealth);

			m_AnimatorController.RegisterAnimation ("Death", DeactiveObject);
		}

		#endregion

		#region Attack

		protected virtual void AttackToTarget(string animationName) {
			m_DidAttack = true;
			if (this.GetOtherInteractive () == false)
				return;
			if (m_TargetAttack != null) {
				var direction = m_TargetAttack.GetPosition () - this.GetPosition ();
				var distance = this.GetDistanceToTarget () * this.GetDistanceToTarget () + m_TargetAttack.GetSize();
				if (direction.sqrMagnitude <= distance) {
					m_TargetAttack.ApplyDamage (this, GetPureDamage (), CEnum.EAttackType.Pure);
				}
			}
		}

		protected virtual void AttackPureDamage(string animationName) {
			m_DidAttack = true;
			if (this.GetOtherInteractive () == false)
				return;
			if (m_TargetAttack != null) {
				var direction = m_TargetAttack.GetPosition () - this.GetPosition ();
				var distance = this.GetDistanceToTarget () * this.GetDistanceToTarget () + m_TargetAttack.GetSize();
				if (direction.sqrMagnitude <= distance) {
					m_TargetAttack.ApplyDamage (this, GetPureDamage (), CEnum.EAttackType.Pure);
				}
			}
		}

		protected virtual void AttackPhysicDamage(string animationName) {
			m_DidAttack = true;
			if (this.GetOtherInteractive () == false)
				return;
			if (m_TargetAttack != null) {
				m_TargetAttack.ApplyDamage (this, GetPhysicDamage(), CEnum.EAttackType.Physic);
			}
		}

		protected virtual void AttackMagicDamage(string animationName) {
			m_DidAttack = true;
			if (this.GetOtherInteractive () == false)
				return;
			if (m_TargetAttack != null) {
				m_TargetAttack.ApplyDamage (this, GetMagicDamage(), CEnum.EAttackType.Magic);
			}
		}

		#endregion

		#region Buff

		protected virtual void BuffHealth(string animationName) {
			m_DidAttack = true;
			if (this.GetOtherInteractive () == false)
				return;
			this.ApplyBuff (this, 50, CEnum.EStatusType.Health);
		}

		protected virtual void BuffAllyHealth(string animationName) {
			m_DidAttack = true;
			if (this.GetOtherInteractive () == false)
				return;
			this.ApplyBuff (this, 50, CEnum.EStatusType.Health);
		}

		#endregion

	}
}
