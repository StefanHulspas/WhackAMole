using ScriptableDataType;
using UnityEngine;

public class GameController : MonoBehaviour
{
	[Header("Game Settings")]
	[SerializeField]
	private float _playtimeInSeconds = 60f;
	[SerializeField]
	private float _baseTimeBetweenMoles = 2f;
	[SerializeField]
	private float _baseMoleActiveTime = 2f;
	[SerializeField]
	private LayerMask _moleLayerMask = default;
	[SerializeField]
	private FieldController _fieldController = default;


	[Header("Game Variables In Scene")]
	[SerializeField]
	private Camera _mainCamera;
	[SerializeField]
	private IntData _currentScore = default;
	[SerializeField]
	private FloatData _timeRemaining = default;
	[SerializeField]
	private GameObject _endGameCanvas = default;
	
	private float _gameTime;
	private float _nextMoleSpawn;
	
	private void OnEnable()
	{
		if (_mainCamera == null) _mainCamera = Camera.main;
		ResetGame();
	}
	
	private void ResetGame() 
	{
		_gameTime = 0f;
		_nextMoleSpawn = _baseTimeBetweenMoles;
		_currentScore.Value = 0;
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
			CheckPositionForMole(Input.mousePosition);
		}
	}

	private void HandleTouch()
	{
		Touch[] touches = Input.touches;
		for (int i = 0; i < touches.Length; i++)
		{
			if (touches[i].phase == TouchPhase.Began)
			{
				CheckPositionForMole(touches[i].position);
			}
		}
	}

	private void CheckPositionForMole(Vector3 position) 
	{
		Ray ray = _mainCamera.ScreenPointToRay(position);
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit, 1000, _moleLayerMask))
		{
			MoleEntity moleController = raycastHit.collider.transform.GetComponentInParent<MoleEntity>();
			MoleHit(moleController);
		}
		else
		{
			MissedMole();
		}
	}

	private void MissedMole()
	{
		_currentScore.Value -= 5;
	}

	private void MoleHit(MoleEntity moleEntity)
	{
		ChangeScore(moleEntity.Behaviour.AdjustmentOnClick);
		_fieldController.DisableMole(moleEntity);
	}

	private void ChangeScore(ScoreAdjustment adjustment) {
		_currentScore.Value += adjustment.AdjustScoreByAmount;
	}

	private void HandleGameLogic()
	{
		_gameTime += Time.deltaTime;
		_timeRemaining.Value = _playtimeInSeconds - _gameTime;
		if (_gameTime >= _playtimeInSeconds) EndGame();
		else if (_gameTime >= _nextMoleSpawn) {
			SpawnNewMole();
		}
	}

	private void EndGame() 
	{
		_fieldController.DisableAllMoles();
		MenuController.Instance.TransitionToMenu(_endGameCanvas);
	}

	private void SpawnNewMole()
	{
		MoleEntity newMole = _fieldController.SpawnNewMole();
		if (newMole != null)
		{
			newMole.SetActive(_baseMoleActiveTime, MoleTimeout);
			_nextMoleSpawn += _baseTimeBetweenMoles;
		}
	}

	private void MoleTimeout(MoleEntity timedoutMole)
	{
		ChangeScore(timedoutMole.Behaviour.AdjustmentOnTimeout);
		_fieldController.DisableMole(timedoutMole);
	}
}