using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	/// <summary>Indicates that the attributed type was previously defined in COM.</summary>
	// Token: 0x020006FE RID: 1790
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
	[ComVisible(true)]
	public sealed class ComImportAttribute : Attribute
	{
		// Token: 0x0600408B RID: 16523 RVA: 0x000E1196 File Offset: 0x000DF396
		internal static Attribute GetCustomAttribute(RuntimeType type)
		{
			if ((type.Attributes & TypeAttributes.Import) == TypeAttributes.NotPublic)
			{
				return null;
			}
			return new ComImportAttribute();
		}

		// Token: 0x0600408C RID: 16524 RVA: 0x000E11AD File Offset: 0x000DF3AD
		internal static bool IsDefined(RuntimeType type)
		{
			return (type.Attributes & TypeAttributes.Import) > TypeAttributes.NotPublic;
		}
	}
}
