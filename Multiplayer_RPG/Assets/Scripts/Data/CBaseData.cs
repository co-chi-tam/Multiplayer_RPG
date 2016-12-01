using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DataReflection;

namespace SurvivalTest {
	[Serializable]
	public class CBaseData {

		public string id;

		public CBaseData ()
		{
			this.id = string.Empty;
		}

	}
}
