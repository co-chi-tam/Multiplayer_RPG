using System;
using System.Collections;

namespace SurvivalTest {
	public interface IItem {

		string GetID();
		string GetName();
		string GetAvatar();
		int GetInventorySlot();
		void SetInventorySlot(int value);

		CEnum.EItemSlot GetItemSlot ();
		void SetItemSlot(CEnum.EItemSlot value);

		int GetCurrentAmount ();
		void SetCurrentAmount (int value);
		int GetMaxAmount();

		void SetRate(int value);
		int GetRate();

		void SetOwner (CObjectController value);
		CObjectController GetOwner ();

		object GetController();

	}
}
