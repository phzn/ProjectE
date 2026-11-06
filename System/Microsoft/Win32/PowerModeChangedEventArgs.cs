using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	/// <summary>Provides data for the <see cref="E:Microsoft.Win32.SystemEvents.PowerModeChanged" /> event.</summary>
	// Token: 0x0200011F RID: 287
	[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
	public class PowerModeChangedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Win32.PowerModeChangedEventArgs" /> class using the specified power mode event type.</summary>
		/// <param name="mode">One of the <see cref="T:Microsoft.Win32.PowerModes" /> values that represents the type of power mode event.</param>
		// Token: 0x060006C5 RID: 1733 RVA: 0x00013694 File Offset: 0x00011894
		public PowerModeChangedEventArgs(PowerModes mode)
		{
			this.mymode = mode;
		}

		/// <summary>Gets an identifier that indicates the type of the power mode event that has occurred.</summary>
		/// <returns>One of the <see cref="T:Microsoft.Win32.PowerModes" /> values.</returns>
		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060006C6 RID: 1734 RVA: 0x000136A3 File Offset: 0x000118A3
		public PowerModes Mode
		{
			get
			{
				return this.mymode;
			}
		}

		// Token: 0x040004CD RID: 1229
		private PowerModes mymode;
	}
}
