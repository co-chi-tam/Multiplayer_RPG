using System;
using System.Collections;

namespace SurvivalTest {
	public class CErrorResponse {

		public int errorCode;
		public string errorContent;

		public CErrorResponse ()
		{
			this.errorCode = 0;
			this.errorContent = string.Empty;
		}

		public CErrorResponse (int code, string content)
		{
			this.errorCode = code;
			this.errorContent = content;
		}

	}
}
