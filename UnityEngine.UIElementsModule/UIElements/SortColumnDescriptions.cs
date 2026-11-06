using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnityEngine.UIElements
{
	// Token: 0x02000106 RID: 262
	[Serializable]
	public class SortColumnDescriptions : ICollection<SortColumnDescription>, IEnumerable<SortColumnDescription>, IEnumerable
	{
		// Token: 0x14000033 RID: 51
		// (add) Token: 0x06000900 RID: 2304 RVA: 0x000231FC File Offset: 0x000213FC
		// (remove) Token: 0x06000901 RID: 2305 RVA: 0x00023234 File Offset: 0x00021434
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal event Action changed;

		// Token: 0x06000902 RID: 2306 RVA: 0x0002326C File Offset: 0x0002146C
		public IEnumerator<SortColumnDescription> GetEnumerator()
		{
			return this.m_Descriptions.GetEnumerator();
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x0002328C File Offset: 0x0002148C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x000232A4 File Offset: 0x000214A4
		public void Add(SortColumnDescription item)
		{
			this.Insert(this.m_Descriptions.Count, item);
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x000232BC File Offset: 0x000214BC
		public void Clear()
		{
			while (this.m_Descriptions.Count > 0)
			{
				this.Remove(this.m_Descriptions[0]);
			}
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x000232F4 File Offset: 0x000214F4
		public bool Contains(SortColumnDescription item)
		{
			return this.m_Descriptions.Contains(item);
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x00023312 File Offset: 0x00021512
		public void CopyTo(SortColumnDescription[] array, int arrayIndex)
		{
			this.m_Descriptions.CopyTo(array, arrayIndex);
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x00023324 File Offset: 0x00021524
		public bool Remove(SortColumnDescription desc)
		{
			bool flag = desc == null;
			if (flag)
			{
				throw new ArgumentException("Cannot remove null description");
			}
			bool flag2 = this.m_Descriptions.Remove(desc);
			bool flag3;
			if (flag2)
			{
				desc.column = null;
				desc.changed -= this.OnDescriptionChanged;
				Action action = this.changed;
				if (action != null)
				{
					action();
				}
				flag3 = true;
			}
			else
			{
				flag3 = false;
			}
			return flag3;
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0002338C File Offset: 0x0002158C
		private void OnDescriptionChanged(SortColumnDescription desc)
		{
			Action action = this.changed;
			if (action != null)
			{
				action();
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x0600090A RID: 2314 RVA: 0x000233A1 File Offset: 0x000215A1
		public int Count
		{
			get
			{
				return this.m_Descriptions.Count;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x0600090B RID: 2315 RVA: 0x000233AE File Offset: 0x000215AE
		public bool IsReadOnly
		{
			get
			{
				return this.m_Descriptions.IsReadOnly;
			}
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x000233BC File Offset: 0x000215BC
		public int IndexOf(SortColumnDescription desc)
		{
			return this.m_Descriptions.IndexOf(desc);
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x000233DC File Offset: 0x000215DC
		public void Insert(int index, SortColumnDescription desc)
		{
			bool flag = desc == null;
			if (flag)
			{
				throw new ArgumentException("Cannot insert null description");
			}
			bool flag2 = this.Contains(desc);
			if (flag2)
			{
				throw new ArgumentException("Already contains this description");
			}
			this.m_Descriptions.Insert(index, desc);
			desc.changed += this.OnDescriptionChanged;
			Action action = this.changed;
			if (action != null)
			{
				action();
			}
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x00023446 File Offset: 0x00021646
		public void RemoveAt(int index)
		{
			this.Remove(this.m_Descriptions[index]);
		}

		// Token: 0x170001B2 RID: 434
		public SortColumnDescription this[int index]
		{
			get
			{
				return this.m_Descriptions[index];
			}
		}

		// Token: 0x04000412 RID: 1042
		[SerializeField]
		private readonly IList<SortColumnDescription> m_Descriptions = new List<SortColumnDescription>();

		// Token: 0x02000107 RID: 263
		internal class UxmlObjectFactory<T> : UxmlObjectFactory<T, SortColumnDescriptions.UxmlObjectTraits<T>> where T : SortColumnDescriptions, new()
		{
		}

		// Token: 0x02000108 RID: 264
		internal class UxmlObjectTraits<T> : UnityEngine.UIElements.UxmlObjectTraits<T> where T : SortColumnDescriptions
		{
			// Token: 0x06000912 RID: 2322 RVA: 0x00023498 File Offset: 0x00021698
			public override void Init(ref T obj, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ref obj, bag, cc);
				List<SortColumnDescription> valueFromBag = this.m_SortColumnDescriptions.GetValueFromBag(bag, cc);
				bool flag = valueFromBag != null;
				if (flag)
				{
					foreach (SortColumnDescription sortColumnDescription in valueFromBag)
					{
						obj.Add(sortColumnDescription);
					}
				}
			}

			// Token: 0x04000414 RID: 1044
			private readonly UxmlObjectListAttributeDescription<SortColumnDescription> m_SortColumnDescriptions = new UxmlObjectListAttributeDescription<SortColumnDescription>();
		}
	}
}
