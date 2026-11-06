using System;
using System.Collections;
using System.Collections.Generic;
using Features.BodyPhysics;
using MagicaCloth2;
using UnityEngine;

namespace BoZo.CharacterCreator.MC.StyleSystem
{
	// Token: 0x02000805 RID: 2053
	[DisallowMultipleComponent]
	public sealed class WardrobeBodyPhysicsController : MonoBehaviour
	{
		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x060036A2 RID: 13986 RVA: 0x0013B2E4 File Offset: 0x001394E4
		// (set) Token: 0x060036A3 RID: 13987 RVA: 0x0013B2EC File Offset: 0x001394EC
		public float CurrentBreastWeight { get; private set; } = 1f;

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x060036A4 RID: 13988 RVA: 0x0013B2F5 File Offset: 0x001394F5
		// (set) Token: 0x060036A5 RID: 13989 RVA: 0x0013B2FD File Offset: 0x001394FD
		public float CurrentButtWeight { get; private set; } = 1f;

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x060036A6 RID: 13990 RVA: 0x0013B306 File Offset: 0x00139506
		public int BreastClothCount
		{
			get
			{
				return this._breastCloths.Count;
			}
		}

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x060036A7 RID: 13991 RVA: 0x0013B313 File Offset: 0x00139513
		public int ButtClothCount
		{
			get
			{
				return this._buttCloths.Count;
			}
		}

		// Token: 0x060036A8 RID: 13992 RVA: 0x0013B320 File Offset: 0x00139520
		private void Awake()
		{
			this.ResolveDependencies();
			this.CacheBodyCloths();
		}

		// Token: 0x060036A9 RID: 13993 RVA: 0x0013B330 File Offset: 0x00139530
		private void OnEnable()
		{
			this.ResolveDependencies();
			this.CacheBodyCloths();
			AnimeBodyPhysics.Ensure(base.gameObject);
			OpenGarmentBodyPhysics.Ensure(base.gameObject);
			this.Subscribe();
			if (this._bodyShapes != null)
			{
				this._bodyShapes.AuthoredValuesChanged += this.RefreshBreastSize;
			}
			this.RefreshBreastSize();
			this.QueueRefresh();
		}

		// Token: 0x060036AA RID: 13994 RVA: 0x0013B398 File Offset: 0x00139598
		private void OnDisable()
		{
			this.Unsubscribe();
			if (this._bodyShapes != null)
			{
				this._bodyShapes.AuthoredValuesChanged -= this.RefreshBreastSize;
			}
			if (this._transition != null)
			{
				base.StopCoroutine(this._transition);
			}
			if (this._deferredRefresh != null)
			{
				base.StopCoroutine(this._deferredRefresh);
			}
			this._transition = null;
			this._deferredRefresh = null;
			this._externalPhysicsCaps.Clear();
			this._openGarments.Clear();
			this.RestoreBaseWeights();
		}

		// Token: 0x060036AB RID: 13995 RVA: 0x0013B424 File Offset: 0x00139624
		private void Update()
		{
			bool flag = this._awaitingClothReplacement;
			if (!flag)
			{
				using (Dictionary<MagicaCloth, float>.KeyCollection.Enumerator enumerator = this._baseBlendWeights.Keys.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current == null)
						{
							flag = true;
							break;
						}
					}
				}
			}
			if (!flag || Time.unscaledTime < this._nextClothRescan)
			{
				return;
			}
			this._nextClothRescan = Time.unscaledTime + 0.1f;
			this.ApplyActiveWardrobeProfile();
		}

		// Token: 0x060036AC RID: 13996 RVA: 0x0013B4B4 File Offset: 0x001396B4
		public void SetExternalPhysicsCap(string sourceId, float breastCap, float buttCap)
		{
			if (string.IsNullOrWhiteSpace(sourceId))
			{
				return;
			}
			this._externalPhysicsCaps[sourceId] = new Vector2(Mathf.Clamp01(breastCap), Mathf.Clamp01(buttCap));
			this.ApplyActiveWardrobeProfile();
		}

		// Token: 0x060036AD RID: 13997 RVA: 0x0013B4E2 File Offset: 0x001396E2
		public void ClearExternalPhysicsCap(string sourceId)
		{
			if (string.IsNullOrWhiteSpace(sourceId))
			{
				return;
			}
			if (this._externalPhysicsCaps.Remove(sourceId))
			{
				this.ApplyActiveWardrobeProfile();
			}
		}

		// Token: 0x060036AE RID: 13998 RVA: 0x0013B504 File Offset: 0x00139704
		public void SetGarmentOpening(string sourceId, string presetId, bool breasts, bool butt)
		{
			if (string.IsNullOrWhiteSpace(sourceId))
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(presetId) || (!breasts && !butt))
			{
				this.ClearGarmentOpening(sourceId);
				return;
			}
			WardrobeBodyPhysicsController.GarmentOpening garmentOpening = new WardrobeBodyPhysicsController.GarmentOpening(presetId, breasts, butt);
			WardrobeBodyPhysicsController.GarmentOpening garmentOpening2;
			if (this._openGarments.TryGetValue(sourceId, out garmentOpening2) && garmentOpening2.Equals(garmentOpening))
			{
				return;
			}
			this._openGarments[sourceId] = garmentOpening;
			this.ApplyActiveWardrobeProfile();
		}

		// Token: 0x060036AF RID: 13999 RVA: 0x0013B56A File Offset: 0x0013976A
		public void ClearGarmentOpening(string sourceId)
		{
			if (string.IsNullOrWhiteSpace(sourceId))
			{
				return;
			}
			if (this._openGarments.Remove(sourceId))
			{
				this.ApplyActiveWardrobeProfile();
			}
		}

		// Token: 0x060036B0 RID: 14000 RVA: 0x0013B58C File Offset: 0x0013978C
		private bool IsOpenOver(string presetId, bool breasts)
		{
			foreach (WardrobeBodyPhysicsController.GarmentOpening garmentOpening in this._openGarments.Values)
			{
				if ((breasts ? garmentOpening.Breasts : garmentOpening.Butt) && string.Equals(garmentOpening.PresetId, presetId, StringComparison.Ordinal))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060036B1 RID: 14001 RVA: 0x0013B608 File Offset: 0x00139808
		private void ResolveDependencies()
		{
			if (this.styleService == null)
			{
				this.styleService = base.GetComponentInChildren<MCStyleCustomizationService>(true);
			}
			if (this._bodyShapes == null)
			{
				this._bodyShapes = base.GetComponentInChildren<CharacterBlendShapeController>(true);
			}
			BlendShapeMapSO.BlendShapeEntry blendShapeEntry;
			if (!(this._bodyShapes != null) || !(this._bodyShapes.BlendShapeMap != null))
			{
				blendShapeEntry = null;
			}
			else
			{
				blendShapeEntry = this._bodyShapes.BlendShapeMap.blendShapeMappings.Find((BlendShapeMapSO.BlendShapeEntry entry) => entry != null && string.Equals(entry.displayName, "Breast Size", StringComparison.OrdinalIgnoreCase));
			}
			this._breastSizeEntry = blendShapeEntry;
		}

		// Token: 0x060036B2 RID: 14002 RVA: 0x0013B6AC File Offset: 0x001398AC
		private void RefreshBreastSize()
		{
			if (this._breastSizeEntry != null)
			{
				this.SetBreastSize(Mathf.RoundToInt(this._bodyShapes.GetEntryAuthoredUiValue(this._breastSizeEntry)));
				return;
			}
			BoobsController componentInChildren = base.GetComponentInChildren<BoobsController>(true);
			if (componentInChildren != null)
			{
				this.SetBreastSize(componentInChildren.CurrentBoobsTypeValue);
			}
		}

		// Token: 0x060036B3 RID: 14003 RVA: 0x0013B6FB File Offset: 0x001398FB
		public void SetBreastSize(int value)
		{
			value = Mathf.Clamp(value, 0, 5);
			if (this._breastSize == value)
			{
				return;
			}
			this._breastSize = value;
			this.ApplyActiveWardrobeProfile();
		}

		// Token: 0x060036B4 RID: 14004 RVA: 0x0013B720 File Offset: 0x00139920
		private void CacheBodyCloths()
		{
			this._breastCloths.Clear();
			this._buttCloths.Clear();
			this._staleCloths.Clear();
			foreach (MagicaCloth magicaCloth in this._baseBlendWeights.Keys)
			{
				if (magicaCloth == null)
				{
					this._staleCloths.Add(magicaCloth);
				}
			}
			foreach (MagicaCloth magicaCloth2 in this._staleCloths)
			{
				this._baseBlendWeights.Remove(magicaCloth2);
			}
			foreach (MagicaCloth magicaCloth3 in base.GetComponentsInChildren<MagicaCloth>(true))
			{
				if (!(magicaCloth3 == null))
				{
					bool flag = this.MatchesBreastCloth(magicaCloth3.gameObject.name);
					bool flag2 = WardrobeBodyPhysicsController.Matches(magicaCloth3.gameObject.name, this.buttClothNames);
					if (flag)
					{
						this._breastCloths.Add(magicaCloth3);
					}
					else if (flag2)
					{
						this._buttCloths.Add(magicaCloth3);
					}
					if ((flag || flag2) && !this._baseBlendWeights.ContainsKey(magicaCloth3))
					{
						float blendWeight;
						bool flag3 = this._baseWeightsByObject.TryGetValue(magicaCloth3.gameObject, out blendWeight);
						if (!flag3)
						{
							blendWeight = magicaCloth3.SerializeData.blendWeight;
							this._baseWeightsByObject.Add(magicaCloth3.gameObject, blendWeight);
						}
						this._baseBlendWeights.Add(magicaCloth3, blendWeight);
						if (flag3)
						{
							magicaCloth3.SerializeData.blendWeight = blendWeight * (flag ? this.CurrentBreastWeight : this.CurrentButtWeight);
							magicaCloth3.SetParameterChange();
						}
					}
				}
			}
			this._awaitingClothReplacement = false;
			foreach (GameObject gameObject in this._baseWeightsByObject.Keys)
			{
				if (gameObject != null && gameObject.GetComponent<MagicaCloth>() == null)
				{
					this._awaitingClothReplacement = true;
					break;
				}
			}
		}

		// Token: 0x060036B5 RID: 14005 RVA: 0x0013B96C File Offset: 0x00139B6C
		private bool MatchesBreastCloth(string value)
		{
			return WardrobeBodyPhysicsController.Matches(value, this.breastClothNames) || (value.EndsWith("_HandstandGravity", StringComparison.Ordinal) && WardrobeBodyPhysicsController.Matches(value.Substring(0, value.Length - "_HandstandGravity".Length), this.breastClothNames));
		}

		// Token: 0x060036B6 RID: 14006 RVA: 0x0013B9BC File Offset: 0x00139BBC
		private static bool Matches(string value, string[] candidates)
		{
			if (candidates == null)
			{
				return false;
			}
			for (int i = 0; i < candidates.Length; i++)
			{
				if (string.Equals(value, candidates[i], StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060036B7 RID: 14007 RVA: 0x0013B9EC File Offset: 0x00139BEC
		private void Subscribe()
		{
			if (this.styleService == null)
			{
				return;
			}
			this.styleService.OnActiveStateChanged -= this.QueueRefresh;
			this.styleService.OnSlotVisibilityChanged -= this.QueueRefresh;
			this.styleService.OnPhysicalSwapCompleted -= this.OnPhysicalChanged;
			this.styleService.OnPhysicalRemoved -= this.OnPhysicalChanged;
			this.styleService.OnActiveStateChanged += this.QueueRefresh;
			this.styleService.OnSlotVisibilityChanged += this.QueueRefresh;
			this.styleService.OnPhysicalSwapCompleted += this.OnPhysicalChanged;
			this.styleService.OnPhysicalRemoved += this.OnPhysicalChanged;
		}

		// Token: 0x060036B8 RID: 14008 RVA: 0x0013BAC0 File Offset: 0x00139CC0
		private void Unsubscribe()
		{
			if (this.styleService == null)
			{
				return;
			}
			this.styleService.OnActiveStateChanged -= this.QueueRefresh;
			this.styleService.OnSlotVisibilityChanged -= this.QueueRefresh;
			this.styleService.OnPhysicalSwapCompleted -= this.OnPhysicalChanged;
			this.styleService.OnPhysicalRemoved -= this.OnPhysicalChanged;
		}

		// Token: 0x060036B9 RID: 14009 RVA: 0x0013BB38 File Offset: 0x00139D38
		private void OnPhysicalChanged(MCStylePartDefinitionSO _, GameObject __)
		{
			this.QueueRefresh();
		}

		// Token: 0x060036BA RID: 14010 RVA: 0x0013BB40 File Offset: 0x00139D40
		public void QueueRefresh()
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this._deferredRefresh != null)
			{
				base.StopCoroutine(this._deferredRefresh);
			}
			this._deferredRefresh = base.StartCoroutine(this.RefreshNextFrame());
		}

		// Token: 0x060036BB RID: 14011 RVA: 0x0013BB71 File Offset: 0x00139D71
		private IEnumerator RefreshNextFrame()
		{
			yield return null;
			this._deferredRefresh = null;
			this.ApplyActiveWardrobeProfile();
			yield break;
		}

		// Token: 0x060036BC RID: 14012 RVA: 0x0013BB80 File Offset: 0x00139D80
		public void ApplyActiveWardrobeProfile()
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			this.CacheBodyCloths();
			float num = ((this._breastSize == 0) ? 0f : 1f);
			float num2 = 1f;
			if (this.styleService != null)
			{
				IReadOnlyList<MCStylePartDefinitionSO> partDefinitions = this.styleService.PartDefinitions;
				for (int i = 0; i < partDefinitions.Count; i++)
				{
					MCStylePartDefinitionSO mcstylePartDefinitionSO = partDefinitions[i];
					if (!(mcstylePartDefinitionSO == null) && !string.IsNullOrWhiteSpace(mcstylePartDefinitionSO.partId))
					{
						MCStylePresetSO activePreset = this.styleService.GetActivePreset(mcstylePartDefinitionSO.partId);
						GameObject spawnedRoot = this.styleService.GetSpawnedRoot(mcstylePartDefinitionSO.partId);
						if (!(activePreset == null) && activePreset.overrideBodyPhysics && WardrobeBodyPhysicsController.HasVisibleRenderer(spawnedRoot))
						{
							if (!this.IsOpenOver(activePreset.presetId, true))
							{
								num = Mathf.Min(num, Mathf.Clamp01(activePreset.breastPhysicsWeight));
							}
							if (!this.IsOpenOver(activePreset.presetId, false))
							{
								num2 = Mathf.Min(num2, Mathf.Clamp01(activePreset.buttPhysicsWeight));
							}
						}
					}
				}
			}
			foreach (Vector2 vector in this._externalPhysicsCaps.Values)
			{
				num = Mathf.Min(num, vector.x);
				num2 = Mathf.Min(num2, vector.y);
			}
			this.CurrentBreastWeight = num;
			this.CurrentButtWeight = num2;
			if (this._transition != null)
			{
				base.StopCoroutine(this._transition);
			}
			this._transition = base.StartCoroutine(this.BlendToTargets(num, num2));
		}

		// Token: 0x060036BD RID: 14013 RVA: 0x0013BD34 File Offset: 0x00139F34
		private static bool HasVisibleRenderer(GameObject root)
		{
			if (root == null || !root.activeInHierarchy)
			{
				return false;
			}
			Renderer[] componentsInChildren = root.GetComponentsInChildren<Renderer>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != null && componentsInChildren[i].enabled && componentsInChildren[i].gameObject.activeInHierarchy)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060036BE RID: 14014 RVA: 0x0013BD91 File Offset: 0x00139F91
		private IEnumerator BlendToTargets(float breastWeight, float buttWeight)
		{
			Dictionary<MagicaCloth, float> starts = new Dictionary<MagicaCloth, float>();
			foreach (MagicaCloth magicaCloth in this._baseBlendWeights.Keys)
			{
				if (magicaCloth != null)
				{
					starts[magicaCloth] = magicaCloth.SerializeData.blendWeight;
				}
			}
			float elapsed = 0f;
			float duration = Mathf.Max(this.transitionDuration, 0.0001f);
			do
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / duration));
				this.ApplyWeights(starts, breastWeight, buttWeight, num);
				yield return null;
			}
			while (elapsed < duration);
			this.ApplyWeights(starts, breastWeight, buttWeight, 1f);
			this._transition = null;
			yield break;
		}

		// Token: 0x060036BF RID: 14015 RVA: 0x0013BDAE File Offset: 0x00139FAE
		private void ApplyWeights(Dictionary<MagicaCloth, float> starts, float breasts, float butt, float t)
		{
			this.ApplyGroup(this._breastCloths, starts, breasts, t);
			this.ApplyGroup(this._buttCloths, starts, butt, t);
		}

		// Token: 0x060036C0 RID: 14016 RVA: 0x0013BDD0 File Offset: 0x00139FD0
		private void ApplyGroup(List<MagicaCloth> cloths, Dictionary<MagicaCloth, float> starts, float multiplier, float t)
		{
			for (int i = 0; i < cloths.Count; i++)
			{
				MagicaCloth magicaCloth = cloths[i];
				float num;
				if (!(magicaCloth == null) && this._baseBlendWeights.TryGetValue(magicaCloth, out num))
				{
					float num3;
					float num2 = (starts.TryGetValue(magicaCloth, out num3) ? num3 : magicaCloth.SerializeData.blendWeight);
					bool flag = cloths == this._breastCloths && (this._breastSize == 0 || multiplier <= 0f);
					magicaCloth.SerializeData.blendWeight = (flag ? 0f : Mathf.Lerp(num2, num * multiplier, t));
					magicaCloth.SetParameterChange();
				}
			}
		}

		// Token: 0x060036C1 RID: 14017 RVA: 0x0013BE7C File Offset: 0x0013A07C
		private void RestoreBaseWeights()
		{
			foreach (KeyValuePair<MagicaCloth, float> keyValuePair in this._baseBlendWeights)
			{
				if (!(keyValuePair.Key == null))
				{
					keyValuePair.Key.SerializeData.blendWeight = keyValuePair.Value;
					keyValuePair.Key.SetParameterChange();
				}
			}
			this.CurrentBreastWeight = 1f;
			this.CurrentButtWeight = 1f;
		}

		// Token: 0x040036E0 RID: 14048
		[SerializeField]
		private MCStyleCustomizationService styleService;

		// Token: 0x040036E1 RID: 14049
		[SerializeField]
		[Min(0f)]
		private float transitionDuration = 0.3f;

		// Token: 0x040036E2 RID: 14050
		[SerializeField]
		private string[] breastClothNames = new string[] { "BoobL", "BoobR" };

		// Token: 0x040036E3 RID: 14051
		[SerializeField]
		private string[] buttClothNames = new string[] { "ButtL", "ButtR" };

		// Token: 0x040036E4 RID: 14052
		private readonly Dictionary<MagicaCloth, float> _baseBlendWeights = new Dictionary<MagicaCloth, float>();

		// Token: 0x040036E5 RID: 14053
		private readonly Dictionary<GameObject, float> _baseWeightsByObject = new Dictionary<GameObject, float>();

		// Token: 0x040036E6 RID: 14054
		private readonly List<MagicaCloth> _staleCloths = new List<MagicaCloth>();

		// Token: 0x040036E7 RID: 14055
		private readonly List<MagicaCloth> _breastCloths = new List<MagicaCloth>();

		// Token: 0x040036E8 RID: 14056
		private readonly List<MagicaCloth> _buttCloths = new List<MagicaCloth>();

		// Token: 0x040036E9 RID: 14057
		private readonly Dictionary<string, Vector2> _externalPhysicsCaps = new Dictionary<string, Vector2>(StringComparer.Ordinal);

		// Token: 0x040036EA RID: 14058
		private readonly Dictionary<string, WardrobeBodyPhysicsController.GarmentOpening> _openGarments = new Dictionary<string, WardrobeBodyPhysicsController.GarmentOpening>(StringComparer.Ordinal);

		// Token: 0x040036EB RID: 14059
		private Coroutine _transition;

		// Token: 0x040036EC RID: 14060
		private Coroutine _deferredRefresh;

		// Token: 0x040036ED RID: 14061
		private CharacterBlendShapeController _bodyShapes;

		// Token: 0x040036EE RID: 14062
		private BlendShapeMapSO.BlendShapeEntry _breastSizeEntry;

		// Token: 0x040036EF RID: 14063
		private int _breastSize = 3;

		// Token: 0x040036F0 RID: 14064
		private bool _awaitingClothReplacement;

		// Token: 0x040036F1 RID: 14065
		private float _nextClothRescan;

		// Token: 0x02000806 RID: 2054
		private readonly struct GarmentOpening : IEquatable<WardrobeBodyPhysicsController.GarmentOpening>
		{
			// Token: 0x060036C3 RID: 14019 RVA: 0x0013BFDE File Offset: 0x0013A1DE
			public GarmentOpening(string presetId, bool breasts, bool butt)
			{
				this.PresetId = presetId;
				this.Breasts = breasts;
				this.Butt = butt;
			}

			// Token: 0x060036C4 RID: 14020 RVA: 0x0013BFF5 File Offset: 0x0013A1F5
			public bool Equals(WardrobeBodyPhysicsController.GarmentOpening other)
			{
				return this.Breasts == other.Breasts && this.Butt == other.Butt && string.Equals(this.PresetId, other.PresetId, StringComparison.Ordinal);
			}

			// Token: 0x040036F4 RID: 14068
			public readonly string PresetId;

			// Token: 0x040036F5 RID: 14069
			public readonly bool Breasts;

			// Token: 0x040036F6 RID: 14070
			public readonly bool Butt;
		}
	}
}
