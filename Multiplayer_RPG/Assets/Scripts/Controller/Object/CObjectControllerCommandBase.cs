using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public partial class CObjectController
	{

		protected virtual void OnRegisterCommand() {
			m_EventComponent.AddEventListener ("NONE", OnCommandNone);
		}	

		public virtual void InvokeCommand(string name, object value) {
			m_EventComponent.InvokeEventListener (name, value);
		}

		protected virtual void OnCommandNone(object value) {
			
		}
	
	}
}

