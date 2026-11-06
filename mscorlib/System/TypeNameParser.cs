using System;
using System.Reflection;
using System.Threading;

namespace System
{
	// Token: 0x02000222 RID: 546
	internal sealed class TypeNameParser
	{
		// Token: 0x0600186F RID: 6255 RVA: 0x0005D84C File Offset: 0x0005BA4C
		internal static Type GetType(string typeName, Func<AssemblyName, Assembly> assemblyResolver, Func<Assembly, string, bool, Type> typeResolver, bool throwOnError, bool ignoreCase, ref StackCrawlMark stackMark)
		{
			return TypeSpec.Parse(typeName).Resolve(assemblyResolver, typeResolver, throwOnError, ignoreCase, ref stackMark);
		}
	}
}
