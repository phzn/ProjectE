using System;
using System.Runtime.CompilerServices;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering
{
	// Token: 0x020000CF RID: 207
	public static class Blitter
	{
		// Token: 0x060006AF RID: 1711 RVA: 0x00020924 File Offset: 0x0001EB24
		public static void Initialize(Shader blitPS, Shader blitColorAndDepthPS)
		{
			if (Blitter.s_Blit != null)
			{
				throw new Exception("Blitter is already initialized. Please only initialize the blitter once or you will leak engine resources. If you need to re-initialize the blitter with different shaders destroy & recreate it.");
			}
			Blitter.s_Blit = CoreUtils.CreateEngineMaterial(blitPS);
			Blitter.s_BlitColorAndDepth = CoreUtils.CreateEngineMaterial(blitColorAndDepthPS);
			Blitter.s_DecodeHdrKeyword = new LocalKeyword(blitPS, "BLIT_DECODE_HDR");
			if (TextureXR.useTexArray)
			{
				Blitter.s_Blit.EnableKeyword("DISABLE_TEXTURE2D_X_ARRAY");
				Blitter.s_BlitTexArray = CoreUtils.CreateEngineMaterial(blitPS);
				Blitter.s_BlitTexArraySingleSlice = CoreUtils.CreateEngineMaterial(blitPS);
				Blitter.s_BlitTexArraySingleSlice.EnableKeyword("BLIT_SINGLE_SLICE");
			}
			if (SystemInfo.graphicsShaderLevel < 30)
			{
				float num = -1f;
				if (SystemInfo.usesReversedZBuffer)
				{
					num = 1f;
				}
				if (!Blitter.s_TriangleMesh)
				{
					Blitter.s_TriangleMesh = new Mesh();
					Blitter.s_TriangleMesh.vertices = Blitter.<Initialize>g__GetFullScreenTriangleVertexPosition|9_0(num);
					Blitter.s_TriangleMesh.uv = Blitter.<Initialize>g__GetFullScreenTriangleTexCoord|9_1();
					Blitter.s_TriangleMesh.triangles = new int[] { 0, 1, 2 };
				}
				if (!Blitter.s_QuadMesh)
				{
					Blitter.s_QuadMesh = new Mesh();
					Blitter.s_QuadMesh.vertices = Blitter.<Initialize>g__GetQuadVertexPosition|9_2(num);
					Blitter.s_QuadMesh.uv = Blitter.<Initialize>g__GetQuadTexCoord|9_3();
					Blitter.s_QuadMesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
				}
			}
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x00020A68 File Offset: 0x0001EC68
		public static void Cleanup()
		{
			CoreUtils.Destroy(Blitter.s_Blit);
			Blitter.s_Blit = null;
			CoreUtils.Destroy(Blitter.s_BlitColorAndDepth);
			Blitter.s_BlitColorAndDepth = null;
			CoreUtils.Destroy(Blitter.s_BlitTexArray);
			Blitter.s_BlitTexArray = null;
			CoreUtils.Destroy(Blitter.s_BlitTexArraySingleSlice);
			Blitter.s_BlitTexArraySingleSlice = null;
			CoreUtils.Destroy(Blitter.s_TriangleMesh);
			Blitter.s_TriangleMesh = null;
			CoreUtils.Destroy(Blitter.s_QuadMesh);
			Blitter.s_QuadMesh = null;
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x00020AD5 File Offset: 0x0001ECD5
		public static Material GetBlitMaterial(TextureDimension dimension, bool singleSlice = false)
		{
			if (dimension != TextureDimension.Tex2DArray)
			{
				return Blitter.s_Blit;
			}
			if (!singleSlice)
			{
				return Blitter.s_BlitTexArray;
			}
			return Blitter.s_BlitTexArraySingleSlice;
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00020AF1 File Offset: 0x0001ECF1
		private static void DrawTriangle(CommandBuffer cmd, Material material, int shaderPass)
		{
			if (SystemInfo.graphicsShaderLevel < 30)
			{
				cmd.DrawMesh(Blitter.s_TriangleMesh, Matrix4x4.identity, material, 0, shaderPass, Blitter.s_PropertyBlock);
				return;
			}
			cmd.DrawProcedural(Matrix4x4.identity, material, shaderPass, MeshTopology.Triangles, 3, 1, Blitter.s_PropertyBlock);
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00020B2A File Offset: 0x0001ED2A
		internal static void DrawQuad(CommandBuffer cmd, Material material, int shaderPass)
		{
			if (SystemInfo.graphicsShaderLevel < 30)
			{
				cmd.DrawMesh(Blitter.s_QuadMesh, Matrix4x4.identity, material, 0, shaderPass, Blitter.s_PropertyBlock);
				return;
			}
			cmd.DrawProcedural(Matrix4x4.identity, material, shaderPass, MeshTopology.Quads, 4, 1, Blitter.s_PropertyBlock);
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00020B63 File Offset: 0x0001ED63
		public static void BlitTexture(CommandBuffer cmd, RTHandle source, Vector4 scaleBias, float mipLevel, bool bilinear)
		{
			Blitter.s_PropertyBlock.SetFloat(Blitter.BlitShaderIDs._BlitMipLevel, mipLevel);
			Blitter.BlitTexture(cmd, source, scaleBias, Blitter.GetBlitMaterial(TextureXR.dimension, false), bilinear ? 1 : 0);
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x00020B90 File Offset: 0x0001ED90
		public static void BlitTexture2D(CommandBuffer cmd, RTHandle source, Vector4 scaleBias, float mipLevel, bool bilinear)
		{
			Blitter.s_PropertyBlock.SetFloat(Blitter.BlitShaderIDs._BlitMipLevel, mipLevel);
			Blitter.BlitTexture(cmd, source, scaleBias, Blitter.GetBlitMaterial(TextureDimension.Tex2D, false), bilinear ? 1 : 0);
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00020BBC File Offset: 0x0001EDBC
		public static void BlitColorAndDepth(CommandBuffer cmd, Texture sourceColor, RenderTexture sourceDepth, Vector4 scaleBias, float mipLevel, bool blitDepth)
		{
			Blitter.s_PropertyBlock.SetFloat(Blitter.BlitShaderIDs._BlitMipLevel, mipLevel);
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitScaleBias, scaleBias);
			Blitter.s_PropertyBlock.SetTexture(Blitter.BlitShaderIDs._BlitTexture, sourceColor);
			if (blitDepth)
			{
				Blitter.s_PropertyBlock.SetTexture(Blitter.BlitShaderIDs._InputDepth, sourceDepth, RenderTextureSubElement.Depth);
			}
			Blitter.DrawTriangle(cmd, Blitter.s_BlitColorAndDepth, blitDepth ? 1 : 0);
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x00020C22 File Offset: 0x0001EE22
		public static void BlitTexture(CommandBuffer cmd, RTHandle source, Vector4 scaleBias, Material material, int pass)
		{
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitScaleBias, scaleBias);
			Blitter.s_PropertyBlock.SetTexture(Blitter.BlitShaderIDs._BlitTexture, source);
			Blitter.DrawTriangle(cmd, material, pass);
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x00020C52 File Offset: 0x0001EE52
		public static void BlitTexture(CommandBuffer cmd, RenderTargetIdentifier source, Vector4 scaleBias, Material material, int pass)
		{
			Blitter.s_PropertyBlock.Clear();
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitScaleBias, scaleBias);
			cmd.SetGlobalTexture(Blitter.BlitShaderIDs._BlitTexture, source);
			Blitter.DrawTriangle(cmd, material, pass);
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x00020C84 File Offset: 0x0001EE84
		public static void BlitTexture(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material, int pass)
		{
			Blitter.s_PropertyBlock.Clear();
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitScaleBias, Vector2.one);
			cmd.SetGlobalTexture(Blitter.BlitShaderIDs._BlitTexture, source);
			cmd.SetRenderTarget(destination);
			Blitter.DrawTriangle(cmd, material, pass);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x00020CD0 File Offset: 0x0001EED0
		public static void BlitTexture(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, RenderBufferLoadAction loadAction, RenderBufferStoreAction storeAction, Material material, int pass)
		{
			Blitter.s_PropertyBlock.Clear();
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitScaleBias, Vector2.one);
			cmd.SetGlobalTexture(Blitter.BlitShaderIDs._BlitTexture, source);
			cmd.SetRenderTarget(destination, loadAction, storeAction);
			Blitter.DrawTriangle(cmd, material, pass);
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00020D20 File Offset: 0x0001EF20
		public static void BlitTexture(CommandBuffer cmd, Vector4 scaleBias, Material material, int pass)
		{
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitScaleBias, scaleBias);
			Blitter.DrawTriangle(cmd, material, pass);
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00020D3C File Offset: 0x0001EF3C
		public static void BlitCameraTexture(CommandBuffer cmd, RTHandle source, RTHandle destination, float mipLevel = 0f, bool bilinear = false)
		{
			Vector2 vector = (source.useScaling ? new Vector2(source.rtHandleProperties.rtHandleScale.x, source.rtHandleProperties.rtHandleScale.y) : Vector2.one);
			CoreUtils.SetRenderTarget(cmd, destination, ClearFlag.None, 0, CubemapFace.Unknown, -1);
			Blitter.BlitTexture(cmd, source, vector, mipLevel, bilinear);
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00020D9C File Offset: 0x0001EF9C
		public static void BlitCameraTexture2D(CommandBuffer cmd, RTHandle source, RTHandle destination, float mipLevel = 0f, bool bilinear = false)
		{
			Vector2 vector = (source.useScaling ? new Vector2(source.rtHandleProperties.rtHandleScale.x, source.rtHandleProperties.rtHandleScale.y) : Vector2.one);
			CoreUtils.SetRenderTarget(cmd, destination, ClearFlag.None, 0, CubemapFace.Unknown, -1);
			Blitter.BlitTexture2D(cmd, source, vector, mipLevel, bilinear);
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x00020DFC File Offset: 0x0001EFFC
		public static void BlitCameraTexture(CommandBuffer cmd, RTHandle source, RTHandle destination, Material material, int pass)
		{
			Vector2 vector = (source.useScaling ? new Vector2(source.rtHandleProperties.rtHandleScale.x, source.rtHandleProperties.rtHandleScale.y) : Vector2.one);
			CoreUtils.SetRenderTarget(cmd, destination, ClearFlag.None, 0, CubemapFace.Unknown, -1);
			Blitter.BlitTexture(cmd, source, vector, material, pass);
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x00020E5C File Offset: 0x0001F05C
		public static void BlitCameraTexture(CommandBuffer cmd, RTHandle source, RTHandle destination, RenderBufferLoadAction loadAction, RenderBufferStoreAction storeAction, Material material, int pass)
		{
			Vector2 vector = (source.useScaling ? new Vector2(source.rtHandleProperties.rtHandleScale.x, source.rtHandleProperties.rtHandleScale.y) : Vector2.one);
			CoreUtils.SetRenderTarget(cmd, destination, loadAction, storeAction, ClearFlag.None, Color.clear, 0, CubemapFace.Unknown, -1);
			Blitter.BlitTexture(cmd, source, vector, material, pass);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x00020EC2 File Offset: 0x0001F0C2
		public static void BlitCameraTexture(CommandBuffer cmd, RTHandle source, RTHandle destination, Vector4 scaleBias, float mipLevel = 0f, bool bilinear = false)
		{
			CoreUtils.SetRenderTarget(cmd, destination, ClearFlag.None, 0, CubemapFace.Unknown, -1);
			Blitter.BlitTexture(cmd, source, scaleBias, mipLevel, bilinear);
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x00020EDC File Offset: 0x0001F0DC
		public static void BlitCameraTexture(CommandBuffer cmd, RTHandle source, RTHandle destination, Rect destViewport, float mipLevel = 0f, bool bilinear = false)
		{
			Vector2 vector = (source.useScaling ? new Vector2(source.rtHandleProperties.rtHandleScale.x, source.rtHandleProperties.rtHandleScale.y) : Vector2.one);
			CoreUtils.SetRenderTarget(cmd, destination, ClearFlag.None, 0, CubemapFace.Unknown, -1);
			cmd.SetViewport(destViewport);
			Blitter.BlitTexture(cmd, source, vector, mipLevel, bilinear);
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x00020F44 File Offset: 0x0001F144
		public static void BlitQuad(CommandBuffer cmd, Texture source, Vector4 scaleBiasTex, Vector4 scaleBiasRT, int mipLevelTex, bool bilinear)
		{
			Blitter.s_PropertyBlock.SetTexture(Blitter.BlitShaderIDs._BlitTexture, source);
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitScaleBias, scaleBiasTex);
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitScaleBiasRt, scaleBiasRT);
			Blitter.s_PropertyBlock.SetFloat(Blitter.BlitShaderIDs._BlitMipLevel, (float)mipLevelTex);
			Blitter.DrawQuad(cmd, Blitter.GetBlitMaterial(source.dimension, false), bilinear ? 3 : 2);
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x00020FB0 File Offset: 0x0001F1B0
		public static void BlitQuadWithPadding(CommandBuffer cmd, Texture source, Vector2 textureSize, Vector4 scaleBiasTex, Vector4 scaleBiasRT, int mipLevelTex, bool bilinear, int paddingInPixels)
		{
			Blitter.s_PropertyBlock.SetTexture(Blitter.BlitShaderIDs._BlitTexture, source);
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitScaleBias, scaleBiasTex);
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitScaleBiasRt, scaleBiasRT);
			Blitter.s_PropertyBlock.SetFloat(Blitter.BlitShaderIDs._BlitMipLevel, (float)mipLevelTex);
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitTextureSize, textureSize);
			Blitter.s_PropertyBlock.SetInt(Blitter.BlitShaderIDs._BlitPaddingSize, paddingInPixels);
			if (source.wrapMode == TextureWrapMode.Repeat)
			{
				Blitter.DrawQuad(cmd, Blitter.GetBlitMaterial(source.dimension, false), bilinear ? 7 : 6);
				return;
			}
			Blitter.DrawQuad(cmd, Blitter.GetBlitMaterial(source.dimension, false), bilinear ? 5 : 4);
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x00021064 File Offset: 0x0001F264
		public static void BlitQuadWithPaddingMultiply(CommandBuffer cmd, Texture source, Vector2 textureSize, Vector4 scaleBiasTex, Vector4 scaleBiasRT, int mipLevelTex, bool bilinear, int paddingInPixels)
		{
			Blitter.s_PropertyBlock.SetTexture(Blitter.BlitShaderIDs._BlitTexture, source);
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitScaleBias, scaleBiasTex);
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitScaleBiasRt, scaleBiasRT);
			Blitter.s_PropertyBlock.SetFloat(Blitter.BlitShaderIDs._BlitMipLevel, (float)mipLevelTex);
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitTextureSize, textureSize);
			Blitter.s_PropertyBlock.SetInt(Blitter.BlitShaderIDs._BlitPaddingSize, paddingInPixels);
			if (source.wrapMode == TextureWrapMode.Repeat)
			{
				Blitter.DrawQuad(cmd, Blitter.GetBlitMaterial(source.dimension, false), bilinear ? 12 : 11);
				return;
			}
			Blitter.DrawQuad(cmd, Blitter.GetBlitMaterial(source.dimension, false), bilinear ? 10 : 9);
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x0002111C File Offset: 0x0001F31C
		public static void BlitOctahedralWithPadding(CommandBuffer cmd, Texture source, Vector2 textureSize, Vector4 scaleBiasTex, Vector4 scaleBiasRT, int mipLevelTex, bool bilinear, int paddingInPixels)
		{
			Blitter.s_PropertyBlock.SetTexture(Blitter.BlitShaderIDs._BlitTexture, source);
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitScaleBias, scaleBiasTex);
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitScaleBiasRt, scaleBiasRT);
			Blitter.s_PropertyBlock.SetFloat(Blitter.BlitShaderIDs._BlitMipLevel, (float)mipLevelTex);
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitTextureSize, textureSize);
			Blitter.s_PropertyBlock.SetInt(Blitter.BlitShaderIDs._BlitPaddingSize, paddingInPixels);
			Blitter.DrawQuad(cmd, Blitter.GetBlitMaterial(source.dimension, false), 8);
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x000211A8 File Offset: 0x0001F3A8
		public static void BlitOctahedralWithPaddingMultiply(CommandBuffer cmd, Texture source, Vector2 textureSize, Vector4 scaleBiasTex, Vector4 scaleBiasRT, int mipLevelTex, bool bilinear, int paddingInPixels)
		{
			Blitter.s_PropertyBlock.SetTexture(Blitter.BlitShaderIDs._BlitTexture, source);
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitScaleBias, scaleBiasTex);
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitScaleBiasRt, scaleBiasRT);
			Blitter.s_PropertyBlock.SetFloat(Blitter.BlitShaderIDs._BlitMipLevel, (float)mipLevelTex);
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitTextureSize, textureSize);
			Blitter.s_PropertyBlock.SetInt(Blitter.BlitShaderIDs._BlitPaddingSize, paddingInPixels);
			Blitter.DrawQuad(cmd, Blitter.GetBlitMaterial(source.dimension, false), 13);
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x00021234 File Offset: 0x0001F434
		public static void BlitCubeToOctahedral2DQuad(CommandBuffer cmd, Texture source, Vector4 scaleBiasRT, int mipLevelTex)
		{
			Blitter.s_PropertyBlock.SetTexture(Blitter.BlitShaderIDs._BlitCubeTexture, source);
			Blitter.s_PropertyBlock.SetFloat(Blitter.BlitShaderIDs._BlitMipLevel, (float)mipLevelTex);
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitScaleBias, new Vector4(1f, 1f, 0f, 0f));
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitScaleBiasRt, scaleBiasRT);
			Blitter.DrawQuad(cmd, Blitter.GetBlitMaterial(source.dimension, false), 14);
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x000212B0 File Offset: 0x0001F4B0
		public static void BlitCubeToOctahedral2DQuadWithPadding(CommandBuffer cmd, Texture source, Vector2 textureSize, Vector4 scaleBiasRT, int mipLevelTex, bool bilinear, int paddingInPixels, Vector4? decodeInstructions = null)
		{
			Material blitMaterial = Blitter.GetBlitMaterial(source.dimension, false);
			Blitter.s_PropertyBlock.SetTexture(Blitter.BlitShaderIDs._BlitCubeTexture, source);
			Blitter.s_PropertyBlock.SetFloat(Blitter.BlitShaderIDs._BlitMipLevel, (float)mipLevelTex);
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitScaleBias, new Vector4(1f, 1f, 0f, 0f));
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitScaleBiasRt, scaleBiasRT);
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitTextureSize, textureSize);
			Blitter.s_PropertyBlock.SetInt(Blitter.BlitShaderIDs._BlitPaddingSize, paddingInPixels);
			cmd.SetKeyword(blitMaterial, in Blitter.s_DecodeHdrKeyword, decodeInstructions != null);
			if (decodeInstructions != null)
			{
				Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitDecodeInstructions, decodeInstructions.Value);
			}
			Blitter.DrawQuad(cmd, blitMaterial, bilinear ? 22 : 21);
			cmd.SetKeyword(blitMaterial, in Blitter.s_DecodeHdrKeyword, false);
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0002139C File Offset: 0x0001F59C
		public static void BlitCubeToOctahedral2DQuadSingleChannel(CommandBuffer cmd, Texture source, Vector4 scaleBiasRT, int mipLevelTex)
		{
			int num = 15;
			if (GraphicsFormatUtility.GetComponentCount(source.graphicsFormat) == 1U)
			{
				if (GraphicsFormatUtility.IsAlphaOnlyFormat(source.graphicsFormat))
				{
					num = 16;
				}
				if (GraphicsFormatUtility.GetSwizzleR(source.graphicsFormat) == FormatSwizzle.FormatSwizzleR)
				{
					num = 17;
				}
			}
			Blitter.s_PropertyBlock.SetTexture(Blitter.BlitShaderIDs._BlitCubeTexture, source);
			Blitter.s_PropertyBlock.SetFloat(Blitter.BlitShaderIDs._BlitMipLevel, (float)mipLevelTex);
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitScaleBias, new Vector4(1f, 1f, 0f, 0f));
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitScaleBiasRt, scaleBiasRT);
			Blitter.DrawQuad(cmd, Blitter.GetBlitMaterial(source.dimension, false), num);
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00021448 File Offset: 0x0001F648
		public static void BlitQuadSingleChannel(CommandBuffer cmd, Texture source, Vector4 scaleBiasTex, Vector4 scaleBiasRT, int mipLevelTex)
		{
			int num = 18;
			if (GraphicsFormatUtility.GetComponentCount(source.graphicsFormat) == 1U)
			{
				if (GraphicsFormatUtility.IsAlphaOnlyFormat(source.graphicsFormat))
				{
					num = 19;
				}
				if (GraphicsFormatUtility.GetSwizzleR(source.graphicsFormat) == FormatSwizzle.FormatSwizzleR)
				{
					num = 20;
				}
			}
			Blitter.s_PropertyBlock.SetTexture(Blitter.BlitShaderIDs._BlitTexture, source);
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitScaleBias, scaleBiasTex);
			Blitter.s_PropertyBlock.SetVector(Blitter.BlitShaderIDs._BlitScaleBiasRt, scaleBiasRT);
			Blitter.s_PropertyBlock.SetFloat(Blitter.BlitShaderIDs._BlitMipLevel, (float)mipLevelTex);
			Blitter.DrawQuad(cmd, Blitter.GetBlitMaterial(source.dimension, false), num);
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x000214E8 File Offset: 0x0001F6E8
		[CompilerGenerated]
		internal static Vector3[] <Initialize>g__GetFullScreenTriangleVertexPosition|9_0(float z)
		{
			Vector3[] array = new Vector3[3];
			for (int i = 0; i < 3; i++)
			{
				Vector2 vector = new Vector2((float)((i << 1) & 2), (float)(i & 2));
				array[i] = new Vector3(vector.x * 2f - 1f, vector.y * 2f - 1f, z);
			}
			return array;
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0002154C File Offset: 0x0001F74C
		[CompilerGenerated]
		internal static Vector2[] <Initialize>g__GetFullScreenTriangleTexCoord|9_1()
		{
			Vector2[] array = new Vector2[3];
			for (int i = 0; i < 3; i++)
			{
				if (SystemInfo.graphicsUVStartsAtTop)
				{
					array[i] = new Vector2((float)((i << 1) & 2), 1f - (float)(i & 2));
				}
				else
				{
					array[i] = new Vector2((float)((i << 1) & 2), (float)(i & 2));
				}
			}
			return array;
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x000215A8 File Offset: 0x0001F7A8
		[CompilerGenerated]
		internal static Vector3[] <Initialize>g__GetQuadVertexPosition|9_2(float z)
		{
			Vector3[] array = new Vector3[4];
			for (uint num = 0U; num < 4U; num += 1U)
			{
				uint num2 = num >> 1;
				uint num3 = num & 1U;
				float num4 = num2;
				float num5 = (1U - (num2 + num3)) & 1U;
				array[(int)num] = new Vector3(num4, num5, z);
			}
			return array;
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x000215F4 File Offset: 0x0001F7F4
		[CompilerGenerated]
		internal static Vector2[] <Initialize>g__GetQuadTexCoord|9_3()
		{
			Vector2[] array = new Vector2[4];
			for (uint num = 0U; num < 4U; num += 1U)
			{
				uint num2 = num >> 1;
				uint num3 = num & 1U;
				float num4 = num2;
				float num5 = (num2 + num3) & 1U;
				if (SystemInfo.graphicsUVStartsAtTop)
				{
					num5 = 1f - num5;
				}
				array[(int)num] = new Vector2(num4, num5);
			}
			return array;
		}

		// Token: 0x0400046F RID: 1135
		private static Material s_Blit;

		// Token: 0x04000470 RID: 1136
		private static Material s_BlitTexArray;

		// Token: 0x04000471 RID: 1137
		private static Material s_BlitTexArraySingleSlice;

		// Token: 0x04000472 RID: 1138
		private static Material s_BlitColorAndDepth;

		// Token: 0x04000473 RID: 1139
		private static MaterialPropertyBlock s_PropertyBlock = new MaterialPropertyBlock();

		// Token: 0x04000474 RID: 1140
		private static Mesh s_TriangleMesh;

		// Token: 0x04000475 RID: 1141
		private static Mesh s_QuadMesh;

		// Token: 0x04000476 RID: 1142
		private static LocalKeyword s_DecodeHdrKeyword;

		// Token: 0x020001C7 RID: 455
		private static class BlitShaderIDs
		{
			// Token: 0x0400075E RID: 1886
			public static readonly int _BlitTexture = Shader.PropertyToID("_BlitTexture");

			// Token: 0x0400075F RID: 1887
			public static readonly int _BlitCubeTexture = Shader.PropertyToID("_BlitCubeTexture");

			// Token: 0x04000760 RID: 1888
			public static readonly int _BlitScaleBias = Shader.PropertyToID("_BlitScaleBias");

			// Token: 0x04000761 RID: 1889
			public static readonly int _BlitScaleBiasRt = Shader.PropertyToID("_BlitScaleBiasRt");

			// Token: 0x04000762 RID: 1890
			public static readonly int _BlitMipLevel = Shader.PropertyToID("_BlitMipLevel");

			// Token: 0x04000763 RID: 1891
			public static readonly int _BlitTextureSize = Shader.PropertyToID("_BlitTextureSize");

			// Token: 0x04000764 RID: 1892
			public static readonly int _BlitPaddingSize = Shader.PropertyToID("_BlitPaddingSize");

			// Token: 0x04000765 RID: 1893
			public static readonly int _BlitDecodeInstructions = Shader.PropertyToID("_BlitDecodeInstructions");

			// Token: 0x04000766 RID: 1894
			public static readonly int _InputDepth = Shader.PropertyToID("_InputDepthTexture");
		}
	}
}
