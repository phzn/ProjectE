using System;

namespace UnityEngine.AzureSky
{
	// Token: 0x0200001C RID: 28
	[ExecuteInEditMode]
	[AddComponentMenu("Azure[Sky] Dynamic Skybox/Azure Sky Renderer")]
	public sealed class AzureSkyRenderer : MonoBehaviour
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00003C75 File Offset: 0x00001E75
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x00003C7D File Offset: 0x00001E7D
		public Transform sunTransform
		{
			get
			{
				return this.m_sunTransform;
			}
			set
			{
				this.m_sunTransform = value;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00003C86 File Offset: 0x00001E86
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x00003C8E File Offset: 0x00001E8E
		public Transform moonTransform
		{
			get
			{
				return this.m_moonTransform;
			}
			set
			{
				this.m_moonTransform = value;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00003C97 File Offset: 0x00001E97
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x00003C9F File Offset: 0x00001E9F
		public Transform starfieldTransform
		{
			get
			{
				return this.m_starfieldTransform;
			}
			set
			{
				this.m_starfieldTransform = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00003CA8 File Offset: 0x00001EA8
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x00003CB0 File Offset: 0x00001EB0
		public Material skyMaterial
		{
			get
			{
				return this.m_skyMaterial;
			}
			set
			{
				this.m_skyMaterial = value;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00003CB9 File Offset: 0x00001EB9
		// (set) Token: 0x060000BA RID: 186 RVA: 0x00003CC1 File Offset: 0x00001EC1
		public Material fogMaterial
		{
			get
			{
				return this.m_fogMaterial;
			}
			set
			{
				this.m_fogMaterial = value;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00003CCA File Offset: 0x00001ECA
		// (set) Token: 0x060000BC RID: 188 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public Cubemap sunTexture
		{
			get
			{
				return this.m_sunTexture;
			}
			set
			{
				this.m_sunTexture = value;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00003CDB File Offset: 0x00001EDB
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00003CE3 File Offset: 0x00001EE3
		public Cubemap moonTexture
		{
			get
			{
				return this.m_moonTexture;
			}
			set
			{
				this.m_moonTexture = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00003CEC File Offset: 0x00001EEC
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00003CF4 File Offset: 0x00001EF4
		public Cubemap starfieldTexture
		{
			get
			{
				return this.m_starfieldTexture;
			}
			set
			{
				this.m_starfieldTexture = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00003CFD File Offset: 0x00001EFD
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00003D05 File Offset: 0x00001F05
		public Cubemap constellationTexture
		{
			get
			{
				return this.m_constellationTexture;
			}
			set
			{
				this.m_constellationTexture = value;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00003D0E File Offset: 0x00001F0E
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00003D16 File Offset: 0x00001F16
		public Texture2D dynamicCloudTexture
		{
			get
			{
				return this.m_dynamicCloudTexture;
			}
			set
			{
				this.m_dynamicCloudTexture = value;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00003D1F File Offset: 0x00001F1F
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x00003D27 File Offset: 0x00001F27
		public Shader emptySkyShader
		{
			get
			{
				return this.m_emptySkyShader;
			}
			set
			{
				this.m_emptySkyShader = value;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00003D30 File Offset: 0x00001F30
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00003D38 File Offset: 0x00001F38
		public Shader dynamicCloudShader
		{
			get
			{
				return this.m_dynamicCloudShader;
			}
			set
			{
				this.m_dynamicCloudShader = value;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00003D41 File Offset: 0x00001F41
		// (set) Token: 0x060000CA RID: 202 RVA: 0x00003D49 File Offset: 0x00001F49
		public Vector3 wavelength
		{
			get
			{
				return this.m_wavelength;
			}
			set
			{
				this.m_wavelength = value;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00003D52 File Offset: 0x00001F52
		// (set) Token: 0x060000CC RID: 204 RVA: 0x00003D5A File Offset: 0x00001F5A
		public float molecularDensity
		{
			get
			{
				return this.m_molecularDensity;
			}
			set
			{
				this.m_molecularDensity = value;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00003D63 File Offset: 0x00001F63
		// (set) Token: 0x060000CE RID: 206 RVA: 0x00003D6B File Offset: 0x00001F6B
		public float kr
		{
			get
			{
				return this.m_kr;
			}
			set
			{
				this.m_kr = value;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00003D74 File Offset: 0x00001F74
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x00003D7C File Offset: 0x00001F7C
		public float km
		{
			get
			{
				return this.m_km;
			}
			set
			{
				this.m_km = value;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00003D85 File Offset: 0x00001F85
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x00003D8D File Offset: 0x00001F8D
		public float rayleigh
		{
			get
			{
				return this.m_rayleigh;
			}
			set
			{
				this.m_rayleigh = value;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00003D96 File Offset: 0x00001F96
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x00003D9E File Offset: 0x00001F9E
		public float mie
		{
			get
			{
				return this.m_mie;
			}
			set
			{
				this.m_mie = value;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00003DA7 File Offset: 0x00001FA7
		// (set) Token: 0x060000D6 RID: 214 RVA: 0x00003DAF File Offset: 0x00001FAF
		public float mieDirectionalityFactor
		{
			get
			{
				return this.m_mieDirectionalityFactor;
			}
			set
			{
				this.m_mieDirectionalityFactor = value;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00003DB8 File Offset: 0x00001FB8
		// (set) Token: 0x060000D8 RID: 216 RVA: 0x00003DC0 File Offset: 0x00001FC0
		public float scattering
		{
			get
			{
				return this.m_scattering;
			}
			set
			{
				this.m_scattering = value;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00003DC9 File Offset: 0x00001FC9
		// (set) Token: 0x060000DA RID: 218 RVA: 0x00003DD1 File Offset: 0x00001FD1
		public float skyLuminance
		{
			get
			{
				return this.m_skyLuminance;
			}
			set
			{
				this.m_skyLuminance = value;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00003DDA File Offset: 0x00001FDA
		// (set) Token: 0x060000DC RID: 220 RVA: 0x00003DE2 File Offset: 0x00001FE2
		public float exposure
		{
			get
			{
				return this.m_exposure;
			}
			set
			{
				this.m_exposure = value;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000DD RID: 221 RVA: 0x00003DEB File Offset: 0x00001FEB
		// (set) Token: 0x060000DE RID: 222 RVA: 0x00003DF3 File Offset: 0x00001FF3
		public Color rayleighColor
		{
			get
			{
				return this.m_rayleighColor;
			}
			set
			{
				this.m_rayleighColor = value;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00003DFC File Offset: 0x00001FFC
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x00003E04 File Offset: 0x00002004
		public Color mieColor
		{
			get
			{
				return this.m_mieColor;
			}
			set
			{
				this.m_mieColor = value;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00003E0D File Offset: 0x0000200D
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x00003E15 File Offset: 0x00002015
		public float sunSize
		{
			get
			{
				return this.m_sunSize;
			}
			set
			{
				this.m_sunSize = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00003E1E File Offset: 0x0000201E
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00003E26 File Offset: 0x00002026
		public float sunOpacity
		{
			get
			{
				return this.m_sunOpacity;
			}
			set
			{
				this.m_sunOpacity = value;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00003E2F File Offset: 0x0000202F
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x00003E37 File Offset: 0x00002037
		public Color sunColor
		{
			get
			{
				return this.m_sunColor;
			}
			set
			{
				this.m_sunColor = value;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00003E40 File Offset: 0x00002040
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x00003E48 File Offset: 0x00002048
		public float moonSize
		{
			get
			{
				return this.m_moonSize;
			}
			set
			{
				this.m_moonSize = value;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00003E51 File Offset: 0x00002051
		// (set) Token: 0x060000EA RID: 234 RVA: 0x00003E59 File Offset: 0x00002059
		public float moonOpacity
		{
			get
			{
				return this.m_moonOpacity;
			}
			set
			{
				this.m_moonOpacity = value;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00003E62 File Offset: 0x00002062
		// (set) Token: 0x060000EC RID: 236 RVA: 0x00003E6A File Offset: 0x0000206A
		public Color moonColor
		{
			get
			{
				return this.m_moonColor;
			}
			set
			{
				this.m_moonColor = value;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00003E73 File Offset: 0x00002073
		// (set) Token: 0x060000EE RID: 238 RVA: 0x00003E7B File Offset: 0x0000207B
		public Vector3 moonRotationOffset
		{
			get
			{
				return this.m_moonRotationOffset;
			}
			set
			{
				this.m_moonRotationOffset = value;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00003E84 File Offset: 0x00002084
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x00003E8C File Offset: 0x0000208C
		public float starsIntensity
		{
			get
			{
				return this.m_starsIntensity;
			}
			set
			{
				this.m_starsIntensity = value;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00003E95 File Offset: 0x00002095
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x00003E9D File Offset: 0x0000209D
		public float milkyWayIntensity
		{
			get
			{
				return this.m_milkyWayIntensity;
			}
			set
			{
				this.m_milkyWayIntensity = value;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00003EA6 File Offset: 0x000020A6
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x00003EAE File Offset: 0x000020AE
		public Color starfieldColor
		{
			get
			{
				return this.m_starfieldColor;
			}
			set
			{
				this.m_starfieldColor = value;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00003EB7 File Offset: 0x000020B7
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x00003EBF File Offset: 0x000020BF
		public float skyExtinction
		{
			get
			{
				return this.m_skyExtinction;
			}
			set
			{
				this.m_skyExtinction = value;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00003EC8 File Offset: 0x000020C8
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00003ED0 File Offset: 0x000020D0
		public float constellationIntensity
		{
			get
			{
				return this.m_constellationIntensity;
			}
			set
			{
				this.m_constellationIntensity = value;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00003ED9 File Offset: 0x000020D9
		// (set) Token: 0x060000FA RID: 250 RVA: 0x00003EE1 File Offset: 0x000020E1
		public Color constellationColor
		{
			get
			{
				return this.m_constellationColor;
			}
			set
			{
				this.m_constellationColor = value;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00003EEA File Offset: 0x000020EA
		// (set) Token: 0x060000FC RID: 252 RVA: 0x00003EF2 File Offset: 0x000020F2
		public Vector3 starfieldRotationOffset
		{
			get
			{
				return this.m_starfieldRotationOffset;
			}
			set
			{
				this.m_starfieldRotationOffset = value;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00003EFB File Offset: 0x000020FB
		// (set) Token: 0x060000FE RID: 254 RVA: 0x00003F03 File Offset: 0x00002103
		public float mieDistance
		{
			get
			{
				return this.m_mieDistance;
			}
			set
			{
				this.m_mieDistance = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00003F0C File Offset: 0x0000210C
		// (set) Token: 0x06000100 RID: 256 RVA: 0x00003F14 File Offset: 0x00002114
		public float globalFogDistance
		{
			get
			{
				return this.m_globalFogDistance;
			}
			set
			{
				this.m_globalFogDistance = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00003F1D File Offset: 0x0000211D
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00003F25 File Offset: 0x00002125
		public float globalFogSmoothStep
		{
			get
			{
				return this.m_globalFogSmoothStep;
			}
			set
			{
				this.m_globalFogSmoothStep = value;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00003F2E File Offset: 0x0000212E
		// (set) Token: 0x06000104 RID: 260 RVA: 0x00003F36 File Offset: 0x00002136
		public float globalFogDensity
		{
			get
			{
				return this.m_globalFogDensity;
			}
			set
			{
				this.m_globalFogDensity = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00003F3F File Offset: 0x0000213F
		// (set) Token: 0x06000106 RID: 262 RVA: 0x00003F47 File Offset: 0x00002147
		public float heightFogDistance
		{
			get
			{
				return this.m_heightFogDistance;
			}
			set
			{
				this.m_heightFogDistance = value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00003F50 File Offset: 0x00002150
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00003F58 File Offset: 0x00002158
		public float heightFogSmoothStep
		{
			get
			{
				return this.m_heightFogSmoothStep;
			}
			set
			{
				this.m_heightFogSmoothStep = value;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00003F61 File Offset: 0x00002161
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00003F69 File Offset: 0x00002169
		public float heightFogDensity
		{
			get
			{
				return this.m_heightFogDensity;
			}
			set
			{
				this.m_heightFogDensity = value;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00003F72 File Offset: 0x00002172
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00003F7A File Offset: 0x0000217A
		public float heightFogStartAltitude
		{
			get
			{
				return this.m_heightFogStartAltitude;
			}
			set
			{
				this.m_heightFogStartAltitude = value;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00003F83 File Offset: 0x00002183
		// (set) Token: 0x0600010E RID: 270 RVA: 0x00003F8B File Offset: 0x0000218B
		public float heightFogEndAltitude
		{
			get
			{
				return this.m_heightFogEndAltitude;
			}
			set
			{
				this.m_heightFogEndAltitude = value;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00003F94 File Offset: 0x00002194
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00003F9C File Offset: 0x0000219C
		public float fogBluishIntensity
		{
			get
			{
				return this.m_fogBluishIntensity;
			}
			set
			{
				this.m_fogBluishIntensity = value;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00003FA5 File Offset: 0x000021A5
		// (set) Token: 0x06000112 RID: 274 RVA: 0x00003FAD File Offset: 0x000021AD
		public float heightFogScatteringMultiplier
		{
			get
			{
				return this.m_heightFogScatteringMultiplier;
			}
			set
			{
				this.m_heightFogScatteringMultiplier = value;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00003FB6 File Offset: 0x000021B6
		// (set) Token: 0x06000114 RID: 276 RVA: 0x00003FBE File Offset: 0x000021BE
		public float dynamicCloudAltitude
		{
			get
			{
				return this.m_dynamicCloudAltitude;
			}
			set
			{
				this.m_dynamicCloudAltitude = value;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00003FC7 File Offset: 0x000021C7
		// (set) Token: 0x06000116 RID: 278 RVA: 0x00003FCF File Offset: 0x000021CF
		public float dynamicCloudDirection
		{
			get
			{
				return this.m_dynamicCloudDirection;
			}
			set
			{
				this.m_dynamicCloudDirection = value;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00003FD8 File Offset: 0x000021D8
		// (set) Token: 0x06000118 RID: 280 RVA: 0x00003FE0 File Offset: 0x000021E0
		public float dynamicCloudSpeed
		{
			get
			{
				return this.m_dynamicCloudSpeed;
			}
			set
			{
				this.m_dynamicCloudSpeed = value;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00003FE9 File Offset: 0x000021E9
		// (set) Token: 0x0600011A RID: 282 RVA: 0x00003FF1 File Offset: 0x000021F1
		public float dynamicCloudDensity
		{
			get
			{
				return this.m_dynamicCloudDensity;
			}
			set
			{
				this.m_dynamicCloudDensity = value;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00003FFA File Offset: 0x000021FA
		// (set) Token: 0x0600011C RID: 284 RVA: 0x00004002 File Offset: 0x00002202
		public Color dynamicCloudColor1
		{
			get
			{
				return this.m_dynamicCloudColor1;
			}
			set
			{
				this.m_dynamicCloudColor1 = value;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600011D RID: 285 RVA: 0x0000400B File Offset: 0x0000220B
		// (set) Token: 0x0600011E RID: 286 RVA: 0x00004013 File Offset: 0x00002213
		public Color dynamicCloudColor2
		{
			get
			{
				return this.m_dynamicCloudColor2;
			}
			set
			{
				this.m_dynamicCloudColor2 = value;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600011F RID: 287 RVA: 0x0000401C File Offset: 0x0000221C
		public Vector2 dynamicCloudUV
		{
			get
			{
				return this.m_dynamicCloudUV;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00004024 File Offset: 0x00002224
		// (set) Token: 0x06000121 RID: 289 RVA: 0x0000402C File Offset: 0x0000222C
		public AzureUpdateMode updateMode
		{
			get
			{
				return this.m_updateMode;
			}
			set
			{
				this.m_updateMode = value;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00004035 File Offset: 0x00002235
		// (set) Token: 0x06000123 RID: 291 RVA: 0x0000403D File Offset: 0x0000223D
		public float skyUpdateInterval
		{
			get
			{
				return this.m_skyUpdateInterval;
			}
			set
			{
				this.m_skyUpdateInterval = Mathf.Max(0.01f, value);
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00004050 File Offset: 0x00002250
		// (set) Token: 0x06000125 RID: 293 RVA: 0x00004058 File Offset: 0x00002258
		public float cloudUpdateInterval
		{
			get
			{
				return this.m_cloudUpdateInterval;
			}
			set
			{
				this.m_cloudUpdateInterval = Mathf.Max(0f, value);
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000126 RID: 294 RVA: 0x0000406B File Offset: 0x0000226B
		// (set) Token: 0x06000127 RID: 295 RVA: 0x00004073 File Offset: 0x00002273
		public bool useScatteringCache
		{
			get
			{
				return this.m_useScatteringCache;
			}
			set
			{
				this.m_useScatteringCache = value;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000128 RID: 296 RVA: 0x0000407C File Offset: 0x0000227C
		// (set) Token: 0x06000129 RID: 297 RVA: 0x00004084 File Offset: 0x00002284
		public AzureCloudMode cloudMode
		{
			get
			{
				return this.m_cloudMode;
			}
			set
			{
				this.m_cloudMode = value;
				this.UpdateCloudMode();
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00004093 File Offset: 0x00002293
		private void Awake()
		{
			this.UpdateCloudMode();
			this.InitializeSkySystem();
			if (Application.isPlaying && this.m_updateMode == AzureUpdateMode.LocallyEveryFrame)
			{
				this.UpdateSkySystem();
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x000040B8 File Offset: 0x000022B8
		private void Update()
		{
			if (Application.isPlaying)
			{
				if (this.m_cloudMode == AzureCloudMode.Dynamic && (this.m_cloudUpdateInterval <= 0f || Time.time - this.m_lastCloudUpdateTime >= this.m_cloudUpdateInterval))
				{
					this.m_dynamicCloudUV = this.CalculeDynamicCloudUV();
					Shader.SetGlobalVector(AzureShaderUniforms.DynamicCloudDirection, this.m_dynamicCloudUV);
					this.m_lastCloudUpdateTime = Time.time;
				}
				if (this.m_updateMode == AzureUpdateMode.LocallyEveryFrame && Time.time - this.m_lastSkyUpdateTime >= this.m_skyUpdateInterval)
				{
					this.UpdateSkySystem();
					this.m_lastSkyUpdateTime = Time.time;
				}
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00004152 File Offset: 0x00002352
		private void OnEnable()
		{
			if (this.m_skyMaterial)
			{
				RenderSettings.skybox = this.m_skyMaterial;
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000416C File Offset: 0x0000236C
		public void InitializeSkySystem()
		{
			Shader.SetGlobalTexture(AzureShaderUniforms.SunTexture, this.m_sunTexture);
			Shader.SetGlobalTexture(AzureShaderUniforms.MoonTexture, this.m_moonTexture);
			Shader.SetGlobalTexture(AzureShaderUniforms.StarfieldTexture, this.m_starfieldTexture);
			Shader.SetGlobalTexture(AzureShaderUniforms.ConstellationTexture, this.m_constellationTexture);
			Shader.SetGlobalTexture(AzureShaderUniforms.DynamicCloudTexture, this.m_dynamicCloudTexture);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000041CC File Offset: 0x000023CC
		public void UpdateSkySystem()
		{
			this.UpdateScatteringCache();
			this.m_moonRotation = Quaternion.Euler(this.m_moonRotationOffset);
			this.m_moonRotationMatrix = Matrix4x4.TRS(Vector3.zero, this.m_moonRotation, Vector3.one);
			this.m_starfieldRotation = Quaternion.Euler(this.m_starfieldRotationOffset);
			this.m_starfieldRotationMatrix = Matrix4x4.TRS(Vector3.zero, this.m_starfieldRotation, Vector3.one);
			Shader.SetGlobalVector(AzureShaderUniforms.SunDirection, -this.m_sunTransform.forward);
			Shader.SetGlobalVector(AzureShaderUniforms.MoonDirection, -this.m_moonTransform.forward);
			Shader.SetGlobalMatrix(AzureShaderUniforms.SunMatrix, this.m_sunTransform.worldToLocalMatrix);
			Shader.SetGlobalMatrix(AzureShaderUniforms.MoonMatrix, this.m_moonRotationMatrix * this.m_moonTransform.worldToLocalMatrix);
			Shader.SetGlobalMatrix(AzureShaderUniforms.UpDirectionMatrix, base.transform.worldToLocalMatrix);
			Shader.SetGlobalMatrix(AzureShaderUniforms.StarfieldMatrix, this.m_starfieldRotationMatrix * this.m_starfieldTransform.worldToLocalMatrix);
			Shader.SetGlobalFloat(AzureShaderUniforms.Kr, this.m_kr);
			Shader.SetGlobalFloat(AzureShaderUniforms.Km, this.m_km);
			Shader.SetGlobalVector(AzureShaderUniforms.Rayleigh, this.m_cachedRayleigh * this.m_rayleigh);
			Shader.SetGlobalVector(AzureShaderUniforms.Mie, this.m_cachedMie * this.m_mie);
			Shader.SetGlobalVector(AzureShaderUniforms.MieG, this.m_cachedMieG);
			Shader.SetGlobalFloat(AzureShaderUniforms.MieDistance, this.m_mieDistance);
			Shader.SetGlobalFloat(AzureShaderUniforms.Scattering, this.m_scattering);
			Shader.SetGlobalFloat(AzureShaderUniforms.SkyLuminance, this.m_skyLuminance);
			Shader.SetGlobalFloat(AzureShaderUniforms.Exposure, this.m_exposure);
			Shader.SetGlobalColor(AzureShaderUniforms.RayleighColor, this.m_rayleighColor);
			Shader.SetGlobalColor(AzureShaderUniforms.MieColor, this.m_mieColor);
			Shader.SetGlobalFloat(AzureShaderUniforms.SunRadius, this.m_sunSize * 696340f * 10f);
			Shader.SetGlobalVector(AzureShaderUniforms.SunPosition, -this.m_sunTransform.forward * 149600000f);
			Shader.SetGlobalFloat(AzureShaderUniforms.SunOpacity, this.m_sunOpacity);
			Shader.SetGlobalColor(AzureShaderUniforms.SunColor, this.m_sunColor);
			Shader.SetGlobalFloat(AzureShaderUniforms.MoonRadius, this.m_moonSize * 1737.4f * 10f);
			Shader.SetGlobalVector(AzureShaderUniforms.MoonPosition, -this.m_moonTransform.forward * 384400f);
			Shader.SetGlobalFloat(AzureShaderUniforms.MoonOpacity, this.m_moonOpacity);
			Shader.SetGlobalColor(AzureShaderUniforms.MoonColor, this.m_moonColor);
			Shader.SetGlobalFloat(AzureShaderUniforms.StarsIntensity, this.m_starsIntensity);
			Shader.SetGlobalFloat(AzureShaderUniforms.MilkyWayIntensity, this.m_milkyWayIntensity);
			Shader.SetGlobalColor(AzureShaderUniforms.StarFieldColor, this.m_starfieldColor);
			Shader.SetGlobalFloat(AzureShaderUniforms.SkyExtinction, this.m_skyExtinction);
			Shader.SetGlobalFloat(AzureShaderUniforms.ConstellationIntensity, this.m_constellationIntensity);
			Shader.SetGlobalColor(AzureShaderUniforms.ConstellationColor, this.m_constellationColor);
			Shader.SetGlobalFloat(AzureShaderUniforms.GlobalFogDistance, this.m_globalFogDistance);
			Shader.SetGlobalFloat(AzureShaderUniforms.GlobalFogSmoothStep, this.m_globalFogSmoothStep);
			Shader.SetGlobalFloat(AzureShaderUniforms.GlobalFogDensity, this.m_globalFogDensity);
			Shader.SetGlobalFloat(AzureShaderUniforms.HeightFogDistance, this.m_heightFogDistance);
			Shader.SetGlobalFloat(AzureShaderUniforms.HeightFogSmoothStep, this.m_heightFogSmoothStep);
			Shader.SetGlobalFloat(AzureShaderUniforms.HeightFogDensity, this.m_heightFogDensity);
			Shader.SetGlobalFloat(AzureShaderUniforms.HeightFogStartAltitude, this.m_heightFogStartAltitude);
			Shader.SetGlobalFloat(AzureShaderUniforms.HeightFogEndAltitude, this.m_heightFogEndAltitude);
			Shader.SetGlobalFloat(AzureShaderUniforms.FogBluishIntensity, this.m_fogBluishIntensity);
			Shader.SetGlobalFloat(AzureShaderUniforms.HeightFogScatteringMultiplier, this.m_heightFogScatteringMultiplier);
			Shader.SetGlobalFloat(AzureShaderUniforms.DynamicCloudAltitude, this.m_dynamicCloudAltitude);
			Shader.SetGlobalFloat(AzureShaderUniforms.DynamicCloudDensity, Mathf.Lerp(25f, 0f, this.m_dynamicCloudDensity));
			Shader.SetGlobalVector(AzureShaderUniforms.DynamicCloudColor1, this.m_dynamicCloudColor1);
			Shader.SetGlobalVector(AzureShaderUniforms.DynamicCloudColor2, this.m_dynamicCloudColor2);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000045D8 File Offset: 0x000027D8
		private void UpdateScatteringCache()
		{
			if (!this.m_useScatteringCache)
			{
				this.m_cachedRayleigh = this.ComputeRayleigh();
				this.m_cachedMie = this.ComputeMie();
				this.m_cachedMieG = this.ComputeMieG();
				return;
			}
			if (this.m_scatteringCacheDirty || this.m_lastCachedWavelength != this.m_wavelength || !Mathf.Approximately(this.m_lastCachedMolecularDensity, this.m_molecularDensity) || !Mathf.Approximately(this.m_lastCachedMieDirectionalityFactor, this.m_mieDirectionalityFactor))
			{
				this.m_cachedRayleigh = this.ComputeRayleigh();
				this.m_cachedMie = this.ComputeMie();
				this.m_cachedMieG = this.ComputeMieG();
				this.m_lastCachedWavelength = this.m_wavelength;
				this.m_lastCachedMolecularDensity = this.m_molecularDensity;
				this.m_lastCachedMieDirectionalityFactor = this.m_mieDirectionalityFactor;
				this.m_scatteringCacheDirty = false;
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000046A4 File Offset: 0x000028A4
		private Vector3 ComputeRayleigh()
		{
			Vector3 one = Vector3.one;
			Vector3 vector = this.m_wavelength * 1E-09f;
			float num = 1.0003f;
			float num2 = 0.035f;
			float num3 = num * num;
			float num4 = this.m_molecularDensity * 1E+25f;
			float num5 = 248.05023f * ((num3 - 1f) * (num3 - 1f)) / (3f * num4) * ((6f + 3f * num2) / (6f - 7f * num2));
			one.x = num5 / Mathf.Pow(vector.x, 4f);
			one.y = num5 / Mathf.Pow(vector.y, 4f);
			one.z = num5 / Mathf.Pow(vector.z, 4f);
			return one;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00004770 File Offset: 0x00002970
		private Vector3 ComputeMie()
		{
			Vector3 vector = new Vector3(686f, 678f, 682f);
			float num = 2.6209998E-08f;
			Vector3 vector2;
			vector2.x = 434f * num * 3.1415927f * Mathf.Pow(12.566371f / this.m_wavelength.x, 2f) * vector.x;
			vector2.y = 434f * num * 3.1415927f * Mathf.Pow(12.566371f / this.m_wavelength.y, 2f) * vector.y;
			vector2.z = 434f * num * 3.1415927f * Mathf.Pow(12.566371f / this.m_wavelength.z, 2f) * vector.z;
			return vector2;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000483F File Offset: 0x00002A3F
		private Vector3 ComputeMieG()
		{
			return new Vector3(1f - this.m_mieDirectionalityFactor * this.m_mieDirectionalityFactor, 1f + this.m_mieDirectionalityFactor * this.m_mieDirectionalityFactor, 2f * this.m_mieDirectionalityFactor);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00004878 File Offset: 0x00002A78
		private Vector2 CalculeDynamicCloudUV()
		{
			float num = this.m_dynamicCloudUV.x;
			float num2 = this.m_dynamicCloudUV.y;
			float num3 = this.m_dynamicCloudSpeed * 0.05f * Time.deltaTime;
			num += num3 * Mathf.Sin(0.01745329f * this.m_dynamicCloudDirection);
			num2 += num3 * Mathf.Cos(0.01745329f * this.m_dynamicCloudDirection);
			if (num >= 1f)
			{
				num -= 1f;
			}
			if (num2 >= 1f)
			{
				num2 -= 1f;
			}
			return new Vector2(num, num2);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00004904 File Offset: 0x00002B04
		public void UpdateCloudMode()
		{
			AzureCloudMode cloudMode = this.m_cloudMode;
			if (cloudMode != AzureCloudMode.Off)
			{
				if (cloudMode != AzureCloudMode.Dynamic)
				{
					return;
				}
				if (this.m_dynamicCloudShader != null)
				{
					this.m_skyMaterial.shader = this.m_dynamicCloudShader;
				}
			}
			else if (this.m_emptySkyShader != null)
			{
				this.m_skyMaterial.shader = this.m_emptySkyShader;
				return;
			}
		}

		// Token: 0x040000C8 RID: 200
		[SerializeField]
		private Transform m_sunTransform;

		// Token: 0x040000C9 RID: 201
		[SerializeField]
		private Transform m_moonTransform;

		// Token: 0x040000CA RID: 202
		[SerializeField]
		private Transform m_starfieldTransform;

		// Token: 0x040000CB RID: 203
		[SerializeField]
		private Material m_skyMaterial;

		// Token: 0x040000CC RID: 204
		[SerializeField]
		private Material m_fogMaterial;

		// Token: 0x040000CD RID: 205
		[SerializeField]
		private Cubemap m_sunTexture;

		// Token: 0x040000CE RID: 206
		[SerializeField]
		private Cubemap m_moonTexture;

		// Token: 0x040000CF RID: 207
		[SerializeField]
		private Cubemap m_starfieldTexture;

		// Token: 0x040000D0 RID: 208
		[SerializeField]
		private Cubemap m_constellationTexture;

		// Token: 0x040000D1 RID: 209
		[SerializeField]
		private Texture2D m_dynamicCloudTexture;

		// Token: 0x040000D2 RID: 210
		[SerializeField]
		private Shader m_emptySkyShader;

		// Token: 0x040000D3 RID: 211
		[SerializeField]
		private Shader m_dynamicCloudShader;

		// Token: 0x040000D4 RID: 212
		[SerializeField]
		private Vector3 m_wavelength = new Vector3(680f, 550f, 450f);

		// Token: 0x040000D5 RID: 213
		[SerializeField]
		private float m_molecularDensity = 2.545f;

		// Token: 0x040000D6 RID: 214
		[SerializeField]
		private float m_kr = 8400f;

		// Token: 0x040000D7 RID: 215
		[SerializeField]
		private float m_km = 1200f;

		// Token: 0x040000D8 RID: 216
		[SerializeField]
		private float m_rayleigh = 1.5f;

		// Token: 0x040000D9 RID: 217
		[SerializeField]
		private float m_mie = 1f;

		// Token: 0x040000DA RID: 218
		[SerializeField]
		private float m_mieDirectionalityFactor = 0.75f;

		// Token: 0x040000DB RID: 219
		[SerializeField]
		private float m_scattering = 15f;

		// Token: 0x040000DC RID: 220
		[SerializeField]
		private float m_skyLuminance = 0.1f;

		// Token: 0x040000DD RID: 221
		[SerializeField]
		private float m_exposure = 2f;

		// Token: 0x040000DE RID: 222
		[SerializeField]
		private Color m_rayleighColor = Color.white;

		// Token: 0x040000DF RID: 223
		[SerializeField]
		private Color m_mieColor = Color.white;

		// Token: 0x040000E0 RID: 224
		[SerializeField]
		private float m_sunSize = 1f;

		// Token: 0x040000E1 RID: 225
		[SerializeField]
		private float m_sunOpacity = 1f;

		// Token: 0x040000E2 RID: 226
		[SerializeField]
		private Color m_sunColor = Color.white;

		// Token: 0x040000E3 RID: 227
		[SerializeField]
		private float m_moonSize = 1f;

		// Token: 0x040000E4 RID: 228
		[SerializeField]
		private float m_moonOpacity = 1f;

		// Token: 0x040000E5 RID: 229
		[SerializeField]
		private Color m_moonColor = Color.white;

		// Token: 0x040000E6 RID: 230
		[SerializeField]
		private Vector3 m_moonRotationOffset = Vector3.zero;

		// Token: 0x040000E7 RID: 231
		[SerializeField]
		private float m_starsIntensity = 0.5f;

		// Token: 0x040000E8 RID: 232
		[SerializeField]
		private float m_milkyWayIntensity;

		// Token: 0x040000E9 RID: 233
		[SerializeField]
		private Color m_starfieldColor = Color.white;

		// Token: 0x040000EA RID: 234
		[SerializeField]
		private float m_skyExtinction;

		// Token: 0x040000EB RID: 235
		[SerializeField]
		private float m_constellationIntensity;

		// Token: 0x040000EC RID: 236
		[SerializeField]
		private Color m_constellationColor = Color.white;

		// Token: 0x040000ED RID: 237
		[SerializeField]
		private Vector3 m_starfieldRotationOffset = Vector3.zero;

		// Token: 0x040000EE RID: 238
		private Quaternion m_moonRotation;

		// Token: 0x040000EF RID: 239
		private Matrix4x4 m_moonRotationMatrix;

		// Token: 0x040000F0 RID: 240
		private Quaternion m_starfieldRotation;

		// Token: 0x040000F1 RID: 241
		private Matrix4x4 m_starfieldRotationMatrix;

		// Token: 0x040000F2 RID: 242
		[SerializeField]
		private float m_mieDistance = 1f;

		// Token: 0x040000F3 RID: 243
		[SerializeField]
		private float m_globalFogDistance = 1000f;

		// Token: 0x040000F4 RID: 244
		[SerializeField]
		private float m_globalFogSmoothStep = 0.25f;

		// Token: 0x040000F5 RID: 245
		[SerializeField]
		private float m_globalFogDensity = 1f;

		// Token: 0x040000F6 RID: 246
		[SerializeField]
		private float m_heightFogDistance = 100f;

		// Token: 0x040000F7 RID: 247
		[SerializeField]
		private float m_heightFogSmoothStep = 1f;

		// Token: 0x040000F8 RID: 248
		[SerializeField]
		private float m_heightFogDensity;

		// Token: 0x040000F9 RID: 249
		[SerializeField]
		private float m_heightFogStartAltitude;

		// Token: 0x040000FA RID: 250
		[SerializeField]
		private float m_heightFogEndAltitude = 100f;

		// Token: 0x040000FB RID: 251
		[SerializeField]
		private float m_fogBluishIntensity = 0.15f;

		// Token: 0x040000FC RID: 252
		[SerializeField]
		private float m_heightFogScatteringMultiplier = 0.5f;

		// Token: 0x040000FD RID: 253
		[SerializeField]
		private float m_dynamicCloudAltitude = 7.5f;

		// Token: 0x040000FE RID: 254
		[SerializeField]
		private float m_dynamicCloudDirection;

		// Token: 0x040000FF RID: 255
		[SerializeField]
		private float m_dynamicCloudSpeed = 0.1f;

		// Token: 0x04000100 RID: 256
		[SerializeField]
		private float m_dynamicCloudDensity = 0.75f;

		// Token: 0x04000101 RID: 257
		[SerializeField]
		private Color m_dynamicCloudColor1 = Color.white;

		// Token: 0x04000102 RID: 258
		[SerializeField]
		private Color m_dynamicCloudColor2 = Color.white;

		// Token: 0x04000103 RID: 259
		private Vector2 m_dynamicCloudUV = Vector2.zero;

		// Token: 0x04000104 RID: 260
		[SerializeField]
		private AzureUpdateMode m_updateMode;

		// Token: 0x04000105 RID: 261
		[SerializeField]
		private float m_skyUpdateInterval = 0.1f;

		// Token: 0x04000106 RID: 262
		[SerializeField]
		private float m_cloudUpdateInterval = 0.033f;

		// Token: 0x04000107 RID: 263
		[SerializeField]
		private bool m_useScatteringCache = true;

		// Token: 0x04000108 RID: 264
		[SerializeField]
		private AzureCloudMode m_cloudMode = AzureCloudMode.Dynamic;

		// Token: 0x04000109 RID: 265
		private float m_lastSkyUpdateTime = float.MinValue;

		// Token: 0x0400010A RID: 266
		private float m_lastCloudUpdateTime = float.MinValue;

		// Token: 0x0400010B RID: 267
		private Vector3 m_cachedRayleigh;

		// Token: 0x0400010C RID: 268
		private Vector3 m_cachedMie;

		// Token: 0x0400010D RID: 269
		private Vector3 m_cachedMieG;

		// Token: 0x0400010E RID: 270
		private bool m_scatteringCacheDirty = true;

		// Token: 0x0400010F RID: 271
		private Vector3 m_lastCachedWavelength;

		// Token: 0x04000110 RID: 272
		private float m_lastCachedMolecularDensity;

		// Token: 0x04000111 RID: 273
		private float m_lastCachedMieDirectionalityFactor;
	}
}
