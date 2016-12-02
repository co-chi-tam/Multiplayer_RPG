using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CBaseController : CBaseMonoBehaviour {

		#region Properties

		protected bool m_Active;

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

		public override void SetActive (bool value)
		{
//			base.SetActive (value);
			m_Active = value;
		}

		public override bool GetActive() {
			base.GetActive ();
			return m_Active;
		}

		#endregion

	}
}
