using System;

namespace Unity.Entities
{
	// Token: 0x0200031C RID: 796
	public struct RequestSceneLoaded : IComponentData, IQueryTypeParameter
	{
		// Token: 0x040007C3 RID: 1987
		public SceneLoadFlags LoadFlags;
	}
}
