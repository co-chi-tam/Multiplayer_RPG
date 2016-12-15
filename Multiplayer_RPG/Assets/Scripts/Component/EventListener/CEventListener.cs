using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CEventListener {

		#region Properties

		public Action<object> OnListener;

		#endregion

		#region Contructor

		public CEventListener ()
		{
			this.OnListener = null;
		}

		#endregion

		#region Main methods

		public void AddListener(Action<object> onEvent) {
			this.OnListener -= onEvent;
			this.OnListener += onEvent;
		}

		public void RemoveListener(Action<object> onEvent) {
			this.OnListener -= onEvent;
		}

		public void RemoveAllListener() {
			this.OnListener = null;
		}

		public void Invoke(object value) {
			if (this.OnListener != null) {
				this.OnListener (value);
			}
		}

		#endregion

	}
}
