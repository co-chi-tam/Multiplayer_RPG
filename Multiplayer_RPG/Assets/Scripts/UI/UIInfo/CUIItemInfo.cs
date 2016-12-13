using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CUIItemInfo : CBaseMonoBehaviour {

		[SerializeField]	private Text m_UIItemAmountText;
		[SerializeField]	private Image m_UIItemImage;
	
		public void SetAmountText(string value) {
			m_UIItemAmountText.text = value;
		}

		public void SetImage(string value) {
			m_UIItemImage.sprite = CUtil.FindSprite (value);
		}

	}
}
