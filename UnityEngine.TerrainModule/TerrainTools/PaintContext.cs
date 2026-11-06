using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Internal;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.TerrainUtils;

namespace UnityEngine.TerrainTools
{
	// Token: 0x02000022 RID: 34
	[MovedFrom("UnityEngine.Experimental.TerrainAPI")]
	public class PaintContext
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001CA RID: 458 RVA: 0x00005334 File Offset: 0x00003534
		public Terrain originTerrain { get; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001CB RID: 459 RVA: 0x0000533C File Offset: 0x0000353C
		public RectInt pixelRect { get; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001CC RID: 460 RVA: 0x00005344 File Offset: 0x00003544
		public int targetTextureWidth { get; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001CD RID: 461 RVA: 0x0000534C File Offset: 0x0000354C
		public int targetTextureHeight { get; }

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00005354 File Offset: 0x00003554
		public Vector2 pixelSize { get; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060001CF RID: 463 RVA: 0x0000535C File Offset: 0x0000355C
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x00005364 File Offset: 0x00003564
		public RenderTexture sourceRenderTexture { get; private set; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000536D File Offset: 0x0000356D
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x00005375 File Offset: 0x00003575
		public RenderTexture destinationRenderTexture { get; private set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x0000537E File Offset: 0x0000357E
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x00005386 File Offset: 0x00003586
		public RenderTexture oldRenderTexture { get; private set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x00005390 File Offset: 0x00003590
		public int terrainCount
		{
			get
			{
				return this.m_TerrainTiles.Count;
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x000053B0 File Offset: 0x000035B0
		public Terrain GetTerrain(int terrainIndex)
		{
			return this.m_TerrainTiles[terrainIndex].terrain;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x000053D4 File Offset: 0x000035D4
		public RectInt GetClippedPixelRectInTerrainPixels(int terrainIndex)
		{
			return this.m_TerrainTiles[terrainIndex].clippedTerrainPixels;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x000053F8 File Offset: 0x000035F8
		public RectInt GetClippedPixelRectInRenderTexturePixels(int terrainIndex)
		{
			return this.m_TerrainTiles[terrainIndex].clippedPCPixels;
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x0000541B File Offset: 0x0000361B
		public float heightWorldSpaceMin
		{
			get
			{
				return this.m_HeightWorldSpaceMin;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060001DA RID: 474 RVA: 0x00005423 File Offset: 0x00003623
		public float heightWorldSpaceSize
		{
			get
			{
				return this.m_HeightWorldSpaceMax - this.m_HeightWorldSpaceMin;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001DB RID: 475 RVA: 0x00005432 File Offset: 0x00003632
		public static float kNormalizedHeightScale
		{
			get
			{
				return 0.4999771f;
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060001DC RID: 476 RVA: 0x0000543C File Offset: 0x0000363C
		// (remove) Token: 0x060001DD RID: 477 RVA: 0x00005470 File Offset: 0x00003670
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal static event Action<PaintContext.ITerrainInfo, PaintContext.ToolAction, string> onTerrainTileBeforePaint;

		// Token: 0x060001DE RID: 478 RVA: 0x000054A4 File Offset: 0x000036A4
		internal static int ClampContextResolution(int resolution)
		{
			return Mathf.Clamp(resolution, 1, 8192);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x000054C4 File Offset: 0x000036C4
		public PaintContext(Terrain terrain, RectInt pixelRect, int targetTextureWidth, int targetTextureHeight, [DefaultValue("true")] bool sharedBoundaryTexel = true, [DefaultValue("true")] bool fillOutsideTerrain = true)
		{
			this.originTerrain = terrain;
			this.pixelRect = pixelRect;
			this.targetTextureWidth = targetTextureWidth;
			this.targetTextureHeight = targetTextureHeight;
			TerrainData terrainData = terrain.terrainData;
			this.pixelSize = new Vector2(terrainData.size.x / ((float)targetTextureWidth - (sharedBoundaryTexel ? 1f : 0f)), terrainData.size.z / ((float)targetTextureHeight - (sharedBoundaryTexel ? 1f : 0f)));
			this.FindTerrainTilesUnlimited(sharedBoundaryTexel, fillOutsideTerrain);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00005554 File Offset: 0x00003754
		public static PaintContext CreateFromBounds(Terrain terrain, Rect boundsInTerrainSpace, int inputTextureWidth, int inputTextureHeight, [DefaultValue("0")] int extraBorderPixels = 0, [DefaultValue("true")] bool sharedBoundaryTexel = true, [DefaultValue("true")] bool fillOutsideTerrain = true)
		{
			return new PaintContext(terrain, TerrainPaintUtility.CalcPixelRectFromBounds(terrain, boundsInTerrainSpace, inputTextureWidth, inputTextureHeight, extraBorderPixels, sharedBoundaryTexel), inputTextureWidth, inputTextureHeight, sharedBoundaryTexel, fillOutsideTerrain);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00005580 File Offset: 0x00003780
		private void FindTerrainTilesUnlimited(bool sharedBoundaryTexel, bool fillOutsideTerrain)
		{
			float minX = this.originTerrain.transform.position.x + this.pixelSize.x * (float)this.pixelRect.xMin;
			float minZ = this.originTerrain.transform.position.z + this.pixelSize.y * (float)this.pixelRect.yMin;
			float maxX = this.originTerrain.transform.position.x + this.pixelSize.x * (float)(this.pixelRect.xMax - 1);
			float maxZ = this.originTerrain.transform.position.z + this.pixelSize.y * (float)(this.pixelRect.yMax - 1);
			this.m_HeightWorldSpaceMin = this.originTerrain.GetPosition().y;
			this.m_HeightWorldSpaceMax = this.m_HeightWorldSpaceMin + this.originTerrain.terrainData.size.y;
			Predicate<Terrain> predicate = delegate(Terrain t)
			{
				float x = t.transform.position.x;
				float z = t.transform.position.z;
				float num4 = t.transform.position.x + t.terrainData.size.x;
				float num5 = t.transform.position.z + t.terrainData.size.z;
				return x <= maxX && num4 >= minX && z <= maxZ && num5 >= minZ;
			};
			TerrainMap terrainMap = TerrainMap.CreateFromConnectedNeighbors(this.originTerrain, predicate, false);
			this.m_TerrainTiles = new List<PaintContext.TerrainTile>();
			bool flag = terrainMap != null;
			if (flag)
			{
				foreach (KeyValuePair<TerrainTileCoord, Terrain> keyValuePair in terrainMap.terrainTiles)
				{
					TerrainTileCoord key = keyValuePair.Key;
					Terrain value = keyValuePair.Value;
					int num = key.tileX * (this.targetTextureWidth - (sharedBoundaryTexel ? 1 : 0));
					int num2 = key.tileZ * (this.targetTextureHeight - (sharedBoundaryTexel ? 1 : 0));
					RectInt rectInt = new RectInt(num, num2, this.targetTextureWidth, this.targetTextureHeight);
					bool flag2 = this.pixelRect.Overlaps(rectInt);
					if (flag2)
					{
						int num3 = (fillOutsideTerrain ? Mathf.Max(this.targetTextureWidth, this.targetTextureHeight) : 0);
						this.m_TerrainTiles.Add(PaintContext.TerrainTile.Make(value, num, num2, this.pixelRect, this.targetTextureWidth, this.targetTextureHeight, num3));
						this.m_HeightWorldSpaceMin = Mathf.Min(this.m_HeightWorldSpaceMin, value.GetPosition().y);
						this.m_HeightWorldSpaceMax = Mathf.Max(this.m_HeightWorldSpaceMax, value.GetPosition().y + value.terrainData.size.y);
					}
				}
			}
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000583C File Offset: 0x00003A3C
		public void CreateRenderTargets(RenderTextureFormat colorFormat)
		{
			int num = PaintContext.ClampContextResolution(this.pixelRect.width);
			int num2 = PaintContext.ClampContextResolution(this.pixelRect.height);
			bool flag = num != this.pixelRect.width || num2 != this.pixelRect.height;
			if (flag)
			{
				Debug.LogWarning(string.Format("\nTERRAIN EDITOR INTERNAL ERROR: An attempt to create a PaintContext with dimensions of {0}x{1} was made,\nwhereas the maximum supported resolution is {2}. The size has been clamped to {3}.", new object[]
				{
					this.pixelRect.width,
					this.pixelRect.height,
					8192,
					8192
				}));
			}
			this.sourceRenderTexture = RenderTexture.GetTemporary(num, num2, 16, colorFormat, RenderTextureReadWrite.Linear);
			this.destinationRenderTexture = RenderTexture.GetTemporary(num, num2, 0, colorFormat, RenderTextureReadWrite.Linear);
			this.sourceRenderTexture.wrapMode = TextureWrapMode.Clamp;
			this.sourceRenderTexture.filterMode = FilterMode.Point;
			this.oldRenderTexture = RenderTexture.active;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00005948 File Offset: 0x00003B48
		public void Cleanup(bool restoreRenderTexture = true)
		{
			if (restoreRenderTexture)
			{
				RenderTexture.active = this.oldRenderTexture;
			}
			RenderTexture.ReleaseTemporary(this.sourceRenderTexture);
			RenderTexture.ReleaseTemporary(this.destinationRenderTexture);
			this.sourceRenderTexture = null;
			this.destinationRenderTexture = null;
			this.oldRenderTexture = null;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00005998 File Offset: 0x00003B98
		private void GatherInternal(Func<PaintContext.ITerrainInfo, Texture> terrainToTexture, Color defaultColor, string operationName, Material blitMaterial = null, int blitPass = 0, Action<PaintContext.ITerrainInfo> beforeBlit = null, Action<PaintContext.ITerrainInfo> afterBlit = null)
		{
			bool flag = blitMaterial == null;
			if (flag)
			{
				blitMaterial = TerrainPaintUtility.GetBlitMaterial();
			}
			RenderTexture.active = this.sourceRenderTexture;
			GL.Clear(true, true, defaultColor);
			GL.PushMatrix();
			GL.LoadPixelMatrix(0f, (float)this.pixelRect.width, 0f, (float)this.pixelRect.height);
			for (int i = 0; i < this.m_TerrainTiles.Count; i++)
			{
				PaintContext.TerrainTile terrainTile = this.m_TerrainTiles[i];
				bool flag2 = !terrainTile.gatherEnable;
				if (!flag2)
				{
					Texture texture = terrainToTexture(terrainTile);
					bool flag3 = texture == null || !terrainTile.gatherEnable;
					if (!flag3)
					{
						bool flag4 = texture.width != this.targetTextureWidth || texture.height != this.targetTextureHeight;
						if (flag4)
						{
							Debug.LogWarning(operationName + " requires the same resolution texture for all Terrains - mismatched Terrains are ignored.", terrainTile.terrain);
						}
						else
						{
							if (beforeBlit != null)
							{
								beforeBlit(terrainTile);
							}
							bool flag5 = !terrainTile.gatherEnable;
							if (!flag5)
							{
								FilterMode filterMode = texture.filterMode;
								texture.filterMode = FilterMode.Point;
								blitMaterial.SetTexture("_MainTex", texture);
								blitMaterial.SetPass(blitPass);
								TerrainPaintUtility.DrawQuadPadded(terrainTile.clippedPCPixels, terrainTile.paddedPCPixels, terrainTile.clippedTerrainPixels, terrainTile.paddedTerrainPixels, texture);
								texture.filterMode = filterMode;
								if (afterBlit != null)
								{
									afterBlit(terrainTile);
								}
							}
						}
					}
				}
			}
			GL.PopMatrix();
			RenderTexture.active = this.oldRenderTexture;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00005B50 File Offset: 0x00003D50
		private void ScatterInternal(Func<PaintContext.ITerrainInfo, RenderTexture> terrainToRT, string operationName, Material blitMaterial = null, int blitPass = 0, Action<PaintContext.ITerrainInfo> beforeBlit = null, Action<PaintContext.ITerrainInfo> afterBlit = null)
		{
			RenderTexture active = RenderTexture.active;
			bool flag = blitMaterial == null;
			if (flag)
			{
				blitMaterial = TerrainPaintUtility.GetBlitMaterial();
			}
			for (int i = 0; i < this.m_TerrainTiles.Count; i++)
			{
				PaintContext.TerrainTile terrainTile = this.m_TerrainTiles[i];
				bool flag2 = !terrainTile.scatterEnable;
				if (!flag2)
				{
					RenderTexture renderTexture = terrainToRT(terrainTile);
					bool flag3 = renderTexture == null || !terrainTile.scatterEnable;
					if (!flag3)
					{
						bool flag4 = renderTexture.width != this.targetTextureWidth || renderTexture.height != this.targetTextureHeight;
						if (flag4)
						{
							Debug.LogWarning(operationName + " requires the same resolution for all Terrains - mismatched Terrains are ignored.", terrainTile.terrain);
						}
						else
						{
							if (beforeBlit != null)
							{
								beforeBlit(terrainTile);
							}
							bool flag5 = !terrainTile.scatterEnable;
							if (!flag5)
							{
								RenderTexture.active = renderTexture;
								GL.PushMatrix();
								GL.LoadPixelMatrix(0f, (float)renderTexture.width, 0f, (float)renderTexture.height);
								FilterMode filterMode = this.destinationRenderTexture.filterMode;
								this.destinationRenderTexture.filterMode = FilterMode.Point;
								blitMaterial.SetTexture("_MainTex", this.destinationRenderTexture);
								blitMaterial.SetPass(blitPass);
								TerrainPaintUtility.DrawQuad(terrainTile.clippedTerrainPixels, terrainTile.clippedPCPixels, this.destinationRenderTexture);
								this.destinationRenderTexture.filterMode = filterMode;
								GL.PopMatrix();
								if (afterBlit != null)
								{
									afterBlit(terrainTile);
								}
							}
						}
					}
				}
			}
			RenderTexture.active = active;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00005CF8 File Offset: 0x00003EF8
		public void Gather(Func<PaintContext.ITerrainInfo, Texture> terrainSource, Color defaultColor, Material blitMaterial = null, int blitPass = 0, Action<PaintContext.ITerrainInfo> beforeBlit = null, Action<PaintContext.ITerrainInfo> afterBlit = null)
		{
			bool flag = terrainSource != null;
			if (flag)
			{
				this.GatherInternal(terrainSource, defaultColor, "PaintContext.Gather", blitMaterial, blitPass, beforeBlit, afterBlit);
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00005D24 File Offset: 0x00003F24
		public void Scatter(Func<PaintContext.ITerrainInfo, RenderTexture> terrainDest, Material blitMaterial = null, int blitPass = 0, Action<PaintContext.ITerrainInfo> beforeBlit = null, Action<PaintContext.ITerrainInfo> afterBlit = null)
		{
			bool flag = terrainDest != null;
			if (flag)
			{
				this.ScatterInternal(terrainDest, "PaintContext.Scatter", blitMaterial, blitPass, beforeBlit, afterBlit);
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00005D50 File Offset: 0x00003F50
		public void GatherHeightmap()
		{
			Material blitMaterial = TerrainPaintUtility.GetHeightBlitMaterial();
			blitMaterial.SetFloat("_Height_Offset", 0f);
			blitMaterial.SetFloat("_Height_Scale", 1f);
			this.GatherInternal((PaintContext.ITerrainInfo t) => t.terrain.terrainData.heightmapTexture, new Color(0f, 0f, 0f, 0f), "PaintContext.GatherHeightmap", blitMaterial, 0, delegate(PaintContext.ITerrainInfo t)
			{
				blitMaterial.SetFloat("_Height_Offset", (t.terrain.GetPosition().y - this.heightWorldSpaceMin) / this.heightWorldSpaceSize * PaintContext.kNormalizedHeightScale);
				blitMaterial.SetFloat("_Height_Scale", t.terrain.terrainData.size.y / this.heightWorldSpaceSize);
			}, null);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00005DFC File Offset: 0x00003FFC
		public void ScatterHeightmap(string editorUndoName)
		{
			Material blitMaterial = TerrainPaintUtility.GetHeightBlitMaterial();
			blitMaterial.SetFloat("_Height_Offset", 0f);
			blitMaterial.SetFloat("_Height_Scale", 1f);
			this.ScatterInternal((PaintContext.ITerrainInfo t) => t.terrain.terrainData.heightmapTexture, "PaintContext.ScatterHeightmap", blitMaterial, 0, delegate(PaintContext.ITerrainInfo t)
			{
				Action<PaintContext.ITerrainInfo, PaintContext.ToolAction, string> action = PaintContext.onTerrainTileBeforePaint;
				if (action != null)
				{
					action(t, PaintContext.ToolAction.PaintHeightmap, editorUndoName);
				}
				blitMaterial.SetFloat("_Height_Offset", (this.heightWorldSpaceMin - t.terrain.GetPosition().y) / t.terrain.terrainData.size.y * PaintContext.kNormalizedHeightScale);
				blitMaterial.SetFloat("_Height_Scale", this.heightWorldSpaceSize / t.terrain.terrainData.size.y);
			}, delegate(PaintContext.ITerrainInfo t)
			{
				TerrainHeightmapSyncControl terrainHeightmapSyncControl = (t.terrain.drawInstanced ? TerrainHeightmapSyncControl.None : TerrainHeightmapSyncControl.HeightAndLod);
				t.terrain.terrainData.DirtyHeightmapRegion(t.clippedTerrainPixels, terrainHeightmapSyncControl);
				PaintContext.OnTerrainPainted(t, PaintContext.ToolAction.PaintHeightmap);
			});
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00005EB4 File Offset: 0x000040B4
		public void GatherHoles()
		{
			this.GatherInternal((PaintContext.ITerrainInfo t) => t.terrain.terrainData.holesTexture, new Color(0f, 0f, 0f, 0f), "PaintContext.GatherHoles", null, 0, null, null);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00005F0C File Offset: 0x0000410C
		public void ScatterHoles(string editorUndoName)
		{
			this.ScatterInternal(delegate(PaintContext.ITerrainInfo t)
			{
				Action<PaintContext.ITerrainInfo, PaintContext.ToolAction, string> action = PaintContext.onTerrainTileBeforePaint;
				if (action != null)
				{
					action(t, PaintContext.ToolAction.PaintHoles, editorUndoName);
				}
				t.terrain.terrainData.CopyActiveRenderTextureToTexture(TerrainData.HolesTextureName, 0, t.clippedPCPixels, t.clippedTerrainPixels.min, true);
				PaintContext.OnTerrainPainted(t, PaintContext.ToolAction.PaintHoles);
				return null;
			}, "PaintContext.ScatterHoles", null, 0, null, null);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00005F44 File Offset: 0x00004144
		public void GatherNormals()
		{
			this.GatherInternal((PaintContext.ITerrainInfo t) => t.terrain.normalmapTexture, new Color(0.5f, 0.5f, 0.5f, 0.5f), "PaintContext.GatherNormals", null, 0, null, null);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00005F9C File Offset: 0x0000419C
		private PaintContext.SplatmapUserData GetTerrainLayerUserData(PaintContext.ITerrainInfo context, TerrainLayer terrainLayer = null, bool addLayerIfDoesntExist = false)
		{
			PaintContext.SplatmapUserData splatmapUserData = context.userData as PaintContext.SplatmapUserData;
			bool flag = splatmapUserData != null;
			if (flag)
			{
				bool flag2 = terrainLayer == null || terrainLayer == splatmapUserData.terrainLayer;
				if (flag2)
				{
					return splatmapUserData;
				}
				splatmapUserData = null;
			}
			bool flag3 = splatmapUserData == null;
			if (flag3)
			{
				int num = -1;
				bool flag4 = terrainLayer != null;
				if (flag4)
				{
					num = TerrainPaintUtility.FindTerrainLayerIndex(context.terrain, terrainLayer);
					bool flag5 = num == -1 && addLayerIfDoesntExist;
					if (flag5)
					{
						Action<PaintContext.ITerrainInfo, PaintContext.ToolAction, string> action = PaintContext.onTerrainTileBeforePaint;
						if (action != null)
						{
							action(context, PaintContext.ToolAction.AddTerrainLayer, "Adding Terrain Layer");
						}
						num = TerrainPaintUtility.AddTerrainLayer(context.terrain, terrainLayer);
					}
				}
				bool flag6 = num != -1;
				if (flag6)
				{
					splatmapUserData = new PaintContext.SplatmapUserData();
					splatmapUserData.terrainLayer = terrainLayer;
					splatmapUserData.terrainLayerIndex = num;
					splatmapUserData.mapIndex = num >> 2;
					splatmapUserData.channelIndex = num & 3;
				}
				context.userData = splatmapUserData;
			}
			return splatmapUserData;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00006090 File Offset: 0x00004290
		public void GatherAlphamap(TerrainLayer inputLayer, bool addLayerIfDoesntExist = true)
		{
			bool flag = inputLayer == null;
			if (!flag)
			{
				Material copyTerrainLayerMaterial = TerrainPaintUtility.GetCopyTerrainLayerMaterial();
				Vector4[] layerMasks = new Vector4[]
				{
					new Vector4(1f, 0f, 0f, 0f),
					new Vector4(0f, 1f, 0f, 0f),
					new Vector4(0f, 0f, 1f, 0f),
					new Vector4(0f, 0f, 0f, 1f)
				};
				this.GatherInternal(delegate(PaintContext.ITerrainInfo t)
				{
					PaintContext.SplatmapUserData terrainLayerUserData = this.GetTerrainLayerUserData(t, inputLayer, addLayerIfDoesntExist);
					bool flag2 = terrainLayerUserData != null;
					Texture texture;
					if (flag2)
					{
						texture = TerrainPaintUtility.GetTerrainAlphaMapChecked(t.terrain, terrainLayerUserData.mapIndex);
					}
					else
					{
						texture = null;
					}
					return texture;
				}, new Color(0f, 0f, 0f, 0f), "PaintContext.GatherAlphamap", copyTerrainLayerMaterial, 0, delegate(PaintContext.ITerrainInfo t)
				{
					PaintContext.SplatmapUserData terrainLayerUserData2 = this.GetTerrainLayerUserData(t, null, false);
					bool flag3 = terrainLayerUserData2 == null;
					if (!flag3)
					{
						copyTerrainLayerMaterial.SetVector("_LayerMask", layerMasks[terrainLayerUserData2.channelIndex]);
					}
				}, null);
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x000061AC File Offset: 0x000043AC
		public void ScatterAlphamap(string editorUndoName)
		{
			Vector4[] layerMasks = new Vector4[]
			{
				new Vector4(1f, 0f, 0f, 0f),
				new Vector4(0f, 1f, 0f, 0f),
				new Vector4(0f, 0f, 1f, 0f),
				new Vector4(0f, 0f, 0f, 1f)
			};
			Material copyTerrainLayerMaterial = TerrainPaintUtility.GetCopyTerrainLayerMaterial();
			RenderTexture tempTarget = RenderTexture.GetTemporary(new RenderTextureDescriptor(this.destinationRenderTexture.width, this.destinationRenderTexture.height, GraphicsFormat.R8G8B8A8_UNorm, GraphicsFormat.None)
			{
				sRGB = false,
				useMipMap = false,
				autoGenerateMips = false
			});
			this.ScatterInternal(delegate(PaintContext.ITerrainInfo t)
			{
				PaintContext.SplatmapUserData terrainLayerUserData = this.GetTerrainLayerUserData(t, null, false);
				bool flag = terrainLayerUserData != null;
				if (flag)
				{
					Action<PaintContext.ITerrainInfo, PaintContext.ToolAction, string> action = PaintContext.onTerrainTileBeforePaint;
					if (action != null)
					{
						action(t, PaintContext.ToolAction.PaintTexture, editorUndoName);
					}
					int mapIndex = terrainLayerUserData.mapIndex;
					int channelIndex = terrainLayerUserData.channelIndex;
					Texture2D texture2D = t.terrain.terrainData.alphamapTextures[mapIndex];
					this.destinationRenderTexture.filterMode = FilterMode.Point;
					this.sourceRenderTexture.filterMode = FilterMode.Point;
					for (int i = 0; i <= t.terrain.terrainData.alphamapTextureCount; i++)
					{
						bool flag2 = i == mapIndex;
						if (!flag2)
						{
							int num = ((i == t.terrain.terrainData.alphamapTextureCount) ? mapIndex : i);
							Texture2D texture2D2 = t.terrain.terrainData.alphamapTextures[num];
							bool flag3 = texture2D2.width != this.targetTextureWidth || texture2D2.height != this.targetTextureHeight;
							if (flag3)
							{
								Debug.LogWarning("PaintContext alphamap operations must use the same resolution for all Terrains - mismatched Terrains are ignored.", t.terrain);
							}
							else
							{
								RenderTexture.active = tempTarget;
								GL.PushMatrix();
								GL.LoadPixelMatrix(0f, (float)tempTarget.width, 0f, (float)tempTarget.height);
								copyTerrainLayerMaterial.SetTexture("_MainTex", this.destinationRenderTexture);
								copyTerrainLayerMaterial.SetTexture("_OldAlphaMapTexture", this.sourceRenderTexture);
								copyTerrainLayerMaterial.SetTexture("_OriginalTargetAlphaMap", texture2D);
								copyTerrainLayerMaterial.SetTexture("_AlphaMapTexture", texture2D2);
								copyTerrainLayerMaterial.SetVector("_LayerMask", (num == mapIndex) ? layerMasks[channelIndex] : Vector4.zero);
								copyTerrainLayerMaterial.SetVector("_OriginalTargetAlphaMask", layerMasks[channelIndex]);
								copyTerrainLayerMaterial.SetPass(1);
								TerrainPaintUtility.DrawQuad2(t.clippedPCPixels, t.clippedPCPixels, this.destinationRenderTexture, t.clippedTerrainPixels, texture2D2);
								GL.PopMatrix();
								t.terrain.terrainData.CopyActiveRenderTextureToTexture(TerrainData.AlphamapTextureName, num, t.clippedPCPixels, t.clippedTerrainPixels.min, true);
							}
						}
					}
					RenderTexture.active = null;
					PaintContext.OnTerrainPainted(t, PaintContext.ToolAction.PaintTexture);
				}
				return null;
			}, "PaintContext.ScatterAlphamap", copyTerrainLayerMaterial, 0, null, null);
			RenderTexture.ReleaseTemporary(tempTarget);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x000062D8 File Offset: 0x000044D8
		private static void OnTerrainPainted(PaintContext.ITerrainInfo tile, PaintContext.ToolAction action)
		{
			for (int i = 0; i < PaintContext.s_PaintedTerrain.Count; i++)
			{
				bool flag = tile.terrain == PaintContext.s_PaintedTerrain[i].terrain;
				if (flag)
				{
					PaintContext.PaintedTerrain paintedTerrain = PaintContext.s_PaintedTerrain[i];
					paintedTerrain.action |= action;
					PaintContext.s_PaintedTerrain[i] = paintedTerrain;
					return;
				}
			}
			PaintContext.s_PaintedTerrain.Add(new PaintContext.PaintedTerrain
			{
				terrain = tile.terrain,
				action = action
			});
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00006374 File Offset: 0x00004574
		public static void ApplyDelayedActions()
		{
			for (int i = 0; i < PaintContext.s_PaintedTerrain.Count; i++)
			{
				PaintContext.PaintedTerrain paintedTerrain = PaintContext.s_PaintedTerrain[i];
				TerrainData terrainData = paintedTerrain.terrain.terrainData;
				bool flag = terrainData == null;
				if (!flag)
				{
					bool flag2 = (paintedTerrain.action & PaintContext.ToolAction.PaintHeightmap) > PaintContext.ToolAction.None;
					if (flag2)
					{
						terrainData.SyncHeightmap();
					}
					bool flag3 = (paintedTerrain.action & PaintContext.ToolAction.PaintHoles) > PaintContext.ToolAction.None;
					if (flag3)
					{
						terrainData.SyncTexture(TerrainData.HolesTextureName);
					}
					bool flag4 = (paintedTerrain.action & PaintContext.ToolAction.PaintTexture) > PaintContext.ToolAction.None;
					if (flag4)
					{
						terrainData.SetBaseMapDirty();
						terrainData.SyncTexture(TerrainData.AlphamapTextureName);
					}
					paintedTerrain.terrain.editorRenderFlags = TerrainRenderFlags.all;
				}
			}
			PaintContext.s_PaintedTerrain.Clear();
		}

		// Token: 0x04000090 RID: 144
		private List<PaintContext.TerrainTile> m_TerrainTiles;

		// Token: 0x04000091 RID: 145
		private float m_HeightWorldSpaceMin;

		// Token: 0x04000092 RID: 146
		private float m_HeightWorldSpaceMax;

		// Token: 0x04000094 RID: 148
		internal const int k_MinimumResolution = 1;

		// Token: 0x04000095 RID: 149
		internal const int k_MaximumResolution = 8192;

		// Token: 0x04000096 RID: 150
		private static List<PaintContext.PaintedTerrain> s_PaintedTerrain = new List<PaintContext.PaintedTerrain>();

		// Token: 0x02000023 RID: 35
		public interface ITerrainInfo
		{
			// Token: 0x170000A7 RID: 167
			// (get) Token: 0x060001F3 RID: 499
			Terrain terrain { get; }

			// Token: 0x170000A8 RID: 168
			// (get) Token: 0x060001F4 RID: 500
			RectInt clippedTerrainPixels { get; }

			// Token: 0x170000A9 RID: 169
			// (get) Token: 0x060001F5 RID: 501
			RectInt clippedPCPixels { get; }

			// Token: 0x170000AA RID: 170
			// (get) Token: 0x060001F6 RID: 502
			RectInt paddedTerrainPixels { get; }

			// Token: 0x170000AB RID: 171
			// (get) Token: 0x060001F7 RID: 503
			RectInt paddedPCPixels { get; }

			// Token: 0x170000AC RID: 172
			// (get) Token: 0x060001F8 RID: 504
			// (set) Token: 0x060001F9 RID: 505
			bool gatherEnable { get; set; }

			// Token: 0x170000AD RID: 173
			// (get) Token: 0x060001FA RID: 506
			// (set) Token: 0x060001FB RID: 507
			bool scatterEnable { get; set; }

			// Token: 0x170000AE RID: 174
			// (get) Token: 0x060001FC RID: 508
			// (set) Token: 0x060001FD RID: 509
			object userData { get; set; }
		}

		// Token: 0x02000024 RID: 36
		private class TerrainTile : PaintContext.ITerrainInfo
		{
			// Token: 0x170000AF RID: 175
			// (get) Token: 0x060001FE RID: 510 RVA: 0x00006450 File Offset: 0x00004650
			Terrain PaintContext.ITerrainInfo.terrain
			{
				get
				{
					return this.terrain;
				}
			}

			// Token: 0x170000B0 RID: 176
			// (get) Token: 0x060001FF RID: 511 RVA: 0x00006468 File Offset: 0x00004668
			RectInt PaintContext.ITerrainInfo.clippedTerrainPixels
			{
				get
				{
					return this.clippedTerrainPixels;
				}
			}

			// Token: 0x170000B1 RID: 177
			// (get) Token: 0x06000200 RID: 512 RVA: 0x00006480 File Offset: 0x00004680
			RectInt PaintContext.ITerrainInfo.clippedPCPixels
			{
				get
				{
					return this.clippedPCPixels;
				}
			}

			// Token: 0x170000B2 RID: 178
			// (get) Token: 0x06000201 RID: 513 RVA: 0x00006498 File Offset: 0x00004698
			RectInt PaintContext.ITerrainInfo.paddedTerrainPixels
			{
				get
				{
					return this.paddedTerrainPixels;
				}
			}

			// Token: 0x170000B3 RID: 179
			// (get) Token: 0x06000202 RID: 514 RVA: 0x000064B0 File Offset: 0x000046B0
			RectInt PaintContext.ITerrainInfo.paddedPCPixels
			{
				get
				{
					return this.paddedPCPixels;
				}
			}

			// Token: 0x170000B4 RID: 180
			// (get) Token: 0x06000203 RID: 515 RVA: 0x000064C8 File Offset: 0x000046C8
			// (set) Token: 0x06000204 RID: 516 RVA: 0x000064E0 File Offset: 0x000046E0
			bool PaintContext.ITerrainInfo.gatherEnable
			{
				get
				{
					return this.gatherEnable;
				}
				set
				{
					this.gatherEnable = value;
				}
			}

			// Token: 0x170000B5 RID: 181
			// (get) Token: 0x06000205 RID: 517 RVA: 0x000064EC File Offset: 0x000046EC
			// (set) Token: 0x06000206 RID: 518 RVA: 0x00006504 File Offset: 0x00004704
			bool PaintContext.ITerrainInfo.scatterEnable
			{
				get
				{
					return this.scatterEnable;
				}
				set
				{
					this.scatterEnable = value;
				}
			}

			// Token: 0x170000B6 RID: 182
			// (get) Token: 0x06000207 RID: 519 RVA: 0x00006510 File Offset: 0x00004710
			// (set) Token: 0x06000208 RID: 520 RVA: 0x00006528 File Offset: 0x00004728
			object PaintContext.ITerrainInfo.userData
			{
				get
				{
					return this.userData;
				}
				set
				{
					this.userData = value;
				}
			}

			// Token: 0x06000209 RID: 521 RVA: 0x00006534 File Offset: 0x00004734
			public static PaintContext.TerrainTile Make(Terrain terrain, int tileOriginPixelsX, int tileOriginPixelsY, RectInt pixelRect, int targetTextureWidth, int targetTextureHeight, int edgePad = 0)
			{
				PaintContext.TerrainTile terrainTile = new PaintContext.TerrainTile
				{
					terrain = terrain,
					gatherEnable = true,
					scatterEnable = true,
					tileOriginPixels = new Vector2Int(tileOriginPixelsX, tileOriginPixelsY),
					clippedTerrainPixels = new RectInt
					{
						x = Mathf.Max(0, pixelRect.x - tileOriginPixelsX),
						y = Mathf.Max(0, pixelRect.y - tileOriginPixelsY),
						xMax = Mathf.Min(targetTextureWidth, pixelRect.xMax - tileOriginPixelsX),
						yMax = Mathf.Min(targetTextureHeight, pixelRect.yMax - tileOriginPixelsY)
					}
				};
				terrainTile.clippedPCPixels = new RectInt(terrainTile.clippedTerrainPixels.x + terrainTile.tileOriginPixels.x - pixelRect.x, terrainTile.clippedTerrainPixels.y + terrainTile.tileOriginPixels.y - pixelRect.y, terrainTile.clippedTerrainPixels.width, terrainTile.clippedTerrainPixels.height);
				int num = ((terrain.leftNeighbor == null) ? edgePad : 0);
				int num2 = ((terrain.rightNeighbor == null) ? edgePad : 0);
				int num3 = ((terrain.bottomNeighbor == null) ? edgePad : 0);
				int num4 = ((terrain.topNeighbor == null) ? edgePad : 0);
				terrainTile.paddedTerrainPixels = new RectInt
				{
					x = Mathf.Max(-num, pixelRect.x - tileOriginPixelsX - num),
					y = Mathf.Max(-num3, pixelRect.y - tileOriginPixelsY - num3),
					xMax = Mathf.Min(targetTextureWidth + num2, pixelRect.xMax - tileOriginPixelsX + num2),
					yMax = Mathf.Min(targetTextureHeight + num4, pixelRect.yMax - tileOriginPixelsY + num4)
				};
				terrainTile.paddedPCPixels = new RectInt(terrainTile.clippedPCPixels.min + (terrainTile.paddedTerrainPixels.min - terrainTile.clippedTerrainPixels.min), terrainTile.clippedPCPixels.size + (terrainTile.paddedTerrainPixels.size - terrainTile.clippedTerrainPixels.size));
				bool flag = terrainTile.clippedTerrainPixels.width == 0 || terrainTile.clippedTerrainPixels.height == 0;
				if (flag)
				{
					terrainTile.gatherEnable = false;
					terrainTile.scatterEnable = false;
					Debug.LogError("PaintContext.ClipTerrainTiles found 0 content rect");
				}
				return terrainTile;
			}

			// Token: 0x04000097 RID: 151
			public Terrain terrain;

			// Token: 0x04000098 RID: 152
			public Vector2Int tileOriginPixels;

			// Token: 0x04000099 RID: 153
			public RectInt clippedTerrainPixels;

			// Token: 0x0400009A RID: 154
			public RectInt clippedPCPixels;

			// Token: 0x0400009B RID: 155
			public RectInt paddedTerrainPixels;

			// Token: 0x0400009C RID: 156
			public RectInt paddedPCPixels;

			// Token: 0x0400009D RID: 157
			public object userData;

			// Token: 0x0400009E RID: 158
			public bool gatherEnable;

			// Token: 0x0400009F RID: 159
			public bool scatterEnable;
		}

		// Token: 0x02000025 RID: 37
		private class SplatmapUserData
		{
			// Token: 0x040000A0 RID: 160
			public TerrainLayer terrainLayer;

			// Token: 0x040000A1 RID: 161
			public int terrainLayerIndex;

			// Token: 0x040000A2 RID: 162
			public int mapIndex;

			// Token: 0x040000A3 RID: 163
			public int channelIndex;
		}

		// Token: 0x02000026 RID: 38
		[Flags]
		internal enum ToolAction
		{
			// Token: 0x040000A5 RID: 165
			None = 0,
			// Token: 0x040000A6 RID: 166
			PaintHeightmap = 1,
			// Token: 0x040000A7 RID: 167
			PaintTexture = 2,
			// Token: 0x040000A8 RID: 168
			PaintHoles = 4,
			// Token: 0x040000A9 RID: 169
			AddTerrainLayer = 8
		}

		// Token: 0x02000027 RID: 39
		private struct PaintedTerrain
		{
			// Token: 0x040000AA RID: 170
			public Terrain terrain;

			// Token: 0x040000AB RID: 171
			public PaintContext.ToolAction action;
		}
	}
}
