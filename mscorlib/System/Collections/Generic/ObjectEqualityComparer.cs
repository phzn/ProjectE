using System;

namespace System.Collections.Generic
{
	// Token: 0x02000AC8 RID: 2760
	[Serializable]
	internal class ObjectEqualityComparer<T> : EqualityComparer<T>
	{
		// Token: 0x0600629F RID: 25247 RVA: 0x0014A20E File Offset: 0x0014840E
		public override bool Equals(T x, T y)
		{
			if (x != null)
			{
				return y != null && x.Equals(y);
			}
			return y == null;
		}

		// Token: 0x060062A0 RID: 25248 RVA: 0x00149FBA File Offset: 0x001481BA
		public override int GetHashCode(T obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetHashCode();
		}

		// Token: 0x060062A1 RID: 25249 RVA: 0x0014A244 File Offset: 0x00148444
		internal override int IndexOf(T[] array, T value, int startIndex, int count)
		{
			int num = startIndex + count;
			if (value == null)
			{
				for (int i = startIndex; i < num; i++)
				{
					if (array[i] == null)
					{
						return i;
					}
				}
			}
			else
			{
				for (int j = startIndex; j < num; j++)
				{
					if (array[j] != null && array[j].Equals(value))
					{
						return j;
					}
				}
			}
			return -1;
		}

		// Token: 0x060062A2 RID: 25250 RVA: 0x0014A2B8 File Offset: 0x001484B8
		internal override int LastIndexOf(T[] array, T value, int startIndex, int count)
		{
			int num = startIndex - count + 1;
			if (value == null)
			{
				for (int i = startIndex; i >= num; i--)
				{
					if (array[i] == null)
					{
						return i;
					}
				}
			}
			else
			{
				for (int j = startIndex; j >= num; j--)
				{
					if (array[j] != null && array[j].Equals(value))
					{
						return j;
					}
				}
			}
			return -1;
		}

		// Token: 0x060062A3 RID: 25251 RVA: 0x0014A32B File Offset: 0x0014852B
		public override bool Equals(object obj)
		{
			return obj is ObjectEqualityComparer<T>;
		}

		// Token: 0x060062A4 RID: 25252 RVA: 0x00149C92 File Offset: 0x00147E92
		public override int GetHashCode()
		{
			return base.GetType().Name.GetHashCode();
		}
	}
}
