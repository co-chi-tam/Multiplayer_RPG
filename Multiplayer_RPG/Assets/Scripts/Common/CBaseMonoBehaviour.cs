using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	
	public class CBaseMonoBehaviour : MonoBehaviour {

		#region Properties

		protected Transform m_Transform;
		protected bool m_IsVisible = true;

		#endregion

		#region Implementation MonoBehaviour

		protected virtual void OnEnable() {
		
		}

		protected virtual void OnDisable() {
			
		}

		protected virtual void Init() {

		}

		protected virtual void Awake() {
			m_Transform = this.transform;
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
			
		}

		public virtual void UpdateBaseTime(float dt) {
			
		}

		public virtual void FixedUpdateBaseTime(float dt) {

		}

		public virtual void OnBecameVisible() {
			m_IsVisible = true;
		}

		public virtual void OnBecameInvisible() {
			m_IsVisible = false;
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
			return m_Transform.position;
		}

		public virtual void SetRotation(Vector3 value) {
			m_Transform.rotation = Quaternion.Euler (value);
		}

		public virtual Vector3 GetRotation() {
			return m_Transform.rotation.eulerAngles;
		}

		public virtual bool GetIsVisible() {
			return m_IsVisible;
		}

		#endregion

	}
}