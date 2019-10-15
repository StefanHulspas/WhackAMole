using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreVisualizer : MonoBehaviour
{
	[SerializeField]
	private HighScoreController _highScoreManager = default;
	[SerializeField]
	private HighScoreEntry _highScoreEntryPrefab = default;

	private void OnEnable()
	{
		List<PlayerHighScore> currentHighScores = _highScoreManager.GetCurrentHighScores;
		for (int place = 0; place < currentHighScores.Count; place++) {
			if (transform.childCount > place) {
				transform.GetChild(place).GetComponent<HighScoreEntry>().UpdateFields(place + 1, currentHighScores[place]);
			} else {
				HighScoreEntry newEntry = Instantiate(_highScoreEntryPrefab, transform);
				newEntry.UpdateFields(place + 1, currentHighScores[place]);
			}
		}
	}
}