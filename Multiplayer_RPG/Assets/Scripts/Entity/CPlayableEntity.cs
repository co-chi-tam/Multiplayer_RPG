﻿using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CPlayableEntity : CEntity {

		#region Properties

		public CUserData userData;

		protected string m_Talk = string.Empty;

		#endregion

		#region Implementation MonoBehaviour 

		public override void Init ()
		{
			base.Init ();
			m_ObjectSyn.SetName (userData.displayName);
			m_ObjectSyn.SetToken (userData.token);
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
			if (m_SkillAnimation != (int)m_ObjectSyn.GetCurrentSkill ()) {
				m_SkillAnimation = (int)m_ObjectSyn.GetCurrentSkill ();
				CmdUpdateSkillAnimation (m_SkillAnimation);
				m_SkillAnimation = 0;
				m_ObjectSyn.SetCurrentSkill (CEnum.EAnimation.Idle);
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
		internal virtual void CmdUpdateSkillAnimation(int animSkill) {
			m_SkillAnimation = animSkill;
			m_ObjectSyn.UpdateSkillInput ((CEnum.EAnimation)animSkill);
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
