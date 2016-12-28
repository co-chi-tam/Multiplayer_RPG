using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ObjectPool;

namespace SurvivalTest {
	public class CObjectManager : CMonoSingleton<CObjectManager> {

		#region Properties

		public Action<string, CBaseController> OnGetObject;
		public Action<string, CBaseController> OnSetObject;

		protected Dictionary<string, ObjectPool<CBaseController>> m_ObjectPools;

		#endregion

		#region Monobehaviour

		protected override void Awake ()
		{
			base.Awake ();
			m_ObjectPools = new Dictionary<string, ObjectPool<CBaseController>> ();
		}

		protected override void Start ()
		{
			base.Start ();
		}

		#endregion

		#region Main methods

		public CBaseController FindObject(string id, string name) {
			if (m_ObjectPools.ContainsKey (name)) {
				var pool = m_ObjectPools [name];
				pool.FindUsingItem ((x) => {
					return x.GetName() == name;
				});
			} 
			return null;
		}

		public void GetObjectModified(string name, Func<CBaseController, CBaseController> onModify) {
			var objGet = this.GetObject (name);
			if (onModify != null) {
				var objModify = onModify (objGet);
				if (this.OnGetObject != null) {
					this.OnGetObject (name, objModify);
				}
			}
		}

		public CBaseController GetObject(string name){
			if (m_ObjectPools.ContainsKey (name)) {
				var objGet = m_ObjectPools [name].Get ();
				if (objGet != null) {
					objGet.transform.SetParent (this.transform);
					return objGet;
				}
			} else {
				m_ObjectPools [name] = new ObjectPool<CBaseController> ();
			}
			var loadedObj = CResourceManager.Instance.LoadResourceOrAsset<CBaseController> (name);
			if (loadedObj != null) {
				var newObj = Instantiate (loadedObj);
				m_ObjectPools [name].Set (newObj);
				var objAlready = m_ObjectPools [name].Get();
				objAlready.transform.SetParent (this.transform);
				return objAlready;
			}
			return null;
		}

		public void SetObject(string name, CBaseController obj) {
			if (obj == null) 
				return;
			if (m_ObjectPools.ContainsKey (name)) {
				// TODO
			} else {
				m_ObjectPools [name] = new ObjectPool<CBaseController> ();
			}
			m_ObjectPools [name].Set (obj);
			if (this.OnSetObject != null) {
				this.OnSetObject (name, obj);
			}
			obj.transform.SetParent (this.transform);
		}

		#endregion

	}
}

