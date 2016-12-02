using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

namespace SurvivalTest {
	
	public partial class CCharacterController {

		#region Main methods

		protected override void OnRegisterFSM() {
			base.OnRegisterFSM ();
			var idleState 		= new FSMCharacterIdleState (this);
			var moveState 		= new FSMCharacterMoveState (this);
			var attackState 	= new FSMCharacterAttackState (this);
			var seekState 		= new FSMCharacterSeekState (this);
			var autoAttackState = new FSMCharacterAutoAttackState (this);
			var autoSeekState 	= new FSMCharacterAutoSeekState (this);
			var deathState 		= new FSMCharacterDeathState (this);
			var waitingState	= new FSMCharacterWaitingState (this);

			m_FSMManager.RegisterState ("CharacterIdleState", 		idleState);
			m_FSMManager.RegisterState ("CharacterMoveState", 		moveState);
			m_FSMManager.RegisterState ("CharacterAttackState", 	attackState);
			m_FSMManager.RegisterState ("CharacterSeekState", 		seekState);
			m_FSMManager.RegisterState ("CharacterAutoAttackState", autoAttackState);
			m_FSMManager.RegisterState ("CharacterAutoSeekState",	autoSeekState);
			m_FSMManager.RegisterState ("CharacterDeathState", 		deathState);
			m_FSMManager.RegisterState ("CharacterWaitingState",	waitingState);

			m_FSMManager.RegisterCondition ("IsDeath", 				IsDeath);
			m_FSMManager.RegisterCondition ("DidMoveToPosition",	DidMoveToPosition);
			m_FSMManager.RegisterCondition ("DidMoveToTargetAttack", DidMoveToTargetAttack);
			m_FSMManager.RegisterCondition ("DidAttack", 			this.GetDidAttack);
			m_FSMManager.RegisterCondition ("HaveTargetAttack",		HaveTargetAttack);
			m_FSMManager.RegisterCondition ("HaveTargetInRange",	HaveTargetInRange);
		}

		#endregion

		#region FSM

		internal virtual bool HaveTargetInRange() {
			if (m_TargetAttack == null || m_TargetAttack.GetActive() == false)
				return false;
			var direction = m_TargetAttack.GetPosition () - this.GetPosition ();
			var distance = this.GetSeekRadius () * this.GetSeekRadius ();
			return direction.sqrMagnitude <= distance;
		}

		internal virtual bool DidMoveToTargetAttack() {
			if (m_TargetAttack == null)
				return false;
			m_MovableComponent.targetPosition = m_TargetAttack.GetPosition ();
			return m_MovableComponent.DidMoveToTarget (m_TargetAttack.GetPosition());
		}

		internal virtual bool DidMoveToPosition() {
			return m_MovableComponent.DidMoveToTarget ();
		}

		internal virtual bool HaveTargetAttack() {
			if (m_TargetAttack == null)
				return false;
			return m_TargetAttack.GetActive();
		}

		internal override bool IsDeath ()
		{
			return GetCurrentHealth () <= 0;
		}

		internal override bool DidEndWaiting() {
			m_WaitingPerAction -= Time.fixedDeltaTime;
			return m_WaitingPerAction <= 0f;
		}

		#endregion

		#region Getter && Setter 

		public override string GetFSMStateName ()
		{
			base.GetFSMStateName ();
			return m_FSMManager.currentStateName;
		}

		public override void SetFSMStateName (string value)
		{
			base.SetFSMStateName (value);
			m_FSMManager.SetState (value);
		}

		public override string GetFSMName ()
		{
			base.GetFSMName ();
			return m_FSMText.name;
		}

		#endregion

	}
}
