using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000236 RID: 566
	public struct StylePropertyNameCollection : IEnumerable<StylePropertyName>, IEnumerable
	{
		// Token: 0x0600103F RID: 4159 RVA: 0x0003B6A5 File Offset: 0x000398A5
		internal StylePropertyNameCollection(List<StylePropertyName> list)
		{
			this.propertiesList = list;
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x0003B6B0 File Offset: 0x000398B0
		public StylePropertyNameCollection.Enumerator GetEnumerator()
		{
			return new StylePropertyNameCollection.Enumerator(this.propertiesList.GetEnumerator());
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x0003B6D4 File Offset: 0x000398D4
		IEnumerator<StylePropertyName> IEnumerable<StylePropertyName>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x0003B6F4 File Offset: 0x000398F4
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x0003B714 File Offset: 0x00039914
		public bool Contains(StylePropertyName stylePropertyName)
		{
			bool flag2;
			using (List<StylePropertyName>.Enumerator enumerator = this.propertiesList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					StylePropertyName stylePropertyName2 = enumerator.Current;
					bool flag = stylePropertyName2 == stylePropertyName;
					if (flag)
					{
						return true;
					}
				}
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x04000736 RID: 1846
		internal List<StylePropertyName> propertiesList;

		// Token: 0x02000237 RID: 567
		public struct Enumerator : IEnumerator<StylePropertyName>, IEnumerator, IDisposable
		{
			// Token: 0x06001044 RID: 4164 RVA: 0x0003B778 File Offset: 0x00039978
			internal Enumerator(List<StylePropertyName>.Enumerator enumerator)
			{
				this.m_Enumerator = enumerator;
			}

			// Token: 0x06001045 RID: 4165 RVA: 0x0003B782 File Offset: 0x00039982
			public bool MoveNext()
			{
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x17000371 RID: 881
			// (get) Token: 0x06001046 RID: 4166 RVA: 0x0003B78F File Offset: 0x0003998F
			public StylePropertyName Current
			{
				get
				{
					return this.m_Enumerator.Current;
				}
			}

			// Token: 0x17000372 RID: 882
			// (get) Token: 0x06001047 RID: 4167 RVA: 0x0003B79C File Offset: 0x0003999C
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06001048 RID: 4168 RVA: 0x00003CD2 File Offset: 0x00001ED2
			public void Reset()
			{
			}

			// Token: 0x06001049 RID: 4169 RVA: 0x0003B7A9 File Offset: 0x000399A9
			public void Dispose()
			{
				this.m_Enumerator.Dispose();
			}

			// Token: 0x04000737 RID: 1847
			private List<StylePropertyName>.Enumerator m_Enumerator;
		}
	}
}
