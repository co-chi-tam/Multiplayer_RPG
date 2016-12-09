using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

namespace SurvivalTest {
	[RequireComponent(typeof(CapsuleCollider))]
	public class CNatureObjectController : CCharacterController {

		protected override void Init ()
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
			if (this.GetDataUpdate()) {
				m_Data = TinyJSON.JSON.Load (m_DataText.text).Make<CCharacterData> ();
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

		public override void ApplyDamage (IBattlable attacker, int damage, CEnum.EElementType damageType)
		{
			if (this.GetOtherInteractive () == false)
				return;
			m_BattleComponent.ApplyDamage (1, CEnum.EElementType.Pure);
		}

		public override void SetActive (bool value)
		{
//			base.SetActive (value);
			m_Active = value;
		}
	
	}
}
