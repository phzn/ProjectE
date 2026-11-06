using System;
using System.Configuration;
using System.Net.Security;
using System.Net.Sockets;

namespace System.Net.Configuration
{
	// Token: 0x0200075A RID: 1882
	internal sealed class SettingsSectionInternal
	{
		// Token: 0x17000D64 RID: 3428
		// (get) Token: 0x06003B52 RID: 15186 RVA: 0x000CC2F4 File Offset: 0x000CA4F4
		internal static SettingsSectionInternal Section
		{
			get
			{
				return SettingsSectionInternal.instance;
			}
		}

		// Token: 0x17000D65 RID: 3429
		// (get) Token: 0x06003B53 RID: 15187 RVA: 0x000CC2FB File Offset: 0x000CA4FB
		// (set) Token: 0x06003B54 RID: 15188 RVA: 0x000CC303 File Offset: 0x000CA503
		internal bool UseNagleAlgorithm { get; set; }

		// Token: 0x17000D66 RID: 3430
		// (get) Token: 0x06003B55 RID: 15189 RVA: 0x000CC30C File Offset: 0x000CA50C
		// (set) Token: 0x06003B56 RID: 15190 RVA: 0x000CC314 File Offset: 0x000CA514
		internal bool Expect100Continue { get; set; }

		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x06003B57 RID: 15191 RVA: 0x000CC31D File Offset: 0x000CA51D
		// (set) Token: 0x06003B58 RID: 15192 RVA: 0x000CC325 File Offset: 0x000CA525
		internal bool CheckCertificateName { get; private set; }

		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x06003B59 RID: 15193 RVA: 0x000CC32E File Offset: 0x000CA52E
		// (set) Token: 0x06003B5A RID: 15194 RVA: 0x000CC336 File Offset: 0x000CA536
		internal int DnsRefreshTimeout { get; set; }

		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x06003B5B RID: 15195 RVA: 0x000CC33F File Offset: 0x000CA53F
		// (set) Token: 0x06003B5C RID: 15196 RVA: 0x000CC347 File Offset: 0x000CA547
		internal bool EnableDnsRoundRobin { get; set; }

		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x06003B5D RID: 15197 RVA: 0x000CC350 File Offset: 0x000CA550
		// (set) Token: 0x06003B5E RID: 15198 RVA: 0x000CC358 File Offset: 0x000CA558
		internal bool CheckCertificateRevocationList { get; set; }

		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x06003B5F RID: 15199 RVA: 0x000CC361 File Offset: 0x000CA561
		// (set) Token: 0x06003B60 RID: 15200 RVA: 0x000CC369 File Offset: 0x000CA569
		internal EncryptionPolicy EncryptionPolicy { get; private set; }

		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x06003B61 RID: 15201 RVA: 0x000CC374 File Offset: 0x000CA574
		internal bool Ipv6Enabled
		{
			get
			{
				try
				{
					SettingsSection settingsSection = (SettingsSection)ConfigurationManager.GetSection("system.net/settings");
					if (settingsSection != null)
					{
						return settingsSection.Ipv6.Enabled;
					}
				}
				catch
				{
				}
				return true;
			}
		}

		// Token: 0x0400236E RID: 9070
		private static readonly SettingsSectionInternal instance = new SettingsSectionInternal();

		// Token: 0x0400236F RID: 9071
		internal UnicodeEncodingConformance WebUtilityUnicodeEncodingConformance;

		// Token: 0x04002370 RID: 9072
		internal UnicodeDecodingConformance WebUtilityUnicodeDecodingConformance;

		// Token: 0x04002371 RID: 9073
		internal readonly bool HttpListenerUnescapeRequestUrl = true;

		// Token: 0x04002372 RID: 9074
		internal readonly IPProtectionLevel IPProtectionLevel = IPProtectionLevel.Unspecified;
	}
}
