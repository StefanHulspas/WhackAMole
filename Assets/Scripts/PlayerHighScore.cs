using System;

[Serializable]
public struct PlayerHighScore : IComparable<PlayerHighScore>
{
	public string _name;
	public int _score;

	public PlayerHighScore(string name, int score) {
		_name = name;
		_score = score;
	}

	public int CompareTo(PlayerHighScore other)
	{
		return -_score.CompareTo(other._score);
	}
}