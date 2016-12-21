using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	[RequireComponent(typeof(Animator))]
	public class AnimatorCustom : MonoBehaviour, IAnimation {

		#region Properties

		private Animator m_Animator;
		private Dictionary<string, Action<string>> m_AnimatorEvents;

		#endregion

		#region MonoBehaviour

		protected virtual void Awake() {
			m_Animator = this.GetComponent<Animator> ();
			m_AnimatorEvents = new Dictionary<string, Action<string>> ();
		}

		#endregion

		#region Main methods

		public void InvokeAnimation(string name) {
			if (m_AnimatorEvents.ContainsKey (name) == false)
				return;
			m_AnimatorEvents[name] (name);
		}

		public bool AddEventAnimation(string animationName, string nameEvent, float time, Action<string> callback) {
			var animationClips = m_Animator.runtimeAnimatorController.animationClips;
			for (int i = 0; i < animationClips.Length; i++) {
				var clip = animationClips [i];
				if (clip.name == animationName) {
					var animationEvent = new AnimationEvent ();
					animationEvent.functionName = "Invoke";
					animationEvent.stringParameter = nameEvent;
					animationEvent.time = time;
					RegisterAnimation (nameEvent, callback);
					return true;
				}
			}
			return false;
		}

		public void RegisterAnimation(string name, Action<string> callback) {
			if (m_AnimatorEvents.ContainsKey (name))
				return;
			m_AnimatorEvents.Add (name, callback);
		}

		public void UnRegisterAnimation(string name) {
			if (m_AnimatorEvents.ContainsKey (name) == false)
				return;
			m_AnimatorEvents.Remove (name);
		}

		public void RemoveAllEvent() {
			m_AnimatorEvents.Clear ();
		}

		#endregion

		#region Getter && Setter

		public float GetTime() {
			if (m_Animator == null)
				return 0f;
			return (float)m_Animator.GetTime ();
		}

		public void SetTime(float value) {
			if (m_Animator == null)
				return;
			m_Animator.SetTime ((double) value);
		}

		public void SetInteger(string name, int value) {
			if (m_Animator == null)
				return;
			m_Animator.SetInteger (name, value);
		}

		public void SetFloat(string name, float value) {
			if (m_Animator == null)
				return;
			m_Animator.SetFloat (name, value);
		}

		public void SetBool(string name, bool value) {
			if (m_Animator == null)
				return;
			m_Animator.SetBool (name, value);
		}

		public void SetTrigger(string name) {
			if (m_Animator == null)
				return;
			m_Animator.SetTrigger (name);
		}

		public int GetInteger(string name) {
			if (m_Animator == null)
				return 0;
			return m_Animator.GetInteger (name);
		}

		public float GetFloat(string name) {
			if (m_Animator == null)
				return 0f;
			return m_Animator.GetFloat (name);
		}

		public bool GetBool(string name) {
			if (m_Animator == null)
				return false;
			return m_Animator.GetBool (name);
		}

		public virtual void SetEnable(bool value) {
			if (m_Animator == null)
				return;
			this.enabled = value;
			this.m_Animator.enabled = value;
		}

		public virtual bool GetEnable() {
			if (m_Animator == null)
				return false;
			return this.enabled && this.m_Animator.enabled;
		}

		#endregion

	}
}
