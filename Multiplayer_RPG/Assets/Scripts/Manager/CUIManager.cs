using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using ObjectPool;

namespace SurvivalTest {
	public class CUIManager : CMonoSingleton<CUIManager> {

		#region Properties

		[Header("Control")]
		[SerializeField]	private GameObject m_UIControlPanel;

		[Header("Inventory")]
		[SerializeField]	private CUIInventory m_UIInventory;

		[Header("Info")]
		[SerializeField]	private GameObject m_UIInfoRootPanel;
		[SerializeField]	private CUIObjectInfo m_UIObjectInfoPrefab;

		[Header("Character Status")]
		[SerializeField]	private CUImageFillAmount m_UIHealthStatusImage;
		[SerializeField]	private CUImageFillAmount m_UISanityStatusImage;
		[SerializeField]	private CUImageFillAmount m_UIHungerStatusImage;

		[Header("Event")]
		public UnityEvent OnAddItemInventory;
		[Header("Event Touch")]
		public UnityEvent OnTouchDownScreen;
		public UnityEvent OnTouchUpScreen;
		public UnityEvent OnTouchMovedScreen;
		public UnityEvent OnTouchStationaryScreen;

		public Action<CEnum.EAnimation> OnEventSkillInput;
		public Action<string> OnEventTalkInput;
		public Action<string> OnEventEmotionInput;

		private Vector3 m_CurrentTouchPosition;
		private ObjectPool<CUIObjectInfo> m_UIObjInfoPool;

		#endregion

		#region MonoBehaviour

		protected override void Awake ()
		{
			base.Awake ();
			this.m_UIControlPanel.SetActive (false);
			this.m_UIInfoRootPanel.SetActive (false);
			this.m_UIObjInfoPool = new ObjectPool<CUIObjectInfo> ();
		}

		protected override void Start ()
		{
			base.Start ();
		}

		public override void LateUpdateBaseTime (float dt)
		{
			base.LateUpdateBaseTime (dt);
#if UNITY_EDITOR || UNITY_STANDALONE
			if (Input.GetMouseButtonDown(0)) {
				this.OnTouchDownScreen.Invoke ();
				m_CurrentTouchPosition = Input.mousePosition;
			} else if (Input.GetMouseButtonUp(0)) {
				this.OnTouchUpScreen.Invoke ();
			} else if (Input.GetMouseButton(0)) {
				if (m_CurrentTouchPosition != Input.mousePosition) {
					this.OnTouchMovedScreen.Invoke ();
					m_CurrentTouchPosition = Input.mousePosition;
				} else {
					this.OnTouchStationaryScreen.Invoke ();
				}
			}
#elif UNITY_ANDROID
			if (Input.touchCount > 0) {
				var touchInput = Input.GetTouch (0);
				var touchPhase = touchInput.phase;
				switch (touchPhase) {
				default:
				case TouchPhase.Began:
					this.OnTouchDownScreen.Invoke ();
					break;
				case TouchPhase.Ended:
					this.OnTouchUpScreen.Invoke ();
					break;
				case TouchPhase.Moved:
					this.OnTouchMovedScreen.Invoke ();
					m_CurrentTouchPosition = touchInput.position;
					break;
				case TouchPhase.Stationary:
					this.OnTouchStationaryScreen.Invoke ();
					m_CurrentTouchPosition = touchInput.position;
					break;
				}
			} 
#endif
		}

		#endregion

		#region Main methods

		public void LoadInventoryItems(IItem[] items, Action<object> onExecuteObject) {
			m_UIInventory.LoadItems (items, onExecuteObject);
		}

		public void RegisterUIControl(bool value, Action<CEnum.EAnimation> eventControl, 
				Action<string> eventTalk, 
				Action<string> eventEmotion) {
			this.m_UIControlPanel.SetActive (value);
			// Reset
			this.OnEventSkillInput = null;
			this.OnEventTalkInput = null;
			this.OnEventEmotionInput = null;
			// Register
			this.OnEventSkillInput = eventControl;
			this.OnEventTalkInput = eventTalk;
			this.OnEventEmotionInput = eventEmotion;
		}

		public void RegisterUIStatus(IEventListener target) {
			this.m_UIHealthStatusImage.SetTarget (target);
			this.m_UISanityStatusImage.SetTarget (target);
			this.m_UIHungerStatusImage.SetTarget (target);
		}

		public void RegisterUIInfo(IStatus value, bool showName, bool showStatus) {
			this.m_UIInfoRootPanel.SetActive (true);
			var waitingToCreate = 2;
			while (waitingToCreate > 0) {
				var uiInfoPool = this.m_UIObjInfoPool.Get ();
				if (uiInfoPool != null) {
					var uiInfoRect = uiInfoPool.transform as RectTransform;
					uiInfoPool.owner = value;
					uiInfoPool.ShowName (showName);
					uiInfoPool.ShowStatus (showStatus);
					uiInfoRect.SetParent (m_UIInfoRootPanel.transform);
					// Reset position
					uiInfoRect.anchoredPosition = Vector2.zero;
					uiInfoRect.localScale = Vector3.one;
					uiInfoRect.sizeDelta = Vector2.one;
					break;
				} else {
					var uiInfoPrefab = Instantiate<CUIObjectInfo> (m_UIObjectInfoPrefab);
					this.m_UIObjInfoPool.Set (uiInfoPrefab);
					uiInfoPrefab.owner = value;
				}
				waitingToCreate--;
			}
		}

		public void UnregisterUIInfo(CUIObjectInfo value) {
			this.m_UIObjInfoPool.Set (value);
		}

		public void PressEmotionItem(Image emotionImage) {
			if (this.OnEventEmotionInput != null) {
				this.OnEventEmotionInput (emotionImage.sprite.name);
			}
		}

		public void SubmitChat(InputField inputTalk) {
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

		#endregion

	}
}
