using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

	namespace SurvivalTest {
	public class CMoveAvoidanceComponent : CMovableComponent {

		public LayerMask obstacles;

		public float distance;
		public float radiusBase = 0f;
		protected float m_SpeedThreshold;

		private float[] m_AngleCheckings = new float[] { 0, -15, 15, -45, 45, -90, 90 }; 
		private float[] m_AngleAvoidances = new float[] { 10, 40, -40, 40, -40, 80, -80 }; 
		private float[] m_lengthAvoidances = new float[] { 3f, 3f, 3f, 3f, 3f, 1.5f, 1.5f }; 

		public CMoveAvoidanceComponent (IMovable movable, NavMeshAgent navMeshAgent) : base(movable, navMeshAgent)
		{
			m_SpeedThreshold = 1f;
		}

		public override void UpdateComponent(float dt) {
			base.UpdateComponent (dt);
		} 

		public override void MoveForwardToTarget(float dt) {
			if (DidMoveToTarget() == false) {
				m_Direction = targetPosition - currentTransform.position;
				m_Angle = Mathf.Atan2 (m_Direction.x, m_Direction.z) * Mathf.Rad2Deg;
				DrawRayCast ();
				var position = currentTransform.forward * m_Target.GetMoveSpeed() * dt * m_SpeedThreshold;
				if (position != Vector3.zero) {
					if (m_NavMeshAgent.isOnNavMesh) {
						m_NavMeshAgent.Move (position);
					} else {
						currentTransform.position = Vector3.Lerp (currentTransform.position, currentTransform.position + position, 0.5f);
					}
				}
				currentTransform.rotation = Quaternion.Lerp (currentTransform.rotation, Quaternion.AngleAxis (m_Angle, Vector3.up), 0.1f);
				#if UNITY_EDITOR
				Debug.DrawRay (currentTransform.position, m_Direction, Color.green);	
				#endif
			}
			Reset ();
		}

		public override void LookForwardToTarget(Vector3 value) {
			if (currentTransform != null) {
				m_Direction = value - currentTransform.position;
				var forward = currentTransform.forward;
				m_Angle = Mathf.Atan2 (m_Direction.x, m_Direction.z) * Mathf.Rad2Deg;
				currentTransform.rotation = Quaternion.Lerp (currentTransform.rotation, Quaternion.AngleAxis (m_Angle, Vector3.up), 0.1f);
			}
		}

		private void DrawRayCast() {
			var forward = currentTransform.forward;
			var tmpAngle = m_Angle;
			var tmpSpeedThreshold = m_SpeedThreshold;
			for (int i = 0; i < m_AngleCheckings.Length; i++) {
				var rayCast = Quaternion.AngleAxis(m_AngleCheckings[i], currentTransform.up) * forward * m_lengthAvoidances[i];
				RaycastHit rayCastHit;
				if (Physics.Raycast (currentTransform.position + (rayCast.normalized * radiusBase), rayCast, out rayCastHit, m_lengthAvoidances[i], obstacles)) {
					var movableHitName = rayCastHit.collider.gameObject.GetInstanceID ().ToString();
					var avoidance = true;
					if (CMovableComponent.MovableObjects.ContainsKey (movableHitName) == true) {
						avoidance = CMovableComponent.MovableObjects [movableHitName].GetIsObstacle();
					} 
					if (avoidance == true) {
						tmpAngle += m_AngleAvoidances [i] * (1f - (rayCastHit.distance / m_lengthAvoidances[i]));
						tmpSpeedThreshold -= 1f / ((float)m_AngleCheckings.Length / 1.15f);
					}
				} 
				#if UNITY_EDITOR
				Debug.DrawRay (currentTransform.position + (rayCast.normalized * radiusBase), rayCast, Color.white);
				#endif
			}
			m_Angle = tmpAngle;
			m_SpeedThreshold = tmpSpeedThreshold;
		}

		protected override void Reset() {
			m_SpeedThreshold = 1f;
		}

		public override bool DidMoveToTarget() {
			if (currentTransform == null)
				return true;
			m_Direction = targetPosition - currentTransform.position;
			return m_Direction.sqrMagnitude <= distance * distance;
		}

		public override bool DidMoveToTarget(Transform target) {
			if (target == null)
				return true;
			if (currentTransform == null)
				return true;
			var direction = target.position - currentTransform.position;
			return direction.sqrMagnitude <= distance * distance;
		}

		public override bool DidMoveToTarget(Vector3 target) {
			if (currentTransform == null)
				return true;
			var direction = target - currentTransform.position;
			return direction.sqrMagnitude <= distance * distance;
		}

	}
}