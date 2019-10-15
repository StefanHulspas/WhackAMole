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
	private float _scoreMultiplierTimeImpact = .95f;

	[SerializeField]
	private ScoreAdjustment AdjustmentOnNoHit = default;


	[Header("Game Variables In Scene")]
	[SerializeField]
	private Camera _mainCamera;

	[SerializeField]
	private IntData _currentScore = default;
	[SerializeField]
	private IntData _scoreMultiplier = default;
	[SerializeField]
	private IntData _scoreMultiplierSteps = default;

	[SerializeField]
	private FloatData _timeRemaining = default;

	[SerializeField]
	private GameObject _postGameCanvas = default;

	[SerializeField]
	private LayerMask _moleLayerMask = default;

	[SerializeField]
	private FieldController _fieldController = default;
	
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
		_scoreMultiplier.Value = 1;
		_scoreMultiplierSteps.Value = 2;
	}

	private void Update()
	{
		HandleInput();
		HandleGameLogic();
	}

	private void HandleInput()
	{
		HandleTouch();

		//So I can easily test on the computer
		//HandleMouseClick();
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
		if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000, _moleLayerMask))
		{
			MoleEntity moleController = raycastHit.collider.transform.GetComponentInParent<MoleEntity>();
			MoleHit(moleController);
		}
		else
		{
			ChangeScore(AdjustmentOnNoHit);
		}
	}

	private void MoleHit(MoleEntity moleEntity)
	{
		ChangeScore(moleEntity.Behaviour.AdjustmentOnClick);
		_fieldController.DisableMole(moleEntity);
	}

	private void ChangeScore(ScoreAdjustment adjustment) {
		int scoreChange = adjustment.AdjustScoreByAmount;
		if (adjustment.ResetScoreMultiplier) {
			ResetScoreMultiplier();
		} else {
			scoreChange *= _scoreMultiplier.Value;
			_scoreMultiplierSteps.Value--;
			if (_scoreMultiplierSteps.Value == 0) {
				_scoreMultiplier++;
				_scoreMultiplierSteps.Value = _scoreMultiplier * 2;
			}
		}
		_currentScore.Value += scoreChange;
	}

	private void ResetScoreMultiplier()
	{
		_scoreMultiplier.Value = 1;
		_scoreMultiplierSteps.Value = 2;
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
		MenuController.Instance.TransitionToMenu(_postGameCanvas);
	}

	private void SpawnNewMole()
	{
		MoleEntity newMole = _fieldController.SpawnNewMole();
		if (newMole != null)
		{
			newMole.SetActive(AdjustTimeByScoreMultiplier(_baseMoleActiveTime), MoleTimeout);
			_nextMoleSpawn += AdjustTimeByScoreMultiplier(_baseTimeBetweenMoles);
		}
	}

	private float AdjustTimeByScoreMultiplier(float timeToAdjust) {
		if (_scoreMultiplier.Value == 1) return timeToAdjust;
		return timeToAdjust * Mathf.Pow(_scoreMultiplierTimeImpact, _scoreMultiplier.Value - 1);
	}

	private void MoleTimeout(MoleEntity timedoutMole)
	{
		ChangeScore(timedoutMole.Behaviour.AdjustmentOnTimeout);
		_fieldController.DisableMole(timedoutMole);
	}
}