using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;

namespace SurvivalTest {
	public class UIJoytick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
	{

		[SerializeField]	private Image m_BackgroundImage;
		[SerializeField]	private Image m_KnobImage;

		public Vector3 InputDirection { get; set; }

		protected virtual void Start() {
			this.InputDirection = Vector3.zero;
			this.SetEnableJoytick (true);
		}

		protected virtual void SetEnableJoytick(bool value) {
			m_BackgroundImage.gameObject.SetActive (value);
			m_KnobImage.gameObject.SetActive (value);
		}

		protected virtual void Reset() {
			this.InputDirection = Vector3.zero;
			m_KnobImage.rectTransform.anchoredPosition = Vector2.zero;
		}

		#region Interface implementation

		public void OnBeginDrag (PointerEventData eventData)
		{
			
		}

		public void OnDrag (PointerEventData eventData)
		{
			var pos = Vector2.zero;
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_BackgroundImage.rectTransform, 
				eventData.position, 
				eventData.pressEventCamera, 
				out pos)) 
			{
				pos.x = (pos.x / m_BackgroundImage.rectTransform.sizeDelta.x);	
				pos.y = (pos.y / m_BackgroundImage.rectTransform.sizeDelta.y);	

				InputDirection = new Vector3 (pos.x * 2f, 0f, pos.y * 2f);
				InputDirection = InputDirection.magnitude > 1f ? InputDirection.normalized : InputDirection;

				m_KnobImage.rectTransform.anchoredPosition = new Vector2 (InputDirection.x * (m_BackgroundImage.rectTransform.sizeDelta.x / 3f) , 
					InputDirection.z * (m_BackgroundImage.rectTransform.sizeDelta.y / 3f));
			}
		}

		public void OnEndDrag (PointerEventData eventData)
		{
			this.Reset ();
		}

		public void OnPointerDown (PointerEventData eventData)
		{
			this.OnDrag (eventData);
		}

		public void OnPointerUp (PointerEventData eventData)
		{
			this.Reset ();
		}

		#endregion


	}
}

