using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	/// <summary>The <see langword="FieldToken" /> struct is an object representation of a token that represents a field.</summary>
	// Token: 0x02000926 RID: 2342
	[ComVisible(true)]
	[Serializable]
	public readonly struct FieldToken : IEquatable<FieldToken>
	{
		// Token: 0x0600503D RID: 20541 RVA: 0x000FAEAE File Offset: 0x000F90AE
		internal FieldToken(int val)
		{
			this.tokValue = val;
		}

		/// <summary>Determines if an object is an instance of <see langword="FieldToken" /> and is equal to this instance.</summary>
		/// <param name="obj">The object to compare to this <see langword="FieldToken" />.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="obj" /> is an instance of <see langword="FieldToken" /> and is equal to this object; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600503E RID: 20542 RVA: 0x000FAEB8 File Offset: 0x000F90B8
		public override bool Equals(object obj)
		{
			bool flag = obj is FieldToken;
			if (flag)
			{
				FieldToken fieldToken = (FieldToken)obj;
				flag = this.tokValue == fieldToken.tokValue;
			}
			return flag;
		}

		/// <summary>Indicates whether the current instance is equal to the specified <see cref="T:System.Reflection.Emit.FieldToken" />.</summary>
		/// <param name="obj">The <see cref="T:System.Reflection.Emit.FieldToken" /> to compare to the current instance.</param>
		/// <returns>
		///   <see langword="true" /> if the value of <paramref name="obj" /> is equal to the value of the current instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600503F RID: 20543 RVA: 0x000FAEE9 File Offset: 0x000F90E9
		public bool Equals(FieldToken obj)
		{
			return this.tokValue == obj.tokValue;
		}

		/// <summary>Indicates whether two <see cref="T:System.Reflection.Emit.FieldToken" /> structures are equal.</summary>
		/// <param name="a">The <see cref="T:System.Reflection.Emit.FieldToken" /> to compare to <paramref name="b" />.</param>
		/// <param name="b">The <see cref="T:System.Reflection.Emit.FieldToken" /> to compare to <paramref name="a" />.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="a" /> is equal to <paramref name="b" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005040 RID: 20544 RVA: 0x000FAEF9 File Offset: 0x000F90F9
		public static bool operator ==(FieldToken a, FieldToken b)
		{
			return object.Equals(a, b);
		}

		/// <summary>Indicates whether two <see cref="T:System.Reflection.Emit.FieldToken" /> structures are not equal.</summary>
		/// <param name="a">The <see cref="T:System.Reflection.Emit.FieldToken" /> to compare to <paramref name="b" />.</param>
		/// <param name="b">The <see cref="T:System.Reflection.Emit.FieldToken" /> to compare to <paramref name="a" />.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="a" /> is not equal to <paramref name="b" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005041 RID: 20545 RVA: 0x000FAF0C File Offset: 0x000F910C
		public static bool operator !=(FieldToken a, FieldToken b)
		{
			return !object.Equals(a, b);
		}

		/// <summary>Generates the hash code for this field.</summary>
		/// <returns>The hash code for this instance.</returns>
		// Token: 0x06005042 RID: 20546 RVA: 0x000FAF22 File Offset: 0x000F9122
		public override int GetHashCode()
		{
			return this.tokValue;
		}

		/// <summary>Retrieves the metadata token for this field.</summary>
		/// <returns>Read-only. Retrieves the metadata token of this field.</returns>
		// Token: 0x17000D35 RID: 3381
		// (get) Token: 0x06005043 RID: 20547 RVA: 0x000FAF22 File Offset: 0x000F9122
		public int Token
		{
			get
			{
				return this.tokValue;
			}
		}

		// Token: 0x0400316B RID: 12651
		internal readonly int tokValue;

		/// <summary>The default FieldToken with <see cref="P:System.Reflection.Emit.FieldToken.Token" /> value 0.</summary>
		// Token: 0x0400316C RID: 12652
		public static readonly FieldToken Empty;
	}
}
