using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000376 RID: 886
	public struct TextShadow : IEquatable<TextShadow>
	{
		// Token: 0x06001DF4 RID: 7668 RVA: 0x00074134 File Offset: 0x00072334
		public override bool Equals(object obj)
		{
			return obj is TextShadow && this.Equals((TextShadow)obj);
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x00074160 File Offset: 0x00072360
		public bool Equals(TextShadow other)
		{
			return other.offset == this.offset && other.blurRadius == this.blurRadius && other.color == this.color;
		}

		// Token: 0x06001DF6 RID: 7670 RVA: 0x000741A8 File Offset: 0x000723A8
		public override int GetHashCode()
		{
			int num = 1500536833;
			num = num * -1521134295 + this.offset.GetHashCode();
			num = num * -1521134295 + this.blurRadius.GetHashCode();
			return num * -1521134295 + this.color.GetHashCode();
		}

		// Token: 0x06001DF7 RID: 7671 RVA: 0x0007420C File Offset: 0x0007240C
		public static bool operator ==(TextShadow style1, TextShadow style2)
		{
			return style1.Equals(style2);
		}

		// Token: 0x06001DF8 RID: 7672 RVA: 0x00074228 File Offset: 0x00072428
		public static bool operator !=(TextShadow style1, TextShadow style2)
		{
			return !(style1 == style2);
		}

		// Token: 0x06001DF9 RID: 7673 RVA: 0x00074244 File Offset: 0x00072444
		public override string ToString()
		{
			return string.Format("offset={0}, blurRadius={1}, color={2}", this.offset, this.blurRadius, this.color);
		}

		// Token: 0x06001DFA RID: 7674 RVA: 0x00074284 File Offset: 0x00072484
		internal static TextShadow LerpUnclamped(TextShadow a, TextShadow b, float t)
		{
			return new TextShadow
			{
				offset = Vector2.LerpUnclamped(a.offset, b.offset, t),
				blurRadius = Mathf.LerpUnclamped(a.blurRadius, b.blurRadius, t),
				color = Color.LerpUnclamped(a.color, b.color, t)
			};
		}

		// Token: 0x04000C7F RID: 3199
		public Vector2 offset;

		// Token: 0x04000C80 RID: 3200
		public float blurRadius;

		// Token: 0x04000C81 RID: 3201
		public Color color;
	}
}
