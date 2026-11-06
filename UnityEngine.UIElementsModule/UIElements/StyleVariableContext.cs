using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine.UIElements
{
	// Token: 0x0200035F RID: 863
	internal class StyleVariableContext
	{
		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06001CC6 RID: 7366 RVA: 0x0006FB08 File Offset: 0x0006DD08
		public List<StyleVariable> variables
		{
			get
			{
				return this.m_Variables;
			}
		}

		// Token: 0x06001CC7 RID: 7367 RVA: 0x0006FB10 File Offset: 0x0006DD10
		public void Add(StyleVariable sv)
		{
			StyleVariableContext.<>c__DisplayClass7_0 CS$<>8__locals1;
			CS$<>8__locals1.hash = sv.GetHashCode();
			int num = this.m_SortedHash.BinarySearch(CS$<>8__locals1.hash);
			bool flag = num >= 0;
			if (flag)
			{
				int i = this.m_Variables.Count - 1;
				bool flag2 = this.m_UnsortedHash[i] == CS$<>8__locals1.hash;
				if (flag2)
				{
					return;
				}
				for (i--; i >= 0; i--)
				{
					bool flag3 = this.m_UnsortedHash[i] == CS$<>8__locals1.hash;
					if (flag3)
					{
						this.m_VariableHash ^= StyleVariableContext.<Add>g__ComputeOrderSensitiveHash|7_0(i, ref CS$<>8__locals1);
						this.m_Variables.RemoveAt(i);
						this.m_UnsortedHash.RemoveAt(i);
						break;
					}
				}
			}
			else
			{
				this.m_SortedHash.Insert(~num, CS$<>8__locals1.hash);
			}
			this.m_VariableHash ^= StyleVariableContext.<Add>g__ComputeOrderSensitiveHash|7_0(this.m_Variables.Count, ref CS$<>8__locals1);
			this.m_Variables.Add(sv);
			this.m_UnsortedHash.Add(CS$<>8__locals1.hash);
		}

		// Token: 0x06001CC8 RID: 7368 RVA: 0x0006FC3C File Offset: 0x0006DE3C
		public void AddInitialRange(StyleVariableContext other)
		{
			bool flag = other.m_Variables.Count > 0;
			if (flag)
			{
				Debug.Assert(this.m_Variables.Count == 0);
				this.m_VariableHash = other.m_VariableHash;
				this.m_Variables.AddRange(other.m_Variables);
				this.m_SortedHash.AddRange(other.m_SortedHash);
				this.m_UnsortedHash.AddRange(other.m_UnsortedHash);
			}
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x0006FCB4 File Offset: 0x0006DEB4
		public void Clear()
		{
			bool flag = this.m_Variables.Count > 0;
			if (flag)
			{
				this.m_Variables.Clear();
				this.m_VariableHash = 0;
				this.m_SortedHash.Clear();
				this.m_UnsortedHash.Clear();
			}
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x0006FD01 File Offset: 0x0006DF01
		public StyleVariableContext()
		{
			this.m_Variables = new List<StyleVariable>();
			this.m_VariableHash = 0;
			this.m_SortedHash = new List<int>();
			this.m_UnsortedHash = new List<int>();
		}

		// Token: 0x06001CCB RID: 7371 RVA: 0x0006FD34 File Offset: 0x0006DF34
		public StyleVariableContext(StyleVariableContext other)
		{
			this.m_Variables = new List<StyleVariable>(other.m_Variables);
			this.m_VariableHash = other.m_VariableHash;
			this.m_SortedHash = new List<int>(other.m_SortedHash);
			this.m_UnsortedHash = new List<int>(other.m_UnsortedHash);
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x0006FD88 File Offset: 0x0006DF88
		public bool TryFindVariable(string name, out StyleVariable v)
		{
			for (int i = this.m_Variables.Count - 1; i >= 0; i--)
			{
				bool flag = this.m_Variables[i].name == name;
				if (flag)
				{
					v = this.m_Variables[i];
					return true;
				}
			}
			v = default(StyleVariable);
			return false;
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x0006FDF8 File Offset: 0x0006DFF8
		public int GetVariableHash()
		{
			return this.m_VariableHash;
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x0006FE1C File Offset: 0x0006E01C
		[CompilerGenerated]
		internal static int <Add>g__ComputeOrderSensitiveHash|7_0(int index, ref StyleVariableContext.<>c__DisplayClass7_0 A_1)
		{
			return (index + 1) * A_1.hash;
		}

		// Token: 0x04000C18 RID: 3096
		public static readonly StyleVariableContext none = new StyleVariableContext();

		// Token: 0x04000C19 RID: 3097
		private int m_VariableHash;

		// Token: 0x04000C1A RID: 3098
		private List<StyleVariable> m_Variables;

		// Token: 0x04000C1B RID: 3099
		private List<int> m_SortedHash;

		// Token: 0x04000C1C RID: 3100
		private List<int> m_UnsortedHash;
	}
}
