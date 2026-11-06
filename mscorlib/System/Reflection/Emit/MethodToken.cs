using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	/// <summary>The <see langword="MethodToken" /> struct is an object representation of a token that represents a method.</summary>
	// Token: 0x02000937 RID: 2359
	[ComVisible(true)]
	[Serializable]
	public readonly struct MethodToken : IEquatable<MethodToken>
	{
		// Token: 0x06005170 RID: 20848 RVA: 0x000FE97D File Offset: 0x000FCB7D
		internal MethodToken(int val)
		{
			this.tokValue = val;
		}

		/// <summary>Tests whether the given object is equal to this <see langword="MethodToken" /> object.</summary>
		/// <param name="obj">The object to compare to this object.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="obj" /> is an instance of <see langword="MethodToken" /> and is equal to this object; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005171 RID: 20849 RVA: 0x000FE988 File Offset: 0x000FCB88
		public override bool Equals(object obj)
		{
			bool flag = obj is MethodToken;
			if (flag)
			{
				MethodToken methodToken = (MethodToken)obj;
				flag = this.tokValue == methodToken.tokValue;
			}
			return flag;
		}

		/// <summary>Indicates whether the current instance is equal to the specified <see cref="T:System.Reflection.Emit.MethodToken" />.</summary>
		/// <param name="obj">The <see cref="T:System.Reflection.Emit.MethodToken" /> to compare to the current instance.</param>
		/// <returns>
		///   <see langword="true" /> if the value of <paramref name="obj" /> is equal to the value of the current instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005172 RID: 20850 RVA: 0x000FE9B9 File Offset: 0x000FCBB9
		public bool Equals(MethodToken obj)
		{
			return this.tokValue == obj.tokValue;
		}

		/// <summary>Indicates whether two <see cref="T:System.Reflection.Emit.MethodToken" /> structures are equal.</summary>
		/// <param name="a">The <see cref="T:System.Reflection.Emit.MethodToken" /> to compare to <paramref name="b" />.</param>
		/// <param name="b">The <see cref="T:System.Reflection.Emit.MethodToken" /> to compare to <paramref name="a" />.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="a" /> is equal to <paramref name="b" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005173 RID: 20851 RVA: 0x000FE9C9 File Offset: 0x000FCBC9
		public static bool operator ==(MethodToken a, MethodToken b)
		{
			return object.Equals(a, b);
		}

		/// <summary>Indicates whether two <see cref="T:System.Reflection.Emit.MethodToken" /> structures are not equal.</summary>
		/// <param name="a">The <see cref="T:System.Reflection.Emit.MethodToken" /> to compare to <paramref name="b" />.</param>
		/// <param name="b">The <see cref="T:System.Reflection.Emit.MethodToken" /> to compare to <paramref name="a" />.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="a" /> is not equal to <paramref name="b" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005174 RID: 20852 RVA: 0x000FE9DC File Offset: 0x000FCBDC
		public static bool operator !=(MethodToken a, MethodToken b)
		{
			return !object.Equals(a, b);
		}

		/// <summary>Returns the generated hash code for this method.</summary>
		/// <returns>The hash code for this instance.</returns>
		// Token: 0x06005175 RID: 20853 RVA: 0x000FE9F2 File Offset: 0x000FCBF2
		public override int GetHashCode()
		{
			return this.tokValue;
		}

		/// <summary>Returns the metadata token for this method.</summary>
		/// <returns>Read-only. Returns the metadata token for this method.</returns>
		// Token: 0x17000D7B RID: 3451
		// (get) Token: 0x06005176 RID: 20854 RVA: 0x000FE9F2 File Offset: 0x000FCBF2
		public int Token
		{
			get
			{
				return this.tokValue;
			}
		}

		// Token: 0x040031D8 RID: 12760
		internal readonly int tokValue;

		/// <summary>The default <see langword="MethodToken" /> with <see cref="P:System.Reflection.Emit.MethodToken.Token" /> value 0.</summary>
		// Token: 0x040031D9 RID: 12761
		public static readonly MethodToken Empty;
	}
}
