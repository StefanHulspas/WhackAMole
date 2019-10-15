using System;
using UnityEngine;

public class MoleController : MonoBehaviour
{
	public MoleBehaviour Behaviour { get; private set; }

	[SerializeField]
	private Transform _background = default;
	[SerializeField]
	private float _moleSizePercent = .9f;

	private bool _isActive = false;
	private float _activeTime;
	private Action<MoleController> _timeoutCallback;

	private void Update()
	{
		if (!_isActive) return;
		if (_activeTime <= 0) {
			SetInactive();
			_timeoutCallback(this);
		}
	}

	public void Setup(float size) 
	{
		_background.localScale = new Vector3(size * _moleSizePercent, 1, size * _moleSizePercent);
		_background.gameObject.SetActive(false);
	}

	public void SetBehaviour(MoleBehaviour behaviour) 
	{
		Behaviour = behaviour;
	}

	public void SetActive(float activeTime, Action<MoleController> timeoutCallback) 
	{
		_activeTime = activeTime;
		_background.gameObject.SetActive(true);
		_timeoutCallback = timeoutCallback;
	}

	public void SetInactive() 
	{
		_isActive = false;
		_background.gameObject.SetActive(false);
	}
}