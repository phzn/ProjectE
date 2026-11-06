using System;
using UnityEngine.Scripting.APIUpdating;

namespace Unity.Jobs
{
	// Token: 0x02000017 RID: 23
	[MovedFrom(true, "Unity.Entities", "Unity.Entities", null)]
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public class RegisterGenericJobTypeAttribute : Attribute
	{
		// Token: 0x06000047 RID: 71 RVA: 0x000027C5 File Offset: 0x000009C5
		public RegisterGenericJobTypeAttribute(Type type)
		{
			this.ConcreteType = type;
		}

		// Token: 0x0400000D RID: 13
		public Type ConcreteType;
	}
}
