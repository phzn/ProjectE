using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200007D RID: 125
	[NullableContext(1)]
	public interface ISerializationBinder
	{
		// Token: 0x06000666 RID: 1638
		Type BindToType([Nullable(2)] string assemblyName, string typeName);

		// Token: 0x06000667 RID: 1639
		[NullableContext(2)]
		void BindToName([Nullable(1)] Type serializedType, out string assemblyName, out string typeName);
	}
}
