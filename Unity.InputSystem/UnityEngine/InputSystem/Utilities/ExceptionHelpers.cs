using System;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x02000132 RID: 306
	internal static class ExceptionHelpers
	{
		// Token: 0x0600110D RID: 4365 RVA: 0x00051AC4 File Offset: 0x0004FCC4
		public static bool IsExceptionIndicatingBugInCode(this Exception exception)
		{
			return exception is NullReferenceException || exception is IndexOutOfRangeException || exception is ArgumentException;
		}
	}
}
