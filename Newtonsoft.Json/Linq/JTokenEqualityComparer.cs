using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000C4 RID: 196
	public class JTokenEqualityComparer : IEqualityComparer<JToken>
	{
		// Token: 0x06000B54 RID: 2900 RVA: 0x0002C6EA File Offset: 0x0002A8EA
		[NullableContext(2)]
		public bool Equals(JToken x, JToken y)
		{
			return JToken.DeepEquals(x, y);
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x0002C6F3 File Offset: 0x0002A8F3
		[NullableContext(1)]
		public int GetHashCode(JToken obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetDeepHashCode();
		}
	}
}
