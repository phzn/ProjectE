using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200007B RID: 123
	[NullableContext(1)]
	public interface IContractResolver
	{
		// Token: 0x06000661 RID: 1633
		JsonContract ResolveContract(Type type);
	}
}
