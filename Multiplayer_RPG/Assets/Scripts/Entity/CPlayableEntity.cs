using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CPlayableEntity : CEntity {

		#region Properties

		public CUserData userData;

		#endregion

		#region Implementation MonoBehaviour 

		protected override void Awake ()
		{
			base.Awake ();
			userData = new CUserData ();
		}

		// Active on Server
		public override void OnServerLoadedObject ()
		{
			base.OnServerLoadedObject ();
			m_ObjectSyn.SetUnderControl (false);
		}

		// Active On local and is local player
		public override void OnLocalPlayerLoadedObject ()
		{
			base.OnLocalPlayerLoadedObject ();
			m_ObjectSyn.SetUnderControl (true);
		}

		// Active On local and is client
		public override void OnClientLoadedObject ()
		{
			base.OnClientLoadedObject ();
			m_ObjectSyn.SetUnderControl (false);
		}

		#endregion

		#region Main methods

		#endregion

		#region Server

		[ServerCallback]
		public override void OnServerFixedUpdateSynData(float dt) {
			base.OnServerFixedUpdateSynData (dt);
			// Update Info
			RpcUpdateUserData (userData.displayName, userData.token);
		}

		#endregion

		#region Client

		[ClientCallback]
		public override void OnClientFixedUpdateBaseTime(float dt) {
			if (m_ObjectSyn == null)
				return;
			base.OnClientFixedUpdateBaseTime (dt);
			// CMD Move position
			if (m_MovePosition != m_ObjectSyn.GetMovePosition ()) {
				m_MovePosition = m_ObjectSyn.GetMovePosition ();
				CmdUpdateMovePosition (m_MovePosition);
			}
			// CMD current skill
			if (m_SkillAnimation != (int)m_ObjectSyn.GetCurrentSkill ()) {
				m_SkillAnimation = (int)m_ObjectSyn.GetCurrentSkill ();
				CmdUpdateSkillAnimation (m_SkillAnimation);
				m_SkillAnimation = 0;
				m_ObjectSyn.SetCurrentSkill (CEnum.EAnimation.Idle);
			}
		}

		#endregion

		#region Command

		[Command]
		internal virtual void CmdUpdateMovePosition(Vector3 position) {
			m_MovePosition = position;
			m_ObjectSyn.SetMovePosition (position);
		}

		[Command]
		internal virtual void CmdUpdateSkillAnimation(int animSkill) {
			m_SkillAnimation = animSkill;
			m_ObjectSyn.UpdateSkillInput ((CEnum.EAnimation)animSkill);
		}

		#endregion

		#region RPC

		[ClientRpc]
		internal virtual void RpcUpdateUserData(string name, string token) {
			userData.displayName = name;
			userData.token = token;
		}

		#endregion

	}

}
