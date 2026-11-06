using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000152 RID: 338
	internal class AncestorFilter
	{
		// Token: 0x06000AF3 RID: 2803 RVA: 0x0002C27F File Offset: 0x0002A47F
		private void AddHash(int hash)
		{
			this.m_HashStack.Push(hash);
			this.m_CountingBloomFilter.InsertHash((uint)hash);
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0002C29C File Offset: 0x0002A49C
		public unsafe bool IsCandidate(StyleComplexSelector complexSel)
		{
			int i = 0;
			while (i < 4)
			{
				bool flag = *((ref complexSel.ancestorHashes.hashes.FixedElementField) + (IntPtr)i * 4) == 0;
				bool flag2;
				if (flag)
				{
					flag2 = true;
				}
				else
				{
					bool flag3 = !this.m_CountingBloomFilter.ContainsHash((uint)(*((ref complexSel.ancestorHashes.hashes.FixedElementField) + (IntPtr)i * 4)));
					if (!flag3)
					{
						i++;
						continue;
					}
					flag2 = false;
				}
				return flag2;
			}
			return true;
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0002C310 File Offset: 0x0002A510
		public void PushElement(VisualElement element)
		{
			int count = this.m_HashStack.Count;
			this.AddHash(element.typeName.GetHashCode() * 13);
			bool flag = !string.IsNullOrEmpty(element.name);
			if (flag)
			{
				this.AddHash(element.name.GetHashCode() * 17);
			}
			foreach (string text in element.classList)
			{
				this.AddHash(text.GetHashCode() * 19);
			}
			this.m_HashStack.Push(this.m_HashStack.Count - count);
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0002C3D4 File Offset: 0x0002A5D4
		public void PopElement()
		{
			int i = this.m_HashStack.Peek();
			this.m_HashStack.Pop();
			while (i > 0)
			{
				int num = this.m_HashStack.Peek();
				this.m_CountingBloomFilter.RemoveHash((uint)num);
				this.m_HashStack.Pop();
				i--;
			}
		}

		// Token: 0x0400053B RID: 1339
		private CountingBloomFilter m_CountingBloomFilter;

		// Token: 0x0400053C RID: 1340
		private Stack<int> m_HashStack = new Stack<int>(100);
	}
}
