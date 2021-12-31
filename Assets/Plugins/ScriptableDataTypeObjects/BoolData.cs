using UnityEngine;

namespace ScriptableDataType
{
	[CreateAssetMenu(fileName = "New BoolData", menuName = "ScriptableObjects/VarData/Bool", order = 1)]
	public class BoolData : VarData<bool>
	{
		public BoolData(bool b):base(b) { }
		
		public static implicit operator BoolData (bool data) {
			return new BoolData(data);
		}
	}
}