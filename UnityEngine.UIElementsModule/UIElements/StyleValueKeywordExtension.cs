using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200035C RID: 860
	internal static class StyleValueKeywordExtension
	{
		// Token: 0x06001CC3 RID: 7363 RVA: 0x0006FA20 File Offset: 0x0006DC20
		public static string ToUssString(this StyleValueKeyword svk)
		{
			string text;
			switch (svk)
			{
			case StyleValueKeyword.Inherit:
				text = "inherit";
				break;
			case StyleValueKeyword.Initial:
				text = "initial";
				break;
			case StyleValueKeyword.Auto:
				text = "auto";
				break;
			case StyleValueKeyword.Unset:
				text = "unset";
				break;
			case StyleValueKeyword.True:
				text = "true";
				break;
			case StyleValueKeyword.False:
				text = "false";
				break;
			case StyleValueKeyword.None:
				text = "none";
				break;
			default:
				throw new ArgumentOutOfRangeException("svk", svk, "Unknown StyleValueKeyword");
			}
			return text;
		}
	}
}
