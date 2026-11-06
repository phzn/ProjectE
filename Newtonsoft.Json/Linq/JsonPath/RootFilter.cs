using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000D9 RID: 217
	[NullableContext(1)]
	[Nullable(0)]
	internal class RootFilter : PathFilter
	{
		// Token: 0x06000C0F RID: 3087 RVA: 0x0002FE07 File Offset: 0x0002E007
		private RootFilter()
		{
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x0002FE0F File Offset: 0x0002E00F
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, [Nullable(2)] JsonSelectSettings settings)
		{
			return new JToken[] { root };
		}

		// Token: 0x040003E3 RID: 995
		public static readonly RootFilter Instance = new RootFilter();
	}
}
