using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace MapGeneration.Runtime
{
	// Token: 0x0200076D RID: 1901
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-400)]
	public sealed class MallRuntimeRendererBudget : MonoBehaviour
	{
		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x06003145 RID: 12613 RVA: 0x00114FBE File Offset: 0x001131BE
		public int EligibleRenderers
		{
			get
			{
				return this.eligibleRenderers;
			}
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x06003146 RID: 12614 RVA: 0x00114FC6 File Offset: 0x001131C6
		public int CurrentlyCulledRenderers
		{
			get
			{
				return this.currentlyCulledRenderers;
			}
		}

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x06003147 RID: 12615 RVA: 0x00114FCE File Offset: 0x001131CE
		public int BudgetedNpcAnimators
		{
			get
			{
				return this.budgetedNpcAnimators;
			}
		}

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x06003148 RID: 12616 RVA: 0x00114FD6 File Offset: 0x001131D6
		public int CurrentlyCulledNpcRenderers
		{
			get
			{
				return this.currentlyCulledNpcRenderers;
			}
		}

		// Token: 0x06003149 RID: 12617 RVA: 0x00114FE0 File Offset: 0x001131E0
		private void OnEnable()
		{
			if (this.enableStaticDistanceCulling)
			{
				this.CacheRenderers();
			}
			else
			{
				this._renderers = Array.Empty<MeshRenderer>();
				this.eligibleRenderers = 0;
				this.currentlyCulledRenderers = 0;
			}
			this.CacheNpcRenderers();
			this._cursor = 0;
			this._nextNpcUpdateTime = 0f;
		}

		// Token: 0x0600314A RID: 12618 RVA: 0x00115030 File Offset: 0x00113230
		private void Update()
		{
			if (this._camera == null || !this._camera.isActiveAndEnabled)
			{
				this._camera = Camera.main;
			}
			if (this._camera == null)
			{
				return;
			}
			Vector3 position = this._camera.transform.position;
			if (this.enableStaticDistanceCulling && this._renderers.Length != 0)
			{
				int num = Mathf.Min(this.renderersPerFrame, this._renderers.Length);
				for (int i = 0; i < num; i++)
				{
					int cursor = this._cursor;
					this._cursor = cursor + 1;
					int num2 = cursor;
					if (this._cursor >= this._renderers.Length)
					{
						this._cursor = 0;
					}
					this.ApplyRendererBudget(num2, position);
				}
			}
			if (this.enableNpcDistanceCulling && Time.unscaledTime >= this._nextNpcUpdateTime)
			{
				this._nextNpcUpdateTime = Time.unscaledTime + this.npcUpdateInterval;
				this.ApplyNpcBudget(position);
			}
			if (this.enableNpcAnimationThrottling)
			{
				this.ApplyNpcAnimationBudget(position);
			}
		}

		// Token: 0x0600314B RID: 12619 RVA: 0x00115128 File Offset: 0x00113328
		private void CacheRenderers()
		{
			Transform transform = this.FindMallRoot();
			if (transform == null)
			{
				this._renderers = Array.Empty<MeshRenderer>();
				this.eligibleRenderers = 0;
				return;
			}
			MeshRenderer[] componentsInChildren = transform.GetComponentsInChildren<MeshRenderer>(true);
			HashSet<MeshRenderer> hashSet = MallRuntimeRendererBudget.BuildDynamicRendererSet(transform);
			List<Material> list = new List<Material>(8);
			List<MeshRenderer> list2 = new List<MeshRenderer>(componentsInChildren.Length);
			List<Vector3> list3 = new List<Vector3>(componentsInChildren.Length);
			List<float> list4 = new List<float>(componentsInChildren.Length);
			List<bool> list5 = new List<bool>(componentsInChildren.Length);
			List<ReflectionProbeUsage> list6 = new List<ReflectionProbeUsage>(componentsInChildren.Length);
			foreach (MeshRenderer meshRenderer in componentsInChildren)
			{
				if (MallRuntimeRendererBudget.IsSafeStaticCandidate(meshRenderer, hashSet, list))
				{
					Bounds bounds = meshRenderer.bounds;
					Vector3 size = bounds.size;
					float num = Mathf.Max(size.x, Mathf.Max(size.y, size.z));
					if (num > 0.001f && num <= this.neverCullLargerThan)
					{
						float num2 = Mathf.Clamp(num * this.distancePerMeterOfSize, this.minimumCullDistance, this.maximumCullDistance);
						list2.Add(meshRenderer);
						list3.Add(bounds.center);
						list4.Add(num2);
						list5.Add(meshRenderer.forceRenderingOff);
						list6.Add(meshRenderer.reflectionProbeUsage);
						if (this.simplifyReflectionProbeBlending && meshRenderer.reflectionProbeUsage == ReflectionProbeUsage.BlendProbes)
						{
							meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Simple;
						}
					}
				}
			}
			this._renderers = list2.ToArray();
			this._centres = list3.ToArray();
			this._cullDistances = list4.ToArray();
			this._authoredForceRenderingOff = list5.ToArray();
			this._authoredReflectionProbeUsage = list6.ToArray();
			this.eligibleRenderers = this._renderers.Length;
			this.currentlyCulledRenderers = 0;
		}

		// Token: 0x0600314C RID: 12620 RVA: 0x001152DC File Offset: 0x001134DC
		private void ApplyRendererBudget(int index, Vector3 cameraPosition)
		{
			MeshRenderer meshRenderer = this._renderers[index];
			if (meshRenderer == null)
			{
				return;
			}
			bool flag = this._authoredForceRenderingOff[index];
			object obj = meshRenderer.forceRenderingOff && !flag;
			float num = this._cullDistances[index];
			object obj2 = obj;
			float num2 = ((obj2 != null) ? Mathf.Max(this.minimumCullDistance, num - this.hysteresis) : num);
			bool flag2 = (this._centres[index] - cameraPosition).sqrMagnitude > num2 * num2;
			if (obj2 == flag2)
			{
				return;
			}
			meshRenderer.forceRenderingOff = flag || flag2;
			this.currentlyCulledRenderers += (flag2 ? 1 : (-1));
		}

		// Token: 0x0600314D RID: 12621 RVA: 0x00115380 File Offset: 0x00113580
		private Transform FindMallRoot()
		{
			GameObject[] rootGameObjects = base.gameObject.scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				Transform[] componentsInChildren = rootGameObjects[i].GetComponentsInChildren<Transform>(true);
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					if (componentsInChildren[j].name == "MonoChunk - 3x3 - Mall Full Interior")
					{
						return componentsInChildren[j];
					}
				}
			}
			return null;
		}

		// Token: 0x0600314E RID: 12622 RVA: 0x001153E8 File Offset: 0x001135E8
		private static HashSet<MeshRenderer> BuildDynamicRendererSet(Transform mallRoot)
		{
			HashSet<MeshRenderer> hashSet = new HashSet<MeshRenderer>();
			MallRuntimeRendererBudget.AddChildRenderers<Animator>(mallRoot.GetComponentsInChildren<Animator>(true), hashSet);
			MallRuntimeRendererBudget.AddChildRenderers<Rigidbody>(mallRoot.GetComponentsInChildren<Rigidbody>(true), hashSet);
			MallRuntimeRendererBudget.AddChildRenderers<Animation>(mallRoot.GetComponentsInChildren<Animation>(true), hashSet);
			MallRuntimeRendererBudget.AddChildRenderers<ArticulationBody>(mallRoot.GetComponentsInChildren<ArticulationBody>(true), hashSet);
			MallRuntimeRendererBudget.AddChildRenderers<Cloth>(mallRoot.GetComponentsInChildren<Cloth>(true), hashSet);
			MallRuntimeRendererBudget.AddChildRenderers<global::CharacterController>(mallRoot.GetComponentsInChildren<global::CharacterController>(true), hashSet);
			HashSet<string> hashSet2 = new HashSet<string>(StringComparer.Ordinal) { "MallEscalatorConveyor", "GymExerciseStation", "GymTreadmillBeltScroller", "HackMeStoreZone", "GachaClawRig", "GachaPlushField", "opencloseDoor", "opencloseDoor1", "PhotoBoothController", "PhotoBoothCurtainPhysics" };
			foreach (MonoBehaviour monoBehaviour in mallRoot.GetComponentsInChildren<MonoBehaviour>(true))
			{
				if (!(monoBehaviour == null) && hashSet2.Contains(monoBehaviour.GetType().Name))
				{
					MeshRenderer[] componentsInChildren2 = monoBehaviour.GetComponentsInChildren<MeshRenderer>(true);
					for (int j = 0; j < componentsInChildren2.Length; j++)
					{
						if (componentsInChildren2[j] != null)
						{
							hashSet.Add(componentsInChildren2[j]);
						}
					}
				}
			}
			foreach (Transform transform in mallRoot.GetComponentsInChildren<Transform>(true))
			{
				if (!(transform == null) && (!(transform.name != "Area_Electronic") || !(transform.name != "Area_Electronic (1)") || !(transform.name != "Area_ElectronicR")))
				{
					MeshRenderer[] componentsInChildren4 = transform.GetComponentsInChildren<MeshRenderer>(true);
					for (int l = 0; l < componentsInChildren4.Length; l++)
					{
						if (componentsInChildren4[l] != null)
						{
							hashSet.Add(componentsInChildren4[l]);
						}
					}
				}
			}
			return hashSet;
		}

		// Token: 0x0600314F RID: 12623 RVA: 0x001155E0 File Offset: 0x001137E0
		private static void AddChildRenderers<T>(T[] dynamicComponents, HashSet<MeshRenderer> result) where T : Component
		{
			foreach (T t in dynamicComponents)
			{
				if (!(t == null))
				{
					MeshRenderer[] componentsInChildren = t.GetComponentsInChildren<MeshRenderer>(true);
					for (int j = 0; j < componentsInChildren.Length; j++)
					{
						if (componentsInChildren[j] != null)
						{
							result.Add(componentsInChildren[j]);
						}
					}
				}
			}
		}

		// Token: 0x06003150 RID: 12624 RVA: 0x00115644 File Offset: 0x00113844
		private static bool IsSafeStaticCandidate(MeshRenderer renderer, HashSet<MeshRenderer> dynamicRenderers, List<Material> materialBuffer)
		{
			if (renderer == null || !renderer.gameObject.isStatic || renderer.forceRenderingOff)
			{
				return false;
			}
			if (dynamicRenderers.Contains(renderer))
			{
				return false;
			}
			materialBuffer.Clear();
			renderer.GetSharedMaterials(materialBuffer);
			for (int i = 0; i < materialBuffer.Count; i++)
			{
				if (materialBuffer[i] != null && materialBuffer[i].renderQueue >= 3000)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003151 RID: 12625 RVA: 0x001156C0 File Offset: 0x001138C0
		private void CacheNpcRenderers()
		{
			Animator[] array = Object.FindObjectsOfType<Animator>(true);
			List<Animator> list = new List<Animator>(array.Length);
			List<AnimatorCullingMode> list2 = new List<AnimatorCullingMode>(array.Length);
			List<bool> list3 = new List<bool>(array.Length);
			List<Renderer> list4 = new List<Renderer>(array.Length * 2);
			List<int> list5 = new List<int>(array.Length * 2);
			List<bool> list6 = new List<bool>(array.Length * 2);
			List<bool> list7 = new List<bool>(array.Length * 2);
			List<ReflectionProbeUsage> list8 = new List<ReflectionProbeUsage>(array.Length * 2);
			List<MotionVectorGenerationMode> list9 = new List<MotionVectorGenerationMode>(array.Length * 2);
			List<bool> list10 = new List<bool>(array.Length * 2);
			List<SkinQuality> list11 = new List<SkinQuality>(array.Length * 2);
			List<bool> list12 = new List<bool>(array.Length * 2);
			foreach (Animator animator in array)
			{
				if (!(animator == null) && !(animator.gameObject.scene != base.gameObject.scene) && MallRuntimeRendererBudget.IsNpcAnimator(animator))
				{
					int count = list.Count;
					list.Add(animator);
					list2.Add(animator.cullingMode);
					list3.Add(animator.enabled);
					animator.cullingMode = AnimatorCullingMode.CullCompletely;
					foreach (Renderer renderer in animator.GetComponentsInChildren<Renderer>(true))
					{
						if (renderer is MeshRenderer || renderer is SkinnedMeshRenderer)
						{
							list4.Add(renderer);
							list5.Add(count);
							list6.Add(!(renderer is SkinnedMeshRenderer));
							list7.Add(renderer.forceRenderingOff);
							list8.Add(renderer.reflectionProbeUsage);
							list9.Add(renderer.motionVectorGenerationMode);
							SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
							list10.Add(skinnedMeshRenderer != null && skinnedMeshRenderer.skinnedMotionVectors);
							list11.Add((skinnedMeshRenderer != null) ? skinnedMeshRenderer.quality : SkinQuality.Auto);
							list12.Add(skinnedMeshRenderer != null && skinnedMeshRenderer.updateWhenOffscreen);
							renderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
							if (skinnedMeshRenderer != null)
							{
								skinnedMeshRenderer.skinnedMotionVectors = false;
								skinnedMeshRenderer.updateWhenOffscreen = false;
								if (this.reduceNpcSkinningQuality && (skinnedMeshRenderer.quality == SkinQuality.Auto || skinnedMeshRenderer.quality == SkinQuality.Bone4))
								{
									skinnedMeshRenderer.quality = SkinQuality.Bone2;
								}
							}
							if (this.disableNpcReflectionProbes)
							{
								renderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
							}
							else if (renderer.reflectionProbeUsage == ReflectionProbeUsage.BlendProbes)
							{
								renderer.reflectionProbeUsage = ReflectionProbeUsage.Simple;
							}
						}
					}
				}
			}
			this._npcAnimators = list.ToArray();
			this._authoredNpcCullingModes = list2.ToArray();
			this._authoredNpcAnimatorEnabled = list3.ToArray();
			this._npcNextAnimationTick = new float[this._npcAnimators.Length];
			this._npcAnimationDisableFrame = new int[this._npcAnimators.Length];
			for (int k = 0; k < this._npcAnimators.Length; k++)
			{
				this._npcNextAnimationTick[k] = Time.unscaledTime + (float)(k % 7) * 0.011f;
			}
			this._npcRenderers = list4.ToArray();
			this._npcRendererOwners = list5.ToArray();
			this._npcRendererIsDetail = list6.ToArray();
			this._authoredNpcForceRenderingOff = list7.ToArray();
			this._authoredNpcProbeUsage = list8.ToArray();
			this._authoredNpcMotionVectors = list9.ToArray();
			this._authoredNpcSkinnedMotionVectors = list10.ToArray();
			this._authoredNpcSkinQuality = list11.ToArray();
			this._authoredNpcUpdateWhenOffscreen = list12.ToArray();
			this.budgetedNpcAnimators = this._npcAnimators.Length;
			this.currentlyCulledNpcRenderers = 0;
		}

		// Token: 0x06003152 RID: 12626 RVA: 0x00115A3C File Offset: 0x00113C3C
		private void ApplyNpcBudget(Vector3 cameraPosition)
		{
			float num = this.npcDetailCullDistance * this.npcDetailCullDistance;
			float num2 = this.npcFullCullDistance * this.npcFullCullDistance;
			this.currentlyCulledNpcRenderers = 0;
			for (int i = 0; i < this._npcRenderers.Length; i++)
			{
				Renderer renderer = this._npcRenderers[i];
				int num3 = this._npcRendererOwners[i];
				if (!(renderer == null) && num3 >= 0 && num3 < this._npcAnimators.Length && !(this._npcAnimators[num3] == null))
				{
					float sqrMagnitude = (this._npcAnimators[num3].transform.position - cameraPosition).sqrMagnitude;
					bool flag = sqrMagnitude > num2 || (this._npcRendererIsDetail[i] && sqrMagnitude > num);
					renderer.forceRenderingOff = this._authoredNpcForceRenderingOff[i] || flag;
					if (flag && !this._authoredNpcForceRenderingOff[i])
					{
						this.currentlyCulledNpcRenderers++;
					}
				}
			}
		}

		// Token: 0x06003153 RID: 12627 RVA: 0x00115B38 File Offset: 0x00113D38
		private void ApplyNpcAnimationBudget(Vector3 cameraPosition)
		{
			float num = this.npcFullRateDistance * this.npcFullRateDistance;
			float num2 = this.npcMidRateDistance * this.npcMidRateDistance;
			float unscaledTime = Time.unscaledTime;
			int frameCount = Time.frameCount;
			for (int i = 0; i < this._npcAnimators.Length; i++)
			{
				Animator animator = this._npcAnimators[i];
				if (!(animator == null) && i < this._authoredNpcAnimatorEnabled.Length && this._authoredNpcAnimatorEnabled[i])
				{
					float sqrMagnitude = (animator.transform.position - cameraPosition).sqrMagnitude;
					if (sqrMagnitude <= num)
					{
						if (!animator.enabled)
						{
							animator.enabled = true;
						}
					}
					else
					{
						float num3 = ((sqrMagnitude <= num2) ? Mathf.Max(5f, this.npcMidRateFps) : Mathf.Max(2f, this.npcFarRateFps));
						if (unscaledTime >= this._npcNextAnimationTick[i])
						{
							animator.enabled = true;
							this._npcAnimationDisableFrame[i] = frameCount + 1;
							this._npcNextAnimationTick[i] = unscaledTime + 1f / num3;
						}
						else if (frameCount >= this._npcAnimationDisableFrame[i] && animator.enabled)
						{
							animator.enabled = false;
						}
					}
				}
			}
		}

		// Token: 0x06003154 RID: 12628 RVA: 0x00115C74 File Offset: 0x00113E74
		private static bool IsNpcAnimator(Animator animator)
		{
			Transform transform = animator.transform;
			int num = 0;
			while (transform != null && num < 4)
			{
				if (transform.name.IndexOf("NPC", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return true;
				}
				num++;
				transform = transform.parent;
			}
			return false;
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x00115CBC File Offset: 0x00113EBC
		private void OnDisable()
		{
			int num = Mathf.Min(this._renderers.Length, this._authoredForceRenderingOff.Length);
			for (int i = 0; i < num; i++)
			{
				MeshRenderer meshRenderer = this._renderers[i];
				if (!(meshRenderer == null))
				{
					meshRenderer.forceRenderingOff = this._authoredForceRenderingOff[i];
					if (i < this._authoredReflectionProbeUsage.Length)
					{
						meshRenderer.reflectionProbeUsage = this._authoredReflectionProbeUsage[i];
					}
				}
			}
			this.currentlyCulledRenderers = 0;
			this.RestoreNpcRenderers();
		}

		// Token: 0x06003156 RID: 12630 RVA: 0x00115D34 File Offset: 0x00113F34
		private void RestoreNpcRenderers()
		{
			int num = Mathf.Min(this._npcAnimators.Length, this._authoredNpcCullingModes.Length);
			for (int i = 0; i < num; i++)
			{
				if (this._npcAnimators[i] != null)
				{
					this._npcAnimators[i].cullingMode = this._authoredNpcCullingModes[i];
					if (i < this._authoredNpcAnimatorEnabled.Length)
					{
						this._npcAnimators[i].enabled = this._authoredNpcAnimatorEnabled[i];
					}
				}
			}
			int num2 = Mathf.Min(this._npcRenderers.Length, this._authoredNpcForceRenderingOff.Length);
			for (int j = 0; j < num2; j++)
			{
				Renderer renderer = this._npcRenderers[j];
				if (!(renderer == null))
				{
					renderer.forceRenderingOff = this._authoredNpcForceRenderingOff[j];
					renderer.reflectionProbeUsage = this._authoredNpcProbeUsage[j];
					renderer.motionVectorGenerationMode = this._authoredNpcMotionVectors[j];
					SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
					if (skinnedMeshRenderer != null)
					{
						skinnedMeshRenderer.skinnedMotionVectors = this._authoredNpcSkinnedMotionVectors[j];
						if (j < this._authoredNpcSkinQuality.Length)
						{
							skinnedMeshRenderer.quality = this._authoredNpcSkinQuality[j];
						}
						if (j < this._authoredNpcUpdateWhenOffscreen.Length)
						{
							skinnedMeshRenderer.updateWhenOffscreen = this._authoredNpcUpdateWhenOffscreen[j];
						}
					}
				}
			}
			this.currentlyCulledNpcRenderers = 0;
		}

		// Token: 0x040031B3 RID: 12723
		private const string MallRootName = "MonoChunk - 3x3 - Mall Full Interior";

		// Token: 0x040031B4 RID: 12724
		[SerializeField]
		[Min(10f)]
		private float minimumCullDistance = 24f;

		// Token: 0x040031B5 RID: 12725
		[SerializeField]
		[Min(1f)]
		private float distancePerMeterOfSize = 36f;

		// Token: 0x040031B6 RID: 12726
		[SerializeField]
		[Min(20f)]
		private float maximumCullDistance = 78f;

		// Token: 0x040031B7 RID: 12727
		[SerializeField]
		[Min(0.5f)]
		private float neverCullLargerThan = 3f;

		// Token: 0x040031B8 RID: 12728
		[SerializeField]
		[Min(0f)]
		private float hysteresis = 5f;

		// Token: 0x040031B9 RID: 12729
		[SerializeField]
		[Min(128f)]
		private int renderersPerFrame = 2048;

		// Token: 0x040031BA RID: 12730
		[SerializeField]
		private bool simplifyReflectionProbeBlending = true;

		// Token: 0x040031BB RID: 12731
		[SerializeField]
		private bool enableStaticDistanceCulling;

		// Token: 0x040031BC RID: 12732
		[Header("NPC Render Budget")]
		[SerializeField]
		private bool enableNpcDistanceCulling;

		// Token: 0x040031BD RID: 12733
		[SerializeField]
		[Min(5f)]
		private float npcDetailCullDistance = 22f;

		// Token: 0x040031BE RID: 12734
		[SerializeField]
		[Min(10f)]
		private float npcFullCullDistance = 50f;

		// Token: 0x040031BF RID: 12735
		[SerializeField]
		[Min(0.05f)]
		private float npcUpdateInterval = 0.2f;

		// Token: 0x040031C0 RID: 12736
		[SerializeField]
		private bool reduceNpcSkinningQuality = true;

		// Token: 0x040031C1 RID: 12737
		[SerializeField]
		private bool disableNpcReflectionProbes = true;

		// Token: 0x040031C2 RID: 12738
		[SerializeField]
		private bool enableNpcAnimationThrottling = true;

		// Token: 0x040031C3 RID: 12739
		[SerializeField]
		[Min(1f)]
		private float npcFullRateDistance = 12f;

		// Token: 0x040031C4 RID: 12740
		[SerializeField]
		[Min(1f)]
		private float npcMidRateDistance = 30f;

		// Token: 0x040031C5 RID: 12741
		[SerializeField]
		[Range(5f, 30f)]
		private float npcMidRateFps = 15f;

		// Token: 0x040031C6 RID: 12742
		[SerializeField]
		[Range(2f, 15f)]
		private float npcFarRateFps = 6f;

		// Token: 0x040031C7 RID: 12743
		[SerializeField]
		[HideInInspector]
		private int eligibleRenderers;

		// Token: 0x040031C8 RID: 12744
		[SerializeField]
		[HideInInspector]
		private int currentlyCulledRenderers;

		// Token: 0x040031C9 RID: 12745
		[SerializeField]
		[HideInInspector]
		private int budgetedNpcAnimators;

		// Token: 0x040031CA RID: 12746
		[SerializeField]
		[HideInInspector]
		private int currentlyCulledNpcRenderers;

		// Token: 0x040031CB RID: 12747
		private MeshRenderer[] _renderers = Array.Empty<MeshRenderer>();

		// Token: 0x040031CC RID: 12748
		private Vector3[] _centres = Array.Empty<Vector3>();

		// Token: 0x040031CD RID: 12749
		private float[] _cullDistances = Array.Empty<float>();

		// Token: 0x040031CE RID: 12750
		private bool[] _authoredForceRenderingOff = Array.Empty<bool>();

		// Token: 0x040031CF RID: 12751
		private ReflectionProbeUsage[] _authoredReflectionProbeUsage = Array.Empty<ReflectionProbeUsage>();

		// Token: 0x040031D0 RID: 12752
		private Camera _camera;

		// Token: 0x040031D1 RID: 12753
		private int _cursor;

		// Token: 0x040031D2 RID: 12754
		private Animator[] _npcAnimators = Array.Empty<Animator>();

		// Token: 0x040031D3 RID: 12755
		private AnimatorCullingMode[] _authoredNpcCullingModes = Array.Empty<AnimatorCullingMode>();

		// Token: 0x040031D4 RID: 12756
		private bool[] _authoredNpcAnimatorEnabled = Array.Empty<bool>();

		// Token: 0x040031D5 RID: 12757
		private float[] _npcNextAnimationTick = Array.Empty<float>();

		// Token: 0x040031D6 RID: 12758
		private int[] _npcAnimationDisableFrame = Array.Empty<int>();

		// Token: 0x040031D7 RID: 12759
		private Renderer[] _npcRenderers = Array.Empty<Renderer>();

		// Token: 0x040031D8 RID: 12760
		private int[] _npcRendererOwners = Array.Empty<int>();

		// Token: 0x040031D9 RID: 12761
		private bool[] _npcRendererIsDetail = Array.Empty<bool>();

		// Token: 0x040031DA RID: 12762
		private bool[] _authoredNpcForceRenderingOff = Array.Empty<bool>();

		// Token: 0x040031DB RID: 12763
		private ReflectionProbeUsage[] _authoredNpcProbeUsage = Array.Empty<ReflectionProbeUsage>();

		// Token: 0x040031DC RID: 12764
		private MotionVectorGenerationMode[] _authoredNpcMotionVectors = Array.Empty<MotionVectorGenerationMode>();

		// Token: 0x040031DD RID: 12765
		private bool[] _authoredNpcSkinnedMotionVectors = Array.Empty<bool>();

		// Token: 0x040031DE RID: 12766
		private SkinQuality[] _authoredNpcSkinQuality = Array.Empty<SkinQuality>();

		// Token: 0x040031DF RID: 12767
		private bool[] _authoredNpcUpdateWhenOffscreen = Array.Empty<bool>();

		// Token: 0x040031E0 RID: 12768
		private float _nextNpcUpdateTime;
	}
}
