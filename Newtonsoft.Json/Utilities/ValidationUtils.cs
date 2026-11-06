using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200006F RID: 111
	internal static class ValidationUtils
	{
		// Token: 0x060005F3 RID: 1523 RVA: 0x00018ABC File Offset: 0x00016CBC
		[NullableContext(1)]
		public static void ArgumentNotNull([Nullable(2)] [NotNull] object value, string parameterName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(parameterName);
			}
		}
	}
}
