using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
	private readonly string _playerPrefKey = "HighScoreList";
	[SerializeField]
	private List<PlayerHighScore> _highScores = new List<PlayerHighScore>();
	private int _highScoreSize = 5;

	private void OnEnable()
	{
		if (PlayerPrefs.HasKey(_playerPrefKey))
			JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(_playerPrefKey), this);
	}

	private void OnDisable()
	{
		PlayerPrefs.SetString(_playerPrefKey, JsonUtility.ToJson(this));
		PlayerPrefs.Save();
	}

	public bool IsNewScoreHighScore(int newScore) {
		return _highScores.Count < _highScoreSize || _highScores[_highScores.Count - 1]._score < newScore;
	}

	public void AddNewHighScore(PlayerHighScore newHighScore) {
		_highScores.Add(newHighScore);
		_highScores.Sort();
		while (_highScores.Count > _highScoreSize) {
			_highScores.RemoveAt(_highScoreSize);
		}
	}

	public List<PlayerHighScore> GetCurrentHighScores => _highScores;
}
