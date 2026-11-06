using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200002A RID: 42
	public struct BackgroundSize : IEquatable<BackgroundSize>
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600019F RID: 415 RVA: 0x00004AB0 File Offset: 0x00002CB0
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x00004AC8 File Offset: 0x00002CC8
		public BackgroundSizeType sizeType
		{
			get
			{
				return this.m_SizeType;
			}
			set
			{
				this.m_SizeType = value;
				this.m_X = new Length(0f);
				this.m_Y = new Length(0f);
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x00004AF4 File Offset: 0x00002CF4
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x00004B0C File Offset: 0x00002D0C
		public Length x
		{
			get
			{
				return this.m_X;
			}
			set
			{
				this.m_X = value;
				this.m_SizeType = BackgroundSizeType.Length;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x00004B20 File Offset: 0x00002D20
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x00004B38 File Offset: 0x00002D38
		public Length y
		{
			get
			{
				return this.m_Y;
			}
			set
			{
				this.m_Y = value;
				this.m_SizeType = BackgroundSizeType.Length;
			}
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00004B49 File Offset: 0x00002D49
		public BackgroundSize(Length sizeX, Length sizeY)
		{
			this.m_SizeType = BackgroundSizeType.Length;
			this.m_X = sizeX;
			this.m_Y = sizeY;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00004AC8 File Offset: 0x00002CC8
		public BackgroundSize(BackgroundSizeType sizeType)
		{
			this.m_SizeType = sizeType;
			this.m_X = new Length(0f);
			this.m_Y = new Length(0f);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00004B64 File Offset: 0x00002D64
		internal static BackgroundSize Initial()
		{
			return BackgroundPropertyHelper.ConvertScaleModeToBackgroundSize(ScaleMode.StretchToFill);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00004B7C File Offset: 0x00002D7C
		public override bool Equals(object obj)
		{
			return obj is BackgroundSize && this.Equals((BackgroundSize)obj);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00004BA8 File Offset: 0x00002DA8
		public bool Equals(BackgroundSize other)
		{
			return other.x == this.x && other.y == this.y && other.sizeType == this.sizeType;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00004BF4 File Offset: 0x00002DF4
		public override int GetHashCode()
		{
			int num = 1500536833;
			num = num * -1521134295 + this.m_SizeType.GetHashCode();
			num = num * -1521134295 + this.m_X.GetHashCode();
			return num * -1521134295 + this.m_Y.GetHashCode();
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00004C5C File Offset: 0x00002E5C
		public static bool operator ==(BackgroundSize style1, BackgroundSize style2)
		{
			return style1.Equals(style2);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00004C78 File Offset: 0x00002E78
		public static bool operator !=(BackgroundSize style1, BackgroundSize style2)
		{
			return !(style1 == style2);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00004C94 File Offset: 0x00002E94
		public override string ToString()
		{
			return string.Format("(sizeType:{0} x:{1}, y:{2})", this.sizeType, this.x, this.y);
		}

		// Token: 0x04000078 RID: 120
		private BackgroundSizeType m_SizeType;

		// Token: 0x04000079 RID: 121
		private Length m_X;

		// Token: 0x0400007A RID: 122
		private Length m_Y;
	}
}
