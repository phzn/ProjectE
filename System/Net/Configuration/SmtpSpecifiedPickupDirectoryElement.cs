using System;
using System.Configuration;

namespace System.Net.Configuration
{
	/// <summary>Represents an SMTP pickup directory configuration element.</summary>
	// Token: 0x0200077B RID: 1915
	public sealed class SmtpSpecifiedPickupDirectoryElement : ConfigurationElement
	{
		// Token: 0x06003C5C RID: 15452 RVA: 0x000CE183 File Offset: 0x000CC383
		static SmtpSpecifiedPickupDirectoryElement()
		{
			SmtpSpecifiedPickupDirectoryElement.properties.Add(SmtpSpecifiedPickupDirectoryElement.pickupDirectoryLocationProp);
		}

		/// <summary>Gets or sets the folder where applications save mail messages to be processed by the SMTP server.</summary>
		/// <returns>A string that specifies the pickup directory for email messages.</returns>
		// Token: 0x17000DCE RID: 3534
		// (get) Token: 0x06003C5D RID: 15453 RVA: 0x000CE1B7 File Offset: 0x000CC3B7
		// (set) Token: 0x06003C5E RID: 15454 RVA: 0x000CE1C9 File Offset: 0x000CC3C9
		[ConfigurationProperty("pickupDirectoryLocation")]
		public string PickupDirectoryLocation
		{
			get
			{
				return (string)base[SmtpSpecifiedPickupDirectoryElement.pickupDirectoryLocationProp];
			}
			set
			{
				base[SmtpSpecifiedPickupDirectoryElement.pickupDirectoryLocationProp] = value;
			}
		}

		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x06003C5F RID: 15455 RVA: 0x000CE1D7 File Offset: 0x000CC3D7
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return SmtpSpecifiedPickupDirectoryElement.properties;
			}
		}

		// Token: 0x040023C6 RID: 9158
		private static ConfigurationProperty pickupDirectoryLocationProp = new ConfigurationProperty("pickupDirectoryLocation", typeof(string));

		// Token: 0x040023C7 RID: 9159
		private static ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();
	}
}
