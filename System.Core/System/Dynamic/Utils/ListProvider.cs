using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace System.Dynamic.Utils
{
	// Token: 0x0200032B RID: 811
	internal abstract class ListProvider<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable where T : class
	{
		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001875 RID: 6261
		protected abstract T First { get; }

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001876 RID: 6262
		protected abstract int ElementCount { get; }

		// Token: 0x06001877 RID: 6263
		protected abstract T GetElement(int index);

		// Token: 0x06001878 RID: 6264 RVA: 0x0005297C File Offset: 0x00050B7C
		public int IndexOf(T item)
		{
			if (this.First == item)
			{
				return 0;
			}
			int i = 1;
			int elementCount = this.ElementCount;
			while (i < elementCount)
			{
				if (this.GetElement(i) == item)
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x00034D18 File Offset: 0x00032F18
		[ExcludeFromCodeCoverage]
		public void Insert(int index, T item)
		{
			throw ContractUtils.Unreachable;
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x00034D18 File Offset: 0x00032F18
		[ExcludeFromCodeCoverage]
		public void RemoveAt(int index)
		{
			throw ContractUtils.Unreachable;
		}

		// Token: 0x1700043F RID: 1087
		public T this[int index]
		{
			get
			{
				if (index == 0)
				{
					return this.First;
				}
				return this.GetElement(index);
			}
			[ExcludeFromCodeCoverage]
			set
			{
				throw ContractUtils.Unreachable;
			}
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x00034D18 File Offset: 0x00032F18
		[ExcludeFromCodeCoverage]
		public void Add(T item)
		{
			throw ContractUtils.Unreachable;
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x00034D18 File Offset: 0x00032F18
		[ExcludeFromCodeCoverage]
		public void Clear()
		{
			throw ContractUtils.Unreachable;
		}

		// Token: 0x0600187F RID: 6271 RVA: 0x000529DB File Offset: 0x00050BDB
		public bool Contains(T item)
		{
			return this.IndexOf(item) != -1;
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x000529EC File Offset: 0x00050BEC
		public void CopyTo(T[] array, int index)
		{
			ContractUtils.RequiresNotNull(array, "array");
			if (index < 0)
			{
				throw Error.ArgumentOutOfRange("index");
			}
			int elementCount = this.ElementCount;
			if (index + elementCount > array.Length)
			{
				throw new ArgumentException();
			}
			array[index++] = this.First;
			for (int i = 1; i < elementCount; i++)
			{
				array[index++] = this.GetElement(i);
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06001881 RID: 6273 RVA: 0x00052A59 File Offset: 0x00050C59
		public int Count
		{
			get
			{
				return this.ElementCount;
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06001882 RID: 6274 RVA: 0x00007E1D File Offset: 0x0000601D
		[ExcludeFromCodeCoverage]
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x00034D18 File Offset: 0x00032F18
		[ExcludeFromCodeCoverage]
		public bool Remove(T item)
		{
			throw ContractUtils.Unreachable;
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x00052A61 File Offset: 0x00050C61
		public IEnumerator<T> GetEnumerator()
		{
			yield return this.First;
			int i = 1;
			int j = this.ElementCount;
			while (i < j)
			{
				yield return this.GetElement(i);
				int num = i;
				i = num + 1;
			}
			yield break;
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x00052A70 File Offset: 0x00050C70
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
