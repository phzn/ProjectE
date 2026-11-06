using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200000C RID: 12
	internal class PolyTree : PolyNode
	{
		// Token: 0x0600004A RID: 74 RVA: 0x00002F20 File Offset: 0x00001120
		public void Clear()
		{
			for (int i = 0; i < this.m_AllPolys.Count; i++)
			{
				this.m_AllPolys[i] = null;
			}
			this.m_AllPolys.Clear();
			this.m_Childs.Clear();
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002F66 File Offset: 0x00001166
		public PolyNode GetFirst()
		{
			if (this.m_Childs.Count > 0)
			{
				return this.m_Childs[0];
			}
			return null;
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002F84 File Offset: 0x00001184
		public int Total
		{
			get
			{
				int num = this.m_AllPolys.Count;
				if (num > 0 && this.m_Childs[0] != this.m_AllPolys[0])
				{
					num--;
				}
				return num;
			}
		}

		// Token: 0x04000025 RID: 37
		internal List<PolyNode> m_AllPolys = new List<PolyNode>();
	}
}
