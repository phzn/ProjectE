using System;
using System.Configuration;
using System.Net.Mail;

namespace System.Net.Configuration
{
	/// <summary>Represents the SMTP section in the <see langword="System.Net" /> configuration file.</summary>
	// Token: 0x0200077A RID: 1914
	public sealed class SmtpSection : ConfigurationSection
	{
		/// <summary>Gets or sets the Simple Mail Transport Protocol (SMTP) delivery method. The default delivery method is <see cref="F:System.Net.Mail.SmtpDeliveryMethod.Network" />.</summary>
		/// <returns>A string that represents the SMTP delivery method.</returns>
		// Token: 0x17000DC8 RID: 3528
		// (get) Token: 0x06003C52 RID: 15442 RVA: 0x000CE0F5 File Offset: 0x000CC2F5
		// (set) Token: 0x06003C53 RID: 15443 RVA: 0x000CE107 File Offset: 0x000CC307
		[ConfigurationProperty("deliveryMethod", DefaultValue = "Network")]
		public SmtpDeliveryMethod DeliveryMethod
		{
			get
			{
				return (SmtpDeliveryMethod)base["deliveryMethod"];
			}
			set
			{
				base["deliveryMethod"] = value;
			}
		}

		/// <summary>Gets or sets the delivery format to use for sending outgoing email using the Simple Mail Transport Protocol (SMTP).</summary>
		/// <returns>Returns <see cref="T:System.Net.Mail.SmtpDeliveryFormat" />.  
		///  The delivery format to use for sending outgoing email using SMTP.</returns>
		// Token: 0x17000DC9 RID: 3529
		// (get) Token: 0x06003C54 RID: 15444 RVA: 0x000CE11A File Offset: 0x000CC31A
		// (set) Token: 0x06003C55 RID: 15445 RVA: 0x000CE12C File Offset: 0x000CC32C
		[ConfigurationProperty("deliveryFormat", DefaultValue = SmtpDeliveryFormat.SevenBit)]
		public SmtpDeliveryFormat DeliveryFormat
		{
			get
			{
				return (SmtpDeliveryFormat)base["deliveryFormat"];
			}
			set
			{
				base["deliveryFormat"] = value;
			}
		}

		/// <summary>Gets or sets the default value that indicates who the email message is from.</summary>
		/// <returns>A string that represents the default value indicating who a mail message is from.</returns>
		// Token: 0x17000DCA RID: 3530
		// (get) Token: 0x06003C56 RID: 15446 RVA: 0x000CE13F File Offset: 0x000CC33F
		// (set) Token: 0x06003C57 RID: 15447 RVA: 0x000CE151 File Offset: 0x000CC351
		[ConfigurationProperty("from")]
		public string From
		{
			get
			{
				return (string)base["from"];
			}
			set
			{
				base["from"] = value;
			}
		}

		/// <summary>Gets the configuration element that controls the network settings used by the Simple Mail Transport Protocol (SMTP). file.<see cref="T:System.Net.Configuration.SmtpNetworkElement" />.</summary>
		/// <returns>A <see cref="T:System.Net.Configuration.SmtpNetworkElement" /> object.  
		///  The configuration element that controls the network settings used by SMTP.</returns>
		// Token: 0x17000DCB RID: 3531
		// (get) Token: 0x06003C58 RID: 15448 RVA: 0x000CE15F File Offset: 0x000CC35F
		[ConfigurationProperty("network")]
		public SmtpNetworkElement Network
		{
			get
			{
				return (SmtpNetworkElement)base["network"];
			}
		}

		/// <summary>Gets the pickup directory that will be used by the SMPT client.</summary>
		/// <returns>A <see cref="T:System.Net.Configuration.SmtpSpecifiedPickupDirectoryElement" /> object that specifies the pickup directory folder.</returns>
		// Token: 0x17000DCC RID: 3532
		// (get) Token: 0x06003C59 RID: 15449 RVA: 0x000CE171 File Offset: 0x000CC371
		[ConfigurationProperty("specifiedPickupDirectory")]
		public SmtpSpecifiedPickupDirectoryElement SpecifiedPickupDirectory
		{
			get
			{
				return (SmtpSpecifiedPickupDirectoryElement)base["specifiedPickupDirectory"];
			}
		}

		// Token: 0x17000DCD RID: 3533
		// (get) Token: 0x06003C5A RID: 15450 RVA: 0x00031787 File Offset: 0x0002F987
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return base.Properties;
			}
		}
	}
}
