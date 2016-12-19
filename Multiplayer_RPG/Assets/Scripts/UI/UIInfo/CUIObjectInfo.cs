using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CUIObjectInfo : CBaseMonoBehaviour {

		#region Properties

		[SerializeField]	private Text m_UIObjNameText;
		[SerializeField]	private Image m_UIObjHPImage; 
		[SerializeField]	private Text m_UIObjChatText;
		[SerializeField]	private Image m_UIEmotionImage;  

		public IStatus owner;
		// Chat
		[SerializeField]	private float m_ChatShowTimeInterval = 10f;
		private CountdownTime m_ChatTextCountdown;
		private List<string> m_ChatQueue;
		private int m_ChatTextIndex = 0;
		// Emotion
		[SerializeField]	private float m_EmotionShowTimeInterval = 15f;
		private CountdownTime m_EmotionCountdown;
		private List<string> m_EmotionQueue;
		private int m_EmotionIndex = 0;

		#endregion

		#region Monoehaviour

		protected override void Awake ()
		{
			base.Awake ();
			m_ChatQueue = new List<string> ();
			m_EmotionQueue = new List<string> ();
			m_ChatTextCountdown = new CountdownTime (m_ChatShowTimeInterval, true);
			m_EmotionCountdown = new CountdownTime (m_EmotionShowTimeInterval, true);

			m_UIEmotionImage.gameObject.SetActive (false);
		}

		protected override void LateUpdate ()
		{
			base.LateUpdate ();
			if (owner != null && owner.GetIsVisible ()) {
				// Info
				this.UpdateOwnerInfo ();
				// Position
				this.UpdateScreenPosition ();
				// Chat
				this.UpdateChatText ();
				// Emotion
				this.UpdateEmotionImage ();
			} 
			if (owner.GetActive () == false) {
				this.OnOwnerInactive ();
			}
		}

		#endregion

		#region Main methods

		public void OnOwnerInactive() {
			// Chat
			this.m_ChatTextIndex = 0;
			this.m_ChatQueue.Clear ();
			this.m_ChatTextCountdown.Reset ();
			// Emotion
			this.m_EmotionIndex = 0;
			this.m_EmotionQueue.Clear ();
			this.m_EmotionCountdown.Reset ();
			// Object
			this.gameObject.SetActive (false);
			CUIManager.Instance.UnregisterUIInfo (this);
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

		private void UpdateChatText() {
			if (string.IsNullOrEmpty(owner.GetChat ()) == false && 
				m_ChatQueue.Contains (owner.GetChat ()) == false) {
				this.ShowChat (owner.GetChat ());
			}
			if (m_ChatQueue.Count != m_ChatTextIndex) {
				m_ChatTextCountdown.Reset ();
				m_UIObjChatText.text = this.FormatChatText (m_ChatQueue[m_ChatTextIndex]);
				m_ChatTextIndex++;
			}  else if (m_ChatTextCountdown.UpdateTime (Time.deltaTime)) {
				m_UIObjChatText.text = string.Empty;
			}
		}

		private void UpdateEmotionImage() {
			if (string.IsNullOrEmpty(owner.GetEmotion ()) == false && 
				m_EmotionQueue.Contains (owner.GetEmotion ()) == false) {
				this.ShowEmotion (owner.GetEmotion ());
			}
			if (m_EmotionQueue.Count != m_EmotionIndex) {
				m_EmotionCountdown.Reset ();
				m_UIEmotionImage.gameObject.SetActive (true);
				m_UIEmotionImage.sprite = CUtil.FindSprite (this.FormatEmotionText(m_EmotionQueue[m_EmotionIndex]));
				m_EmotionIndex++;
			}  else if (m_EmotionCountdown.UpdateTime (Time.deltaTime)) {
				m_UIEmotionImage.gameObject.SetActive (false);
			}
		}

		public void ShowChat(string value) {
			if (m_ChatQueue.Contains (value) == false) {
				m_ChatQueue.Add (value);
			}
		}

		public void ShowEmotion(string value) {
			if (m_EmotionQueue.Contains (value) == false) {
				m_EmotionQueue.Add (value);
			}
		}

		public string FormatChatText(string value) {
			var formatedText = value.Split (new String[] { ":=:" }, StringSplitOptions.RemoveEmptyEntries);
			if (formatedText.Length == 2) {
				var textLength = formatedText [1].Length > 30 ? 30 : formatedText [1].Length;
				var rightText = formatedText [1].Substring (0, textLength);
				return rightText;
			}
			return string.Empty;
		}

		public string FormatEmotionText(string value) {
			var formatedText = value.Split (new String[] { ":=:" }, StringSplitOptions.RemoveEmptyEntries);
			if (formatedText.Length == 2) {
				return formatedText[1];
			}
			return string.Empty;
		}

		#endregion

	}
}
