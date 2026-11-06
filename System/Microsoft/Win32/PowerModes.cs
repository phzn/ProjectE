using System;

namespace Microsoft.Win32
{
	/// <summary>Defines identifiers for power mode events reported by the operating system.</summary>
	// Token: 0x02000121 RID: 289
	public enum PowerModes
	{
		/// <summary>The operating system is about to resume from a suspended state.</summary>
		// Token: 0x040004CF RID: 1231
		Resume = 1,
		/// <summary>A power mode status notification event has been raised by the operating system. This might indicate a weak or charging battery, a transition between AC power and battery, or another change in the status of the system power supply.</summary>
		// Token: 0x040004D0 RID: 1232
		StatusChange,
		/// <summary>The operating system is about to be suspended.</summary>
		// Token: 0x040004D1 RID: 1233
		Suspend
	}
}
