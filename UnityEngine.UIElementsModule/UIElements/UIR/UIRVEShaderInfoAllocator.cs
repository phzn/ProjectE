using System;
using Unity.Collections;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x02000479 RID: 1145
	internal struct UIRVEShaderInfoAllocator
	{
		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06002360 RID: 9056 RVA: 0x00089B5C File Offset: 0x00087D5C
		private static int pageWidth
		{
			get
			{
				return 32;
			}
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06002361 RID: 9057 RVA: 0x00089B70 File Offset: 0x00087D70
		private static int pageHeight
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x00089B84 File Offset: 0x00087D84
		private static Vector2Int AllocToTexelCoord(ref BitmapAllocator32 allocator, BMPAlloc alloc)
		{
			ushort num;
			ushort num2;
			allocator.GetAllocPageAtlasLocation(alloc.page, out num, out num2);
			return new Vector2Int((int)alloc.bitIndex * allocator.entryWidth + (int)num, (int)alloc.pageLine * allocator.entryHeight + (int)num2);
		}

		// Token: 0x06002363 RID: 9059 RVA: 0x00089BCC File Offset: 0x00087DCC
		private static int AllocToConstantBufferIndex(BMPAlloc alloc)
		{
			return (int)alloc.pageLine * UIRVEShaderInfoAllocator.pageWidth + (int)alloc.bitIndex;
		}

		// Token: 0x06002364 RID: 9060 RVA: 0x00089BF4 File Offset: 0x00087DF4
		private static bool AtlasRectMatchesPage(ref BitmapAllocator32 allocator, BMPAlloc defAlloc, RectInt atlasRect)
		{
			ushort num;
			ushort num2;
			allocator.GetAllocPageAtlasLocation(defAlloc.page, out num, out num2);
			return (int)num == atlasRect.xMin && (int)num2 == atlasRect.yMin && allocator.entryWidth * UIRVEShaderInfoAllocator.pageWidth == atlasRect.width && allocator.entryHeight * UIRVEShaderInfoAllocator.pageHeight == atlasRect.height;
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06002365 RID: 9061 RVA: 0x00089C58 File Offset: 0x00087E58
		public NativeSlice<Transform3x4> transformConstants
		{
			get
			{
				return this.m_Transforms;
			}
		}

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06002366 RID: 9062 RVA: 0x00089C78 File Offset: 0x00087E78
		public NativeSlice<Vector4> clipRectConstants
		{
			get
			{
				return this.m_ClipRects;
			}
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06002367 RID: 9063 RVA: 0x00089C98 File Offset: 0x00087E98
		public Texture atlas
		{
			get
			{
				bool storageReallyCreated = this.m_StorageReallyCreated;
				Texture texture;
				if (storageReallyCreated)
				{
					texture = this.m_Storage.texture;
				}
				else
				{
					texture = (this.m_VertexTexturingEnabled ? UIRenderDevice.defaultShaderInfoTexFloat : UIRenderDevice.defaultShaderInfoTexARGB8);
				}
				return texture;
			}
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06002368 RID: 9064 RVA: 0x00089CD8 File Offset: 0x00087ED8
		public bool internalAtlasCreated
		{
			get
			{
				return this.m_StorageReallyCreated;
			}
		}

		// Token: 0x06002369 RID: 9065 RVA: 0x00089CF0 File Offset: 0x00087EF0
		public void Construct()
		{
			this.m_OpacityAllocator = (this.m_ColorAllocator = (this.m_ClipRectAllocator = (this.m_TransformAllocator = (this.m_TextSettingsAllocator = default(BitmapAllocator32)))));
			this.m_TransformAllocator.Construct(UIRVEShaderInfoAllocator.pageHeight, 1, 3);
			this.m_TransformAllocator.ForceFirstAlloc((ushort)UIRVEShaderInfoAllocator.identityTransformTexel.x, (ushort)UIRVEShaderInfoAllocator.identityTransformTexel.y);
			this.m_ClipRectAllocator.Construct(UIRVEShaderInfoAllocator.pageHeight, 1, 1);
			this.m_ClipRectAllocator.ForceFirstAlloc((ushort)UIRVEShaderInfoAllocator.infiniteClipRectTexel.x, (ushort)UIRVEShaderInfoAllocator.infiniteClipRectTexel.y);
			this.m_OpacityAllocator.Construct(UIRVEShaderInfoAllocator.pageHeight, 1, 1);
			this.m_OpacityAllocator.ForceFirstAlloc((ushort)UIRVEShaderInfoAllocator.fullOpacityTexel.x, (ushort)UIRVEShaderInfoAllocator.fullOpacityTexel.y);
			this.m_ColorAllocator.Construct(UIRVEShaderInfoAllocator.pageHeight, 1, 1);
			this.m_ColorAllocator.ForceFirstAlloc((ushort)UIRVEShaderInfoAllocator.clearColorTexel.x, (ushort)UIRVEShaderInfoAllocator.clearColorTexel.y);
			this.m_TextSettingsAllocator.Construct(UIRVEShaderInfoAllocator.pageHeight, 1, 4);
			this.m_TextSettingsAllocator.ForceFirstAlloc((ushort)UIRVEShaderInfoAllocator.defaultTextCoreSettingsTexel.x, (ushort)UIRVEShaderInfoAllocator.defaultTextCoreSettingsTexel.y);
			this.m_VertexTexturingEnabled = UIRenderDevice.vertexTexturingIsAvailable;
			bool flag = !this.m_VertexTexturingEnabled;
			if (flag)
			{
				int num = 20;
				this.m_Transforms = new NativeArray<Transform3x4>(num, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				this.m_ClipRects = new NativeArray<Vector4>(num, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				this.m_Transforms[0] = new Transform3x4
				{
					v0 = UIRVEShaderInfoAllocator.identityTransformRow0Value,
					v1 = UIRVEShaderInfoAllocator.identityTransformRow1Value,
					v2 = UIRVEShaderInfoAllocator.identityTransformRow2Value
				};
				this.m_ClipRects[0] = UIRVEShaderInfoAllocator.infiniteClipRectValue;
			}
		}

		// Token: 0x0600236A RID: 9066 RVA: 0x00089EE0 File Offset: 0x000880E0
		private void ReallyCreateStorage()
		{
			bool vertexTexturingEnabled = this.m_VertexTexturingEnabled;
			if (vertexTexturingEnabled)
			{
				this.m_Storage = new ShaderInfoStorageRGBAFloat(64, 4096);
			}
			else
			{
				this.m_Storage = new ShaderInfoStorageRGBA32(64, 4096);
			}
			RectInt rectInt;
			this.m_Storage.AllocateRect(UIRVEShaderInfoAllocator.pageWidth * this.m_TransformAllocator.entryWidth, UIRVEShaderInfoAllocator.pageHeight * this.m_TransformAllocator.entryHeight, out rectInt);
			RectInt rectInt2;
			this.m_Storage.AllocateRect(UIRVEShaderInfoAllocator.pageWidth * this.m_ClipRectAllocator.entryWidth, UIRVEShaderInfoAllocator.pageHeight * this.m_ClipRectAllocator.entryHeight, out rectInt2);
			RectInt rectInt3;
			this.m_Storage.AllocateRect(UIRVEShaderInfoAllocator.pageWidth * this.m_OpacityAllocator.entryWidth, UIRVEShaderInfoAllocator.pageHeight * this.m_OpacityAllocator.entryHeight, out rectInt3);
			RectInt rectInt4;
			this.m_Storage.AllocateRect(UIRVEShaderInfoAllocator.pageWidth * this.m_ColorAllocator.entryWidth, UIRVEShaderInfoAllocator.pageHeight * this.m_ColorAllocator.entryHeight, out rectInt4);
			RectInt rectInt5;
			this.m_Storage.AllocateRect(UIRVEShaderInfoAllocator.pageWidth * this.m_TextSettingsAllocator.entryWidth, UIRVEShaderInfoAllocator.pageHeight * this.m_TextSettingsAllocator.entryHeight, out rectInt5);
			bool flag = !UIRVEShaderInfoAllocator.AtlasRectMatchesPage(ref this.m_TransformAllocator, UIRVEShaderInfoAllocator.identityTransform, rectInt);
			if (flag)
			{
				throw new Exception("Atlas identity transform allocation failed unexpectedly");
			}
			bool flag2 = !UIRVEShaderInfoAllocator.AtlasRectMatchesPage(ref this.m_ClipRectAllocator, UIRVEShaderInfoAllocator.infiniteClipRect, rectInt2);
			if (flag2)
			{
				throw new Exception("Atlas infinite clip rect allocation failed unexpectedly");
			}
			bool flag3 = !UIRVEShaderInfoAllocator.AtlasRectMatchesPage(ref this.m_OpacityAllocator, UIRVEShaderInfoAllocator.fullOpacity, rectInt3);
			if (flag3)
			{
				throw new Exception("Atlas full opacity allocation failed unexpectedly");
			}
			bool flag4 = !UIRVEShaderInfoAllocator.AtlasRectMatchesPage(ref this.m_ColorAllocator, UIRVEShaderInfoAllocator.clearColor, rectInt4);
			if (flag4)
			{
				throw new Exception("Atlas clear color allocation failed unexpectedly");
			}
			bool flag5 = !UIRVEShaderInfoAllocator.AtlasRectMatchesPage(ref this.m_TextSettingsAllocator, UIRVEShaderInfoAllocator.defaultTextCoreSettings, rectInt5);
			if (flag5)
			{
				throw new Exception("Atlas text setting allocation failed unexpectedly");
			}
			this.SetTransformValue(UIRVEShaderInfoAllocator.identityTransform, UIRVEShaderInfoAllocator.identityTransformValue);
			this.SetClipRectValue(UIRVEShaderInfoAllocator.infiniteClipRect, UIRVEShaderInfoAllocator.infiniteClipRectValue);
			this.SetOpacityValue(UIRVEShaderInfoAllocator.fullOpacity, UIRVEShaderInfoAllocator.fullOpacityValue.w);
			this.SetColorValue(UIRVEShaderInfoAllocator.clearColor, UIRVEShaderInfoAllocator.clearColorValue, false);
			this.SetTextCoreSettingValue(UIRVEShaderInfoAllocator.defaultTextCoreSettings, UIRVEShaderInfoAllocator.defaultTextCoreSettingsValue, false);
			this.m_StorageReallyCreated = true;
		}

		// Token: 0x0600236B RID: 9067 RVA: 0x0008A134 File Offset: 0x00088334
		public void Dispose()
		{
			bool flag = this.m_Storage != null;
			if (flag)
			{
				this.m_Storage.Dispose();
			}
			this.m_Storage = null;
			bool isCreated = this.m_ClipRects.IsCreated;
			if (isCreated)
			{
				this.m_ClipRects.Dispose();
			}
			bool isCreated2 = this.m_Transforms.IsCreated;
			if (isCreated2)
			{
				this.m_Transforms.Dispose();
			}
			this.m_StorageReallyCreated = false;
		}

		// Token: 0x0600236C RID: 9068 RVA: 0x0008A19F File Offset: 0x0008839F
		public void IssuePendingStorageChanges()
		{
			BaseShaderInfoStorage storage = this.m_Storage;
			if (storage != null)
			{
				storage.UpdateTexture();
			}
		}

		// Token: 0x0600236D RID: 9069 RVA: 0x0008A1B4 File Offset: 0x000883B4
		public BMPAlloc AllocTransform()
		{
			bool flag = !this.m_StorageReallyCreated;
			if (flag)
			{
				this.ReallyCreateStorage();
			}
			bool vertexTexturingEnabled = this.m_VertexTexturingEnabled;
			BMPAlloc bmpalloc;
			if (vertexTexturingEnabled)
			{
				bmpalloc = this.m_TransformAllocator.Allocate(this.m_Storage);
			}
			else
			{
				BMPAlloc bmpalloc2 = this.m_TransformAllocator.Allocate(null);
				bool flag2 = UIRVEShaderInfoAllocator.AllocToConstantBufferIndex(bmpalloc2) < this.m_Transforms.Length;
				if (flag2)
				{
					bmpalloc = bmpalloc2;
				}
				else
				{
					this.m_TransformAllocator.Free(bmpalloc2);
					bmpalloc = BMPAlloc.Invalid;
				}
			}
			return bmpalloc;
		}

		// Token: 0x0600236E RID: 9070 RVA: 0x0008A234 File Offset: 0x00088434
		public BMPAlloc AllocClipRect()
		{
			bool flag = !this.m_StorageReallyCreated;
			if (flag)
			{
				this.ReallyCreateStorage();
			}
			bool vertexTexturingEnabled = this.m_VertexTexturingEnabled;
			BMPAlloc bmpalloc;
			if (vertexTexturingEnabled)
			{
				bmpalloc = this.m_ClipRectAllocator.Allocate(this.m_Storage);
			}
			else
			{
				BMPAlloc bmpalloc2 = this.m_ClipRectAllocator.Allocate(null);
				bool flag2 = UIRVEShaderInfoAllocator.AllocToConstantBufferIndex(bmpalloc2) < this.m_ClipRects.Length;
				if (flag2)
				{
					bmpalloc = bmpalloc2;
				}
				else
				{
					this.m_ClipRectAllocator.Free(bmpalloc2);
					bmpalloc = BMPAlloc.Invalid;
				}
			}
			return bmpalloc;
		}

		// Token: 0x0600236F RID: 9071 RVA: 0x0008A2B4 File Offset: 0x000884B4
		public BMPAlloc AllocOpacity()
		{
			bool flag = !this.m_StorageReallyCreated;
			if (flag)
			{
				this.ReallyCreateStorage();
			}
			return this.m_OpacityAllocator.Allocate(this.m_Storage);
		}

		// Token: 0x06002370 RID: 9072 RVA: 0x0008A2EC File Offset: 0x000884EC
		public BMPAlloc AllocColor()
		{
			bool flag = !this.m_StorageReallyCreated;
			if (flag)
			{
				this.ReallyCreateStorage();
			}
			return this.m_ColorAllocator.Allocate(this.m_Storage);
		}

		// Token: 0x06002371 RID: 9073 RVA: 0x0008A324 File Offset: 0x00088524
		public BMPAlloc AllocTextCoreSettings(TextCoreSettings settings)
		{
			bool flag = !this.m_StorageReallyCreated;
			if (flag)
			{
				this.ReallyCreateStorage();
			}
			return this.m_TextSettingsAllocator.Allocate(this.m_Storage);
		}

		// Token: 0x06002372 RID: 9074 RVA: 0x0008A35C File Offset: 0x0008855C
		public void SetTransformValue(BMPAlloc alloc, Matrix4x4 xform)
		{
			Debug.Assert(alloc.IsValid());
			bool vertexTexturingEnabled = this.m_VertexTexturingEnabled;
			if (vertexTexturingEnabled)
			{
				Vector2Int vector2Int = UIRVEShaderInfoAllocator.AllocToTexelCoord(ref this.m_TransformAllocator, alloc);
				this.m_Storage.SetTexel(vector2Int.x, vector2Int.y, xform.GetRow(0));
				this.m_Storage.SetTexel(vector2Int.x, vector2Int.y + 1, xform.GetRow(1));
				this.m_Storage.SetTexel(vector2Int.x, vector2Int.y + 2, xform.GetRow(2));
			}
			else
			{
				this.m_Transforms[UIRVEShaderInfoAllocator.AllocToConstantBufferIndex(alloc)] = new Transform3x4
				{
					v0 = xform.GetRow(0),
					v1 = xform.GetRow(1),
					v2 = xform.GetRow(2)
				};
			}
		}

		// Token: 0x06002373 RID: 9075 RVA: 0x0008A458 File Offset: 0x00088658
		public void SetClipRectValue(BMPAlloc alloc, Vector4 clipRect)
		{
			Debug.Assert(alloc.IsValid());
			bool vertexTexturingEnabled = this.m_VertexTexturingEnabled;
			if (vertexTexturingEnabled)
			{
				Vector2Int vector2Int = UIRVEShaderInfoAllocator.AllocToTexelCoord(ref this.m_ClipRectAllocator, alloc);
				this.m_Storage.SetTexel(vector2Int.x, vector2Int.y, clipRect);
			}
			else
			{
				this.m_ClipRects[UIRVEShaderInfoAllocator.AllocToConstantBufferIndex(alloc)] = clipRect;
			}
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x0008A4C4 File Offset: 0x000886C4
		public void SetOpacityValue(BMPAlloc alloc, float opacity)
		{
			Debug.Assert(alloc.IsValid());
			Vector2Int vector2Int = UIRVEShaderInfoAllocator.AllocToTexelCoord(ref this.m_OpacityAllocator, alloc);
			this.m_Storage.SetTexel(vector2Int.x, vector2Int.y, new Color(1f, 1f, 1f, opacity));
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x0008A51C File Offset: 0x0008871C
		public void SetColorValue(BMPAlloc alloc, Color color, bool isEditorContext)
		{
			Debug.Assert(alloc.IsValid());
			Vector2Int vector2Int = UIRVEShaderInfoAllocator.AllocToTexelCoord(ref this.m_ColorAllocator, alloc);
			bool flag = QualitySettings.activeColorSpace == ColorSpace.Linear && !isEditorContext;
			if (flag)
			{
				this.m_Storage.SetTexel(vector2Int.x, vector2Int.y, color.linear);
			}
			else
			{
				this.m_Storage.SetTexel(vector2Int.x, vector2Int.y, color);
			}
		}

		// Token: 0x06002376 RID: 9078 RVA: 0x0008A598 File Offset: 0x00088798
		public void SetTextCoreSettingValue(BMPAlloc alloc, TextCoreSettings settings, bool isEditorContext)
		{
			Debug.Assert(alloc.IsValid());
			Vector2Int vector2Int = UIRVEShaderInfoAllocator.AllocToTexelCoord(ref this.m_TextSettingsAllocator, alloc);
			Color color = new Color(-settings.underlayOffset.x, settings.underlayOffset.y, settings.underlaySoftness, settings.outlineWidth);
			bool flag = QualitySettings.activeColorSpace == ColorSpace.Linear && !isEditorContext;
			if (flag)
			{
				this.m_Storage.SetTexel(vector2Int.x, vector2Int.y, settings.faceColor.linear);
				this.m_Storage.SetTexel(vector2Int.x, vector2Int.y + 1, settings.outlineColor.linear);
				this.m_Storage.SetTexel(vector2Int.x, vector2Int.y + 2, settings.underlayColor.linear);
			}
			else
			{
				this.m_Storage.SetTexel(vector2Int.x, vector2Int.y, settings.faceColor);
				this.m_Storage.SetTexel(vector2Int.x, vector2Int.y + 1, settings.outlineColor);
				this.m_Storage.SetTexel(vector2Int.x, vector2Int.y + 2, settings.underlayColor);
			}
			this.m_Storage.SetTexel(vector2Int.x, vector2Int.y + 3, color);
		}

		// Token: 0x06002377 RID: 9079 RVA: 0x0008A6FA File Offset: 0x000888FA
		public void FreeTransform(BMPAlloc alloc)
		{
			Debug.Assert(alloc.IsValid());
			this.m_TransformAllocator.Free(alloc);
		}

		// Token: 0x06002378 RID: 9080 RVA: 0x0008A717 File Offset: 0x00088917
		public void FreeClipRect(BMPAlloc alloc)
		{
			Debug.Assert(alloc.IsValid());
			this.m_ClipRectAllocator.Free(alloc);
		}

		// Token: 0x06002379 RID: 9081 RVA: 0x0008A734 File Offset: 0x00088934
		public void FreeOpacity(BMPAlloc alloc)
		{
			Debug.Assert(alloc.IsValid());
			this.m_OpacityAllocator.Free(alloc);
		}

		// Token: 0x0600237A RID: 9082 RVA: 0x0008A751 File Offset: 0x00088951
		public void FreeColor(BMPAlloc alloc)
		{
			Debug.Assert(alloc.IsValid());
			this.m_ColorAllocator.Free(alloc);
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x0008A76E File Offset: 0x0008896E
		public void FreeTextCoreSettings(BMPAlloc alloc)
		{
			Debug.Assert(alloc.IsValid());
			this.m_TextSettingsAllocator.Free(alloc);
		}

		// Token: 0x0600237C RID: 9084 RVA: 0x0008A78C File Offset: 0x0008898C
		public Color32 TransformAllocToVertexData(BMPAlloc alloc)
		{
			Debug.Assert(UIRVEShaderInfoAllocator.pageWidth == 32 && UIRVEShaderInfoAllocator.pageHeight == 8);
			ushort num = 0;
			ushort num2 = 0;
			bool vertexTexturingEnabled = this.m_VertexTexturingEnabled;
			if (vertexTexturingEnabled)
			{
				this.m_TransformAllocator.GetAllocPageAtlasLocation(alloc.page, out num, out num2);
			}
			return new Color32((byte)(num >> 5), (byte)(num2 >> 3), (byte)((int)alloc.pageLine * UIRVEShaderInfoAllocator.pageWidth + (int)alloc.bitIndex), 0);
		}

		// Token: 0x0600237D RID: 9085 RVA: 0x0008A800 File Offset: 0x00088A00
		public Color32 ClipRectAllocToVertexData(BMPAlloc alloc)
		{
			Debug.Assert(UIRVEShaderInfoAllocator.pageWidth == 32 && UIRVEShaderInfoAllocator.pageHeight == 8);
			ushort num = 0;
			ushort num2 = 0;
			bool vertexTexturingEnabled = this.m_VertexTexturingEnabled;
			if (vertexTexturingEnabled)
			{
				this.m_ClipRectAllocator.GetAllocPageAtlasLocation(alloc.page, out num, out num2);
			}
			return new Color32((byte)(num >> 5), (byte)(num2 >> 3), (byte)((int)alloc.pageLine * UIRVEShaderInfoAllocator.pageWidth + (int)alloc.bitIndex), 0);
		}

		// Token: 0x0600237E RID: 9086 RVA: 0x0008A874 File Offset: 0x00088A74
		public Color32 OpacityAllocToVertexData(BMPAlloc alloc)
		{
			Debug.Assert(UIRVEShaderInfoAllocator.pageWidth == 32 && UIRVEShaderInfoAllocator.pageHeight == 8);
			ushort num;
			ushort num2;
			this.m_OpacityAllocator.GetAllocPageAtlasLocation(alloc.page, out num, out num2);
			return new Color32((byte)(num >> 5), (byte)(num2 >> 3), (byte)((int)alloc.pageLine * UIRVEShaderInfoAllocator.pageWidth + (int)alloc.bitIndex), 0);
		}

		// Token: 0x0600237F RID: 9087 RVA: 0x0008A8D8 File Offset: 0x00088AD8
		public Color32 ColorAllocToVertexData(BMPAlloc alloc)
		{
			Debug.Assert(UIRVEShaderInfoAllocator.pageWidth == 32 && UIRVEShaderInfoAllocator.pageHeight == 8);
			ushort num;
			ushort num2;
			this.m_ColorAllocator.GetAllocPageAtlasLocation(alloc.page, out num, out num2);
			return new Color32((byte)(num >> 5), (byte)(num2 >> 3), (byte)((int)alloc.pageLine * UIRVEShaderInfoAllocator.pageWidth + (int)alloc.bitIndex), 0);
		}

		// Token: 0x06002380 RID: 9088 RVA: 0x0008A93C File Offset: 0x00088B3C
		public Color32 TextCoreSettingsToVertexData(BMPAlloc alloc)
		{
			Debug.Assert(UIRVEShaderInfoAllocator.pageWidth == 32 && UIRVEShaderInfoAllocator.pageHeight == 8);
			ushort num;
			ushort num2;
			this.m_TextSettingsAllocator.GetAllocPageAtlasLocation(alloc.page, out num, out num2);
			return new Color32((byte)(num >> 5), (byte)(num2 >> 3), (byte)((int)alloc.pageLine * UIRVEShaderInfoAllocator.pageWidth + (int)alloc.bitIndex), 0);
		}

		// Token: 0x04001087 RID: 4231
		private BaseShaderInfoStorage m_Storage;

		// Token: 0x04001088 RID: 4232
		private BitmapAllocator32 m_TransformAllocator;

		// Token: 0x04001089 RID: 4233
		private BitmapAllocator32 m_ClipRectAllocator;

		// Token: 0x0400108A RID: 4234
		private BitmapAllocator32 m_OpacityAllocator;

		// Token: 0x0400108B RID: 4235
		private BitmapAllocator32 m_ColorAllocator;

		// Token: 0x0400108C RID: 4236
		private BitmapAllocator32 m_TextSettingsAllocator;

		// Token: 0x0400108D RID: 4237
		private bool m_StorageReallyCreated;

		// Token: 0x0400108E RID: 4238
		private bool m_VertexTexturingEnabled;

		// Token: 0x0400108F RID: 4239
		private NativeArray<Transform3x4> m_Transforms;

		// Token: 0x04001090 RID: 4240
		private NativeArray<Vector4> m_ClipRects;

		// Token: 0x04001091 RID: 4241
		internal static readonly Vector2Int identityTransformTexel = new Vector2Int(0, 0);

		// Token: 0x04001092 RID: 4242
		internal static readonly Vector2Int infiniteClipRectTexel = new Vector2Int(0, 32);

		// Token: 0x04001093 RID: 4243
		internal static readonly Vector2Int fullOpacityTexel = new Vector2Int(32, 32);

		// Token: 0x04001094 RID: 4244
		internal static readonly Vector2Int clearColorTexel = new Vector2Int(0, 40);

		// Token: 0x04001095 RID: 4245
		internal static readonly Vector2Int defaultTextCoreSettingsTexel = new Vector2Int(32, 0);

		// Token: 0x04001096 RID: 4246
		internal static readonly Matrix4x4 identityTransformValue = Matrix4x4.identity;

		// Token: 0x04001097 RID: 4247
		internal static readonly Vector4 identityTransformRow0Value = UIRVEShaderInfoAllocator.identityTransformValue.GetRow(0);

		// Token: 0x04001098 RID: 4248
		internal static readonly Vector4 identityTransformRow1Value = UIRVEShaderInfoAllocator.identityTransformValue.GetRow(1);

		// Token: 0x04001099 RID: 4249
		internal static readonly Vector4 identityTransformRow2Value = UIRVEShaderInfoAllocator.identityTransformValue.GetRow(2);

		// Token: 0x0400109A RID: 4250
		internal static readonly Vector4 infiniteClipRectValue = new Vector4(0f, 0f, 0f, 0f);

		// Token: 0x0400109B RID: 4251
		internal static readonly Vector4 fullOpacityValue = new Vector4(1f, 1f, 1f, 1f);

		// Token: 0x0400109C RID: 4252
		internal static readonly Vector4 clearColorValue = new Vector4(0f, 0f, 0f, 0f);

		// Token: 0x0400109D RID: 4253
		internal static readonly TextCoreSettings defaultTextCoreSettingsValue = new TextCoreSettings
		{
			faceColor = Color.white,
			outlineColor = Color.clear,
			outlineWidth = 0f,
			underlayColor = Color.clear,
			underlayOffset = Vector2.zero,
			underlaySoftness = 0f
		};

		// Token: 0x0400109E RID: 4254
		public static readonly BMPAlloc identityTransform;

		// Token: 0x0400109F RID: 4255
		public static readonly BMPAlloc infiniteClipRect;

		// Token: 0x040010A0 RID: 4256
		public static readonly BMPAlloc fullOpacity;

		// Token: 0x040010A1 RID: 4257
		public static readonly BMPAlloc clearColor;

		// Token: 0x040010A2 RID: 4258
		public static readonly BMPAlloc defaultTextCoreSettings;
	}
}
