using System;

namespace Unity.Entities
{
	// Token: 0x02000220 RID: 544
	[Obsolete("Use EntityQueryBuilder (UnityUpgradable) -> EntityQueryBuilder")]
	public struct EntityQueryDescBuilder
	{
		// Token: 0x06000D8F RID: 3471 RVA: 0x0003F1DE File Offset: 0x0003D3DE
		[Obsolete("Use WithOptions(EntityQueryOptions) (UnityUpgradable) -> EntityQueryBuilder.WithOptions(*)", false)]
		public EntityQueryDescBuilder Options(EntityQueryOptions options)
		{
			return this;
		}
	}
}
