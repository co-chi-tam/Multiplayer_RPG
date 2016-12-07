using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CUIManager : CMonoSingleton<CUIManager> {

		[SerializeField]	private GameObject m_UIInfoRootPanel;
		[SerializeField]	private CUIObjectInfo m_UIObjectInfoPrefab;

		public Action<CEnum.EAnimation> OnEventInputSkill;

		protected override void Awake ()
		{
			base.Awake ();
		}

		public override void FixedUpdateBaseTime (float dt)
		{
			base.FixedUpdateBaseTime (dt);
			if (Input.GetKeyDown (KeyCode.A)) {
				PressedBasicSkill ();
			}
			if (Input.GetKeyDown (KeyCode.B)) {
				PressedSkill2 ();
			}
			if (Input.GetKeyDown (KeyCode.X)) {
				PressedSkill3 ();
			}
			if (Input.GetKeyDown (KeyCode.Y)) {
				PressedSkill4 ();
			}
		}

		public void RegisterUIInfo(IStatus value) {
			var uiInfoPrefab = Instantiate<CUIObjectInfo> (m_UIObjectInfoPrefab);
			var uiInfoRect = uiInfoPrefab.transform as RectTransform;
			uiInfoPrefab.owner = value;
			uiInfoRect.SetParent (m_UIInfoRootPanel.transform);
			uiInfoRect.anchoredPosition = Vector2.zero;
			uiInfoRect.localScale = Vector3.one;
			uiInfoRect.sizeDelta = Vector2.one;
		}

		public void PressedBasicSkill() {
			if (OnEventInputSkill != null) {
				OnEventInputSkill (CEnum.EAnimation.Attack_1);
			}
		}

		public void PressedSkill2() {
			if (OnEventInputSkill != null) {
				OnEventInputSkill (CEnum.EAnimation.Attack_2);
			}
		}

		public void PressedSkill3() {
			if (OnEventInputSkill != null) {
				OnEventInputSkill (CEnum.EAnimation.Attack_3);
			}
		}

		public void PressedSkill4() {
			if (OnEventInputSkill != null) {
				OnEventInputSkill (CEnum.EAnimation.Attack_4);
			}
		}

	}
}
