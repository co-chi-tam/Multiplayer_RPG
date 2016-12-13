using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ObjectPool;

namespace SurvivalTest {
	public class CObjectManager : CMonoSingleton<CObjectManager> {

		public Action OnGetObject;
		public Action OnSetObject;

		private Dictionary<string, ObjectPool<CBaseController>> m_ObjectPools;

		protected override void Awake ()
		{
			base.Awake ();
			m_ObjectPools = new Dictionary<string, ObjectPool<CBaseController>> ();
		}

		protected override void Start ()
		{
			base.Start ();
		}

		public CBaseController GetObject(string name){
			if (m_ObjectPools.ContainsKey (name)) {
				var objGet = m_ObjectPools [name].Get ();
				if (objGet != null) {
					if (this.OnGetObject != null) {
						this.OnGetObject ();
					}
					return objGet;
				}
			} else {
				m_ObjectPools [name] = new ObjectPool<CBaseController> ();
			}
			var resourceLoads = Resources.LoadAll<CBaseController> ("Prefabs");
			for (int i = 0; i < resourceLoads.Length; i++) {
				if (resourceLoads [i].name == name) {
					var newObj = Instantiate (resourceLoads [i]);
					m_ObjectPools [name].Create (newObj);
					var objAlready = m_ObjectPools [name].Get();
					if (this.OnGetObject != null) {
						this.OnGetObject ();
					}
					return objAlready;
				}
			}
			return null;
		}

		public void SetObject(string name, CBaseController obj) {
			if (m_ObjectPools.ContainsKey (name)) {
				m_ObjectPools [name].Set (obj);
			} else {
				m_ObjectPools [name] = new ObjectPool<CBaseController> ();
				m_ObjectPools [name].Add (obj);
			}
			if (this.OnSetObject != null) {
				this.OnSetObject ();
			}
		}
	
	}
}

