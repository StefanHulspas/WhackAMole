using ScriptableDataType;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
	[Header("Field Settings")]
	[SerializeField]
	private Vector2 _gridSize = new Vector2(2, 2);
	[SerializeField]
	private Vector2 _fieldAnchorMin = new Vector2(.05f, .05f);
	[SerializeField]
	private Vector2 _fieldAnchorMax= new Vector2(.95f, .8f);

	[Header("Game Settings")]
	[SerializeField]
	private float _playtimeInSeconds = 60f;
	[SerializeField]
	private float _baseTimeBetweenMoles = 2f;
	[SerializeField]
	private float _baseMoleActiveTime = 2f;
	[SerializeField]
	private MoleController _molePrefab = default;
    [SerializeField]
    private List<MoleBehaviour> _possibleMoleBehaviours = new List<MoleBehaviour>();
    [SerializeField]
	private LayerMask _moleLayerMask = default;


	[Header("Game Variables In Scene")]
	[SerializeField]
	private Camera _mainCamera;
	[SerializeField]
	private IntData _currentScore = default;
	[SerializeField]
	private FloatData _timeRemaining = default;
	[SerializeField]
	private GameObject _endGameCanvas = default;

	private Vector3 _screenBotLeft;
	private Vector3 _screenTopRight;

	private List<MoleController> _inactiveMoles = new List<MoleController>();
	private List<MoleController> _activeMoles = new List<MoleController>();

	private float _gameTime;
	private float _nextMoleSpawn;
    private float _totalBehaviourChance = 0;

	private Transform _moleCollection;
	private bool isSetup = false;

	// Start is called before the first frame update
	void Start()
	{
		if (_mainCamera == null) _mainCamera = Camera.main;

		_screenBotLeft = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width * _fieldAnchorMin.x, Screen.height * _fieldAnchorMin.y, _mainCamera.transform.position.y));
		_screenTopRight = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width * _fieldAnchorMax.x, Screen.height * _fieldAnchorMax.y, _mainCamera.transform.position.y));

        for (int i = 0; i < _possibleMoleBehaviours.Count; i++)
        {
            _totalBehaviourChance += _possibleMoleBehaviours[i]._ChanceForBehaviour;
        }

        isSetup = true;
	}

	private void OnEnable()
	{
		if (!isSetup)
			StartCoroutine(DelayedStart());
		else
			StartGame();
	}

	private IEnumerator DelayedStart() {
		yield return new WaitUntil(() => isSetup);
		StartGame();
	}

	private void StartGame() {
		ResetGame();
		PreCreateMoles();
	}

	private void ResetGame() {
		if (_moleCollection != null)
		{
			_inactiveMoles.Clear();
			_activeMoles.Clear();
			Destroy(_moleCollection.gameObject);
		}
		_moleCollection = new GameObject("Mole Collection").transform;
		_moleCollection.parent = transform;

		_gameTime = 0f;
		_nextMoleSpawn = _baseTimeBetweenMoles;
        _currentScore.Value = 0;

		if (_gridSize.x <= 0) _gridSize.x = 1;
		if (_gridSize.y <= 0) _gridSize.y = 1;
	}

	private void PreCreateMoles() {
		Vector3 botMid = new Vector3((_screenBotLeft.x + _screenTopRight.x) / 2, _screenBotLeft.y, _screenBotLeft.z);
		float screenWorldWidth = _screenTopRight.x - _screenBotLeft.x;
		float screenWorldHeight = _screenTopRight.z - _screenBotLeft.z;
		float possibleMoleWidth = screenWorldWidth / _gridSize.x;
		float possibleMoleHeight = screenWorldHeight / _gridSize.y;
		float moleSize = possibleMoleWidth < possibleMoleHeight ? possibleMoleWidth : possibleMoleHeight;
		
		for (int x = 0; x < _gridSize.x; x++) {
			for (int y = 0; y < _gridSize.y; y++) {
				Vector3 position = new Vector3(botMid.x - moleSize * _gridSize.x / 2 + moleSize / 2 + x * moleSize, _screenBotLeft.y, _screenBotLeft.z + moleSize / 2 + y * moleSize);
				MoleController newMole = Instantiate(_molePrefab, position, Quaternion.identity, _moleCollection);
				newMole.Setup(moleSize, MoleTimeout);
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
				MoleHit(moleController);
			} else
            {
                MissedMole();
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
					MoleHit(moleController);
				} else
                {
                    MissedMole();
                }
			}
		}
	}

    private void MissedMole()
    {
        _currentScore.Value -= 5;
    }

    private void MoleHit(MoleController moleController)
	{
        _currentScore.Value += moleController.Behaviour._scoreAdjustmentOnClick;
		moleController.PopDownMole();
		_inactiveMoles.Add(moleController);
	}

	private void HandleGameLogic()
	{
		_gameTime += Time.deltaTime;
		_timeRemaining.Value = _playtimeInSeconds - _gameTime;
		if (_gameTime >= _playtimeInSeconds) EndGame();
		else if (_gameTime >= _nextMoleSpawn) {
			SpawnNewMole();
			_nextMoleSpawn += _baseTimeBetweenMoles;
		}
	}

	private void EndGame() {
		gameObject.SetActive(false);
		_endGameCanvas.SetActive(true);
	}

	private void SpawnNewMole()
	{
		if (_inactiveMoles.Count == 0) return;
		MoleController MoleToSpawn = _inactiveMoles[Random.Range(0, _inactiveMoles.Count - 1)];
		MoleToSpawn.PopUpMole(_baseMoleActiveTime, GetRandomMoleBehaviour());
		_inactiveMoles.Remove(MoleToSpawn);
		_activeMoles.Add(MoleToSpawn);
	}

    private MoleBehaviour GetRandomMoleBehaviour()
    {
        float chance = Random.Range(0, _totalBehaviourChance);
        for (int i = 0; i < _possibleMoleBehaviours.Count; i++)
        {
            chance -= _possibleMoleBehaviours[i]._ChanceForBehaviour;
            if (chance <= 0) return _possibleMoleBehaviours[i];
        }
        return _possibleMoleBehaviours[_possibleMoleBehaviours.Count - 1];
    }

    private void MoleTimeout(MoleController timedoutMole) {
		_activeMoles.Remove(timedoutMole);
		_inactiveMoles.Add(timedoutMole);
	}
}