using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace SurvivalTest {
	public class CMsgRegisterPlayer : MessageBase {

		public CUserData userData;

		public CMsgRegisterPlayer () : base()
		{
			this.userData = new CUserData ();
		}

	}
}
