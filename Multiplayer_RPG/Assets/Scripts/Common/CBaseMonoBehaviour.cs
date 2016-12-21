using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	
	public class CBaseMonoBehaviour : MonoBehaviour {

		#region Properties

		public CEventListener OnObjectVisible;
		public CEventListener OnObjectInvisible;

		protected Transform m_Transform;
		protected bool m_IsVisible = true;

		#endregion

		#region Implementation MonoBehaviour

		protected virtual void OnEnable() {
		
		}

		protected virtual void OnDisable() {
			
		}

		public virtual void Init() {

		}

		protected virtual void Awake() {
			m_Transform = this.transform;
			OnObjectVisible = new CEventListener ();
			OnObjectInvisible = new CEventListener ();
		}

		protected virtual void Start () {
		
		}
		
		protected virtual void Update () {
			this.UpdateBaseTime (Time.deltaTime);
		}

		protected virtual void FixedUpdate() {
			this.FixedUpdateBaseTime (Time.fixedDeltaTime);
		}

		protected virtual void LateUpdate() {
			this.LateUpdateBaseTime (Time.deltaTime);
		}

		public virtual void UpdateBaseTime(float dt) {
			
		}

		public virtual void FixedUpdateBaseTime(float dt) {

		}

		public virtual void LateUpdateBaseTime(float dt) {

		}

		public virtual void OnBecameVisible() {
			m_IsVisible = true;
			if (this.OnObjectVisible != null) {
				this.OnObjectVisible.Invoke (this);
			}
		}

		public virtual void OnBecameInvisible() {
			m_IsVisible = false;
			if (this.OnObjectInvisible != null) {
				this.OnObjectInvisible.Invoke (this);
			}
		}

		protected virtual void OnDrawGizmos() {
			
		}

		#endregion

		#region Getter && Setter

		public virtual string GetID() {
			return string.Empty;
		}

		public virtual void SetID(string value) {
				
		}

		public virtual void SetActive(bool value) {
			this.gameObject.SetActive (value);
		}

		public virtual bool GetActive() {
			if (this == null)
				return false;
			return this.gameObject.activeInHierarchy;
		}

		public virtual void SetPosition(Vector3 value) {
			m_Transform.position = value;
		}

		public virtual Vector3 GetPosition() {
			if (m_Transform == null)
				return Vector3.zero;
			return m_Transform.position;
		}

		public virtual void SetRotation(Vector3 value) {
			m_Transform.rotation = Quaternion.Euler (value);
		}

		public virtual Vector3 GetRotation() {
			if (m_Transform == null)
				return Vector3.zero;
			return m_Transform.rotation.eulerAngles;
		}

		public virtual bool GetIsVisible() {
			return m_IsVisible;
		}

		#endregion

	}
}