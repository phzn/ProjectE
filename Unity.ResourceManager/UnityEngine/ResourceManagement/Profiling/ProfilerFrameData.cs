using System;
using System.Collections.Generic;

namespace UnityEngine.ResourceManagement.Profiling
{
	// Token: 0x02000051 RID: 81
	internal class ProfilerFrameData<T1, T2>
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x00007223 File Offset: 0x00005423
		internal Dictionary<T1, T2> Data
		{
			get
			{
				return this.m_Data;
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000722B File Offset: 0x0000542B
		public ProfilerFrameData()
		{
			this.m_Data = new Dictionary<T1, T2>(32);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00007240 File Offset: 0x00005440
		public ProfilerFrameData(int count)
		{
			this.m_Data = new Dictionary<T1, T2>(count);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00007254 File Offset: 0x00005454
		public bool Add(T1 key, T2 value)
		{
			int num = (this.m_Data.ContainsKey(key) ? 1 : 0);
			this.m_Data[key] = value;
			this.m_Version += 1U;
			return num == 0;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00007280 File Offset: 0x00005480
		internal bool Remove(T1 key)
		{
			bool flag = this.m_Data.Remove(key);
			if (flag)
			{
				this.m_Version += 1U;
			}
			return flag;
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x000072A0 File Offset: 0x000054A0
		public T2[] Values
		{
			get
			{
				if (this.m_ArrayVersion == this.m_Version)
				{
					return this.m_Array ?? Array.Empty<T2>();
				}
				this.m_Array = new T2[this.m_Data.Count];
				this.m_Data.Values.CopyTo(this.m_Array, 0);
				this.m_ArrayVersion = this.m_Version;
				return this.m_Array;
			}
		}

		// Token: 0x1700006F RID: 111
		public T2 this[T1 key]
		{
			get
			{
				T2 t;
				if (!this.m_Data.TryGetValue(key, out t))
				{
					throw new ArgumentOutOfRangeException("Key " + key.ToString() + " not found for FrameData");
				}
				return t;
			}
			set
			{
				T2 t;
				if (this.m_Array != null && this.m_Data.TryGetValue(key, out t))
				{
					for (int i = 0; i < this.m_Array.Length; i++)
					{
						if (this.m_Array[i].Equals(t))
						{
							this.m_Array[i] = value;
							break;
						}
					}
				}
				this.m_Data[key] = value;
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x000073C1 File Offset: 0x000055C1
		public bool TryGetValue(T1 key, out T2 value)
		{
			return this.m_Data.TryGetValue(key, out value);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x000073D0 File Offset: 0x000055D0
		public bool ContainsKey(T1 key)
		{
			return this.m_Data.ContainsKey(key);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x000073DE File Offset: 0x000055DE
		public IEnumerable<KeyValuePair<T1, T2>> Enumerate()
		{
			foreach (KeyValuePair<T1, T2> keyValuePair in this.m_Data)
			{
				yield return keyValuePair;
			}
			Dictionary<T1, T2>.Enumerator enumerator = default(Dictionary<T1, T2>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x040000C5 RID: 197
		private Dictionary<T1, T2> m_Data;

		// Token: 0x040000C6 RID: 198
		private T2[] m_Array;

		// Token: 0x040000C7 RID: 199
		private uint m_Version;

		// Token: 0x040000C8 RID: 200
		private uint m_ArrayVersion;
	}
}
