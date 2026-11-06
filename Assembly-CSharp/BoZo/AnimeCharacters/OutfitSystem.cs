using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MagicaCloth2;
using UnityEngine;
using UnityEngine.Events;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008C2 RID: 2242
	public class OutfitSystem : MonoBehaviour
	{
		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x06003C3D RID: 15421 RVA: 0x00166A30 File Offset: 0x00164C30
		// (set) Token: 0x06003C3E RID: 15422 RVA: 0x00166A38 File Offset: 0x00164C38
		public bool muteHeightChange { get; private set; }

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x06003C3F RID: 15423 RVA: 0x00166A41 File Offset: 0x00164C41
		// (set) Token: 0x06003C40 RID: 15424 RVA: 0x00166A49 File Offset: 0x00164C49
		public float height { get; private set; }

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x06003C41 RID: 15425 RVA: 0x00166A52 File Offset: 0x00164C52
		// (set) Token: 0x06003C42 RID: 15426 RVA: 0x00166A8E File Offset: 0x00164C8E
		public Animator animator
		{
			get
			{
				if (this._animator == null)
				{
					this._animator = base.GetComponentInParent<Animator>();
					if (this._animator == null)
					{
						this._animator = base.GetComponentInChildren<Animator>();
					}
				}
				return this._animator;
			}
			private set
			{
				this._animator = value;
			}
		}

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x06003C43 RID: 15427 RVA: 0x00166A97 File Offset: 0x00164C97
		// (set) Token: 0x06003C44 RID: 15428 RVA: 0x00166A9F File Offset: 0x00164C9F
		public float stance { get; private set; }

		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x06003C45 RID: 15429 RVA: 0x00166AA8 File Offset: 0x00164CA8
		// (set) Token: 0x06003C46 RID: 15430 RVA: 0x00166AB0 File Offset: 0x00164CB0
		public bool initalized { get; private set; }

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x06003C47 RID: 15431 RVA: 0x00166AB9 File Offset: 0x00164CB9
		// (set) Token: 0x06003C48 RID: 15432 RVA: 0x00166AC1 File Offset: 0x00164CC1
		public bool isDirty { get; private set; }

		// Token: 0x06003C49 RID: 15433 RVA: 0x00166ACC File Offset: 0x00164CCC
		private void OnValidate()
		{
			if (Application.isPlaying && base.gameObject.scene.isLoaded && this.loadMode == OutfitSystem.LoadMode.OnStartAndOnValidate)
			{
				base.Invoke("LoadFromObject", 0f);
			}
		}

		// Token: 0x06003C4A RID: 15434 RVA: 0x00166B0D File Offset: 0x00164D0D
		private void Awake()
		{
			this.Init();
			if (this.mergeOnAwake)
			{
				this.mergedMode = true;
			}
		}

		// Token: 0x06003C4B RID: 15435 RVA: 0x00166B24 File Offset: 0x00164D24
		private void Start()
		{
			if (this.loadMode == OutfitSystem.LoadMode.OnStart || this.loadMode == OutfitSystem.LoadMode.OnStartAndOnValidate)
			{
				this.LoadFromObject();
			}
		}

		// Token: 0x06003C4C RID: 15436 RVA: 0x00166B40 File Offset: 0x00164D40
		public void Init()
		{
			if (this.initalized)
			{
				return;
			}
			if (this.CharacterBody == null)
			{
				Debug.LogWarning("Outfit System does not have a Rig assigned please assign one to prevent this warning", base.gameObject);
				Debug.LogWarning("Attempting auto rig assignment...");
				foreach (SkinnedMeshRenderer skinnedMeshRenderer in base.GetComponentsInChildren<SkinnedMeshRenderer>(true))
				{
					if (skinnedMeshRenderer.name == "BMAC_Body")
					{
						this.CharacterBody = skinnedMeshRenderer;
						Debug.Log("Rig Found Successfully!");
						break;
					}
				}
				Debug.LogError("Search Failed. Please Assign Mannually", base.gameObject);
				return;
			}
			this.CharacterRenderBounds = this.CharacterBody.localBounds;
			this.InitBoneMap();
			this.InitBodyShapes();
			this.InitBodyMods();
			this.InitClothColliders();
			this.initalized = true;
		}

		// Token: 0x06003C4D RID: 15437 RVA: 0x00166C00 File Offset: 0x00164E00
		private void InitBoneMap()
		{
			this.boneMap.Clear();
			foreach (Transform transform in this.CharacterBody.bones)
			{
				if (!this.boneMap.ContainsKey(transform.name))
				{
					this.boneMap.Add(transform.name, transform);
				}
			}
		}

		// Token: 0x06003C4E RID: 15438 RVA: 0x00166C5C File Offset: 0x00164E5C
		private void InitBodyShapes()
		{
			Outfit outfit = this.GetOutfit("Body");
			this.bodyShapes.Clear();
			this.tagShapes.Clear();
			Mesh mesh;
			int num;
			if (outfit != null)
			{
				mesh = outfit.skinnedRenderer.sharedMesh;
				num = outfit.skinnedRenderer.sharedMesh.blendShapeCount;
			}
			else
			{
				mesh = this.CharacterBody.sharedMesh;
				num = this.CharacterBody.sharedMesh.blendShapeCount;
			}
			for (int i = 0; i < num; i++)
			{
				mesh.GetBlendShapeName(i);
				string text = mesh.GetBlendShapeName(i);
				string[] array = text.Split(".", StringSplitOptions.None);
				if (array.Length > 1)
				{
					text = array[1];
				}
				array = text.Split("_", StringSplitOptions.None);
				if (array.Length > 1)
				{
					if (array[0] == "Shape")
					{
						this.bodyShapes.Add(array[1], i);
					}
					if (array[0] == "Tag")
					{
						this.tagShapes.Add(array[1], i);
					}
				}
			}
		}

		// Token: 0x06003C4F RID: 15439 RVA: 0x00166D68 File Offset: 0x00164F68
		private void InitBodyMods()
		{
			List<BodyShapeModifier> list = new List<BodyShapeModifier>(base.GetComponentsInChildren<BodyShapeModifier>());
			this.bodyModifiers.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				this.bodyModifiers.Add(list[i].name, list[i]);
			}
		}

		// Token: 0x06003C50 RID: 15440 RVA: 0x00166DBC File Offset: 0x00164FBC
		private void InitFaceShapes()
		{
			Outfit outfit = this.GetOutfit("Head");
			this.faceShapes.Clear();
			Mesh mesh;
			if (outfit != null)
			{
				mesh = outfit.skinnedRenderer.sharedMesh;
			}
			else
			{
				mesh = this.CharacterBody.sharedMesh;
			}
			int blendShapeCount = mesh.blendShapeCount;
			for (int i = 0; i < blendShapeCount; i++)
			{
				mesh.GetBlendShapeName(i);
				string text = mesh.GetBlendShapeName(i);
				string[] array = text.Split(".", StringSplitOptions.None);
				if (array.Length > 1)
				{
					text = array[1];
				}
				array = text.Split("_", StringSplitOptions.None);
				if (array.Length > 1 && array[0] == "Shape")
				{
					this.faceShapes.Add(array[1], i);
				}
			}
		}

		// Token: 0x06003C51 RID: 15441 RVA: 0x00166E79 File Offset: 0x00165079
		private void InitClothColliders()
		{
			this.ClothColliders = base.GetComponentsInChildren<ColliderComponent>();
		}

		// Token: 0x06003C52 RID: 15442 RVA: 0x00166E87 File Offset: 0x00165087
		public void LoadFromObject(CharacterObject saveData)
		{
			this.characterData = saveData;
			this.LoadFromObject();
		}

		// Token: 0x06003C53 RID: 15443 RVA: 0x00166E98 File Offset: 0x00165098
		[ContextMenu("Load")]
		public void LoadFromObject()
		{
			if (this.characterData && this._characterData != this.characterData)
			{
				this.SaveID = this.characterData.name;
				if (this.mergedMode)
				{
					this.data = this.characterData.data;
					this.isDirty = true;
					this.MergeCharacter();
					return;
				}
				this._characterData = this.characterData;
				this.LoadCharacter(this.characterData.data);
			}
		}

		// Token: 0x06003C54 RID: 15444 RVA: 0x00166F1A File Offset: 0x0016511A
		[ContextMenu("LoadByID")]
		public void LoadFromID()
		{
			this.LoadFromID(this.SaveID);
		}

		// Token: 0x06003C55 RID: 15445 RVA: 0x00166F28 File Offset: 0x00165128
		public void LoadFromID(string saveName)
		{
			if (string.IsNullOrEmpty(saveName))
			{
				return;
			}
			this.SaveID = saveName;
			CharacterData dataFromID = BMAC_SaveSystem.GetDataFromID(this.SaveID);
			if (dataFromID == null)
			{
				return;
			}
			this.LoadCharacter(dataFromID);
		}

		// Token: 0x06003C56 RID: 15446 RVA: 0x00166F5C File Offset: 0x0016515C
		private async void LoadCharacter(CharacterData data)
		{
			if (this.mergedMode)
			{
				this.data = data;
				this.isDirty = true;
				this.MergeCharacter();
			}
			else
			{
				await BMAC_SaveSystem.LoadCharacter(this, data, false, this.async);
			}
		}

		// Token: 0x06003C57 RID: 15447 RVA: 0x00166F9C File Offset: 0x0016519C
		[ContextMenu("SaveToObject")]
		public void SaveToObject()
		{
			if (!this.characterData)
			{
				Debug.LogWarning("Character Data Field is empty. Please provide a BSMC_CharacterObject to " + base.transform.name);
				return;
			}
			BMAC_SaveSystem.SaveCharacter(this, this.characterData.data.characterName, this.characterData.icon, null);
		}

		// Token: 0x06003C58 RID: 15448 RVA: 0x00166FF3 File Offset: 0x001651F3
		[ContextMenu("SaveByID")]
		public void SaveByID()
		{
			this.SaveByID(this.SaveID);
		}

		// Token: 0x06003C59 RID: 15449 RVA: 0x00167004 File Offset: 0x00165204
		public void SaveByID(string characterName)
		{
			if (string.IsNullOrEmpty(characterName))
			{
				Debug.LogWarning("No ID provided saving aborted");
				return;
			}
			if (!File.Exists(BMAC_SaveSystem.iconFilePath + "/" + characterName + ".png"))
			{
				byte[] array = new Texture2D(2, 2, TextureFormat.RGBA32, false).EncodeToPNG();
				File.WriteAllBytes(BMAC_SaveSystem.iconFilePath + "/" + characterName + ".png", array);
			}
			BMAC_SaveSystem.SaveCharacter(this, characterName, null, null);
		}

		// Token: 0x06003C5A RID: 15450 RVA: 0x00167074 File Offset: 0x00165274
		public void RemoveOutfit(Outfit outfit, bool destory)
		{
			Outfit outfit2;
			if (this.Outfits.TryGetValue(outfit.Type, out outfit2) && destory && outfit2 != null)
			{
				Object.Destroy(outfit2.gameObject);
				this.Outfits[outfit.Type] = null;
			}
			this.RemoveTags(outfit);
			UnityAction<Outfit> onOutfitChanged = this.OnOutfitChanged;
			if (onOutfitChanged == null)
			{
				return;
			}
			onOutfitChanged(null);
		}

		// Token: 0x06003C5B RID: 15451 RVA: 0x001670D8 File Offset: 0x001652D8
		public void RemoveOutfit(OutfitType type, bool destory)
		{
			if (type == null)
			{
				return;
			}
			Outfit outfit;
			if (this.Outfits.TryGetValue(type, out outfit) && destory && outfit != null)
			{
				Object.Destroy(outfit.gameObject);
				this.Outfits[type] = null;
			}
			this.RemoveTags(outfit);
			UnityAction<Outfit> onOutfitChanged = this.OnOutfitChanged;
			if (onOutfitChanged == null)
			{
				return;
			}
			onOutfitChanged(null);
		}

		// Token: 0x06003C5C RID: 15452 RVA: 0x0016713C File Offset: 0x0016533C
		public void RemoveTags(Outfit removedOutfit)
		{
			if (removedOutfit == null)
			{
				return;
			}
			this.tags.RemoveAll((string item) => removedOutfit.tags.Contains(item));
		}

		// Token: 0x06003C5D RID: 15453 RVA: 0x00167180 File Offset: 0x00165380
		public void RemoveAllOutfits()
		{
			foreach (Outfit outfit in new List<Outfit>(this.Outfits.Values))
			{
				if (!(outfit == null))
				{
					Object.Destroy(outfit.gameObject);
				}
			}
			this.Outfits.Clear();
			this.tags.Clear();
			UnityAction<Outfit> onOutfitChanged = this.OnOutfitChanged;
			if (onOutfitChanged == null)
			{
				return;
			}
			onOutfitChanged(null);
		}

		// Token: 0x06003C5E RID: 15454 RVA: 0x00167214 File Offset: 0x00165414
		public Outfit InstantiateOutfit(Outfit outfit)
		{
			Outfit outfit2 = Object.Instantiate<Outfit>(outfit, base.transform);
			outfit2.name = outfit2.name.Replace("(Clone)", "");
			return outfit2;
		}

		// Token: 0x06003C5F RID: 15455 RVA: 0x00167240 File Offset: 0x00165440
		public void AttachOutfit(Outfit outfit)
		{
			if (!this.initalized)
			{
				return;
			}
			if (this.mergedMode)
			{
				this.outfitData[outfit.Type.name] = outfit.GetOutfitData();
				this.data.outfitDatas = this.outfitData.Values.ToList<OutfitData>();
				this.isDirty = true;
				Object.Destroy(outfit.gameObject);
				if (this.autoUpdate)
				{
					this.MergeCharacter();
				}
				return;
			}
			if (!this.KnownOutfitTypes.ContainsKey(outfit.Type.name))
			{
				this.KnownOutfitTypes.Add(outfit.Type.name, outfit.Type);
			}
			this.ReplaceOutfit(outfit);
			this.MergeBones(outfit);
			if (outfit.skinnedRenderer)
			{
				this.UpdateCharacterBounds(outfit);
			}
			this.SynchronizeOutfitBlendShapes(outfit);
			if (outfit.Type.name == "Head")
			{
				this.InitFaceShapes();
			}
			if (outfit.Type.name == "Body")
			{
				this.InitBodyShapes();
			}
			this.tags.AddRange(outfit.tags);
			this.ApplyTags();
			UnityAction<Outfit> onOutfitChanged = this.OnOutfitChanged;
			if (onOutfitChanged == null)
			{
				return;
			}
			onOutfitChanged(outfit);
		}

		// Token: 0x06003C60 RID: 15456 RVA: 0x00167378 File Offset: 0x00165578
		private void ApplyShapesToOufit(Outfit outfit)
		{
			List<string> list = new List<string>(this.bodyShapes.Keys);
			for (int i = 0; i < list.Count; i++)
			{
				float shape = this.GetShape(list[i]);
				if (shape > -9999f)
				{
					outfit.SetShape(list[i], shape);
				}
			}
		}

		// Token: 0x06003C61 RID: 15457 RVA: 0x001673CC File Offset: 0x001655CC
		private void SynchronizeOutfitBlendShapes(Outfit outfit)
		{
			if (outfit == null)
			{
				return;
			}
			Outfit outfit2 = this.GetOutfit("Body");
			Outfit outfit3 = this.GetOutfit("Head");
			OutfitSystem.SynchronizeFromRenderer(outfit, (outfit2 != null) ? outfit2.skinnedRenderer : this.CharacterBody);
			if (outfit3 != null && outfit3.skinnedRenderer != null && (outfit2 == null || outfit3.skinnedRenderer != outfit2.skinnedRenderer))
			{
				OutfitSystem.SynchronizeFromRenderer(outfit, outfit3.skinnedRenderer);
			}
		}

		// Token: 0x06003C62 RID: 15458 RVA: 0x00167458 File Offset: 0x00165658
		private static void SynchronizeFromRenderer(Outfit target, SkinnedMeshRenderer source)
		{
			if (target == null || source == null || source.sharedMesh == null)
			{
				return;
			}
			Mesh sharedMesh = source.sharedMesh;
			for (int i = 0; i < sharedMesh.blendShapeCount; i++)
			{
				target.SetShape(sharedMesh.GetBlendShapeName(i), source.GetBlendShapeWeight(i));
			}
		}

		// Token: 0x06003C63 RID: 15459 RVA: 0x001674B4 File Offset: 0x001656B4
		public void SynchronizeAllOutfitBlendShapes()
		{
			List<Outfit> outfits = this.GetOutfits();
			for (int i = 0; i < outfits.Count; i++)
			{
				Outfit outfit = outfits[i];
				if (outfit != null)
				{
					this.SynchronizeOutfitBlendShapes(outfit);
				}
			}
		}

		// Token: 0x06003C64 RID: 15460 RVA: 0x001674F4 File Offset: 0x001656F4
		public void SetShape(string key, float value)
		{
			SkinnedMeshRenderer skinnedMeshRenderer = null;
			int num = -1;
			Outfit outfit = this.GetOutfit("Body");
			Outfit outfit2 = this.GetOutfit("Head");
			int num2;
			int num3;
			int num4;
			if (this.bodyShapes.TryGetValue(key, out num2) && outfit)
			{
				num = num2;
				if (outfit != null)
				{
					skinnedMeshRenderer = outfit.skinnedRenderer;
				}
			}
			else if (this.faceShapes.TryGetValue(key, out num3) && outfit2)
			{
				num = num3;
				if (outfit2 != null)
				{
					skinnedMeshRenderer = outfit2.skinnedRenderer;
				}
			}
			else if (this.bodyShapes.TryGetValue(key, out num4))
			{
				num = num4;
				skinnedMeshRenderer = this.CharacterBody;
			}
			if (skinnedMeshRenderer != null)
			{
				skinnedMeshRenderer.SetBlendShapeWeight(num, value);
			}
			UnityAction<string, float> onShapeChanged = this.OnShapeChanged;
			if (onShapeChanged == null)
			{
				return;
			}
			onShapeChanged(key, value);
		}

		// Token: 0x06003C65 RID: 15461 RVA: 0x001675B4 File Offset: 0x001657B4
		private void ApplyTags()
		{
			if (this.GetOutfit("Body") != null)
			{
				return;
			}
			List<string> list = new List<string>(this.tagShapes.Keys);
			if (!this.CharacterBody)
			{
				return;
			}
			if (this.CharacterBody.sharedMesh.blendShapeCount == 0)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (this.ContainsTag(list[i]))
				{
					this.CharacterBody.SetBlendShapeWeight(this.tagShapes[list[i]], 100f);
				}
				else
				{
					this.CharacterBody.SetBlendShapeWeight(this.tagShapes[list[i]], 0f);
				}
			}
		}

		// Token: 0x06003C66 RID: 15462 RVA: 0x0016766D File Offset: 0x0016586D
		public void SetStance(float value)
		{
			if (this.animator != null && this.HasAnimatorParameter(this.animator, "Stance"))
			{
				this.animator.SetFloat("Stance", value);
			}
			this.stance = value;
		}

		// Token: 0x06003C67 RID: 15463 RVA: 0x001676A8 File Offset: 0x001658A8
		private bool HasAnimatorParameter(Animator anim, string paramName)
		{
			if (anim == null || anim.runtimeAnimatorController == null)
			{
				return false;
			}
			AnimatorControllerParameter[] parameters = anim.parameters;
			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i].name == paramName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003C68 RID: 15464 RVA: 0x001676F8 File Offset: 0x001658F8
		public void SetHeight(float value)
		{
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y - this.height, base.transform.localPosition.z);
			this.height = value;
			if (!this.muteHeightChange)
			{
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y + value, base.transform.localPosition.z);
			}
		}

		// Token: 0x06003C69 RID: 15465 RVA: 0x001677A0 File Offset: 0x001659A0
		public void MuteHeightChange(bool value)
		{
			if (value == this.muteHeightChange)
			{
				return;
			}
			this.muteHeightChange = value;
			float num = this.height;
			if (this.muteHeightChange)
			{
				num = -num;
			}
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y + num, base.transform.localPosition.z);
		}

		// Token: 0x06003C6A RID: 15466 RVA: 0x00167814 File Offset: 0x00165A14
		private void ReplaceOutfit(Outfit outfit)
		{
			Outfit outfit2;
			if (this.Outfits.TryGetValue(outfit.Type, out outfit2))
			{
				if (this.Outfits[outfit.Type])
				{
					if (outfit.transform != this.Outfits[outfit.Type].transform)
					{
						Object.Destroy(outfit2.gameObject);
					}
					else
					{
						UnityAction<Outfit> onOutfitChanged = this.OnOutfitChanged;
						if (onOutfitChanged != null)
						{
							onOutfitChanged(outfit);
						}
					}
				}
				this.Outfits[outfit.Type] = outfit;
				return;
			}
			this.Outfits.Add(outfit.Type, outfit);
		}

		// Token: 0x06003C6B RID: 15467 RVA: 0x001678B8 File Offset: 0x00165AB8
		private void MergeBones(Outfit outfit)
		{
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in outfit.skinnedRenderers)
			{
				if (outfit.AttachPoint == "" && skinnedMeshRenderer)
				{
					if (!outfit.Initalized)
					{
						Transform[] array = skinnedMeshRenderer.bones.ToArray<Transform>();
						Transform[] array2 = new Transform[skinnedMeshRenderer.bones.Length];
						for (int j = 0; j < array.Length; j++)
						{
							Transform transform = array[j];
							Transform transform2;
							if (!this.boneMap.TryGetValue(transform.name, out transform2) || transform2 == null)
							{
								Transform transform3 = transform.parent;
								while (transform3 != null && (!this.boneMap.TryGetValue(transform3.name, out transform2) || !(transform2 != null)))
								{
									transform3 = transform3.parent;
								}
							}
							array2[j] = transform2;
						}
						skinnedMeshRenderer.bones = array2;
						skinnedMeshRenderer.rootBone = this.CharacterBody.rootBone;
					}
				}
				else
				{
					Transform transform4 = null;
					try
					{
						transform4 = this.boneMap[outfit.AttachPoint];
					}
					catch
					{
						Debug.LogError(string.Concat(new string[] { base.name, " is missing ", outfit.AttachPoint, " that ", outfit.name, " requires" }));
						return;
					}
					outfit.transform.parent = transform4.transform;
					outfit.transform.position = transform4.position;
					outfit.transform.rotation = transform4.rotation;
					outfit.transform.localScale = Vector3.one;
					OutfitSystem.ApplyHatReferenceTransform(outfit, transform4);
				}
			}
			outfit.ActivateCloth(this.boneMap);
			outfit.Initalized = true;
			if (outfit.outfitRenderer && outfit.AttachPoint != "")
			{
				Transform transform5 = null;
				try
				{
					transform5 = this.boneMap[outfit.AttachPoint];
				}
				catch
				{
					Debug.LogError(string.Concat(new string[] { base.name, " is missing ", outfit.AttachPoint, " that ", outfit.name, " requires" }));
					return;
				}
				outfit.transform.parent = transform5.transform;
				outfit.transform.position = transform5.position;
				outfit.transform.rotation = transform5.rotation;
				outfit.transform.localScale = Vector3.one;
				OutfitSystem.ApplyHatReferenceTransform(outfit, transform5);
				return;
			}
		}

		// Token: 0x06003C6C RID: 15468 RVA: 0x00167B74 File Offset: 0x00165D74
		private static void ApplyHatReferenceTransform(Outfit outfit, Transform attachBone)
		{
			if (outfit == null || attachBone == null)
			{
				return;
			}
			string name = outfit.name;
			if (string.IsNullOrEmpty(name))
			{
				return;
			}
			if (name.IndexOf("Hat", StringComparison.OrdinalIgnoreCase) < 0)
			{
				return;
			}
			string text = name.Replace("(Clone)", "").Trim();
			Transform transform = null;
			for (int i = 0; i < attachBone.childCount; i++)
			{
				Transform child = attachBone.GetChild(i);
				if (!(child == outfit.transform) && child.name == text)
				{
					transform = child;
					break;
				}
			}
			if (transform == null)
			{
				return;
			}
			outfit.transform.localPosition = transform.localPosition;
			outfit.transform.localRotation = transform.localRotation;
			outfit.transform.localScale = transform.localScale;
			transform.gameObject.SetActive(false);
		}

		// Token: 0x06003C6D RID: 15469 RVA: 0x00167C54 File Offset: 0x00165E54
		public void UpdateCharacterBounds(Outfit outfit)
		{
			foreach (Outfit outfit2 in this.Outfits.Values)
			{
				if (!(outfit2 == null))
				{
					foreach (SkinnedMeshRenderer skinnedMeshRenderer in outfit2.skinnedRenderers)
					{
						if (skinnedMeshRenderer != null)
						{
							skinnedMeshRenderer.localBounds = this.CharacterRenderBounds;
						}
					}
				}
			}
		}

		// Token: 0x06003C6E RID: 15470 RVA: 0x00167CE0 File Offset: 0x00165EE0
		public bool ContainsTag(string tag)
		{
			return this.tags.Contains(tag);
		}

		// Token: 0x06003C6F RID: 15471 RVA: 0x00167CEE File Offset: 0x00165EEE
		public void AddTag(string tag)
		{
			if (this.tags.Contains(tag))
			{
				return;
			}
			this.tags.Add(tag);
			this.ApplyTags();
			UnityAction<Outfit> onOutfitChanged = this.OnOutfitChanged;
			if (onOutfitChanged == null)
			{
				return;
			}
			onOutfitChanged(null);
		}

		// Token: 0x06003C70 RID: 15472 RVA: 0x00167D24 File Offset: 0x00165F24
		public Outfit GetOutfit(OutfitType outfitType)
		{
			Outfit outfit;
			if (this.Outfits.TryGetValue(outfitType, out outfit))
			{
				return outfit;
			}
			return null;
		}

		// Token: 0x06003C71 RID: 15473 RVA: 0x00167D44 File Offset: 0x00165F44
		public Outfit GetOutfit(string outfitType)
		{
			OutfitType outfitType2;
			Outfit outfit;
			if (this.KnownOutfitTypes.TryGetValue(outfitType, out outfitType2) && this.Outfits.TryGetValue(outfitType2, out outfit))
			{
				return outfit;
			}
			return null;
		}

		// Token: 0x06003C72 RID: 15474 RVA: 0x00167D74 File Offset: 0x00165F74
		public List<Outfit> GetOutfits()
		{
			return new List<Outfit>(this.Outfits.Values);
		}

		// Token: 0x06003C73 RID: 15475 RVA: 0x00167D86 File Offset: 0x00165F86
		public List<string> GetShapes()
		{
			return this.bodyShapes.Keys.ToList<string>();
		}

		// Token: 0x06003C74 RID: 15476 RVA: 0x00167D98 File Offset: 0x00165F98
		public List<string> GetFaceShapes()
		{
			return this.faceShapes.Keys.ToList<string>();
		}

		// Token: 0x06003C75 RID: 15477 RVA: 0x00167DAA File Offset: 0x00165FAA
		public ColliderComponent[] GetClothColliders()
		{
			return this.ClothColliders;
		}

		// Token: 0x06003C76 RID: 15478 RVA: 0x00167DB4 File Offset: 0x00165FB4
		public float GetShape(string key)
		{
			int num;
			if (!this.bodyShapes.TryGetValue(key, out num))
			{
				return -10000f;
			}
			Outfit outfit = this.GetOutfit("Body");
			if (outfit != null)
			{
				return outfit.skinnedRenderer.GetBlendShapeWeight(num);
			}
			return -10000f;
		}

		// Token: 0x06003C77 RID: 15479 RVA: 0x00167DFE File Offset: 0x00165FFE
		public Dictionary<string, BodyShapeModifier> GetMods()
		{
			return this.bodyModifiers;
		}

		// Token: 0x06003C78 RID: 15480 RVA: 0x00167E06 File Offset: 0x00166006
		public Dictionary<string, Transform> GetBones()
		{
			return this.boneMap;
		}

		// Token: 0x06003C79 RID: 15481 RVA: 0x00167E10 File Offset: 0x00166010
		public float GetShapeValue(string key)
		{
			float num = -1f;
			Outfit outfit = this.GetOutfit("Body");
			if (outfit == null)
			{
				return -1f;
			}
			int num2;
			int num3;
			if (this.bodyShapes.TryGetValue(key, out num2))
			{
				num = outfit.skinnedRenderer.GetBlendShapeWeight(num2);
			}
			else if (this.faceShapes.TryGetValue(key, out num3))
			{
				Outfit outfit2 = this.GetOutfit("Head");
				if (outfit2 == null)
				{
					return -1f;
				}
				num = outfit2.skinnedRenderer.GetBlendShapeWeight(num3);
			}
			return num;
		}

		// Token: 0x06003C7A RID: 15482 RVA: 0x00167E9C File Offset: 0x0016609C
		public float GetShapeValue(int key)
		{
			Outfit outfit = this.GetOutfit("Body");
			SkinnedMeshRenderer skinnedMeshRenderer;
			if (outfit == null)
			{
				skinnedMeshRenderer = this.CharacterBody;
			}
			else
			{
				skinnedMeshRenderer = outfit.skinnedRenderer;
			}
			return skinnedMeshRenderer.GetBlendShapeWeight(key);
		}

		// Token: 0x06003C7B RID: 15483 RVA: 0x00167ED8 File Offset: 0x001660D8
		public Dictionary<string, float> GetBodyShapeValues()
		{
			Dictionary<string, float> dictionary = new Dictionary<string, float>();
			int[] array = this.bodyShapes.Values.ToArray<int>();
			string[] array2 = this.bodyShapes.Keys.ToArray<string>();
			Outfit outfit = this.GetOutfit("Body");
			SkinnedMeshRenderer skinnedMeshRenderer;
			if (outfit == null)
			{
				skinnedMeshRenderer = this.CharacterBody;
			}
			else
			{
				skinnedMeshRenderer = outfit.skinnedRenderer;
			}
			for (int i = 0; i < array.Length; i++)
			{
				float blendShapeWeight = skinnedMeshRenderer.GetBlendShapeWeight(array[i]);
				dictionary.Add(array2[i], blendShapeWeight);
			}
			return dictionary;
		}

		// Token: 0x06003C7C RID: 15484 RVA: 0x00167F60 File Offset: 0x00166160
		public Dictionary<string, float> GetFaceShapeValues()
		{
			Dictionary<string, float> dictionary = new Dictionary<string, float>();
			int[] array = this.faceShapes.Values.ToArray<int>();
			string[] array2 = this.faceShapes.Keys.ToArray<string>();
			Outfit outfit = this.GetOutfit("Head");
			SkinnedMeshRenderer skinnedMeshRenderer;
			if (outfit == null)
			{
				skinnedMeshRenderer = this.CharacterBody;
			}
			else
			{
				skinnedMeshRenderer = outfit.skinnedRenderer;
			}
			for (int i = 0; i < array.Length; i++)
			{
				float blendShapeWeight = skinnedMeshRenderer.GetBlendShapeWeight(array[i]);
				dictionary.Add(array2[i], blendShapeWeight);
			}
			return dictionary;
		}

		// Token: 0x06003C7D RID: 15485 RVA: 0x00167FE8 File Offset: 0x001661E8
		public SkinnedMeshRenderer GetCharacterBody()
		{
			return this.CharacterBody;
		}

		// Token: 0x06003C7E RID: 15486 RVA: 0x00167FF0 File Offset: 0x001661F0
		public void SetCharacterBody(GameObject newBody)
		{
			SkinnedMeshRenderer componentInChildren = newBody.GetComponentInChildren<SkinnedMeshRenderer>();
			if (componentInChildren == null)
			{
				return;
			}
			this.RemoveAllOutfits();
			UnityAction onBeforeRigChanged = this.OnBeforeRigChanged;
			if (onBeforeRigChanged != null)
			{
				onBeforeRigChanged();
			}
			Object.DestroyImmediate(this.CharacterBody.transform.parent.gameObject);
			newBody.transform.parent = base.transform;
			newBody.transform.localPosition = Vector3.zero;
			newBody.transform.localRotation = Quaternion.identity;
			newBody.transform.localScale = Vector3.one;
			this.CharacterBody = componentInChildren;
			this.InitBoneMap();
			this.InitBodyShapes();
			this.InitBodyMods();
			this.InitClothColliders();
			UnityAction<SkinnedMeshRenderer> onRigChanged = this.OnRigChanged;
			if (onRigChanged != null)
			{
				onRigChanged(this.CharacterBody);
			}
			base.Invoke("RebindBody", 0f);
		}

		// Token: 0x06003C7F RID: 15487 RVA: 0x001680C6 File Offset: 0x001662C6
		public void RebindBody()
		{
			this.animator.Rebind();
		}

		// Token: 0x06003C80 RID: 15488 RVA: 0x001680D3 File Offset: 0x001662D3
		[ContextMenu("Merge")]
		public void MergeCharacter()
		{
			this.MergeCharacterAsync();
		}

		// Token: 0x06003C81 RID: 15489 RVA: 0x001680DC File Offset: 0x001662DC
		public async Task MergeCharacterAsync()
		{
			if (Application.isPlaying && base.gameObject.scene.isLoaded)
			{
				BoZo_CharacterOptimizer boZo_CharacterOptimizer = new BoZo_CharacterOptimizer();
				if (this.mergedMode && this.isDirty)
				{
					await boZo_CharacterOptimizer.OptimizeCharacterAsync(this, this.data);
				}
				else
				{
					CharacterData characterData = BMAC_SaveSystem.GetCharacterData(this);
					this.data = characterData;
					foreach (Outfit outfit in this.GetOutfits())
					{
						if (outfit)
						{
							this.outfitData[outfit.Type.name] = outfit.GetOutfitData();
						}
					}
					await boZo_CharacterOptimizer.OptimizeCharacterAsync(this, characterData);
					if (this.animator != null && this.HasAnimatorParameter(this.animator, "isMerged"))
					{
						this.animator.SetBool("isMerged", true);
					}
					this.mergedMode = true;
				}
			}
			else
			{
				Debug.LogWarning("For stability reason Character Merging is only available in Play Mode");
			}
		}

		// Token: 0x06003C82 RID: 15490 RVA: 0x00004F31 File Offset: 0x00003131
		[ContextMenu("SaveToPrefab")]
		public void SaveCharacterToPrefab()
		{
		}

		// Token: 0x04003C4B RID: 15435
		public CharacterObject characterData;

		// Token: 0x04003C4C RID: 15436
		private CharacterObject _characterData;

		// Token: 0x04003C4D RID: 15437
		public string SaveID;

		// Token: 0x04003C4E RID: 15438
		[SerializeField]
		private SkinnedMeshRenderer CharacterBody;

		// Token: 0x04003C51 RID: 15441
		private Animator _animator;

		// Token: 0x04003C53 RID: 15443
		private Bounds CharacterRenderBounds;

		// Token: 0x04003C54 RID: 15444
		private Dictionary<string, Transform> boneMap = new Dictionary<string, Transform>();

		// Token: 0x04003C55 RID: 15445
		public Dictionary<OutfitType, Outfit> Outfits = new Dictionary<OutfitType, Outfit>();

		// Token: 0x04003C56 RID: 15446
		public Dictionary<string, OutfitType> KnownOutfitTypes = new Dictionary<string, OutfitType>();

		// Token: 0x04003C57 RID: 15447
		private Dictionary<string, int> bodyShapes = new Dictionary<string, int>();

		// Token: 0x04003C58 RID: 15448
		private Dictionary<string, int> faceShapes = new Dictionary<string, int>();

		// Token: 0x04003C59 RID: 15449
		private Dictionary<string, int> tagShapes = new Dictionary<string, int>();

		// Token: 0x04003C5A RID: 15450
		public Dictionary<string, BodyShapeModifier> bodyModifiers = new Dictionary<string, BodyShapeModifier>();

		// Token: 0x04003C5B RID: 15451
		private List<string> tags = new List<string>();

		// Token: 0x04003C5C RID: 15452
		public UnityAction<Outfit> OnOutfitChanged;

		// Token: 0x04003C5D RID: 15453
		public UnityAction OnBeforeRigChanged;

		// Token: 0x04003C5E RID: 15454
		public UnityAction<SkinnedMeshRenderer> OnRigChanged;

		// Token: 0x04003C5F RID: 15455
		public UnityAction<string, float> OnShapeChanged;

		// Token: 0x04003C61 RID: 15457
		public string prefabName;

		// Token: 0x04003C62 RID: 15458
		public Material mergeMaterial;

		// Token: 0x04003C63 RID: 15459
		[Tooltip("Resolution of the merged texture atlas. Lower = faster merge + less memory. Valid: 512, 1024, 2048, 4096.")]
		public int atlasSize = 2048;

		// Token: 0x04003C64 RID: 15460
		public bool mergedMode;

		// Token: 0x04003C65 RID: 15461
		public bool mergeBase;

		// Token: 0x04003C66 RID: 15462
		public bool mergeOnAwake;

		// Token: 0x04003C67 RID: 15463
		public bool autoUpdate;

		// Token: 0x04003C68 RID: 15464
		public CharacterData data;

		// Token: 0x04003C69 RID: 15465
		private Dictionary<string, OutfitData> outfitData = new Dictionary<string, OutfitData>();

		// Token: 0x04003C6A RID: 15466
		public Dictionary<string, Texture2D> customMaps = new Dictionary<string, Texture2D>();

		// Token: 0x04003C6C RID: 15468
		public MergedMaterialData[] materialData;

		// Token: 0x04003C6D RID: 15469
		public OutfitSystem.LoadMode loadMode;

		// Token: 0x04003C6E RID: 15470
		public bool async;

		// Token: 0x04003C6F RID: 15471
		private ColliderComponent[] ClothColliders;

		// Token: 0x020008C3 RID: 2243
		public enum LoadMode
		{
			// Token: 0x04003C71 RID: 15473
			OnStartAndOnValidate,
			// Token: 0x04003C72 RID: 15474
			OnStart,
			// Token: 0x04003C73 RID: 15475
			Manual
		}
	}
}
