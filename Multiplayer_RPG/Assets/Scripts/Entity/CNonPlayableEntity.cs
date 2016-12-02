using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CNonPlayableEntity : CEntity {

		#region Properties

		protected string m_TargetAttackId = "-1";

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
			var targetAttack = m_ObjectSyn.GetTargetAttack ();
			if (targetAttack != null && targetAttack.GetActive ()) {
				this.m_TargetAttackId = targetAttack.GetID ();
				RpcUpdateTargetAttack (this.m_TargetAttackId);
			} else {
				this.m_TargetAttackId = "-1";
				RpcUpdateTargetAttack ("-1");
			}
		}

		#endregion

		#region Client

		public override void OnClientFixedUpdateSyncTime (float dt)
		{
			base.OnClientFixedUpdateSyncTime (dt);
			if (m_NetworkManager == null || m_ObjectSyn == null)
				return;
			CObjectController targetAttack = null;
			if (this.m_TargetAttackId.Equals ("-1") == false) {
				var targetEntity = m_NetworkManager.FindEntity (m_TargetAttackId);
				if (targetEntity != null) {
					var controller = targetEntity.GetController () as CObjectController;
					targetAttack = controller;
				} 
			}
			m_ObjectSyn.SetTargetAttack (targetAttack);
		}

		#endregion

		#region Command

		#endregion

		#region RPC

		[ClientRpc]
		internal virtual void RpcUpdateTargetAttack(string id) {
			this.m_TargetAttackId = id;
		}

		#endregion

	}

}
