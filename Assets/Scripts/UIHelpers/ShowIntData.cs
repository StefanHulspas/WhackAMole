using ScriptableDataType;
using TMPro;
using UnityEngine;

public class ShowIntData : MonoBehaviour
{
	[SerializeField]
	private IntData _intValue = default;

	protected TMP_Text _textField;

	private void OnEnable()
	{
		_textField = GetComponent<TMP_Text>();
		_intValue.AddListener(UpdateValue);
	}

	private void OnDisable()
	{
		_intValue.RemoveListener(UpdateValue);
	}

	protected virtual void UpdateValue(int newValue)
	{
		_textField.text = newValue.ToString();
	}
}