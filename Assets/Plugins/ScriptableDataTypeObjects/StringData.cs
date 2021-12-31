using UnityEngine;

namespace ScriptableDataType
{
	[CreateAssetMenu(fileName = "New StringData", menuName = "ScriptableObjects/VarData/String", order = 1)]
	public class StringData : VarData<string>
	{
		public StringData(string b) : base(b) { }
		
		public static implicit operator StringData (string data) {
			return new StringData(data);
		}
	}
}