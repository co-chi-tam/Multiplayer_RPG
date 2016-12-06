using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

namespace SurvivalTest {
	public class CSkillController : CCharacterController {

		protected string m_StateName;
		protected CSkillData m_Data;

		protected override void Start ()
		{
			base.Start ();
			if (this.GetDataUpdate()) {
				m_Data = TinyJSON.JSON.Load ("Data/Skill/InstantSkillData").Make<CSkillData> ();
			}
			var fsmJson = Resources.Load <TextAsset> (m_Data.fsmPath);
			m_FSMManager.LoadFSM (fsmJson.text);
			SetActive (true);
		}

		public override void FixedUpdateBaseTime (float dt)
		{
			base.FixedUpdateBaseTime (dt);
			if (this.GetActive() && this.GetLocalUpdate()) {
				UpdateFSM (dt);
			}
		}

		public override void UpdateFSM(float dt) {
			base.UpdateFSM (dt);
			m_FSMManager.UpdateState (dt);
			m_StateName = m_FSMManager.currentStateName;
		}

		public override string GetFSMStateName ()
		{
			base.GetFSMStateName ();
			return m_StateName;
		}

		protected override void OnRegisterFSM ()
		{
			base.OnRegisterFSM ();
			var idleState = new FSMSkillIdleState (this);
			var activeState = new FSMSkillActiveState (this);
			var deactiveState = new FSMSkillDeactiveState (this);

			m_FSMManager.RegisterState ("SkillIdleState", idleState);
			m_FSMManager.RegisterState ("SkillActiveState", activeState);
			m_FSMManager.RegisterState ("SkillDeactiveState", deactiveState);
		}

		public override void SetData (CObjectData value)
		{
			base.SetData (value);
			m_Data = value as CSkillData;
		}
	
	}
}
