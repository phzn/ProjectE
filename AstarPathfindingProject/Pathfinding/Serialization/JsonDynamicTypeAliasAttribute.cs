using System;

namespace Pathfinding.Serialization
{
	// Token: 0x02000250 RID: 592
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class JsonDynamicTypeAliasAttribute : Attribute
	{
		// Token: 0x06000EC2 RID: 3778 RVA: 0x0005AA93 File Offset: 0x00058C93
		public JsonDynamicTypeAliasAttribute(string alias, Type type)
		{
			this.alias = alias;
			this.type = type;
		}

		// Token: 0x04000B15 RID: 2837
		public string alias;

		// Token: 0x04000B16 RID: 2838
		public Type type;
	}
}
