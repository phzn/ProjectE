using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000070 RID: 112
	internal static class CachedAttributeGetter<T> where T : Attribute
	{
		// Token: 0x060005F4 RID: 1524 RVA: 0x00018AC8 File Offset: 0x00016CC8
		[NullableContext(1)]
		[return: Nullable(2)]
		public static T GetAttribute(object type)
		{
			return CachedAttributeGetter<T>.TypeAttributeCache.Get(type);
		}

		// Token: 0x04000229 RID: 553
		[Nullable(new byte[] { 1, 1, 2 })]
		private static readonly ThreadSafeStore<object, T> TypeAttributeCache = new ThreadSafeStore<object, T>(new Func<object, T>(JsonTypeReflector.GetAttribute<T>));
	}
}
