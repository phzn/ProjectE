using System;
using System.Collections.Generic;

namespace System.Threading
{
	// Token: 0x02000285 RID: 645
	internal static class AsyncLocalValueMap
	{
		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06001D66 RID: 7526 RVA: 0x0006DB74 File Offset: 0x0006BD74
		public static IAsyncLocalValueMap Empty { get; } = new AsyncLocalValueMap.EmptyAsyncLocalValueMap();

		// Token: 0x02000286 RID: 646
		internal sealed class EmptyAsyncLocalValueMap : IAsyncLocalValueMap
		{
			// Token: 0x06001D68 RID: 7528 RVA: 0x0006DB88 File Offset: 0x0006BD88
			public IAsyncLocalValueMap Set(IAsyncLocal key, object value)
			{
				if (value == null)
				{
					return this;
				}
				return new AsyncLocalValueMap.OneElementAsyncLocalValueMap(key, value);
			}

			// Token: 0x06001D69 RID: 7529 RVA: 0x0006DBA5 File Offset: 0x0006BDA5
			public bool TryGetValue(IAsyncLocal key, out object value)
			{
				value = null;
				return false;
			}
		}

		// Token: 0x02000287 RID: 647
		internal sealed class OneElementAsyncLocalValueMap : IAsyncLocalValueMap
		{
			// Token: 0x06001D6B RID: 7531 RVA: 0x0006DBAB File Offset: 0x0006BDAB
			public OneElementAsyncLocalValueMap(IAsyncLocal key, object value)
			{
				this._key1 = key;
				this._value1 = value;
			}

			// Token: 0x06001D6C RID: 7532 RVA: 0x0006DBC4 File Offset: 0x0006BDC4
			public IAsyncLocalValueMap Set(IAsyncLocal key, object value)
			{
				if (value != null)
				{
					if (key != this._key1)
					{
						return new AsyncLocalValueMap.TwoElementAsyncLocalValueMap(this._key1, this._value1, key, value);
					}
					return new AsyncLocalValueMap.OneElementAsyncLocalValueMap(key, value);
				}
				else
				{
					if (key != this._key1)
					{
						return this;
					}
					return AsyncLocalValueMap.Empty;
				}
			}

			// Token: 0x06001D6D RID: 7533 RVA: 0x0006DC0F File Offset: 0x0006BE0F
			public bool TryGetValue(IAsyncLocal key, out object value)
			{
				if (key == this._key1)
				{
					value = this._value1;
					return true;
				}
				value = null;
				return false;
			}

			// Token: 0x04001A29 RID: 6697
			private readonly IAsyncLocal _key1;

			// Token: 0x04001A2A RID: 6698
			private readonly object _value1;
		}

		// Token: 0x02000288 RID: 648
		private sealed class TwoElementAsyncLocalValueMap : IAsyncLocalValueMap
		{
			// Token: 0x06001D6E RID: 7534 RVA: 0x0006DC28 File Offset: 0x0006BE28
			public TwoElementAsyncLocalValueMap(IAsyncLocal key1, object value1, IAsyncLocal key2, object value2)
			{
				this._key1 = key1;
				this._value1 = value1;
				this._key2 = key2;
				this._value2 = value2;
			}

			// Token: 0x06001D6F RID: 7535 RVA: 0x0006DC50 File Offset: 0x0006BE50
			public IAsyncLocalValueMap Set(IAsyncLocal key, object value)
			{
				if (value != null)
				{
					if (key == this._key1)
					{
						return new AsyncLocalValueMap.TwoElementAsyncLocalValueMap(key, value, this._key2, this._value2);
					}
					if (key != this._key2)
					{
						return new AsyncLocalValueMap.ThreeElementAsyncLocalValueMap(this._key1, this._value1, this._key2, this._value2, key, value);
					}
					return new AsyncLocalValueMap.TwoElementAsyncLocalValueMap(this._key1, this._value1, key, value);
				}
				else
				{
					if (key == this._key1)
					{
						return new AsyncLocalValueMap.OneElementAsyncLocalValueMap(this._key2, this._value2);
					}
					if (key != this._key2)
					{
						return this;
					}
					return new AsyncLocalValueMap.OneElementAsyncLocalValueMap(this._key1, this._value1);
				}
			}

			// Token: 0x06001D70 RID: 7536 RVA: 0x0006DCFD File Offset: 0x0006BEFD
			public bool TryGetValue(IAsyncLocal key, out object value)
			{
				if (key == this._key1)
				{
					value = this._value1;
					return true;
				}
				if (key == this._key2)
				{
					value = this._value2;
					return true;
				}
				value = null;
				return false;
			}

			// Token: 0x04001A2B RID: 6699
			private readonly IAsyncLocal _key1;

			// Token: 0x04001A2C RID: 6700
			private readonly IAsyncLocal _key2;

			// Token: 0x04001A2D RID: 6701
			private readonly object _value1;

			// Token: 0x04001A2E RID: 6702
			private readonly object _value2;
		}

		// Token: 0x02000289 RID: 649
		private sealed class ThreeElementAsyncLocalValueMap : IAsyncLocalValueMap
		{
			// Token: 0x06001D71 RID: 7537 RVA: 0x0006DD29 File Offset: 0x0006BF29
			public ThreeElementAsyncLocalValueMap(IAsyncLocal key1, object value1, IAsyncLocal key2, object value2, IAsyncLocal key3, object value3)
			{
				this._key1 = key1;
				this._value1 = value1;
				this._key2 = key2;
				this._value2 = value2;
				this._key3 = key3;
				this._value3 = value3;
			}

			// Token: 0x06001D72 RID: 7538 RVA: 0x0006DD60 File Offset: 0x0006BF60
			public IAsyncLocalValueMap Set(IAsyncLocal key, object value)
			{
				if (value != null)
				{
					if (key == this._key1)
					{
						return new AsyncLocalValueMap.ThreeElementAsyncLocalValueMap(key, value, this._key2, this._value2, this._key3, this._value3);
					}
					if (key == this._key2)
					{
						return new AsyncLocalValueMap.ThreeElementAsyncLocalValueMap(this._key1, this._value1, key, value, this._key3, this._value3);
					}
					if (key == this._key3)
					{
						return new AsyncLocalValueMap.ThreeElementAsyncLocalValueMap(this._key1, this._value1, this._key2, this._value2, key, value);
					}
					AsyncLocalValueMap.MultiElementAsyncLocalValueMap multiElementAsyncLocalValueMap = new AsyncLocalValueMap.MultiElementAsyncLocalValueMap(4);
					multiElementAsyncLocalValueMap.UnsafeStore(0, this._key1, this._value1);
					multiElementAsyncLocalValueMap.UnsafeStore(1, this._key2, this._value2);
					multiElementAsyncLocalValueMap.UnsafeStore(2, this._key3, this._value3);
					multiElementAsyncLocalValueMap.UnsafeStore(3, key, value);
					return multiElementAsyncLocalValueMap;
				}
				else
				{
					if (key == this._key1)
					{
						return new AsyncLocalValueMap.TwoElementAsyncLocalValueMap(this._key2, this._value2, this._key3, this._value3);
					}
					if (key == this._key2)
					{
						return new AsyncLocalValueMap.TwoElementAsyncLocalValueMap(this._key1, this._value1, this._key3, this._value3);
					}
					if (key != this._key3)
					{
						return this;
					}
					return new AsyncLocalValueMap.TwoElementAsyncLocalValueMap(this._key1, this._value1, this._key2, this._value2);
				}
			}

			// Token: 0x06001D73 RID: 7539 RVA: 0x0006DEB5 File Offset: 0x0006C0B5
			public bool TryGetValue(IAsyncLocal key, out object value)
			{
				if (key == this._key1)
				{
					value = this._value1;
					return true;
				}
				if (key == this._key2)
				{
					value = this._value2;
					return true;
				}
				if (key == this._key3)
				{
					value = this._value3;
					return true;
				}
				value = null;
				return false;
			}

			// Token: 0x04001A2F RID: 6703
			private readonly IAsyncLocal _key1;

			// Token: 0x04001A30 RID: 6704
			private readonly IAsyncLocal _key2;

			// Token: 0x04001A31 RID: 6705
			private readonly IAsyncLocal _key3;

			// Token: 0x04001A32 RID: 6706
			private readonly object _value1;

			// Token: 0x04001A33 RID: 6707
			private readonly object _value2;

			// Token: 0x04001A34 RID: 6708
			private readonly object _value3;
		}

		// Token: 0x0200028A RID: 650
		private sealed class MultiElementAsyncLocalValueMap : IAsyncLocalValueMap
		{
			// Token: 0x06001D74 RID: 7540 RVA: 0x0006DEF4 File Offset: 0x0006C0F4
			internal MultiElementAsyncLocalValueMap(int count)
			{
				this._keyValues = new KeyValuePair<IAsyncLocal, object>[count];
			}

			// Token: 0x06001D75 RID: 7541 RVA: 0x0006DF08 File Offset: 0x0006C108
			internal void UnsafeStore(int index, IAsyncLocal key, object value)
			{
				this._keyValues[index] = new KeyValuePair<IAsyncLocal, object>(key, value);
			}

			// Token: 0x06001D76 RID: 7542 RVA: 0x0006DF20 File Offset: 0x0006C120
			public IAsyncLocalValueMap Set(IAsyncLocal key, object value)
			{
				int i = 0;
				while (i < this._keyValues.Length)
				{
					if (key == this._keyValues[i].Key)
					{
						if (value != null)
						{
							AsyncLocalValueMap.MultiElementAsyncLocalValueMap multiElementAsyncLocalValueMap = new AsyncLocalValueMap.MultiElementAsyncLocalValueMap(this._keyValues.Length);
							Array.Copy(this._keyValues, 0, multiElementAsyncLocalValueMap._keyValues, 0, this._keyValues.Length);
							multiElementAsyncLocalValueMap._keyValues[i] = new KeyValuePair<IAsyncLocal, object>(key, value);
							return multiElementAsyncLocalValueMap;
						}
						if (this._keyValues.Length != 4)
						{
							AsyncLocalValueMap.MultiElementAsyncLocalValueMap multiElementAsyncLocalValueMap2 = new AsyncLocalValueMap.MultiElementAsyncLocalValueMap(this._keyValues.Length - 1);
							if (i != 0)
							{
								Array.Copy(this._keyValues, 0, multiElementAsyncLocalValueMap2._keyValues, 0, i);
							}
							if (i != this._keyValues.Length - 1)
							{
								Array.Copy(this._keyValues, i + 1, multiElementAsyncLocalValueMap2._keyValues, i, this._keyValues.Length - i - 1);
							}
							return multiElementAsyncLocalValueMap2;
						}
						if (i == 0)
						{
							return new AsyncLocalValueMap.ThreeElementAsyncLocalValueMap(this._keyValues[1].Key, this._keyValues[1].Value, this._keyValues[2].Key, this._keyValues[2].Value, this._keyValues[3].Key, this._keyValues[3].Value);
						}
						if (i == 1)
						{
							return new AsyncLocalValueMap.ThreeElementAsyncLocalValueMap(this._keyValues[0].Key, this._keyValues[0].Value, this._keyValues[2].Key, this._keyValues[2].Value, this._keyValues[3].Key, this._keyValues[3].Value);
						}
						if (i != 2)
						{
							return new AsyncLocalValueMap.ThreeElementAsyncLocalValueMap(this._keyValues[0].Key, this._keyValues[0].Value, this._keyValues[1].Key, this._keyValues[1].Value, this._keyValues[2].Key, this._keyValues[2].Value);
						}
						return new AsyncLocalValueMap.ThreeElementAsyncLocalValueMap(this._keyValues[0].Key, this._keyValues[0].Value, this._keyValues[1].Key, this._keyValues[1].Value, this._keyValues[3].Key, this._keyValues[3].Value);
					}
					else
					{
						i++;
					}
				}
				if (value == null)
				{
					return this;
				}
				if (this._keyValues.Length < 16)
				{
					AsyncLocalValueMap.MultiElementAsyncLocalValueMap multiElementAsyncLocalValueMap3 = new AsyncLocalValueMap.MultiElementAsyncLocalValueMap(this._keyValues.Length + 1);
					Array.Copy(this._keyValues, 0, multiElementAsyncLocalValueMap3._keyValues, 0, this._keyValues.Length);
					multiElementAsyncLocalValueMap3._keyValues[this._keyValues.Length] = new KeyValuePair<IAsyncLocal, object>(key, value);
					return multiElementAsyncLocalValueMap3;
				}
				AsyncLocalValueMap.ManyElementAsyncLocalValueMap manyElementAsyncLocalValueMap = new AsyncLocalValueMap.ManyElementAsyncLocalValueMap(17);
				foreach (KeyValuePair<IAsyncLocal, object> keyValuePair in this._keyValues)
				{
					manyElementAsyncLocalValueMap[keyValuePair.Key] = keyValuePair.Value;
				}
				manyElementAsyncLocalValueMap[key] = value;
				return manyElementAsyncLocalValueMap;
			}

			// Token: 0x06001D77 RID: 7543 RVA: 0x0006E278 File Offset: 0x0006C478
			public bool TryGetValue(IAsyncLocal key, out object value)
			{
				foreach (KeyValuePair<IAsyncLocal, object> keyValuePair in this._keyValues)
				{
					if (key == keyValuePair.Key)
					{
						value = keyValuePair.Value;
						return true;
					}
				}
				value = null;
				return false;
			}

			// Token: 0x04001A35 RID: 6709
			internal const int MaxMultiElements = 16;

			// Token: 0x04001A36 RID: 6710
			private readonly KeyValuePair<IAsyncLocal, object>[] _keyValues;
		}

		// Token: 0x0200028B RID: 651
		private sealed class ManyElementAsyncLocalValueMap : Dictionary<IAsyncLocal, object>, IAsyncLocalValueMap
		{
			// Token: 0x06001D78 RID: 7544 RVA: 0x0006E2BB File Offset: 0x0006C4BB
			public ManyElementAsyncLocalValueMap(int capacity)
				: base(capacity)
			{
			}

			// Token: 0x06001D79 RID: 7545 RVA: 0x0006E2C4 File Offset: 0x0006C4C4
			public IAsyncLocalValueMap Set(IAsyncLocal key, object value)
			{
				int count = base.Count;
				bool flag = base.ContainsKey(key);
				if (value != null)
				{
					AsyncLocalValueMap.ManyElementAsyncLocalValueMap manyElementAsyncLocalValueMap = new AsyncLocalValueMap.ManyElementAsyncLocalValueMap(count + (flag ? 0 : 1));
					foreach (KeyValuePair<IAsyncLocal, object> keyValuePair in this)
					{
						manyElementAsyncLocalValueMap[keyValuePair.Key] = keyValuePair.Value;
					}
					manyElementAsyncLocalValueMap[key] = value;
					return manyElementAsyncLocalValueMap;
				}
				if (!flag)
				{
					return this;
				}
				if (count == 17)
				{
					AsyncLocalValueMap.MultiElementAsyncLocalValueMap multiElementAsyncLocalValueMap = new AsyncLocalValueMap.MultiElementAsyncLocalValueMap(16);
					int num = 0;
					foreach (KeyValuePair<IAsyncLocal, object> keyValuePair2 in this)
					{
						if (key != keyValuePair2.Key)
						{
							multiElementAsyncLocalValueMap.UnsafeStore(num++, keyValuePair2.Key, keyValuePair2.Value);
						}
					}
					return multiElementAsyncLocalValueMap;
				}
				AsyncLocalValueMap.ManyElementAsyncLocalValueMap manyElementAsyncLocalValueMap2 = new AsyncLocalValueMap.ManyElementAsyncLocalValueMap(count - 1);
				foreach (KeyValuePair<IAsyncLocal, object> keyValuePair3 in this)
				{
					if (key != keyValuePair3.Key)
					{
						manyElementAsyncLocalValueMap2[keyValuePair3.Key] = keyValuePair3.Value;
					}
				}
				return manyElementAsyncLocalValueMap2;
			}
		}
	}
}
