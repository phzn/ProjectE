using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000042 RID: 66
	[Serializable]
	public abstract class TMP_InputValidator : ScriptableObject
	{
		// Token: 0x0600032C RID: 812
		public abstract char Validate(ref string text, ref int pos, char ch);
	}
}
