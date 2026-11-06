using System;
using System.Reflection;

namespace Internal.Runtime.Augments
{
	// Token: 0x020000C7 RID: 199
	internal class ReflectionExecutionDomainCallbacks
	{
		// Token: 0x060004B8 RID: 1208 RVA: 0x00017849 File Offset: 0x00015A49
		internal Exception CreateMissingMetadataException(Type attributeType)
		{
			return new MissingMetadataException();
		}
	}
}
