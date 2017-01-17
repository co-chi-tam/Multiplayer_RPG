using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

namespace UICustomize {
	[RequireComponent (typeof(MaskableGraphic))]
	public class CClickable : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, 
									IPointerEnterHandler, IPointerExitHandler, 
									IPointerClickHandler
	{

		#region Properties

		public UnityEvent OnEventPointerUp;
		public UnityEvent OnEventPointerDown;
		public UnityEvent OnEventPointerEnter;
		public UnityEvent OnEventPointerExit;
		public UnityEvent OnEventPointerClick;

		#endregion

		#region Interface implementation

		public void OnPointerUp (PointerEventData eventData)
		{
			OnEventPointerUp.Invoke ();
		}

		public void OnPointerDown (PointerEventData eventData)
		{
			OnEventPointerDown.Invoke ();
		}

		public void OnPointerEnter (PointerEventData eventData)
		{
			OnEventPointerEnter.Invoke ();
		}

		public void OnPointerExit (PointerEventData eventData)
		{
			OnEventPointerExit.Invoke ();
		}

		public void OnPointerClick (PointerEventData eventData)
		{
			OnEventPointerClick.Invoke ();
		}

		#endregion
	}
}
