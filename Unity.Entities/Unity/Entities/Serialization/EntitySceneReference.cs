using System;
using UnityEngine;

namespace Unity.Entities.Serialization
{
	// Token: 0x02000386 RID: 902
	[Serializable]
	public struct EntitySceneReference : IEquatable<EntitySceneReference>
	{
		// Token: 0x06001801 RID: 6145 RVA: 0x00059E5C File Offset: 0x0005805C
		internal EntitySceneReference(UntypedWeakReferenceId sceneId)
		{
			this.Id = sceneId;
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x00059E68 File Offset: 0x00058068
		public EntitySceneReference(Hash128 guid, int sectionIndex)
		{
			this.Id = new UntypedWeakReferenceId(new RuntimeGlobalObjectId
			{
				AssetGUID = guid,
				IdentifierType = 1,
				SceneObjectIdentifier0 = (long)sectionIndex
			}, WeakReferenceGenerationType.EntityScene);
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06001803 RID: 6147 RVA: 0x00059EA3 File Offset: 0x000580A3
		public bool IsReferenceValid
		{
			get
			{
				return this.Id.IsValid;
			}
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x00059EB5 File Offset: 0x000580B5
		public bool Equals(EntitySceneReference other)
		{
			return this.Id.Equals(other.Id);
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x00059EC8 File Offset: 0x000580C8
		public override bool Equals(object obj)
		{
			if (obj is EntitySceneReference)
			{
				EntitySceneReference entitySceneReference = (EntitySceneReference)obj;
				return this.Equals(entitySceneReference);
			}
			return false;
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x00059EED File Offset: 0x000580ED
		public override int GetHashCode()
		{
			return this.Id.GetHashCode();
		}

		// Token: 0x04000926 RID: 2342
		[SerializeField]
		internal UntypedWeakReferenceId Id;
	}
}
