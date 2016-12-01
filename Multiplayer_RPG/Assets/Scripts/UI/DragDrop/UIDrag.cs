using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class UIDrag : UIPointer, IBeginDragHandler, IDragHandler, IEndDragHandler {

	#region Properties

	[SerializeField]	public GameObject root;
	[SerializeField]	public GameObject content;
	[SerializeField]	public GameObject target;
	[SerializeField]	private EDragState m_DragState = EDragState.Free;
	[SerializeField]	private UIGroup m_Group;

	public enum EDragState : int {
		Free 		= 0,
		BeginDrag 	= 1,
		Drag 		= 2,
		EndDrag 	= 3
	}

	private Vector2 m_StartPosition;
	private Transform m_TargetContent;
	private bool m_NeedUpdate = false;
	private Vector2 m_CurrentDirection;

	public UIDrop dropableObject;
	public Action<Vector2> OnEventBeginDrag;
	public Action<Vector2> OnEventDrag;
	public Action<Vector2, float> OnEventUpdateDrag;
	public Action<Vector2> OnEventEndDrag;

	public UnityEvent OnUnitBeginDrag;
	public UnityEvent OnUnitDrag;
	public UnityEvent OnUnitUpdateDrag;
	public UnityEvent OnUnitEndDrag;

	#endregion

	#region Monobehaviour

	protected override void Awake ()
	{
		base.Awake ();
		Init ();
	}

	protected override void OnEnable ()
	{
		base.OnEnable ();
		Init ();
	}

	protected override void Init() {
		base.Init ();
		m_StartPosition = target.transform.localPosition;
		m_TargetContent = target.transform.parent;
	}

	protected override void UpdateBaseTime (float dt)
	{
		base.UpdateBaseTime (dt);
		if (m_NeedUpdate) {
			if (OnEventUpdateDrag != null) {
				OnEventUpdateDrag (m_CurrentDirection, dt);
			}
			OnUnitUpdateDrag.Invoke();
		}
	}

	#endregion

	#region IDragHandler implementation

	void IBeginDragHandler.OnBeginDrag (PointerEventData eventData)
	{
		OnItemBeginDrag (eventData.position);
		if (OnEventBeginDrag != null) {
			OnEventBeginDrag(eventData.position);
		}
		OnUnitBeginDrag.Invoke();
		m_DragState = EDragState.BeginDrag;
		m_NeedUpdate = true;
		var eventPos = eventData.position;
		eventPos.x -= m_RectTransform.sizeDelta.x;
		eventPos.y -= m_RectTransform.sizeDelta.y;
		var direction = eventPos - m_StartPosition;
		m_CurrentDirection = direction.normalized;
	}

	public void OnDrag (PointerEventData eventData)
	{
		OnItemDrag (eventData.position);
		if (OnEventDrag != null) {
			OnEventDrag(eventData.position);
		}
		OnUnitDrag.Invoke();
		m_DragState = EDragState.Drag;
		m_NeedUpdate = true;
		var eventPos = eventData.position;
		eventPos.x -= m_RectTransform.sizeDelta.x;
		eventPos.y -= m_RectTransform.sizeDelta.y;
		var direction = eventPos - m_StartPosition;
		m_CurrentDirection = direction.normalized;
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		OnItemEndDrag (eventData.position);
		if (OnEventEndDrag != null) {
			OnEventEndDrag(eventData.position);
		}
		OnUnitEndDrag.Invoke();
		m_DragState = EDragState.EndDrag;
		m_NeedUpdate = false;
	}

	#endregion

	#region IMember implementation

	public override void Clear ()
	{
		base.Clear ();
		DestroyImmediate (target);
	}

	#endregion

	#region Main methods

	protected virtual void OnItemBeginDrag(Vector2 position) {
		target.transform.SetParent (root.transform);
		target.transform.position = position;
	}

	protected virtual void OnItemDrag(Vector2 position) {
		target.transform.position = position;
	}

	protected virtual void OnItemEndDrag(Vector2 position) {
		target.transform.SetParent (m_TargetContent.transform);
		target.transform.localPosition = m_StartPosition;
	}

	public void SetState(EDragState state) {
		m_DragState = state;
	}

	public EDragState GetState() {
		return m_DragState;
	}

	#endregion
}
