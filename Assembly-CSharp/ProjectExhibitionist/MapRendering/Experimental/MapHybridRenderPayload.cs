using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProjectExhibitionist.MapRendering.Experimental
{
	// Token: 0x020009BB RID: 2491
	[DisallowMultipleComponent]
	public sealed class MapHybridRenderPayload : MonoBehaviour
	{
		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x06004602 RID: 17922 RVA: 0x001A7D45 File Offset: 0x001A5F45
		// (set) Token: 0x06004603 RID: 17923 RVA: 0x001A7D4C File Offset: 0x001A5F4C
		public static bool HasFailure { get; internal set; }

		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x06004604 RID: 17924 RVA: 0x001A7D54 File Offset: 0x001A5F54
		// (set) Token: 0x06004605 RID: 17925 RVA: 0x001A7D5B File Offset: 0x001A5F5B
		public static string LastError { get; internal set; }

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x06004606 RID: 17926 RVA: 0x001A7D63 File Offset: 0x001A5F63
		// (set) Token: 0x06004607 RID: 17927 RVA: 0x001A7D6A File Offset: 0x001A5F6A
		public static int FailureCount { get; internal set; }

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x06004608 RID: 17928 RVA: 0x001A7D72 File Offset: 0x001A5F72
		// (set) Token: 0x06004609 RID: 17929 RVA: 0x001A7D79 File Offset: 0x001A5F79
		public static int RegisteredGroups { get; internal set; }

		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x0600460A RID: 17930 RVA: 0x001A7D81 File Offset: 0x001A5F81
		// (set) Token: 0x0600460B RID: 17931 RVA: 0x001A7D88 File Offset: 0x001A5F88
		public static int ActivePayloads { get; internal set; }

		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x0600460C RID: 17932 RVA: 0x001A7D90 File Offset: 0x001A5F90
		// (set) Token: 0x0600460D RID: 17933 RVA: 0x001A7D97 File Offset: 0x001A5F97
		public static int ActiveInstances { get; internal set; }

		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x0600460E RID: 17934 RVA: 0x001A7D9F File Offset: 0x001A5F9F
		// (set) Token: 0x0600460F RID: 17935 RVA: 0x001A7DA6 File Offset: 0x001A5FA6
		public static int PendingVisiblePayloads { get; internal set; }

		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x06004610 RID: 17936 RVA: 0x001A7DAE File Offset: 0x001A5FAE
		// (set) Token: 0x06004611 RID: 17937 RVA: 0x001A7DB5 File Offset: 0x001A5FB5
		public static int ReflectionProbeEntries { get; internal set; }

		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x06004612 RID: 17938 RVA: 0x001A7DBD File Offset: 0x001A5FBD
		// (set) Token: 0x06004613 RID: 17939 RVA: 0x001A7DC4 File Offset: 0x001A5FC4
		public static int UploadCount { get; internal set; }

		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x06004614 RID: 17940 RVA: 0x001A7DCC File Offset: 0x001A5FCC
		// (set) Token: 0x06004615 RID: 17941 RVA: 0x001A7DD3 File Offset: 0x001A5FD3
		public static int VisibilityChanges { get; internal set; }

		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x06004616 RID: 17942 RVA: 0x001A7DDB File Offset: 0x001A5FDB
		// (set) Token: 0x06004617 RID: 17943 RVA: 0x001A7DE2 File Offset: 0x001A5FE2
		public static int OriginShiftCount { get; internal set; }

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x06004618 RID: 17944 RVA: 0x001A7DEA File Offset: 0x001A5FEA
		// (set) Token: 0x06004619 RID: 17945 RVA: 0x001A7DF1 File Offset: 0x001A5FF1
		public static int ExpectedSourceInstances { get; internal set; }

		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x0600461A RID: 17946 RVA: 0x001A7DF9 File Offset: 0x001A5FF9
		// (set) Token: 0x0600461B RID: 17947 RVA: 0x001A7E00 File Offset: 0x001A6000
		public static int RegisteredSourceInstances { get; internal set; }

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x0600461C RID: 17948 RVA: 0x001A7E08 File Offset: 0x001A6008
		// (set) Token: 0x0600461D RID: 17949 RVA: 0x001A7E0F File Offset: 0x001A600F
		public static int DuplicateNativeRendererCount { get; internal set; }

		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x0600461E RID: 17950 RVA: 0x001A7E17 File Offset: 0x001A6017
		public static string FailureReason
		{
			get
			{
				return MapHybridRenderPayload.LastError;
			}
		}

		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x0600461F RID: 17951 RVA: 0x001A7E1E File Offset: 0x001A601E
		public static int ActivePayloadCount
		{
			get
			{
				return MapHybridRenderPayload.ActivePayloads;
			}
		}

		// Token: 0x17000BE9 RID: 3049
		// (get) Token: 0x06004620 RID: 17952 RVA: 0x001A7E25 File Offset: 0x001A6025
		public static int ActiveInstanceCount
		{
			get
			{
				return MapHybridRenderPayload.ActiveInstances;
			}
		}

		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x06004621 RID: 17953 RVA: 0x001A7E2C File Offset: 0x001A602C
		public static int RegisteredPrototypeCount
		{
			get
			{
				return MapHybridRenderPayload.RegisteredGroups;
			}
		}

		// Token: 0x17000BEB RID: 3051
		// (get) Token: 0x06004622 RID: 17954 RVA: 0x001A7E25 File Offset: 0x001A6025
		public static int LastUploadInstanceCount
		{
			get
			{
				return MapHybridRenderPayload.ActiveInstances;
			}
		}

		// Token: 0x17000BEC RID: 3052
		// (get) Token: 0x06004623 RID: 17955 RVA: 0x001A7E33 File Offset: 0x001A6033
		public static int UncoveredSourceInstances
		{
			get
			{
				return Math.Max(0, MapHybridRenderPayload.ExpectedSourceInstances - MapHybridRenderPayload.RegisteredSourceInstances);
			}
		}

		// Token: 0x06004624 RID: 17956 RVA: 0x001A7E48 File Offset: 0x001A6048
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetDiagnostics()
		{
			MapHybridRenderPayload.HasFailure = false;
			MapHybridRenderPayload.LastError = null;
			MapHybridRenderPayload.FailureCount = (MapHybridRenderPayload.RegisteredGroups = (MapHybridRenderPayload.ActivePayloads = (MapHybridRenderPayload.ActiveInstances = 0)));
			MapHybridRenderPayload.PendingVisiblePayloads = (MapHybridRenderPayload.ReflectionProbeEntries = (MapHybridRenderPayload.UploadCount = (MapHybridRenderPayload.VisibilityChanges = (MapHybridRenderPayload.OriginShiftCount = 0))));
			MapHybridRenderPayload.ExpectedSourceInstances = (MapHybridRenderPayload.RegisteredSourceInstances = (MapHybridRenderPayload.DuplicateNativeRendererCount = 0));
			MapHybridRenderService.ResetStatic();
		}

		// Token: 0x06004625 RID: 17957 RVA: 0x001A7EAE File Offset: 0x001A60AE
		private void OnEnable()
		{
			if (Application.isPlaying)
			{
				MapHybridRenderService.Get().Add(this);
			}
		}

		// Token: 0x06004626 RID: 17958 RVA: 0x001A7EC2 File Offset: 0x001A60C2
		private void OnDisable()
		{
			MapHybridRenderService.SetInactive(this);
		}

		// Token: 0x06004627 RID: 17959 RVA: 0x001A7ECA File Offset: 0x001A60CA
		private void OnDestroy()
		{
			MapHybridRenderService.Remove(this);
		}

		// Token: 0x040045A0 RID: 17824
		public MapHybridRenderPayload.Entry[] entries;

		// Token: 0x040045A1 RID: 17825
		public Renderer[] visibilitySentinels;

		// Token: 0x040045A2 RID: 17826
		public GameObject nativeFallbackRoot;

		// Token: 0x040045A3 RID: 17827
		public GameObject nativeResidualRoot;

		// Token: 0x020009BC RID: 2492
		[Serializable]
		public sealed class Entry
		{
			// Token: 0x040045B2 RID: 17842
			public Mesh mesh;

			// Token: 0x040045B3 RID: 17843
			public Material[] materials;

			// Token: 0x040045B4 RID: 17844
			public Matrix4x4 localMatrix = Matrix4x4.identity;

			// Token: 0x040045B5 RID: 17845
			public int layer;

			// Token: 0x040045B6 RID: 17846
			public ShadowCastingMode shadows = ShadowCastingMode.On;

			// Token: 0x040045B7 RID: 17847
			public bool receiveShadows = true;

			// Token: 0x040045B8 RID: 17848
			public uint renderingLayerMask = 1U;

			// Token: 0x040045B9 RID: 17849
			public LightProbeUsage lightProbeUsage;

			// Token: 0x040045BA RID: 17850
			public ReflectionProbeUsage reflectionProbeUsage;

			// Token: 0x040045BB RID: 17851
			public Renderer sentinel;
		}
	}
}
