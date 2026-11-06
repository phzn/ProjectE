using System;
using BoZo.CharacterCreator;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x02001253 RID: 4691
	[Serializable]
	public class CoverBlendShapeEntry
	{
		// Token: 0x17001747 RID: 5959
		// (get) Token: 0x060091DC RID: 37340 RVA: 0x003BC678 File Offset: 0x003BA878
		public bool HasOverride
		{
			get
			{
				return this._hasOverride;
			}
		}

		// Token: 0x060091DD RID: 37341 RVA: 0x003BC680 File Offset: 0x003BA880
		public void RelinquishSharedOverride()
		{
			this._hasOverride = false;
			this.CurrentValue = 0f;
		}

		// Token: 0x060091DE RID: 37342 RVA: 0x003BC694 File Offset: 0x003BA894
		public float ResolveTargetValue(float boobsType)
		{
			if (this.TargetsByBoobsType == null || this.TargetsByBoobsType.Length != 6)
			{
				return this.TargetValue;
			}
			float num = Mathf.Clamp(boobsType, 0f, 5f);
			int num2 = Mathf.FloorToInt(num);
			return Mathf.Lerp(this.TargetsByBoobsType[num2], this.TargetsByBoobsType[Mathf.Min(num2 + 1, 5)], num - (float)num2);
		}

		// Token: 0x060091DF RID: 37343 RVA: 0x003BC6F4 File Offset: 0x003BA8F4
		public void CacheIndex()
		{
			if (this.TargetRenderer == null || this.TargetRenderer.sharedMesh == null || string.IsNullOrEmpty(this.BlendShapeName))
			{
				this.CachedIndex = -1;
				return;
			}
			this.CachedIndex = this.TargetRenderer.sharedMesh.GetBlendShapeIndex(this.BlendShapeName);
			this._manager = BlendShapeManager.ResolveForRig(this.TargetRenderer);
			if (this.CachedIndex >= 0 && !this._hasOverride)
			{
				this._fallbackBaseline = this.TargetRenderer.GetBlendShapeWeight(this.CachedIndex);
			}
		}

		// Token: 0x060091E0 RID: 37344 RVA: 0x003BC78C File Offset: 0x003BA98C
		private float GetExternalValue()
		{
			if (!(this._manager != null))
			{
				return this._fallbackBaseline;
			}
			return this._manager.GetEffectiveBlendShapeValue(this.BlendShapeName, "CoverPose", true);
		}

		// Token: 0x060091E1 RID: 37345 RVA: 0x003BC7BC File Offset: 0x003BA9BC
		public void Advance(float target, float step, float influence = 1f)
		{
			if (this.PreserveExternalValue)
			{
				float num = ((!this._hasOverride && this._manager != null) ? this._manager.GetEffectiveBlendShapeValue(this.BlendShapeName, null, true) : this.GetExternalValue());
				this.CurrentValue = Mathf.Max(this.CurrentValue, num);
				target = Mathf.Max(target, this.CurrentValue);
			}
			this.CurrentValue = Mathf.MoveTowards(this.CurrentValue, target, Mathf.Max(0f, step));
			this.Apply(influence);
		}

		// Token: 0x060091E2 RID: 37346 RVA: 0x003BC848 File Offset: 0x003BAA48
		public void Release(float step)
		{
			if (!this._hasOverride)
			{
				return;
			}
			float externalValue = this.GetExternalValue();
			this.CurrentValue = Mathf.MoveTowards(this._lastAppliedValue, externalValue, Mathf.Max(0f, step));
			if (this.PreserveExternalValue)
			{
				this.CurrentValue = Mathf.Max(this.CurrentValue, externalValue);
			}
			if (Mathf.Approximately(this.CurrentValue, externalValue))
			{
				this.RemoveOverride();
				return;
			}
			this.Apply(1f);
		}

		// Token: 0x060091E3 RID: 37347 RVA: 0x003BC8BC File Offset: 0x003BAABC
		public void Apply(float influence = 1f)
		{
			if (this.CachedIndex < 0)
			{
				return;
			}
			float num = Mathf.Lerp(this.GetExternalValue(), this.CurrentValue, Mathf.Clamp01(influence));
			this._lastAppliedValue = num;
			this._hasOverride = true;
			if (this._manager != null)
			{
				this._manager.SetOverride(this.BlendShapeName, num, "CoverPose", BlendShapePriority.Pose, false);
				return;
			}
			if (this.TargetRenderer != null)
			{
				this.TargetRenderer.SetBlendShapeWeight(this.CachedIndex, num);
			}
		}

		// Token: 0x060091E4 RID: 37348 RVA: 0x003BC944 File Offset: 0x003BAB44
		public void RemoveOverride()
		{
			if (this._manager != null && !string.IsNullOrEmpty(this.BlendShapeName))
			{
				this._manager.RemoveOverride(this.BlendShapeName, "CoverPose", true, true, 0f);
			}
			else if (this.TargetRenderer != null && this.CachedIndex >= 0)
			{
				this.TargetRenderer.SetBlendShapeWeight(this.CachedIndex, this._fallbackBaseline);
			}
			this._hasOverride = false;
			this.CurrentValue = 0f;
		}

		// Token: 0x060091E5 RID: 37349 RVA: 0x003BC9CC File Offset: 0x003BABCC
		public void RestoreUnanimatedValue()
		{
			if (this.TargetRenderer == null || this.TargetRenderer.sharedMesh == null || this.CachedIndex < 0 || this.CachedIndex >= this.TargetRenderer.sharedMesh.blendShapeCount)
			{
				return;
			}
			if (this._manager == null)
			{
				this._manager = BlendShapeManager.ResolveForRig(this.TargetRenderer);
			}
			float num = ((this._manager != null) ? this._manager.GetEffectiveBlendShapeValue(this.BlendShapeName, "CoverPose", false) : this._fallbackBaseline);
			this.TargetRenderer.SetBlendShapeWeight(this.CachedIndex, num);
		}

		// Token: 0x04009632 RID: 38450
		[Tooltip("The SkinnedMeshRenderer containing the blend shape")]
		public SkinnedMeshRenderer TargetRenderer;

		// Token: 0x04009633 RID: 38451
		[Tooltip("Name of the blend shape to modify")]
		public string BlendShapeName;

		// Token: 0x04009634 RID: 38452
		[Tooltip("Target value when this cover pose is active (0-100)")]
		[Range(0f, 100f)]
		public float TargetValue = 100f;

		// Token: 0x04009635 RID: 38453
		[Tooltip("Optional six targets ordered by BoobsType: Flat, Smaller, Small, Default, Natural, Balloon. Empty uses TargetValue.")]
		public float[] TargetsByBoobsType = Array.Empty<float>();

		// Token: 0x04009636 RID: 38454
		[Tooltip("Optional activation speed for this shape. Zero uses the controller speed (for slow stimulation shapes).")]
		[Min(0f)]
		public float BlendSpeedOverride;

		// Token: 0x04009637 RID: 38455
		[Tooltip("Start from the effective existing response and never lower it while this pose is active.")]
		public bool PreserveExternalValue;

		// Token: 0x04009638 RID: 38456
		[Tooltip("Release this deformation when a vehicle's only covering hand leaves the chest for lower cover.")]
		public bool RequiresChestContact;

		// Token: 0x04009639 RID: 38457
		[NonSerialized]
		private BlendShapeManager _manager;

		// Token: 0x0400963A RID: 38458
		[NonSerialized]
		private bool _hasOverride;

		// Token: 0x0400963B RID: 38459
		[NonSerialized]
		private float _fallbackBaseline;

		// Token: 0x0400963C RID: 38460
		[NonSerialized]
		private float _lastAppliedValue;

		// Token: 0x0400963D RID: 38461
		private const string SourceId = "CoverPose";

		// Token: 0x0400963E RID: 38462
		[NonSerialized]
		public int CachedIndex = -1;

		// Token: 0x0400963F RID: 38463
		[NonSerialized]
		public float CurrentValue;
	}
}
