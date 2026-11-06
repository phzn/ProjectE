using System;
using System.Collections.Generic;
using Unity.Properties;

namespace Unity.Serialization.Json
{
	// Token: 0x02000062 RID: 98
	internal class DictionaryElementProperty<TDictionary, TKey, TValue> : Property<TDictionary, TValue> where TDictionary : IDictionary<TKey, TValue>
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0000A430 File Offset: 0x00008630
		public override string Name
		{
			get
			{
				TKey key = this.Key;
				return key.ToString();
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0000262E File Offset: 0x0000082E
		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000A451 File Offset: 0x00008651
		public override TValue GetValue(ref TDictionary container)
		{
			return container[this.Key];
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000A465 File Offset: 0x00008665
		public override void SetValue(ref TDictionary container, TValue value)
		{
			container[this.Key] = value;
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000A47A File Offset: 0x0000867A
		// (set) Token: 0x0600025A RID: 602 RVA: 0x0000A482 File Offset: 0x00008682
		public TKey Key { get; internal set; }
	}
}
