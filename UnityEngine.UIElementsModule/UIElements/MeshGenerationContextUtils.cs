using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002AD RID: 685
	internal static class MeshGenerationContextUtils
	{
		// Token: 0x060013B4 RID: 5044 RVA: 0x0004528B File Offset: 0x0004348B
		public static void Rectangle(this MeshGenerationContext mgc, MeshGenerationContextUtils.RectangleParams rectParams)
		{
			mgc.painter.DrawRectangle(rectParams);
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x0004529B File Offset: 0x0004349B
		public static void Border(this MeshGenerationContext mgc, MeshGenerationContextUtils.BorderParams borderParams)
		{
			mgc.painter.DrawBorder(borderParams);
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x000452AC File Offset: 0x000434AC
		public static void Text(this MeshGenerationContext mgc, TextElement te)
		{
			bool flag = TextUtilities.IsFontAssigned(te);
			if (flag)
			{
				mgc.painter.DrawText(te);
			}
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x000452D4 File Offset: 0x000434D4
		private static Vector2 ConvertBorderRadiusPercentToPoints(Vector2 borderRectSize, Length length)
		{
			float num = length.value;
			float num2 = length.value;
			bool flag = length.unit == LengthUnit.Percent;
			if (flag)
			{
				num = borderRectSize.x * length.value / 100f;
				num2 = borderRectSize.y * length.value / 100f;
			}
			num = Mathf.Max(num, 0f);
			num2 = Mathf.Max(num2, 0f);
			return new Vector2(num, num2);
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x00045350 File Offset: 0x00043550
		public unsafe static void GetVisualElementRadii(VisualElement ve, out Vector2 topLeft, out Vector2 bottomLeft, out Vector2 topRight, out Vector2 bottomRight)
		{
			IResolvedStyle resolvedStyle = ve.resolvedStyle;
			Vector2 vector = new Vector2(resolvedStyle.width, resolvedStyle.height);
			ComputedStyle computedStyle = *ve.computedStyle;
			topLeft = MeshGenerationContextUtils.ConvertBorderRadiusPercentToPoints(vector, computedStyle.borderTopLeftRadius);
			bottomLeft = MeshGenerationContextUtils.ConvertBorderRadiusPercentToPoints(vector, computedStyle.borderBottomLeftRadius);
			topRight = MeshGenerationContextUtils.ConvertBorderRadiusPercentToPoints(vector, computedStyle.borderTopRightRadius);
			bottomRight = MeshGenerationContextUtils.ConvertBorderRadiusPercentToPoints(vector, computedStyle.borderBottomRightRadius);
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x000453D4 File Offset: 0x000435D4
		public static void AdjustBackgroundSizeForBorders(VisualElement visualElement, ref MeshGenerationContextUtils.RectangleParams rectParams)
		{
			IResolvedStyle resolvedStyle = visualElement.resolvedStyle;
			Vector4 zero = Vector4.zero;
			bool flag = resolvedStyle.borderLeftWidth >= 1f && resolvedStyle.borderLeftColor.a >= 1f;
			if (flag)
			{
				zero.x = 0.5f;
			}
			bool flag2 = resolvedStyle.borderTopWidth >= 1f && resolvedStyle.borderTopColor.a >= 1f;
			if (flag2)
			{
				zero.y = 0.5f;
			}
			bool flag3 = resolvedStyle.borderRightWidth >= 1f && resolvedStyle.borderRightColor.a >= 1f;
			if (flag3)
			{
				zero.z = 0.5f;
			}
			bool flag4 = resolvedStyle.borderBottomWidth >= 1f && resolvedStyle.borderBottomColor.a >= 1f;
			if (flag4)
			{
				zero.w = 0.5f;
			}
			rectParams.rectInset = zero;
		}

		// Token: 0x020002AE RID: 686
		public struct BorderParams
		{
			// Token: 0x060013BA RID: 5050 RVA: 0x000454D8 File Offset: 0x000436D8
			internal MeshBuilderNative.NativeBorderParams ToNativeParams()
			{
				return new MeshBuilderNative.NativeBorderParams
				{
					rect = this.rect,
					leftColor = this.leftColor,
					topColor = this.topColor,
					rightColor = this.rightColor,
					bottomColor = this.bottomColor,
					leftWidth = this.leftWidth,
					topWidth = this.topWidth,
					rightWidth = this.rightWidth,
					bottomWidth = this.bottomWidth,
					topLeftRadius = this.topLeftRadius,
					topRightRadius = this.topRightRadius,
					bottomRightRadius = this.bottomRightRadius,
					bottomLeftRadius = this.bottomLeftRadius,
					leftColorPage = this.leftColorPage.ToNativeColorPage(),
					topColorPage = this.topColorPage.ToNativeColorPage(),
					rightColorPage = this.rightColorPage.ToNativeColorPage(),
					bottomColorPage = this.bottomColorPage.ToNativeColorPage()
				};
			}

			// Token: 0x0400090A RID: 2314
			public Rect rect;

			// Token: 0x0400090B RID: 2315
			public Color playmodeTintColor;

			// Token: 0x0400090C RID: 2316
			public Color leftColor;

			// Token: 0x0400090D RID: 2317
			public Color topColor;

			// Token: 0x0400090E RID: 2318
			public Color rightColor;

			// Token: 0x0400090F RID: 2319
			public Color bottomColor;

			// Token: 0x04000910 RID: 2320
			public float leftWidth;

			// Token: 0x04000911 RID: 2321
			public float topWidth;

			// Token: 0x04000912 RID: 2322
			public float rightWidth;

			// Token: 0x04000913 RID: 2323
			public float bottomWidth;

			// Token: 0x04000914 RID: 2324
			public Vector2 topLeftRadius;

			// Token: 0x04000915 RID: 2325
			public Vector2 topRightRadius;

			// Token: 0x04000916 RID: 2326
			public Vector2 bottomRightRadius;

			// Token: 0x04000917 RID: 2327
			public Vector2 bottomLeftRadius;

			// Token: 0x04000918 RID: 2328
			public Material material;

			// Token: 0x04000919 RID: 2329
			internal ColorPage leftColorPage;

			// Token: 0x0400091A RID: 2330
			internal ColorPage topColorPage;

			// Token: 0x0400091B RID: 2331
			internal ColorPage rightColorPage;

			// Token: 0x0400091C RID: 2332
			internal ColorPage bottomColorPage;
		}

		// Token: 0x020002AF RID: 687
		public struct RectangleParams
		{
			// Token: 0x060013BB RID: 5051 RVA: 0x000455E4 File Offset: 0x000437E4
			public static MeshGenerationContextUtils.RectangleParams MakeSolid(Rect rect, Color color, ContextType panelContext)
			{
				Color color2 = ((panelContext == ContextType.Editor) ? UIElementsUtility.editorPlayModeTintColor : Color.white);
				return new MeshGenerationContextUtils.RectangleParams
				{
					rect = rect,
					color = color,
					uv = new Rect(0f, 0f, 1f, 1f),
					playmodeTintColor = color2
				};
			}

			// Token: 0x060013BC RID: 5052 RVA: 0x00045648 File Offset: 0x00043848
			private static void AdjustUVsForScaleMode(Rect rect, Rect uv, Texture texture, ScaleMode scaleMode, out Rect rectOut, out Rect uvOut)
			{
				float num = Mathf.Abs((float)texture.width * uv.width / ((float)texture.height * uv.height));
				float num2 = rect.width / rect.height;
				switch (scaleMode)
				{
				case ScaleMode.StretchToFill:
					break;
				case ScaleMode.ScaleAndCrop:
				{
					bool flag = num2 > num;
					if (flag)
					{
						float num3 = uv.height * (num / num2);
						float num4 = (uv.height - num3) * 0.5f;
						uv = new Rect(uv.x, uv.y + num4, uv.width, num3);
					}
					else
					{
						float num5 = uv.width * (num2 / num);
						float num6 = (uv.width - num5) * 0.5f;
						uv = new Rect(uv.x + num6, uv.y, num5, uv.height);
					}
					break;
				}
				case ScaleMode.ScaleToFit:
				{
					bool flag2 = num2 > num;
					if (flag2)
					{
						float num7 = num / num2;
						rect = new Rect(rect.xMin + rect.width * (1f - num7) * 0.5f, rect.yMin, num7 * rect.width, rect.height);
					}
					else
					{
						float num8 = num2 / num;
						rect = new Rect(rect.xMin, rect.yMin + rect.height * (1f - num8) * 0.5f, rect.width, num8 * rect.height);
					}
					break;
				}
				default:
					throw new NotImplementedException();
				}
				rectOut = rect;
				uvOut = uv;
			}

			// Token: 0x060013BD RID: 5053 RVA: 0x000457EC File Offset: 0x000439EC
			private static void AdjustSpriteUVsForScaleMode(Rect containerRect, Rect srcRect, Rect spriteGeomRect, Sprite sprite, ScaleMode scaleMode, out Rect rectOut, out Rect uvOut)
			{
				float num = sprite.rect.width / sprite.rect.height;
				float num2 = containerRect.width / containerRect.height;
				Rect rect = spriteGeomRect;
				rect.position -= sprite.bounds.min;
				rect.position /= sprite.bounds.size;
				rect.size /= sprite.bounds.size;
				Vector2 position = rect.position;
				position.y = 1f - rect.size.y - position.y;
				rect.position = position;
				switch (scaleMode)
				{
				case ScaleMode.StretchToFill:
				{
					Vector2 size = containerRect.size;
					containerRect.position = rect.position * size;
					containerRect.size = rect.size * size;
					break;
				}
				case ScaleMode.ScaleAndCrop:
				{
					Rect rect2 = containerRect;
					bool flag = num2 > num;
					if (flag)
					{
						rect2.height = rect2.width / num;
						rect2.position = new Vector2(rect2.position.x, -(rect2.height - containerRect.height) / 2f);
					}
					else
					{
						rect2.width = rect2.height * num;
						rect2.position = new Vector2(-(rect2.width - containerRect.width) / 2f, rect2.position.y);
					}
					Vector2 size2 = rect2.size;
					rect2.position += rect.position * size2;
					rect2.size = rect.size * size2;
					Rect rect3 = MeshGenerationContextUtils.RectangleParams.RectIntersection(containerRect, rect2);
					bool flag2 = rect3.width < 1E-30f || rect3.height < 1E-30f;
					if (flag2)
					{
						rect3 = Rect.zero;
					}
					else
					{
						Rect rect4 = rect3;
						rect4.position -= rect2.position;
						rect4.position /= rect2.size;
						rect4.size /= rect2.size;
						Vector2 position2 = rect4.position;
						position2.y = 1f - rect4.size.y - position2.y;
						rect4.position = position2;
						srcRect.position += rect4.position * srcRect.size;
						srcRect.size *= rect4.size;
					}
					containerRect = rect3;
					break;
				}
				case ScaleMode.ScaleToFit:
				{
					bool flag3 = num2 > num;
					if (flag3)
					{
						float num3 = num / num2;
						containerRect = new Rect(containerRect.xMin + containerRect.width * (1f - num3) * 0.5f, containerRect.yMin, num3 * containerRect.width, containerRect.height);
					}
					else
					{
						float num4 = num2 / num;
						containerRect = new Rect(containerRect.xMin, containerRect.yMin + containerRect.height * (1f - num4) * 0.5f, containerRect.width, num4 * containerRect.height);
					}
					containerRect.position += rect.position * containerRect.size;
					containerRect.size *= rect.size;
					break;
				}
				default:
					throw new NotImplementedException();
				}
				rectOut = containerRect;
				uvOut = srcRect;
			}

			// Token: 0x060013BE RID: 5054 RVA: 0x00045C00 File Offset: 0x00043E00
			internal static Rect RectIntersection(Rect a, Rect b)
			{
				Rect zero = Rect.zero;
				zero.min = Vector2.Max(a.min, b.min);
				zero.max = Vector2.Min(a.max, b.max);
				zero.size = Vector2.Max(zero.size, Vector2.zero);
				return zero;
			}

			// Token: 0x060013BF RID: 5055 RVA: 0x00045C68 File Offset: 0x00043E68
			private static Rect ComputeGeomRect(Sprite sprite)
			{
				Vector2 vector = new Vector2(float.MaxValue, float.MaxValue);
				Vector2 vector2 = new Vector2(float.MinValue, float.MinValue);
				foreach (Vector2 vector3 in sprite.vertices)
				{
					vector = Vector2.Min(vector, vector3);
					vector2 = Vector2.Max(vector2, vector3);
				}
				return new Rect(vector, vector2 - vector);
			}

			// Token: 0x060013C0 RID: 5056 RVA: 0x00045CE0 File Offset: 0x00043EE0
			private static Rect ComputeUVRect(Sprite sprite)
			{
				Vector2 vector = new Vector2(float.MaxValue, float.MaxValue);
				Vector2 vector2 = new Vector2(float.MinValue, float.MinValue);
				foreach (Vector2 vector3 in sprite.uv)
				{
					vector = Vector2.Min(vector, vector3);
					vector2 = Vector2.Max(vector2, vector3);
				}
				return new Rect(vector, vector2 - vector);
			}

			// Token: 0x060013C1 RID: 5057 RVA: 0x00045D58 File Offset: 0x00043F58
			private static Rect ApplyPackingRotation(Rect uv, SpritePackingRotation rotation)
			{
				switch (rotation)
				{
				case SpritePackingRotation.FlipHorizontal:
				{
					uv.position += new Vector2(uv.size.x, 0f);
					Vector2 size = uv.size;
					size.x = -size.x;
					uv.size = size;
					break;
				}
				case SpritePackingRotation.FlipVertical:
				{
					uv.position += new Vector2(0f, uv.size.y);
					Vector2 size2 = uv.size;
					size2.y = -size2.y;
					uv.size = size2;
					break;
				}
				case SpritePackingRotation.Rotate180:
					uv.position += uv.size;
					uv.size = -uv.size;
					break;
				}
				return uv;
			}

			// Token: 0x060013C2 RID: 5058 RVA: 0x00045E5C File Offset: 0x0004405C
			public static MeshGenerationContextUtils.RectangleParams MakeTextured(Rect rect, Rect uv, Texture texture, ScaleMode scaleMode, ContextType panelContext)
			{
				Color color = ((panelContext == ContextType.Editor) ? UIElementsUtility.editorPlayModeTintColor : Color.white);
				MeshGenerationContextUtils.RectangleParams.AdjustUVsForScaleMode(rect, uv, texture, scaleMode, out rect, out uv);
				Vector2 vector = new Vector2((float)texture.width, (float)texture.height);
				return new MeshGenerationContextUtils.RectangleParams
				{
					rect = rect,
					subRect = new Rect(0f, 0f, 1f, 1f),
					uv = uv,
					color = Color.white,
					texture = texture,
					contentSize = vector,
					textureSize = vector,
					scaleMode = scaleMode,
					playmodeTintColor = color
				};
			}

			// Token: 0x060013C3 RID: 5059 RVA: 0x00045F14 File Offset: 0x00044114
			public static MeshGenerationContextUtils.RectangleParams MakeSprite(Rect containerRect, Rect subRect, Sprite sprite, ScaleMode scaleMode, ContextType panelContext, bool hasRadius, ref Vector4 slices, bool useForRepeat = false)
			{
				bool flag = sprite == null || sprite.bounds.size.x < 1E-30f || sprite.bounds.size.y < 1E-30f;
				MeshGenerationContextUtils.RectangleParams rectangleParams2;
				if (flag)
				{
					MeshGenerationContextUtils.RectangleParams rectangleParams = default(MeshGenerationContextUtils.RectangleParams);
					rectangleParams2 = rectangleParams;
				}
				else
				{
					bool flag2 = sprite.texture == null;
					if (flag2)
					{
						Debug.LogWarning("Ignoring textureless sprite named \"" + sprite.name + "\", please import as a VectorImage instead");
						MeshGenerationContextUtils.RectangleParams rectangleParams = default(MeshGenerationContextUtils.RectangleParams);
						rectangleParams2 = rectangleParams;
					}
					else
					{
						Color color = ((panelContext == ContextType.Editor) ? UIElementsUtility.editorPlayModeTintColor : Color.white);
						Rect rect = MeshGenerationContextUtils.RectangleParams.ComputeGeomRect(sprite);
						Rect rect2 = MeshGenerationContextUtils.RectangleParams.ComputeUVRect(sprite);
						Vector4 border = sprite.border;
						bool flag3 = border != Vector4.zero || slices != Vector4.zero;
						bool flag4 = subRect != new Rect(0f, 0f, 1f, 1f);
						bool flag5 = scaleMode == ScaleMode.ScaleAndCrop || flag3 || hasRadius || useForRepeat || flag4;
						bool flag6 = flag5 && sprite.packed && sprite.packingRotation > SpritePackingRotation.None;
						if (flag6)
						{
							rect2 = MeshGenerationContextUtils.RectangleParams.ApplyPackingRotation(rect2, sprite.packingRotation);
						}
						bool flag7 = flag4;
						Rect rect3;
						if (flag7)
						{
							rect3 = subRect;
							rect3.position *= rect2.size;
							rect3.position += rect2.position;
							rect3.size *= rect2.size;
						}
						else
						{
							rect3 = rect2;
						}
						Rect rect4;
						Rect rect5;
						MeshGenerationContextUtils.RectangleParams.AdjustSpriteUVsForScaleMode(containerRect, rect3, rect, sprite, scaleMode, out rect4, out rect5);
						Rect rect6 = rect;
						rect6.size /= sprite.bounds.size;
						rect6.position -= sprite.bounds.min;
						rect6.position /= sprite.bounds.size;
						rect6.position = new Vector2(rect6.position.x, 1f - (rect6.position.y + rect6.height));
						MeshGenerationContextUtils.RectangleParams rectangleParams = new MeshGenerationContextUtils.RectangleParams
						{
							rect = rect4,
							uv = rect5,
							subRect = rect6,
							color = Color.white,
							texture = (flag5 ? sprite.texture : null),
							sprite = (flag5 ? null : sprite),
							contentSize = sprite.rect.size,
							textureSize = new Vector2((float)sprite.texture.width, (float)sprite.texture.height),
							spriteGeomRect = rect,
							scaleMode = scaleMode,
							playmodeTintColor = color,
							meshFlags = (sprite.packed ? MeshGenerationContext.MeshFlags.SkipDynamicAtlas : MeshGenerationContext.MeshFlags.None)
						};
						MeshGenerationContextUtils.RectangleParams rectangleParams3 = rectangleParams;
						Vector4 vector = new Vector4(border.x, border.w, border.z, border.y);
						bool flag8 = slices != Vector4.zero && vector != Vector4.zero && vector != slices;
						if (flag8)
						{
							Debug.LogWarning(string.Format("Sprite \"{0}\" borders {1} are overridden by style slices {2}", sprite.name, vector, slices));
						}
						else
						{
							bool flag9 = slices == Vector4.zero;
							if (flag9)
							{
								slices = vector;
							}
						}
						rectangleParams2 = rectangleParams3;
					}
				}
				return rectangleParams2;
			}

			// Token: 0x060013C4 RID: 5060 RVA: 0x000462E4 File Offset: 0x000444E4
			public static MeshGenerationContextUtils.RectangleParams MakeVectorTextured(Rect rect, Rect uv, VectorImage vectorImage, ScaleMode scaleMode, ContextType panelContext)
			{
				Color color = ((panelContext == ContextType.Editor) ? UIElementsUtility.editorPlayModeTintColor : Color.white);
				return new MeshGenerationContextUtils.RectangleParams
				{
					rect = rect,
					subRect = new Rect(0f, 0f, 1f, 1f),
					uv = uv,
					color = Color.white,
					vectorImage = vectorImage,
					contentSize = new Vector2(vectorImage.width, vectorImage.height),
					scaleMode = scaleMode,
					playmodeTintColor = color
				};
			}

			// Token: 0x060013C5 RID: 5061 RVA: 0x00046380 File Offset: 0x00044580
			internal bool HasRadius(float epsilon)
			{
				return (this.topLeftRadius.x > epsilon && this.topLeftRadius.y > epsilon) || (this.topRightRadius.x > epsilon && this.topRightRadius.y > epsilon) || (this.bottomRightRadius.x > epsilon && this.bottomRightRadius.y > epsilon) || (this.bottomLeftRadius.x > epsilon && this.bottomLeftRadius.y > epsilon);
			}

			// Token: 0x060013C6 RID: 5062 RVA: 0x00046408 File Offset: 0x00044608
			internal bool HasSlices(float epsilon)
			{
				return (float)this.leftSlice > epsilon || (float)this.topSlice > epsilon || (float)this.rightSlice > epsilon || (float)this.bottomSlice > epsilon;
			}

			// Token: 0x060013C7 RID: 5063 RVA: 0x00046448 File Offset: 0x00044648
			internal MeshBuilderNative.NativeRectParams ToNativeParams(Rect uvRegion)
			{
				return new MeshBuilderNative.NativeRectParams
				{
					rect = this.rect,
					subRect = this.subRect,
					backgroundRepeatRect = this.backgroundRepeatRect,
					uv = this.uv,
					uvRegion = uvRegion,
					color = this.color,
					scaleMode = this.scaleMode,
					topLeftRadius = this.topLeftRadius,
					topRightRadius = this.topRightRadius,
					bottomRightRadius = this.bottomRightRadius,
					bottomLeftRadius = this.bottomLeftRadius,
					contentSize = this.contentSize,
					textureSize = this.textureSize,
					texturePixelsPerPoint = 1f,
					leftSlice = this.leftSlice,
					topSlice = this.topSlice,
					rightSlice = this.rightSlice,
					bottomSlice = this.bottomSlice,
					sliceScale = this.sliceScale,
					rectInset = this.rectInset,
					colorPage = this.colorPage.ToNativeColorPage()
				};
			}

			// Token: 0x0400091D RID: 2333
			public Rect rect;

			// Token: 0x0400091E RID: 2334
			public Rect uv;

			// Token: 0x0400091F RID: 2335
			public Color color;

			// Token: 0x04000920 RID: 2336
			public Rect subRect;

			// Token: 0x04000921 RID: 2337
			public Rect backgroundRepeatRect;

			// Token: 0x04000922 RID: 2338
			public BackgroundPosition backgroundPositionX;

			// Token: 0x04000923 RID: 2339
			public BackgroundPosition backgroundPositionY;

			// Token: 0x04000924 RID: 2340
			public BackgroundRepeat backgroundRepeat;

			// Token: 0x04000925 RID: 2341
			public BackgroundSize backgroundSize;

			// Token: 0x04000926 RID: 2342
			public Texture texture;

			// Token: 0x04000927 RID: 2343
			public Sprite sprite;

			// Token: 0x04000928 RID: 2344
			public VectorImage vectorImage;

			// Token: 0x04000929 RID: 2345
			public Material material;

			// Token: 0x0400092A RID: 2346
			public ScaleMode scaleMode;

			// Token: 0x0400092B RID: 2347
			public Color playmodeTintColor;

			// Token: 0x0400092C RID: 2348
			public Vector2 topLeftRadius;

			// Token: 0x0400092D RID: 2349
			public Vector2 topRightRadius;

			// Token: 0x0400092E RID: 2350
			public Vector2 bottomRightRadius;

			// Token: 0x0400092F RID: 2351
			public Vector2 bottomLeftRadius;

			// Token: 0x04000930 RID: 2352
			public Vector2 contentSize;

			// Token: 0x04000931 RID: 2353
			public Vector2 textureSize;

			// Token: 0x04000932 RID: 2354
			public int leftSlice;

			// Token: 0x04000933 RID: 2355
			public int topSlice;

			// Token: 0x04000934 RID: 2356
			public int rightSlice;

			// Token: 0x04000935 RID: 2357
			public int bottomSlice;

			// Token: 0x04000936 RID: 2358
			public float sliceScale;

			// Token: 0x04000937 RID: 2359
			internal Rect spriteGeomRect;

			// Token: 0x04000938 RID: 2360
			public Vector4 rectInset;

			// Token: 0x04000939 RID: 2361
			internal ColorPage colorPage;

			// Token: 0x0400093A RID: 2362
			internal MeshGenerationContext.MeshFlags meshFlags;
		}
	}
}
