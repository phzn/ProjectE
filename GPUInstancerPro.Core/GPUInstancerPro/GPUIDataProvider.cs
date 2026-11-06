using System;
using System.Collections.Generic;

namespace GPUInstancerPro
{
	// Token: 0x02000006 RID: 6
	public abstract class GPUIDataProvider<K, T> : IGPUIDisposable, IDisposable
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020C8 File Offset: 0x000002C8
		// (set) Token: 0x06000006 RID: 6 RVA: 0x000020D0 File Offset: 0x000002D0
		public bool IsInitialized { get; protected set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000007 RID: 7 RVA: 0x000020D9 File Offset: 0x000002D9
		public int Count
		{
			get
			{
				if (this._dataDict == null)
				{
					return 0;
				}
				return this._dataDict.Count;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000020F0 File Offset: 0x000002F0
		public Dictionary<K, T>.ValueCollection Values
		{
			get
			{
				if (this._dataDict == null)
				{
					return null;
				}
				return this._dataDict.Values;
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002107 File Offset: 0x00000307
		public virtual void Initialize()
		{
			this.IsInitialized = true;
			if (this._dataDict == null)
			{
				this._dataDict = new Dictionary<K, T>();
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002123 File Offset: 0x00000323
		public virtual void ReleaseBuffers()
		{
			this.IsInitialized = false;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000212C File Offset: 0x0000032C
		public virtual void Dispose()
		{
			this.ReleaseBuffers();
			this._dataDict = null;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000213B File Offset: 0x0000033B
		public virtual void Reset()
		{
			this.Dispose();
			this.Initialize();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002149 File Offset: 0x00000349
		public virtual bool TryGetData(K key, out T result)
		{
			if (!this.IsInitialized)
			{
				result = default(T);
				return false;
			}
			return this._dataDict.TryGetValue(key, out result);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002169 File Offset: 0x00000369
		public virtual bool Remove(K key)
		{
			return this.IsInitialized && this._dataDict.Remove(key);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002181 File Offset: 0x00000381
		public virtual bool AddOrSet(K key, T value)
		{
			if (!this.IsInitialized)
			{
				return false;
			}
			this._dataDict[key] = value;
			return true;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000219C File Offset: 0x0000039C
		public virtual T GetFirstValue()
		{
			if (!this.IsInitialized)
			{
				return default(T);
			}
			Dictionary<K, T>.Enumerator enumerator = this._dataDict.GetEnumerator();
			enumerator.MoveNext();
			KeyValuePair<K, T> keyValuePair = enumerator.Current;
			return keyValuePair.Value;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000021E0 File Offset: 0x000003E0
		public virtual T GetValueAtIndex(int index)
		{
			if (!this.IsInitialized || this._dataDict.Count <= index)
			{
				return default(T);
			}
			Dictionary<K, T>.Enumerator enumerator = this._dataDict.GetEnumerator();
			for (int i = 0; i <= index; i++)
			{
				enumerator.MoveNext();
			}
			KeyValuePair<K, T> keyValuePair = enumerator.Current;
			return keyValuePair.Value;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000223C File Offset: 0x0000043C
		public virtual KeyValuePair<K, T> GetKVPairAtIndex(int index)
		{
			if (!this.IsInitialized || this._dataDict.Count <= index)
			{
				return default(KeyValuePair<K, T>);
			}
			Dictionary<K, T>.Enumerator enumerator = this._dataDict.GetEnumerator();
			for (int i = 0; i <= index; i++)
			{
				enumerator.MoveNext();
			}
			return enumerator.Current;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002290 File Offset: 0x00000490
		public Dictionary<K, T>.Enumerator GetEnumerator()
		{
			if (!this.IsInitialized)
			{
				return default(Dictionary<K, T>.Enumerator);
			}
			return this._dataDict.GetEnumerator();
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000022BA File Offset: 0x000004BA
		public bool ContainsKey(K key)
		{
			return this.IsInitialized && this._dataDict.ContainsKey(key);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000022D2 File Offset: 0x000004D2
		public virtual bool ContainsValue(T value)
		{
			return this.IsInitialized && this._dataDict.ContainsValue(value);
		}

		// Token: 0x04000007 RID: 7
		protected Dictionary<K, T> _dataDict;

		// Token: 0x04000008 RID: 8
		public Action OnInitialized;
	}
}
