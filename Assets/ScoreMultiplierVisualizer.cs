using ScriptableDataType;
using TMPro;
using UnityEngine;

public class ScoreMultiplierVisualizer : ShowIntData
{
	protected override void UpdateValue(int newValue)
	{
		if (newValue > 1) 
			_textField.text = newValue.ToString() + "x";
		else {
			_textField.text = string.Empty;
		}
	}
}
