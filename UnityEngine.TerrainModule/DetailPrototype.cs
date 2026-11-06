using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000010 RID: 16
	[NativeHeader("TerrainScriptingClasses.h")]
	[NativeHeader("Modules/Terrain/Public/TerrainDataScriptingInterface.h")]
	[UsedByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class DetailPrototype
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00002614 File Offset: 0x00000814
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x0000262C File Offset: 0x0000082C
		public GameObject prototype
		{
			get
			{
				return this.m_Prototype;
			}
			set
			{
				this.m_Prototype = value;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00002638 File Offset: 0x00000838
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x00002650 File Offset: 0x00000850
		public Texture2D prototypeTexture
		{
			get
			{
				return this.m_PrototypeTexture;
			}
			set
			{
				this.m_PrototypeTexture = value;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x0000265C File Offset: 0x0000085C
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x00002674 File Offset: 0x00000874
		public float minWidth
		{
			get
			{
				return this.m_MinWidth;
			}
			set
			{
				this.m_MinWidth = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00002680 File Offset: 0x00000880
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x00002698 File Offset: 0x00000898
		public float maxWidth
		{
			get
			{
				return this.m_MaxWidth;
			}
			set
			{
				this.m_MaxWidth = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x000026A4 File Offset: 0x000008A4
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x000026BC File Offset: 0x000008BC
		public float minHeight
		{
			get
			{
				return this.m_MinHeight;
			}
			set
			{
				this.m_MinHeight = value;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000AA RID: 170 RVA: 0x000026C8 File Offset: 0x000008C8
		// (set) Token: 0x060000AB RID: 171 RVA: 0x000026E0 File Offset: 0x000008E0
		public float maxHeight
		{
			get
			{
				return this.m_MaxHeight;
			}
			set
			{
				this.m_MaxHeight = value;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000AC RID: 172 RVA: 0x000026EC File Offset: 0x000008EC
		// (set) Token: 0x060000AD RID: 173 RVA: 0x00002704 File Offset: 0x00000904
		public int noiseSeed
		{
			get
			{
				return this.m_NoiseSeed;
			}
			set
			{
				this.m_NoiseSeed = value;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00002710 File Offset: 0x00000910
		// (set) Token: 0x060000AF RID: 175 RVA: 0x00002728 File Offset: 0x00000928
		public float noiseSpread
		{
			get
			{
				return this.m_NoiseSpread;
			}
			set
			{
				this.m_NoiseSpread = value;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00002734 File Offset: 0x00000934
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x0000274C File Offset: 0x0000094C
		public float density
		{
			get
			{
				return this.m_Density;
			}
			set
			{
				this.m_Density = value;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00002758 File Offset: 0x00000958
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x0000222B File Offset: 0x0000042B
		[Obsolete("bendFactor has no effect and is deprecated.", false)]
		public float bendFactor
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00002770 File Offset: 0x00000970
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x00002788 File Offset: 0x00000988
		public float holeEdgePadding
		{
			get
			{
				return this.m_HoleEdgePadding;
			}
			set
			{
				this.m_HoleEdgePadding = value;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00002794 File Offset: 0x00000994
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x000027AC File Offset: 0x000009AC
		public Color healthyColor
		{
			get
			{
				return this.m_HealthyColor;
			}
			set
			{
				this.m_HealthyColor = value;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x000027B8 File Offset: 0x000009B8
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x000027D0 File Offset: 0x000009D0
		public Color dryColor
		{
			get
			{
				return this.m_DryColor;
			}
			set
			{
				this.m_DryColor = value;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000BA RID: 186 RVA: 0x000027DC File Offset: 0x000009DC
		// (set) Token: 0x060000BB RID: 187 RVA: 0x000027F4 File Offset: 0x000009F4
		public DetailRenderMode renderMode
		{
			get
			{
				return (DetailRenderMode)this.m_RenderMode;
			}
			set
			{
				this.m_RenderMode = (int)value;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00002800 File Offset: 0x00000A00
		// (set) Token: 0x060000BD RID: 189 RVA: 0x0000281B File Offset: 0x00000A1B
		public bool usePrototypeMesh
		{
			get
			{
				return this.m_UsePrototypeMesh != 0;
			}
			set
			{
				this.m_UsePrototypeMesh = (value ? 1 : 0);
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000BE RID: 190 RVA: 0x0000282C File Offset: 0x00000A2C
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00002847 File Offset: 0x00000A47
		public bool useInstancing
		{
			get
			{
				return this.m_UseInstancing != 0;
			}
			set
			{
				this.m_UseInstancing = (value ? 1 : 0);
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00002858 File Offset: 0x00000A58
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00002870 File Offset: 0x00000A70
		public float targetCoverage
		{
			get
			{
				return this.m_TargetCoverage;
			}
			set
			{
				this.m_TargetCoverage = value;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x0000287C File Offset: 0x00000A7C
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x00002897 File Offset: 0x00000A97
		public bool useDensityScaling
		{
			get
			{
				return this.m_UseDensityScaling != 0;
			}
			set
			{
				this.m_UseDensityScaling = (value ? 1 : 0);
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x000028A8 File Offset: 0x00000AA8
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x000028C0 File Offset: 0x00000AC0
		public float alignToGround
		{
			get
			{
				return this.m_AlignToGround;
			}
			set
			{
				this.m_AlignToGround = value;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x000028CC File Offset: 0x00000ACC
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x000028E4 File Offset: 0x00000AE4
		public float positionJitter
		{
			get
			{
				return this.m_PositionJitter;
			}
			set
			{
				this.m_PositionJitter = value;
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000028F0 File Offset: 0x00000AF0
		public DetailPrototype()
		{
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000029BC File Offset: 0x00000BBC
		public DetailPrototype(DetailPrototype other)
		{
			this.m_Prototype = other.m_Prototype;
			this.m_PrototypeTexture = other.m_PrototypeTexture;
			this.m_HealthyColor = other.m_HealthyColor;
			this.m_DryColor = other.m_DryColor;
			this.m_MinWidth = other.m_MinWidth;
			this.m_MaxWidth = other.m_MaxWidth;
			this.m_MinHeight = other.m_MinHeight;
			this.m_MaxHeight = other.m_MaxHeight;
			this.m_NoiseSeed = other.m_NoiseSeed;
			this.m_NoiseSpread = other.m_NoiseSpread;
			this.m_Density = other.m_Density;
			this.m_HoleEdgePadding = other.m_HoleEdgePadding;
			this.m_RenderMode = other.m_RenderMode;
			this.m_UsePrototypeMesh = other.m_UsePrototypeMesh;
			this.m_UseInstancing = other.m_UseInstancing;
			this.m_UseDensityScaling = other.m_UseDensityScaling;
			this.m_AlignToGround = other.m_AlignToGround;
			this.m_PositionJitter = other.m_PositionJitter;
			this.m_TargetCoverage = other.m_TargetCoverage;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00002B6C File Offset: 0x00000D6C
		public override bool Equals(object obj)
		{
			return this.Equals(obj as DetailPrototype);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00002B8C File Offset: 0x00000D8C
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00002BA4 File Offset: 0x00000DA4
		private bool Equals(DetailPrototype other)
		{
			bool flag = other == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = other == this;
				if (flag3)
				{
					flag2 = true;
				}
				else
				{
					bool flag4 = base.GetType() != other.GetType();
					flag2 = !flag4 && (this.m_Prototype == other.m_Prototype && this.m_PrototypeTexture == other.m_PrototypeTexture && this.m_HealthyColor == other.m_HealthyColor && this.m_DryColor == other.m_DryColor && this.m_MinWidth == other.m_MinWidth && this.m_MaxWidth == other.m_MaxWidth && this.m_MinHeight == other.m_MinHeight && this.m_MaxHeight == other.m_MaxHeight && this.m_NoiseSeed == other.m_NoiseSeed && this.m_NoiseSpread == other.m_NoiseSpread && this.m_Density == other.m_Density && this.m_HoleEdgePadding == other.m_HoleEdgePadding && this.m_RenderMode == other.m_RenderMode && this.m_UsePrototypeMesh == other.m_UsePrototypeMesh && this.m_UseInstancing == other.m_UseInstancing && this.m_TargetCoverage == other.m_TargetCoverage) && this.m_UseDensityScaling == other.m_UseDensityScaling;
				}
			}
			return flag2;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00002D10 File Offset: 0x00000F10
		public bool Validate()
		{
			string text;
			return DetailPrototype.ValidateDetailPrototype(this, out text);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00002D25 File Offset: 0x00000F25
		public bool Validate(out string errorMessage)
		{
			return DetailPrototype.ValidateDetailPrototype(this, out errorMessage);
		}

		// Token: 0x060000CF RID: 207
		[FreeFunction("TerrainDataScriptingInterface::ValidateDetailPrototype")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool ValidateDetailPrototype([NotNull("ArgumentNullException")] DetailPrototype prototype, out string errorMessage);

		// Token: 0x060000D0 RID: 208 RVA: 0x00002D30 File Offset: 0x00000F30
		internal static bool IsModeSupportedByRenderPipeline(DetailRenderMode renderMode, bool useInstancing, out string errorMessage)
		{
			bool flag = GraphicsSettings.currentRenderPipeline != null;
			if (flag)
			{
				bool flag2 = renderMode == DetailRenderMode.GrassBillboard && GraphicsSettings.currentRenderPipeline.terrainDetailGrassBillboardShader == null;
				if (flag2)
				{
					errorMessage = "The current render pipeline does not support Billboard details. Details will not be rendered.";
					return false;
				}
				bool flag3 = renderMode == DetailRenderMode.VertexLit && !useInstancing && GraphicsSettings.currentRenderPipeline.terrainDetailLitShader == null;
				if (flag3)
				{
					errorMessage = "The current render pipeline does not support VertexLit details. Details will be rendered using the default shader.";
					return false;
				}
				bool flag4 = renderMode == DetailRenderMode.Grass && GraphicsSettings.currentRenderPipeline.terrainDetailGrassShader == null;
				if (flag4)
				{
					errorMessage = "The current render pipeline does not support Grass details. Details will be rendered using the default shader without alpha test and animation.";
					return false;
				}
			}
			errorMessage = string.Empty;
			return true;
		}

		// Token: 0x0400002A RID: 42
		internal static readonly Color DefaultHealthColor = new Color(0.2627451f, 0.9764706f, 0.16470589f, 1f);

		// Token: 0x0400002B RID: 43
		internal static readonly Color DefaultDryColor = new Color(0.8039216f, 0.7372549f, 0.101960786f, 1f);

		// Token: 0x0400002C RID: 44
		internal GameObject m_Prototype = null;

		// Token: 0x0400002D RID: 45
		internal Texture2D m_PrototypeTexture = null;

		// Token: 0x0400002E RID: 46
		internal Color m_HealthyColor = DetailPrototype.DefaultHealthColor;

		// Token: 0x0400002F RID: 47
		internal Color m_DryColor = DetailPrototype.DefaultDryColor;

		// Token: 0x04000030 RID: 48
		internal float m_MinWidth = 1f;

		// Token: 0x04000031 RID: 49
		internal float m_MaxWidth = 2f;

		// Token: 0x04000032 RID: 50
		internal float m_MinHeight = 1f;

		// Token: 0x04000033 RID: 51
		internal float m_MaxHeight = 2f;

		// Token: 0x04000034 RID: 52
		internal int m_NoiseSeed = 0;

		// Token: 0x04000035 RID: 53
		internal float m_NoiseSpread = 0.1f;

		// Token: 0x04000036 RID: 54
		internal float m_Density = 1f;

		// Token: 0x04000037 RID: 55
		internal float m_HoleEdgePadding = 0f;

		// Token: 0x04000038 RID: 56
		internal int m_RenderMode = 2;

		// Token: 0x04000039 RID: 57
		internal int m_UsePrototypeMesh = 0;

		// Token: 0x0400003A RID: 58
		internal int m_UseInstancing = 0;

		// Token: 0x0400003B RID: 59
		internal int m_UseDensityScaling = 0;

		// Token: 0x0400003C RID: 60
		internal float m_AlignToGround = 0f;

		// Token: 0x0400003D RID: 61
		internal float m_PositionJitter = 0f;

		// Token: 0x0400003E RID: 62
		internal float m_TargetCoverage = 1f;
	}
}
