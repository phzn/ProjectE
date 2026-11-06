using System;
using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using MagicaCloth2;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;

namespace ProjectExhibitionist.PhotoBooth
{
	// Token: 0x02000976 RID: 2422
	[DefaultExecutionOrder(-500)]
	[DisallowMultipleComponent]
	public sealed class PhotoBoothCurtainPhysics : MonoBehaviour
	{
		// Token: 0x0600429A RID: 17050 RVA: 0x0018DE12 File Offset: 0x0018C012
		public void Configure(MagicaCloth target)
		{
			this.curtain = target;
		}

		// Token: 0x0600429B RID: 17051 RVA: 0x0018DE1B File Offset: 0x0018C01B
		private void OnEnable()
		{
			this.DisableLegacyEntranceBlockers();
		}

		// Token: 0x0600429C RID: 17052 RVA: 0x0018DE24 File Offset: 0x0018C024
		private void Awake()
		{
			if (this.curtain == null)
			{
				this.curtain = base.GetComponent<MagicaCloth>();
			}
			if (this.curtain != null)
			{
				this.curtain.Initialize();
				this.curtain.DisableAutoBuild();
			}
			this.DisableLegacyEntranceBlockers();
		}

		// Token: 0x0600429D RID: 17053 RVA: 0x0018DE78 File Offset: 0x0018C078
		private void DisableLegacyEntranceBlockers()
		{
			foreach (Collider collider in base.GetComponentsInChildren<Collider>(true))
			{
				if (collider != null && !collider.isTrigger)
				{
					collider.enabled = false;
				}
			}
			PhotoBoothController componentInParent = base.GetComponentInParent<PhotoBoothController>();
			Transform transform = ((componentInParent != null) ? componentInParent.transform : base.transform.parent);
			if (transform == null)
			{
				return;
			}
			foreach (MeshCollider meshCollider in transform.GetComponentsInChildren<MeshCollider>(true))
			{
				if (meshCollider != null && meshCollider.transform.name == "00_Booth_A_Main")
				{
					meshCollider.enabled = false;
				}
			}
		}

		// Token: 0x0600429E RID: 17054 RVA: 0x0018DF31 File Offset: 0x0018C131
		private IEnumerator Start()
		{
			if (this.curtain == null)
			{
				yield break;
			}
			vThirdPersonController player = null;
			while (player == null)
			{
				if (PlayerRegistry.Player != null)
				{
					player = PlayerRegistry.Player.GetComponentInChildren<vThirdPersonController>(true);
				}
				if (player == null)
				{
					player = Object.FindObjectOfType<vThirdPersonController>();
				}
				if (player == null)
				{
					yield return null;
				}
			}
			this.BindPlayerColliders(player);
			this.curtain.enabled = true;
			if (this.curtain.BuildAndRun())
			{
				ClothQualityService.Register(this.curtain);
			}
			else
			{
				Debug.LogError(string.Concat(new string[]
				{
					"[PhotoBoothCurtain] Magica Cloth runtime build failed for '",
					base.transform.root.name,
					"/",
					base.name,
					"'."
				}), this);
			}
			yield break;
		}

		// Token: 0x0600429F RID: 17055 RVA: 0x0018DF40 File Offset: 0x0018C140
		private void BindPlayerColliders(vThirdPersonController player)
		{
			List<ColliderComponent> colliderList = this.curtain.SerializeData.colliderCollisionConstraint.colliderList;
			colliderList.Clear();
			HashSet<int> hashSet = new HashSet<int>();
			foreach (ColliderComponent colliderComponent in player.GetComponentsInChildren<ColliderComponent>(true))
			{
				if (!(colliderComponent == null) && colliderComponent.enabled && colliderComponent.gameObject.activeInHierarchy && hashSet.Add(colliderComponent.GetInstanceID()))
				{
					colliderList.Add(colliderComponent);
				}
			}
			if (colliderList.Count > 0 || !this.createFallbackBodyCollider)
			{
				return;
			}
			this._fallbackCollider = player.GetComponent<MagicaCapsuleCollider>();
			if (this._fallbackCollider == null)
			{
				this._fallbackCollider = player.gameObject.AddComponent<MagicaCapsuleCollider>();
			}
			this._fallbackCollider.direction = MagicaCapsuleCollider.Direction.Y;
			this._fallbackCollider.alignedOnCenter = true;
			this._fallbackCollider.center = new Vector3(0f, 0.9f, 0f);
			this._fallbackCollider.SetSize(0.32f, 0.32f, 1.15f);
			colliderList.Add(this._fallbackCollider);
		}

		// Token: 0x040042B6 RID: 17078
		[SerializeField]
		private MagicaCloth curtain;

		// Token: 0x040042B7 RID: 17079
		[SerializeField]
		private bool createFallbackBodyCollider = true;

		// Token: 0x040042B8 RID: 17080
		private MagicaCapsuleCollider _fallbackCollider;
	}
}
