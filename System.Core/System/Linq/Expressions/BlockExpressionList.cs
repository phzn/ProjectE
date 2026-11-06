using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic.Utils;

namespace System.Linq.Expressions
{
	// Token: 0x02000238 RID: 568
	internal class BlockExpressionList : IList<Expression>, ICollection<Expression>, IEnumerable<Expression>, IEnumerable
	{
		// Token: 0x06000F8D RID: 3981 RVA: 0x00035454 File Offset: 0x00033654
		internal BlockExpressionList(BlockExpression provider, Expression arg0)
		{
			this._block = provider;
			this._arg0 = arg0;
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x0003546C File Offset: 0x0003366C
		public int IndexOf(Expression item)
		{
			if (this._arg0 == item)
			{
				return 0;
			}
			for (int i = 1; i < this._block.ExpressionCount; i++)
			{
				if (this._block.GetExpression(i) == item)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x00034D18 File Offset: 0x00032F18
		[ExcludeFromCodeCoverage]
		public void Insert(int index, Expression item)
		{
			throw ContractUtils.Unreachable;
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x00034D18 File Offset: 0x00032F18
		[ExcludeFromCodeCoverage]
		public void RemoveAt(int index)
		{
			throw ContractUtils.Unreachable;
		}

		// Token: 0x17000287 RID: 647
		public Expression this[int index]
		{
			get
			{
				if (index == 0)
				{
					return this._arg0;
				}
				return this._block.GetExpression(index);
			}
			[ExcludeFromCodeCoverage]
			set
			{
				throw ContractUtils.Unreachable;
			}
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x00034D18 File Offset: 0x00032F18
		[ExcludeFromCodeCoverage]
		public void Add(Expression item)
		{
			throw ContractUtils.Unreachable;
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x00034D18 File Offset: 0x00032F18
		[ExcludeFromCodeCoverage]
		public void Clear()
		{
			throw ContractUtils.Unreachable;
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x000354C4 File Offset: 0x000336C4
		public bool Contains(Expression item)
		{
			return this.IndexOf(item) != -1;
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x000354D4 File Offset: 0x000336D4
		public void CopyTo(Expression[] array, int index)
		{
			ContractUtils.RequiresNotNull(array, "array");
			if (index < 0)
			{
				throw Error.ArgumentOutOfRange("index");
			}
			int expressionCount = this._block.ExpressionCount;
			if (index + expressionCount > array.Length)
			{
				throw new ArgumentException();
			}
			array[index++] = this._arg0;
			for (int i = 1; i < expressionCount; i++)
			{
				array[index++] = this._block.GetExpression(i);
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000F97 RID: 3991 RVA: 0x00035543 File Offset: 0x00033743
		public int Count
		{
			get
			{
				return this._block.ExpressionCount;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000F98 RID: 3992 RVA: 0x00034D18 File Offset: 0x00032F18
		[ExcludeFromCodeCoverage]
		public bool IsReadOnly
		{
			get
			{
				throw ContractUtils.Unreachable;
			}
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x00034D18 File Offset: 0x00032F18
		[ExcludeFromCodeCoverage]
		public bool Remove(Expression item)
		{
			throw ContractUtils.Unreachable;
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x00035550 File Offset: 0x00033750
		public IEnumerator<Expression> GetEnumerator()
		{
			yield return this._arg0;
			int num;
			for (int i = 1; i < this._block.ExpressionCount; i = num + 1)
			{
				yield return this._block.GetExpression(i);
				num = i;
			}
			yield break;
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x0003555F File Offset: 0x0003375F
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000954 RID: 2388
		private readonly BlockExpression _block;

		// Token: 0x04000955 RID: 2389
		private readonly Expression _arg0;
	}
}
