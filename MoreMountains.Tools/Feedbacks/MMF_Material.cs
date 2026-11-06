using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001F6 RID: 502
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the material of the target renderer everytime it's played.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Renderer/Material")]
	public class MMF_Material : MMF_Feedback
	{
		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000D1A RID: 3354 RVA: 0x00038C94 File Offset: 0x00036E94
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x00038C97 File Offset: 0x00036E97
		protected override void AutomateTargetAcquisition()
		{
			this.TargetRenderer = this.FindAutomatedTarget<Renderer>();
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000D1C RID: 3356 RVA: 0x00038CA5 File Offset: 0x00036EA5
		// (set) Token: 0x06000D1D RID: 3357 RVA: 0x00038CBB File Offset: 0x00036EBB
		public override float FeedbackDuration
		{
			get
			{
				if (!this.InterpolateTransition)
				{
					return 0f;
				}
				return this.TransitionDuration;
			}
			set
			{
				if (this.InterpolateTransition)
				{
					this.TransitionDuration = value;
				}
			}
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x00038CCC File Offset: 0x00036ECC
		public virtual float GetTime()
		{
			if (this.ComputedTimescaleMode != TimescaleModes.Scaled)
			{
				return Time.unscaledTime;
			}
			return Time.time;
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x00038CE1 File Offset: 0x00036EE1
		public virtual float GetDeltaTime()
		{
			if (this.ComputedTimescaleMode != TimescaleModes.Scaled)
			{
				return Time.unscaledDeltaTime;
			}
			return Time.deltaTime;
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x00038CF6 File Offset: 0x00036EF6
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			this.InitializeMaterials();
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x00038D08 File Offset: 0x00036F08
		protected virtual void InitializeMaterials()
		{
			if (this.TargetRenderer == null)
			{
				return;
			}
			this._currentIndex = this.InitialIndex;
			this._tempMaterials = new Material[this.TargetRenderer.materials.Length];
			this._initialMaterials = new Material[this.TargetRenderer.materials.Length];
			for (int i = 0; i < this._initialMaterials.Length; i++)
			{
				this._initialMaterials[i] = new Material(this.TargetRenderer.materials[i]);
			}
			if (this.RendererMaterialIndexes == null)
			{
				this.RendererMaterialIndexes = new int[1];
			}
			if (this.RendererMaterialIndexes.Length == 0)
			{
				this.RendererMaterialIndexes = new int[1];
				this.RendererMaterialIndexes[0] = 0;
			}
			this._coroutines = new Coroutine[this.RendererMaterialIndexes.Length];
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x00038DD4 File Offset: 0x00036FD4
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Material.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.Materials.Count == 0)
			{
				Debug.LogError("[MMFeedbackMaterial on " + this.Owner.name + "] The Materials array is empty.");
				return;
			}
			int num = this.DetermineNextIndex();
			if (this.Materials[num] == null)
			{
				Debug.LogError("[MMFeedbackMaterial on " + this.Owner.name + "] Attempting to switch to a null material.");
				return;
			}
			if (this.InterpolateTransition)
			{
				for (int i = 0; i < this.RendererMaterialIndexes.Length; i++)
				{
					if (this._coroutines[i] != null)
					{
						this.Owner.StopCoroutine(this._coroutines[i]);
					}
					this._coroutines[i] = this.Owner.StartCoroutine(this.TransitionMaterial(this.TargetRenderer.materials[this.RendererMaterialIndexes[i]], this.Materials[num], this.RendererMaterialIndexes[i]));
				}
				return;
			}
			this.ApplyMaterial(this.Materials[num]);
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x00038EE8 File Offset: 0x000370E8
		protected virtual void ApplyMaterial(Material material)
		{
			this._tempMaterials = this.TargetRenderer.materials;
			for (int i = 0; i < this.RendererMaterialIndexes.Length; i++)
			{
				this._tempMaterials[this.RendererMaterialIndexes[i]] = material;
			}
			this.TargetRenderer.materials = this._tempMaterials;
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x00038F3C File Offset: 0x0003713C
		protected virtual void LerpMaterial(Material fromMaterial, Material toMaterial, float t, int materialIndex)
		{
			this._tempMaterials = this.TargetRenderer.materials;
			for (int i = 0; i < this.RendererMaterialIndexes.Length; i++)
			{
				this._tempMaterials[materialIndex].Lerp(fromMaterial, toMaterial, t);
			}
			this.TargetRenderer.materials = this._tempMaterials;
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x00038F8F File Offset: 0x0003718F
		protected virtual IEnumerator TransitionMaterial(Material originalMaterial, Material newMaterial, int materialIndex)
		{
			this.IsPlaying = true;
			this._startedAt = this.GetTime();
			while (this.GetTime() - this._startedAt < this.TransitionDuration)
			{
				float num = MMFeedbacksHelpers.Remap(this.GetTime() - this._startedAt, 0f, this.TransitionDuration, 0f, 1f);
				float num2 = this.TransitionCurve.Evaluate(num);
				this.LerpMaterial(originalMaterial, newMaterial, num2, materialIndex);
				yield return null;
			}
			float num3 = this.TransitionCurve.Evaluate(1f);
			this.LerpMaterial(originalMaterial, newMaterial, num3, materialIndex);
			this.IsPlaying = false;
			yield break;
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x00038FB4 File Offset: 0x000371B4
		protected virtual int DetermineNextIndex()
		{
			MMF_Material.Methods method = this.Method;
			if (method == MMF_Material.Methods.Sequential)
			{
				this._currentIndex++;
				if (this._currentIndex >= this.Materials.Count)
				{
					this._currentIndex = (this.Loop ? 0 : (this._currentIndex - 1));
				}
				return this._currentIndex;
			}
			if (method == MMF_Material.Methods.Random)
			{
				int num = Random.Range(0, this.Materials.Count);
				if (this.AlwaysNewMaterial)
				{
					while (this._currentIndex == num)
					{
						num = Random.Range(0, this.Materials.Count);
					}
				}
				this._currentIndex = num;
				return this._currentIndex;
			}
			return 0;
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x0003905C File Offset: 0x0003725C
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			base.CustomStopFeedback(position, feedbacksIntensity);
			if (this.Active && MMF_Material.FeedbackTypeAuthorized && this._coroutines != null)
			{
				this.IsPlaying = false;
				for (int i = 0; i < this.RendererMaterialIndexes.Length; i++)
				{
					if (this._coroutines[i] != null)
					{
						this.Owner.StopCoroutine(this._coroutines[i]);
					}
					this._coroutines[i] = null;
				}
			}
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x000390C8 File Offset: 0x000372C8
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Material.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetRenderer.materials = this._initialMaterials;
			this.InitializeMaterials();
		}

		// Token: 0x04000EFB RID: 3835
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000EFC RID: 3836
		[MMFInspectorGroup("Target Material", true, 61, true, false)]
		[Tooltip("the renderer to change material on")]
		public Renderer TargetRenderer;

		// Token: 0x04000EFD RID: 3837
		[FormerlySerializedAs("MaterialIndexes")]
		[Tooltip("the list of material indexes we want to change on the target renderer. If left empty, will only target the material at index 0")]
		public int[] RendererMaterialIndexes;

		// Token: 0x04000EFE RID: 3838
		[MMFInspectorGroup("Material Change", true, 33, false, false)]
		[Tooltip("the selected method")]
		public MMF_Material.Methods Method;

		// Token: 0x04000EFF RID: 3839
		[MMFEnumCondition("Method", new int[] { 0 })]
		[Tooltip("whether or not the sequential order should loop")]
		public bool Loop = true;

		// Token: 0x04000F00 RID: 3840
		[MMFEnumCondition("Method", new int[] { 1 })]
		[Tooltip("whether or not to always pick a new material in random mode")]
		public bool AlwaysNewMaterial = true;

		// Token: 0x04000F01 RID: 3841
		[Tooltip("the initial index to start with")]
		public int InitialIndex;

		// Token: 0x04000F02 RID: 3842
		[Tooltip("the list of materials to pick from")]
		public List<Material> Materials;

		// Token: 0x04000F03 RID: 3843
		[MMFInspectorGroup("Interpolation", true, 35, false, false)]
		public bool InterpolateTransition;

		// Token: 0x04000F04 RID: 3844
		public float TransitionDuration = 1f;

		// Token: 0x04000F05 RID: 3845
		public AnimationCurve TransitionCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04000F06 RID: 3846
		protected int _currentIndex;

		// Token: 0x04000F07 RID: 3847
		protected float _startedAt;

		// Token: 0x04000F08 RID: 3848
		protected Coroutine[] _coroutines;

		// Token: 0x04000F09 RID: 3849
		protected Material[] _tempMaterials;

		// Token: 0x04000F0A RID: 3850
		protected Material[] _initialMaterials;

		// Token: 0x020004C8 RID: 1224
		public enum Methods
		{
			// Token: 0x0400223F RID: 8767
			Sequential,
			// Token: 0x04002240 RID: 8768
			Random
		}
	}
}
