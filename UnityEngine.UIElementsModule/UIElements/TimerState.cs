using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002B9 RID: 697
	public struct TimerState : IEquatable<TimerState>
	{
		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001430 RID: 5168 RVA: 0x00048035 File Offset: 0x00046235
		// (set) Token: 0x06001431 RID: 5169 RVA: 0x0004803D File Offset: 0x0004623D
		public long start { readonly get; set; }

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06001432 RID: 5170 RVA: 0x00048046 File Offset: 0x00046246
		// (set) Token: 0x06001433 RID: 5171 RVA: 0x0004804E File Offset: 0x0004624E
		public long now { readonly get; set; }

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06001434 RID: 5172 RVA: 0x00048058 File Offset: 0x00046258
		public long deltaTime
		{
			get
			{
				return this.now - this.start;
			}
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x00048078 File Offset: 0x00046278
		public override bool Equals(object obj)
		{
			return obj is TimerState && this.Equals((TimerState)obj);
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x000480A4 File Offset: 0x000462A4
		public bool Equals(TimerState other)
		{
			return this.start == other.start && this.now == other.now && this.deltaTime == other.deltaTime;
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x000480E8 File Offset: 0x000462E8
		public override int GetHashCode()
		{
			int num = 540054806;
			num = num * -1521134295 + this.start.GetHashCode();
			num = num * -1521134295 + this.now.GetHashCode();
			return num * -1521134295 + this.deltaTime.GetHashCode();
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x00048148 File Offset: 0x00046348
		public static bool operator ==(TimerState state1, TimerState state2)
		{
			return state1.Equals(state2);
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x00048164 File Offset: 0x00046364
		public static bool operator !=(TimerState state1, TimerState state2)
		{
			return !(state1 == state2);
		}
	}
}
