using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000359 RID: 857
	internal static class StyleValueFunctionExtension
	{
		// Token: 0x06001CBE RID: 7358 RVA: 0x0006F930 File Offset: 0x0006DB30
		public static StyleValueFunction FromUssString(string ussValue)
		{
			ussValue = ussValue.ToLowerInvariant();
			string text = ussValue;
			string text2 = text;
			StyleValueFunction styleValueFunction;
			if (!(text2 == "var"))
			{
				if (!(text2 == "env"))
				{
					if (!(text2 == "linear-gradient"))
					{
						throw new ArgumentOutOfRangeException("ussValue", ussValue, "Unknown function name");
					}
					styleValueFunction = StyleValueFunction.LinearGradient;
				}
				else
				{
					styleValueFunction = StyleValueFunction.Env;
				}
			}
			else
			{
				styleValueFunction = StyleValueFunction.Var;
			}
			return styleValueFunction;
		}

		// Token: 0x06001CBF RID: 7359 RVA: 0x0006F994 File Offset: 0x0006DB94
		public static string ToUssString(this StyleValueFunction svf)
		{
			string text;
			switch (svf)
			{
			case StyleValueFunction.Var:
				text = "var";
				break;
			case StyleValueFunction.Env:
				text = "env";
				break;
			case StyleValueFunction.LinearGradient:
				text = "linear-gradient";
				break;
			default:
				throw new ArgumentOutOfRangeException("svf", svf, "Unknown StyleValueFunction");
			}
			return text;
		}

		// Token: 0x04000BF9 RID: 3065
		public const string k_Var = "var";

		// Token: 0x04000BFA RID: 3066
		public const string k_Env = "env";

		// Token: 0x04000BFB RID: 3067
		public const string k_LinearGradient = "linear-gradient";
	}
}
