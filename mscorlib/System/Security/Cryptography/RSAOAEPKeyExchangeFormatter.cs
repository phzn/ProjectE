using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Creates Optimal Asymmetric Encryption Padding (OAEP) key exchange data using <see cref="T:System.Security.Cryptography.RSA" />.</summary>
	// Token: 0x020004AD RID: 1197
	[ComVisible(true)]
	public class RSAOAEPKeyExchangeFormatter : AsymmetricKeyExchangeFormatter
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.RSAOAEPKeyExchangeFormatter" /> class.</summary>
		// Token: 0x0600300F RID: 12303 RVA: 0x000AE878 File Offset: 0x000ACA78
		public RSAOAEPKeyExchangeFormatter()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.RSAOAEPKeyExchangeFormatter" /> class with the specified key.</summary>
		/// <param name="key">The instance of the <see cref="T:System.Security.Cryptography.RSA" /> algorithm that holds the public key.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is <see langword="null" />.</exception>
		// Token: 0x06003010 RID: 12304 RVA: 0x000AE880 File Offset: 0x000ACA80
		public RSAOAEPKeyExchangeFormatter(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		/// <summary>Gets or sets the parameter used to create padding in the key exchange creation process.</summary>
		/// <returns>The parameter value.</returns>
		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06003011 RID: 12305 RVA: 0x000AE8A2 File Offset: 0x000ACAA2
		// (set) Token: 0x06003012 RID: 12306 RVA: 0x000AE8BE File Offset: 0x000ACABE
		public byte[] Parameter
		{
			get
			{
				if (this.ParameterValue != null)
				{
					return (byte[])this.ParameterValue.Clone();
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.ParameterValue = (byte[])value.Clone();
					return;
				}
				this.ParameterValue = null;
			}
		}

		/// <summary>Gets the parameters for the Optimal Asymmetric Encryption Padding (OAEP) key exchange.</summary>
		/// <returns>An XML string containing the parameters of the OAEP key exchange operation.</returns>
		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06003013 RID: 12307 RVA: 0x0000AF5E File Offset: 0x0000915E
		public override string Parameters
		{
			get
			{
				return null;
			}
		}

		/// <summary>Gets or sets the random number generator algorithm to use in the creation of the key exchange.</summary>
		/// <returns>The instance of a random number generator algorithm to use.</returns>
		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06003014 RID: 12308 RVA: 0x000AE8DC File Offset: 0x000ACADC
		// (set) Token: 0x06003015 RID: 12309 RVA: 0x000AE8E4 File Offset: 0x000ACAE4
		public RandomNumberGenerator Rng
		{
			get
			{
				return this.RngValue;
			}
			set
			{
				this.RngValue = value;
			}
		}

		/// <summary>Sets the public key to use for encrypting the key exchange data.</summary>
		/// <param name="key">The instance of the <see cref="T:System.Security.Cryptography.RSA" /> algorithm that holds the public key.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="key" /> is <see langword="null" />.</exception>
		// Token: 0x06003016 RID: 12310 RVA: 0x000AE8ED File Offset: 0x000ACAED
		public override void SetKey(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
			this._rsaOverridesEncrypt = null;
		}

		/// <summary>Creates the encrypted key exchange data from the specified input data.</summary>
		/// <param name="rgbData">The secret information to be passed in the key exchange.</param>
		/// <returns>The encrypted key exchange data to be sent to the intended recipient.</returns>
		/// <exception cref="T:System.Security.Cryptography.CryptographicUnexpectedOperationException">The key is missing.</exception>
		// Token: 0x06003017 RID: 12311 RVA: 0x000AE918 File Offset: 0x000ACB18
		[SecuritySafeCritical]
		public override byte[] CreateKeyExchange(byte[] rgbData)
		{
			if (this._rsaKey == null)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("No asymmetric key object has been associated with this formatter object."));
			}
			if (this.OverridesEncrypt)
			{
				return this._rsaKey.Encrypt(rgbData, RSAEncryptionPadding.OaepSHA1);
			}
			return Utils.RsaOaepEncrypt(this._rsaKey, SHA1.Create(), new PKCS1MaskGenerationMethod(), RandomNumberGenerator.Create(), rgbData);
		}

		/// <summary>Creates the encrypted key exchange data from the specified input data.</summary>
		/// <param name="rgbData">The secret information to be passed in the key exchange.</param>
		/// <param name="symAlgType">This parameter is not used in the current version.</param>
		/// <returns>The encrypted key exchange data to be sent to the intended recipient.</returns>
		// Token: 0x06003018 RID: 12312 RVA: 0x000AE972 File Offset: 0x000ACB72
		public override byte[] CreateKeyExchange(byte[] rgbData, Type symAlgType)
		{
			return this.CreateKeyExchange(rgbData);
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06003019 RID: 12313 RVA: 0x000AE97C File Offset: 0x000ACB7C
		private bool OverridesEncrypt
		{
			get
			{
				if (this._rsaOverridesEncrypt == null)
				{
					this._rsaOverridesEncrypt = new bool?(Utils.DoesRsaKeyOverride(this._rsaKey, "Encrypt", new Type[]
					{
						typeof(byte[]),
						typeof(RSAEncryptionPadding)
					}));
				}
				return this._rsaOverridesEncrypt.Value;
			}
		}

		// Token: 0x040021CC RID: 8652
		private byte[] ParameterValue;

		// Token: 0x040021CD RID: 8653
		private RSA _rsaKey;

		// Token: 0x040021CE RID: 8654
		private bool? _rsaOverridesEncrypt;

		// Token: 0x040021CF RID: 8655
		private RandomNumberGenerator RngValue;
	}
}
