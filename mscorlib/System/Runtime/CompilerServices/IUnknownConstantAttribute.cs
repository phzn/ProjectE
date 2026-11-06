using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	/// <summary>Indicates that the default value for the attributed field or parameter is an instance of <see cref="T:System.Runtime.InteropServices.UnknownWrapper" />, where the <see cref="P:System.Runtime.InteropServices.UnknownWrapper.WrappedObject" /> is <see langword="null" />. This class cannot be inherited.</summary>
	// Token: 0x02000842 RID: 2114
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class IUnknownConstantAttribute : CustomConstantAttribute
	{
		/// <summary>Gets the <see langword="IUnknown" /> constant stored in this attribute.</summary>
		/// <returns>The <see langword="IUnknown" /> constant stored in this attribute. Only <see langword="null" /> is allowed for an <see langword="IUnknown" /> constant value.</returns>
		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x060046BE RID: 18110 RVA: 0x000E7169 File Offset: 0x000E5369
		public override object Value
		{
			get
			{
				return new UnknownWrapper(null);
			}
		}
	}
}
