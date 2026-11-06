using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Logging;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x020002C7 RID: 711
	[Serializable]
	public class HashGenerator : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x060013F1 RID: 5105 RVA: 0x0007FB14 File Offset: 0x0007DD14
		public async Task SetHashes(List<FObject> fobjects)
		{
			DALogger.Log("SetHashes");
			Action<FObject> <>9__1;
			await Task.Run(delegate
			{
				IEnumerable<FObject> fobjects2 = fobjects;
				Action<FObject> action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate(FObject fobject)
					{
						List<FObjectHashData> list = new List<FObjectHashData>();
						this.GetHashRecursive(fobject, fobject, list, 0);
						List<FObjectHashData> list2 = list.OrderBy<FObjectHashData, int>((FObjectHashData x) => x.Indent).ToList<FObjectHashData>();
						string text = this.ConvertToTreeString(list, false);
						string text2 = this.ConvertToTreeString(list2, true);
						fobject.Data.Hash = text.GetDeterministicHashCode();
						if (fobject.IsDownloadableType())
						{
							fobject.Data.HashDataTree = text2;
						}
						StringBuilder stringBuilder = new StringBuilder();
						this.AddFirstLevel(list.First<FObjectHashData>(), true, stringBuilder);
						fobject.Data.HashData = stringBuilder.ToString();
					});
				}
				Parallel.ForEach<FObject>(fobjects2, action);
			}, this.monoBeh.GetToken(TokenType.Import));
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x0007FB60 File Offset: 0x0007DD60
		private void GetHashRecursive(FObject target, FObject fobject, List<FObjectHashData> values, int indent)
		{
			FObjectHashData fobjectHashData = new FObjectHashData(fobject, new List<FieldHashData>(), new List<EffectHashData>(), indent);
			try
			{
				fobjectHashData.FieldHashes.Add(new FieldHashData("IsVisible", fobject.IsVisible()));
				Vector2 vector;
				if (target.Data.InsideDownloadable && fobject.TryGetLocalPosition(out vector))
				{
					fobjectHashData.FieldHashes.Add(new FieldHashData("rtPos", vector));
				}
				fobjectHashData.FieldHashes.Add(new FieldHashData("GetFigmaRotationAngle", fobject.GetFigmaRotationAngle()));
				if (!fobject.FillGeometry.IsEmpty<FillGeometry>())
				{
					fobjectHashData.FieldHashes.Add(new FieldHashData("FillGeometry", fobject.FillGeometry[0].Path.GetDeterministicHashCode()));
				}
				if (!fobject.StrokeGeometry.IsEmpty<FillGeometry>())
				{
					fobjectHashData.FieldHashes.Add(new FieldHashData("StrokeGeometry", fobject.StrokeGeometry[0].Path.GetDeterministicHashCode()));
				}
				fobjectHashData.FieldHashes.Add(new FieldHashData("StrokeWeight", fobject.StrokeWeight));
				fobjectHashData.FieldHashes.Add(new FieldHashData("StrokeAlign", fobject.StrokeAlign));
				if (fobject.CornerRadiuses.IsEmpty<float>())
				{
					fobjectHashData.FieldHashes.Add(new FieldHashData("CornerRadius", fobject.CornerRadius));
				}
				else
				{
					fobjectHashData.FieldHashes.Add(new FieldHashData("CornerRadiuses", string.Join<float>(" ", fobject.CornerRadiuses)));
				}
				fobjectHashData.FieldHashes.Add(new FieldHashData("BlendMode", fobject.BlendMode));
				fobjectHashData.FieldHashes.Add(new FieldHashData("Opacity", fobject.Opacity));
				fobjectHashData.FieldHashes.Add(new FieldHashData("ClipsContent", fobject.ClipsContent));
				fobjectHashData.FieldHashes.Add(new FieldHashData("LayoutMode", fobject.LayoutMode));
				fobjectHashData.FieldHashes.Add(new FieldHashData("ItemSpacing", fobject.ItemSpacing));
				fobjectHashData.FieldHashes.Add(new FieldHashData("PrimaryAxisAlignItems", fobject.PrimaryAxisAlignItems));
				fobjectHashData.FieldHashes.Add(new FieldHashData("CounterAxisAlignItems", fobject.CounterAxisAlignItems));
				fobjectHashData.FieldHashes.Add(new FieldHashData("PaddingLeft", fobject.PaddingLeft));
				fobjectHashData.FieldHashes.Add(new FieldHashData("PaddingRight", fobject.PaddingRight));
				fobjectHashData.FieldHashes.Add(new FieldHashData("PaddingTop", fobject.PaddingTop));
				fobjectHashData.FieldHashes.Add(new FieldHashData("PaddingBottom", fobject.PaddingBottom));
				fobjectHashData.FieldHashes.Add(new FieldHashData("HorizontalPadding", fobject.HorizontalPadding));
				fobjectHashData.FieldHashes.Add(new FieldHashData("VerticalPadding", fobject.VerticalPadding));
				if (fobject.Type == NodeType.VECTOR)
				{
					fobjectHashData.FieldHashes.Add(new FieldHashData("StrokeCap", fobject.StrokeCap));
					fobjectHashData.FieldHashes.Add(new FieldHashData("StrokeJoin", fobject.StrokeJoin));
					fobjectHashData.FieldHashes.Add(new FieldHashData("StrokeMiterAngle", fobject.StrokeMiterAngle));
					if (!fobject.StrokeDashes.IsEmpty<float>())
					{
						fobjectHashData.FieldHashes.Add(new FieldHashData("StrokeDashes", string.Join<float>(" ", fobject.StrokeDashes)));
					}
				}
				if (target.Type == NodeType.TEXT && fobject.Type == NodeType.TEXT)
				{
					fobjectHashData.FieldHashes.Add(new FieldHashData("Characters", fobject.Characters));
					fobjectHashData.EffectDatas.Add(this.GetHash("TextStyle", fobject.Style));
				}
				else if (target.Data.InsideDownloadable)
				{
					fobjectHashData.FieldHashes.Add(new FieldHashData("Size", fobject.Size));
				}
				fobjectHashData.EffectDatas.Add(this.GetHash("Fills", fobject, fobject.Fills));
				fobjectHashData.EffectDatas.Add(this.GetHash("Effects", fobject, fobject.Effects));
				fobjectHashData.EffectDatas.Add(this.GetHash("Strokes", fobject, fobject.Strokes));
				fobjectHashData.EffectDatas.Add(this.GetHash("ArcData", fobject.ArcData));
				fobjectHashData.FieldHashes.AddRange(fobjectHashData.FieldHashes);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			values.Add(fobjectHashData);
			if (!fobject.Children.IsEmpty<FObject>())
			{
				if (fobject.Children.Count > FcuConfig.ChildParsingLimit)
				{
					fobjectHashData.FieldHashes.Add(new FieldHashData("Children", fobject.Children.Count));
					return;
				}
				for (int i = 0; i < fobject.Children.Count; i++)
				{
					this.GetHashRecursive(target, fobject.Children[i], values, fobjectHashData.Indent + i);
				}
			}
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x00080130 File Offset: 0x0007E330
		private EffectHashData GetHash(string effectName, Style style)
		{
			EffectHashData effectHashData = new EffectHashData(effectName, new List<FieldHashData>());
			if (style.IsDefault<Style>())
			{
				return effectHashData;
			}
			try
			{
				effectHashData.Data.Add(new FieldHashData("FontFamily", style.FontFamily));
				effectHashData.Data.Add(new FieldHashData("FontWeight", style.FontWeight));
				effectHashData.Data.Add(new FieldHashData("FontSize", style.FontSize));
				effectHashData.Data.Add(new FieldHashData("TextAlignHorizontal", style.TextAlignHorizontal));
				effectHashData.Data.Add(new FieldHashData("TextAlignVertical", style.TextAlignVertical));
				effectHashData.Data.Add(new FieldHashData("LetterSpacing", style.LetterSpacing));
				effectHashData.Data.Add(new FieldHashData("LineHeightPx", style.LineHeightPx));
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			return effectHashData;
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x00080250 File Offset: 0x0007E450
		private EffectHashData GetHash(string effectName, FObject fobject, List<Paint> fills)
		{
			EffectHashData effectHashData = new EffectHashData(effectName, new List<FieldHashData>());
			if (fills.IsEmpty<Paint>())
			{
				return effectHashData;
			}
			foreach (Paint paint in fills)
			{
				try
				{
					effectHashData.Data.Add(new FieldHashData("IsVisible", paint.IsVisible()));
					effectHashData.Data.Add(new FieldHashData("Type", paint.Type));
					effectHashData.Data.Add(new FieldHashData("Opacity", paint.Opacity));
					if (fobject.Data.Graphic.SpriteSingleColor.IsDefault<Color>() || fobject.Data.InsideDownloadable)
					{
						effectHashData.Data.Add(new FieldHashData("Color", paint.Color));
					}
					else
					{
						effectHashData.Data.Add(new FieldHashData("Color", Color.white));
					}
					effectHashData.Data.Add(new FieldHashData("BlendMode", paint.BlendMode));
					effectHashData.Data.Add(new FieldHashData("ScaleMode", paint.ScaleMode));
					effectHashData.Data.Add(new FieldHashData("ScalingFactor", paint.ScalingFactor));
					effectHashData.Data.Add(new FieldHashData("Rotation", paint.Rotation));
					effectHashData.Data.Add(new FieldHashData("ImageRef", paint.ImageRef));
					effectHashData.Data.Add(new FieldHashData("GifRef", paint.GifRef));
					effectHashData.Data.Add(new FieldHashData("Exposure", paint.Filters.Exposure));
					effectHashData.Data.Add(new FieldHashData("Contrast", paint.Filters.Contrast));
					effectHashData.Data.Add(new FieldHashData("Saturation", paint.Filters.Saturation));
					effectHashData.Data.Add(new FieldHashData("Temperature", paint.Filters.Temperature));
					effectHashData.Data.Add(new FieldHashData("Tint", paint.Filters.Tint));
					effectHashData.Data.Add(new FieldHashData("Highlights", paint.Filters.Highlights));
					effectHashData.Data.Add(new FieldHashData("Shadows", paint.Filters.Shadows));
					if (!paint.ImageTransform.IsEmpty<List<float>>())
					{
						List<float> list = paint.ImageTransform.SelectMany<List<float>, float>((List<float> x) => x).ToList<float>();
						effectHashData.Data.Add(new FieldHashData("ImageTransform", list));
					}
					if (!paint.GradientStops.IsEmpty<GradientStop>())
					{
						foreach (GradientStop gradientStop in paint.GradientStops)
						{
							if (fobject.Data.Graphic.SpriteSingleColor.IsDefault<Color>() || fobject.Data.InsideDownloadable)
							{
								effectHashData.Data.Add(new FieldHashData("Color", gradientStop.Color));
							}
							else
							{
								effectHashData.Data.Add(new FieldHashData("Color", Color.white));
							}
							effectHashData.Data.Add(new FieldHashData("Position", gradientStop.Position));
						}
					}
					if (!paint.GradientHandlePositions.IsEmpty<Vector2>())
					{
						effectHashData.Data.Add(new FieldHashData("GradientHandlePositions", string.Join<Vector2>(" ", paint.GradientHandlePositions)));
					}
				}
				catch (Exception ex)
				{
					FcuLogger.Debug(ex, FcuDebugSettingsFlags.LogError);
				}
			}
			return effectHashData;
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x0008070C File Offset: 0x0007E90C
		private EffectHashData GetHash(string effectName, ArcData arcData)
		{
			EffectHashData effectHashData = new EffectHashData(effectName, new List<FieldHashData>());
			try
			{
				effectHashData.Data.Add(new FieldHashData("StartingAngle", arcData.StartingAngle));
				effectHashData.Data.Add(new FieldHashData("EndingAngle", arcData.EndingAngle));
				effectHashData.Data.Add(new FieldHashData("InnerRadius", arcData.InnerRadius));
			}
			catch (Exception ex)
			{
				FcuLogger.Debug(ex, FcuDebugSettingsFlags.LogError);
			}
			return effectHashData;
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x000807AC File Offset: 0x0007E9AC
		private EffectHashData GetHash(string effectName, FObject fobject, List<Effect> effects)
		{
			EffectHashData effectHashData = new EffectHashData(effectName, new List<FieldHashData>());
			if (effects.IsEmpty<Effect>())
			{
				return effectHashData;
			}
			foreach (Effect effect in effects)
			{
				try
				{
					effectHashData.Data.Add(new FieldHashData("IsVisible", effect.IsVisible()));
					effectHashData.Data.Add(new FieldHashData("Type", effect.Type));
					effectHashData.Data.Add(new FieldHashData("Radius", effect.Radius));
					if (fobject.Data.Graphic.SpriteSingleColor.IsDefault<Color>() || fobject.Data.InsideDownloadable)
					{
						effectHashData.Data.Add(new FieldHashData("Color", effect.Color));
					}
					else
					{
						effectHashData.Data.Add(new FieldHashData("Color", Color.white));
					}
					effectHashData.Data.Add(new FieldHashData("BlendMode", effect.BlendMode));
					effectHashData.Data.Add(new FieldHashData("Offset", effect.Offset));
					effectHashData.Data.Add(new FieldHashData("Spread", effect.Spread));
					effectHashData.Data.Add(new FieldHashData("ShowShadowBehindNode", effect.ShowShadowBehindNode));
				}
				catch (Exception ex)
				{
					FcuLogger.Debug(ex, FcuDebugSettingsFlags.LogError);
				}
			}
			return effectHashData;
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x00080998 File Offset: 0x0007EB98
		private static void Dash(StringBuilder sb, int indentLevel)
		{
			sb.Append(new string('—', indentLevel * 4));
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x000809AE File Offset: 0x0007EBAE
		private static void Indent(StringBuilder sb, int indentLevel)
		{
			sb.Append(new string(' ', indentLevel * 4));
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x000809C4 File Offset: 0x0007EBC4
		private string ConvertToTreeString(List<FObjectHashData> objectHashes, bool includeNames)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (FObjectHashData fobjectHashData in objectHashes)
			{
				this.AddFirstLevel(fobjectHashData, includeNames, stringBuilder);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x00080A20 File Offset: 0x0007EC20
		private void AddFirstLevel(FObjectHashData objectHash, bool includeNames, StringBuilder sb)
		{
			if (includeNames)
			{
				HashGenerator.Indent(sb, objectHash.Indent);
				HashGenerator.Dash(sb, 1);
				sb.AppendLine(objectHash.FObject.Data.NameHierarchy ?? "");
			}
			foreach (FieldHashData fieldHashData in objectHash.FieldHashes)
			{
				if (!string.Format("{0}", fieldHashData.Data).IsEmpty())
				{
					if (includeNames)
					{
						HashGenerator.Indent(sb, objectHash.Indent + 1);
					}
					sb.AppendLine(string.Format("{0} | {1}", fieldHashData.Name, fieldHashData.Data));
				}
			}
			foreach (EffectHashData effectHashData in objectHash.EffectDatas)
			{
				if (!effectHashData.Data.IsEmpty<FieldHashData>())
				{
					if (includeNames)
					{
						HashGenerator.Indent(sb, objectHash.Indent + 1);
						HashGenerator.Dash(sb, 1);
						sb.AppendLine(effectHashData.Name ?? "");
					}
					foreach (FieldHashData fieldHashData2 in effectHashData.Data)
					{
						if (!string.Format("{0}", fieldHashData2.Data).IsEmpty())
						{
							if (includeNames)
							{
								HashGenerator.Indent(sb, objectHash.Indent + 2);
							}
							sb.AppendLine(string.Format("{0} | {1}", fieldHashData2.Name, fieldHashData2.Data));
						}
					}
				}
			}
		}
	}
}
