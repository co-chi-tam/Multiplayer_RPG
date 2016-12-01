using System;
using System.Collections;

public class ObjectProperty<T> {

	private T m_Value;
	public T Value {
		get { return m_Value; }
		set { m_Value = value; }
	}

	private bool m_Enable;
	public bool Enable {
		get { return m_Enable; }
		set { m_Enable = value; }
	}

	public ObjectProperty ()
	{
		m_Value = default (T);
		m_Enable = true;
	}

	public ObjectProperty (T value)
	{
		m_Value = value;
		m_Enable = true;
	}

	public ObjectProperty (T value, bool enable)
	{
		m_Value = value;
		m_Enable = enable;
	}

	public static explicit operator ObjectProperty<T> (T value)
	{
		var tmp = new ObjectProperty<T>(value);
		return tmp;
	}

	public static implicit operator T(ObjectProperty<T> value) 
	{
		return value.Value;
	}

	public override string ToString ()
	{
		return m_Value.ToString ();
	}

}
