using System;
using System.Collections;

namespace SurvivalTest {
	public interface IItem {

		string GetName();
		string GetAvatar();

		CEnum.EItemSlot GetItemSlot ();
		void SetItemSlot(CEnum.EItemSlot value);

		int GetCurrentAmount ();
		void SetCurrentAmount (int value);
		int GetMaxAmount();

		void SetOwner (CObjectController value);
		CObjectController GetOwner ();

		object GetController();
	}
}
