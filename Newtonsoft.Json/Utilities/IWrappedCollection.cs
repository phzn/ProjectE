using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000046 RID: 70
	internal interface IWrappedCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000457 RID: 1111
		[Nullable(1)]
		object UnderlyingCollection
		{
			[NullableContext(1)]
			get;
		}
	}
}
