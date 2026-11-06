using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Computes a Hash-based Message Authentication Code (HMAC) using the <see cref="T:System.Security.Cryptography.SHA384" /> hash function.</summary>
	// Token: 0x02000494 RID: 1172
	[ComVisible(true)]
	public class HMACSHA384 : HMAC
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.HMACSHA384" /> class by using a randomly generated key.</summary>
		// Token: 0x06002F08 RID: 12040 RVA: 0x000A7E65 File Offset: 0x000A6065
		public HMACSHA384()
			: this(Utils.GenerateRandom(128))
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.HMACSHA384" /> class by using the specified key data.</summary>
		/// <param name="key">The secret key for <see cref="T:System.Security.Cryptography.HMACSHA384" /> encryption. The key can be any length. However, the recommended size is 128 bytes. If the key is more than 128 bytes long, it is hashed (using SHA-384) to derive a 128-byte key. If it is less than 128 bytes long, it is padded to 128 bytes.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="key" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06002F09 RID: 12041 RVA: 0x000A7E78 File Offset: 0x000A6078
		[SecuritySafeCritical]
		public HMACSHA384(byte[] key)
		{
			this.m_hashName = "SHA384";
			this.m_hash1 = HMAC.GetHashAlgorithmWithFipsFallback(() => new SHA384Managed(), () => HashAlgorithm.Create("System.Security.Cryptography.SHA384CryptoServiceProvider"));
			this.m_hash2 = HMAC.GetHashAlgorithmWithFipsFallback(() => new SHA384Managed(), () => HashAlgorithm.Create("System.Security.Cryptography.SHA384CryptoServiceProvider"));
			this.HashSizeValue = 384;
			base.BlockSizeValue = this.BlockSize;
			base.InitializeKey(key);
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06002F0A RID: 12042 RVA: 0x000A7F51 File Offset: 0x000A6151
		private int BlockSize
		{
			get
			{
				if (!this.m_useLegacyBlockSize)
				{
					return 128;
				}
				return 64;
			}
		}

		/// <summary>Provides a workaround for the .NET Framework 2.0 implementation of the <see cref="T:System.Security.Cryptography.HMACSHA384" /> algorithm, which is inconsistent with the .NET Framework 2.0 Service Pack 1 implementation of the algorithm.</summary>
		/// <returns>
		///   <see langword="true" /> to enable .NET Framework 2.0 Service Pack 1 applications to interact with .NET Framework 2.0 applications; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06002F0B RID: 12043 RVA: 0x000A7F63 File Offset: 0x000A6163
		// (set) Token: 0x06002F0C RID: 12044 RVA: 0x000A7F6B File Offset: 0x000A616B
		public bool ProduceLegacyHmacValues
		{
			get
			{
				return this.m_useLegacyBlockSize;
			}
			set
			{
				this.m_useLegacyBlockSize = value;
				base.BlockSizeValue = this.BlockSize;
				base.InitializeKey(this.KeyValue);
			}
		}

		// Token: 0x04002171 RID: 8561
		private bool m_useLegacyBlockSize = Utils._ProduceLegacyHmacValues();
	}
}
