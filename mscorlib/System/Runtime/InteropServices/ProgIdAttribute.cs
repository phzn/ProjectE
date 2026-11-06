using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Allows the user to specify the ProgID of a class.</summary>
	// Token: 0x020006F0 RID: 1776
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	[ComVisible(true)]
	public sealed class ProgIdAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see langword="ProgIdAttribute" /> with the specified ProgID.</summary>
		/// <param name="progId">The ProgID to be assigned to the class.</param>
		// Token: 0x06004074 RID: 16500 RVA: 0x000E1009 File Offset: 0x000DF209
		public ProgIdAttribute(string progId)
		{
			this._val = progId;
		}

		/// <summary>Gets the ProgID of the class.</summary>
		/// <returns>The ProgID of the class.</returns>
		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x06004075 RID: 16501 RVA: 0x000E1018 File Offset: 0x000DF218
		public string Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04002A3D RID: 10813
		internal string _val;
	}
}
