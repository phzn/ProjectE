using System;

namespace Unity.Serialization.Json
{
	// Token: 0x0200003D RID: 61
	public interface IContravariantJsonMigration<in TValue> : IJsonMigration
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000154 RID: 340
		int Version { get; }

		// Token: 0x06000155 RID: 341
		object Migrate(in JsonMigrationContext context);
	}
}
