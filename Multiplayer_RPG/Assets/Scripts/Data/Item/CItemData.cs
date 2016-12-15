using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	[Serializable]
	public class CItemData : CObjectData {

		public CEnum.EItemSlot itemSlot;
		public int currentAmount;
		public int maxAmount;
		public int rate;

		public CItemData () : base () {
			this.itemSlot = CEnum.EItemSlot.Inventory;

			this.currentHealth = 1;
			this.maxHealth = 1;

			this.currentAmount = 0;
			this.maxAmount = 0;

			this.rate = 0;
		}

	}
}
