using TMPro;
using UnityEngine;

public class HighScoreEntry : MonoBehaviour
{
	[SerializeField]
	private TMP_Text _placeField = default;
	[SerializeField]
	private TMP_Text _nameField = default;
	[SerializeField]
	private TMP_Text _scoreField = default;

	public void UpdateFields(int place, PlayerHighScore highScore) {
		_placeField.text = $"{place}.";
		_nameField.text = highScore._name;
		_scoreField.text = highScore._score.ToString();
	}
}