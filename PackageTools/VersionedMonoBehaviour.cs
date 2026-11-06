using System;
using Pathfinding.Drawing;
using Pathfinding.Serialization;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000006 RID: 6
	public abstract class VersionedMonoBehaviour : MonoBehaviourGizmos, ISerializationCallbackReceiver, IVersionedMonoBehaviourInternal, IEntityIndex
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000020CE File Offset: 0x000002CE
		// (set) Token: 0x06000007 RID: 7 RVA: 0x000020D6 File Offset: 0x000002D6
		int IEntityIndex.EntityIndex { get; set; }

		// Token: 0x06000008 RID: 8 RVA: 0x000020E0 File Offset: 0x000002E0
		protected virtual void Awake()
		{
			if (Application.isPlaying)
			{
				if (this.version == 0)
				{
					Migrations migrations = new Migrations(int.MaxValue);
					this.OnUpgradeSerializedData(ref migrations, true);
					this.version = migrations.allMigrations;
					return;
				}
				((IVersionedMonoBehaviourInternal)this).UpgradeFromUnityThread();
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002124 File Offset: 0x00000324
		protected virtual void Reset()
		{
			Migrations migrations = new Migrations(int.MaxValue);
			this.OnUpgradeSerializedData(ref migrations, true);
			this.version = migrations.allMigrations;
			this.DisableGizmosIcon();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002158 File Offset: 0x00000358
		private void DisableGizmosIcon()
		{
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000215A File Offset: 0x0000035A
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000215C File Offset: 0x0000035C
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			this.UpgradeSerializedData(false);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002168 File Offset: 0x00000368
		protected void UpgradeSerializedData(bool isUnityThread)
		{
			Migrations migrations = new Migrations(this.version);
			this.OnUpgradeSerializedData(ref migrations, isUnityThread);
			if (migrations.ignore)
			{
				return;
			}
			if (migrations.IsLegacyFormat)
			{
				throw new Exception("Failed to migrate from the legacy format");
			}
			if ((migrations.finishedMigrations & ~(migrations.allMigrations != 0)) != 0)
			{
				throw new Exception("Run more migrations than there are migrations to run. Finished: " + migrations.finishedMigrations.ToString("X") + " all: " + migrations.allMigrations.ToString("X"));
			}
			if (isUnityThread && (migrations.allMigrations & ~(migrations.finishedMigrations != 0)) != 0)
			{
				throw new Exception("Some migrations were registered, but they did not run. Finished: " + migrations.finishedMigrations.ToString("X") + " all: " + migrations.allMigrations.ToString("X"));
			}
			this.version = migrations.finishedMigrations;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002248 File Offset: 0x00000448
		protected virtual void OnUpgradeSerializedData(ref Migrations migrations, bool unityThread)
		{
			int num;
			if (migrations.TryMigrateFromLegacyFormat(out num) && num > 1)
			{
				throw new Exception("Reached base class without having migrated the legacy format, and the legacy version is not version 1.");
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000226E File Offset: 0x0000046E
		void IVersionedMonoBehaviourInternal.UpgradeFromUnityThread()
		{
			this.UpgradeSerializedData(true);
		}

		// Token: 0x04000002 RID: 2
		[SerializeField]
		[HideInInspector]
		private int version;
	}
}
