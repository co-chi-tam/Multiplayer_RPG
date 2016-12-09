using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

namespace SurvivalTest {
	public class CSkillController : CCharacterController {

		protected override void Start ()
		{
			base.Start ();
			if (this.GetDataUpdate()) {
				m_Data = TinyJSON.JSON.Load (m_DataText.text).Make<CSkillData> ();
			}
			var fsmJson = Resources.Load <TextAsset> (m_Data.fsmPath);
			m_FSMManager.LoadFSM (fsmJson.text);
			SetActive (true);
		}

		public override void FixedUpdateBaseTime (float dt)
		{
			base.FixedUpdateBaseTime (dt);
			if (this.GetActive()) {
				UpdateFSM (dt);
			}
		}

		public override void UpdateFSM(float dt) {
			base.UpdateFSM (dt);
			m_FSMManager.UpdateState (dt);
		}

		protected override void OnRegisterFSM ()
		{
			base.OnRegisterFSM ();
			var idleState = new FSMSkillIdleState (this);
			var moveState = new FSMSkillMoveState (this);
			var activeState = new FSMSkillActiveState (this);
			var deactiveState = new FSMSkillDeactiveState (this);

			m_FSMManager.RegisterState ("SkillIdleState", idleState);
			m_FSMManager.RegisterState ("SkillMoveState", moveState);
			m_FSMManager.RegisterState ("SkillActiveState", activeState);
			m_FSMManager.RegisterState ("SkillDeactiveState", deactiveState);
		}
	
		public override float GetDistanceToTarget ()
		{
			base.GetDistanceToTarget ();
			if (m_TargetInteract == null)
				return 0.1f;
			return m_TargetInteract.GetSize() / 2f;
		}

	}
}
