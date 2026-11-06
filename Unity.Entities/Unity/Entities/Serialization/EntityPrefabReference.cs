using System;
using UnityEngine;

namespace Unity.Entities.Serialization
{
	// Token: 0x02000385 RID: 901
	[Serializable]
	public struct EntityPrefabReference : IEquatable<EntityPrefabReference>
	{
		// Token: 0x060017F8 RID: 6136 RVA: 0x00059D98 File Offset: 0x00057F98
		internal EntityPrefabReference(UntypedWeakReferenceId prefabId)
		{
			this.Id = prefabId;
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x00059DA1 File Offset: 0x00057FA1
		public static bool operator ==(EntityPrefabReference left, EntityPrefabReference right)
		{
			return left.Equals(right);
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x00059DAB File Offset: 0x00057FAB
		public static bool operator !=(EntityPrefabReference left, EntityPrefabReference right)
		{
			return !left.Equals(right);
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x00059DB8 File Offset: 0x00057FB8
		public EntityPrefabReference(Hash128 guid)
		{
			this.Id = new UntypedWeakReferenceId(new RuntimeGlobalObjectId
			{
				AssetGUID = guid,
				IdentifierType = 1
			}, WeakReferenceGenerationType.EntityPrefab);
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060017FC RID: 6140 RVA: 0x00059DEA File Offset: 0x00057FEA
		public Hash128 AssetGUID
		{
			get
			{
				return this.Id.GlobalId.AssetGUID;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060017FD RID: 6141 RVA: 0x00059DFC File Offset: 0x00057FFC
		public bool IsReferenceValid
		{
			get
			{
				return this.Id.IsValid;
			}
		}

		// Token: 0x060017FE RID: 6142 RVA: 0x00059E0E File Offset: 0x0005800E
		public bool Equals(EntityPrefabReference other)
		{
			return this.Id.Equals(other.Id);
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x00059E24 File Offset: 0x00058024
		public override bool Equals(object obj)
		{
			if (obj is EntityPrefabReference)
			{
				EntityPrefabReference entityPrefabReference = (EntityPrefabReference)obj;
				return this.Equals(entityPrefabReference);
			}
			return false;
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x00059E49 File Offset: 0x00058049
		public override int GetHashCode()
		{
			return this.Id.GetHashCode();
		}

		// Token: 0x04000925 RID: 2341
		[SerializeField]
		internal UntypedWeakReferenceId Id;
	}
}
