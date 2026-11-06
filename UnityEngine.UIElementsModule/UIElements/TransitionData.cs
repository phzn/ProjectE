using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020002F0 RID: 752
	internal struct TransitionData : IStyleDataGroup<TransitionData>, IEquatable<TransitionData>
	{
		// Token: 0x06001972 RID: 6514 RVA: 0x00062C00 File Offset: 0x00060E00
		public TransitionData Copy()
		{
			return new TransitionData
			{
				transitionDelay = new List<TimeValue>(this.transitionDelay),
				transitionDuration = new List<TimeValue>(this.transitionDuration),
				transitionProperty = new List<StylePropertyName>(this.transitionProperty),
				transitionTimingFunction = new List<EasingFunction>(this.transitionTimingFunction)
			};
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x00062C64 File Offset: 0x00060E64
		public void CopyFrom(ref TransitionData other)
		{
			bool flag = this.transitionDelay != other.transitionDelay;
			if (flag)
			{
				this.transitionDelay.Clear();
				this.transitionDelay.AddRange(other.transitionDelay);
			}
			bool flag2 = this.transitionDuration != other.transitionDuration;
			if (flag2)
			{
				this.transitionDuration.Clear();
				this.transitionDuration.AddRange(other.transitionDuration);
			}
			bool flag3 = this.transitionProperty != other.transitionProperty;
			if (flag3)
			{
				this.transitionProperty.Clear();
				this.transitionProperty.AddRange(other.transitionProperty);
			}
			bool flag4 = this.transitionTimingFunction != other.transitionTimingFunction;
			if (flag4)
			{
				this.transitionTimingFunction.Clear();
				this.transitionTimingFunction.AddRange(other.transitionTimingFunction);
			}
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x00062D48 File Offset: 0x00060F48
		public static bool operator ==(TransitionData lhs, TransitionData rhs)
		{
			return lhs.transitionDelay == rhs.transitionDelay && lhs.transitionDuration == rhs.transitionDuration && lhs.transitionProperty == rhs.transitionProperty && lhs.transitionTimingFunction == rhs.transitionTimingFunction;
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x00062D98 File Offset: 0x00060F98
		public static bool operator !=(TransitionData lhs, TransitionData rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x00062DB4 File Offset: 0x00060FB4
		public bool Equals(TransitionData other)
		{
			return other == this;
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x00062DD4 File Offset: 0x00060FD4
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is TransitionData && this.Equals((TransitionData)obj);
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x00062E0C File Offset: 0x0006100C
		public override int GetHashCode()
		{
			int num = this.transitionDelay.GetHashCode();
			num = (num * 397) ^ this.transitionDuration.GetHashCode();
			num = (num * 397) ^ this.transitionProperty.GetHashCode();
			return (num * 397) ^ this.transitionTimingFunction.GetHashCode();
		}

		// Token: 0x04000AAE RID: 2734
		public List<TimeValue> transitionDelay;

		// Token: 0x04000AAF RID: 2735
		public List<TimeValue> transitionDuration;

		// Token: 0x04000AB0 RID: 2736
		public List<StylePropertyName> transitionProperty;

		// Token: 0x04000AB1 RID: 2737
		public List<EasingFunction> transitionTimingFunction;
	}
}
