using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200006D RID: 109
	[NullableContext(1)]
	[Nullable(0)]
	internal class ThreadSafeStore<TKey, [Nullable(2)] TValue>
	{
		// Token: 0x060005DF RID: 1503 RVA: 0x0001890F File Offset: 0x00016B0F
		public ThreadSafeStore(Func<TKey, TValue> creator)
		{
			ValidationUtils.ArgumentNotNull(creator, "creator");
			this._creator = creator;
			this._concurrentStore = new ConcurrentDictionary<TKey, TValue>();
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00018934 File Offset: 0x00016B34
		public TValue Get(TKey key)
		{
			return this._concurrentStore.GetOrAdd(key, this._creator);
		}

		// Token: 0x04000227 RID: 551
		private readonly ConcurrentDictionary<TKey, TValue> _concurrentStore;

		// Token: 0x04000228 RID: 552
		private readonly Func<TKey, TValue> _creator;
	}
}
