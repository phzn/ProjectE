using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	/// <summary>Provides data for the <see cref="E:Microsoft.Win32.SystemEvents.UserPreferenceChanged" /> event.</summary>
	// Token: 0x0200012E RID: 302
	[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
	public class UserPreferenceChangedEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:Microsoft.Win32.UserPreferenceChangedEventArgs" /> class using the specified user preference category identifier.</summary>
		/// <param name="category">One of the <see cref="T:Microsoft.Win32.UserPreferenceCategory" /> values that indicates the user preference category that has changed.</param>
		// Token: 0x06000707 RID: 1799 RVA: 0x0001385E File Offset: 0x00011A5E
		public UserPreferenceChangedEventArgs(UserPreferenceCategory category)
		{
			this.mycategory = category;
		}

		/// <summary>Gets the category of user preferences that has changed.</summary>
		/// <returns>One of the <see cref="T:Microsoft.Win32.UserPreferenceCategory" /> values that indicates the category of user preferences that has changed.</returns>
		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x0001386D File Offset: 0x00011A6D
		public UserPreferenceCategory Category
		{
			get
			{
				return this.mycategory;
			}
		}

		// Token: 0x040004F5 RID: 1269
		private UserPreferenceCategory mycategory;
	}
}
