using System;
using System.Collections.Generic;
using System.Linq;
using MagicaCloth2;
using ProjectExhibitionist.Core.Rendering;
using UnityEngine;
using UnityEngine.Events;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008CA RID: 2250
	public class BoZo_MagicaClothSupport : MonoBehaviour, IOutfitExtension<Texture2D>, IOutfitExtension
	{
		// Token: 0x06003C90 RID: 15504 RVA: 0x001685CB File Offset: 0x001667CB
		public Texture2D GetValue()
		{
			return this.influenceMap;
		}

		// Token: 0x06003C91 RID: 15505 RVA: 0x001685CB File Offset: 0x001667CB
		object IOutfitExtension.GetValue()
		{
			return this.influenceMap;
		}

		// Token: 0x06003C92 RID: 15506 RVA: 0x001685D3 File Offset: 0x001667D3
		public Type GetValueType()
		{
			return typeof(Texture2D);
		}

		// Token: 0x06003C93 RID: 15507 RVA: 0x001685E0 File Offset: 0x001667E0
		private void OnEnable()
		{
			this.system = base.GetComponentInParent<OutfitSystem>();
			if (this.system)
			{
				OutfitSystem outfitSystem = this.system;
				outfitSystem.OnRigChanged = (UnityAction<SkinnedMeshRenderer>)Delegate.Combine(outfitSystem.OnRigChanged, new UnityAction<SkinnedMeshRenderer>(this.OnCharacterMerged));
			}
		}

		// Token: 0x06003C94 RID: 15508 RVA: 0x00168630 File Offset: 0x00166830
		private void OnDisable()
		{
			this.system = base.GetComponentInParent<OutfitSystem>();
			if (this.system)
			{
				OutfitSystem outfitSystem = this.system;
				outfitSystem.OnRigChanged = (UnityAction<SkinnedMeshRenderer>)Delegate.Remove(outfitSystem.OnRigChanged, new UnityAction<SkinnedMeshRenderer>(this.OnCharacterMerged));
			}
		}

		// Token: 0x06003C95 RID: 15509 RVA: 0x0016867D File Offset: 0x0016687D
		public void Initalize(OutfitSystem outfitSystem, Outfit outfit)
		{
			if (this.cloth)
			{
				return;
			}
			this.cloth = base.gameObject.AddComponent<MagicaCloth>();
			this.cloth.Initialize();
			this.cloth.DisableAutoBuild();
		}

		// Token: 0x06003C96 RID: 15510 RVA: 0x001686B4 File Offset: 0x001668B4
		private void OnCharacterMerged(SkinnedMeshRenderer rig)
		{
			if (this.system == null)
			{
				return;
			}
			if (this.cloth)
			{
				Object.Destroy(this.cloth);
				this.cloth = null;
			}
			this.initalized = false;
			if (this.system.customMaps.ContainsKey("MagicaClothExtension"))
			{
				this.influenceMap = this.system.customMaps["MagicaClothExtension"];
			}
			this.Initalize(null, null);
			this.Execute(this.system, null);
		}

		// Token: 0x06003C97 RID: 15511 RVA: 0x00168740 File Offset: 0x00166940
		public void Execute(OutfitSystem outfitSystem, Outfit outfit)
		{
			if (this.initalized)
			{
				return;
			}
			switch (this.type)
			{
			case BoZo_MagicaClothSupport.ClothType.Mesh:
				this.SetMeshCloth(outfitSystem, outfit);
				break;
			case BoZo_MagicaClothSupport.ClothType.Bone:
				this.SetBoneCloth(outfitSystem, outfit);
				break;
			case BoZo_MagicaClothSupport.ClothType.Spring:
				this.SetBoneCloth(outfitSystem, outfit);
				break;
			}
			this.initalized = true;
		}

		// Token: 0x06003C98 RID: 15512 RVA: 0x00168794 File Offset: 0x00166994
		private void SetMeshCloth(OutfitSystem outfitSystem, Outfit outfit)
		{
			ClothSerializeData serializeData = this.cloth.SerializeData;
			SkinnedMeshRenderer skinnedMeshRenderer;
			if (outfit)
			{
				skinnedMeshRenderer = outfit.skinnedRenderer;
			}
			else
			{
				skinnedMeshRenderer = outfitSystem.GetCharacterBody();
			}
			serializeData.sourceRenderers.Add(skinnedMeshRenderer);
			serializeData.reductionSetting.shapeDistance = this.reductionSetting;
			serializeData.paintMode = ClothSerializeData.PaintMode.Texture_Fixed_Move;
			serializeData.paintMaps.Add(this.influenceMap);
			if (this.clothPreset)
			{
				this.cloth.SerializeData.ImportJson(this.clothPreset.ToString());
			}
			serializeData.radius = new CurveSerializeData(this.collisionSize);
			ColliderComponent[] clothColliders = outfitSystem.GetClothColliders();
			serializeData.colliderCollisionConstraint.colliderList = clothColliders.ToList<ColliderComponent>();
			outfitSystem.RebindBody();
			this.cloth.enabled = true;
			this.cloth.BuildAndRun();
			ClothQualityService.Register(this.cloth);
		}

		// Token: 0x06003C99 RID: 15513 RVA: 0x00168878 File Offset: 0x00166A78
		private void SetBoneCloth(OutfitSystem outfitSystem, Outfit outfit)
		{
			ClothSerializeData serializeData = this.cloth.SerializeData;
			if (this.type == BoZo_MagicaClothSupport.ClothType.Bone)
			{
				serializeData.clothType = ClothProcess.ClothType.BoneCloth;
			}
			if (this.type == BoZo_MagicaClothSupport.ClothType.Spring)
			{
				serializeData.clothType = ClothProcess.ClothType.BoneSpring;
			}
			SkinnedMeshRenderer skinnedMeshRenderer;
			if (outfit)
			{
				skinnedMeshRenderer = outfit.skinnedRenderer;
			}
			else
			{
				skinnedMeshRenderer = outfitSystem.GetCharacterBody();
			}
			if (this.rootBonesString.Count != 0)
			{
				serializeData.rootBones = skinnedMeshRenderer.bones.Where<Transform>((Transform bone) => this.rootBonesString.Contains(bone.name)).ToList<Transform>();
			}
			else
			{
				serializeData.rootBones = this.rootBones;
			}
			if (this.clothPreset)
			{
				this.cloth.SerializeData.ImportJson(this.clothPreset.ToString());
			}
			serializeData.radius = new CurveSerializeData(this.collisionSize);
			ColliderComponent[] clothColliders = outfitSystem.GetClothColliders();
			serializeData.colliderCollisionConstraint.colliderList = clothColliders.ToList<ColliderComponent>();
			this.cloth.enabled = true;
			this.cloth.BuildAndRun();
			ClothQualityService.Register(this.cloth);
		}

		// Token: 0x06003C9A RID: 15514 RVA: 0x0016897E File Offset: 0x00166B7E
		public string GetID()
		{
			return "MagicaClothExtension";
		}

		// Token: 0x04003C8A RID: 15498
		public const string id = "MagicaClothExtension";

		// Token: 0x04003C8B RID: 15499
		private bool initalized;

		// Token: 0x04003C8C RID: 15500
		private OutfitSystem system;

		// Token: 0x04003C8D RID: 15501
		public BoZo_MagicaClothSupport.ClothType type;

		// Token: 0x04003C8E RID: 15502
		[Header("Bones")]
		public bool boneReferenceByString;

		// Token: 0x04003C8F RID: 15503
		public List<Transform> rootBones;

		// Token: 0x04003C90 RID: 15504
		public List<string> rootBonesString;

		// Token: 0x04003C91 RID: 15505
		public float collisionSize = 0.025f;

		// Token: 0x04003C92 RID: 15506
		[Header("Mesh")]
		public Texture2D influenceMap;

		// Token: 0x04003C93 RID: 15507
		[Range(0f, 0.2f)]
		public float reductionSetting = 0.065f;

		// Token: 0x04003C94 RID: 15508
		[Header("Preset")]
		public TextAsset clothPreset;

		// Token: 0x04003C95 RID: 15509
		public List<Transform> transforms = new List<Transform>();

		// Token: 0x04003C96 RID: 15510
		private MagicaCloth cloth;

		// Token: 0x020008CB RID: 2251
		public enum ClothType
		{
			// Token: 0x04003C98 RID: 15512
			Mesh,
			// Token: 0x04003C99 RID: 15513
			Bone,
			// Token: 0x04003C9A RID: 15514
			Spring
		}
	}
}
