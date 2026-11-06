using System;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000B16 RID: 2838
	public class PrefabCustomizationSpawnProvider : MonoBehaviour, ICustomizationSpawnProvider
	{
		// Token: 0x17000D5A RID: 3418
		// (get) Token: 0x0600504F RID: 20559 RVA: 0x001F1947 File Offset: 0x001EFB47
		public CustomizationTargetType TargetType
		{
			get
			{
				return this.targetType;
			}
		}

		// Token: 0x17000D5B RID: 3419
		// (get) Token: 0x06005050 RID: 20560 RVA: 0x001F194F File Offset: 0x001EFB4F
		public bool HasValidPrefab
		{
			get
			{
				return this.preSpawnedTarget != null || (this.allowLegacyPrefabInstantiateFallback && this.prefab != null);
			}
		}

		// Token: 0x06005051 RID: 20561 RVA: 0x001F1977 File Offset: 0x001EFB77
		public bool CanSpawn(CustomizationSessionContext context)
		{
			return context != null && context.targetType == this.targetType && this.HasValidPrefab;
		}

		// Token: 0x06005052 RID: 20562 RVA: 0x001F1994 File Offset: 0x001EFB94
		public GameObject Spawn(CustomizationSessionContext context, Transform fallbackSpawnPoint)
		{
			if (this.preSpawnedTarget != null)
			{
				this.preSpawnedTarget.SetActive(true);
				return this.preSpawnedTarget;
			}
			if (!this.allowLegacyPrefabInstantiateFallback || this.prefab == null)
			{
				return null;
			}
			Transform transform = ((context != null && context.spawnPoint != null) ? context.spawnPoint : fallbackSpawnPoint);
			if (transform != null)
			{
				return Object.Instantiate<GameObject>(this.prefab, transform.position, transform.rotation);
			}
			return Object.Instantiate<GameObject>(this.prefab);
		}

		// Token: 0x06005053 RID: 20563 RVA: 0x001F1A24 File Offset: 0x001EFC24
		public void Despawn(GameObject runtimeInstance)
		{
			if (runtimeInstance == null)
			{
				if (this.preSpawnedTarget != null)
				{
					this.preSpawnedTarget.SetActive(false);
				}
				return;
			}
			if (runtimeInstance == this.preSpawnedTarget || !this.allowLegacyPrefabInstantiateFallback)
			{
				runtimeInstance.SetActive(false);
				return;
			}
			Object.Destroy(runtimeInstance);
		}

		// Token: 0x06005054 RID: 20564 RVA: 0x001F1A79 File Offset: 0x001EFC79
		public void DeactivatePreview()
		{
			if (this.preSpawnedTarget != null)
			{
				this.preSpawnedTarget.SetActive(false);
			}
		}

		// Token: 0x17000D5C RID: 3420
		// (get) Token: 0x06005055 RID: 20565 RVA: 0x001F1A95 File Offset: 0x001EFC95
		public GameObject PreSpawnedTarget
		{
			get
			{
				return this.preSpawnedTarget;
			}
		}

		// Token: 0x04005116 RID: 20758
		[SerializeField]
		private CustomizationTargetType targetType = CustomizationTargetType.MainCharacter;

		// Token: 0x04005117 RID: 20759
		[Header("Pre-Spawned Target")]
		[SerializeField]
		private GameObject preSpawnedTarget;

		// Token: 0x04005118 RID: 20760
		[Header("Legacy Fallback")]
		[SerializeField]
		private GameObject prefab;

		// Token: 0x04005119 RID: 20761
		[SerializeField]
		private bool allowLegacyPrefabInstantiateFallback;
	}
}
