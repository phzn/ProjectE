using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	/// <summary>Provides data for the <see cref="E:Microsoft.Win32.SystemEvents.UserPreferenceChanging" /> event.</summary>
	// Token: 0x02000130 RID: 304
	[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
	public class UserPreferenceChangingEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Win32.UserPreferenceChangingEventArgs" /> class using the specified user preference category identifier.</summary>
		/// <param name="category">One of the <see cref="T:Microsoft.Win32.UserPreferenceCategory" /> values that indicate the user preference category that is changing.</param>
		// Token: 0x0600070D RID: 1805 RVA: 0x00013875 File Offset: 0x00011A75
		public UserPreferenceChangingEventArgs(UserPreferenceCategory category)
		{
			this.mycategory = category;
		}

		/// <summary>Gets the category of user preferences that is changing.</summary>
		/// <returns>One of the <see cref="T:Microsoft.Win32.UserPreferenceCategory" /> values that indicates the category of user preferences that is changing.</returns>
		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600070E RID: 1806 RVA: 0x00013884 File Offset: 0x00011A84
		public UserPreferenceCategory Category
		{
			get
			{
				return this.mycategory;
			}
		}

		// Token: 0x040004F6 RID: 1270
		private UserPreferenceCategory mycategory;
	}
}
