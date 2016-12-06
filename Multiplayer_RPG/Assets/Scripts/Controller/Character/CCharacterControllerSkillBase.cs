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
			m_AnimatorController.RegisterAnimation ("MeleeAttack", MeleeAttackTarget);
			m_AnimatorController.RegisterAnimation ("RangeAttack", RangeAttackTarget);
			m_AnimatorController.RegisterAnimation ("MagicAttack", MagicAttackTarget);

			m_AnimatorController.RegisterAnimation ("BuffHealth", BuffHealth);

			m_AnimatorController.RegisterAnimation ("Death", DeactiveObject);
		}

		#endregion

		#region Attack

		protected virtual void MeleeAttackTarget(string animationName) {
			m_DidAttack = true;
			if (this.GetOtherInteractive () == false)
				return;
			if (m_TargetInteract != null) {
				var direction = m_TargetInteract.GetPosition () - this.GetPosition ();
				var distance = this.GetDistanceToTarget () * this.GetDistanceToTarget () + m_TargetInteract.GetSize();
				if (direction.sqrMagnitude <= distance) {
					m_TargetInteract.ApplyDamage (this, GetPureDamage (), CEnum.EElementType.Pure);
				}
			}
		}

		protected virtual void RangeAttackTarget(string animationName) {
			m_DidAttack = true;	
			if (this.GetOtherInteractive () == false)
				return;
			if (m_TargetInteract != null) {
				var target = m_TargetInteract;
				var rangeAttackSkill = CObjectManager.Instance.GetObject("Prefabs/Skill/RangeAttackSkill") as CSkillController;
				rangeAttackSkill.Init ();
				rangeAttackSkill.SetStartPosition (this.GetPosition ());
				rangeAttackSkill.SetPosition (this.GetPosition ());
				rangeAttackSkill.SetMovePosition (target.GetPosition ());
				rangeAttackSkill.OnEndAction = null;
				rangeAttackSkill.OnEndAction += () => {
					target.ApplyDamage (this, GetPureDamage (), CEnum.EElementType.Pure);
					rangeAttackSkill.SetActive(false);
					CObjectManager.Instance.SetObject("Prefabs/Skill/RangeAttackSkill", rangeAttackSkill);
				};
			}
		}

		protected virtual void MagicAttackTarget(string animationName) {
			if (this.GetOtherInteractive () == false)
				return;
			if (m_TargetInteract != null) {
				
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

		#endregion

	}
}
