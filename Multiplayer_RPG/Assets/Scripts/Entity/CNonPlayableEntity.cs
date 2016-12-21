using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CNonPlayableEntity : CEntity {

		#region Properties

		protected string m_OwnerId = "-1";
		protected IItem m_ItemObjSync;

		#endregion

		#region Implementation MonoBehaviour 

		// Active on Server
		public override void OnServerLoadedObject ()
		{
			base.OnServerLoadedObject ();
			m_ObjectSyn.SetUnderControl (true);
			CObjectManager.Instance.SetObject (m_ObjectSyn.GetName (), m_ObjectSyn.GetController () as CBaseController);
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

		#region Main methods

		protected override void LoadObjectSync (GameObject value)
		{
			base.LoadObjectSync (value);
			m_ItemObjSync = value.GetComponent<IItem> ();
		}

		#endregion

		#region Server

		[ServerCallback]
		public override void OnServerFixedUpdateSynData (float dt)
		{
			base.OnServerFixedUpdateSynData (dt);
			var owner = m_ObjectSyn.GetOwner ();
			if (owner != null && owner.GetActive ()) {
				this.m_OwnerId = owner.GetID ();
				RpcUpdateOnwer (this.m_OwnerId);
			} else {
				this.m_OwnerId = "-1";
				RpcUpdateOnwer ("-1");
			}
			if (m_ItemObjSync != null) {
				RpcUpdateItemData (m_ItemObjSync.GetCurrentAmount());
			}
		}

		#endregion

		#region Client

		[ClientCallback]
		public override void OnClientFixedUpdateSyncTime(float dt) {
			base.OnClientFixedUpdateSyncTime (dt);
			if (m_ObjectSyn == null)
				return;
			CObjectController onwer = null;
			if (this.m_OwnerId.Equals ("-1") == false) {
				var ownerEntity = m_NetworkManager.FindEntity (this.m_OwnerId);
				if (ownerEntity != null) {
					var controller = ownerEntity.GetController () as CObjectController;
					onwer = controller;
				} 
			}
			m_ObjectSyn.SetOwner (onwer);
		}

		#endregion

		#region Command

		#endregion

		#region RPC

		[ClientRpc]
		internal virtual void RpcUpdateOnwer(string id) {
			this.m_OwnerId = id;
		}

		[ClientRpc]
		internal virtual void RpcUpdateItemData(int amount) {
			if (m_ItemObjSync == null)
				return;
			m_ItemObjSync.SetCurrentAmount (amount);
		}

		#endregion

	}

}
