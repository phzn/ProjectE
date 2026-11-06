using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000027 RID: 39
	public struct BackgroundPosition : IEquatable<BackgroundPosition>
	{
		// Token: 0x0600018A RID: 394 RVA: 0x00004693 File Offset: 0x00002893
		public BackgroundPosition(BackgroundPositionKeyword keyword)
		{
			this.keyword = keyword;
			this.offset = new Length(0f);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000046AD File Offset: 0x000028AD
		public BackgroundPosition(BackgroundPositionKeyword keyword, Length offset)
		{
			this.keyword = keyword;
			this.offset = offset;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000046C0 File Offset: 0x000028C0
		internal static BackgroundPosition Initial()
		{
			return BackgroundPropertyHelper.ConvertScaleModeToBackgroundPosition(ScaleMode.StretchToFill);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x000046D8 File Offset: 0x000028D8
		public override bool Equals(object obj)
		{
			return obj is BackgroundPosition && this.Equals((BackgroundPosition)obj);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00004704 File Offset: 0x00002904
		public bool Equals(BackgroundPosition other)
		{
			return other.offset == this.offset && other.keyword == this.keyword;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000473C File Offset: 0x0000293C
		public override int GetHashCode()
		{
			int num = 1500536833;
			num = num * -1521134295 + this.keyword.GetHashCode();
			return num * -1521134295 + this.offset.GetHashCode();
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000478C File Offset: 0x0000298C
		public static bool operator ==(BackgroundPosition style1, BackgroundPosition style2)
		{
			return style1.Equals(style2);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000047A8 File Offset: 0x000029A8
		public static bool operator !=(BackgroundPosition style1, BackgroundPosition style2)
		{
			return !(style1 == style2);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x000047C4 File Offset: 0x000029C4
		public override string ToString()
		{
			return string.Format("(type:{0} x:{1})", this.keyword, this.offset);
		}

		// Token: 0x04000074 RID: 116
		public BackgroundPositionKeyword keyword;

		// Token: 0x04000075 RID: 117
		public Length offset;
	}
}
