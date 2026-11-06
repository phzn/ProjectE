using System;
using System.Collections.Concurrent;

namespace System.Runtime.Serialization
{
	// Token: 0x02000657 RID: 1623
	internal static class SerializationEventsCache
	{
		// Token: 0x06003CAF RID: 15535 RVA: 0x000D1CE0 File Offset: 0x000CFEE0
		internal static SerializationEvents GetSerializationEventsForType(Type t)
		{
			return SerializationEventsCache.s_cache.GetOrAdd(t, (Type type) => new SerializationEvents(type));
		}

		// Token: 0x04002728 RID: 10024
		private static readonly ConcurrentDictionary<Type, SerializationEvents> s_cache = new ConcurrentDictionary<Type, SerializationEvents>();
	}
}
