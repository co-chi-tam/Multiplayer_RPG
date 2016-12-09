using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CUIManager : CMonoSingleton<CUIManager> {

		[Header("Control")]
		[SerializeField]	private GameObject m_UIControlPanel;

		[Header("Info")]
		[SerializeField]	private GameObject m_UIInfoRootPanel;
		[SerializeField]	private CUIObjectInfo m_UIObjectInfoPrefab;

		public Action<CEnum.EAnimation> OnEventSkillInput;
		public Action<string> OnEventTalkInput;
		public Action<string> OnEventEmotionInput;

		protected override void Awake ()
		{
			base.Awake ();
			m_UIControlPanel.SetActive (false);
		}

		public override void FixedUpdateBaseTime (float dt)
		{
			base.FixedUpdateBaseTime (dt);
			if (Input.GetKeyDown (KeyCode.A)) {
				PressedBasicSkill ();
			}
		}

		public void RegisterUIControl(bool value, Action<CEnum.EAnimation> eventControl, 
			Action<string> eventTalk, 
			Action<string> eventEmotion) {
			m_UIControlPanel.SetActive (value);
			this.OnEventSkillInput = null;
			this.OnEventSkillInput = eventControl;
			this.OnEventTalkInput = eventTalk;
			this.OnEventEmotionInput = eventEmotion;	
		}

		public void RegisterUIInfo(IStatus value, bool showName, bool showStatus) {
			var uiInfoPrefab = Instantiate<CUIObjectInfo> (m_UIObjectInfoPrefab);
			var uiInfoRect = uiInfoPrefab.transform as RectTransform;
			uiInfoPrefab.owner = value;
			uiInfoPrefab.ShowName (showName);
			uiInfoPrefab.ShowStatus (showStatus);
			uiInfoRect.SetParent (m_UIInfoRootPanel.transform);
			uiInfoRect.anchoredPosition = Vector2.zero;
			uiInfoRect.localScale = Vector3.one;
			uiInfoRect.sizeDelta = Vector2.one;
		}

		public void PressEmotionItem(Image emotionImage) {
			if (this.OnEventEmotionInput != null) {
				this.OnEventEmotionInput (emotionImage.sprite.name);
			}
		}

		public void PressTalk(InputField inputTalk) {
			var submitText = inputTalk.text;
			if (string.IsNullOrEmpty (submitText))
				return;
			if (this.OnEventTalkInput != null) {
				this.OnEventTalkInput (submitText);
			}
			inputTalk.text = string.Empty;
		}

		public void PressedBasicSkill() {
			if (this.OnEventSkillInput != null) {
				this.OnEventSkillInput (CEnum.EAnimation.Attack_1);
			}
		}

		public void PressedSkill2() {
			if (this.OnEventSkillInput != null) {
				this.OnEventSkillInput (CEnum.EAnimation.Attack_2);
			}
		}

		public void PressedSkill3() {
			if (this.OnEventSkillInput != null) {
				this.OnEventSkillInput (CEnum.EAnimation.Attack_3);
			}
		}

		public void PressedSkill4() {
			if (this.OnEventSkillInput != null) {
				this.OnEventSkillInput (CEnum.EAnimation.Attack_4);
			}
		}

	}
}
