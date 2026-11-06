using System;

namespace UnityEngine.Timeline
{
	// Token: 0x0200001C RID: 28
	internal struct DiscreteTime : IComparable
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x0000749C File Offset: 0x0000569C
		public static double tickValue
		{
			get
			{
				return 1E-12;
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000074A7 File Offset: 0x000056A7
		public DiscreteTime(DiscreteTime time)
		{
			this.m_DiscreteTime = time.m_DiscreteTime;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x000074B5 File Offset: 0x000056B5
		private DiscreteTime(long time)
		{
			this.m_DiscreteTime = time;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x000074BE File Offset: 0x000056BE
		public DiscreteTime(double time)
		{
			this.m_DiscreteTime = DiscreteTime.DoubleToDiscreteTime(time);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x000074CC File Offset: 0x000056CC
		public DiscreteTime(float time)
		{
			this.m_DiscreteTime = DiscreteTime.FloatToDiscreteTime(time);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000074DA File Offset: 0x000056DA
		public DiscreteTime(int time)
		{
			this.m_DiscreteTime = DiscreteTime.IntToDiscreteTime(time);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000074E8 File Offset: 0x000056E8
		public DiscreteTime(int frame, double fps)
		{
			this.m_DiscreteTime = DiscreteTime.DoubleToDiscreteTime((double)frame * fps);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x000074F9 File Offset: 0x000056F9
		public DiscreteTime OneTickBefore()
		{
			return new DiscreteTime(this.m_DiscreteTime - 1L);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00007509 File Offset: 0x00005709
		public DiscreteTime OneTickAfter()
		{
			return new DiscreteTime(this.m_DiscreteTime + 1L);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00007519 File Offset: 0x00005719
		public long GetTick()
		{
			return this.m_DiscreteTime;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00007521 File Offset: 0x00005721
		public static DiscreteTime FromTicks(long ticks)
		{
			return new DiscreteTime(ticks);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00007529 File Offset: 0x00005729
		public int CompareTo(object obj)
		{
			if (obj is DiscreteTime)
			{
				return this.m_DiscreteTime.CompareTo(((DiscreteTime)obj).m_DiscreteTime);
			}
			return 1;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000754B File Offset: 0x0000574B
		public bool Equals(DiscreteTime other)
		{
			return this.m_DiscreteTime == other.m_DiscreteTime;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000755B File Offset: 0x0000575B
		public override bool Equals(object obj)
		{
			return obj is DiscreteTime && this.Equals((DiscreteTime)obj);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00007574 File Offset: 0x00005774
		private static long DoubleToDiscreteTime(double time)
		{
			double num = time / 1E-12 + 0.5;
			if (num < 9.223372036854776E+18 && num > -9.223372036854776E+18)
			{
				return (long)num;
			}
			throw new ArgumentOutOfRangeException("Time is over the discrete range.");
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x000075BC File Offset: 0x000057BC
		private static long FloatToDiscreteTime(float time)
		{
			float num = time / 1E-12f + 0.5f;
			if (num < 9.223372E+18f && num > -9.223372E+18f)
			{
				return (long)num;
			}
			throw new ArgumentOutOfRangeException("Time is over the discrete range.");
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x000075F4 File Offset: 0x000057F4
		private static long IntToDiscreteTime(int time)
		{
			return DiscreteTime.DoubleToDiscreteTime((double)time);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x000075FD File Offset: 0x000057FD
		private static double ToDouble(long time)
		{
			return (double)time * 1E-12;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000760B File Offset: 0x0000580B
		private static float ToFloat(long time)
		{
			return (float)DiscreteTime.ToDouble(time);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00007614 File Offset: 0x00005814
		public static explicit operator double(DiscreteTime b)
		{
			return DiscreteTime.ToDouble(b.m_DiscreteTime);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00007621 File Offset: 0x00005821
		public static explicit operator float(DiscreteTime b)
		{
			return DiscreteTime.ToFloat(b.m_DiscreteTime);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000762E File Offset: 0x0000582E
		public static explicit operator long(DiscreteTime b)
		{
			return b.m_DiscreteTime;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00007636 File Offset: 0x00005836
		public static explicit operator DiscreteTime(double time)
		{
			return new DiscreteTime(time);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000763E File Offset: 0x0000583E
		public static explicit operator DiscreteTime(float time)
		{
			return new DiscreteTime(time);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00007646 File Offset: 0x00005846
		public static implicit operator DiscreteTime(int time)
		{
			return new DiscreteTime(time);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000764E File Offset: 0x0000584E
		public static explicit operator DiscreteTime(long time)
		{
			return new DiscreteTime(time);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00007656 File Offset: 0x00005856
		public static bool operator ==(DiscreteTime lhs, DiscreteTime rhs)
		{
			return lhs.m_DiscreteTime == rhs.m_DiscreteTime;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00007666 File Offset: 0x00005866
		public static bool operator !=(DiscreteTime lhs, DiscreteTime rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00007672 File Offset: 0x00005872
		public static bool operator >(DiscreteTime lhs, DiscreteTime rhs)
		{
			return lhs.m_DiscreteTime > rhs.m_DiscreteTime;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00007682 File Offset: 0x00005882
		public static bool operator <(DiscreteTime lhs, DiscreteTime rhs)
		{
			return lhs.m_DiscreteTime < rhs.m_DiscreteTime;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00007692 File Offset: 0x00005892
		public static bool operator <=(DiscreteTime lhs, DiscreteTime rhs)
		{
			return lhs.m_DiscreteTime <= rhs.m_DiscreteTime;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x000076A5 File Offset: 0x000058A5
		public static bool operator >=(DiscreteTime lhs, DiscreteTime rhs)
		{
			return lhs.m_DiscreteTime >= rhs.m_DiscreteTime;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x000076B8 File Offset: 0x000058B8
		public static DiscreteTime operator +(DiscreteTime lhs, DiscreteTime rhs)
		{
			return new DiscreteTime(lhs.m_DiscreteTime + rhs.m_DiscreteTime);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x000076CC File Offset: 0x000058CC
		public static DiscreteTime operator -(DiscreteTime lhs, DiscreteTime rhs)
		{
			return new DiscreteTime(lhs.m_DiscreteTime - rhs.m_DiscreteTime);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x000076E0 File Offset: 0x000058E0
		public override string ToString()
		{
			return this.m_DiscreteTime.ToString();
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x000076ED File Offset: 0x000058ED
		public override int GetHashCode()
		{
			return this.m_DiscreteTime.GetHashCode();
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000076FA File Offset: 0x000058FA
		public static DiscreteTime Min(DiscreteTime lhs, DiscreteTime rhs)
		{
			return new DiscreteTime(Math.Min(lhs.m_DiscreteTime, rhs.m_DiscreteTime));
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00007712 File Offset: 0x00005912
		public static DiscreteTime Max(DiscreteTime lhs, DiscreteTime rhs)
		{
			return new DiscreteTime(Math.Max(lhs.m_DiscreteTime, rhs.m_DiscreteTime));
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000772A File Offset: 0x0000592A
		public static double SnapToNearestTick(double time)
		{
			return DiscreteTime.ToDouble(DiscreteTime.DoubleToDiscreteTime(time));
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00007737 File Offset: 0x00005937
		public static float SnapToNearestTick(float time)
		{
			return DiscreteTime.ToFloat(DiscreteTime.FloatToDiscreteTime(time));
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00007744 File Offset: 0x00005944
		public static long GetNearestTick(double time)
		{
			return DiscreteTime.DoubleToDiscreteTime(time);
		}

		// Token: 0x040000AC RID: 172
		private const double k_Tick = 1E-12;

		// Token: 0x040000AD RID: 173
		public static readonly DiscreteTime kMaxTime = new DiscreteTime(long.MaxValue);

		// Token: 0x040000AE RID: 174
		private readonly long m_DiscreteTime;
	}
}
