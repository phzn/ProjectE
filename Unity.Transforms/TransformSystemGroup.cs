using System;
using Unity.Entities;
using UnityEngine.Scripting;

namespace Unity.Transforms
{
	// Token: 0x02000004 RID: 4
	[WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor, WorldSystemFilterFlags.Default)]
	public class TransformSystemGroup : ComponentSystemGroup
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020BF File Offset: 0x000002BF
		[Preserve]
		public TransformSystemGroup()
		{
		}
	}
}
