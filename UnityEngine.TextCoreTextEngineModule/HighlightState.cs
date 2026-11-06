using System;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x02000038 RID: 56
	internal struct HighlightState
	{
		// Token: 0x06000182 RID: 386 RVA: 0x0001DAB8 File Offset: 0x0001BCB8
		public HighlightState(Color32 color, Offset padding)
		{
			this.color = color;
			this.padding = padding;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0001DACC File Offset: 0x0001BCCC
		public static bool operator ==(HighlightState lhs, HighlightState rhs)
		{
			return lhs.color.r == rhs.color.r && lhs.color.g == rhs.color.g && lhs.color.b == rhs.color.b && lhs.color.a == rhs.color.a && lhs.padding == rhs.padding;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0001DB54 File Offset: 0x0001BD54
		public static bool operator !=(HighlightState lhs, HighlightState rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0001DB70 File Offset: 0x0001BD70
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0001DB94 File Offset: 0x0001BD94
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0001DBB8 File Offset: 0x0001BDB8
		public bool Equals(HighlightState other)
		{
			return base.Equals(other);
		}

		// Token: 0x04000267 RID: 615
		public Color32 color;

		// Token: 0x04000268 RID: 616
		public Offset padding;
	}
}
