using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CameraController : CMonoSingleton<CameraController> {

	    public Transform target;	
		public Vector3 m_OffsetPosition;
		public Vector3 m_OffsetRotation;

		protected override void Awake ()
		{
			base.Awake ();
		}

		protected override void Update() {
			if (target != null) {
				m_Transform.position = Vector3.Lerp (m_Transform.position, target.transform.position + m_OffsetPosition, 0.1f);
	//			var direction = target.transform.position - m_Transform.position;
	//			m_Transform.rotation = Quaternion.LookRotation (direction + m_OffsetRotation);
			}
		}
	}
}