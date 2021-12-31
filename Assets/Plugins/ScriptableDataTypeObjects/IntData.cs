using UnityEngine;

namespace ScriptableDataType
{
	[CreateAssetMenu(fileName = "New IntData", menuName = "ScriptableObjects/VarData/Int", order = 1)]
	public class IntData : VarData<int>
	{
		public IntData(int b) : base(b) { }
		
		public static implicit operator IntData (int data) {
			return new IntData(data);
		}
	}
}