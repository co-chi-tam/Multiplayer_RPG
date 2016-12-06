using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CNonPlayableEntity : CEntity {

		#region Properties

		protected string m_TargetInteractiveId = "-1";

		#endregion

		#region Implementation MonoBehaviour 

		// Active on Server
		public override void OnServerLoadedObject ()
		{
			base.OnServerLoadedObject ();
			m_ObjectSyn.SetUnderControl (true);
		}

		// Active On local and is local player
		public override void OnLocalPlayerLoadedObject ()
		{
			base.OnLocalPlayerLoadedObject ();
			m_ObjectSyn.SetUnderControl (false);
		}

		// Active On local and is client
		public override void OnClientLoadedObject ()
		{
			base.OnClientLoadedObject ();
			m_ObjectSyn.SetUnderControl (false);
		}

		#endregion

		#region Server

		[ServerCallback]
		public override void OnServerFixedUpdateSynData (float dt)
		{
			base.OnServerFixedUpdateSynData (dt);
			var targetAttack = m_ObjectSyn.GetTargetInteract ();
			if (targetAttack != null && targetAttack.GetActive ()) {
				this.m_TargetInteractiveId = targetAttack.GetID ();
				RpcUpdateTargetInteractive (this.m_TargetInteractiveId);
			} else {
				this.m_TargetInteractiveId = "-1";
				RpcUpdateTargetInteractive ("-1");
			}
		}

		#endregion

		#region Client

		public override void OnClientFixedUpdateSyncTime (float dt)
		{
			base.OnClientFixedUpdateSyncTime (dt);
			if (m_NetworkManager == null || m_ObjectSyn == null)
				return;
			CObjectController targetInteractive = null;
			if (this.m_TargetInteractiveId.Equals ("-1") == false) {
				var targetEntity = m_NetworkManager.FindEntity (m_TargetInteractiveId);
				if (targetEntity != null) {
					var controller = targetEntity.GetController () as CObjectController;
					targetInteractive = controller;
				} 
			}
			m_ObjectSyn.SetTargetInteract (targetInteractive);
		}

		#endregion

		#region Command

		#endregion

		#region RPC

		[ClientRpc]
		internal virtual void RpcUpdateTargetInteractive(string id) {
			this.m_TargetInteractiveId = id;
		}

		#endregion

	}

}
