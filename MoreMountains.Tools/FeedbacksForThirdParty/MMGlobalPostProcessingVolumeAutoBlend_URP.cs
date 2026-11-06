using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Rendering;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000354 RID: 852
	[RequireComponent(typeof(Volume))]
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMGlobalPostProcessingVolumeAutoBlend_URP")]
	public class MMGlobalPostProcessingVolumeAutoBlend_URP : MonoBehaviour
	{
		// Token: 0x0600176A RID: 5994 RVA: 0x000632B3 File Offset: 0x000614B3
		protected float GetTime()
		{
			if (this.TimeScale != MMGlobalPostProcessingVolumeAutoBlend_URP.TimeScales.Unscaled)
			{
				return Time.time;
			}
			return Time.unscaledTime;
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x000632C9 File Offset: 0x000614C9
		protected virtual void Awake()
		{
			this._volume = base.gameObject.GetComponent<Volume>();
			this._volume.weight = this.InitialWeight;
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x000632ED File Offset: 0x000614ED
		protected virtual void OnEnable()
		{
			if (this.BlendTriggerMode == MMGlobalPostProcessingVolumeAutoBlend_URP.BlendTriggerModes.OnEnable && !this._blending)
			{
				this.Blend();
			}
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x00063308 File Offset: 0x00061508
		public virtual void Blend()
		{
			if (this._blending && !this.Interruptable)
			{
				return;
			}
			this._initial = (this.StartFromCurrentValue ? this._volume.weight : this.InitialWeight);
			this._destination = this.FinalWeight;
			this.StartBlending();
		}

		// Token: 0x0600176E RID: 5998 RVA: 0x0006335C File Offset: 0x0006155C
		public virtual void BlendBack()
		{
			if (this._blending && !this.Interruptable)
			{
				return;
			}
			this._initial = (this.StartFromCurrentValue ? this._volume.weight : this.FinalWeight);
			this._destination = this.InitialWeight;
			this.StartBlending();
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x000633AD File Offset: 0x000615AD
		protected virtual void StartBlending()
		{
			this._startTime = this.GetTime();
			this._blending = true;
			base.enabled = true;
			if (this.DisableVolumeOnZeroWeight)
			{
				this._volume.enabled = true;
			}
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x000633DD File Offset: 0x000615DD
		public virtual void StopBlending()
		{
			this._blending = false;
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x000633E8 File Offset: 0x000615E8
		protected virtual void Update()
		{
			if (!this._blending)
			{
				return;
			}
			float num = this.GetTime() - this._startTime;
			if (num < this.BlendDuration)
			{
				float num2 = MMFeedbacksHelpers.Remap(num, 0f, this.BlendDuration, 0f, 1f);
				this._volume.weight = Mathf.LerpUnclamped(this._initial, this._destination, this.Curve.Evaluate(num2));
				return;
			}
			this._volume.weight = this._destination;
			this._blending = false;
			if (this.DisableVolumeOnZeroWeight && this._volume.weight == 0f)
			{
				this._volume.enabled = false;
			}
			if (this.DisableSelfAfterEnd)
			{
				base.enabled = false;
			}
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x000634A7 File Offset: 0x000616A7
		public virtual void RestoreInitialValues()
		{
			this._volume.weight = this._initial;
		}

		// Token: 0x04001AEC RID: 6892
		[Header("Blend")]
		public MMGlobalPostProcessingVolumeAutoBlend_URP.BlendTriggerModes BlendTriggerMode;

		// Token: 0x04001AED RID: 6893
		public float BlendDuration = 1f;

		// Token: 0x04001AEE RID: 6894
		public AnimationCurve Curve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04001AEF RID: 6895
		[Header("Weight")]
		[Range(0f, 1f)]
		public float InitialWeight;

		// Token: 0x04001AF0 RID: 6896
		[Range(0f, 1f)]
		public float FinalWeight = 1f;

		// Token: 0x04001AF1 RID: 6897
		[Header("Behaviour")]
		public MMGlobalPostProcessingVolumeAutoBlend_URP.TimeScales TimeScale = MMGlobalPostProcessingVolumeAutoBlend_URP.TimeScales.Unscaled;

		// Token: 0x04001AF2 RID: 6898
		public bool DisableVolumeOnZeroWeight = true;

		// Token: 0x04001AF3 RID: 6899
		public bool DisableSelfAfterEnd = true;

		// Token: 0x04001AF4 RID: 6900
		public bool Interruptable = true;

		// Token: 0x04001AF5 RID: 6901
		public bool StartFromCurrentValue = true;

		// Token: 0x04001AF6 RID: 6902
		[Header("Tests")]
		[MMFInspectorButton("Blend")]
		public bool TestBlend;

		// Token: 0x04001AF7 RID: 6903
		[MMFInspectorButton("BlendBack")]
		public bool TestBlendBackwards;

		// Token: 0x04001AF8 RID: 6904
		protected float _initial;

		// Token: 0x04001AF9 RID: 6905
		protected float _destination;

		// Token: 0x04001AFA RID: 6906
		protected float _startTime;

		// Token: 0x04001AFB RID: 6907
		protected bool _blending;

		// Token: 0x04001AFC RID: 6908
		protected Volume _volume;

		// Token: 0x02000586 RID: 1414
		public enum TimeScales
		{
			// Token: 0x040024E4 RID: 9444
			Scaled,
			// Token: 0x040024E5 RID: 9445
			Unscaled
		}

		// Token: 0x02000587 RID: 1415
		public enum BlendTriggerModes
		{
			// Token: 0x040024E7 RID: 9447
			OnEnable,
			// Token: 0x040024E8 RID: 9448
			Script
		}
	}
}
