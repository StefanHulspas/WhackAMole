using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	[Header("Field Settings")]
	[SerializeField]
	private Vector2 _gridSize = new Vector2(2, 2);
	[SerializeField]
	private Vector2 _FieldAnchorMin = new Vector2(.05f, .05f);
	[SerializeField]
	private Vector2 _FieldAnchorMax= new Vector2(.95f, .8f);

	[Header("Game Settings")]
	[SerializeField]
	private float _playtimeInSeconds = 60f;
	[SerializeField]
	private float _baseTimeBetweenMoles = 2f;
	[SerializeField]
	private float _baseMoleActiveTime = 2f;
	[SerializeField]
	private MoleController _molePrefab;
	[SerializeField]
	private LayerMask _moleLayerMask;


	[Header("Game Variables In Scene")]
	[SerializeField]
	private Camera _mainCamera;

	private Vector3 _screenBotLeft;
	private Vector3 _screenTopRight;

	private List<MoleController> _inactiveMoles = new List<MoleController>();
	private List<MoleController> _activeMoles = new List<MoleController>();

	private float nextMoleSpawn;

	// Start is called before the first frame update
	void Start()
	{
		if (_mainCamera == null) _mainCamera = Camera.main;

		_screenBotLeft = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width * _FieldAnchorMin.x, Screen.height * _FieldAnchorMin.y, _mainCamera.transform.position.y));
		_screenTopRight = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width * _FieldAnchorMax.x, Screen.height * _FieldAnchorMax.y, _mainCamera.transform.position.y));

		PreCreateMoles();

		nextMoleSpawn = _playtimeInSeconds - _baseTimeBetweenMoles;
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
				MoleController newMole = Instantiate(_molePrefab, position, Quaternion.identity, transform);
				newMole.Setup(moleSize);
				_inactiveMoles.Add(newMole);
			}
		}
	}

	private void Update()
	{
		HandleInput();
		HandleGameLogic();
	}

	private void HandleInput()
	{
		HandleTouch();
		HandleMouseClick();
	}

	private void HandleMouseClick()
	{
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, 1000, _moleLayerMask))
			{
				MoleController moleController = raycastHit.collider.transform.GetComponentInParent<MoleController>();
				moleController.PopDownMole();
				_inactiveMoles.Add(moleController);
			}
		}
	}

	private void HandleTouch()
	{
		Touch[] touches = Input.touches;
		for (int i = 0; i < touches.Length; i++)
		{
			if (touches[i].phase == TouchPhase.Began)
			{
				Ray ray = _mainCamera.ScreenPointToRay(touches[i].position);
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, out raycastHit, 1000, _moleLayerMask))
				{
					MoleController moleController = raycastHit.collider.transform.GetComponentInParent<MoleController>();
					moleController.PopDownMole();
					_inactiveMoles.Add(moleController);
				}
			}
		}
	}

	private void HandleGameLogic()
	{
		_playtimeInSeconds -= Time.deltaTime;
		if (_playtimeInSeconds <= 0) return;
		else if (_playtimeInSeconds <= nextMoleSpawn) {
			SpawnNewMole();
			nextMoleSpawn = _playtimeInSeconds - _baseTimeBetweenMoles;
		}
	}

	private void SpawnNewMole()
	{
		if (_inactiveMoles.Count == 0) return;
		MoleController MoleToSpawn = _inactiveMoles[Random.Range(0, _inactiveMoles.Count - 1)];
		MoleToSpawn.PopUpMole(_baseMoleActiveTime, MoleTimeout);
		_inactiveMoles.Remove(MoleToSpawn);
		_activeMoles.Add(MoleToSpawn);
	}

	private void MoleTimeout(MoleController timedoutMole) {
		_activeMoles.Remove(timedoutMole);
		_inactiveMoles.Add(timedoutMole);
	}
}