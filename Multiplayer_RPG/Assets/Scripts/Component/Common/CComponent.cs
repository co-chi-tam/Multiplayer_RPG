using System;
using System.Collections;

	namespace SurvivalTest {
	public class CComponent {

		protected float m_TotalTime;
		protected float m_DeltaTime;

		public CComponent ()
		{
			
		}

		public virtual void UpdateComponent(float dt) {
			m_TotalTime += dt;
			m_DeltaTime = dt;
		}

		public virtual void Clear() {
			m_TotalTime = 0f;
		}

	}
}