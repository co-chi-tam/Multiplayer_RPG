using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

namespace SurvivalTest {
	public class CSkillController : CCharacterController, ISkill {

		#region Monobehavipur

		protected override void Start ()
		{
			base.Start ();
			if (this.GetDataUpdate()) {
				m_Data = TinyJSON.JSON.Load (m_DataText.text).Make<CSkillData> ();
			}
			var fsmJson = Resources.Load <TextAsset> (m_Data.fsmPath);
			m_FSMManager.LoadFSM (fsmJson.text);
			this.SetActive (true);
			this.SetEnable (true);
		}

		public override void FixedUpdateBaseTime (float dt)
		{
			base.FixedUpdateBaseTime (dt);
			if (this.GetActive()) {
				UpdateFSM (dt);
			}
		}

		#endregion

		#region Main methods

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
			var inactiveState = new FSMSkillInactiveState (this);

			m_FSMManager.RegisterState ("SkillIdleState", idleState);
			m_FSMManager.RegisterState ("SkillMoveState", moveState);
			m_FSMManager.RegisterState ("SkillActiveState", activeState);
			m_FSMManager.RegisterState ("SkillInactiveState", inactiveState);
		}

		public virtual void RemoveAllStartActionListener() {
			this.OnStartAction.RemoveAllListener ();
		}

		public virtual void AddStartActionListener(Action<object> value) {
			this.OnStartAction.AddListener (value);
		}

		public virtual void RemoveAllEndActionListener() {
			this.OnEndAction.RemoveAllListener ();
		}

		public virtual void AddEndActionListener(Action<object> value) {
			this.OnEndAction.AddListener (value);
		}

		#endregion

		#region Getter && Setter
	
		public override float GetDistanceToTarget ()
		{
//			base.GetDistanceToTarget ();
			if (m_TargetInteract == null)
				return 0.1f;
			return m_TargetInteract.GetSize() / 2f;
		}

		#endregion

	}
}
