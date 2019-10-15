using TMPro;
using UnityEngine;

public class InputFieldLimitCharacters : MonoBehaviour
{
	[SerializeField]
	private int _maxStringCharacters = 5;

	private TMP_InputField _inputField;
	
	public void OnEnable()
	{
		if (!_inputField)
			_inputField = GetComponent<TMP_InputField>();
	}

	public void OnValueChanged(string newValue) 
	{
		if (newValue.Length > _maxStringCharacters)
			_inputField.text = newValue.Substring(0, _maxStringCharacters);
	}
}