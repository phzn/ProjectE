using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Unity.Mathematics;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000086 RID: 134
	internal class LightCookieManager : IDisposable
	{
		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x0001DE2C File Offset: 0x0001C02C
		// (set) Token: 0x06000520 RID: 1312 RVA: 0x0001DE34 File Offset: 0x0001C034
		internal bool IsKeywordLightCookieEnabled { get; private set; }

		// Token: 0x06000521 RID: 1313 RVA: 0x0001DE3D File Offset: 0x0001C03D
		public LightCookieManager(ref LightCookieManager.Settings settings)
		{
			this.m_Settings = settings;
			this.m_WorkMem = new LightCookieManager.WorkMemory();
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0001DE74 File Offset: 0x0001C074
		private void InitAdditionalLights(int size)
		{
			Vector2Int vector2Int;
			if (this.m_Settings.atlas.useMips)
			{
				LightCookieManager.Settings.AtlasSettings atlas = this.m_Settings.atlas;
				if (atlas.isPow2)
				{
					vector2Int = this.m_Settings.atlas.resolution;
					this.m_AdditionalLightsCookieAtlas = new PowerOfTwoTextureAtlas(vector2Int.x, 4, this.m_Settings.atlas.format, FilterMode.Bilinear, "Universal Light Cookie Pow2 Atlas", true);
					goto IL_00B7;
				}
			}
			vector2Int = this.m_Settings.atlas.resolution;
			int x = vector2Int.x;
			vector2Int = this.m_Settings.atlas.resolution;
			this.m_AdditionalLightsCookieAtlas = new Texture2DAtlas(x, vector2Int.y, this.m_Settings.atlas.format, FilterMode.Bilinear, false, "Universal Light Cookie Atlas", false);
			IL_00B7:
			this.m_AdditionalLightsCookieShaderData = new LightCookieManager.LightCookieShaderData(size, this.m_Settings.useStructuredBuffer);
			this.m_VisibleLightIndexToShaderDataIndex = new int[this.m_Settings.maxAdditionalLights + 1];
			this.m_CookieSizeDivisor = 1;
			this.m_PrevCookieRequestPixelCount = uint.MaxValue;
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0001DF75 File Offset: 0x0001C175
		public bool isInitialized()
		{
			return this.m_AdditionalLightsCookieAtlas != null && this.m_AdditionalLightsCookieShaderData != null;
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0001DF8A File Offset: 0x0001C18A
		public void Dispose()
		{
			Texture2DAtlas additionalLightsCookieAtlas = this.m_AdditionalLightsCookieAtlas;
			if (additionalLightsCookieAtlas != null)
			{
				additionalLightsCookieAtlas.Release();
			}
			LightCookieManager.LightCookieShaderData additionalLightsCookieShaderData = this.m_AdditionalLightsCookieShaderData;
			if (additionalLightsCookieShaderData == null)
			{
				return;
			}
			additionalLightsCookieShaderData.Dispose();
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0001DFAD File Offset: 0x0001C1AD
		public int GetLightCookieShaderDataIndex(int visibleLightIndex)
		{
			if (!this.isInitialized())
			{
				return -1;
			}
			return this.m_VisibleLightIndexToShaderDataIndex[visibleLightIndex];
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0001DFC4 File Offset: 0x0001C1C4
		public void Setup(ScriptableRenderContext ctx, CommandBuffer cmd, ref LightData lightData)
		{
			using (new ProfilingScope(cmd, ProfilingSampler.Get<URPProfileId>(URPProfileId.LightCookies)))
			{
				bool flag = lightData.mainLightIndex >= 0;
				if (flag)
				{
					VisibleLight visibleLight = lightData.visibleLights[lightData.mainLightIndex];
					flag = this.SetupMainLight(cmd, ref visibleLight);
				}
				bool flag2 = lightData.additionalLightsCount > 0;
				if (flag2)
				{
					flag2 = this.SetupAdditionalLights(cmd, ref lightData);
				}
				if (!flag2)
				{
					if (this.m_VisibleLightIndexToShaderDataIndex != null && this.m_AdditionalLightsCookieShaderData.isUploaded)
					{
						int num = this.m_VisibleLightIndexToShaderDataIndex.Length;
						for (int i = 0; i < num; i++)
						{
							this.m_VisibleLightIndexToShaderDataIndex[i] = -1;
						}
					}
					LightCookieManager.LightCookieShaderData additionalLightsCookieShaderData = this.m_AdditionalLightsCookieShaderData;
					if (additionalLightsCookieShaderData != null)
					{
						additionalLightsCookieShaderData.Clear(cmd);
					}
				}
				this.IsKeywordLightCookieEnabled = flag || flag2;
				CoreUtils.SetKeyword(cmd, "_LIGHT_COOKIES", this.IsKeywordLightCookieEnabled);
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0001E0AC File Offset: 0x0001C2AC
		private bool SetupMainLight(CommandBuffer cmd, ref VisibleLight visibleMainLight)
		{
			Light light = visibleMainLight.light;
			Texture cookie = light.cookie;
			bool flag = cookie != null;
			if (flag)
			{
				Matrix4x4 identity = Matrix4x4.identity;
				float num = (float)this.GetLightCookieShaderFormat(cookie.graphicsFormat);
				UniversalAdditionalLightData universalAdditionalLightData;
				if (light.TryGetComponent<UniversalAdditionalLightData>(out universalAdditionalLightData))
				{
					this.GetLightUVScaleOffset(ref universalAdditionalLightData, ref identity);
				}
				Matrix4x4 matrix4x = LightCookieManager.s_DirLightProj * identity * visibleMainLight.localToWorldMatrix.inverse;
				cmd.SetGlobalTexture(LightCookieManager.ShaderProperty.mainLightTexture, cookie);
				cmd.SetGlobalMatrix(LightCookieManager.ShaderProperty.mainLightWorldToLight, matrix4x);
				cmd.SetGlobalFloat(LightCookieManager.ShaderProperty.mainLightCookieTextureFormat, num);
				return flag;
			}
			cmd.SetGlobalTexture(LightCookieManager.ShaderProperty.mainLightTexture, Texture2D.whiteTexture);
			cmd.SetGlobalMatrix(LightCookieManager.ShaderProperty.mainLightWorldToLight, Matrix4x4.identity);
			cmd.SetGlobalFloat(LightCookieManager.ShaderProperty.mainLightCookieTextureFormat, -1f);
			return flag;
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0001E17C File Offset: 0x0001C37C
		private LightCookieManager.LightCookieShaderFormat GetLightCookieShaderFormat(GraphicsFormat cookieFormat)
		{
			if (cookieFormat <= GraphicsFormat.R16_UInt)
			{
				if (cookieFormat <= GraphicsFormat.R8_UInt)
				{
					if (cookieFormat <= GraphicsFormat.R8_UNorm)
					{
						if (cookieFormat == GraphicsFormat.R8_SRGB || cookieFormat == GraphicsFormat.R8_UNorm)
						{
							return LightCookieManager.LightCookieShaderFormat.Red;
						}
					}
					else if (cookieFormat == GraphicsFormat.R8_SNorm || cookieFormat == GraphicsFormat.R8_UInt)
					{
						return LightCookieManager.LightCookieShaderFormat.Red;
					}
				}
				else if (cookieFormat <= GraphicsFormat.R16_UNorm)
				{
					if (cookieFormat == GraphicsFormat.R8_SInt || cookieFormat == GraphicsFormat.R16_UNorm)
					{
						return LightCookieManager.LightCookieShaderFormat.Red;
					}
				}
				else if (cookieFormat == GraphicsFormat.R16_SNorm || cookieFormat == GraphicsFormat.R16_UInt)
				{
					return LightCookieManager.LightCookieShaderFormat.Red;
				}
			}
			else if (cookieFormat <= GraphicsFormat.R16_SFloat)
			{
				if (cookieFormat <= GraphicsFormat.R32_UInt)
				{
					if (cookieFormat == GraphicsFormat.R16_SInt || cookieFormat == GraphicsFormat.R32_UInt)
					{
						return LightCookieManager.LightCookieShaderFormat.Red;
					}
				}
				else if (cookieFormat == GraphicsFormat.R32_SInt || cookieFormat == GraphicsFormat.R16_SFloat)
				{
					return LightCookieManager.LightCookieShaderFormat.Red;
				}
			}
			else if (cookieFormat <= (GraphicsFormat)55)
			{
				if (cookieFormat == GraphicsFormat.R32_SFloat)
				{
					return LightCookieManager.LightCookieShaderFormat.Red;
				}
				if (cookieFormat - (GraphicsFormat)54 <= 1)
				{
					return LightCookieManager.LightCookieShaderFormat.Alpha;
				}
			}
			else if (cookieFormat - GraphicsFormat.R_BC4_UNorm <= 1 || cookieFormat - GraphicsFormat.R_EAC_UNorm <= 1)
			{
				return LightCookieManager.LightCookieShaderFormat.Red;
			}
			return LightCookieManager.LightCookieShaderFormat.RGB;
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0001E214 File Offset: 0x0001C414
		private void GetLightUVScaleOffset(ref UniversalAdditionalLightData additionalLightData, ref Matrix4x4 uvTransform)
		{
			Vector2 vector = Vector2.one / additionalLightData.lightCookieSize;
			Vector2 lightCookieOffset = additionalLightData.lightCookieOffset;
			if (Mathf.Abs(vector.x) < half.MinValue)
			{
				vector.x = Mathf.Sign(vector.x) * half.MinValue;
			}
			if (Mathf.Abs(vector.y) < half.MinValue)
			{
				vector.y = Mathf.Sign(vector.y) * half.MinValue;
			}
			uvTransform = Matrix4x4.Scale(new Vector3(vector.x, vector.y, 1f));
			uvTransform.SetColumn(3, new Vector4(-lightCookieOffset.x * vector.x, -lightCookieOffset.y * vector.y, 0f, 1f));
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001E2E4 File Offset: 0x0001C4E4
		private bool SetupAdditionalLights(CommandBuffer cmd, ref LightData lightData)
		{
			int num = Math.Min(this.m_Settings.maxAdditionalLights, lightData.visibleLights.Length);
			this.m_WorkMem.Resize(num);
			int num2 = this.FilterAndValidateAdditionalLights(ref lightData, this.m_WorkMem.lightMappings);
			if (num2 <= 0)
			{
				return false;
			}
			if (!this.isInitialized())
			{
				this.InitAdditionalLights(num2);
			}
			LightCookieManager.WorkSlice<LightCookieManager.LightCookieMapping> workSlice = new LightCookieManager.WorkSlice<LightCookieManager.LightCookieMapping>(this.m_WorkMem.lightMappings, num2);
			int num3 = this.UpdateAdditionalLightsAtlas(cmd, ref workSlice, this.m_WorkMem.uvRects);
			LightCookieManager.WorkSlice<Vector4> workSlice2 = new LightCookieManager.WorkSlice<Vector4>(this.m_WorkMem.uvRects, num3);
			this.UploadAdditionalLights(cmd, ref lightData, ref workSlice, ref workSlice2);
			return workSlice2.length > 0;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001E394 File Offset: 0x0001C594
		private int FilterAndValidateAdditionalLights(ref LightData lightData, LightCookieManager.LightCookieMapping[] validLightMappings)
		{
			int mainLightIndex = lightData.mainLightIndex;
			int num = 0;
			int num2 = 0;
			int length = lightData.visibleLights.Length;
			for (int i = 0; i < length; i++)
			{
				if (i == mainLightIndex)
				{
					num--;
				}
				else
				{
					ref VisibleLight ptr = ref lightData.visibleLights.UnsafeElementAtMutable<VisibleLight>(i);
					Light light = ptr.light;
					if (!(light.cookie == null))
					{
						LightType lightType = ptr.lightType;
						if (lightType != LightType.Spot && lightType != LightType.Point)
						{
							Debug.LogWarning(string.Concat(new string[]
							{
								"Additional ",
								lightType.ToString(),
								" light called '",
								light.name,
								"' has a light cookie which will not be visible."
							}), light);
						}
						else
						{
							LightCookieManager.LightCookieMapping lightCookieMapping;
							lightCookieMapping.visibleLightIndex = (ushort)i;
							lightCookieMapping.lightBufferIndex = (ushort)(i + num);
							lightCookieMapping.light = light;
							if ((int)lightCookieMapping.lightBufferIndex >= validLightMappings.Length || num2 + 1 >= validLightMappings.Length)
							{
								if (length > this.m_Settings.maxAdditionalLights && Time.frameCount - this.m_PrevWarnFrame > 108000)
								{
									this.m_PrevWarnFrame = Time.frameCount;
									Debug.LogWarning(string.Concat(new string[]
									{
										"Max light cookies (",
										validLightMappings.Length.ToString(),
										") reached. Some visible lights (",
										(length - i - 1).ToString(),
										") might skip light cookie rendering."
									}));
									break;
								}
								break;
							}
							else
							{
								validLightMappings[num2++] = lightCookieMapping;
							}
						}
					}
				}
			}
			return num2;
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0001E51C File Offset: 0x0001C71C
		private int UpdateAdditionalLightsAtlas(CommandBuffer cmd, ref LightCookieManager.WorkSlice<LightCookieManager.LightCookieMapping> validLightMappings, Vector4[] textureAtlasUVRects)
		{
			validLightMappings.Sort(LightCookieManager.LightCookieMapping.s_CompareByCookieSize);
			uint num = this.ComputeCookieRequestPixelCount(ref validLightMappings);
			Vector2Int referenceSize = this.m_AdditionalLightsCookieAtlas.AtlasTexture.referenceSize;
			float num2 = num / (float)(referenceSize.x * referenceSize.y);
			int num3 = this.ApproximateCookieSizeDivisor(num2);
			if (num3 < this.m_CookieSizeDivisor && num < this.m_PrevCookieRequestPixelCount)
			{
				this.m_AdditionalLightsCookieAtlas.ResetAllocator();
				this.m_CookieSizeDivisor = num3;
			}
			int i = 0;
			while (i <= 0)
			{
				i = this.FetchUVRects(cmd, ref validLightMappings, textureAtlasUVRects, this.m_CookieSizeDivisor);
				if (i <= 0)
				{
					this.m_AdditionalLightsCookieAtlas.ResetAllocator();
					this.m_CookieSizeDivisor = Mathf.Max(this.m_CookieSizeDivisor + 1, num3);
					this.m_PrevCookieRequestPixelCount = num;
				}
			}
			return i;
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0001E5D8 File Offset: 0x0001C7D8
		private int FetchUVRects(CommandBuffer cmd, ref LightCookieManager.WorkSlice<LightCookieManager.LightCookieMapping> validLightMappings, Vector4[] textureAtlasUVRects, int cookieSizeDivisor)
		{
			int num = 0;
			int i = 0;
			while (i < validLightMappings.length)
			{
				Texture cookie = validLightMappings[i].light.cookie;
				Vector4 vector = Vector4.zero;
				if (cookie.dimension == TextureDimension.Cube)
				{
					vector = this.FetchCube(cmd, cookie, cookieSizeDivisor);
				}
				else
				{
					vector = this.Fetch2D(cmd, cookie, cookieSizeDivisor);
				}
				if (!(vector != Vector4.zero))
				{
					if (cookieSizeDivisor > 16)
					{
						Debug.LogWarning("Light cookies atlas is extremely full! Some of the light cookies were discarded. Increase light cookie atlas space or reduce the amount of unique light cookies.");
						return num;
					}
					return 0;
				}
				else
				{
					if (!SystemInfo.graphicsUVStartsAtTop)
					{
						vector.w = 1f - vector.w - vector.y;
					}
					textureAtlasUVRects[num++] = vector;
					i++;
				}
			}
			return num;
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0001E688 File Offset: 0x0001C888
		private uint ComputeCookieRequestPixelCount(ref LightCookieManager.WorkSlice<LightCookieManager.LightCookieMapping> validLightMappings)
		{
			uint num = 0U;
			int num2 = 0;
			for (int i = 0; i < validLightMappings.length; i++)
			{
				Texture cookie = validLightMappings[i].light.cookie;
				int instanceID = cookie.GetInstanceID();
				if (instanceID != num2)
				{
					num2 = instanceID;
					int num3 = cookie.width * cookie.height;
					num += (uint)num3;
				}
			}
			return num;
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0001E6E1 File Offset: 0x0001C8E1
		private int ApproximateCookieSizeDivisor(float requestAtlasRatio)
		{
			return (int)Mathf.Max(Mathf.Ceil(Mathf.Sqrt(requestAtlasRatio)), 1f);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0001E6FC File Offset: 0x0001C8FC
		private Vector4 Fetch2D(CommandBuffer cmd, Texture cookie, int cookieSizeDivisor = 1)
		{
			Vector4 zero = Vector4.zero;
			int num = Mathf.Max(cookie.width / cookieSizeDivisor, 4);
			int num2 = Mathf.Max(cookie.height / cookieSizeDivisor, 4);
			Vector2 vector = new Vector2((float)num, (float)num2);
			if (this.m_AdditionalLightsCookieAtlas.IsCached(out zero, cookie))
			{
				this.m_AdditionalLightsCookieAtlas.UpdateTexture(cmd, cookie, ref zero, true, true);
			}
			else
			{
				this.m_AdditionalLightsCookieAtlas.AllocateTexture(cmd, ref zero, cookie, num, num2, -1);
			}
			this.AdjustUVRect(ref zero, cookie, ref vector);
			return zero;
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0001E77C File Offset: 0x0001C97C
		private Vector4 FetchCube(CommandBuffer cmd, Texture cookie, int cookieSizeDivisor = 1)
		{
			Vector4 zero = Vector4.zero;
			int num = Mathf.Max(this.ComputeOctahedralCookieSize(cookie) / cookieSizeDivisor, 4);
			if (this.m_AdditionalLightsCookieAtlas.IsCached(out zero, cookie))
			{
				this.m_AdditionalLightsCookieAtlas.UpdateTexture(cmd, cookie, ref zero, true, true);
			}
			else
			{
				this.m_AdditionalLightsCookieAtlas.AllocateTexture(cmd, ref zero, cookie, num, num, -1);
			}
			Vector2 vector = Vector2.one * (float)num;
			this.AdjustUVRect(ref zero, cookie, ref vector);
			return zero;
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0001E7F0 File Offset: 0x0001C9F0
		private int ComputeOctahedralCookieSize(Texture cookie)
		{
			int num = Math.Max(cookie.width, cookie.height);
			LightCookieManager.Settings.AtlasSettings atlas = this.m_Settings.atlas;
			if (atlas.isPow2)
			{
				num *= Mathf.NextPowerOfTwo((int)this.m_Settings.cubeOctahedralSizeScale);
			}
			else
			{
				num = (int)((float)num * this.m_Settings.cubeOctahedralSizeScale + 0.5f);
			}
			return num;
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0001E854 File Offset: 0x0001CA54
		private void AdjustUVRect(ref Vector4 uvScaleOffset, Texture cookie, ref Vector2 cookieSize)
		{
			if (uvScaleOffset != Vector4.zero)
			{
				if (this.m_Settings.atlas.useMips)
				{
					PowerOfTwoTextureAtlas powerOfTwoTextureAtlas = this.m_AdditionalLightsCookieAtlas as PowerOfTwoTextureAtlas;
					int num = ((powerOfTwoTextureAtlas == null) ? 1 : powerOfTwoTextureAtlas.mipPadding);
					Vector2 vector = Vector2.one * (float)((int)Mathf.Pow(2f, (float)num)) * 2f;
					uvScaleOffset = PowerOfTwoTextureAtlas.GetPayloadScaleOffset(in cookieSize, in vector, in uvScaleOffset);
					return;
				}
				this.ShrinkUVRect(ref uvScaleOffset, 0.5f, ref cookieSize);
			}
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0001E8E0 File Offset: 0x0001CAE0
		private void ShrinkUVRect(ref Vector4 uvScaleOffset, float amountPixels, ref Vector2 cookieSize)
		{
			Vector2 vector = Vector2.one * amountPixels / cookieSize;
			Vector2 vector2 = (cookieSize - Vector2.one * (amountPixels * 2f)) / cookieSize;
			uvScaleOffset.z += uvScaleOffset.x * vector.x;
			uvScaleOffset.w += uvScaleOffset.y * vector.y;
			uvScaleOffset.x *= vector2.x;
			uvScaleOffset.y *= vector2.y;
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0001E97C File Offset: 0x0001CB7C
		private void UploadAdditionalLights(CommandBuffer cmd, ref LightData lightData, ref LightCookieManager.WorkSlice<LightCookieManager.LightCookieMapping> validLightMappings, ref LightCookieManager.WorkSlice<Vector4> validUvRects)
		{
			cmd.SetGlobalTexture(LightCookieManager.ShaderProperty.additionalLightsCookieAtlasTexture, this.m_AdditionalLightsCookieAtlas.AtlasTexture);
			cmd.SetGlobalFloat(LightCookieManager.ShaderProperty.additionalLightsCookieAtlasTextureFormat, (float)this.GetLightCookieShaderFormat(this.m_AdditionalLightsCookieAtlas.AtlasTexture.rt.graphicsFormat));
			if (this.m_VisibleLightIndexToShaderDataIndex.Length < lightData.visibleLights.Length)
			{
				this.m_VisibleLightIndexToShaderDataIndex = new int[lightData.visibleLights.Length];
			}
			int num = Math.Min(this.m_VisibleLightIndexToShaderDataIndex.Length, lightData.visibleLights.Length);
			for (int i = 0; i < num; i++)
			{
				this.m_VisibleLightIndexToShaderDataIndex[i] = -1;
			}
			this.m_AdditionalLightsCookieShaderData.Resize(this.m_Settings.maxAdditionalLights);
			Matrix4x4[] worldToLights = this.m_AdditionalLightsCookieShaderData.worldToLights;
			LightCookieManager.ShaderBitArray cookieEnableBits = this.m_AdditionalLightsCookieShaderData.cookieEnableBits;
			Vector4[] atlasUVRects = this.m_AdditionalLightsCookieShaderData.atlasUVRects;
			float[] lightTypes = this.m_AdditionalLightsCookieShaderData.lightTypes;
			Array.Clear(atlasUVRects, 0, atlasUVRects.Length);
			cookieEnableBits.Clear();
			for (int j = 0; j < validUvRects.length; j++)
			{
				int visibleLightIndex = (int)validLightMappings[j].visibleLightIndex;
				int lightBufferIndex = (int)validLightMappings[j].lightBufferIndex;
				this.m_VisibleLightIndexToShaderDataIndex[visibleLightIndex] = lightBufferIndex;
				ref VisibleLight ptr = ref lightData.visibleLights.UnsafeElementAtMutable<VisibleLight>(visibleLightIndex);
				lightTypes[lightBufferIndex] = (float)ptr.lightType;
				worldToLights[lightBufferIndex] = ptr.localToWorldMatrix.inverse;
				atlasUVRects[lightBufferIndex] = validUvRects[j];
				cookieEnableBits[lightBufferIndex] = true;
				if (ptr.lightType == LightType.Spot)
				{
					float spotAngle = ptr.spotAngle;
					float range = ptr.range;
					Matrix4x4 matrix4x = Matrix4x4.Perspective(spotAngle, 1f, 0.001f, range);
					matrix4x.SetColumn(2, matrix4x.GetColumn(2) * -1f);
					worldToLights[lightBufferIndex] = matrix4x * worldToLights[lightBufferIndex];
				}
			}
			this.m_AdditionalLightsCookieShaderData.Upload(cmd);
		}

		// Token: 0x040003B6 RID: 950
		private static readonly Matrix4x4 s_DirLightProj = Matrix4x4.Ortho(-0.5f, 0.5f, -0.5f, 0.5f, -0.5f, 0.5f);

		// Token: 0x040003B7 RID: 951
		private Texture2DAtlas m_AdditionalLightsCookieAtlas;

		// Token: 0x040003B8 RID: 952
		private LightCookieManager.LightCookieShaderData m_AdditionalLightsCookieShaderData;

		// Token: 0x040003B9 RID: 953
		private readonly LightCookieManager.Settings m_Settings;

		// Token: 0x040003BA RID: 954
		private LightCookieManager.WorkMemory m_WorkMem;

		// Token: 0x040003BB RID: 955
		private int[] m_VisibleLightIndexToShaderDataIndex;

		// Token: 0x040003BC RID: 956
		private const int k_MaxCookieSizeDivisor = 16;

		// Token: 0x040003BD RID: 957
		private int m_CookieSizeDivisor = 1;

		// Token: 0x040003BE RID: 958
		private uint m_PrevCookieRequestPixelCount = uint.MaxValue;

		// Token: 0x040003BF RID: 959
		private int m_PrevWarnFrame = -1;

		// Token: 0x0200018F RID: 399
		private static class ShaderProperty
		{
			// Token: 0x04000A7C RID: 2684
			public static readonly int mainLightTexture = Shader.PropertyToID("_MainLightCookieTexture");

			// Token: 0x04000A7D RID: 2685
			public static readonly int mainLightWorldToLight = Shader.PropertyToID("_MainLightWorldToLight");

			// Token: 0x04000A7E RID: 2686
			public static readonly int mainLightCookieTextureFormat = Shader.PropertyToID("_MainLightCookieTextureFormat");

			// Token: 0x04000A7F RID: 2687
			public static readonly int additionalLightsCookieAtlasTexture = Shader.PropertyToID("_AdditionalLightsCookieAtlasTexture");

			// Token: 0x04000A80 RID: 2688
			public static readonly int additionalLightsCookieAtlasTextureFormat = Shader.PropertyToID("_AdditionalLightsCookieAtlasTextureFormat");

			// Token: 0x04000A81 RID: 2689
			public static readonly int additionalLightsCookieEnableBits = Shader.PropertyToID("_AdditionalLightsCookieEnableBits");

			// Token: 0x04000A82 RID: 2690
			public static readonly int additionalLightsCookieAtlasUVRectBuffer = Shader.PropertyToID("_AdditionalLightsCookieAtlasUVRectBuffer");

			// Token: 0x04000A83 RID: 2691
			public static readonly int additionalLightsCookieAtlasUVRects = Shader.PropertyToID("_AdditionalLightsCookieAtlasUVRects");

			// Token: 0x04000A84 RID: 2692
			public static readonly int additionalLightsWorldToLightBuffer = Shader.PropertyToID("_AdditionalLightsWorldToLightBuffer");

			// Token: 0x04000A85 RID: 2693
			public static readonly int additionalLightsLightTypeBuffer = Shader.PropertyToID("_AdditionalLightsLightTypeBuffer");

			// Token: 0x04000A86 RID: 2694
			public static readonly int additionalLightsWorldToLights = Shader.PropertyToID("_AdditionalLightsWorldToLights");

			// Token: 0x04000A87 RID: 2695
			public static readonly int additionalLightsLightTypes = Shader.PropertyToID("_AdditionalLightsLightTypes");
		}

		// Token: 0x02000190 RID: 400
		private enum LightCookieShaderFormat
		{
			// Token: 0x04000A89 RID: 2697
			None = -1,
			// Token: 0x04000A8A RID: 2698
			RGB,
			// Token: 0x04000A8B RID: 2699
			Alpha,
			// Token: 0x04000A8C RID: 2700
			Red
		}

		// Token: 0x02000191 RID: 401
		public struct Settings
		{
			// Token: 0x06000AC4 RID: 2756 RVA: 0x00049684 File Offset: 0x00047884
			public static LightCookieManager.Settings Create()
			{
				LightCookieManager.Settings settings;
				settings.atlas.resolution = new Vector2Int(1024, 1024);
				settings.atlas.format = GraphicsFormat.R8G8B8A8_SRGB;
				settings.atlas.useMips = false;
				settings.maxAdditionalLights = UniversalRenderPipeline.maxVisibleAdditionalLights;
				settings.cubeOctahedralSizeScale = ((settings.atlas.useMips && settings.atlas.isPow2) ? 2f : 2.5f);
				settings.useStructuredBuffer = RenderingUtils.useStructuredBuffer;
				return settings;
			}

			// Token: 0x04000A8D RID: 2701
			public LightCookieManager.Settings.AtlasSettings atlas;

			// Token: 0x04000A8E RID: 2702
			public int maxAdditionalLights;

			// Token: 0x04000A8F RID: 2703
			public float cubeOctahedralSizeScale;

			// Token: 0x04000A90 RID: 2704
			public bool useStructuredBuffer;

			// Token: 0x0200022E RID: 558
			public struct AtlasSettings
			{
				// Token: 0x1700026E RID: 622
				// (get) Token: 0x06000C4C RID: 3148 RVA: 0x0004D510 File Offset: 0x0004B710
				public bool isPow2
				{
					get
					{
						return Mathf.IsPowerOfTwo(this.resolution.x) && Mathf.IsPowerOfTwo(this.resolution.y);
					}
				}

				// Token: 0x1700026F RID: 623
				// (get) Token: 0x06000C4D RID: 3149 RVA: 0x0004D536 File Offset: 0x0004B736
				public bool isSquare
				{
					get
					{
						return this.resolution.x == this.resolution.y;
					}
				}

				// Token: 0x04000D35 RID: 3381
				public Vector2Int resolution;

				// Token: 0x04000D36 RID: 3382
				public GraphicsFormat format;

				// Token: 0x04000D37 RID: 3383
				public bool useMips;
			}
		}

		// Token: 0x02000192 RID: 402
		private struct Sorting
		{
			// Token: 0x06000AC5 RID: 2757 RVA: 0x0004970D File Offset: 0x0004790D
			public static void QuickSort<T>(T[] data, Func<T, T, int> compare)
			{
				LightCookieManager.Sorting.QuickSort<T>(data, 0, data.Length - 1, compare);
			}

			// Token: 0x06000AC6 RID: 2758 RVA: 0x0004971C File Offset: 0x0004791C
			public static void QuickSort<T>(T[] data, int start, int end, Func<T, T, int> compare)
			{
				int num = end - start;
				if (num < 1)
				{
					return;
				}
				if (num < 8)
				{
					LightCookieManager.Sorting.InsertionSort<T>(data, start, end, compare);
					return;
				}
				if (start < end)
				{
					int num2 = LightCookieManager.Sorting.Partition<T>(data, start, end, compare);
					if (num2 >= 1)
					{
						LightCookieManager.Sorting.QuickSort<T>(data, start, num2, compare);
					}
					if (num2 + 1 < end)
					{
						LightCookieManager.Sorting.QuickSort<T>(data, num2 + 1, end, compare);
					}
				}
			}

			// Token: 0x06000AC7 RID: 2759 RVA: 0x0004976C File Offset: 0x0004796C
			private static T Median3Pivot<T>(T[] data, int start, int pivot, int end, Func<T, T, int> compare)
			{
				LightCookieManager.Sorting.<>c__DisplayClass2_0<T> CS$<>8__locals1;
				CS$<>8__locals1.data = data;
				if (compare(CS$<>8__locals1.data[end], CS$<>8__locals1.data[start]) < 0)
				{
					LightCookieManager.Sorting.<Median3Pivot>g__Swap|2_0<T>(start, end, ref CS$<>8__locals1);
				}
				if (compare(CS$<>8__locals1.data[pivot], CS$<>8__locals1.data[start]) < 0)
				{
					LightCookieManager.Sorting.<Median3Pivot>g__Swap|2_0<T>(start, pivot, ref CS$<>8__locals1);
				}
				if (compare(CS$<>8__locals1.data[end], CS$<>8__locals1.data[pivot]) < 0)
				{
					LightCookieManager.Sorting.<Median3Pivot>g__Swap|2_0<T>(pivot, end, ref CS$<>8__locals1);
				}
				return CS$<>8__locals1.data[pivot];
			}

			// Token: 0x06000AC8 RID: 2760 RVA: 0x00049810 File Offset: 0x00047A10
			private static int Partition<T>(T[] data, int start, int end, Func<T, T, int> compare)
			{
				int num = end - start;
				int num2 = start + num / 2;
				T t = LightCookieManager.Sorting.Median3Pivot<T>(data, start, num2, end, compare);
				for (;;)
				{
					if (compare(data[start], t) >= 0)
					{
						while (compare(data[end], t) > 0)
						{
							end--;
						}
						if (start >= end)
						{
							break;
						}
						T t2 = data[start];
						data[start++] = data[end];
						data[end--] = t2;
					}
					else
					{
						start++;
					}
				}
				return end;
			}

			// Token: 0x06000AC9 RID: 2761 RVA: 0x00049894 File Offset: 0x00047A94
			public static void InsertionSort<T>(T[] data, int start, int end, Func<T, T, int> compare)
			{
				for (int i = start + 1; i < end + 1; i++)
				{
					T t = data[i];
					int num = i - 1;
					while (num >= 0 && compare(t, data[num]) < 0)
					{
						data[num + 1] = data[num];
						num--;
					}
					data[num + 1] = t;
				}
			}

			// Token: 0x06000ACA RID: 2762 RVA: 0x000498F4 File Offset: 0x00047AF4
			[CompilerGenerated]
			internal static void <Median3Pivot>g__Swap|2_0<T>(int a, int b, ref LightCookieManager.Sorting.<>c__DisplayClass2_0<T> A_2)
			{
				T t = A_2.data[a];
				A_2.data[a] = A_2.data[b];
				A_2.data[b] = t;
			}
		}

		// Token: 0x02000193 RID: 403
		private struct LightCookieMapping
		{
			// Token: 0x04000A91 RID: 2705
			public ushort visibleLightIndex;

			// Token: 0x04000A92 RID: 2706
			public ushort lightBufferIndex;

			// Token: 0x04000A93 RID: 2707
			public Light light;

			// Token: 0x04000A94 RID: 2708
			public static Func<LightCookieManager.LightCookieMapping, LightCookieManager.LightCookieMapping, int> s_CompareByCookieSize = delegate(LightCookieManager.LightCookieMapping a, LightCookieManager.LightCookieMapping b)
			{
				Texture cookie = a.light.cookie;
				Texture cookie2 = b.light.cookie;
				int num = cookie.width * cookie.height;
				int num2 = cookie2.width * cookie2.height - num;
				if (num2 == 0)
				{
					int instanceID = cookie.GetInstanceID();
					int instanceID2 = cookie2.GetInstanceID();
					return instanceID - instanceID2;
				}
				return num2;
			};

			// Token: 0x04000A95 RID: 2709
			public static Func<LightCookieManager.LightCookieMapping, LightCookieManager.LightCookieMapping, int> s_CompareByBufferIndex = (LightCookieManager.LightCookieMapping a, LightCookieManager.LightCookieMapping b) => (int)(a.lightBufferIndex - b.lightBufferIndex);
		}

		// Token: 0x02000194 RID: 404
		private readonly struct WorkSlice<T>
		{
			// Token: 0x06000ACC RID: 2764 RVA: 0x0004995F File Offset: 0x00047B5F
			public WorkSlice(T[] src, int srcLen = -1)
			{
				this = new LightCookieManager.WorkSlice<T>(src, 0, srcLen);
			}

			// Token: 0x06000ACD RID: 2765 RVA: 0x0004996A File Offset: 0x00047B6A
			public WorkSlice(T[] src, int srcStart, int srcLen = -1)
			{
				this.m_Data = src;
				this.m_Start = srcStart;
				this.m_Length = ((srcLen < 0) ? src.Length : Math.Min(srcLen, src.Length));
			}

			// Token: 0x1700024C RID: 588
			public T this[int index]
			{
				get
				{
					return this.m_Data[this.m_Start + index];
				}
				set
				{
					this.m_Data[this.m_Start + index] = value;
				}
			}

			// Token: 0x1700024D RID: 589
			// (get) Token: 0x06000AD0 RID: 2768 RVA: 0x000499BD File Offset: 0x00047BBD
			public int length
			{
				get
				{
					return this.m_Length;
				}
			}

			// Token: 0x1700024E RID: 590
			// (get) Token: 0x06000AD1 RID: 2769 RVA: 0x000499C5 File Offset: 0x00047BC5
			public int capacity
			{
				get
				{
					return this.m_Data.Length;
				}
			}

			// Token: 0x06000AD2 RID: 2770 RVA: 0x000499CF File Offset: 0x00047BCF
			public void Sort(Func<T, T, int> compare)
			{
				if (this.m_Length > 1)
				{
					LightCookieManager.Sorting.QuickSort<T>(this.m_Data, this.m_Start, this.m_Start + this.m_Length - 1, compare);
				}
			}

			// Token: 0x04000A96 RID: 2710
			private readonly T[] m_Data;

			// Token: 0x04000A97 RID: 2711
			private readonly int m_Start;

			// Token: 0x04000A98 RID: 2712
			private readonly int m_Length;
		}

		// Token: 0x02000195 RID: 405
		private class WorkMemory
		{
			// Token: 0x06000AD3 RID: 2771 RVA: 0x000499FC File Offset: 0x00047BFC
			public void Resize(int size)
			{
				int num = size;
				LightCookieManager.LightCookieMapping[] array = this.lightMappings;
				int? num2 = ((array != null) ? new int?(array.Length) : null);
				if ((num <= num2.GetValueOrDefault()) & (num2 != null))
				{
					return;
				}
				size = Math.Max(size, (size + 15) / 16 * 16);
				this.lightMappings = new LightCookieManager.LightCookieMapping[size];
				this.uvRects = new Vector4[size];
			}

			// Token: 0x04000A99 RID: 2713
			public LightCookieManager.LightCookieMapping[] lightMappings;

			// Token: 0x04000A9A RID: 2714
			public Vector4[] uvRects;
		}

		// Token: 0x02000196 RID: 406
		private struct ShaderBitArray
		{
			// Token: 0x1700024F RID: 591
			// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x00049A70 File Offset: 0x00047C70
			public int elemLength
			{
				get
				{
					if (this.m_Data != null)
					{
						return this.m_Data.Length;
					}
					return 0;
				}
			}

			// Token: 0x17000250 RID: 592
			// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x00049A84 File Offset: 0x00047C84
			public int bitCapacity
			{
				get
				{
					return this.elemLength * 32;
				}
			}

			// Token: 0x17000251 RID: 593
			// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x00049A8F File Offset: 0x00047C8F
			public float[] data
			{
				get
				{
					return this.m_Data;
				}
			}

			// Token: 0x06000AD8 RID: 2776 RVA: 0x00049A98 File Offset: 0x00047C98
			public void Resize(int bitCount)
			{
				if (this.bitCapacity > bitCount)
				{
					return;
				}
				int num = (bitCount + 31) / 32;
				int num2 = num;
				float[] data = this.m_Data;
				int? num3 = ((data != null) ? new int?(data.Length) : null);
				if ((num2 == num3.GetValueOrDefault()) & (num3 != null))
				{
					return;
				}
				float[] array = new float[num];
				if (this.m_Data != null)
				{
					for (int i = 0; i < this.m_Data.Length; i++)
					{
						array[i] = this.m_Data[i];
					}
				}
				this.m_Data = array;
			}

			// Token: 0x06000AD9 RID: 2777 RVA: 0x00049B24 File Offset: 0x00047D24
			public void Clear()
			{
				for (int i = 0; i < this.m_Data.Length; i++)
				{
					this.m_Data[i] = 0f;
				}
			}

			// Token: 0x06000ADA RID: 2778 RVA: 0x00049B51 File Offset: 0x00047D51
			private void GetElementIndexAndBitOffset(int index, out int elemIndex, out int bitOffset)
			{
				elemIndex = index >> 5;
				bitOffset = index & 31;
			}

			// Token: 0x17000252 RID: 594
			public unsafe bool this[int index]
			{
				get
				{
					int num;
					int num2;
					this.GetElementIndexAndBitOffset(index, out num, out num2);
					float[] data;
					float* ptr;
					if ((data = this.m_Data) == null || data.Length == 0)
					{
						ptr = null;
					}
					else
					{
						ptr = &data[0];
					}
					uint* ptr2 = (uint*)(ptr + num);
					return (*ptr2 & (1U << num2)) > 0U;
				}
				set
				{
					int num;
					int num2;
					this.GetElementIndexAndBitOffset(index, out num, out num2);
					float[] array;
					float* ptr;
					if ((array = this.m_Data) == null || array.Length == 0)
					{
						ptr = null;
					}
					else
					{
						ptr = &array[0];
					}
					uint* ptr2 = (uint*)(ptr + num);
					if (value)
					{
						*ptr2 |= 1U << num2;
					}
					else
					{
						*ptr2 &= ~(1U << num2);
					}
					array = null;
				}
			}

			// Token: 0x06000ADD RID: 2781 RVA: 0x00049C0C File Offset: 0x00047E0C
			public unsafe override string ToString()
			{
				int num = Math.Min(this.bitCapacity, 4096);
				byte* ptr = stackalloc byte[(UIntPtr)num];
				for (int i = 0; i < num; i++)
				{
					ptr[i] = (this[i] ? 49 : 48);
				}
				return new string((sbyte*)ptr, 0, num, Encoding.UTF8);
			}

			// Token: 0x04000A9B RID: 2715
			private const int k_BitsPerElement = 32;

			// Token: 0x04000A9C RID: 2716
			private const int k_ElementShift = 5;

			// Token: 0x04000A9D RID: 2717
			private const int k_ElementMask = 31;

			// Token: 0x04000A9E RID: 2718
			private float[] m_Data;
		}

		// Token: 0x02000197 RID: 407
		private class LightCookieShaderData : IDisposable
		{
			// Token: 0x17000253 RID: 595
			// (get) Token: 0x06000ADE RID: 2782 RVA: 0x00049C5C File Offset: 0x00047E5C
			public Matrix4x4[] worldToLights
			{
				get
				{
					return this.m_WorldToLightCpuData;
				}
			}

			// Token: 0x17000254 RID: 596
			// (get) Token: 0x06000ADF RID: 2783 RVA: 0x00049C64 File Offset: 0x00047E64
			public LightCookieManager.ShaderBitArray cookieEnableBits
			{
				get
				{
					return this.m_CookieEnableBitsCpuData;
				}
			}

			// Token: 0x17000255 RID: 597
			// (get) Token: 0x06000AE0 RID: 2784 RVA: 0x00049C6C File Offset: 0x00047E6C
			public Vector4[] atlasUVRects
			{
				get
				{
					return this.m_AtlasUVRectCpuData;
				}
			}

			// Token: 0x17000256 RID: 598
			// (get) Token: 0x06000AE1 RID: 2785 RVA: 0x00049C74 File Offset: 0x00047E74
			public float[] lightTypes
			{
				get
				{
					return this.m_LightTypeCpuData;
				}
			}

			// Token: 0x17000257 RID: 599
			// (get) Token: 0x06000AE2 RID: 2786 RVA: 0x00049C7C File Offset: 0x00047E7C
			// (set) Token: 0x06000AE3 RID: 2787 RVA: 0x00049C84 File Offset: 0x00047E84
			public bool isUploaded { get; set; }

			// Token: 0x06000AE4 RID: 2788 RVA: 0x00049C8D File Offset: 0x00047E8D
			public LightCookieShaderData(int size, bool useStructuredBuffer)
			{
				this.m_UseStructuredBuffer = useStructuredBuffer;
				this.Resize(size);
			}

			// Token: 0x06000AE5 RID: 2789 RVA: 0x00049CA3 File Offset: 0x00047EA3
			public void Dispose()
			{
				if (this.m_UseStructuredBuffer)
				{
					ComputeBuffer worldToLightBuffer = this.m_WorldToLightBuffer;
					if (worldToLightBuffer != null)
					{
						worldToLightBuffer.Dispose();
					}
					ComputeBuffer atlasUVRectBuffer = this.m_AtlasUVRectBuffer;
					if (atlasUVRectBuffer != null)
					{
						atlasUVRectBuffer.Dispose();
					}
					ComputeBuffer lightTypeBuffer = this.m_LightTypeBuffer;
					if (lightTypeBuffer == null)
					{
						return;
					}
					lightTypeBuffer.Dispose();
				}
			}

			// Token: 0x06000AE6 RID: 2790 RVA: 0x00049CE0 File Offset: 0x00047EE0
			public void Resize(int size)
			{
				if (size <= this.m_Size)
				{
					return;
				}
				if (this.m_Size > 0)
				{
					this.Dispose();
				}
				this.m_WorldToLightCpuData = new Matrix4x4[size];
				this.m_AtlasUVRectCpuData = new Vector4[size];
				this.m_LightTypeCpuData = new float[size];
				this.m_CookieEnableBitsCpuData.Resize(size);
				if (this.m_UseStructuredBuffer)
				{
					this.m_WorldToLightBuffer = new ComputeBuffer(size, Marshal.SizeOf<Matrix4x4>());
					this.m_AtlasUVRectBuffer = new ComputeBuffer(size, Marshal.SizeOf<Vector4>());
					this.m_LightTypeBuffer = new ComputeBuffer(size, Marshal.SizeOf<float>());
				}
				this.m_Size = size;
			}

			// Token: 0x06000AE7 RID: 2791 RVA: 0x00049D78 File Offset: 0x00047F78
			public void Upload(CommandBuffer cmd)
			{
				if (this.m_UseStructuredBuffer)
				{
					this.m_WorldToLightBuffer.SetData(this.m_WorldToLightCpuData);
					this.m_AtlasUVRectBuffer.SetData(this.m_AtlasUVRectCpuData);
					this.m_LightTypeBuffer.SetData(this.m_LightTypeCpuData);
					cmd.SetGlobalBuffer(LightCookieManager.ShaderProperty.additionalLightsWorldToLightBuffer, this.m_WorldToLightBuffer);
					cmd.SetGlobalBuffer(LightCookieManager.ShaderProperty.additionalLightsCookieAtlasUVRectBuffer, this.m_AtlasUVRectBuffer);
					cmd.SetGlobalBuffer(LightCookieManager.ShaderProperty.additionalLightsLightTypeBuffer, this.m_LightTypeBuffer);
				}
				else
				{
					cmd.SetGlobalMatrixArray(LightCookieManager.ShaderProperty.additionalLightsWorldToLights, this.m_WorldToLightCpuData);
					cmd.SetGlobalVectorArray(LightCookieManager.ShaderProperty.additionalLightsCookieAtlasUVRects, this.m_AtlasUVRectCpuData);
					cmd.SetGlobalFloatArray(LightCookieManager.ShaderProperty.additionalLightsLightTypes, this.m_LightTypeCpuData);
				}
				cmd.SetGlobalFloatArray(LightCookieManager.ShaderProperty.additionalLightsCookieEnableBits, this.m_CookieEnableBitsCpuData.data);
				this.isUploaded = true;
			}

			// Token: 0x06000AE8 RID: 2792 RVA: 0x00049E45 File Offset: 0x00048045
			public void Clear(CommandBuffer cmd)
			{
				if (this.isUploaded)
				{
					this.m_CookieEnableBitsCpuData.Clear();
					cmd.SetGlobalFloatArray(LightCookieManager.ShaderProperty.additionalLightsCookieEnableBits, this.m_CookieEnableBitsCpuData.data);
					this.isUploaded = false;
				}
			}

			// Token: 0x04000A9F RID: 2719
			private int m_Size;

			// Token: 0x04000AA0 RID: 2720
			private bool m_UseStructuredBuffer;

			// Token: 0x04000AA1 RID: 2721
			private Matrix4x4[] m_WorldToLightCpuData;

			// Token: 0x04000AA2 RID: 2722
			private Vector4[] m_AtlasUVRectCpuData;

			// Token: 0x04000AA3 RID: 2723
			private float[] m_LightTypeCpuData;

			// Token: 0x04000AA4 RID: 2724
			private LightCookieManager.ShaderBitArray m_CookieEnableBitsCpuData;

			// Token: 0x04000AA5 RID: 2725
			private ComputeBuffer m_WorldToLightBuffer;

			// Token: 0x04000AA6 RID: 2726
			private ComputeBuffer m_AtlasUVRectBuffer;

			// Token: 0x04000AA7 RID: 2727
			private ComputeBuffer m_LightTypeBuffer;
		}
	}
}
