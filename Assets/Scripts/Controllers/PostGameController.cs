using ScriptableDataType;
using TMPro;
using UnityEngine;

public class PostGameController : MonoBehaviour
{
	[SerializeField]
	private GameObject _newHighScore = default;
	[SerializeField]
	private GameObject _notHighScore = default;
	[SerializeField]
	private HighScoreController _highScoreManager = default;
	[SerializeField]
	private TMP_Text _finalScoreField = default;
	[SerializeField]
	private TMP_InputField _nameField = default;
	[SerializeField]
	private IntData _finalScore = default;

	private void OnEnable()
	{
		_finalScoreField.text = $"Congratulations!\nYou scored {_finalScore.Value} points!";
		if (_highScoreManager.IsNewScoreHighScore(_finalScore.Value))
		{
			_newHighScore.SetActive(true);
		} else {
			_notHighScore.SetActive(true);
		}
	}

	public void AddScoreToHighScoreList() {
		PlayerHighScore newHighScore = new PlayerHighScore(_nameField.text, _finalScore.Value);
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