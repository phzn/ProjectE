using System;
using DA_Assets.DAI;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x0200037E RID: 894
	[FcuPropertyHeader(FcuLocKey.label_svg_importer_settings, FcuLocKey.tooltip_svg_importer_settings)]
	[Serializable]
	public class SVGImporterSettings : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06001747 RID: 5959 RVA: 0x00096F11 File Offset: 0x00095111
		// (set) Token: 0x06001748 RID: 5960 RVA: 0x00096F19 File Offset: 0x00095119
		[FcuInspectorProperty(ComponentType.IntField, FcuLocKey.label_gradient_resolution, FcuLocKey.tooltip_gradient_resolution, 0f, 1f)]
		public int GradientResolution
		{
			get
			{
				return this.gradientResolution;
			}
			set
			{
				base.SetValue<int>(ref this.gradientResolution, value);
			}
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06001749 RID: 5961 RVA: 0x00096F28 File Offset: 0x00095128
		// (set) Token: 0x0600174A RID: 5962 RVA: 0x00096F30 File Offset: 0x00095130
		[FcuInspectorProperty(ComponentType.Vector2Field, FcuLocKey.label_custom_pivot, FcuLocKey.tooltip_custom_pivot, 0f, 1f)]
		public Vector2 CustomPivot
		{
			get
			{
				return this.customPivot;
			}
			set
			{
				base.SetValue<Vector2>(ref this.customPivot, value);
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x0600174B RID: 5963 RVA: 0x00096F3F File Offset: 0x0009513F
		// (set) Token: 0x0600174C RID: 5964 RVA: 0x00096F47 File Offset: 0x00095147
		[FcuInspectorProperty(ComponentType.Toggle, FcuLocKey.label_generate_physics_shape, FcuLocKey.tooltip_generate_physics_shape, 0f, 1f)]
		public bool GeneratePhysicsShape
		{
			get
			{
				return this.generatePhysicsShape;
			}
			set
			{
				base.SetValue<bool>(ref this.generatePhysicsShape, value);
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x0600174D RID: 5965 RVA: 0x00096F56 File Offset: 0x00095156
		// (set) Token: 0x0600174E RID: 5966 RVA: 0x00096F5E File Offset: 0x0009515E
		[FcuInspectorProperty(ComponentType.FloatField, FcuLocKey.label_step_distance, FcuLocKey.tooltip_step_distance, 0f, 1f)]
		public float StepDistance
		{
			get
			{
				return this.stepDistance;
			}
			set
			{
				base.SetValue<float>(ref this.stepDistance, value);
			}
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x0600174F RID: 5967 RVA: 0x00096F6D File Offset: 0x0009516D
		// (set) Token: 0x06001750 RID: 5968 RVA: 0x00096F75 File Offset: 0x00095175
		[FcuInspectorProperty(ComponentType.FloatField, FcuLocKey.label_sampling_steps, FcuLocKey.tooltip_sampling_steps, 0f, 1f)]
		public float SamplingSteps
		{
			get
			{
				return this.samplingSteps;
			}
			set
			{
				base.SetValue<float>(ref this.samplingSteps, value);
			}
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001751 RID: 5969 RVA: 0x00096F84 File Offset: 0x00095184
		// (set) Token: 0x06001752 RID: 5970 RVA: 0x00096F8C File Offset: 0x0009518C
		[FcuInspectorProperty(ComponentType.Toggle, FcuLocKey.label_advanced_mode, FcuLocKey.tooltip_advanced_mode, 0f, 1f)]
		public bool AdvancedMode
		{
			get
			{
				return this.advancedMode;
			}
			set
			{
				base.SetValue<bool>(ref this.advancedMode, value);
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001753 RID: 5971 RVA: 0x00096F9B File Offset: 0x0009519B
		// (set) Token: 0x06001754 RID: 5972 RVA: 0x00096FA3 File Offset: 0x000951A3
		[FcuInspectorProperty(ComponentType.Toggle, FcuLocKey.label_max_cord_deviation_enabled, FcuLocKey.tooltip_max_cord_deviation_enabled, 0f, 1f)]
		public bool MaxCordDeviationEnabled
		{
			get
			{
				return this.maxCordDeviationEnabled;
			}
			set
			{
				base.SetValue<bool>(ref this.maxCordDeviationEnabled, value);
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001755 RID: 5973 RVA: 0x00096FB2 File Offset: 0x000951B2
		// (set) Token: 0x06001756 RID: 5974 RVA: 0x00096FBA File Offset: 0x000951BA
		[FcuInspectorProperty(ComponentType.FloatField, FcuLocKey.label_max_cord_deviation, FcuLocKey.tooltip_max_cord_deviation, 0f, 1f)]
		public float MaxCordDeviation
		{
			get
			{
				return this.maxCordDeviation;
			}
			set
			{
				base.SetValue<float>(ref this.maxCordDeviation, value);
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001757 RID: 5975 RVA: 0x00096FC9 File Offset: 0x000951C9
		// (set) Token: 0x06001758 RID: 5976 RVA: 0x00096FD1 File Offset: 0x000951D1
		[FcuInspectorProperty(ComponentType.Toggle, FcuLocKey.label_max_tangent_angle_enabled, FcuLocKey.tooltip_max_tangent_angle_enabled, 0f, 1f)]
		public bool MaxTangentAngleEnabled
		{
			get
			{
				return this.maxTangentAngleEnabled;
			}
			set
			{
				base.SetValue<bool>(ref this.maxTangentAngleEnabled, value);
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001759 RID: 5977 RVA: 0x00096FE0 File Offset: 0x000951E0
		// (set) Token: 0x0600175A RID: 5978 RVA: 0x00096FE8 File Offset: 0x000951E8
		[FcuInspectorProperty(ComponentType.FloatField, FcuLocKey.label_max_tangent_angle, FcuLocKey.tooltip_max_tangent_angle, 0f, 1f)]
		public float MaxTangentAngle
		{
			get
			{
				return this.maxTangentAngle;
			}
			set
			{
				base.SetValue<float>(ref this.maxTangentAngle, value);
			}
		}

		// Token: 0x040018EA RID: 6378
		[SerializeField]
		private int gradientResolution = 64;

		// Token: 0x040018EB RID: 6379
		[SerializeField]
		private Vector2 customPivot = new Vector2(0.5f, 0.5f);

		// Token: 0x040018EC RID: 6380
		[SerializeField]
		private bool generatePhysicsShape;

		// Token: 0x040018ED RID: 6381
		[SerializeField]
		private float stepDistance = 1f;

		// Token: 0x040018EE RID: 6382
		[SerializeField]
		private float samplingSteps = 3f;

		// Token: 0x040018EF RID: 6383
		[SerializeField]
		private bool advancedMode = true;

		// Token: 0x040018F0 RID: 6384
		[SerializeField]
		private bool maxCordDeviationEnabled;

		// Token: 0x040018F1 RID: 6385
		[SerializeField]
		private float maxCordDeviation = 1f;

		// Token: 0x040018F2 RID: 6386
		[SerializeField]
		private bool maxTangentAngleEnabled;

		// Token: 0x040018F3 RID: 6387
		[SerializeField]
		private float maxTangentAngle = 5f;
	}
}
