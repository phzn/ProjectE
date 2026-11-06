using System;
using System.Configuration;
using Unity;

namespace System.Net.Configuration
{
	/// <summary>Represents the network element in the SMTP configuration file. This class cannot be inherited.</summary>
	// Token: 0x02000779 RID: 1913
	public sealed class SmtpNetworkElement : ConfigurationElement
	{
		/// <summary>Determines whether or not default user credentials are used to access an SMTP server. The default value is <see langword="false" />.</summary>
		/// <returns>
		///   <see langword="true" /> indicates that default user credentials will be used to access the SMTP server; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x06003C3F RID: 15423 RVA: 0x000CE006 File Offset: 0x000CC206
		// (set) Token: 0x06003C40 RID: 15424 RVA: 0x000CE018 File Offset: 0x000CC218
		[ConfigurationProperty("defaultCredentials", DefaultValue = "False")]
		public bool DefaultCredentials
		{
			get
			{
				return (bool)base["defaultCredentials"];
			}
			set
			{
				base["defaultCredentials"] = value;
			}
		}

		/// <summary>Gets or sets the name of the SMTP server.</summary>
		/// <returns>A string that represents the name of the SMTP server to connect to.</returns>
		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x06003C41 RID: 15425 RVA: 0x000CE02B File Offset: 0x000CC22B
		// (set) Token: 0x06003C42 RID: 15426 RVA: 0x000CE03D File Offset: 0x000CC23D
		[ConfigurationProperty("host")]
		public string Host
		{
			get
			{
				return (string)base["host"];
			}
			set
			{
				base["host"] = value;
			}
		}

		/// <summary>Gets or sets the user password to use to connect to an SMTP mail server.</summary>
		/// <returns>A string that represents the password to use to connect to an SMTP mail server.</returns>
		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x06003C43 RID: 15427 RVA: 0x000CE04B File Offset: 0x000CC24B
		// (set) Token: 0x06003C44 RID: 15428 RVA: 0x000CE05D File Offset: 0x000CC25D
		[ConfigurationProperty("password")]
		public string Password
		{
			get
			{
				return (string)base["password"];
			}
			set
			{
				base["password"] = value;
			}
		}

		/// <summary>Gets or sets the port that SMTP clients use to connect to an SMTP mail server. The default value is 25.</summary>
		/// <returns>A string that represents the port to connect to an SMTP mail server.</returns>
		// Token: 0x17000DC2 RID: 3522
		// (get) Token: 0x06003C45 RID: 15429 RVA: 0x000CE06B File Offset: 0x000CC26B
		// (set) Token: 0x06003C46 RID: 15430 RVA: 0x000CE07D File Offset: 0x000CC27D
		[ConfigurationProperty("port", DefaultValue = "25")]
		public int Port
		{
			get
			{
				return (int)base["port"];
			}
			set
			{
				base["port"] = value;
			}
		}

		/// <summary>Gets or sets the user name to connect to an SMTP mail server.</summary>
		/// <returns>A string that represents the user name to connect to an SMTP mail server.</returns>
		// Token: 0x17000DC3 RID: 3523
		// (get) Token: 0x06003C47 RID: 15431 RVA: 0x000CE090 File Offset: 0x000CC290
		// (set) Token: 0x06003C48 RID: 15432 RVA: 0x000CE0A2 File Offset: 0x000CC2A2
		[ConfigurationProperty("userName", DefaultValue = null)]
		public string UserName
		{
			get
			{
				return (string)base["userName"];
			}
			set
			{
				base["userName"] = value;
			}
		}

		/// <summary>Gets or sets the Service Provider Name (SPN) to use for authentication when using extended protection to connect to an SMTP mail server.</summary>
		/// <returns>A string that represents the SPN to use for authentication when using extended protection to connect to an SMTP mail server.</returns>
		// Token: 0x17000DC4 RID: 3524
		// (get) Token: 0x06003C49 RID: 15433 RVA: 0x000CE0B0 File Offset: 0x000CC2B0
		// (set) Token: 0x06003C4A RID: 15434 RVA: 0x000CE0C2 File Offset: 0x000CC2C2
		[ConfigurationProperty("targetName", DefaultValue = null)]
		public string TargetName
		{
			get
			{
				return (string)base["targetName"];
			}
			set
			{
				base["targetName"] = value;
			}
		}

		/// <summary>Gets or sets whether SSL is used to access an SMTP mail server. The default value is <see langword="false" />.</summary>
		/// <returns>
		///   <see langword="true" /> indicates that SSL will be used to access the SMTP mail server; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DC5 RID: 3525
		// (get) Token: 0x06003C4B RID: 15435 RVA: 0x000CE0D0 File Offset: 0x000CC2D0
		// (set) Token: 0x06003C4C RID: 15436 RVA: 0x000CE0E2 File Offset: 0x000CC2E2
		[ConfigurationProperty("enableSsl", DefaultValue = false)]
		public bool EnableSsl
		{
			get
			{
				return (bool)base["enableSsl"];
			}
			set
			{
				base["enableSsl"] = value;
			}
		}

		// Token: 0x17000DC6 RID: 3526
		// (get) Token: 0x06003C4D RID: 15437 RVA: 0x00031787 File Offset: 0x0002F987
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return base.Properties;
			}
		}

		// Token: 0x06003C4E RID: 15438 RVA: 0x00003917 File Offset: 0x00001B17
		protected override void PostDeserialize()
		{
		}

		/// <summary>Gets or sets the client domain name used in the initial SMTP protocol request to connect to an SMTP mail server.</summary>
		/// <returns>A string that represents the client domain name used in the initial SMTP protocol request to connect to an SMTP mail server.</returns>
		// Token: 0x17000DC7 RID: 3527
		// (get) Token: 0x06003C50 RID: 15440 RVA: 0x00032884 File Offset: 0x00030A84
		// (set) Token: 0x06003C51 RID: 15441 RVA: 0x00013BCA File Offset: 0x00011DCA
		public string ClientDomain
		{
			get
			{
				global::Unity.ThrowStub.ThrowNotSupportedException();
				return null;
			}
			set
			{
				global::Unity.ThrowStub.ThrowNotSupportedException();
			}
		}
	}
}
