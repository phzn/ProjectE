using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008BA RID: 2234
	public class Outfit : OutfitBase
	{
		// Token: 0x06003BE8 RID: 15336 RVA: 0x00164ACC File Offset: 0x00162CCC
		internal Object ReadPoiGameplayReference(UniversityGameplayField field, int elementIndex)
		{
			switch (field)
			{
			case UniversityGameplayField.Outfit_LinkedColorSets_linkedType:
				if (this.LinkedColorSets[elementIndex] == null)
				{
					throw new InvalidOperationException("Missing linked outfit color-set element.");
				}
				return this.LinkedColorSets[elementIndex].linkedType;
			case UniversityGameplayField.Outfit_OutfitIcon:
				return this.OutfitIcon;
			case UniversityGameplayField.Outfit_Type:
				return this.Type;
			default:
				throw new InvalidOperationException("Unsupported Outfit POI binding: " + field.ToString());
			}
		}

		// Token: 0x06003BE9 RID: 15337 RVA: 0x00164B40 File Offset: 0x00162D40
		internal void WritePoiGameplayReference(UniversityGameplayField field, Object asset, int elementIndex)
		{
			switch (field)
			{
			case UniversityGameplayField.Outfit_LinkedColorSets_linkedType:
				if (this.LinkedColorSets[elementIndex] == null)
				{
					throw new InvalidOperationException("Missing linked outfit color-set element.");
				}
				this.LinkedColorSets[elementIndex].linkedType = (OutfitType)asset;
				return;
			case UniversityGameplayField.Outfit_OutfitIcon:
				this.OutfitIcon = (Sprite)asset;
				return;
			case UniversityGameplayField.Outfit_Type:
				this.Type = (OutfitType)asset;
				return;
			default:
				throw new InvalidOperationException("Unsupported Outfit POI binding: " + field.ToString());
			}
		}

		// Token: 0x06003BEA RID: 15338 RVA: 0x00164BC3 File Offset: 0x00162DC3
		internal int GetPoiGameplayElementCount(UniversityGameplayField field)
		{
			if (field != UniversityGameplayField.Outfit_LinkedColorSets_linkedType)
			{
				throw new InvalidOperationException("Unsupported Outfit indexed POI binding: " + field.ToString());
			}
			if (this.LinkedColorSets == null)
			{
				return 0;
			}
			return this.LinkedColorSets.Length;
		}

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x06003BEB RID: 15339 RVA: 0x00164BF9 File Offset: 0x00162DF9
		// (set) Token: 0x06003BEC RID: 15340 RVA: 0x00164C01 File Offset: 0x00162E01
		public bool Initalized { get; set; }

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x06003BED RID: 15341 RVA: 0x00164C0A File Offset: 0x00162E0A
		// (set) Token: 0x06003BEE RID: 15342 RVA: 0x00164C12 File Offset: 0x00162E12
		public bool PreserveStateWhilePooled { get; set; }

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x06003BEF RID: 15343 RVA: 0x00164C1B File Offset: 0x00162E1B
		// (set) Token: 0x06003BF0 RID: 15344 RVA: 0x00164C23 File Offset: 0x00162E23
		public SkinnedMeshRenderer skinnedRenderer { get; private set; }

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x06003BF1 RID: 15345 RVA: 0x00164C2C File Offset: 0x00162E2C
		// (set) Token: 0x06003BF2 RID: 15346 RVA: 0x00164C34 File Offset: 0x00162E34
		public SkinnedMeshRenderer[] skinnedRenderers { get; private set; }

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x06003BF3 RID: 15347 RVA: 0x00164C3D File Offset: 0x00162E3D
		// (set) Token: 0x06003BF4 RID: 15348 RVA: 0x00164C45 File Offset: 0x00162E45
		public Renderer outfitRenderer { get; private set; }

		// Token: 0x06003BF5 RID: 15349 RVA: 0x00164C50 File Offset: 0x00162E50
		private void OnValidate()
		{
			if (Application.isPlaying && base.gameObject.scene.isLoaded)
			{
				if (this.system == null)
				{
					this.system = base.GetComponentInParent<OutfitSystem>();
				}
				this.SetColorInital();
			}
		}

		// Token: 0x06003BF6 RID: 15350 RVA: 0x00164C9C File Offset: 0x00162E9C
		private void Awake()
		{
			this.skinnedRenderer = base.GetComponentInChildren<SkinnedMeshRenderer>();
			if (this.skinnedRenderer)
			{
				this.material = this.skinnedRenderer.material;
			}
			this.skinnedRenderers = base.GetComponentsInChildren<SkinnedMeshRenderer>();
			this.outfitRenderer = base.GetComponentInChildren<Renderer>();
			SkinnedMeshRenderer[] skinnedRenderers = this.skinnedRenderers;
			for (int i = 0; i < skinnedRenderers.Length; i++)
			{
				skinnedRenderers[i].sharedMaterial = this.material;
			}
			this.InitSetUpShapes();
		}

		// Token: 0x06003BF7 RID: 15351 RVA: 0x00164D14 File Offset: 0x00162F14
		private void OnEnable()
		{
			if (!this.Initalized)
			{
				this.Attach();
			}
		}

		// Token: 0x06003BF8 RID: 15352 RVA: 0x00164D24 File Offset: 0x00162F24
		private void OnDisable()
		{
			if (!this.system)
			{
				return;
			}
			if (this.PreserveStateWhilePooled && !this.system.gameObject.activeInHierarchy)
			{
				return;
			}
			OutfitSystem outfitSystem = this.system;
			outfitSystem.OnOutfitChanged = (UnityAction<Outfit>)Delegate.Remove(outfitSystem.OnOutfitChanged, new UnityAction<Outfit>(this.OnOutfitUpdate));
			OutfitSystem outfitSystem2 = this.system;
			outfitSystem2.OnShapeChanged = (UnityAction<string, float>)Delegate.Remove(outfitSystem2.OnShapeChanged, new UnityAction<string, float>(this.SetShape));
			this.system.RemoveOutfit(this, false);
		}

		// Token: 0x06003BF9 RID: 15353 RVA: 0x00164DB8 File Offset: 0x00162FB8
		private void OnDestroy()
		{
			if (!this.system)
			{
				return;
			}
			OutfitSystem outfitSystem = this.system;
			outfitSystem.OnOutfitChanged = (UnityAction<Outfit>)Delegate.Remove(outfitSystem.OnOutfitChanged, new UnityAction<Outfit>(this.OnOutfitUpdate));
			OutfitSystem outfitSystem2 = this.system;
			outfitSystem2.OnShapeChanged = (UnityAction<string, float>)Delegate.Remove(outfitSystem2.OnShapeChanged, new UnityAction<string, float>(this.SetShape));
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x00164E24 File Offset: 0x00163024
		private void Start()
		{
			if (Application.isPlaying && base.gameObject.scene.isLoaded && !this.Initalized)
			{
				this.Attach();
			}
			this.SetColorInital();
		}

		// Token: 0x06003BFB RID: 15355 RVA: 0x00164E61 File Offset: 0x00163061
		public void Attach(Transform parent)
		{
			base.transform.parent = parent;
			this.Attach();
		}

		// Token: 0x06003BFC RID: 15356 RVA: 0x00164E75 File Offset: 0x00163075
		public void Attach(OutfitSystem system)
		{
			base.transform.parent = system.transform;
			this.Attach();
		}

		// Token: 0x06003BFD RID: 15357 RVA: 0x00164E90 File Offset: 0x00163090
		public void Attach()
		{
			this.system = base.GetComponentInParent<OutfitSystem>();
			this.skinnedRenderer = base.GetComponentInChildren<SkinnedMeshRenderer>();
			this.outfitRenderer = base.GetComponentInChildren<Renderer>();
			if (this.system == null)
			{
				return;
			}
			if (!this.system.initalized)
			{
				return;
			}
			IOutfitExtension[] componentsInChildren = base.GetComponentsInChildren<IOutfitExtension>();
			IOutfitExtension[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Initalize(this.system, this);
			}
			if (this.originalBones.Length != 0 && this.skinnedRenderer)
			{
				this.skinnedRenderer.bones = this.originalBones;
				this.skinnedRenderer.rootBone = this.originalRootBone;
			}
			OutfitSystem outfitSystem = this.system;
			outfitSystem.OnOutfitChanged = (UnityAction<Outfit>)Delegate.Combine(outfitSystem.OnOutfitChanged, new UnityAction<Outfit>(this.OnOutfitUpdate));
			OutfitSystem outfitSystem2 = this.system;
			outfitSystem2.OnShapeChanged = (UnityAction<string, float>)Delegate.Combine(outfitSystem2.OnShapeChanged, new UnityAction<string, float>(this.SetShape));
			this.system.AttachOutfit(this);
			this.RemoveIncompatible();
			this.CheckTags();
			this.CopySystemShapes();
			array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Execute(this.system, this);
			}
		}

		// Token: 0x06003BFE RID: 15358 RVA: 0x00164FC7 File Offset: 0x001631C7
		private void OnOutfitUpdate(Outfit newOutfit)
		{
			if (this.RemoveIfIncompatible(newOutfit))
			{
				return;
			}
			this.CheckTags();
		}

		// Token: 0x06003BFF RID: 15359 RVA: 0x00164FDC File Offset: 0x001631DC
		private void CheckTags()
		{
			List<string> list = new List<string>(this.tagShapes.Keys);
			if (!this.skinnedRenderer)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (this.system.ContainsTag(list[i]))
				{
					this.skinnedRenderer.SetBlendShapeWeight(this.tagShapes[list[i]], 100f);
				}
				else
				{
					this.skinnedRenderer.SetBlendShapeWeight(this.tagShapes[list[i]], 0f);
				}
			}
		}

		// Token: 0x06003C00 RID: 15360 RVA: 0x00165074 File Offset: 0x00163274
		private void CopySystemShapes()
		{
			if (this.system)
			{
				string[] array = this.shapes.Keys.ToArray<string>();
				for (int i = 0; i < array.Length; i++)
				{
					float shape = this.system.GetShape(array[i]);
					if (shape != -10000f)
					{
						this.SetShape(array[i], shape);
					}
				}
			}
		}

		// Token: 0x06003C01 RID: 15361 RVA: 0x001650D0 File Offset: 0x001632D0
		private void InitSetUpShapes()
		{
			this.shapes.Clear();
			this.tagShapes.Clear();
			this.rendererShapes.Clear();
			if (this.skinnedRenderers == null || this.skinnedRenderers.Length == 0)
			{
				this.skinnedRenderers = base.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			}
			for (int i = 0; i < this.skinnedRenderers.Length; i++)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = this.skinnedRenderers[i];
				if (!(skinnedMeshRenderer == null) && !(skinnedMeshRenderer.sharedMesh == null))
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
					Mesh sharedMesh = skinnedMeshRenderer.sharedMesh;
					for (int j = 0; j < sharedMesh.blendShapeCount; j++)
					{
						bool flag;
						bool flag2;
						string canonicalShapeKey = Outfit.GetCanonicalShapeKey(sharedMesh.GetBlendShapeName(j), out flag, out flag2);
						if (!string.IsNullOrEmpty(canonicalShapeKey))
						{
							if (flag2)
							{
								dictionary[canonicalShapeKey] = j;
								if (skinnedMeshRenderer == this.skinnedRenderer || !this.shapes.ContainsKey(canonicalShapeKey))
								{
									this.shapes[canonicalShapeKey] = j;
								}
							}
							if (flag && (skinnedMeshRenderer == this.skinnedRenderer || !this.tagShapes.ContainsKey(canonicalShapeKey)))
							{
								this.tagShapes[canonicalShapeKey] = j;
							}
						}
					}
					this.rendererShapes[skinnedMeshRenderer] = dictionary;
				}
			}
		}

		// Token: 0x06003C02 RID: 15362 RVA: 0x0016521C File Offset: 0x0016341C
		public void SetShape(string key, float value)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				return;
			}
			bool flag;
			bool flag2;
			key = Outfit.GetCanonicalShapeKey(key, out flag, out flag2);
			foreach (KeyValuePair<SkinnedMeshRenderer, Dictionary<string, int>> keyValuePair in this.rendererShapes)
			{
				int num;
				if (keyValuePair.Key != null && keyValuePair.Value.TryGetValue(key, out num))
				{
					keyValuePair.Key.SetBlendShapeWeight(num, value);
				}
			}
		}

		// Token: 0x06003C03 RID: 15363 RVA: 0x001652AC File Offset: 0x001634AC
		private static string GetCanonicalShapeKey(string rawName, out bool isTag, out bool isShape)
		{
			isTag = false;
			isShape = false;
			if (string.IsNullOrWhiteSpace(rawName))
			{
				return null;
			}
			string text = rawName.Trim();
			int num = text.LastIndexOfAny(new char[] { '.', ':' });
			if (num >= 0 && num < text.Length - 1)
			{
				text = text.Substring(num + 1);
			}
			int num2 = text.IndexOf('_');
			if (num2 > 0 && num2 < text.Length - 1)
			{
				string text2 = text.Substring(0, num2);
				isTag = string.Equals(text2, "Tag", StringComparison.OrdinalIgnoreCase);
				isShape = string.Equals(text2, "Shape", StringComparison.OrdinalIgnoreCase);
				if (isTag | isShape)
				{
					return text.Substring(num2 + 1);
				}
			}
			isShape = true;
			return text;
		}

		// Token: 0x06003C04 RID: 15364 RVA: 0x00165354 File Offset: 0x00163554
		private void RemoveIncompatible()
		{
			foreach (OutfitType outfitType in this.IncompatibleSets)
			{
				this.system.RemoveOutfit(outfitType, true);
			}
		}

		// Token: 0x06003C05 RID: 15365 RVA: 0x00165388 File Offset: 0x00163588
		private bool RemoveIfIncompatible(Outfit outfit)
		{
			if (outfit == null)
			{
				return false;
			}
			foreach (OutfitType outfitType in this.IncompatibleSets)
			{
				if (outfit.Type == outfitType)
				{
					this.system.RemoveOutfit(this, true);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003C06 RID: 15366 RVA: 0x001653D8 File Offset: 0x001635D8
		private void SetColorInital()
		{
			if (!this.outfitRenderer)
			{
				return;
			}
			Material material = this.outfitRenderer.material;
			for (int i = 0; i < this.defaultColors.Length; i++)
			{
				material.SetColor("_Color_" + (1 + i).ToString(), this.defaultColors[i]);
			}
		}

		// Token: 0x06003C07 RID: 15367 RVA: 0x0016543C File Offset: 0x0016363C
		public override void SetColor(Color color, int index, bool linkedChanged = false)
		{
			if (this.system == null)
			{
				this.system = base.GetComponentInParent<OutfitSystem>();
			}
			if (this.outfitRenderer == null)
			{
				this.outfitRenderer = base.GetComponentInChildren<Renderer>();
			}
			if (this.customShader)
			{
				this.SetColor(color);
			}
			else
			{
				this.outfitRenderer.material.SetColor("_Color_" + index.ToString(), color);
			}
			foreach (LinkedColorSets linkedColorSets2 in this.LinkedColorSets)
			{
				if (!linkedChanged)
				{
					Outfit outfit = this.system.GetOutfit(linkedColorSets2.linkedType);
					if (!(outfit == null) && index <= linkedColorSets2.linkedChannelRange)
					{
						outfit.SetColor(color, index, true);
					}
				}
			}
		}

		// Token: 0x06003C08 RID: 15368 RVA: 0x001654FC File Offset: 0x001636FC
		public override void SetSwatch(int swatchIndex, bool linkedChanged = false)
		{
			if (!this.customShader)
			{
				return;
			}
			if (!this.material)
			{
				this.material = base.GetComponentInChildren<Renderer>().material;
			}
			if (swatchIndex + 1 > this.outfitSwatches.Count)
			{
				return;
			}
			Texture texture = Resources.Load<Texture>(this.outfitSwatches[swatchIndex].swatchID);
			this.material.mainTexture = texture;
			this.currentSwatch = swatchIndex;
			foreach (LinkedColorSets linkedColorSets2 in this.LinkedColorSets)
			{
				if (!linkedChanged)
				{
					Outfit outfit = this.system.GetOutfit(linkedColorSets2.linkedType);
					if (!(outfit == null))
					{
						outfit.SetSwatch(swatchIndex, true);
					}
				}
			}
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x001655B0 File Offset: 0x001637B0
		public OutfitData GetOutfitData()
		{
			OutfitData outfitData = new OutfitData();
			string text = this.Type.name + "/" + base.name;
			text = text.Replace("(Clone)", "");
			outfitData.outfit = text;
			if (this.customShader)
			{
				outfitData.color = this.GetColor(1);
				outfitData.swatch = this.currentSwatch;
			}
			else
			{
				outfitData.colors = this.GetColors();
				Texture decal = this.GetDecal();
				if (decal != null)
				{
					outfitData.decal = "Decal/" + decal.name;
					outfitData.decalColors = this.GetDecalColors();
					outfitData.decalScale = this.GetDecalSize();
				}
				else
				{
					outfitData.decal = "";
				}
				Texture pattern = this.GetPattern();
				if (pattern != null)
				{
					outfitData.pattern = "Pattern/" + pattern.name;
					outfitData.patternColors = this.GetPatternColors();
					outfitData.patternScale = this.GetPatternSize();
				}
				else
				{
					outfitData.pattern = "";
				}
			}
			return outfitData;
		}

		// Token: 0x06003C0A RID: 15370 RVA: 0x00004F31 File Offset: 0x00003131
		private void InitCloth()
		{
		}

		// Token: 0x06003C0B RID: 15371 RVA: 0x00004F31 File Offset: 0x00003131
		public void ActivateCloth(Dictionary<string, Transform> boneMap)
		{
		}

		// Token: 0x04003C18 RID: 15384
		[SerializeField]
		private bool AttachInEditMode;

		// Token: 0x04003C19 RID: 15385
		private OutfitSystem system;

		// Token: 0x04003C1A RID: 15386
		public string OutfitName;

		// Token: 0x04003C1B RID: 15387
		public Sprite OutfitIcon;

		// Token: 0x04003C1C RID: 15388
		public string[] ColorChannels = new string[] { "Base" };

		// Token: 0x04003C1D RID: 15389
		public string TextureCatagory;

		// Token: 0x04003C1E RID: 15390
		public bool supportDecals;

		// Token: 0x04003C1F RID: 15391
		public bool supportPatterns;

		// Token: 0x04003C20 RID: 15392
		public bool showCharacterCreator = true;

		// Token: 0x04003C24 RID: 15396
		[SerializeField]
		public OutfitType Type;

		// Token: 0x04003C25 RID: 15397
		public string AttachPoint;

		// Token: 0x04003C26 RID: 15398
		public Color[] defaultColors;

		// Token: 0x04003C27 RID: 15399
		public string[] tags;

		// Token: 0x04003C28 RID: 15400
		private Dictionary<string, int> tagShapes = new Dictionary<string, int>();

		// Token: 0x04003C29 RID: 15401
		private Dictionary<string, int> shapes = new Dictionary<string, int>();

		// Token: 0x04003C2A RID: 15402
		private readonly Dictionary<SkinnedMeshRenderer, Dictionary<string, int>> rendererShapes = new Dictionary<SkinnedMeshRenderer, Dictionary<string, int>>();

		// Token: 0x04003C2B RID: 15403
		public LinkedColorSets[] LinkedColorSets;

		// Token: 0x04003C2C RID: 15404
		public OutfitType[] IncompatibleSets;

		// Token: 0x04003C2D RID: 15405
		public int currentSwatch;

		// Token: 0x04003C2E RID: 15406
		public List<OutfitSwatch> outfitSwatches = new List<OutfitSwatch>();

		// Token: 0x04003C2F RID: 15407
		public Transform[] originalBones;

		// Token: 0x04003C30 RID: 15408
		public Transform originalRootBone;

		// Token: 0x04003C31 RID: 15409
		public Transform editorAttachPoint;
	}
}
