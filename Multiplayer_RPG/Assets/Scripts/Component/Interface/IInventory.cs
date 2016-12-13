using System;
using System.Collections;

namespace SurvivalTest {
	public interface IInventory {

		void AddInventoryItem(IItem value);
		void AddEquipmentItem(CEnum.EItemSlot slot, IItem value);

	}
}
