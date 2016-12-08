using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

namespace SurvivalTest {
	[CanEditMultipleObjects]
	[CustomEditor (typeof(CObjectController), true)]
	public class CObjectInspector : Editor {

		private IStatus m_Target;

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();
			m_Target = target as IStatus;
			if (Application.isPlaying == false)
				return;
			GUILayout.Label ("***Status***");
			EditorGUILayout.LabelField ("Active:", m_Target.GetActive ().ToString ());
			EditorGUILayout.LabelField ("Animation:", m_Target.GetAnimation ().ToString ());
			EditorGUILayout.LabelField ("Object Type:", m_Target.GetObjectType ().ToString ());
			EditorGUILayout.LabelField ("FSM State Name:", m_Target.GetFSMStateName ());
			EditorGUILayout.LabelField ("FSM Path:", m_Target.GetFSMName ());
			GUILayout.Label ("***Data***");
			EditorGUILayout.LabelField ("Name:", m_Target.GetName());
			EditorGUILayout.LabelField ("HP:", m_Target.GetCurrentHealth() + " / " + m_Target.GetMaxHealth());
			EditorGUILayout.LabelField ("Attack Damage:", m_Target.GetAttackDamage().ToString ());
			EditorGUILayout.LabelField ("Physic Defend:", m_Target.GetPhysicDefend().ToString ());
			EditorGUILayout.LabelField ("Attack Speed:", m_Target.GetAttackSpeed().ToString ());
			EditorGUILayout.LabelField ("Move Position:", m_Target.GetMovePosition().ToString ());
			EditorGUILayout.LabelField ("Move Speed:", m_Target.GetMoveSpeed().ToString ());
			EditorGUILayout.LabelField ("Seek Radius:", m_Target.GetSeekRadius().ToString ());
			EditorGUILayout.LabelField ("Start Position:", m_Target.GetStartPosition().ToString ());
		}

	}
}
