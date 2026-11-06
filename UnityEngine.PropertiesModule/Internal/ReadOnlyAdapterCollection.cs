using System;
using System.Collections.Generic;

namespace Unity.Properties.Internal
{
	// Token: 0x020000CA RID: 202
	internal readonly struct ReadOnlyAdapterCollection
	{
		// Token: 0x060003FE RID: 1022 RVA: 0x0000C6F6 File Offset: 0x0000A8F6
		public ReadOnlyAdapterCollection(List<IPropertyVisitorAdapter> adapters)
		{
			this.m_Adapters = adapters;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000C700 File Offset: 0x0000A900
		public ReadOnlyAdapterCollection.Enumerator GetEnumerator()
		{
			return new ReadOnlyAdapterCollection.Enumerator(this);
		}

		// Token: 0x04000188 RID: 392
		private readonly List<IPropertyVisitorAdapter> m_Adapters;

		// Token: 0x020000CB RID: 203
		public struct Enumerator
		{
			// Token: 0x170000AD RID: 173
			// (get) Token: 0x06000400 RID: 1024 RVA: 0x0000C71D File Offset: 0x0000A91D
			// (set) Token: 0x06000401 RID: 1025 RVA: 0x0000C725 File Offset: 0x0000A925
			public IPropertyVisitorAdapter Current { readonly get; private set; }

			// Token: 0x06000402 RID: 1026 RVA: 0x0000C72E File Offset: 0x0000A92E
			public Enumerator(ReadOnlyAdapterCollection collection)
			{
				this.m_Adapters = collection.m_Adapters;
				this.m_Index = 0;
				this.Current = null;
			}

			// Token: 0x06000403 RID: 1027 RVA: 0x0000C74C File Offset: 0x0000A94C
			public bool MoveNext()
			{
				bool flag = this.m_Adapters == null;
				bool flag2;
				if (flag)
				{
					flag2 = false;
				}
				else
				{
					bool flag3 = this.m_Index >= this.m_Adapters.Count;
					if (flag3)
					{
						flag2 = false;
					}
					else
					{
						this.Current = this.m_Adapters[this.m_Index];
						this.m_Index++;
						flag2 = true;
					}
				}
				return flag2;
			}

			// Token: 0x06000404 RID: 1028 RVA: 0x0000C7B4 File Offset: 0x0000A9B4
			private void Reset()
			{
				this.m_Index = 0;
				this.Current = null;
			}

			// Token: 0x04000189 RID: 393
			private List<IPropertyVisitorAdapter> m_Adapters;

			// Token: 0x0400018A RID: 394
			private int m_Index;
		}
	}
}
