using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020000A1 RID: 161
	[ExecuteAlways]
	[AddComponentMenu("Rendering/Lens Flare (SRP)")]
	public sealed class LensFlareComponentSRP : MonoBehaviour
	{
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x0001B6C8 File Offset: 0x000198C8
		// (set) Token: 0x06000534 RID: 1332 RVA: 0x0001B6D0 File Offset: 0x000198D0
		public LensFlareDataSRP lensFlareData
		{
			get
			{
				return this.m_LensFlareData;
			}
			set
			{
				this.m_LensFlareData = value;
				this.OnValidate();
			}
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0001B6E0 File Offset: 0x000198E0
		public float celestialProjectedOcclusionRadius(Camera mainCam)
		{
			float num = (float)Math.Tan((double)LensFlareComponentSRP.sCelestialAngularRadius) * mainCam.farClipPlane;
			return this.occlusionRadius * num;
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0001B709 File Offset: 0x00019909
		private void OnEnable()
		{
			if (this.lensFlareData)
			{
				LensFlareCommonSRP.Instance.AddData(this);
				return;
			}
			LensFlareCommonSRP.Instance.RemoveData(this);
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0001B72F File Offset: 0x0001992F
		private void OnDisable()
		{
			LensFlareCommonSRP.Instance.RemoveData(this);
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0001B73C File Offset: 0x0001993C
		private void OnValidate()
		{
			if (base.isActiveAndEnabled && this.lensFlareData != null)
			{
				LensFlareCommonSRP.Instance.AddData(this);
				return;
			}
			LensFlareCommonSRP.Instance.RemoveData(this);
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0001B76C File Offset: 0x0001996C
		public LensFlareComponentSRP()
		{
			AnimationCurve animationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
			float num = 1f;
			bool flag = false;
			Vector2 vector = new Vector2(0f, 1f);
			this.occlusionRemapCurve = new TextureCurve(animationCurve, num, flag, in vector);
			base..ctor();
		}

		// Token: 0x04000379 RID: 889
		[SerializeField]
		private LensFlareDataSRP m_LensFlareData;

		// Token: 0x0400037A RID: 890
		[Min(0f)]
		public float intensity = 1f;

		// Token: 0x0400037B RID: 891
		[Min(1E-05f)]
		public float maxAttenuationDistance = 100f;

		// Token: 0x0400037C RID: 892
		[Min(1E-05f)]
		public float maxAttenuationScale = 100f;

		// Token: 0x0400037D RID: 893
		public AnimationCurve distanceAttenuationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x0400037E RID: 894
		public AnimationCurve scaleByDistanceCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x0400037F RID: 895
		public bool attenuationByLightShape = true;

		// Token: 0x04000380 RID: 896
		public AnimationCurve radialScreenAttenuationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 1f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04000381 RID: 897
		public bool useOcclusion;

		// Token: 0x04000382 RID: 898
		[Min(0f)]
		public float occlusionRadius = 0.1f;

		// Token: 0x04000383 RID: 899
		public bool useBackgroundCloudOcclusion;

		// Token: 0x04000384 RID: 900
		[Range(1f, 64f)]
		public uint sampleCount = 32U;

		// Token: 0x04000385 RID: 901
		public float occlusionOffset = 0.05f;

		// Token: 0x04000386 RID: 902
		[Min(0f)]
		public float scale = 1f;

		// Token: 0x04000387 RID: 903
		public bool allowOffScreen;

		// Token: 0x04000388 RID: 904
		public bool volumetricCloudOcclusion;

		// Token: 0x04000389 RID: 905
		private static float sCelestialAngularRadius = 0.057595868f;

		// Token: 0x0400038A RID: 906
		public TextureCurve occlusionRemapCurve;
	}
}
