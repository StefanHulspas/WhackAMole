﻿using UnityEngine;

public class MenuManager : MonoBehaviour
{
	public static MenuManager Instance { get; private set; }
	[SerializeField]
	private GameObject _currentActiveMenu;

	private void Start()
	{
		if (Instance != null) {
			Debug.LogError("Multiple MenuControllers in scene");
		}
		Instance = this;
	}

	public void TransitionToMenu(GameObject newMenu)
	{
		_currentActiveMenu.SetActive(false);
		_currentActiveMenu = newMenu;
		_currentActiveMenu.SetActive(true);
	}
}