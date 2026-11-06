using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Logging;
using DA_Assets.SVGMeshUnity;
using DA_Assets.Tools;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x0200034B RID: 843
	[Serializable]
	public class SpriteGenerator : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x0600165D RID: 5725 RVA: 0x000928C0 File Offset: 0x00090AC0
		public async Task GenerateSprites(List<FObject> fobjects)
		{
			List<FObject> generative = fobjects.Where<FObject>((FObject x) => x.Data.NeedGenerate).ToList<FObject>();
			if (!generative.IsEmpty<FObject>())
			{
				if (this.camera == null)
				{
					GameObject gameObject = MonoBehExtensions.CreateEmptyGameObject(null, null);
					gameObject.name = "SpriteGeneratorCamera";
					gameObject.TryAddComponent<Camera>(out this.camera, false);
					this.camera.orthographic = true;
					this.camera.backgroundColor = new Color(0f, 0f, 0f, 0f);
					this.camera.clearFlags = CameraClearFlags.Color;
				}
				int generatedCount = 0;
				DACycles.ForEach<FObject>(generative, delegate(FObject fobject)
				{
					FcuLogger.Debug(string.Format("GenerateSprites | {0} | {1}", fobject.Data.NameHierarchy, fobject.Data.NeedGenerate), FcuDebugSettingsFlags.LogDefault);
					try
					{
						Texture2D texture2D = null;
						Texture2D texture2D2 = null;
						Texture2D texture2D3 = null;
						FGraphic graphic = fobject.Data.Graphic;
						if (!graphic.HasFill && graphic.HasStroke && !fobject.ContainsRoundedCorners())
						{
							IndividualStrokeWeights individualStrokeWeights = fobject.IndividualStrokeWeights;
							if (individualStrokeWeights.IsDefault<IndividualStrokeWeights>())
							{
								individualStrokeWeights = new IndividualStrokeWeights
								{
									Left = fobject.StrokeWeight,
									Right = fobject.StrokeWeight,
									Top = fobject.StrokeWeight,
									Bottom = fobject.StrokeWeight
								};
							}
							if (individualStrokeWeights.Left != 0f && individualStrokeWeights.Left < 1f)
							{
								individualStrokeWeights.Left = 1f;
							}
							if (individualStrokeWeights.Right != 0f && individualStrokeWeights.Right < 1f)
							{
								individualStrokeWeights.Right = 1f;
							}
							if (individualStrokeWeights.Top != 0f && individualStrokeWeights.Top < 1f)
							{
								individualStrokeWeights.Top = 1f;
							}
							if (individualStrokeWeights.Bottom != 0f && individualStrokeWeights.Bottom < 1f)
							{
								individualStrokeWeights.Bottom = 1f;
							}
							float imageScale = this.monoBeh.Settings.ImageSpritesSettings.ImageScale;
							int num = (int)(fobject.Size.x * imageScale);
							int num2 = (int)(fobject.Size.y * imageScale);
							int num3 = (int)(individualStrokeWeights.Left * imageScale);
							int num4 = (int)(individualStrokeWeights.Right * imageScale);
							int num5 = (int)(individualStrokeWeights.Top * imageScale);
							int num6 = (int)(individualStrokeWeights.Bottom * imageScale);
							texture2D3 = TextureBorderDrawer.CreateTextureWithBorder(num, num2, num3, num4, num5, num6, Color.white);
						}
						else
						{
							Vector2Int vector2Int = default(Vector2Int);
							if (graphic.HasFill)
							{
								Vector2 vector = fobject.Size;
								vector -= new Vector2Int(1, 0);
								Vector2Int vector2Int2;
								vector.IsSupportedRenderSize(this.monoBeh.Settings.ImageSpritesSettings.ImageScale, out vector2Int, out vector2Int2);
								string path = fobject.FillGeometry[0].Path;
								Color color;
								if (graphic.HasFill && graphic.HasStroke)
								{
									color = graphic.Fill.SolidPaint.Color;
								}
								else
								{
									color = Color.white;
								}
								texture2D = this.GenerateTexture(path, vector, vector2Int2, color);
							}
							if (graphic.HasStroke)
							{
								Vector2 vector2 = fobject.Size;
								vector2 += new Vector2(fobject.StrokeWeight * 2f, fobject.StrokeWeight * 2f);
								Vector2Int vector2Int3;
								vector2.IsSupportedRenderSize(this.monoBeh.Settings.ImageSpritesSettings.ImageScale, out vector2Int, out vector2Int3);
								string path2 = fobject.StrokeGeometry[0].Path;
								Color color2;
								if (graphic.HasFill && graphic.HasStroke)
								{
									color2 = graphic.Stroke.SolidPaint.Color;
								}
								else
								{
									color2 = Color.white;
								}
								texture2D2 = this.GenerateTexture(path2, vector2, vector2Int3, color2);
							}
							FcuLogger.Debug(string.Format("GenerateSprites | {0} | hasFills: {1} | hasStrokes: {2}", fobject.Data.NameHierarchy, graphic.HasFill, graphic.HasStroke), FcuDebugSettingsFlags.LogDefault);
							if (texture2D != null && texture2D2 != null)
							{
								texture2D3 = texture2D2.Merge(texture2D);
							}
							else if (texture2D2 != null)
							{
								texture2D3 = texture2D2;
							}
							else if (texture2D != null)
							{
								texture2D3 = texture2D;
							}
							if (texture2D3 == null)
							{
								throw new Exception("finalTexture is null");
							}
							texture2D3.Blur(this.monoBeh.Settings.ImageSpritesSettings.ImageScale / this.blurCoof);
							texture2D3.Resize(vector2Int, 0, this.filterMode, this.renderTextureFormat);
						}
						byte[] array = texture2D3.EncodeToPNG();
						texture2D3.Destroy();
						File.WriteAllBytes(fobject.Data.SpritePath, array);
					}
					catch (Exception ex)
					{
						FcuLogger.Debug(string.Format("Can't generate '{0}'\n{1}", fobject.Data.NameHierarchy, ex), FcuDebugSettingsFlags.LogError);
						fobject.Data.FcuImageType = FcuImageType.Drawable;
					}
					int generatedCount2 = generatedCount;
					generatedCount = generatedCount2 + 1;
				}, this.spriteGenerationDelay, 1);
				for (;;)
				{
					DALogger.Log(FcuLocKey.log_generating_sprites.Localize(new object[] { generatedCount, generative.Count }));
					if (generatedCount >= generative.Count)
					{
						break;
					}
					await Task.Delay(1000);
				}
				this.camera.gameObject.Destroy();
			}
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x0009290C File Offset: 0x00090B0C
		public Texture2D GenerateTexture(string svgPath, Vector2 sourceSize, Vector2Int bakeResolution, Color color)
		{
			GameObject gameObject = MonoBehExtensions.CreateEmptyGameObject(null, null);
			gameObject.name = sourceSize.ToString();
			gameObject.transform.position = new Vector3(-20000f, -20000f, 0f);
			Texture2D texture2D;
			try
			{
				this.GenerateMesh(gameObject, sourceSize, svgPath);
				texture2D = this.BakeTexture(gameObject, bakeResolution, color);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				gameObject.Destroy();
			}
			return texture2D;
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x00092990 File Offset: 0x00090B90
		private void GenerateMesh(GameObject meshObject, Vector2 objectSize, string svgPath)
		{
			this.meshFilter = meshObject.AddComponent<MeshFilter>();
			this.meshRenderer = meshObject.AddComponent<MeshRenderer>();
			SVGMesh svgmesh = new SVGMesh();
			svgmesh.Init((float)this.meshUpscaleFactor, false, true, false, false);
			SVGData svgdata = new SVGData();
			svgdata.Path(svgPath);
			svgmesh.Fill(svgdata, this.meshFilter);
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x000929E4 File Offset: 0x00090BE4
		private Texture2D BakeTexture(GameObject meshObject, Vector2Int bakeResolution, Color color)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(bakeResolution.x, bakeResolution.y, 8, this.renderTextureFormat);
			temporary.antiAliasing = this.renderAntialiasing;
			temporary.filterMode = this.filterMode;
			this.camera.targetTexture = temporary;
			this.camera.SetToObject(meshObject);
			this.camera.Render();
			Texture2D texture2D = new Texture2D(bakeResolution.x, bakeResolution.y, this.textureFormat, false);
			texture2D.filterMode = this.filterMode;
			RenderTexture.active = temporary;
			texture2D.ReadPixels(new Rect(0f, 0f, (float)bakeResolution.x, (float)bakeResolution.y), 0, 0);
			texture2D.Apply();
			texture2D.Colorize(color);
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.active = null;
			this.camera.targetTexture = null;
			return texture2D;
		}

		// Token: 0x04001815 RID: 6165
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x04001816 RID: 6166
		[SerializeField]
		private MeshRenderer meshRenderer;

		// Token: 0x04001817 RID: 6167
		[SerializeField]
		private MeshFilter meshFilter;

		// Token: 0x04001818 RID: 6168
		[SerializeField]
		private Camera camera;

		// Token: 0x04001819 RID: 6169
		private float spriteGenerationDelay = 0.25f;

		// Token: 0x0400181A RID: 6170
		private int meshUpscaleFactor = 16;

		// Token: 0x0400181B RID: 6171
		private int renderAntialiasing = 8;

		// Token: 0x0400181C RID: 6172
		private float blurCoof = 10f;

		// Token: 0x0400181D RID: 6173
		private FilterMode filterMode = FilterMode.Bilinear;

		// Token: 0x0400181E RID: 6174
		private TextureFormat textureFormat = TextureFormat.ARGB32;

		// Token: 0x0400181F RID: 6175
		private RenderTextureFormat renderTextureFormat;
	}
}
