using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CUserManager : CMonoSingleton<CUserManager> {

		#region Properties

		public CUserData CurrentUser;

		public string m_UserName;
		private string m_Password;

		private string m_URLLogin = "https://tamco-tinygame.rhcloud.com/login";
		private string m_URLLogout = "https://tamco-tinygame.rhcloud.com/logout";
		private CWWW m_WWW;

		#endregion

		#region Monobehaviour

		protected override void Awake ()
		{
			base.Awake ();
			DontDestroyOnLoad (this.gameObject);
			m_WWW = new CWWW ();
		}

		#endregion

		#region Main methods

		public void Login() {
			this.Login (m_UserName, m_Password);
		}

		public void UpdateUserName(InputField value) {
			m_UserName = value.text;
		}

		public void UpdatePassword(InputField value) {
			m_Password = value.text;
		}

		public virtual void OnClientLoginComplete(CUserData responce) {
			CurrentUser = responce;
			CLog.Debug ("OnClientLoginComplete " + CurrentUser.token);
			CNetworkManager.Instance.OnClientLoginComplete (CurrentUser);
		}

		public virtual void OnClientLoginFail(CErrorResponse responce) {
			CLog.Error ("OnClientLoginFail " + responce.errorContent);
		}

		public virtual void Login(string userName, string userPassword) {
			if (string.IsNullOrEmpty (userName) == true || string.IsNullOrEmpty (userPassword)) {
				OnClientLoginFail (new CErrorResponse (0, "Field not empty."));
				return;
			}
			var fields = new Dictionary<string, string> ();
			fields.Add ("uname", userName);
			fields.Add ("upass", userPassword);
			fields.Add ("macadd", SystemInfo.deviceUniqueIdentifier);
			m_WWW.Post (m_URLLogin, fields, null, (result) => {
				if (result.IndexOf ("errorCode") != -1) {
					var errorResponse = TinyJSON.JSON.Load(result).Make<CErrorResponse>();
					OnClientLoginFail (errorResponse);
				} else {
					var successResponse = TinyJSON.JSON.Load(result).Make<CSuccessResponse<CUserData>>();
					if (successResponse.resultContent[0].isLogin == false) {
						OnClientLoginComplete (successResponse.resultContent[0]);
					} else {
						OnClientLoginFail (new CErrorResponse (1, "This account is already login. Please, log out."));
					}
				}
			}, (error) => {
				if (error.IndexOf ("errorCode") != -1) {
					OnClientLoginFail (new CErrorResponse (0, error));
				} else {
					var errorResponse = TinyJSON.JSON.Load(error).Make<CErrorResponse>();
					OnClientLoginFail (errorResponse);
				}
			});
		}

		public virtual void LogOut(string userName, string userPassword) {
			if (string.IsNullOrEmpty (userName) == true || string.IsNullOrEmpty (userPassword)) {
				return;
			}
			var fields = new Dictionary<string, string> ();
			fields.Add ("uname", userName);
			fields.Add ("upass", userPassword);
			m_WWW.Post (m_URLLogout, fields, null, (result) => {
				
			}, (error) => {
				
			});
		}

		#endregion
	
	}
}
