using System;

namespace System.Net.Mail
{
	/// <summary>Describes the delivery notification options for email.</summary>
	// Token: 0x020007FD RID: 2045
	[Flags]
	public enum DeliveryNotificationOptions
	{
		/// <summary>No notification information will be sent. The mail server will utilize its configured behavior to determine whether it should generate a delivery notification.</summary>
		// Token: 0x04002726 RID: 10022
		None = 0,
		/// <summary>Notify if the delivery is successful.</summary>
		// Token: 0x04002727 RID: 10023
		OnSuccess = 1,
		/// <summary>Notify if the delivery is unsuccessful.</summary>
		// Token: 0x04002728 RID: 10024
		OnFailure = 2,
		/// <summary>Notify if the delivery is delayed.</summary>
		// Token: 0x04002729 RID: 10025
		Delay = 4,
		/// <summary>A notification should not be generated under any circumstances.</summary>
		// Token: 0x0400272A RID: 10026
		Never = 134217728
	}
}
