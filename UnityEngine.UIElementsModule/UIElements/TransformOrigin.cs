using System;
using System.Globalization;

namespace UnityEngine.UIElements
{
	// Token: 0x02000312 RID: 786
	public struct TransformOrigin : IEquatable<TransformOrigin>
	{
		// Token: 0x06001B24 RID: 6948 RVA: 0x0006ABE0 File Offset: 0x00068DE0
		public TransformOrigin(Length x, Length y, float z)
		{
			this.m_X = x;
			this.m_Y = y;
			this.m_Z = z;
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x0006ABF8 File Offset: 0x00068DF8
		public TransformOrigin(Length x, Length y)
		{
			this = new TransformOrigin(x, y, 0f);
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x0006AC0C File Offset: 0x00068E0C
		public static TransformOrigin Initial()
		{
			return new TransformOrigin(Length.Percent(50f), Length.Percent(50f), 0f);
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06001B27 RID: 6951 RVA: 0x0006AC3C File Offset: 0x00068E3C
		// (set) Token: 0x06001B28 RID: 6952 RVA: 0x0006AC44 File Offset: 0x00068E44
		public Length x
		{
			get
			{
				return this.m_X;
			}
			set
			{
				this.m_X = value;
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06001B29 RID: 6953 RVA: 0x0006AC4D File Offset: 0x00068E4D
		// (set) Token: 0x06001B2A RID: 6954 RVA: 0x0006AC55 File Offset: 0x00068E55
		public Length y
		{
			get
			{
				return this.m_Y;
			}
			set
			{
				this.m_Y = value;
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06001B2B RID: 6955 RVA: 0x0006AC5E File Offset: 0x00068E5E
		// (set) Token: 0x06001B2C RID: 6956 RVA: 0x0006AC66 File Offset: 0x00068E66
		public float z
		{
			get
			{
				return this.m_Z;
			}
			set
			{
				this.m_Z = value;
			}
		}

		// Token: 0x06001B2D RID: 6957 RVA: 0x0006AC70 File Offset: 0x00068E70
		public static bool operator ==(TransformOrigin lhs, TransformOrigin rhs)
		{
			return lhs.m_X == rhs.m_X && lhs.m_Y == rhs.m_Y && lhs.m_Z == rhs.m_Z;
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x0006ACBC File Offset: 0x00068EBC
		public static bool operator !=(TransformOrigin lhs, TransformOrigin rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x0006ACD8 File Offset: 0x00068ED8
		public bool Equals(TransformOrigin other)
		{
			return other == this;
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x0006ACF8 File Offset: 0x00068EF8
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is TransformOrigin)
			{
				TransformOrigin transformOrigin = (TransformOrigin)obj;
				flag = this.Equals(transformOrigin);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x0006AD24 File Offset: 0x00068F24
		public override int GetHashCode()
		{
			return (this.m_X.GetHashCode() * 793) ^ (this.m_Y.GetHashCode() * 791) ^ (this.m_Z.GetHashCode() * 571);
		}

		// Token: 0x06001B32 RID: 6962 RVA: 0x0006AD78 File Offset: 0x00068F78
		public override string ToString()
		{
			string text = this.m_Z.ToString(CultureInfo.InvariantCulture.NumberFormat);
			return string.Concat(new string[]
			{
				this.m_X.ToString(),
				" ",
				this.m_Y.ToString(),
				" ",
				text
			});
		}

		// Token: 0x04000B0B RID: 2827
		private Length m_X;

		// Token: 0x04000B0C RID: 2828
		private Length m_Y;

		// Token: 0x04000B0D RID: 2829
		private float m_Z;
	}
}
