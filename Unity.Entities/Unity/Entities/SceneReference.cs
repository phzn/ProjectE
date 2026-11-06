using System;
using Unity.Entities.Serialization;

namespace Unity.Entities
{
	// Token: 0x02000317 RID: 791
	public struct SceneReference : IComponentData, IQueryTypeParameter, IEquatable<SceneReference>
	{
		// Token: 0x0600153F RID: 5439 RVA: 0x000508C9 File Offset: 0x0004EAC9
		public SceneReference(EntitySceneReference sceneReference)
		{
			this.SceneGUID = sceneReference.Id.GlobalId.AssetGUID;
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x000508E1 File Offset: 0x0004EAE1
		public bool Equals(SceneReference other)
		{
			return this.SceneGUID.Equals(other.SceneGUID);
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x000508F4 File Offset: 0x0004EAF4
		public override int GetHashCode()
		{
			return this.SceneGUID.GetHashCode();
		}

		// Token: 0x040007B7 RID: 1975
		public Hash128 SceneGUID;
	}
}
