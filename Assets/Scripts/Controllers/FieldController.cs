using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour
{
	[Header("Field Settings")]
	[SerializeField]
	private Vector2 _gridSize = new Vector2(2, 2);
	[SerializeField]
	private Vector2 _fieldAnchorMin = new Vector2(.05f, .05f);
	[SerializeField]
	private Vector2 _fieldAnchorMax = new Vector2(.95f, .8f);
	[SerializeField]
	private MoleEntity _molePrefab = default;
	[SerializeField]
	private List<MoleBehaviour> _possibleMoleBehaviours = new List<MoleBehaviour>();

	[SerializeField]
	private Camera _mainCamera;

	private Vector3 _screenBotLeft;
	private Vector3 _screenTopRight;

	private List<MoleEntity> _inactiveMoles = new List<MoleEntity>();
	private List<MoleEntity> _activeMoles = new List<MoleEntity>();
	
	private float _totalBehaviourChance = 0;

	void Start()
	{
		if (_mainCamera == null) _mainCamera = Camera.main;
		
		_screenBotLeft = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width * _fieldAnchorMin.x, Screen.height * _fieldAnchorMin.y, _mainCamera.transform.position.y));
		_screenTopRight = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width * _fieldAnchorMax.x, Screen.height * _fieldAnchorMax.y, _mainCamera.transform.position.y));
		
		if (_gridSize.x <= 0) _gridSize.x = 1;
		if (_gridSize.y <= 0) _gridSize.y = 1;

		CalculateTotalBehaviourChance();
		PreCreateMoles();
	}

	private void CalculateTotalBehaviourChance() 
	{
		_totalBehaviourChance = 0;
		for (int i = 0; i < _possibleMoleBehaviours.Count; i++)
		{
			_totalBehaviourChance += _possibleMoleBehaviours[i].ChanceForBehaviour;
		}
	}
	
	private void PreCreateMoles()
	{
		Vector3 botMid = new Vector3((_screenBotLeft.x + _screenTopRight.x) / 2, _screenBotLeft.y, _screenBotLeft.z);
		float screenWorldWidth = _screenTopRight.x - _screenBotLeft.x;
		float screenWorldHeight = _screenTopRight.z - _screenBotLeft.z;
		float possibleMoleWidth = screenWorldWidth / _gridSize.x;
		float possibleMoleHeight = screenWorldHeight / _gridSize.y;
		float moleSize = possibleMoleWidth < possibleMoleHeight ? possibleMoleWidth : possibleMoleHeight;

		for (int x = 0; x < _gridSize.x; x++)
		{
			for (int y = 0; y < _gridSize.y; y++)
			{
				Vector3 position = new Vector3(botMid.x - moleSize * _gridSize.x / 2 + moleSize / 2 + x * moleSize, _screenBotLeft.y, _screenBotLeft.z + moleSize / 2 + y * moleSize);
				MoleEntity newMole = Instantiate(_molePrefab, position, Quaternion.identity, transform);
				newMole.Setup(moleSize);
				_inactiveMoles.Add(newMole);
			}
		}
	}

	public MoleEntity SpawnNewMole()
	{
		if (_inactiveMoles.Count == 0) return null;
		MoleEntity MoleToSpawn = _inactiveMoles[Random.Range(0, _inactiveMoles.Count - 1)];
		_inactiveMoles.Remove(MoleToSpawn);
		_activeMoles.Add(MoleToSpawn);
		MoleToSpawn.SetBehaviour(GetRandomMoleBehaviour());
		return MoleToSpawn;
	}
	
	public void DisableAllMoles()
	{
		foreach (MoleEntity mole in _activeMoles) 
		{ 
			DisableMole(mole);
		}
	}

	public void DisableMole(MoleEntity moleToDisable)
	{
		_activeMoles.Remove(moleToDisable);
		_inactiveMoles.Add(moleToDisable);
		moleToDisable.SetInactive();
	}

	private MoleBehaviour GetRandomMoleBehaviour()
	{
		float chance = Random.Range(0, _totalBehaviourChance);
		for (int i = 0; i < _possibleMoleBehaviours.Count; i++)
		{
			chance -= _possibleMoleBehaviours[i].ChanceForBehaviour;
			if (chance <= 0) return _possibleMoleBehaviours[i];
		}
		return _possibleMoleBehaviours[_possibleMoleBehaviours.Count - 1];
	}
}
