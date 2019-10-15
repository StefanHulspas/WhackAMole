using System;
using UnityEngine;

public class MoleEntity : MonoBehaviour
{
	public MoleBehaviour Behaviour { get; private set; }

	[SerializeField]
	private Transform _background = default;
	[SerializeField]
	private float _moleSizePercent = .9f;

	private bool _isActive = false;
	private float _activeTime;
	private Action<MoleEntity> _timeoutCallback;
	private GameObject _visual;

	private void Update()
	{
		if (!_isActive) return;
		_activeTime -= Time.deltaTime;
		if (_activeTime <= 0) {
			SetInactive();
			_timeoutCallback(this);
		}
	}

	public void Setup(float size) 
	{
		transform.localScale = new Vector3(size * _moleSizePercent, size * _moleSizePercent, size * _moleSizePercent);
		ActivateVisualElements(false);
	}

	public void SetBehaviour(MoleBehaviour behaviour) 
	{
		Behaviour = behaviour;
		if (_visual != null) Destroy(_visual);
		_visual = Instantiate(behaviour.Visual, transform);
	}

	public void SetActive(float activeTime, Action<MoleEntity> timeoutCallback) 
	{
		_activeTime = activeTime * Behaviour.ActiveTimeModifier;
		_timeoutCallback = timeoutCallback;
		ActivateVisualElements(true);
	}

	public void SetInactive()
	{
		ActivateVisualElements(false);
	}

	private void ActivateVisualElements(bool setActive) {
		_background.gameObject.SetActive(setActive);
		_visual?.SetActive(setActive);
		_isActive = setActive;
	}
}