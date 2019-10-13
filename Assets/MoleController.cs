using System;
using UnityEngine;

public class MoleController : MonoBehaviour
{
	[SerializeField]
	private Transform _background;
	[SerializeField]
	private float moleSizePercent = .9f;

	private bool isActive = false;
	private float timeoutTime;
	private Action<MoleController> timeoutCallback;

	private void Update()
	{
		if (!isActive) return;
		if (Time.timeSinceLevelLoad >= timeoutTime) {
			PopDownMole();
			timeoutCallback(this);
		}
	}

	public void Setup(float size) {
		_background.localScale = new Vector3(size * moleSizePercent, .1f, size * moleSizePercent);
		_background.gameObject.SetActive(false);
	}

	public void PopUpMole(float baseMoleActiveTime, Action<MoleController> moleTimeout) {
		timeoutTime = Time.timeSinceLevelLoad + baseMoleActiveTime;
		timeoutCallback = moleTimeout;
		_background.gameObject.SetActive(true);
	}

	public void PopDownMole() {
		isActive = false;
		_background.gameObject.SetActive(false);
	}
}
