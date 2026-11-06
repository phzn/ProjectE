using System;
using Features.Inventory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.Headphones
{
	// Token: 0x020009D2 RID: 2514
	[DisallowMultipleComponent]
	public sealed class HeadphonesEquipment : MonoBehaviour
	{
		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x060046AE RID: 18094 RVA: 0x001AB5A5 File Offset: 0x001A97A5
		public static HeadphonesEquipment Current
		{
			get
			{
				return HeadphonesEquipment._current;
			}
		}

		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x060046AF RID: 18095 RVA: 0x001AB5AC File Offset: 0x001A97AC
		public static bool IsWorn
		{
			get
			{
				return HeadphonesEquipment._current != null && HeadphonesEquipment._current.HasHeadphones;
			}
		}

		// Token: 0x14000077 RID: 119
		// (add) Token: 0x060046B0 RID: 18096 RVA: 0x001AB5C8 File Offset: 0x001A97C8
		// (remove) Token: 0x060046B1 RID: 18097 RVA: 0x001AB5FC File Offset: 0x001A97FC
		public static event Action<bool> WearStateChanged;

		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x060046B2 RID: 18098 RVA: 0x001AB62F File Offset: 0x001A982F
		public bool HasHeadphones
		{
			get
			{
				return this.equippedRoot != null;
			}
		}

		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x060046B3 RID: 18099 RVA: 0x001AB63D File Offset: 0x001A983D
		public Transform EquippedRoot
		{
			get
			{
				return this.equippedRoot;
			}
		}

		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x060046B4 RID: 18100 RVA: 0x001AB645 File Offset: 0x001A9845
		public Transform AttachmentBone
		{
			get
			{
				return this.attachmentBone;
			}
		}

		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x060046B5 RID: 18101 RVA: 0x001AB64D File Offset: 0x001A984D
		public HeadphonesFitProfileSO FitProfile
		{
			get
			{
				return this.fitProfile;
			}
		}

		// Token: 0x060046B6 RID: 18102 RVA: 0x001AB655 File Offset: 0x001A9855
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			HeadphonesEquipment._current = null;
			HeadphonesEquipment.WearStateChanged = null;
		}

		// Token: 0x060046B7 RID: 18103 RVA: 0x001AB663 File Offset: 0x001A9863
		private void Awake()
		{
			HeadphonesEquipment._current = this;
		}

		// Token: 0x060046B8 RID: 18104 RVA: 0x001AB66B File Offset: 0x001A986B
		private void OnDestroy()
		{
			if (HeadphonesEquipment._current != this)
			{
				return;
			}
			bool hasHeadphones = this.HasHeadphones;
			HeadphonesEquipment._current = null;
			if (hasHeadphones)
			{
				Action<bool> wearStateChanged = HeadphonesEquipment.WearStateChanged;
				if (wearStateChanged == null)
				{
					return;
				}
				wearStateChanged(false);
			}
		}

		// Token: 0x060046B9 RID: 18105 RVA: 0x001AB69C File Offset: 0x001A989C
		public static HeadphonesEquipment GetOrAdd(GameObject playerRoot)
		{
			if (playerRoot == null)
			{
				return null;
			}
			HeadphonesEquipment headphonesEquipment = playerRoot.GetComponentInChildren<HeadphonesEquipment>(true);
			if (headphonesEquipment == null)
			{
				headphonesEquipment = playerRoot.AddComponent<HeadphonesEquipment>();
			}
			HeadphonesMusicController.GetOrAdd(playerRoot);
			return headphonesEquipment;
		}

		// Token: 0x060046BA RID: 18106 RVA: 0x001AB6D4 File Offset: 0x001A98D4
		public bool TryEquip(Transform headphonesRoot, HeadphonesFitProfileSO profile)
		{
			if (headphonesRoot == null || this.HasHeadphones)
			{
				return false;
			}
			string text = ((profile != null) ? profile.preferredBoneName : "head.x");
			Transform transform = this.ResolveAttachmentBone(text);
			if (transform == null)
			{
				Debug.LogError("[Headphones] Ciel head bone was not found; keeping the pickup in the world.", this);
				return false;
			}
			headphonesRoot.SetParent(null, true);
			Scene scene = base.gameObject.scene;
			if (scene.IsValid() && scene.isLoaded && headphonesRoot.gameObject.scene != scene)
			{
				SceneManager.MoveGameObjectToScene(headphonesRoot.gameObject, scene);
			}
			headphonesRoot.SetParent(transform, false);
			headphonesRoot.name = "Headphones_Worn";
			this.equippedRoot = headphonesRoot;
			this.attachmentBone = transform;
			this.fitProfile = profile;
			this.ApplyFit();
			int num = LayerMask.NameToLayer((profile != null) ? profile.wornLayerName : "PlayerClothing");
			if (num < 0)
			{
				num = transform.gameObject.layer;
			}
			HeadphonesEquipment.SetLayerRecursively(headphonesRoot, num);
			HeadphonesEquipment._current = this;
			Action<bool> wearStateChanged = HeadphonesEquipment.WearStateChanged;
			if (wearStateChanged != null)
			{
				wearStateChanged(true);
			}
			return true;
		}

		// Token: 0x060046BB RID: 18107 RVA: 0x001AB7E8 File Offset: 0x001A99E8
		public bool TryDropTowardsScreenPoint(Vector2 screenPoint)
		{
			if (!this.HasHeadphones)
			{
				return false;
			}
			Transform transform = this.equippedRoot;
			HeadphonesPickup component = transform.GetComponent<HeadphonesPickup>();
			if (component == null)
			{
				Debug.LogError("[Headphones] The worn visual has no HeadphonesPickup, so it cannot be returned safely to the world.", this);
				return false;
			}
			Scene activeScene = SceneManager.GetActiveScene();
			if (!activeScene.IsValid() || !activeScene.isLoaded)
			{
				Debug.LogWarning("[Headphones] No loaded active scene is available for this drop.", this);
				return false;
			}
			InventoryDropController instance = InventoryDropController.Instance;
			Vector3 vector;
			if (instance != null)
			{
				vector = instance.ResolveCursorDropSpot(screenPoint);
			}
			else
			{
				Vector3 vector2 = Vector3.ProjectOnPlane(base.transform.forward, Vector3.up);
				if (vector2.sqrMagnitude < 0.0001f)
				{
					vector2 = Vector3.forward;
				}
				vector = base.transform.position + vector2.normalized * 0.8f;
			}
			this.equippedRoot = null;
			this.attachmentBone = null;
			this.fitProfile = null;
			transform.SetParent(null, true);
			if (transform.gameObject.scene != activeScene)
			{
				SceneManager.MoveGameObjectToScene(transform.gameObject, activeScene);
			}
			component.RestoreToWorldAndBeginDrop(vector, base.gameObject);
			Action<bool> wearStateChanged = HeadphonesEquipment.WearStateChanged;
			if (wearStateChanged != null)
			{
				wearStateChanged(false);
			}
			return true;
		}

		// Token: 0x060046BC RID: 18108 RVA: 0x001AB910 File Offset: 0x001A9B10
		public void ApplyFit()
		{
			if (this.equippedRoot == null || this.attachmentBone == null)
			{
				return;
			}
			if (this.equippedRoot.parent != this.attachmentBone)
			{
				this.equippedRoot.SetParent(this.attachmentBone, false);
			}
			if (this.fitProfile == null)
			{
				return;
			}
			this.equippedRoot.localPosition = this.fitProfile.localPosition;
			this.equippedRoot.localRotation = this.fitProfile.LocalRotation;
			this.equippedRoot.localScale = this.fitProfile.localScale;
		}

		// Token: 0x060046BD RID: 18109 RVA: 0x001AB9B8 File Offset: 0x001A9BB8
		public Transform ResolveAttachmentBone(string preferredBoneName)
		{
			Transform transform = HeadphonesEquipment.FindNamedBone(base.transform, preferredBoneName);
			if (transform != null)
			{
				return transform;
			}
			foreach (Animator animator in base.GetComponentsInChildren<Animator>(true))
			{
				if (!(animator == null) && !(animator.avatar == null) && animator.avatar.isValid && animator.avatar.isHuman)
				{
					Transform boneTransform = animator.GetBoneTransform(HumanBodyBones.Head);
					if (boneTransform != null)
					{
						return boneTransform;
					}
				}
			}
			transform = HeadphonesEquipment.FindNamedBone(base.transform, "head.x");
			if (!(transform != null))
			{
				return HeadphonesEquipment.FindNamedBone(base.transform, "Head");
			}
			return transform;
		}

		// Token: 0x060046BE RID: 18110 RVA: 0x001ABA6C File Offset: 0x001A9C6C
		private static Transform FindNamedBone(Transform root, string boneName)
		{
			if (root == null || string.IsNullOrWhiteSpace(boneName))
			{
				return null;
			}
			foreach (Transform transform in root.GetComponentsInChildren<Transform>(true))
			{
				if (transform != null && string.Equals(transform.name, boneName, StringComparison.OrdinalIgnoreCase))
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x060046BF RID: 18111 RVA: 0x001ABAC4 File Offset: 0x001A9CC4
		private static void SetLayerRecursively(Transform root, int layer)
		{
			if (root == null || layer < 0)
			{
				return;
			}
			root.gameObject.layer = layer;
			for (int i = 0; i < root.childCount; i++)
			{
				HeadphonesEquipment.SetLayerRecursively(root.GetChild(i), layer);
			}
		}

		// Token: 0x04004647 RID: 17991
		private static HeadphonesEquipment _current;

		// Token: 0x04004648 RID: 17992
		[SerializeField]
		private Transform equippedRoot;

		// Token: 0x04004649 RID: 17993
		[SerializeField]
		private Transform attachmentBone;

		// Token: 0x0400464A RID: 17994
		[SerializeField]
		private HeadphonesFitProfileSO fitProfile;
	}
}
