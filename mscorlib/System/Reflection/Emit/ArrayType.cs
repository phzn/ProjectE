using System;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Reflection.Emit
{
	// Token: 0x02000919 RID: 2329
	[StructLayout(LayoutKind.Sequential)]
	internal class ArrayType : SymbolType
	{
		// Token: 0x06004F50 RID: 20304 RVA: 0x000F98F6 File Offset: 0x000F7AF6
		internal ArrayType(Type elementType, int rank)
			: base(elementType)
		{
			this.rank = rank;
		}

		// Token: 0x06004F51 RID: 20305 RVA: 0x000F9906 File Offset: 0x000F7B06
		internal int GetEffectiveRank()
		{
			return this.rank;
		}

		// Token: 0x06004F52 RID: 20306 RVA: 0x000F9910 File Offset: 0x000F7B10
		internal override Type InternalResolve()
		{
			Type type = this.m_baseType.InternalResolve();
			if (this.rank == 0)
			{
				return type.MakeArrayType();
			}
			return type.MakeArrayType(this.rank);
		}

		// Token: 0x06004F53 RID: 20307 RVA: 0x000F9944 File Offset: 0x000F7B44
		internal override Type RuntimeResolve()
		{
			Type type = this.m_baseType.RuntimeResolve();
			if (this.rank == 0)
			{
				return type.MakeArrayType();
			}
			return type.MakeArrayType(this.rank);
		}

		// Token: 0x06004F54 RID: 20308 RVA: 0x000040F7 File Offset: 0x000022F7
		protected override bool IsArrayImpl()
		{
			return true;
		}

		// Token: 0x06004F55 RID: 20309 RVA: 0x000F9978 File Offset: 0x000F7B78
		public override int GetArrayRank()
		{
			if (this.rank != 0)
			{
				return this.rank;
			}
			return 1;
		}

		// Token: 0x06004F56 RID: 20310 RVA: 0x000F998C File Offset: 0x000F7B8C
		internal override string FormatName(string elementName)
		{
			if (elementName == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder(elementName);
			stringBuilder.Append("[");
			for (int i = 1; i < this.rank; i++)
			{
				stringBuilder.Append(",");
			}
			if (this.rank == 1)
			{
				stringBuilder.Append("*");
			}
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}

		// Token: 0x04003134 RID: 12596
		private int rank;
	}
}
