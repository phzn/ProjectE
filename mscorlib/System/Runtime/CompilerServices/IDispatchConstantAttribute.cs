using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	/// <summary>Indicates that the default value for the attributed field or parameter is an instance of <see cref="T:System.Runtime.InteropServices.DispatchWrapper" />, where the <see cref="P:System.Runtime.InteropServices.DispatchWrapper.WrappedObject" /> is <see langword="null" />.</summary>
	// Token: 0x02000835 RID: 2101
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
	[Serializable]
	public sealed class IDispatchConstantAttribute : CustomConstantAttribute
	{
		/// <summary>Gets the <see langword="IDispatch" /> constant stored in this attribute.</summary>
		/// <returns>The <see langword="IDispatch" /> constant stored in this attribute. Only <see langword="null" /> is allowed for an <see langword="IDispatch" /> constant value.</returns>
		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x060046B7 RID: 18103 RVA: 0x000E7132 File Offset: 0x000E5332
		public override object Value
		{
			get
			{
				return new DispatchWrapper(null);
			}
		}
	}
}
