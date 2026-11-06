using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200004F RID: 79
	internal interface IWrappedDictionary : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060004B4 RID: 1204
		[Nullable(1)]
		object UnderlyingDictionary
		{
			[NullableContext(1)]
			get;
		}
	}
}
