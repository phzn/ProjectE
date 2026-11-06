using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002CE RID: 718
	public struct EasingFunction : IEquatable<EasingFunction>
	{
		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06001565 RID: 5477 RVA: 0x00054FE0 File Offset: 0x000531E0
		// (set) Token: 0x06001566 RID: 5478 RVA: 0x00054FE8 File Offset: 0x000531E8
		public EasingMode mode
		{
			get
			{
				return this.m_Mode;
			}
			set
			{
				this.m_Mode = value;
			}
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x00054FF1 File Offset: 0x000531F1
		public EasingFunction(EasingMode mode)
		{
			this.m_Mode = mode;
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x00054FFC File Offset: 0x000531FC
		public static implicit operator EasingFunction(EasingMode easingMode)
		{
			return new EasingFunction(easingMode);
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x00055014 File Offset: 0x00053214
		public static bool operator ==(EasingFunction lhs, EasingFunction rhs)
		{
			return lhs.m_Mode == rhs.m_Mode;
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x00055034 File Offset: 0x00053234
		public static bool operator !=(EasingFunction lhs, EasingFunction rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x00055050 File Offset: 0x00053250
		public bool Equals(EasingFunction other)
		{
			return other == this;
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x00055070 File Offset: 0x00053270
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is EasingFunction)
			{
				EasingFunction easingFunction = (EasingFunction)obj;
				flag = this.Equals(easingFunction);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x0005509C File Offset: 0x0005329C
		public override string ToString()
		{
			return this.m_Mode.ToString();
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x000550C0 File Offset: 0x000532C0
		public override int GetHashCode()
		{
			return (int)this.m_Mode;
		}

		// Token: 0x040009D5 RID: 2517
		private EasingMode m_Mode;
	}
}
