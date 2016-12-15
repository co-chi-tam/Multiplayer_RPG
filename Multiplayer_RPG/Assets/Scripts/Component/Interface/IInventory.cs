using System;
using System.Collections;

namespace SurvivalTest {
	public interface IInventory {

		void AddInventoryItem(IItem value);
		void AddEquipmentItem(CEnum.EItemSlot slot, IItem value);

		IItem[] GetInventoryItems();
		void SetInventoryItem(int index, IItem item);

		IItem[] GetEquipmentItems();
		void SetEquipmentItem(int index, IItem item);

	}
}
