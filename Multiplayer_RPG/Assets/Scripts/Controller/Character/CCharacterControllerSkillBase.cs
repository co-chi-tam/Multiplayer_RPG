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

			m_AnimatorController.RegisterAnimation ("ChopTree", ChopTree);

			m_AnimatorController.RegisterAnimation ("EndAttack", EndAttack);

			m_AnimatorController.RegisterAnimation ("BuffHealth", BuffHealth);
		}

		#endregion

		#region Attack

		protected virtual void MeleeAttackTarget(string animationName) {
			m_DidAttack = true;
			if (m_TargetInteract != null) {
				var direction = m_TargetInteract.GetPosition () - this.GetPosition ();
				var distance = this.GetDistanceToTarget () * this.GetDistanceToTarget () + m_TargetInteract.GetSize();
				if (direction.sqrMagnitude <= distance) {
					var target = m_TargetInteract;
					var frontPosition = target.GetPosition () + (-direction.normalized * (target.GetSize () / 2f));
					CreateSkillObject ("PhysicBasicSkill", frontPosition, target);
				}
			}
			this.SetCurrentSkill (CEnum.EAnimation.Idle);
		}

		protected virtual void ChopTree(string animationName) {
			m_DidAttack = true;
		}

		protected virtual void EndAttack(string animationName) {
			m_DidAttack = true;
		}

		protected virtual void MultiAttackTarget(string animationName) {
			m_DidAttack = true;
			if (m_TargetInteract != null) {
				var direction = m_TargetInteract.GetPosition () - this.GetPosition ();
				var distance = this.GetDistanceToTarget () * this.GetDistanceToTarget () + m_TargetInteract.GetSize();
				if (direction.sqrMagnitude <= distance) {
					var target = m_TargetInteract;
					var frontPosition = target.GetPosition () + (-direction.normalized * (target.GetSize () / 2f));
					CreateSkillObject ("PhysicBasicSkill", frontPosition, target);
				}
			}
			this.SetCurrentSkill (CEnum.EAnimation.Idle);
		}

		protected virtual void RangeAttackTarget(string animationName) {
			m_DidAttack = true;
			if (m_TargetInteract != null) {
				var target = m_TargetInteract;
				CreateSkillObject ("RangeAttackSkill", this.GetPosition(), target);
			}
			this.SetCurrentSkill (CEnum.EAnimation.Idle);
		}

		private void CreateSkillObject(string name, Vector3 position, params CObjectController[] targets) {
			var objectSkill = this.m_ObjectManager.GetObject(name) as CSkillController;
			objectSkill.Init ();
			objectSkill.SetActive (true);
			objectSkill.SetEnable (true);
			objectSkill.SetStartPosition (position);
			objectSkill.SetPosition (position);
			objectSkill.SetTargetInteract (targets[0]);
			objectSkill.SetMovePosition (targets[0].GetPosition());
			objectSkill.OnStartAction.RemoveAllListener ();
			objectSkill.OnStartAction.AddListener ((x) => {
				for (int i = 0; i < targets.Length; i++) {
					targets[i].ApplyDamage (this, this.GetAttackDamage (), CEnum.EElementType.Pure);
				}
			});
			objectSkill.OnEndAction.RemoveAllListener ();
			objectSkill.OnEndAction.AddListener ((x) => {
				// TODO
			});
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
