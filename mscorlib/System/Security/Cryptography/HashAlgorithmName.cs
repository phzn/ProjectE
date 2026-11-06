using System;

namespace System.Security.Cryptography
{
	/// <summary>Specifies the name of a cryptographic hash algorithm.</summary>
	// Token: 0x02000472 RID: 1138
	public readonly struct HashAlgorithmName : IEquatable<HashAlgorithmName>
	{
		/// <summary>Gets a hash algorithm name that represents "MD5".</summary>
		/// <returns>A hash algorithm name that represents "MD5".</returns>
		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06002E1A RID: 11802 RVA: 0x000A5DDE File Offset: 0x000A3FDE
		public static HashAlgorithmName MD5
		{
			get
			{
				return new HashAlgorithmName("MD5");
			}
		}

		/// <summary>Gets a hash algorithm name that represents "SHA1".</summary>
		/// <returns>A hash algorithm name that represents "SHA1".</returns>
		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06002E1B RID: 11803 RVA: 0x000A5DEA File Offset: 0x000A3FEA
		public static HashAlgorithmName SHA1
		{
			get
			{
				return new HashAlgorithmName("SHA1");
			}
		}

		/// <summary>Gets a hash algorithm name that represents "SHA256".</summary>
		/// <returns>A hash algorithm name that represents "SHA256".</returns>
		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06002E1C RID: 11804 RVA: 0x000A5DF6 File Offset: 0x000A3FF6
		public static HashAlgorithmName SHA256
		{
			get
			{
				return new HashAlgorithmName("SHA256");
			}
		}

		/// <summary>Gets a hash algorithm name that represents "SHA384".</summary>
		/// <returns>A hash algorithm name that represents "SHA384".</returns>
		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06002E1D RID: 11805 RVA: 0x000A5E02 File Offset: 0x000A4002
		public static HashAlgorithmName SHA384
		{
			get
			{
				return new HashAlgorithmName("SHA384");
			}
		}

		/// <summary>Gets a hash algorithm name that represents "SHA512".</summary>
		/// <returns>A hash algorithm name that represents "SHA512".</returns>
		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06002E1E RID: 11806 RVA: 0x000A5E0E File Offset: 0x000A400E
		public static HashAlgorithmName SHA512
		{
			get
			{
				return new HashAlgorithmName("SHA512");
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.HashAlgorithmName" /> structure with a custom name.</summary>
		/// <param name="name">The custom hash algorithm name.</param>
		// Token: 0x06002E1F RID: 11807 RVA: 0x000A5E1A File Offset: 0x000A401A
		public HashAlgorithmName(string name)
		{
			this._name = name;
		}

		/// <summary>Gets the underlying string representation of the algorithm name.</summary>
		/// <returns>The string representation of the algorithm name, or <see langword="null" /> or <see cref="F:System.String.Empty" /> if no hash algorithm is available.</returns>
		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06002E20 RID: 11808 RVA: 0x000A5E23 File Offset: 0x000A4023
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		/// <summary>Returns the string representation of the current <see cref="T:System.Security.Cryptography.HashAlgorithmName" /> instance.</summary>
		/// <returns>The string representation of the current <see cref="T:System.Security.Cryptography.HashAlgorithmName" /> instance.</returns>
		// Token: 0x06002E21 RID: 11809 RVA: 0x000A5E2B File Offset: 0x000A402B
		public override string ToString()
		{
			return this._name ?? string.Empty;
		}

		/// <summary>Returns a value that indicates whether the current instance and a specified object are equal.</summary>
		/// <param name="obj">The object to compare with the current instance.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="obj" /> is a <see cref="T:System.Security.Cryptography.HashAlgorithmName" /> object and its <see cref="P:System.Security.Cryptography.HashAlgorithmName.Name" /> property is equal to that of the current instance. The comparison is ordinal and case-sensitive.</returns>
		// Token: 0x06002E22 RID: 11810 RVA: 0x000A5E3C File Offset: 0x000A403C
		public override bool Equals(object obj)
		{
			return obj is HashAlgorithmName && this.Equals((HashAlgorithmName)obj);
		}

		/// <summary>Returns a value that indicates whether two <see cref="T:System.Security.Cryptography.HashAlgorithmName" /> instances are equal.</summary>
		/// <param name="other">The object to compare with the current instance.</param>
		/// <returns>
		///   <see langword="true" /> if the <see cref="P:System.Security.Cryptography.HashAlgorithmName.Name" /> property of <paramref name="other" /> is equal to that of the current instance. The comparison is ordinal and case-sensitive.</returns>
		// Token: 0x06002E23 RID: 11811 RVA: 0x000A5E54 File Offset: 0x000A4054
		public bool Equals(HashAlgorithmName other)
		{
			return this._name == other._name;
		}

		/// <summary>Returns the hash code for the current instance.</summary>
		/// <returns>The hash code for the current instance, or 0 if no <paramref name="name" /> value was supplied to the <see cref="T:System.Security.Cryptography.HashAlgorithmName" /> constructor.</returns>
		// Token: 0x06002E24 RID: 11812 RVA: 0x000A5E67 File Offset: 0x000A4067
		public override int GetHashCode()
		{
			if (this._name != null)
			{
				return this._name.GetHashCode();
			}
			return 0;
		}

		/// <summary>Determines whether two specified <see cref="T:System.Security.Cryptography.HashAlgorithmName" /> objects are equal.</summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>
		///   <see langword="true" /> if both <paramref name="left" /> and <paramref name="right" /> have the same <see cref="P:System.Security.Cryptography.HashAlgorithmName.Name" /> value; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002E25 RID: 11813 RVA: 0x000A5E7E File Offset: 0x000A407E
		public static bool operator ==(HashAlgorithmName left, HashAlgorithmName right)
		{
			return left.Equals(right);
		}

		/// <summary>Determines whether two specified <see cref="T:System.Security.Cryptography.HashAlgorithmName" /> objects are not equal.</summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>
		///   <see langword="true" /> if both <paramref name="left" /> and <paramref name="right" /> do not have the same <see cref="P:System.Security.Cryptography.HashAlgorithmName.Name" /> value; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002E26 RID: 11814 RVA: 0x000A5E88 File Offset: 0x000A4088
		public static bool operator !=(HashAlgorithmName left, HashAlgorithmName right)
		{
			return !(left == right);
		}

		// Token: 0x04002118 RID: 8472
		private readonly string _name;
	}
}
