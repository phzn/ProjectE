using System;

namespace System.Collections.Generic
{
	// Token: 0x02000ACE RID: 2766
	[Serializable]
	internal sealed class InternalStringComparer : EqualityComparer<string>
	{
		// Token: 0x060062C1 RID: 25281 RVA: 0x0014A51C File Offset: 0x0014871C
		public override int GetHashCode(string obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetHashCode();
		}

		// Token: 0x060062C2 RID: 25282 RVA: 0x0014A529 File Offset: 0x00148729
		public override bool Equals(string x, string y)
		{
			if (x == null)
			{
				return y == null;
			}
			return x == y || x.Equals(y);
		}

		// Token: 0x060062C3 RID: 25283 RVA: 0x0014A540 File Offset: 0x00148740
		internal override int IndexOf(string[] array, string value, int startIndex, int count)
		{
			int num = startIndex + count;
			for (int i = startIndex; i < num; i++)
			{
				if (Array.UnsafeLoad<string>(array, i) == value)
				{
					return i;
				}
			}
			return -1;
		}
	}
}
