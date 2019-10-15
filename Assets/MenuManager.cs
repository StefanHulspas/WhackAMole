using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _currentActiveMenu;

    public void ShowMenu(GameObject newMenu)
    {
        _currentActiveMenu.SetActive(false);
        _currentActiveMenu = newMenu;
        _currentActiveMenu.SetActive(true);
    }
}
