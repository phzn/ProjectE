using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	/// <summary>Provides data for the <see cref="E:Microsoft.Win32.SystemEvents.SessionSwitch" /> event.</summary>
	// Token: 0x02000127 RID: 295
	[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
	public class SessionSwitchEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Win32.SessionSwitchEventArgs" /> class using the specified session change event type identifer.</summary>
		/// <param name="reason">A <see cref="T:Microsoft.Win32.SessionSwitchReason" /> that indicates the type of session change event.</param>
		// Token: 0x060006D9 RID: 1753 RVA: 0x000136EA File Offset: 0x000118EA
		public SessionSwitchEventArgs(SessionSwitchReason reason)
		{
			this.reason = reason;
		}

		/// <summary>Gets an identifier that indicates the type of session change event.</summary>
		/// <returns>A <see cref="T:Microsoft.Win32.SessionSwitchReason" /> indicating the type of the session change event.</returns>
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x000136F9 File Offset: 0x000118F9
		public SessionSwitchReason Reason
		{
			get
			{
				return this.reason;
			}
		}

		// Token: 0x040004D8 RID: 1240
		private SessionSwitchReason reason;
	}
}
