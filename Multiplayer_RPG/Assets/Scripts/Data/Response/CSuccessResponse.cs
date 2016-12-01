using System;
using System.Collections;

namespace SurvivalTest {
	public class CSuccessResponse<T> {

		public int resultCode;
		public T[] resultContent;

		public CSuccessResponse ()
		{
			this.resultCode = 0;
			this.resultContent = null;
		}

	}
}
