using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x02000143 RID: 323
	internal struct OneOrMore<TValue, TList> : IReadOnlyList<TValue>, IEnumerable<TValue>, IEnumerable, IReadOnlyCollection<TValue> where TList : IReadOnlyList<TValue>
	{
		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x060011AB RID: 4523 RVA: 0x0005464C File Offset: 0x0005284C
		public int Count
		{
			get
			{
				if (!this.m_IsSingle)
				{
					TList multiple = this.m_Multiple;
					return multiple.Count;
				}
				return 1;
			}
		}

		// Token: 0x170004DF RID: 1247
		public TValue this[int index]
		{
			get
			{
				if (!this.m_IsSingle)
				{
					TList multiple = this.m_Multiple;
					return multiple[index];
				}
				if (index < 0 || index > 1)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return this.m_Single;
			}
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x000546BC File Offset: 0x000528BC
		public OneOrMore(TValue single)
		{
			this.m_IsSingle = true;
			this.m_Single = single;
			this.m_Multiple = default(TList);
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x000546D8 File Offset: 0x000528D8
		public OneOrMore(TList multiple)
		{
			this.m_IsSingle = false;
			this.m_Single = default(TValue);
			this.m_Multiple = multiple;
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x000546F4 File Offset: 0x000528F4
		public static implicit operator OneOrMore<TValue, TList>(TValue single)
		{
			return new OneOrMore<TValue, TList>(single);
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x000546FC File Offset: 0x000528FC
		public static implicit operator OneOrMore<TValue, TList>(TList multiple)
		{
			return new OneOrMore<TValue, TList>(multiple);
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x00054704 File Offset: 0x00052904
		public IEnumerator<TValue> GetEnumerator()
		{
			return new OneOrMore<TValue, TList>.Enumerator
			{
				m_List = this
			};
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x00054717 File Offset: 0x00052917
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000725 RID: 1829
		private readonly bool m_IsSingle;

		// Token: 0x04000726 RID: 1830
		private readonly TValue m_Single;

		// Token: 0x04000727 RID: 1831
		private readonly TList m_Multiple;

		// Token: 0x02000251 RID: 593
		private class Enumerator : IEnumerator<TValue>, IEnumerator, IDisposable
		{
			// Token: 0x0600165B RID: 5723 RVA: 0x00066BBA File Offset: 0x00064DBA
			public bool MoveNext()
			{
				this.m_Index++;
				return this.m_Index < this.m_List.Count;
			}

			// Token: 0x0600165C RID: 5724 RVA: 0x00066BE0 File Offset: 0x00064DE0
			public void Reset()
			{
				this.m_Index = -1;
			}

			// Token: 0x17000602 RID: 1538
			// (get) Token: 0x0600165D RID: 5725 RVA: 0x00066BE9 File Offset: 0x00064DE9
			public TValue Current
			{
				get
				{
					return this.m_List[this.m_Index];
				}
			}

			// Token: 0x17000603 RID: 1539
			// (get) Token: 0x0600165E RID: 5726 RVA: 0x00066BFC File Offset: 0x00064DFC
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x0600165F RID: 5727 RVA: 0x00066C09 File Offset: 0x00064E09
			public void Dispose()
			{
			}

			// Token: 0x04000C9C RID: 3228
			internal int m_Index = -1;

			// Token: 0x04000C9D RID: 3229
			internal OneOrMore<TValue, TList> m_List;
		}
	}
}
