using UnityEngine;
using System;
using System.Collections;

namespace SurvivalTest {
	public class CInventoryComponent : CComponent {

		public Action<IItem> OnEventUpdateItem;

		// Owner
		protected IInventory m_Inventory;

		// Item
		protected IItem[] m_ItemEquipSlots;
		protected IItem[] m_ItemInventorySlots;

		public CInventoryComponent (IInventory inventory, int slot) : base ()
		{
			this.m_Inventory = inventory;

			// Item equipment
			this.m_ItemEquipSlots = new IItem[4];
			this.m_ItemEquipSlots [(int)CEnum.EItemSlot.Hand] 		= null; 
			this.m_ItemEquipSlots [(int)CEnum.EItemSlot.Back] 		= null; 
			this.m_ItemEquipSlots [(int)CEnum.EItemSlot.Hat]		= null; 
			this.m_ItemEquipSlots [(int)CEnum.EItemSlot.Necklet]	= null; 
			// Inventory
			this.m_ItemInventorySlots = new IItem[slot];
		}

		public CInventoryComponent (IInventory inventory, IItem[] equipmentItems, IItem[] inventoryItems) : base ()
		{
			this.m_Inventory = inventory;
			// Item equipment
			this.m_ItemEquipSlots = equipmentItems; 
			// Inventory
			this.m_ItemInventorySlots = inventoryItems;
		}

		public bool AddInventoryItem(IItem value, Action<IItem> onAddItem, Action<IItem> onUpdateItem) {
			var currentItem = FindItemSlot ((x) => {
				return x != null 
					&& x.GetName () == value.GetName () 
					&& x.GetCurrentAmount() + value.GetCurrentAmount() <= x.GetMaxAmount();
			});
			if (currentItem != null) {
				currentItem.SetInventorySlot (Array.IndexOf (this.m_ItemInventorySlots, currentItem));
				currentItem.SetCurrentAmount (currentItem.GetCurrentAmount () + value.GetCurrentAmount ());
				if (OnEventUpdateItem != null) {
					OnEventUpdateItem (currentItem);
				}
				if (onUpdateItem != null) {
					onUpdateItem (currentItem);
				}
				return true;
			} else {
				for (int i = 0; i < this.m_ItemInventorySlots.Length; i++) {
					if (this.m_ItemInventorySlots [i] == null) {
						this.SetInventoryItem (i, value);
						this.m_ItemInventorySlots [i].SetCurrentAmount (value.GetCurrentAmount());
						if (OnEventUpdateItem != null) {
							OnEventUpdateItem (this.m_ItemInventorySlots [i]);
						}
						if (onAddItem != null) {
							onAddItem (this.m_ItemInventorySlots [i]);
						}
						return true;
					}
				}
			}
			return false;
		}

		public void SwapInventoryItem (IItem origin, IItem end) {
			var originIndex = Array.IndexOf (this.m_ItemInventorySlots, origin);
			var endIndex = Array.IndexOf (this.m_ItemInventorySlots, end);
			if (originIndex != -1 && endIndex != -1) {
				this.m_ItemInventorySlots [originIndex] = end;
				this.m_ItemInventorySlots [endIndex] = origin;	
			}
		}

		public IItem FindItemSlot(Func<IItem, bool> onCondition) {
			for (int i = 0; i < this.m_ItemInventorySlots.Length; i++) {
				if (onCondition != null) {
					if (onCondition (this.m_ItemInventorySlots [i])) {
						return this.m_ItemInventorySlots [i];
					}
				}
			}
			return null;
		}

		public void SetInventoryItem(int slot, IItem value) {
			this.m_ItemInventorySlots [slot] = value;
			this.m_ItemInventorySlots [slot].SetInventorySlot (slot);
		}

		public IItem[] GetInventoryItems() {
			return this.m_ItemInventorySlots;
		}

		public void SetEquipmentItem(CEnum.EItemSlot slot, IItem value) {
			this.m_ItemEquipSlots [(int)slot] = value;
		}

		public IItem[] GetEquipmentItems() {
			return this.m_ItemEquipSlots;
		}
	
	}
}
