using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	/// <summary>Provides data for the <see cref="E:Microsoft.Win32.SystemEvents.SessionEnded" /> event.</summary>
	// Token: 0x02000123 RID: 291
	[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
	public class SessionEndedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Win32.SessionEndedEventArgs" /> class.</summary>
		/// <param name="reason">One of the <see cref="T:Microsoft.Win32.SessionEndReasons" /> values indicating how the session ended.</param>
		// Token: 0x060006CB RID: 1739 RVA: 0x000136AB File Offset: 0x000118AB
		public SessionEndedEventArgs(SessionEndReasons reason)
		{
			this.myreason = reason;
		}

		/// <summary>Gets an identifier that indicates how the session ended.</summary>
		/// <returns>One of the <see cref="T:Microsoft.Win32.SessionEndReasons" /> values that indicates how the session ended.</returns>
		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060006CC RID: 1740 RVA: 0x000136BA File Offset: 0x000118BA
		public SessionEndReasons Reason
		{
			get
			{
				return this.myreason;
			}
		}

		// Token: 0x040004D5 RID: 1237
		private SessionEndReasons myreason;
	}
}
