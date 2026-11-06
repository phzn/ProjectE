using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000067 RID: 103
	[ExecuteAlways]
	[AddComponentMenu("Rendering/URP Decal Projector")]
	public class DecalProjector : MonoBehaviour
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060003B7 RID: 951 RVA: 0x0001611C File Offset: 0x0001431C
		// (remove) Token: 0x060003B8 RID: 952 RVA: 0x00016150 File Offset: 0x00014350
		internal static event DecalProjector.DecalProjectorAction onDecalAdd;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060003B9 RID: 953 RVA: 0x00016184 File Offset: 0x00014384
		// (remove) Token: 0x060003BA RID: 954 RVA: 0x000161B8 File Offset: 0x000143B8
		internal static event DecalProjector.DecalProjectorAction onDecalRemove;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060003BB RID: 955 RVA: 0x000161EC File Offset: 0x000143EC
		// (remove) Token: 0x060003BC RID: 956 RVA: 0x00016220 File Offset: 0x00014420
		internal static event DecalProjector.DecalProjectorAction onDecalPropertyChange;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060003BD RID: 957 RVA: 0x00016254 File Offset: 0x00014454
		// (remove) Token: 0x060003BE RID: 958 RVA: 0x00016288 File Offset: 0x00014488
		internal static event Action onAllDecalPropertyChange;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060003BF RID: 959 RVA: 0x000162BC File Offset: 0x000144BC
		// (remove) Token: 0x060003C0 RID: 960 RVA: 0x000162F0 File Offset: 0x000144F0
		internal static event DecalProjector.DecalProjectorAction onDecalMaterialChange;

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x00016323 File Offset: 0x00014523
		// (set) Token: 0x060003C2 RID: 962 RVA: 0x0001632A File Offset: 0x0001452A
		internal static Material defaultMaterial { get; set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x00016332 File Offset: 0x00014532
		internal static bool isSupported
		{
			get
			{
				return DecalProjector.onDecalAdd != null;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x0001633C File Offset: 0x0001453C
		// (set) Token: 0x060003C5 RID: 965 RVA: 0x00016344 File Offset: 0x00014544
		internal DecalEntity decalEntity { get; set; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x0001634D File Offset: 0x0001454D
		// (set) Token: 0x060003C7 RID: 967 RVA: 0x00016355 File Offset: 0x00014555
		public Material material
		{
			get
			{
				return this.m_Material;
			}
			set
			{
				this.m_Material = value;
				this.OnValidate();
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x00016364 File Offset: 0x00014564
		// (set) Token: 0x060003C9 RID: 969 RVA: 0x0001636C File Offset: 0x0001456C
		public float drawDistance
		{
			get
			{
				return this.m_DrawDistance;
			}
			set
			{
				this.m_DrawDistance = Mathf.Max(0f, value);
				this.OnValidate();
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060003CA RID: 970 RVA: 0x00016385 File Offset: 0x00014585
		// (set) Token: 0x060003CB RID: 971 RVA: 0x0001638D File Offset: 0x0001458D
		public float fadeScale
		{
			get
			{
				return this.m_FadeScale;
			}
			set
			{
				this.m_FadeScale = Mathf.Clamp01(value);
				this.OnValidate();
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060003CC RID: 972 RVA: 0x000163A1 File Offset: 0x000145A1
		// (set) Token: 0x060003CD RID: 973 RVA: 0x000163A9 File Offset: 0x000145A9
		public float startAngleFade
		{
			get
			{
				return this.m_StartAngleFade;
			}
			set
			{
				this.m_StartAngleFade = Mathf.Clamp(value, 0f, 180f);
				this.OnValidate();
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060003CE RID: 974 RVA: 0x000163C7 File Offset: 0x000145C7
		// (set) Token: 0x060003CF RID: 975 RVA: 0x000163CF File Offset: 0x000145CF
		public float endAngleFade
		{
			get
			{
				return this.m_EndAngleFade;
			}
			set
			{
				this.m_EndAngleFade = Mathf.Clamp(value, this.m_StartAngleFade, 180f);
				this.OnValidate();
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x000163EE File Offset: 0x000145EE
		// (set) Token: 0x060003D1 RID: 977 RVA: 0x000163F6 File Offset: 0x000145F6
		public Vector2 uvScale
		{
			get
			{
				return this.m_UVScale;
			}
			set
			{
				this.m_UVScale = value;
				this.OnValidate();
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x00016405 File Offset: 0x00014605
		// (set) Token: 0x060003D3 RID: 979 RVA: 0x0001640D File Offset: 0x0001460D
		public Vector2 uvBias
		{
			get
			{
				return this.m_UVBias;
			}
			set
			{
				this.m_UVBias = value;
				this.OnValidate();
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x0001641C File Offset: 0x0001461C
		// (set) Token: 0x060003D5 RID: 981 RVA: 0x00016424 File Offset: 0x00014624
		public uint renderingLayerMask
		{
			get
			{
				return this.m_DecalLayerMask;
			}
			set
			{
				this.m_DecalLayerMask = value;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x0001642D File Offset: 0x0001462D
		// (set) Token: 0x060003D7 RID: 983 RVA: 0x00016435 File Offset: 0x00014635
		public DecalScaleMode scaleMode
		{
			get
			{
				return this.m_ScaleMode;
			}
			set
			{
				this.m_ScaleMode = value;
				this.OnValidate();
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x00016444 File Offset: 0x00014644
		// (set) Token: 0x060003D9 RID: 985 RVA: 0x0001644C File Offset: 0x0001464C
		public Vector3 pivot
		{
			get
			{
				return this.m_Offset;
			}
			set
			{
				this.m_Offset = value;
				this.OnValidate();
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060003DA RID: 986 RVA: 0x0001645B File Offset: 0x0001465B
		// (set) Token: 0x060003DB RID: 987 RVA: 0x00016463 File Offset: 0x00014663
		public Vector3 size
		{
			get
			{
				return this.m_Size;
			}
			set
			{
				this.m_Size = value;
				this.OnValidate();
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060003DC RID: 988 RVA: 0x00016472 File Offset: 0x00014672
		// (set) Token: 0x060003DD RID: 989 RVA: 0x0001647A File Offset: 0x0001467A
		public float fadeFactor
		{
			get
			{
				return this.m_FadeFactor;
			}
			set
			{
				this.m_FadeFactor = Mathf.Clamp01(value);
				this.OnValidate();
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060003DE RID: 990 RVA: 0x0001648E File Offset: 0x0001468E
		internal Vector3 effectiveScale
		{
			get
			{
				if (this.m_ScaleMode != DecalScaleMode.InheritFromHierarchy)
				{
					return Vector3.one;
				}
				return base.transform.lossyScale;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060003DF RID: 991 RVA: 0x000164AA File Offset: 0x000146AA
		internal Vector3 decalSize
		{
			get
			{
				return new Vector3(this.m_Size.x, this.m_Size.z, this.m_Size.y);
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x000164D2 File Offset: 0x000146D2
		internal Vector3 decalOffset
		{
			get
			{
				return new Vector3(this.m_Offset.x, -this.m_Offset.z, this.m_Offset.y);
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x000164FB File Offset: 0x000146FB
		internal Vector4 uvScaleBias
		{
			get
			{
				return new Vector4(this.m_UVScale.x, this.m_UVScale.y, this.m_UVBias.x, this.m_UVBias.y);
			}
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0001652E File Offset: 0x0001472E
		private void InitMaterial()
		{
			this.m_Material == null;
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0001653D File Offset: 0x0001473D
		private void OnEnable()
		{
			this.InitMaterial();
			this.m_OldMaterial = this.m_Material;
			DecalProjector.DecalProjectorAction decalProjectorAction = DecalProjector.onDecalAdd;
			if (decalProjectorAction == null)
			{
				return;
			}
			decalProjectorAction(this);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00016561 File Offset: 0x00014761
		private void OnDisable()
		{
			DecalProjector.DecalProjectorAction decalProjectorAction = DecalProjector.onDecalRemove;
			if (decalProjectorAction == null)
			{
				return;
			}
			decalProjectorAction(this);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00016574 File Offset: 0x00014774
		internal void OnValidate()
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.m_Material != this.m_OldMaterial)
			{
				DecalProjector.DecalProjectorAction decalProjectorAction = DecalProjector.onDecalMaterialChange;
				if (decalProjectorAction != null)
				{
					decalProjectorAction(this);
				}
				this.m_OldMaterial = this.m_Material;
			}
			else
			{
				DecalProjector.DecalProjectorAction decalProjectorAction2 = DecalProjector.onDecalPropertyChange;
				if (decalProjectorAction2 != null)
				{
					decalProjectorAction2(this);
				}
			}
			this.m_OldDrawDistance = this.m_DrawDistance;
			this.m_OldFadeScale = this.m_FadeScale;
			this.m_OldStartAngleFade = this.m_StartAngleFade;
			this.m_OldEndAngleFade = this.m_EndAngleFade;
			this.m_OldUVScale = this.m_UVScale;
			this.m_OldUVBias = this.m_UVBias;
			this.m_OldScaleMode = this.m_ScaleMode;
			this.m_OldOffset = this.m_Offset;
			this.m_OldSize = this.m_Size;
			this.m_OldFadeFactor = this.m_FadeFactor;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00016648 File Offset: 0x00014848
		private void OnDidApplyAnimationProperties()
		{
			if (this.m_OldMaterial != this.m_Material || Mathf.Abs(this.m_OldDrawDistance - this.m_DrawDistance) > Mathf.Epsilon || Mathf.Abs(this.m_OldFadeScale - this.m_FadeScale) > Mathf.Epsilon || Mathf.Abs(this.m_OldStartAngleFade - this.m_StartAngleFade) > Mathf.Epsilon || Mathf.Abs(this.m_OldEndAngleFade - this.m_EndAngleFade) > Mathf.Epsilon || this.m_OldUVScale != this.m_UVScale || this.m_OldUVBias != this.m_UVBias || this.m_OldScaleMode != this.m_ScaleMode || this.m_OldOffset != this.m_Offset || this.m_OldSize != this.m_Size || Mathf.Abs(this.m_OldFadeFactor - this.m_FadeFactor) > Mathf.Epsilon)
			{
				this.OnValidate();
			}
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00016754 File Offset: 0x00014954
		public bool IsValid()
		{
			return !(this.material == null) && (this.material.FindPass("DBufferProjector") != -1 || this.material.FindPass("DecalProjectorForwardEmissive") != -1 || this.material.FindPass("DecalScreenSpaceProjector") != -1 || this.material.FindPass("DecalGBufferProjector") != -1);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x000167C6 File Offset: 0x000149C6
		internal static void UpdateAllDecalProperties()
		{
			Action action = DecalProjector.onAllDecalPropertyChange;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x040002CC RID: 716
		[SerializeField]
		private Material m_Material;

		// Token: 0x040002CD RID: 717
		[SerializeField]
		private float m_DrawDistance = 1000f;

		// Token: 0x040002CE RID: 718
		[SerializeField]
		[Range(0f, 1f)]
		private float m_FadeScale = 0.9f;

		// Token: 0x040002CF RID: 719
		[SerializeField]
		[Range(0f, 180f)]
		private float m_StartAngleFade = 180f;

		// Token: 0x040002D0 RID: 720
		[SerializeField]
		[Range(0f, 180f)]
		private float m_EndAngleFade = 180f;

		// Token: 0x040002D1 RID: 721
		[SerializeField]
		private Vector2 m_UVScale = new Vector2(1f, 1f);

		// Token: 0x040002D2 RID: 722
		[SerializeField]
		private Vector2 m_UVBias = new Vector2(0f, 0f);

		// Token: 0x040002D3 RID: 723
		[SerializeField]
		private uint m_DecalLayerMask = 1U;

		// Token: 0x040002D4 RID: 724
		[SerializeField]
		private DecalScaleMode m_ScaleMode;

		// Token: 0x040002D5 RID: 725
		[SerializeField]
		internal Vector3 m_Offset = new Vector3(0f, 0f, 0.5f);

		// Token: 0x040002D6 RID: 726
		[SerializeField]
		internal Vector3 m_Size = new Vector3(1f, 1f, 1f);

		// Token: 0x040002D7 RID: 727
		[SerializeField]
		[Range(0f, 1f)]
		private float m_FadeFactor = 1f;

		// Token: 0x040002D8 RID: 728
		private Material m_OldMaterial;

		// Token: 0x040002D9 RID: 729
		private float m_OldDrawDistance = 1000f;

		// Token: 0x040002DA RID: 730
		private float m_OldFadeScale = 0.9f;

		// Token: 0x040002DB RID: 731
		private float m_OldStartAngleFade = 180f;

		// Token: 0x040002DC RID: 732
		private float m_OldEndAngleFade = 180f;

		// Token: 0x040002DD RID: 733
		private Vector2 m_OldUVScale = new Vector2(1f, 1f);

		// Token: 0x040002DE RID: 734
		private Vector2 m_OldUVBias = new Vector2(0f, 0f);

		// Token: 0x040002DF RID: 735
		private DecalScaleMode m_OldScaleMode;

		// Token: 0x040002E0 RID: 736
		private Vector3 m_OldOffset = new Vector3(0f, 0f, 0.5f);

		// Token: 0x040002E1 RID: 737
		private Vector3 m_OldSize = new Vector3(1f, 1f, 1f);

		// Token: 0x040002E2 RID: 738
		private float m_OldFadeFactor = 1f;

		// Token: 0x0200017C RID: 380
		// (Invoke) Token: 0x06000A9B RID: 2715
		internal delegate void DecalProjectorAction(DecalProjector decalProjector);
	}
}
