using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using ObjectPool;

namespace SurvivalTest {

	[RequireComponent(typeof(NetworkIdentity))]
	public class CObjectManagerEntity : NetworkBehaviour {

		protected CObjectManager m_ObjectManager;
		protected CNetworkManager m_NetworkManager;

		protected Dictionary<string, ObjectPool<CEntity>> m_ObjectPools;

		protected virtual void Awake() {
			m_ObjectPools = new Dictionary<string, ObjectPool<CEntity>> ();
		}

		public override void OnStartServer ()
		{
			base.OnStartServer ();
			this.m_NetworkManager = CNetworkManager.GetInstance ();

			this.m_ObjectManager = CObjectManager.GetInstance ();
			this.m_ObjectManager.OnGetObject += OnServerSyncObject;
			this.m_ObjectManager.OnSetObject += OnServerReturnObject;
		}

		[ServerCallback]
		protected virtual void OnServerSyncObject(string name, CBaseController objectSync) {
			var objectSyncController = objectSync as CObjectController;
			var entityNonPlayable = this.GetEntityObject (name, objectSyncController);
			this.m_NetworkManager.OnServerRegisterEntity (entityNonPlayable, entityNonPlayable.GetComponent<NetworkIdentity>().connectionToClient);
			NetworkServer.Spawn (entityNonPlayable.gameObject);
		}

		[ServerCallback]
		protected virtual void OnServerReturnObject(string name, CBaseController objectSync) {
			var id = objectSync.GetID ();
			var entity = m_NetworkManager.FindEntity (id); 
			this.SetEntityObject (name, entity);
		}

		public CEntity GetEntityObject(string name, CObjectController controller){
			if (m_ObjectPools.ContainsKey (name)) {
				var objGet = m_ObjectPools [name].Get ();
				if (objGet != null) {
					this.RepairObject (objGet, controller);
					return objGet;
				}
			} else {
				m_ObjectPools [name] = new ObjectPool<CEntity> ();
			}
			var nonPlayable = (GameObject)GameObject.Instantiate (this.m_NetworkManager.spawnPrefabs[(int)CNetworkManager.EEntityType.NonPlayableEntity], 
				Vector3.zero, Quaternion.identity);
			var entityNonPlayable = nonPlayable.GetComponent<CEntity> ();
			this.RepairObject (entityNonPlayable, controller);
			return entityNonPlayable;
		}

		public void SetEntityObject(string name, CEntity entity){
			if (entity == null)
				return;
			if (m_ObjectPools.ContainsKey (name)) {
				m_ObjectPools [name].Set (entity);
			} else {
				m_ObjectPools [name] = new ObjectPool<CEntity> ();
				m_ObjectPools [name].Add (entity);
			}
			entity.transform.SetParent (this.transform);
		}

		private void RepairObject(CEntity entity, CObjectController controller) {
			entity.SetObjectSync (controller);
			entity.controlData = controller.GetData ();
			entity.SetPosition(controller.GetPosition());
			entity.SetStartPosition(controller.GetPosition());
			entity.name = "Network-" + entity.controlData.name;
			entity.transform.SetParent (this.transform);
		}
	
	}
}

