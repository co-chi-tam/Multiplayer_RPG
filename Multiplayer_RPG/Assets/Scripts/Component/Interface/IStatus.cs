using UnityEngine;
using System;
using System.Collections;

namespace SurvivalTest {
	public interface IStatus : IObjectInfo, ICommunicate {

		#region Data

		void SetData (CObjectData value);
		CObjectData GetData();

		#endregion

		#region Animation

		void SetAnimation (CEnum.EAnimation value);
		CEnum.EAnimation GetAnimation();

		void SetAnimationTime (float value);
		float GetAnimationTime();

		#endregion

		void SetDidAttack (bool value);
		bool GetDidAttack();

		bool GetActive();
		void SetActive(bool value);

		bool GetEnable();
		void SetEnable(bool value);

		void OnBecameVisible();
		void OnBecameInvisible();
		bool GetIsVisible ();

		bool GetUnderControl();
		void SetUnderControl(bool value);

		bool GetLocalUpdate();
		void SetLocalUpdate(bool value);

		bool GetDataUpdate();
		void SetDataUpdate(bool value);

		bool GetOtherInteractive();
		void SetOtherInteractive(bool value);

		string GetFSMStateName();
		void SetFSMStateName(string value);
		string GetFSMName();

		IItem[] GetInventoryItems();
		void SetInventoryItem(int index, IItem item);

		IItem[] GetEquipmentItems();
		void SetEquipmentItem(int index, IItem item);

		#region Transform

		Vector3 GetPosition ();
		void SetPosition(Vector3 position);

		Vector3 GetRotation ();
		void SetRotation(Vector3 rotation);

		Vector3 GetMovePosition ();
		void SetMovePosition(Vector3 position);

		Vector3 GetStartPosition ();
		void SetStartPosition(Vector3 position);

		#endregion

		CEnum.EAnimation GetCurrentSkill();
		void SetCurrentSkill(CEnum.EAnimation value);

		void UpdateFSM (float dt);
		void UpdateTouchInput (float dt);
		void UpdateSkillInput (CEnum.EAnimation skill);
		void UpdateSelectionObject (Vector3 originPoint, Vector3 directionPoint);
		void ExecuteInventoryItem (object value);

		void OnDestroyObject ();

		string GetToken ();
		void SetToken(string value);

		CObjectController GetTargetInteract();
		void SetTargetInteract(CObjectController value);

		CObjectController GetOwner();
		void SetOwner(CObjectController value);

		void AddEventListener (string name, Action<object> onEvent);
		void RemoveEventListener (string name, Action<object> onEvent);
	}
}
