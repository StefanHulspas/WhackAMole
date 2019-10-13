using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	[Header("Field Settings")]
	[SerializeField]
	private Vector2 _gridSize = new Vector2(2, 2);
	[SerializeField]
	private float moleBorderPercent = .1f;

	[Header("Game Settings")]
	[SerializeField]
	private float playtimeInSeconds = 60f;
	[SerializeField]
	private GameObject _molePrefab;


	[Header("Game Variables In Scene")]
	[SerializeField]
	private Camera _mainCamera;

	private Vector3 _screenBotLeft;
	private Vector3 _screenTopRight;

	private List<GameObject> _inactiveMoles = new List<GameObject>();

	// Start is called before the first frame update
	void Start()
	{
		if (_mainCamera == null) _mainCamera = Camera.main;

		_screenBotLeft = _mainCamera.ScreenToWorldPoint(new Vector3(0, 0, _mainCamera.transform.position.y));
		_screenTopRight = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _mainCamera.transform.position.y));

		PreCreateMoles();
	}

	private void PreCreateMoles()
	{
		if (_gridSize.x <= 0) _gridSize.x = 1;
		if (_gridSize.y <= 0) _gridSize.y = 1;

		Vector3 botMid = new Vector3((_screenBotLeft.x + _screenTopRight.x) / 2, _screenBotLeft.y, _screenBotLeft.z);
		float screenWorldWidth = _screenTopRight.x - _screenBotLeft.x;
		float screenWorldHeight = _screenTopRight.z - _screenBotLeft.z;
		float possibleMoleWidth = screenWorldWidth / _gridSize.x;
		float possibleMoleHeight = screenWorldHeight / _gridSize.y;
		float moleSize = possibleMoleWidth < possibleMoleHeight ? possibleMoleWidth : possibleMoleHeight;
		
		for (int x = 0; x < _gridSize.x; x++) {
			for (int y = 0; y < _gridSize.y; y++) {
				Vector3 position = new Vector3(botMid.x - moleSize * _gridSize.x / 2 + moleSize / 2 + x * moleSize, _screenBotLeft.y, _screenBotLeft.z + moleSize / 2 + y * moleSize);
				GameObject newMole = Instantiate(_molePrefab, position, Quaternion.identity, transform);
				newMole.transform.localScale = new Vector3(moleSize, .1f, moleSize) * (1f - moleBorderPercent);
				_inactiveMoles.Add(newMole);
			}
		}

	}


}
