using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq.Expressions.Compiler
{
	// Token: 0x020002B1 RID: 689
	internal sealed class ParameterList : IReadOnlyList<ParameterExpression>, IReadOnlyCollection<ParameterExpression>, IEnumerable<ParameterExpression>, IEnumerable
	{
		// Token: 0x06001470 RID: 5232 RVA: 0x0003F180 File Offset: 0x0003D380
		public ParameterList(IParameterProvider provider)
		{
			this._provider = provider;
		}

		// Token: 0x170003B4 RID: 948
		public ParameterExpression this[int index]
		{
			get
			{
				return this._provider.GetParameter(index);
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06001472 RID: 5234 RVA: 0x0003F19D File Offset: 0x0003D39D
		public int Count
		{
			get
			{
				return this._provider.ParameterCount;
			}
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x0003F1AA File Offset: 0x0003D3AA
		public IEnumerator<ParameterExpression> GetEnumerator()
		{
			int i = 0;
			int j = this._provider.ParameterCount;
			while (i < j)
			{
				yield return this._provider.GetParameter(i);
				int num = i;
				i = num + 1;
			}
			yield break;
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x0003F1B9 File Offset: 0x0003D3B9
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000AB2 RID: 2738
		private readonly IParameterProvider _provider;
	}
}
