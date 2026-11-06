using System;

namespace Unity.Serialization.Json
{
	// Token: 0x0200003C RID: 60
	public interface IJsonMigration<TValue> : IJsonMigration
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000152 RID: 338
		int Version { get; }

		// Token: 0x06000153 RID: 339
		TValue Migrate(in JsonMigrationContext context);
	}
}
