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

			m_AnimatorController.RegisterAnimation ("EndAttack", EndAttack);

			m_AnimatorController.RegisterAnimation ("BuffHealth", BuffHealth);

			m_AnimatorController.RegisterAnimation ("Death", DeactiveObject);
		}

		#endregion

		#region Attack

		protected virtual void EndAttack(string animationName) {
			m_DidAttack = true;
		}

		protected virtual void MeleeAttackTarget(string animationName) {
			if (this.GetOtherInteractive () == false)
				return;
			if (m_TargetInteract != null) {
				var direction = m_TargetInteract.GetPosition () - this.GetPosition ();
				var distance = this.GetDistanceToTarget () * this.GetDistanceToTarget () + m_TargetInteract.GetSize();
				if (direction.sqrMagnitude <= distance) {
					var target = m_TargetInteract;
					var meleeSkill = CObjectManager.Instance.GetObject("Prefabs/Skill/PhysicBasicSkill") as CSkillController;
					var frontPosition = target.GetPosition () + (-direction.normalized * (target.GetSize () / 2f));
					meleeSkill.Init ();
					meleeSkill.SetActive (true);
					meleeSkill.SetStartPosition (frontPosition);
					meleeSkill.SetPosition (frontPosition);
					meleeSkill.SetTargetInteract (target);
					meleeSkill.SetMovePosition (target.GetMovePosition());
					meleeSkill.OnStartAction = null;
					meleeSkill.OnStartAction += () => {
						target.ApplyDamage (this, this.GetAttackDamage (), CEnum.EElementType.Pure);
					};
					meleeSkill.OnEndAction = null;
					meleeSkill.OnEndAction += () => {
						CObjectManager.Instance.SetObject("Prefabs/Skill/PhysicBasicSkill", meleeSkill);
					};
				}
			}
		}

		protected virtual void RangeAttackTarget(string animationName) {
			if (this.GetOtherInteractive () == false)
				return;
			if (m_TargetInteract != null) {
				var target = m_TargetInteract;
				var rangeSkill = CObjectManager.Instance.GetObject("Prefabs/Skill/RangeAttackSkill") as CSkillController;
				rangeSkill.Init ();
				rangeSkill.SetActive (true);
				rangeSkill.SetStartPosition (this.GetPosition ());
				rangeSkill.SetPosition (this.GetPosition ());
				rangeSkill.SetTargetInteract (target);
				rangeSkill.SetMovePosition (target.GetMovePosition());
				rangeSkill.OnStartAction = null;
				rangeSkill.OnStartAction += () => {
					target.ApplyDamage (this, this.GetAttackDamage (), CEnum.EElementType.Pure);
				};
				rangeSkill.OnEndAction = null;
				rangeSkill.OnEndAction += () => {
					CObjectManager.Instance.SetObject("Prefabs/Skill/RangeAttackSkill", rangeSkill);
				};
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
