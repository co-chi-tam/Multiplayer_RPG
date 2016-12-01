using UnityEngine;
using System.Collections;
using SurvivalTest;

public class UIMember : MonoBehaviour, IMember {

	#region Properties

	protected Transform m_Transform;
	protected RectTransform m_RectTransform;

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
		m_RectTransform = this.transform as RectTransform;
	}

	protected virtual void Start () {

	}

	protected virtual void Update () {
		this.UpdateBaseTime (Time.deltaTime);
	}

	protected virtual void UpdateBaseTime(float dt) {

	}

	#endregion

	public virtual void Clear() {
		
	}

}
