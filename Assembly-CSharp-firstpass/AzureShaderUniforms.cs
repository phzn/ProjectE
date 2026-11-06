using System;

namespace UnityEngine.AzureSky
{
	// Token: 0x0200001B RID: 27
	internal static class AzureShaderUniforms
	{
		// Token: 0x0400008C RID: 140
		internal static readonly int SunTexture = Shader.PropertyToID("_Azure_SunTexture");

		// Token: 0x0400008D RID: 141
		internal static readonly int MoonTexture = Shader.PropertyToID("_Azure_MoonTexture");

		// Token: 0x0400008E RID: 142
		internal static readonly int StarfieldTexture = Shader.PropertyToID("_Azure_StarfieldTexture");

		// Token: 0x0400008F RID: 143
		internal static readonly int ConstellationTexture = Shader.PropertyToID("_Azure_ConstellationTexture");

		// Token: 0x04000090 RID: 144
		internal static readonly int DynamicCloudTexture = Shader.PropertyToID("_Azure_DynamicCloudTexture");

		// Token: 0x04000091 RID: 145
		internal static readonly int FogScatteringDataTexture = Shader.PropertyToID("_Azure_FogScatteringDataTexture");

		// Token: 0x04000092 RID: 146
		internal static readonly int SunDirection = Shader.PropertyToID("_Azure_SunDirection");

		// Token: 0x04000093 RID: 147
		internal static readonly int MoonDirection = Shader.PropertyToID("_Azure_MoonDirection");

		// Token: 0x04000094 RID: 148
		internal static readonly int SunMatrix = Shader.PropertyToID("_Azure_SunMatrix");

		// Token: 0x04000095 RID: 149
		internal static readonly int MoonMatrix = Shader.PropertyToID("_Azure_MoonMatrix");

		// Token: 0x04000096 RID: 150
		internal static readonly int UpDirectionMatrix = Shader.PropertyToID("_Azure_UpDirectionMatrix");

		// Token: 0x04000097 RID: 151
		internal static readonly int StarfieldMatrix = Shader.PropertyToID("_Azure_StarfieldMatrix");

		// Token: 0x04000098 RID: 152
		internal static readonly int FrustumCornersMatrix = Shader.PropertyToID("_Azure_FrustumCornersMatrix");

		// Token: 0x04000099 RID: 153
		internal static readonly int Kr = Shader.PropertyToID("_Azure_Kr");

		// Token: 0x0400009A RID: 154
		internal static readonly int Km = Shader.PropertyToID("_Azure_Km");

		// Token: 0x0400009B RID: 155
		internal static readonly int Rayleigh = Shader.PropertyToID("_Azure_Rayleigh");

		// Token: 0x0400009C RID: 156
		internal static readonly int Mie = Shader.PropertyToID("_Azure_Mie");

		// Token: 0x0400009D RID: 157
		internal static readonly int MieG = Shader.PropertyToID("_Azure_MieG");

		// Token: 0x0400009E RID: 158
		internal static readonly int Scattering = Shader.PropertyToID("_Azure_Scattering");

		// Token: 0x0400009F RID: 159
		internal static readonly int SkyLuminance = Shader.PropertyToID("_Azure_SkyLuminance");

		// Token: 0x040000A0 RID: 160
		internal static readonly int Exposure = Shader.PropertyToID("_Azure_Exposure");

		// Token: 0x040000A1 RID: 161
		internal static readonly int RayleighColor = Shader.PropertyToID("_Azure_RayleighColor");

		// Token: 0x040000A2 RID: 162
		internal static readonly int MieColor = Shader.PropertyToID("_Azure_MieColor");

		// Token: 0x040000A3 RID: 163
		internal static readonly int SunRadius = Shader.PropertyToID("_Azure_SunRadius");

		// Token: 0x040000A4 RID: 164
		internal static readonly int SunPosition = Shader.PropertyToID("_Azure_SunPosition");

		// Token: 0x040000A5 RID: 165
		internal static readonly int SunOpacity = Shader.PropertyToID("_Azure_SunOpacity");

		// Token: 0x040000A6 RID: 166
		internal static readonly int SunColor = Shader.PropertyToID("_Azure_SunColor");

		// Token: 0x040000A7 RID: 167
		internal static readonly int MoonRadius = Shader.PropertyToID("_Azure_MoonRadius");

		// Token: 0x040000A8 RID: 168
		internal static readonly int MoonPosition = Shader.PropertyToID("_Azure_MoonPosition");

		// Token: 0x040000A9 RID: 169
		internal static readonly int MoonOpacity = Shader.PropertyToID("_Azure_MoonOpacity");

		// Token: 0x040000AA RID: 170
		internal static readonly int MoonColor = Shader.PropertyToID("_Azure_MoonColor");

		// Token: 0x040000AB RID: 171
		internal static readonly int StarsIntensity = Shader.PropertyToID("_Azure_StarsIntensity");

		// Token: 0x040000AC RID: 172
		internal static readonly int MilkyWayIntensity = Shader.PropertyToID("_Azure_MilkyWayIntensity");

		// Token: 0x040000AD RID: 173
		internal static readonly int StarFieldColor = Shader.PropertyToID("_Azure_StarFieldColor");

		// Token: 0x040000AE RID: 174
		internal static readonly int ConstellationIntensity = Shader.PropertyToID("_Azure_ConstellationIntensity");

		// Token: 0x040000AF RID: 175
		internal static readonly int ConstellationColor = Shader.PropertyToID("_Azure_ConstellationColor");

		// Token: 0x040000B0 RID: 176
		internal static readonly int SkyExtinction = Shader.PropertyToID("_Azure_SkyExtinction");

		// Token: 0x040000B1 RID: 177
		internal static readonly int MieDistance = Shader.PropertyToID("_Azure_MieDistance");

		// Token: 0x040000B2 RID: 178
		internal static readonly int GlobalFogDistance = Shader.PropertyToID("_Azure_GlobalFogDistance");

		// Token: 0x040000B3 RID: 179
		internal static readonly int GlobalFogSmoothStep = Shader.PropertyToID("_Azure_GlobalFogSmooth");

		// Token: 0x040000B4 RID: 180
		internal static readonly int GlobalFogDensity = Shader.PropertyToID("_Azure_GlobalFogDensity");

		// Token: 0x040000B5 RID: 181
		internal static readonly int HeightFogDistance = Shader.PropertyToID("_Azure_HeightFogDistance");

		// Token: 0x040000B6 RID: 182
		internal static readonly int HeightFogSmoothStep = Shader.PropertyToID("_Azure_HeightFogSmooth");

		// Token: 0x040000B7 RID: 183
		internal static readonly int HeightFogDensity = Shader.PropertyToID("_Azure_HeightFogDensity");

		// Token: 0x040000B8 RID: 184
		internal static readonly int HeightFogStartAltitude = Shader.PropertyToID("_Azure_HeightFogStartAltitude");

		// Token: 0x040000B9 RID: 185
		internal static readonly int HeightFogEndAltitude = Shader.PropertyToID("_Azure_HeightFogEndAltitude");

		// Token: 0x040000BA RID: 186
		internal static readonly int FogBluishIntensity = Shader.PropertyToID("_Azure_FogBluishIntensity");

		// Token: 0x040000BB RID: 187
		internal static readonly int HeightFogScatteringMultiplier = Shader.PropertyToID("_Azure_HeightFogScatterMultiplier");

		// Token: 0x040000BC RID: 188
		internal static readonly int DynamicCloudAltitude = Shader.PropertyToID("_Azure_DynamicCloudAltitude");

		// Token: 0x040000BD RID: 189
		internal static readonly int DynamicCloudDirection = Shader.PropertyToID("_Azure_DynamicCloudDirection");

		// Token: 0x040000BE RID: 190
		internal static readonly int DynamicCloudDensity = Shader.PropertyToID("_Azure_DynamicCloudDensity");

		// Token: 0x040000BF RID: 191
		internal static readonly int DynamicCloudColor1 = Shader.PropertyToID("_Azure_DynamicCloudColor1");

		// Token: 0x040000C0 RID: 192
		internal static readonly int DynamicCloudColor2 = Shader.PropertyToID("_Azure_DynamicCloudColor2");

		// Token: 0x040000C1 RID: 193
		internal static readonly int ThunderLightningEffect = Shader.PropertyToID("_Azure_ThunderLightningEffect");

		// Token: 0x040000C2 RID: 194
		internal static readonly int VolumetricLightIntensity = Shader.PropertyToID("_Azure_VolumetricLightIntensity");

		// Token: 0x040000C3 RID: 195
		internal static readonly int VolumetricLightColor = Shader.PropertyToID("_Azure_VolumetricLightColor");

		// Token: 0x040000C4 RID: 196
		internal static readonly int VolumetricLightExtinctionDistance = Shader.PropertyToID("_Azure_VolumetricLightExtinctionDistance");

		// Token: 0x040000C5 RID: 197
		internal static readonly int VolumetricLightExtinctionSmoothStep = Shader.PropertyToID("_Azure_VolumetricLightExtinctionSmoothStep");

		// Token: 0x040000C6 RID: 198
		internal static readonly int VolumetricLightMieG = Shader.PropertyToID("_Azure_VolumetricLightMieG");

		// Token: 0x040000C7 RID: 199
		internal static readonly int VolumetricLightWorldPosition = Shader.PropertyToID("_Azure_VolumetricLightWorldPosition");
	}
}
