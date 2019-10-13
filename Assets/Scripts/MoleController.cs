using System;
using UnityEngine;

public class MoleController : MonoBehaviour
{
    public MoleBehaviour Behaviour;

	[SerializeField]
	private Transform _background = default;
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

	public void Setup(float size, Action<MoleController> moleTimeout) {
		_background.localScale = new Vector3(size * moleSizePercent, .1f, size * moleSizePercent);
		_background.gameObject.SetActive(false);
        timeoutCallback = moleTimeout;
    }

	public void PopUpMole(float baseMoleActiveTime, MoleBehaviour newBehaviour) {
		timeoutTime = Time.timeSinceLevelLoad + baseMoleActiveTime;
		_background.gameObject.SetActive(true);
        Behaviour = newBehaviour;
	}

	public void PopDownMole() {
		isActive = false;
		_background.gameObject.SetActive(false);
	}
}
