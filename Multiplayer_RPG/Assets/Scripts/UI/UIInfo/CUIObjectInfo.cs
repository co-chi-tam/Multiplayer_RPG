using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CUIObjectInfo : CBaseMonoBehaviour {

		[SerializeField]	private Text m_UIObjNameText;
		[SerializeField]	private Image m_UIObjHPImage; 
		[SerializeField]	private Text m_UIObjTalkText; 

		[SerializeField]	private float m_TalkTextShowTimeInterval = 10f;

		public IStatus owner;
		private CountdownTime m_ShowTalkTextCountdown;
		private List<string> m_TalkQueue;
		private int m_TalkIndex = 0;

		protected override void Awake ()
		{
			base.Awake ();
			m_TalkQueue = new List<string> ();
			m_ShowTalkTextCountdown = new CountdownTime (m_TalkTextShowTimeInterval, true);
		}

		protected override void LateUpdate ()
		{
			base.LateUpdate ();
			if (owner != null && owner.GetActive () && owner.GetIsVisible ()) {
				// Info
				UpdateOwnerInfo ();
				// Position
				UpdateScreenPosition ();
				// Communicate
				UpdateTalkText ();
			} else {
				DestroyImmediate (this.gameObject);
			}
		}

		public void ShowName(bool value) {
			m_UIObjNameText.gameObject.SetActive (value);
		}

		public void ShowStatus(bool value) {
			m_UIObjHPImage.gameObject.SetActive (value);
		}

		private void UpdateOwnerInfo() {
			this.name = "UIInfo-" + owner.GetName ();
			m_UIObjNameText.text = owner.GetName ();
			m_UIObjHPImage.fillAmount = (float)owner.GetCurrentHealth () / owner.GetMaxHealth ();
		}

		private void UpdateScreenPosition() {
			var ownerPosition = owner.GetPosition ();
			ownerPosition.y = owner.GetHeight () * 2f;
			var screenPosition = Camera.main.WorldToScreenPoint (ownerPosition);
			m_Transform.position = screenPosition;
		}

		private void UpdateTalkText() {
			if (string.IsNullOrEmpty(owner.GetTalk ()) == false && 
				m_TalkQueue.Contains (owner.GetTalk ()) == false) {
				this.ShowTalk (owner.GetTalk ());
			}
			if (m_TalkQueue.Count != m_TalkIndex) {
				m_UIObjTalkText.text = m_TalkQueue[m_TalkIndex];
				m_TalkIndex++;
			}  else {
				if (m_ShowTalkTextCountdown.UpdateTime (Time.deltaTime)) {
					m_UIObjTalkText.text = string.Empty;
				}
			}
		}

		public void ShowTalk(string value) {
			var formatedText = value.Split (new String[] { ":=:" }, StringSplitOptions.RemoveEmptyEntries);
			if (formatedText.Length == 2) {
				var textLength = formatedText [1].Length > 30 ? 30 : formatedText [1].Length;
				var rightText = formatedText [1].Substring (0, textLength);
				m_TalkQueue.Add (rightText);
			}
		}

	}
}
