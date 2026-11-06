using System;

namespace System.Diagnostics.Tracing
{
	/// <summary>Specifies a type to be passed to the <see cref="M:System.Diagnostics.Tracing.EventSource.Write``1(System.String,System.Diagnostics.Tracing.EventSourceOptions,``0)" /> method.</summary>
	// Token: 0x020009F0 RID: 2544
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
	public class EventDataAttribute : Attribute
	{
		/// <summary>Gets or sets the name to apply to an event if the event type or property is not explicitly named.</summary>
		/// <returns>The name to apply to the event or property.</returns>
		// Token: 0x17000F8B RID: 3979
		// (get) Token: 0x06005ABC RID: 23228 RVA: 0x000479FC File Offset: 0x00045BFC
		// (set) Token: 0x06005ABD RID: 23229 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public string Name
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}
	}
}
