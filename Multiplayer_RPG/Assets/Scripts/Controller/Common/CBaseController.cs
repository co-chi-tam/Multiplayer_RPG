using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CBaseController : CBaseMonoBehaviour {

		#region Properties

		protected bool m_Active;
		protected bool m_Enable;

		#endregion

		#region Implementation MonoBehaviour

		protected override void Awake ()
		{
			base.Awake ();
		}

		#endregion

		#region Main methods

		public virtual void OnDestroyObject() {
			DestroyImmediate (this.gameObject);
		}

		#endregion

		#region Getter && Setter 

		public virtual void SetName(string value) {
			gameObject.name = value;
		}

		public virtual string GetName() {
			return gameObject.name;
		}	

		public override void SetActive (bool value)
		{
			base.SetActive (value);
			m_Active = value;
		}

		public override bool GetActive() {
			base.GetActive ();
			return m_Active;
		}

		public virtual void SetEnable (bool value)
		{
			m_Enable = value;
		}

		public virtual bool GetEnable() {
			return m_Enable;
		}

		#endregion

	}
}
