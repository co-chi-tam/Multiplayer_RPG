﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ObjectPool;

namespace SurvivalTest {
	public class CObjectManager : CMonoSingleton<CObjectManager> {

		public Action<string, CBaseController> OnGetObject;
		public Action<string, CBaseController> OnSetObject;

		private Dictionary<string, ObjectPool<CBaseController>> m_ObjectPools;
		private Queue<CBaseController> m_QueuePools;

		protected override void Awake ()
		{
			base.Awake ();
			m_ObjectPools = new Dictionary<string, ObjectPool<CBaseController>> ();
		}

		protected override void Start ()
		{
			base.Start ();
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
			var resourceLoads = Resources.LoadAll<CBaseController> ("Prefabs");
			for (int i = 0; i < resourceLoads.Length; i++) {
				if (resourceLoads [i].name == name) {
					var newObj = Instantiate (resourceLoads [i]);
					m_ObjectPools [name].Create (newObj);
					var objAlready = m_ObjectPools [name].Get();
					objAlready.transform.SetParent (this.transform);
					return objAlready;
				}
			}
			return null;
		}

		public void SetObject(string name, CBaseController obj) {
			if (obj == null) 
				return;
			if (m_ObjectPools.ContainsKey (name)) {
				m_ObjectPools [name].Set (obj);
			} else {
				m_ObjectPools [name] = new ObjectPool<CBaseController> ();
				m_ObjectPools [name].Add (obj);
			}
			if (this.OnSetObject != null) {
				this.OnSetObject (name, obj);
			}
			obj.transform.SetParent (this.transform);
		}

	}
}

