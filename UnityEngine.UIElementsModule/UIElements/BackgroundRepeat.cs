using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000029 RID: 41
	public struct BackgroundRepeat : IEquatable<BackgroundRepeat>
	{
		// Token: 0x06000197 RID: 407 RVA: 0x00004969 File Offset: 0x00002B69
		public BackgroundRepeat(Repeat repeatX, Repeat repeatY)
		{
			this.x = repeatX;
			this.y = repeatY;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000497C File Offset: 0x00002B7C
		internal static BackgroundRepeat Initial()
		{
			return BackgroundPropertyHelper.ConvertScaleModeToBackgroundRepeat(ScaleMode.StretchToFill);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00004994 File Offset: 0x00002B94
		public override bool Equals(object obj)
		{
			return obj is BackgroundRepeat && this.Equals((BackgroundRepeat)obj);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x000049C0 File Offset: 0x00002BC0
		public bool Equals(BackgroundRepeat other)
		{
			return other.x == this.x && other.y == this.y;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x000049F4 File Offset: 0x00002BF4
		public override int GetHashCode()
		{
			int num = 1500536833;
			num = num * -1521134295 + this.x.GetHashCode();
			return num * -1521134295 + this.y.GetHashCode();
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00004A44 File Offset: 0x00002C44
		public static bool operator ==(BackgroundRepeat style1, BackgroundRepeat style2)
		{
			return style1.Equals(style2);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00004A60 File Offset: 0x00002C60
		public static bool operator !=(BackgroundRepeat style1, BackgroundRepeat style2)
		{
			return !(style1 == style2);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00004A7C File Offset: 0x00002C7C
		public override string ToString()
		{
			return string.Format("(x:{0}, y:{1})", this.x, this.y);
		}

		// Token: 0x04000076 RID: 118
		public Repeat x;

		// Token: 0x04000077 RID: 119
		public Repeat y;
	}
}
