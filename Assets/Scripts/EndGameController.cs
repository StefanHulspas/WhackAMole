using TMPro;
using UnityEngine;

public class EndGameController : MonoBehaviour
{
	[SerializeField]
	private GameObject _newHighScore = default;
	[SerializeField]
	private GameObject _notHighScore = default;
	[SerializeField]
	private GameController _gameController = default;
	[SerializeField]
	private HighScoreManager _highScoreManager = default;
	[SerializeField]
	private TMP_Text _finalScore = default;
	[SerializeField]
	private TMP_InputField _nameField = default;

	private void OnEnable()
	{
		int finalScore = _gameController.GameScore;
		_finalScore.text = $"Congratulations!\nYou scored {finalScore} points!";
		if (_highScoreManager.IsNewScoreHighScore(finalScore))
		{
			_newHighScore.SetActive(true);
		} else {
			_notHighScore.SetActive(true);
		}
	}

	public void AddScoreToHighScoreList() {
		PlayerHighScore newHighScore = new PlayerHighScore(_nameField.text, _gameController.GameScore);
		_highScoreManager.AddNewHighScore(newHighScore);
		_nameField.text = string.Empty;
		_newHighScore.SetActive(false);
		
	}

	private void OnDisable()
	{
		_newHighScore.SetActive(false);
		_notHighScore.SetActive(false);
	}
}
