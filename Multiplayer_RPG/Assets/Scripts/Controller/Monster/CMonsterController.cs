using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CMonsterController : CCharacterController {

		#region MonoImplementation

		public override void Init ()
		{
			base.Init ();
		}

		protected override void Awake ()
		{
			base.Awake ();
		}

		protected override void Start ()
		{
			base.Start ();
			var fsmJson = Resources.Load <TextAsset> (m_Data.fsmPath);
			m_FSMManager.LoadFSM (fsmJson.text);
			SetActive (true);
//			this.m_UIManager.RegisterUIInfo (this, false, false);
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

		#endregion

		#region Getter && Setter

		public override void SetActive (bool value)
		{
//			base.SetActive (value);
			m_Active = value;
		}

		#endregion

	}
}
