using System;
using System.Collections;

public class CountdownTime
{

	private float m_CurrentTime;
	private float m_FinishTime;
	private bool m_Loop = true;

	public CountdownTime ()
	{
		this.m_CurrentTime = 0f;
		this.m_FinishTime = float.MaxValue;
		this.m_Loop = true;
	}

	public CountdownTime (bool loop)
	{
		this.m_CurrentTime = 0f;
		this.m_FinishTime = float.MaxValue;
		this.m_Loop = loop;
	}

	public CountdownTime (float finishTime, bool loop)
	{
		this.m_CurrentTime = 0f;
		this.m_FinishTime = finishTime;
		this.m_Loop = loop;
	}

	public bool UpdateTime(float dt) {
		var onTime = 0f;
		return UpdateTime (dt, out onTime);
	}

	public bool UpdateTime(float dt, out float onTime) {
		onTime = 0f;
		if (m_CurrentTime <= (m_FinishTime + dt)) {
			m_CurrentTime += dt;
			if (m_CurrentTime >= m_FinishTime) {
				onTime = m_CurrentTime;
				if (m_Loop) {
					m_CurrentTime = 0f;
				} else {
					return false;
				}
				return true;
			}
		}
		return false;
	}

	public void Reset() {
		this.m_CurrentTime = 0f;
	}

}

