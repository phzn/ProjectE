using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000023 RID: 35
	public struct HighlightState
	{
		// Token: 0x06000131 RID: 305 RVA: 0x00017648 File Offset: 0x00015848
		public HighlightState(Color32 color, TMP_Offset padding)
		{
			this.color = color;
			this.padding = padding;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00017658 File Offset: 0x00015858
		public static bool operator ==(HighlightState lhs, HighlightState rhs)
		{
			return lhs.color.Compare(rhs.color) && lhs.padding == rhs.padding;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00017680 File Offset: 0x00015880
		public static bool operator !=(HighlightState lhs, HighlightState rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0001768C File Offset: 0x0001588C
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0001769E File Offset: 0x0001589E
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000176B1 File Offset: 0x000158B1
		public bool Equals(HighlightState other)
		{
			return base.Equals(other);
		}

		// Token: 0x04000116 RID: 278
		public Color32 color;

		// Token: 0x04000117 RID: 279
		public TMP_Offset padding;
	}
}
