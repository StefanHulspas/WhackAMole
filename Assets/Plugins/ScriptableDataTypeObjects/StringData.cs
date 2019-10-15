using UnityEngine;

namespace ScriptableDataType
{
	[CreateAssetMenu(fileName = "New StringData", menuName = "ScriptableObjects/VarData/String", order = 1)]
	public class StringData : VarData<string>
	{
		public StringData(string b) : base(b) { }
		public static StringData operator +(StringData a, string b)
		{
			a.Value += b;
			return a;
		}

		public static StringData operator +(StringData a, StringData b)
		{
			a.Value += b.Value;
			return a;
		}

		public static string operator +( string a, StringData b)
		{
			a += b.Value;
			return a;
		}

        
	}
}