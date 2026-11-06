using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Specifies the class identifier of a coclass imported from a type library.</summary>
	// Token: 0x0200070C RID: 1804
	[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
	[ComVisible(true)]
	public sealed class CoClassAttribute : Attribute
	{
		/// <summary>Initializes new instance of the <see cref="T:System.Runtime.InteropServices.CoClassAttribute" /> with the class identifier of the original coclass.</summary>
		/// <param name="coClass">A <see cref="T:System.Type" /> that contains the class identifier of the original coclass.</param>
		// Token: 0x060040B4 RID: 16564 RVA: 0x000E1584 File Offset: 0x000DF784
		public CoClassAttribute(Type coClass)
		{
			this._CoClass = coClass;
		}

		/// <summary>Gets the class identifier of the original coclass.</summary>
		/// <returns>A <see cref="T:System.Type" /> containing the class identifier of the original coclass.</returns>
		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x060040B5 RID: 16565 RVA: 0x000E1593 File Offset: 0x000DF793
		public Type CoClass
		{
			get
			{
				return this._CoClass;
			}
		}

		// Token: 0x04002AE4 RID: 10980
		internal Type _CoClass;
	}
}
