using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableDataType
{
	public class VarData<T> : ScriptableObject
	{
		private List<Action<T>> _listeners;

		public void AddListener(Action<T> action)
		{
			if (_listeners == null) _listeners = new List<Action<T>>();
			_listeners.Add(action);
			action(_value);
		}

		public void RemoveListener(Action<T> action)
		{
			_listeners.Remove(action);
		}

		public void Raise()
		{
			if (_listeners == null) return;
			for (int i = 0; i < _listeners.Count; i++)
			{
				_listeners[i].Invoke(_value);
			}
		}

		[SerializeField]
		private T _value;

		public VarData(T val)
		{
			_value = val;
		}

		public T Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if(this._value != null && this._value.Equals(value))return;

				this._value = value;
				Raise();
			}
		}
	}
}