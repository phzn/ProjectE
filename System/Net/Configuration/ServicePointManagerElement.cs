using System;
using System.Configuration;
using System.Net.Security;
using Unity;

namespace System.Net.Configuration
{
	/// <summary>Represents the default settings used to create connections to a remote computer. This class cannot be inherited.</summary>
	// Token: 0x02000777 RID: 1911
	public sealed class ServicePointManagerElement : ConfigurationElement
	{
		// Token: 0x06003C21 RID: 15393 RVA: 0x000CDC5C File Offset: 0x000CBE5C
		static ServicePointManagerElement()
		{
			ServicePointManagerElement.properties.Add(ServicePointManagerElement.checkCertificateNameProp);
			ServicePointManagerElement.properties.Add(ServicePointManagerElement.checkCertificateRevocationListProp);
			ServicePointManagerElement.properties.Add(ServicePointManagerElement.dnsRefreshTimeoutProp);
			ServicePointManagerElement.properties.Add(ServicePointManagerElement.enableDnsRoundRobinProp);
			ServicePointManagerElement.properties.Add(ServicePointManagerElement.expect100ContinueProp);
			ServicePointManagerElement.properties.Add(ServicePointManagerElement.useNagleAlgorithmProp);
		}

		/// <summary>Gets or sets a Boolean value that controls checking host name information in an X509 certificate.</summary>
		/// <returns>
		///   <see langword="true" /> to specify host name checking; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x06003C23 RID: 15395 RVA: 0x000CDD8B File Offset: 0x000CBF8B
		// (set) Token: 0x06003C24 RID: 15396 RVA: 0x000CDD9D File Offset: 0x000CBF9D
		[ConfigurationProperty("checkCertificateName", DefaultValue = "True")]
		public bool CheckCertificateName
		{
			get
			{
				return (bool)base[ServicePointManagerElement.checkCertificateNameProp];
			}
			set
			{
				base[ServicePointManagerElement.checkCertificateNameProp] = value;
			}
		}

		/// <summary>Gets or sets a Boolean value that indicates whether the certificate is checked against the certificate authority revocation list.</summary>
		/// <returns>
		///   <see langword="true" /> if the certificate revocation list is checked; otherwise, <see langword="false" />.The default value is <see langword="false" />.</returns>
		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x06003C25 RID: 15397 RVA: 0x000CDDB0 File Offset: 0x000CBFB0
		// (set) Token: 0x06003C26 RID: 15398 RVA: 0x000CDDC2 File Offset: 0x000CBFC2
		[ConfigurationProperty("checkCertificateRevocationList", DefaultValue = "False")]
		public bool CheckCertificateRevocationList
		{
			get
			{
				return (bool)base[ServicePointManagerElement.checkCertificateRevocationListProp];
			}
			set
			{
				base[ServicePointManagerElement.checkCertificateRevocationListProp] = value;
			}
		}

		/// <summary>Gets or sets the amount of time after which address information is refreshed.</summary>
		/// <returns>A <see cref="T:System.TimeSpan" /> that specifies when addresses are resolved using DNS.</returns>
		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x06003C27 RID: 15399 RVA: 0x000CDDD5 File Offset: 0x000CBFD5
		// (set) Token: 0x06003C28 RID: 15400 RVA: 0x000CDDE7 File Offset: 0x000CBFE7
		[ConfigurationProperty("dnsRefreshTimeout", DefaultValue = "120000")]
		public int DnsRefreshTimeout
		{
			get
			{
				return (int)base[ServicePointManagerElement.dnsRefreshTimeoutProp];
			}
			set
			{
				base[ServicePointManagerElement.dnsRefreshTimeoutProp] = value;
			}
		}

		/// <summary>Gets or sets a Boolean value that controls using different IP addresses on connections to the same server.</summary>
		/// <returns>
		///   <see langword="true" /> to enable DNS round-robin behavior; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000DB0 RID: 3504
		// (get) Token: 0x06003C29 RID: 15401 RVA: 0x000CDDFA File Offset: 0x000CBFFA
		// (set) Token: 0x06003C2A RID: 15402 RVA: 0x000CDE0C File Offset: 0x000CC00C
		[ConfigurationProperty("enableDnsRoundRobin", DefaultValue = "False")]
		public bool EnableDnsRoundRobin
		{
			get
			{
				return (bool)base[ServicePointManagerElement.enableDnsRoundRobinProp];
			}
			set
			{
				base[ServicePointManagerElement.enableDnsRoundRobinProp] = value;
			}
		}

		/// <summary>Gets or sets a Boolean value that determines whether 100-Continue behavior is used.</summary>
		/// <returns>
		///   <see langword="true" /> to expect 100-Continue responses for <see langword="POST" /> requests; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x17000DB1 RID: 3505
		// (get) Token: 0x06003C2B RID: 15403 RVA: 0x000CDE1F File Offset: 0x000CC01F
		// (set) Token: 0x06003C2C RID: 15404 RVA: 0x000CDE31 File Offset: 0x000CC031
		[ConfigurationProperty("expect100Continue", DefaultValue = "True")]
		public bool Expect100Continue
		{
			get
			{
				return (bool)base[ServicePointManagerElement.expect100ContinueProp];
			}
			set
			{
				base[ServicePointManagerElement.expect100ContinueProp] = value;
			}
		}

		/// <summary>Gets or sets a Boolean value that determines whether the Nagle algorithm is used.</summary>
		/// <returns>
		///   <see langword="true" /> to use the Nagle algorithm; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x17000DB2 RID: 3506
		// (get) Token: 0x06003C2D RID: 15405 RVA: 0x000CDE44 File Offset: 0x000CC044
		// (set) Token: 0x06003C2E RID: 15406 RVA: 0x000CDE56 File Offset: 0x000CC056
		[ConfigurationProperty("useNagleAlgorithm", DefaultValue = "True")]
		public bool UseNagleAlgorithm
		{
			get
			{
				return (bool)base[ServicePointManagerElement.useNagleAlgorithmProp];
			}
			set
			{
				base[ServicePointManagerElement.useNagleAlgorithmProp] = value;
			}
		}

		// Token: 0x17000DB3 RID: 3507
		// (get) Token: 0x06003C2F RID: 15407 RVA: 0x000CDE69 File Offset: 0x000CC069
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ServicePointManagerElement.properties;
			}
		}

		// Token: 0x06003C30 RID: 15408 RVA: 0x00003917 File Offset: 0x00001B17
		[MonoTODO]
		protected override void PostDeserialize()
		{
		}

		/// <summary>Gets or sets the <see cref="T:System.Net.Security.EncryptionPolicy" /> to use.</summary>
		/// <returns>The encryption policy to use for a <see cref="T:System.Net.ServicePointManager" /> instance.</returns>
		// Token: 0x17000DB4 RID: 3508
		// (get) Token: 0x06003C31 RID: 15409 RVA: 0x000CDE70 File Offset: 0x000CC070
		// (set) Token: 0x06003C32 RID: 15410 RVA: 0x00013BCA File Offset: 0x00011DCA
		public EncryptionPolicy EncryptionPolicy
		{
			get
			{
				global::Unity.ThrowStub.ThrowNotSupportedException();
				return EncryptionPolicy.RequireEncryption;
			}
			set
			{
				global::Unity.ThrowStub.ThrowNotSupportedException();
			}
		}

		// Token: 0x040023B8 RID: 9144
		private static ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x040023B9 RID: 9145
		private static ConfigurationProperty checkCertificateNameProp = new ConfigurationProperty("checkCertificateName", typeof(bool), true);

		// Token: 0x040023BA RID: 9146
		private static ConfigurationProperty checkCertificateRevocationListProp = new ConfigurationProperty("checkCertificateRevocationList", typeof(bool), false);

		// Token: 0x040023BB RID: 9147
		private static ConfigurationProperty dnsRefreshTimeoutProp = new ConfigurationProperty("dnsRefreshTimeout", typeof(int), 120000);

		// Token: 0x040023BC RID: 9148
		private static ConfigurationProperty enableDnsRoundRobinProp = new ConfigurationProperty("enableDnsRoundRobin", typeof(bool), false);

		// Token: 0x040023BD RID: 9149
		private static ConfigurationProperty expect100ContinueProp = new ConfigurationProperty("expect100Continue", typeof(bool), true);

		// Token: 0x040023BE RID: 9150
		private static ConfigurationProperty useNagleAlgorithmProp = new ConfigurationProperty("useNagleAlgorithm", typeof(bool), true);
	}
}
