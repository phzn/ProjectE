using System;

namespace System.Globalization
{
	/// <summary>Provides information about the version of Unicode used to compare and order strings.</summary>
	// Token: 0x0200096E RID: 2414
	[Serializable]
	public sealed class SortVersion : IEquatable<SortVersion>
	{
		/// <summary>Gets the full version number of the <see cref="T:System.Globalization.SortVersion" /> object.</summary>
		/// <returns>The version number of this <see cref="T:System.Globalization.SortVersion" /> object.</returns>
		// Token: 0x17000E2B RID: 3627
		// (get) Token: 0x06005549 RID: 21833 RVA: 0x0011E385 File Offset: 0x0011C585
		public int FullVersion
		{
			get
			{
				return this.m_NlsVersion;
			}
		}

		/// <summary>Gets a globally unique identifier for this <see cref="T:System.Globalization.SortVersion" /> object.</summary>
		/// <returns>A globally unique identifier for this <see cref="T:System.Globalization.SortVersion" /> object.</returns>
		// Token: 0x17000E2C RID: 3628
		// (get) Token: 0x0600554A RID: 21834 RVA: 0x0011E38D File Offset: 0x0011C58D
		public Guid SortId
		{
			get
			{
				return this.m_SortId;
			}
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Globalization.SortVersion" /> class.</summary>
		/// <param name="fullVersion">A version number.</param>
		/// <param name="sortId">A sort ID.</param>
		// Token: 0x0600554B RID: 21835 RVA: 0x0011E395 File Offset: 0x0011C595
		public SortVersion(int fullVersion, Guid sortId)
		{
			this.m_SortId = sortId;
			this.m_NlsVersion = fullVersion;
		}

		// Token: 0x0600554C RID: 21836 RVA: 0x0011E3AC File Offset: 0x0011C5AC
		internal SortVersion(int nlsVersion, int effectiveId, Guid customVersion)
		{
			this.m_NlsVersion = nlsVersion;
			if (customVersion == Guid.Empty)
			{
				byte b = (byte)(effectiveId >> 24);
				byte b2 = (byte)((effectiveId & 16711680) >> 16);
				byte b3 = (byte)((effectiveId & 65280) >> 8);
				byte b4 = (byte)(effectiveId & 255);
				customVersion = new Guid(0, 0, 0, 0, 0, 0, 0, b, b2, b3, b4);
			}
			this.m_SortId = customVersion;
		}

		/// <summary>Returns a value that indicates whether this <see cref="T:System.Globalization.SortVersion" /> instance is equal to a specified object.</summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="obj" /> is a <see cref="T:System.Globalization.SortVersion" /> object that represents the same version as this instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600554D RID: 21837 RVA: 0x0011E414 File Offset: 0x0011C614
		public override bool Equals(object obj)
		{
			SortVersion sortVersion = obj as SortVersion;
			return sortVersion != null && this.Equals(sortVersion);
		}

		/// <summary>Returns a value that indicates whether this <see cref="T:System.Globalization.SortVersion" /> instance is equal to a specified <see cref="T:System.Globalization.SortVersion" /> object.</summary>
		/// <param name="other">The object to compare with this instance.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="other" /> represents the same version as this instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600554E RID: 21838 RVA: 0x0011E43A File Offset: 0x0011C63A
		public bool Equals(SortVersion other)
		{
			return !(other == null) && this.m_NlsVersion == other.m_NlsVersion && this.m_SortId == other.m_SortId;
		}

		/// <summary>Returns a hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		// Token: 0x0600554F RID: 21839 RVA: 0x0011E468 File Offset: 0x0011C668
		public override int GetHashCode()
		{
			return (this.m_NlsVersion * 7) | this.m_SortId.GetHashCode();
		}

		/// <summary>Indicates whether two <see cref="T:System.Globalization.SortVersion" /> instances are equal.</summary>
		/// <param name="left">The first instance to compare.</param>
		/// <param name="right">The second instance to compare.</param>
		/// <returns>
		///   <see langword="true" /> if the values of <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005550 RID: 21840 RVA: 0x0011E484 File Offset: 0x0011C684
		public static bool operator ==(SortVersion left, SortVersion right)
		{
			if (left != null)
			{
				return left.Equals(right);
			}
			return right == null || right.Equals(left);
		}

		/// <summary>Indicates whether two <see cref="T:System.Globalization.SortVersion" /> instances are not equal.</summary>
		/// <param name="left">The first instance to compare.</param>
		/// <param name="right">The second instance to compare.</param>
		/// <returns>
		///   <see langword="true" /> if the values of <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005551 RID: 21841 RVA: 0x0011E49D File Offset: 0x0011C69D
		public static bool operator !=(SortVersion left, SortVersion right)
		{
			return !(left == right);
		}

		// Token: 0x040034E2 RID: 13538
		private int m_NlsVersion;

		// Token: 0x040034E3 RID: 13539
		private Guid m_SortId;
	}
}
