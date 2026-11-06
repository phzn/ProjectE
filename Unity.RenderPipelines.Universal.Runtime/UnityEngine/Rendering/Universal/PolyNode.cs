using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200000D RID: 13
	internal class PolyNode
	{
		// Token: 0x0600004E RID: 78 RVA: 0x00002FD4 File Offset: 0x000011D4
		private bool IsHoleNode()
		{
			bool flag = true;
			for (PolyNode polyNode = this.m_Parent; polyNode != null; polyNode = polyNode.m_Parent)
			{
				flag = !flag;
			}
			return flag;
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002FFC File Offset: 0x000011FC
		public int ChildCount
		{
			get
			{
				return this.m_Childs.Count;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00003009 File Offset: 0x00001209
		public List<IntPoint> Contour
		{
			get
			{
				return this.m_polygon;
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003014 File Offset: 0x00001214
		internal void AddChild(PolyNode Child)
		{
			int count = this.m_Childs.Count;
			this.m_Childs.Add(Child);
			Child.m_Parent = this;
			Child.m_Index = count;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003047 File Offset: 0x00001247
		public PolyNode GetNext()
		{
			if (this.m_Childs.Count > 0)
			{
				return this.m_Childs[0];
			}
			return this.GetNextSiblingUp();
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000306C File Offset: 0x0000126C
		internal PolyNode GetNextSiblingUp()
		{
			if (this.m_Parent == null)
			{
				return null;
			}
			if (this.m_Index == this.m_Parent.m_Childs.Count - 1)
			{
				return this.m_Parent.GetNextSiblingUp();
			}
			return this.m_Parent.m_Childs[this.m_Index + 1];
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000054 RID: 84 RVA: 0x000030C1 File Offset: 0x000012C1
		public List<PolyNode> Childs
		{
			get
			{
				return this.m_Childs;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000055 RID: 85 RVA: 0x000030C9 File Offset: 0x000012C9
		public PolyNode Parent
		{
			get
			{
				return this.m_Parent;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000056 RID: 86 RVA: 0x000030D1 File Offset: 0x000012D1
		public bool IsHole
		{
			get
			{
				return this.IsHoleNode();
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000057 RID: 87 RVA: 0x000030D9 File Offset: 0x000012D9
		// (set) Token: 0x06000058 RID: 88 RVA: 0x000030E1 File Offset: 0x000012E1
		public bool IsOpen { get; set; }

		// Token: 0x04000026 RID: 38
		internal PolyNode m_Parent;

		// Token: 0x04000027 RID: 39
		internal List<IntPoint> m_polygon = new List<IntPoint>();

		// Token: 0x04000028 RID: 40
		internal int m_Index;

		// Token: 0x04000029 RID: 41
		internal JoinType m_jointype;

		// Token: 0x0400002A RID: 42
		internal EndType m_endtype;

		// Token: 0x0400002B RID: 43
		internal List<PolyNode> m_Childs = new List<PolyNode>();
	}
}
