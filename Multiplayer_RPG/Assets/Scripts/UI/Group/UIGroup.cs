using UnityEngine;
using System.Collections;
using SurvivalTest;

public class UIGroup : CBaseMonoBehaviour, IGroup {

	#region Properties

	[SerializeField]	private string m_GroupName = "group 1";
	[SerializeField]	public UIMember[] members;

	#endregion

	#region Monobehaviour

	protected override void Awake ()
	{
		base.Awake ();
	}

	public override void UpdateBaseTime (float dt)
	{
		base.UpdateBaseTime (dt);
	}

	#endregion

	public string GetGroupName() {
		return m_GroupName;
	}

}
