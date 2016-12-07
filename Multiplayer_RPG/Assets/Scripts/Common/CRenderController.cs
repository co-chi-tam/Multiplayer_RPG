using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CRenderController : CBaseMonoBehaviour {

		private IStatus m_Root;

		protected override void Awake ()
		{
			base.Awake ();
			m_Root = this.GetComponentInParent<IStatus> ();
		}

		public override void OnBecameVisible() {
			base.OnBecameVisible ();
			m_Root.OnBecameVisible ();
		}

		public override void OnBecameInvisible() {
			base.OnBecameInvisible ();
			m_Root.OnBecameInvisible ();
		}

	}
}
