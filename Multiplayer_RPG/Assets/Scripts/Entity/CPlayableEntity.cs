using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CPlayableEntity : CEntity {

		#region Properties

		public CUserData userData;

		protected string m_Talk = string.Empty;
		protected Vector2 m_TouchPosition;

		#endregion

		#region Implementation MonoBehaviour 

		public override void Init ()
		{
			base.Init ();
			m_ObjectSyn.SetName (userData.displayName);
			m_ObjectSyn.SetToken (userData.token);
			m_ObjectSyn.SetTouchPosition (m_TouchPosition);
		}

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
			if (m_SkillInput != (int)m_ObjectSyn.GetCurrentSkill ()) {
				m_SkillInput = (int)m_ObjectSyn.GetCurrentSkill ();
				// Skill Input
				CmdUpdateSkillInput (m_SkillInput);
				// Reset
				m_SkillInput = (int)CEnum.EAnimation.Idle;
				m_ObjectSyn.SetCurrentSkill (CEnum.EAnimation.Idle);
			}
			// CMD touch position
			if (m_TouchPosition != m_ObjectSyn.GetTouchPosition()) {
				m_TouchPosition = m_ObjectSyn.GetTouchPosition ();
				// Touch Input
				CmdUpdateTouchPosition (m_TouchPosition);
			}
			// CMD talk
			if (m_Talk.Equals (m_ObjectSyn.GetTalk()) == false) {
				m_Talk = m_ObjectSyn.GetTalk ();
				CmdUpdateCommunicate (m_ObjectSyn.GetTalk ());
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
		internal virtual void CmdUpdateSkillInput(int animSkill) {
			m_SkillInput = animSkill;
			m_ObjectSyn.UpdateSkillInput ((CEnum.EAnimation)animSkill);
			m_ObjectSyn.SetCurrentSkill ((CEnum.EAnimation)animSkill);
		}

		[Command]
		internal virtual void CmdUpdateTouchPosition(Vector2 screenPoint) {
//			m_ObjectSyn.UpdateSelectionObject (screenPoint);
			m_ObjectSyn.SetTouchPosition (screenPoint);
		}

		[Command]
		internal virtual void CmdUpdateCommunicate(string talk) {
			m_Talk = talk;
			m_ObjectSyn.SetTalk (talk);
			RpcUpdateCommunicate (talk);
		}

		#endregion

		#region RPC

		[ClientRpc]
		internal virtual void RpcUpdateUserData(string name, string token) {
			userData.displayName = name;
			userData.token = token;
			if (m_ObjectSyn == null)
				return;
			m_ObjectSyn.SetName (name);
			m_ObjectSyn.SetToken (token);
		}

		[ClientRpc]
		internal virtual void RpcUpdateCommunicate(string talk) {
			m_Talk = talk;
			if (m_ObjectSyn == null)
				return;
			m_ObjectSyn.SetTalk (talk);
		}

		#endregion

	}

}
