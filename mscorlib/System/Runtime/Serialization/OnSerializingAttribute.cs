using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	/// <summary>When applied to a method, specifies that the method is during serialization of an object in an object graph. The order of serialization relative to other objects in the graph is non-deterministic.</summary>
	// Token: 0x0200066D RID: 1645
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public sealed class OnSerializingAttribute : Attribute
	{
	}
}
