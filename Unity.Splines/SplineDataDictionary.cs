using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.Splines
{
	// Token: 0x02000025 RID: 37
	[Serializable]
	internal class SplineDataDictionary<T> : IEnumerable<SplineDataKeyValuePair<T>>, IEnumerable
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00007F3E File Offset: 0x0000613E
		public IEnumerable<string> Keys
		{
			get
			{
				return this.m_Data.Select<SplineDataKeyValuePair<T>, string>((SplineDataKeyValuePair<T> x) => x.Key);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00007F6A File Offset: 0x0000616A
		public IEnumerable<SplineData<T>> Values
		{
			get
			{
				return this.m_Data.Select<SplineDataKeyValuePair<T>, SplineData<T>>((SplineDataKeyValuePair<T> x) => x.Value);
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00007F98 File Offset: 0x00006198
		private int FindIndex(string key)
		{
			int i = 0;
			int count = this.m_Data.Count;
			while (i < count)
			{
				if (this.m_Data[i].Key == key)
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00007FDC File Offset: 0x000061DC
		public bool TryGetValue(string key, out SplineData<T> value)
		{
			int num = this.FindIndex(key);
			value = ((num < 0) ? null : this.m_Data[num].Value);
			return num > -1;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00008010 File Offset: 0x00006210
		public SplineData<T> GetOrCreate(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}
			SplineData<T> splineData;
			if (!this.TryGetValue(key, out splineData))
			{
				List<SplineDataKeyValuePair<T>> data = this.m_Data;
				SplineDataKeyValuePair<T> splineDataKeyValuePair = new SplineDataKeyValuePair<T>();
				splineDataKeyValuePair.Key = key;
				splineData = (splineDataKeyValuePair.Value = new SplineData<T>());
				data.Add(splineDataKeyValuePair);
			}
			return splineData;
		}

		// Token: 0x17000052 RID: 82
		public SplineData<T> this[string key]
		{
			get
			{
				SplineData<T> splineData;
				if (!this.TryGetValue(key, out splineData))
				{
					return null;
				}
				return splineData;
			}
			set
			{
				int num = this.FindIndex(key);
				SplineData<T> splineData = new SplineData<T>(value);
				if (num < 0)
				{
					this.m_Data.Add(new SplineDataKeyValuePair<T>
					{
						Key = key,
						Value = splineData
					});
					return;
				}
				this.m_Data[num].Value = splineData;
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x000080CD File Offset: 0x000062CD
		public bool Contains(string key)
		{
			return this.FindIndex(key) > -1;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x000080D9 File Offset: 0x000062D9
		public IEnumerator<SplineDataKeyValuePair<T>> GetEnumerator()
		{
			return this.m_Data.GetEnumerator();
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x000080EB File Offset: 0x000062EB
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)this.m_Data).GetEnumerator();
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x000080F8 File Offset: 0x000062F8
		public bool Remove(string key)
		{
			int num = this.FindIndex(key);
			if (num < 0)
			{
				return false;
			}
			this.m_Data.RemoveAt(num);
			return true;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00008120 File Offset: 0x00006320
		public void RemoveEmpty()
		{
			int i = this.m_Data.Count - 1;
			while (i > -1)
			{
				if (string.IsNullOrEmpty(this.m_Data[i].Key))
				{
					goto IL_004A;
				}
				SplineData<T> value = this.m_Data[i].Value;
				if (value != null && value.Count < 1)
				{
					goto IL_004A;
				}
				IL_0080:
				i--;
				continue;
				IL_004A:
				Debug.Log(string.Format("{0} remove empty key \"{1}\"", typeof(T), this.m_Data[i].Key));
				this.m_Data.RemoveAt(i);
				goto IL_0080;
			}
		}

		// Token: 0x04000086 RID: 134
		[SerializeField]
		private List<SplineDataKeyValuePair<T>> m_Data = new List<SplineDataKeyValuePair<T>>();
	}
}
