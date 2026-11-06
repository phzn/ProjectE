using System;

namespace Pathfinding.Serialization
{
	// Token: 0x02000008 RID: 8
	public struct Migrations
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000013 RID: 19 RVA: 0x0000227F File Offset: 0x0000047F
		public bool IsLegacyFormat
		{
			get
			{
				return (this.finishedMigrations & 1073741824) == 0;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002290 File Offset: 0x00000490
		public int LegacyVersion
		{
			get
			{
				return this.finishedMigrations;
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002298 File Offset: 0x00000498
		public Migrations(int value)
		{
			this.finishedMigrations = value;
			this.allMigrations = 1073741824;
			this.ignore = false;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000022B3 File Offset: 0x000004B3
		public bool TryMigrateFromLegacyFormat(out int legacyVersion)
		{
			legacyVersion = this.finishedMigrations;
			if (this.IsLegacyFormat)
			{
				this = new Migrations(1073741824);
				return true;
			}
			return false;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000022D8 File Offset: 0x000004D8
		public void MarkMigrationFinished(int flag)
		{
			if (this.IsLegacyFormat)
			{
				throw new InvalidOperationException("Version must first be migrated to the bitfield format");
			}
			this.finishedMigrations |= flag;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000022FB File Offset: 0x000004FB
		public bool AddAndMaybeRunMigration(int flag, bool filter = true)
		{
			if ((flag & 1073741824) != 0)
			{
				throw new ArgumentException("Cannot use the MIGRATE_TO_BITFIELD flag when adding a migration");
			}
			this.allMigrations |= flag;
			if (filter)
			{
				bool flag2 = (this.finishedMigrations & flag) != flag;
				this.MarkMigrationFinished(flag);
				return flag2;
			}
			return false;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002339 File Offset: 0x00000539
		public void IgnoreMigrationAttempt()
		{
			this.ignore = true;
		}

		// Token: 0x04000004 RID: 4
		internal int finishedMigrations;

		// Token: 0x04000005 RID: 5
		internal int allMigrations;

		// Token: 0x04000006 RID: 6
		internal bool ignore;

		// Token: 0x04000007 RID: 7
		private const int MIGRATE_TO_BITFIELD = 1073741824;
	}
}
