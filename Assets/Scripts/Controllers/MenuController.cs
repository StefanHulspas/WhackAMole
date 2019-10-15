using UnityEngine;

public class MenuController : MonoBehaviour
{
	public static MenuController Instance { get; private set; }

	[SerializeField]
	private GameObject _currentActiveMenu;

	/* A very simple class to help with menu switching
	 * So I don't duplicate code and for future use when transitions need to be flashier
	 * Should be expanded to actually check if the object is a Menu Object
	 */

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