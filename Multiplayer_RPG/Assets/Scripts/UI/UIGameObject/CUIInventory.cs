using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CUIInventory : CBaseMonoBehaviour {

		[SerializeField]	private CUIItemInfo[] m_Items;

		public void LoadItems(IItem[] items, Action<object> onExecuteObject) {
			for (int i = 0; i < m_Items.Length; i++) {
				if (items [i] != null) {
					m_Items [i].SetItem (items [i], onExecuteObject);
				} else {
					m_Items [i].SetItem (null, null);
				}
			}
		}
	
	}
}
