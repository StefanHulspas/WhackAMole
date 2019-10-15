using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour
{
	[Header("Field Settings")]
	[SerializeField]
	private Vector2 _gridSize = new Vector2(2, 2);
	
	[SerializeField]
	private RectTransform _spawnField = default;
	[SerializeField]
	private MoleEntity _molePrefab = default;
	[SerializeField]
	private List<MoleBehaviour> _possibleMoleBehaviours = new List<MoleBehaviour>();

	[SerializeField]
	private Camera _mainCamera;

	private Vector3 _spawnLimitBotLeft;
	private Vector3 _spawnLimitTopRight;

	private List<MoleEntity> _inactiveMoles = new List<MoleEntity>();
	private List<MoleEntity> _activeMoles = new List<MoleEntity>();
	
	private float _totalBehaviourChance = 0;

	void Start()
	{
		if (_mainCamera == null) _mainCamera = Camera.main;
		
		_spawnLimitBotLeft = _mainCamera.ScreenToWorldPoint(
			new Vector3(Screen.width * _spawnField.anchorMin.x, 
						Screen.height * _spawnField.anchorMin.y, 
						_mainCamera.transform.position.y));
		_spawnLimitTopRight = _mainCamera.ScreenToWorldPoint(
			new Vector3(Screen.width * _spawnField.anchorMax.x, 
						Screen.height * _spawnField.anchorMax.y, 
						_mainCamera.transform.position.y));
		
		//To make sure atleast 1 mole is created
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
		/* To ensure all moles are on the screen are visible I create them from the bottom up depending on the grid
		 * So I first calculate the bottom middle position as a starting point
		 */
		Vector3 botMid = new Vector3((_spawnLimitBotLeft.x + _spawnLimitTopRight.x) / 2, _spawnLimitBotLeft.y, _spawnLimitBotLeft.z);

		/* Then I calculate the maximum size of the squares to fill the field
		 */
		float screenWorldWidth = _spawnLimitTopRight.x - _spawnLimitBotLeft.x;
		float screenWorldHeight = _spawnLimitTopRight.z - _spawnLimitBotLeft.z;
		float possibleMoleWidth = screenWorldWidth / _gridSize.x;
		float possibleMoleHeight = screenWorldHeight / _gridSize.y;
		float moleSize = possibleMoleWidth < possibleMoleHeight ? possibleMoleWidth : possibleMoleHeight;

		/* Using these values I can precreate objects so I dont have to recalculate this at runtime
		 */
		for (int x = 0; x < _gridSize.x; x++)
		{
			for (int y = 0; y < _gridSize.y; y++)
			{
				Vector3 position = new Vector3(botMid.x - moleSize * _gridSize.x / 2 + moleSize / 2 + x * moleSize, _spawnLimitBotLeft.y, _spawnLimitBotLeft.z + moleSize / 2 + y * moleSize);
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
		ActivateMole(MoleToSpawn);
		return MoleToSpawn;
	}
	
	public void DisableAllMoles()
	{
		for(int i = _activeMoles.Count - 1; i >= 0; i--)
		{ 
			DisableMole(_activeMoles[i]);
		}
	}

	public void DisableMole(MoleEntity moleToDisable)
	{
		_activeMoles.Remove(moleToDisable);
		_inactiveMoles.Add(moleToDisable);
		moleToDisable.SetInactive();
	}
	
	public void ActivateMole(MoleEntity moleToActivate)
	{
		_inactiveMoles.Remove(moleToActivate);
		_activeMoles.Add(moleToActivate);
		moleToActivate.SetBehaviour(GetRandomMoleBehaviour());
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