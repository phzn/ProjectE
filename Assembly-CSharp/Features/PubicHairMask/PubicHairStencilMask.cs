using System;
using System.Collections;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.Flash;
using Features.Inventory.Wearable;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.PubicHairMask
{
	// Token: 0x02000DC1 RID: 3521
	public class PubicHairStencilMask : MonoBehaviour
	{
		// Token: 0x060067ED RID: 26605 RVA: 0x0029937B File Offset: 0x0029757B
		public static void NotifyWindLift()
		{
			PubicHairStencilMask.s_windLiftUntil = Time.time + 0.3f;
		}

		// Token: 0x1700111F RID: 4383
		// (get) Token: 0x060067EE RID: 26606 RVA: 0x0029938D File Offset: 0x0029758D
		private static bool WindLiftActive
		{
			get
			{
				return Time.time < PubicHairStencilMask.s_windLiftUntil;
			}
		}

		// Token: 0x060067EF RID: 26607 RVA: 0x0029939B File Offset: 0x0029759B
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStaticState()
		{
			PubicHairStencilMask._instance = null;
			PubicHairStencilMask.s_windLiftUntil = 0f;
			PubicHairStencilMask._warnedMats.Clear();
		}

		// Token: 0x060067F0 RID: 26608 RVA: 0x002993B7 File Offset: 0x002975B7
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Bootstrap()
		{
			if (PubicHairStencilMask._instance != null)
			{
				return;
			}
			new GameObject("PubicHairStencilMask").AddComponent<PubicHairStencilMask>();
		}

		// Token: 0x060067F1 RID: 26609 RVA: 0x002993D7 File Offset: 0x002975D7
		private void Awake()
		{
			if (PubicHairStencilMask._instance != null && PubicHairStencilMask._instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			PubicHairStencilMask._instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x060067F2 RID: 26610 RVA: 0x00299410 File Offset: 0x00297610
		private void OnEnable()
		{
			if (PubicHairStencilMask._instance != this)
			{
				return;
			}
			SceneManager.sceneLoaded -= this.HandleSceneLoaded;
			SceneManager.sceneLoaded += this.HandleSceneLoaded;
			this.EnsurePlayerBindingCurrent(true, true);
		}

		// Token: 0x060067F3 RID: 26611 RVA: 0x0029944A File Offset: 0x0029764A
		private void OnDisable()
		{
			SceneManager.sceneLoaded -= this.HandleSceneLoaded;
			this.Unsubscribe();
			this._wornItems.Clear();
			base.StopAllCoroutines();
			this._bindingRoutine = null;
			this._delayedSweep = null;
		}

		// Token: 0x060067F4 RID: 26612 RVA: 0x00299482 File Offset: 0x00297682
		private void HandleSceneLoaded(Scene ignoredScene, LoadSceneMode ignoredMode)
		{
			this.EnsurePlayerBindingCurrent(false, true);
		}

		// Token: 0x060067F5 RID: 26613 RVA: 0x0019E9AB File Offset: 0x0019CBAB
		private static Transform ResolveRegisteredPlayer()
		{
			if (PlayerRegistry.Player != null)
			{
				return PlayerRegistry.Player;
			}
			if (!(GameSystemsBootstrapper.Instance != null))
			{
				return null;
			}
			return GameSystemsBootstrapper.Instance.Player;
		}

		// Token: 0x060067F6 RID: 26614 RVA: 0x0029948C File Offset: 0x0029768C
		private void EnsurePlayerBindingCurrent(bool refreshExisting = false, bool forceRetry = false)
		{
			Transform transform = PubicHairStencilMask.ResolveRegisteredPlayer();
			if (this._wearables != null && this._boundPlayer == transform && transform != null)
			{
				if (refreshExisting)
				{
					this.ActivateCurrentWearablesBinding();
				}
				return;
			}
			if (this._bindingTarget == transform && (this._bindingRoutine != null || (this._bindingRetryExhausted && !forceRetry)))
			{
				return;
			}
			this.RestartPlayerBinding(transform);
		}

		// Token: 0x060067F7 RID: 26615 RVA: 0x002994F8 File Offset: 0x002976F8
		private void RestartPlayerBinding(Transform registeredPlayer)
		{
			if (this._bindingRoutine != null)
			{
				base.StopCoroutine(this._bindingRoutine);
				this._bindingRoutine = null;
			}
			if (this._delayedSweep != null)
			{
				base.StopCoroutine(this._delayedSweep);
				this._delayedSweep = null;
			}
			this.Unsubscribe();
			this._wearables = null;
			this._wornItems.Clear();
			this._boundPlayer = null;
			this._bindingTarget = registeredPlayer;
			this._bindingRetryExhausted = false;
			this._styleService = null;
			this._processedMats.Clear();
			this.RestoreTrackedFurDefaults();
			this._pubicFurMats.Clear();
			PubicHairStencilMask.s_windLiftUntil = 0f;
			if (this.TryBindWearables(registeredPlayer))
			{
				return;
			}
			if (registeredPlayer == null)
			{
				this._bindingRetryExhausted = true;
				return;
			}
			this._bindingRoutine = base.StartCoroutine(this.WaitForPlayerBinding());
		}

		// Token: 0x060067F8 RID: 26616 RVA: 0x002995C4 File Offset: 0x002977C4
		private void RestoreTrackedFurDefaults()
		{
			foreach (KeyValuePair<Material, Vector4> keyValuePair in this._pubicFurMats)
			{
				Material key = keyValuePair.Key;
				if (!(key == null))
				{
					key.SetVector("_FurVector", keyValuePair.Value);
					key.SetFloat("_StencilComp", 6f);
					if (key.HasProperty("_FurStencilRef"))
					{
						key.SetFloat("_FurStencilComp", 6f);
					}
				}
			}
		}

		// Token: 0x060067F9 RID: 26617 RVA: 0x00299660 File Offset: 0x00297860
		public static IDisposable BeginAppearanceUpdate(Transform appearanceRoot)
		{
			PubicHairStencilMask instance = PubicHairStencilMask._instance;
			if (instance == null || !instance.enabled || instance._wearables == null || appearanceRoot == null || !instance._wearables.transform.IsChildOf(appearanceRoot))
			{
				return null;
			}
			PubicHairStencilMask pubicHairStencilMask = instance;
			int appearanceUpdateDepth = pubicHairStencilMask._appearanceUpdateDepth;
			pubicHairStencilMask._appearanceUpdateDepth = appearanceUpdateDepth + 1;
			if (appearanceUpdateDepth == 0)
			{
				instance.RestoreTrackedFurDefaults();
			}
			return new PubicHairStencilMask.AppearanceUpdateScope(instance);
		}

		// Token: 0x060067FA RID: 26618 RVA: 0x002996CF File Offset: 0x002978CF
		private IEnumerator WaitForPlayerBinding()
		{
			WaitForSeconds retryDelay = new WaitForSeconds(0.5f);
			int attempt = 0;
			while (attempt < 8 && base.isActiveAndEnabled && this._wearables == null)
			{
				yield return retryDelay;
				Transform transform = PubicHairStencilMask.ResolveRegisteredPlayer();
				if (transform != this._bindingTarget)
				{
					break;
				}
				this._bindingTarget = transform;
				if (this.TryBindWearables(transform))
				{
					this._bindingRoutine = null;
					yield break;
				}
				int num = attempt;
				attempt = num + 1;
			}
			this._bindingRoutine = null;
			this._bindingRetryExhausted = true;
			yield break;
		}

		// Token: 0x060067FB RID: 26619 RVA: 0x002996E0 File Offset: 0x002978E0
		private bool TryBindWearables(Transform player)
		{
			if (player == null)
			{
				return false;
			}
			WearableUseController componentInChildren = player.GetComponentInChildren<WearableUseController>(true);
			if (componentInChildren == null)
			{
				return false;
			}
			this._bindingTarget = player;
			this._boundPlayer = player;
			this._wearables = componentInChildren;
			this.ActivateCurrentWearablesBinding();
			return true;
		}

		// Token: 0x060067FC RID: 26620 RVA: 0x00299727 File Offset: 0x00297927
		private void ActivateCurrentWearablesBinding()
		{
			if (this._wearables == null)
			{
				return;
			}
			this.Subscribe();
			this.Sweep();
			this.RestartDeferredSweepSequence();
		}

		// Token: 0x060067FD RID: 26621 RVA: 0x0029974C File Offset: 0x0029794C
		private void Subscribe()
		{
			if (this._wearables == null || this._wearableEventsSubscribed)
			{
				return;
			}
			this._wearables.OnItemWorn += this.OnWearChanged;
			this._wearables.OnItemUnworn += this.OnWearChanged;
			this._wearableEventsSubscribed = true;
		}

		// Token: 0x060067FE RID: 26622 RVA: 0x002997A5 File Offset: 0x002979A5
		private void OnDestroy()
		{
			SceneManager.sceneLoaded -= this.HandleSceneLoaded;
			this.Unsubscribe();
			if (PubicHairStencilMask._instance == this)
			{
				PubicHairStencilMask._instance = null;
			}
		}

		// Token: 0x060067FF RID: 26623 RVA: 0x002997D4 File Offset: 0x002979D4
		private void Unsubscribe()
		{
			if (this._wearableEventsSubscribed && this._wearables != null)
			{
				this._wearables.OnItemWorn -= this.OnWearChanged;
				this._wearables.OnItemUnworn -= this.OnWearChanged;
			}
			this._wearableEventsSubscribed = false;
		}

		// Token: 0x06006800 RID: 26624 RVA: 0x0029982C File Offset: 0x00297A2C
		private void OnWearChanged(WearableItemSO _)
		{
			this.Sweep();
			this.RestartDeferredSweepSequence();
		}

		// Token: 0x06006801 RID: 26625 RVA: 0x0029983A File Offset: 0x00297A3A
		private void RestartDeferredSweepSequence()
		{
			if (this._delayedSweep != null)
			{
				base.StopCoroutine(this._delayedSweep);
			}
			this._delayedSweep = base.StartCoroutine(this.DelayedSweep());
		}

		// Token: 0x06006802 RID: 26626 RVA: 0x00299862 File Offset: 0x00297A62
		private IEnumerator DelayedSweep()
		{
			yield return null;
			this.Sweep();
			yield return new WaitForSeconds(1f);
			this.Sweep();
			yield return new WaitForSeconds(2f);
			this.Sweep();
			this._delayedSweep = null;
			yield break;
		}

		// Token: 0x06006803 RID: 26627 RVA: 0x00299874 File Offset: 0x00297A74
		public void Sweep()
		{
			if (this._wearables == null || this._appearanceUpdateDepth > 0)
			{
				return;
			}
			Transform root = this._wearables.transform.root;
			if (this._styleService == null)
			{
				this._styleService = root.GetComponentInChildren<MCStyleCustomizationService>(true);
			}
			PubicHairStencilMask.ConfigureMaterials(root, this._styleService, this._processedMats, this._pubicFurMats);
			this.UpdateFurSuppression();
		}

		// Token: 0x06006804 RID: 26628 RVA: 0x002998E4 File Offset: 0x00297AE4
		internal static void ConfigureMaterials(Transform root, MCStyleCustomizationService styleService, HashSet<int> processedMats, Dictionary<Material, Vector4> pubicFurMats)
		{
			foreach (Renderer renderer in root.GetComponentsInChildren<Renderer>(true))
			{
				if (!PubicHairStencilMask.IsDecalRenderer(renderer))
				{
					bool flag = PubicHairStencilMask.IsFaceAccessoryRenderer(renderer, styleService);
					Material[] sharedMaterials = renderer.sharedMaterials;
					bool flag2 = false;
					foreach (Material material in sharedMaterials)
					{
						if (!(material == null) && !processedMats.Contains(material.GetInstanceID()))
						{
							PubicHairStencilMask.WarnIfUnmaskableGarment(material, renderer);
							if (PubicHairStencilMask.IsFurMat(material) || flag || PubicHairStencilMask.IsClothMat(material, renderer))
							{
								flag2 = true;
								break;
							}
						}
					}
					if (flag2)
					{
						foreach (Material material2 in renderer.materials)
						{
							if (!(material2 == null) && processedMats.Add(material2.GetInstanceID()))
							{
								if (PubicHairStencilMask.IsFurMat(material2))
								{
									PubicHairStencilMask.ApplyHairSide(material2);
									if (material2.name.StartsWith("Pubic") && material2.HasProperty("_FurVector"))
									{
										pubicFurMats[material2] = material2.GetVector("_FurVector");
									}
								}
								else if (flag)
								{
									PubicHairStencilMask.ApplyBlushCover(material2);
								}
								else if (PubicHairStencilMask.IsClothMat(material2, renderer))
								{
									PubicHairStencilMask.ApplyClothSide(material2);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06006805 RID: 26629 RVA: 0x00299A30 File Offset: 0x00297C30
		private void UpdateFurSuppression()
		{
			if (this._wearables == null || this._pubicFurMats.Count == 0 || this._appearanceUpdateDepth > 0)
			{
				return;
			}
			this._wearables.CopyWornItemsTo(this._wornItems);
			float num = 1f;
			bool flag = false;
			foreach (WearableItemSO wearableItemSO in this._wornItems)
			{
				if (!(wearableItemSO == null) && wearableItemSO.physicallyCoversBottom && this._wearables.IsWorn(wearableItemSO))
				{
					num = Mathf.Min(num, this.FurFactorFor(wearableItemSO));
					if (PubicHairStencilMask.SkirtStencilBypassItems.Contains(wearableItemSO.itemId))
					{
						flag = true;
					}
				}
			}
			bool flag2 = this.AnyFlashGateEngaged() || PubicHairStencilMask.WindLiftActive;
			if (flag2)
			{
				num = 1f;
			}
			bool flag3 = flag2 || (flag && num >= 1f);
			PubicHairStencilMask.ApplyFurSuppression(this._pubicFurMats, num, flag3);
		}

		// Token: 0x06006806 RID: 26630 RVA: 0x00299B40 File Offset: 0x00297D40
		internal static void ApplyFurSuppression(Dictionary<Material, Vector4> pubicFurMats, float factor, bool stencilBypass)
		{
			float num = (float)(stencilBypass ? 8 : 6);
			foreach (KeyValuePair<Material, Vector4> keyValuePair in pubicFurMats)
			{
				Material key = keyValuePair.Key;
				if (!(key == null))
				{
					Vector4 value = keyValuePair.Value;
					key.SetVector("_FurVector", (factor >= 1f) ? value : new Vector4(value.x, value.y, (factor <= 0f) ? 0f : value.z, value.w * factor));
					key.SetFloat("_StencilComp", num);
					if (key.HasProperty("_FurStencilRef"))
					{
						key.SetFloat("_FurStencilComp", num);
					}
				}
			}
		}

		// Token: 0x06006807 RID: 26631 RVA: 0x00299C20 File Offset: 0x00297E20
		private float FurFactorFor(WearableItemSO item)
		{
			return PubicHairStencilMask.FurFactorFor(item, (this._styleService != null) ? this._styleService.GetSlotSurfaceIndex("Outfit.Swimsuit") : 0);
		}

		// Token: 0x06006808 RID: 26632 RVA: 0x00299C49 File Offset: 0x00297E49
		internal static bool BypassesStencil(WearableItemSO item)
		{
			return item != null && PubicHairStencilMask.SkirtStencilBypassItems.Contains(item.itemId);
		}

		// Token: 0x06006809 RID: 26633 RVA: 0x00299C68 File Offset: 0x00297E68
		internal static float FurFactorFor(WearableItemSO item, int variant)
		{
			if (item.itemId == "cloth_swimsuit")
			{
				if (variant < 0 || variant >= PubicHairStencilMask.SwimsuitVariantFactors.Length)
				{
					return 0f;
				}
				return PubicHairStencilMask.SwimsuitVariantFactors[variant];
			}
			else
			{
				float num;
				if (!PubicHairStencilMask.FurFactorByItem.TryGetValue(item.itemId, out num))
				{
					return 0f;
				}
				return num;
			}
		}

		// Token: 0x0600680A RID: 26634 RVA: 0x00299CBE File Offset: 0x00297EBE
		private void Update()
		{
			this.EnsurePlayerBindingCurrent(false, false);
			this.UpdateFurSuppression();
		}

		// Token: 0x0600680B RID: 26635 RVA: 0x00299CCE File Offset: 0x00297ECE
		private bool AnyFlashGateEngaged()
		{
			return FlashGateRegistry.AnyFlashEngaged;
		}

		// Token: 0x0600680C RID: 26636 RVA: 0x00299CD8 File Offset: 0x00297ED8
		private static void WarnIfUnmaskableGarment(Material m, Renderer r)
		{
			if (!(r is SkinnedMeshRenderer) || m.HasProperty("_StencilRef"))
			{
				return;
			}
			if (m.shader == null || m.shader.name != "Universal Render Pipeline/Lit")
			{
				return;
			}
			if (PubicHairStencilMask._warnedMats.Add(m.name))
			{
				Debug.LogWarning(string.Concat(new string[] { "[PubicHairMask] Garment '", r.name, "' uses URP/Lit material '", m.name, "' (no stencil properties) - fur can clip through it. Swap it to lilToon." }), r);
			}
		}

		// Token: 0x0600680D RID: 26637 RVA: 0x00299D70 File Offset: 0x00297F70
		private static bool IsDecalRenderer(Renderer r)
		{
			if (r.name.StartsWith("Decal_"))
			{
				return true;
			}
			Transform parent = r.transform.parent;
			return parent != null && (parent.name == "BodyDecals" || parent.name == "HeadDecals");
		}

		// Token: 0x0600680E RID: 26638 RVA: 0x00299DCC File Offset: 0x00297FCC
		private static bool IsFurMat(Material m)
		{
			return m.shader != null && m.shader.name.Contains("Fur");
		}

		// Token: 0x0600680F RID: 26639 RVA: 0x00299DF3 File Offset: 0x00297FF3
		private static bool HasAuthoredStencil(Material m)
		{
			return m.GetFloat("_StencilRef") != 0f || m.GetFloat("_StencilPass") != 0f;
		}

		// Token: 0x06006810 RID: 26640 RVA: 0x00299E20 File Offset: 0x00298020
		private static bool IsFaceAccessoryRenderer(Renderer r, MCStyleCustomizationService styleService)
		{
			if (styleService != null)
			{
				foreach (string text in PubicHairStencilMask.FaceAccessorySlots)
				{
					GameObject spawnedRoot = styleService.GetSpawnedRoot(text);
					if (spawnedRoot != null && r.transform.IsChildOf(spawnedRoot.transform))
					{
						return true;
					}
				}
			}
			string text2 = r.name.ToLowerInvariant();
			return text2.Contains("mask") || text2.Contains("gag") || text2.Contains("blindfold") || text2.Contains("eyewear") || text2.Contains("glasses") || text2.Contains("shorts_head_face");
		}

		// Token: 0x06006811 RID: 26641 RVA: 0x00299ED4 File Offset: 0x002980D4
		private static bool IsClothMat(Material m, Renderer r)
		{
			if (m.shader == null || !m.HasProperty("_StencilRef"))
			{
				return false;
			}
			if (m.shader.name.Contains("Fur"))
			{
				return false;
			}
			if (PubicHairStencilMask.HasAuthoredStencil(m))
			{
				return false;
			}
			string text = m.name.ToLowerInvariant();
			return !text.StartsWith("decalmat_") && !text.Contains("mat_ciel") && !text.Contains("raincoat") && !text.Contains("bath") && !text.Contains("towel") && !r.name.ToLowerInvariant().Contains("hair");
		}

		// Token: 0x06006812 RID: 26642 RVA: 0x00299F90 File Offset: 0x00298190
		private static void ApplyBlushCover(Material m)
		{
			if (m.shader == null || !m.HasProperty("_StencilRef") || PubicHairStencilMask.HasAuthoredStencil(m))
			{
				return;
			}
			m.SetFloat("_StencilRef", 1f);
			m.SetFloat("_StencilReadMask", 1f);
			m.SetFloat("_StencilWriteMask", 1f);
			m.SetFloat("_StencilComp", 8f);
			m.SetFloat("_StencilPass", 2f);
			m.SetFloat("_StencilFail", 0f);
			m.SetFloat("_StencilZFail", 0f);
		}

		// Token: 0x06006813 RID: 26643 RVA: 0x0029A034 File Offset: 0x00298234
		private static void ApplyHairSide(Material m)
		{
			m.SetFloat("_StencilRef", 32f);
			m.SetFloat("_StencilReadMask", 32f);
			m.SetFloat("_StencilWriteMask", 32f);
			m.SetFloat("_StencilComp", 6f);
			m.SetFloat("_StencilPass", 0f);
			if (m.HasProperty("_FurStencilRef"))
			{
				m.SetFloat("_FurStencilRef", 32f);
				m.SetFloat("_FurStencilReadMask", 32f);
				m.SetFloat("_FurStencilWriteMask", 32f);
				m.SetFloat("_FurStencilComp", 6f);
				m.SetFloat("_FurStencilPass", 0f);
			}
			m.renderQueue = 3000;
		}

		// Token: 0x06006814 RID: 26644 RVA: 0x0029A0FC File Offset: 0x002982FC
		private static void ApplyClothSide(Material m)
		{
			m.SetFloat("_StencilRef", 32f);
			m.SetFloat("_StencilReadMask", 32f);
			m.SetFloat("_StencilWriteMask", 32f);
			m.SetFloat("_StencilComp", 8f);
			m.SetFloat("_StencilPass", 2f);
			m.SetFloat("_StencilFail", 0f);
			m.SetFloat("_StencilZFail", 0f);
		}

		// Token: 0x06006816 RID: 26646 RVA: 0x0029A1A4 File Offset: 0x002983A4
		// Note: this type is marked as 'beforefieldinit'.
		static PubicHairStencilMask()
		{
			float[] array = new float[5];
			array[0] = 0.1f;
			array[3] = 0.1f;
			PubicHairStencilMask.SwimsuitVariantFactors = array;
			PubicHairStencilMask._warnedMats = new HashSet<string>();
			PubicHairStencilMask.FaceAccessorySlots = new string[] { "Outfit.Mask", "Outfit.Blindfold", "Accessory.Eyewear" };
		}

		// Token: 0x04006A63 RID: 27235
		private const int StencilBit = 32;

		// Token: 0x04006A64 RID: 27236
		private const int CompNotEqual = 6;

		// Token: 0x04006A65 RID: 27237
		private const int CompAlways = 8;

		// Token: 0x04006A66 RID: 27238
		private const int OpKeep = 0;

		// Token: 0x04006A67 RID: 27239
		private const int OpReplace = 2;

		// Token: 0x04006A68 RID: 27240
		private const int HairRenderQueue = 3000;

		// Token: 0x04006A69 RID: 27241
		private const int BindingRetryAttempts = 8;

		// Token: 0x04006A6A RID: 27242
		private static PubicHairStencilMask _instance;

		// Token: 0x04006A6B RID: 27243
		private WearableUseController _wearables;

		// Token: 0x04006A6C RID: 27244
		private Transform _boundPlayer;

		// Token: 0x04006A6D RID: 27245
		private Transform _bindingTarget;

		// Token: 0x04006A6E RID: 27246
		private readonly HashSet<int> _processedMats = new HashSet<int>();

		// Token: 0x04006A6F RID: 27247
		private Coroutine _bindingRoutine;

		// Token: 0x04006A70 RID: 27248
		private Coroutine _delayedSweep;

		// Token: 0x04006A71 RID: 27249
		private bool _wearableEventsSubscribed;

		// Token: 0x04006A72 RID: 27250
		private bool _bindingRetryExhausted;

		// Token: 0x04006A73 RID: 27251
		private int _appearanceUpdateDepth;

		// Token: 0x04006A74 RID: 27252
		private readonly Dictionary<Material, Vector4> _pubicFurMats = new Dictionary<Material, Vector4>();

		// Token: 0x04006A75 RID: 27253
		private readonly List<WearableItemSO> _wornItems = new List<WearableItemSO>(8);

		// Token: 0x04006A76 RID: 27254
		private static float s_windLiftUntil;

		// Token: 0x04006A77 RID: 27255
		private const float WindLiftGrace = 0.3f;

		// Token: 0x04006A78 RID: 27256
		private static readonly Dictionary<string, float> FurFactorByItem = new Dictionary<string, float>
		{
			{ "cloth_raincoat", 1f },
			{ "cloth_towel", 1f },
			{ "cloth_expose_panties", 1f },
			{ "cloth_onepiece_swimsuit", 0.1f },
			{ "cloth_bikini_panties", 0.1f },
			{ "cloth_micro_bottom", 0.1f },
			{ "cloth_cheerleader_suite", 1f },
			{ "cloth_university_uniform_full", 1f },
			{ "cloth_university_uniform_skirt", 1f },
			{ "cloth_bandage_pussy", 1f }
		};

		// Token: 0x04006A79 RID: 27257
		private static readonly HashSet<string> SkirtStencilBypassItems = new HashSet<string> { "cloth_cheerleader_suite", "cloth_university_uniform_full", "cloth_university_uniform_skirt" };

		// Token: 0x04006A7A RID: 27258
		private const string SwimsuitItemId = "cloth_swimsuit";

		// Token: 0x04006A7B RID: 27259
		private const string SwimsuitSlotId = "Outfit.Swimsuit";

		// Token: 0x04006A7C RID: 27260
		private static readonly float[] SwimsuitVariantFactors;

		// Token: 0x04006A7D RID: 27261
		private MCStyleCustomizationService _styleService;

		// Token: 0x04006A7E RID: 27262
		private static readonly HashSet<string> _warnedMats;

		// Token: 0x04006A7F RID: 27263
		private static readonly string[] FaceAccessorySlots;

		// Token: 0x02000DC2 RID: 3522
		private sealed class AppearanceUpdateScope : IDisposable
		{
			// Token: 0x06006817 RID: 26647 RVA: 0x0029A2D1 File Offset: 0x002984D1
			public AppearanceUpdateScope(PubicHairStencilMask owner)
			{
				this._owner = owner;
			}

			// Token: 0x06006818 RID: 26648 RVA: 0x0029A2E0 File Offset: 0x002984E0
			public void Dispose()
			{
				PubicHairStencilMask owner = this._owner;
				this._owner = null;
				if (!(owner == null))
				{
					PubicHairStencilMask pubicHairStencilMask = owner;
					int num = pubicHairStencilMask._appearanceUpdateDepth - 1;
					pubicHairStencilMask._appearanceUpdateDepth = num;
					if (num <= 0)
					{
						owner._processedMats.Clear();
						owner._pubicFurMats.Clear();
						owner.Sweep();
						if (Application.isPlaying && owner.isActiveAndEnabled)
						{
							owner.RestartDeferredSweepSequence();
						}
						return;
					}
				}
			}

			// Token: 0x04006A80 RID: 27264
			private PubicHairStencilMask _owner;
		}
	}
}
