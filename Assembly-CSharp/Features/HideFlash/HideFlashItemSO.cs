using System;
using Features.Detection;
using Features.Inventory;
using Invector.vCharacterController;
using UnityEngine;

namespace Features.HideFlash
{
	// Token: 0x020011AB RID: 4523
	[CreateAssetMenu(fileName = "Item_HideFlash_New", menuName = "Features/Inventory/Hide & Flash Item")]
	public class HideFlashItemSO : InventoryItemSO
	{
		// Token: 0x06008A9C RID: 35484 RVA: 0x0038A4EC File Offset: 0x003886EC
		public GameObject GetPropPrefabForBone(int boneIndex)
		{
			if (boneIndex == 0)
			{
				return this.propPrefab;
			}
			int num = boneIndex - 1;
			if (this.extraPropPrefabs == null || num >= this.extraPropPrefabs.Length)
			{
				return null;
			}
			return this.extraPropPrefabs[num];
		}

		// Token: 0x06008A9D RID: 35485 RVA: 0x0038A524 File Offset: 0x00388724
		public Vector3 GetPositionOffset(int boneIndex)
		{
			if (this.bonePositionOffsets != null && boneIndex < this.bonePositionOffsets.Length)
			{
				return this.bonePositionOffsets[boneIndex];
			}
			return Vector3.zero;
		}

		// Token: 0x06008A9E RID: 35486 RVA: 0x0038A54B File Offset: 0x0038874B
		public Vector3 GetRotationOffset(int boneIndex)
		{
			if (this.boneRotationOffsets != null && boneIndex < this.boneRotationOffsets.Length)
			{
				return this.boneRotationOffsets[boneIndex];
			}
			return Vector3.zero;
		}

		// Token: 0x04008D58 RID: 36184
		[Header("Hide & Flash — Prop")]
		[Tooltip("Mesh prefab spawned by HideFlashController. Leave handPrefab (base) empty — it is auto-cleared.")]
		public GameObject propPrefab;

		// Token: 0x04008D59 RID: 36185
		[Tooltip("Optional extra prop prefabs for multi-bone items: index i is spawned on attachBones[i+1] (e.g. 2BoxScrap: propPrefab = right scrap on RightHand, extraPropPrefabs[0] = left scrap on LeftHand). Leave empty for single-prop items.")]
		public GameObject[] extraPropPrefabs;

		// Token: 0x04008D5A RID: 36186
		[Tooltip("If true, the prop contains its own skeleton (e.g. BoxBase + flaps). The skeleton root named 'skeletonRootName' is extracted from the prefab and parented under the bone specified in attachBones[0], so the CielAnimatorController can drive it directly.")]
		public bool isSkinnedProp;

		// Token: 0x04008D5B RID: 36187
		[Tooltip("Name of the skeleton root GameObject inside propPrefab to extract and parent (only used when isSkinnedProp = true). E.g. 'BoxBase'.")]
		public string skeletonRootName = "BoxBase";

		// Token: 0x04008D5C RID: 36188
		[Header("Hide & Flash — Locomotion Type")]
		[Tooltip("Item type ID for the HideAndFlash layer branch. 0=None, 1=Box, 2=TrashCan, 3=Towel (worn), etc. Configure this ID in the CielAnimatorController HideAndFlash layer states.")]
		public int itemTypeId = 1;

		// Token: 0x04008D5D RID: 36189
		[Header("Hide & Flash — Concealment")]
		[Tooltip("Fraction of the body concealed while holding this item closed. 0 = no cover, 0.9 = 90% less visual detection. Removed for the entire flash, including its closing animation.")]
		[Range(0f, 0.95f)]
		public float coverage = 0.35f;

		// Token: 0x04008D5E RID: 36190
		[Header("Hide & Flash — Worn Detail Animator (e.g. Towel)")]
		[Tooltip("When true this item is NOT a hand prop but a garment ALREADY WORN on the body (e.g. the towel). Instead of spawning propPrefab, the controller resolves the worn mesh's own Animator and drives its blendshape clips in sync with the flash action. Leave false for all hand-prop items (Box, SchoolBag, Board, ...).")]
		public bool usesWornDetailAnimator;

		// Token: 0x04008D5F RID: 36191
		[Tooltip("Mesh root name of the worn garment whose detail Animator plays the garment-only clips (matched by name or StartsWith, tolerating a '(Clone)' suffix). E.g. 'Bath_Towel'.")]
		public string wornObjectName = "Bath_Towel";

		// Token: 0x04008D60 RID: 36192
		[Tooltip("Garment-only controller added at runtime to the worn mesh's Animator (e.g. TowelFlash_TowelOnly).")]
		public RuntimeAnimatorController detailOnlyController;

		// Token: 0x04008D61 RID: 36193
		[Tooltip("Trigger set on the worn detail Animator when the flash OPENS (start).")]
		public string detailStartTrigger = "TowelFlashStart";

		// Token: 0x04008D62 RID: 36194
		[Tooltip("Trigger set on the worn detail Animator when the flash CLOSES (end).")]
		public string detailEndTrigger = "TowelFlashEnd";

		// Token: 0x04008D63 RID: 36195
		[Header("Hide & Flash — Pickup Action")]
		[Tooltip("Optional: Invector PlayerActionEntry for pickup animation. If set, equip waits for this animation to complete before showing the prop.")]
		public PlayerActionEntry pickupActionEntry;

		// Token: 0x04008D64 RID: 36196
		[Header("Hide & Flash — Flash Action")]
		[Tooltip("Invector PlayerActionEntry for the flash animation (same pattern as DildoActionDefinition).")]
		public PlayerActionEntry flashActionEntry;

		// Token: 0x04008D65 RID: 36197
		[Tooltip("If true, the flash loops until input is released. If false, plays once.")]
		public bool flashIsLoop;

		// Token: 0x04008D66 RID: 36198
		[Header("Hide & Flash — Flash Input")]
		[Tooltip("Input to trigger the flash sequence while holding this item.")]
		public GenericInput flashInput = new GenericInput("Q", "B", "B");

		// Token: 0x04008D67 RID: 36199
		[Header("Hide & Flash — Attach Bones")]
		[Tooltip("Bones used to anchor the prop. Usually RightHand only, or both hands for large boxes.")]
		public HumanBodyBones[] attachBones = new HumanBodyBones[] { HumanBodyBones.RightHand };

		// Token: 0x04008D68 RID: 36200
		[Header("Hide & Flash — Attach Overrides")]
		[Tooltip("Local position offset applied to each spawned prop instance (index matches attachBones).")]
		public Vector3[] bonePositionOffsets;

		// Token: 0x04008D69 RID: 36201
		[Tooltip("Local rotation offset applied to each spawned prop instance (index matches attachBones).")]
		public Vector3[] boneRotationOffsets;

		// Token: 0x04008D6A RID: 36202
		[Header("Hide & Flash — Audio")]
		public AudioClip flashStartSFX;

		// Token: 0x04008D6B RID: 36203
		public AudioClip flashEndSFX;
	}
}
