using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000040 RID: 64
	[NullableContext(2)]
	[Nullable(0)]
	public static class AotHelper
	{
		// Token: 0x0600041E RID: 1054 RVA: 0x000102A4 File Offset: 0x0000E4A4
		[NullableContext(1)]
		public static void Ensure(Action action)
		{
			if (AotHelper.IsFalse())
			{
				try
				{
					action();
				}
				catch (Exception ex)
				{
					throw new InvalidOperationException("", ex);
				}
			}
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x000102E0 File Offset: 0x0000E4E0
		public static void EnsureType<T>() where T : new()
		{
			AotHelper.Ensure(delegate
			{
				new T();
			});
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00010306 File Offset: 0x0000E506
		public static void EnsureList<T>()
		{
			AotHelper.Ensure(delegate
			{
				List<T> list = new List<T>();
				new HashSet<T>();
				new CollectionWrapper<T>(list);
				new CollectionWrapper<T>(list);
			});
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0001032C File Offset: 0x0000E52C
		public static void EnsureDictionary<TKey, TValue>()
		{
			AotHelper.Ensure(delegate
			{
				new Dictionary<TKey, TValue>();
				new DictionaryWrapper<TKey, TValue>(null);
				new DictionaryWrapper<TKey, TValue>(null);
				new DefaultContractResolver.EnumerableDictionaryWrapper<TKey, TValue>(null);
			});
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00010352 File Offset: 0x0000E552
		public static bool IsFalse()
		{
			return AotHelper.s_alwaysFalse;
		}

		// Token: 0x0400014B RID: 331
		private static bool s_alwaysFalse = DateTime.UtcNow.Year < 0;
	}
}
