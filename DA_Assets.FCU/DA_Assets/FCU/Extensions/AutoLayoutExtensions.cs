using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DA_Assets.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Logging;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.FCU.Extensions
{
	// Token: 0x02000049 RID: 73
	public static class AutoLayoutExtensions
	{
		// Token: 0x060001A2 RID: 418 RVA: 0x00004378 File Offset: 0x00002578
		public static bool TryFixSizeWithStroke(this FObject fobject, float currentY, out float newY)
		{
			newY = 0f;
			if (currentY > 0f)
			{
				return false;
			}
			if (fobject.Strokes.IsEmpty<Paint>())
			{
				return false;
			}
			if (!fobject.Strokes.Any<Paint>((Paint x) => x.Visible.ToBoolNullTrue()))
			{
				return false;
			}
			newY = fobject.StrokeWeight;
			return true;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x000043E0 File Offset: 0x000025E0
		public static bool IsInsideAutoLayout(this FObject parent, out HorizontalOrVerticalLayoutGroup layoutGroup)
		{
			layoutGroup = null;
			if (!parent.ContainsTag(FcuTag.AutoLayoutGroup))
			{
				return false;
			}
			SyncData data = parent.Data;
			return !(((data != null) ? data.GameObject : null) == null) && parent.Data.GameObject.TryGetComponentSafe<HorizontalOrVerticalLayoutGroup>(out layoutGroup);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00004430 File Offset: 0x00002630
		public static RectOffsetCustom AdjustPadding(this RectOffsetCustom padding, Vector2 parentSize, Vector2[] childSizes)
		{
			float num = (float)(padding.left + padding.right);
			float num2 = (float)(padding.top + padding.bottom);
			float num3;
			if (!childSizes.IsEmpty<Vector2>())
			{
				num3 = childSizes.Max<Vector2>((Vector2 c) => c.x);
			}
			else
			{
				num3 = 0f;
			}
			float num4 = num3;
			float num5;
			if (!childSizes.IsEmpty<Vector2>())
			{
				num5 = childSizes.Max<Vector2>((Vector2 c) => c.y);
			}
			else
			{
				num5 = 0f;
			}
			float num6 = num5;
			if (num4 + num > parentSize.x && num != 0f)
			{
				float num7 = num4 + num - parentSize.x;
				float num8 = (float)padding.left / num;
				float num9 = (float)padding.right / num;
				padding.left -= Mathf.CeilToInt(num8 * num7);
				padding.right -= Mathf.CeilToInt(num9 * num7);
			}
			if (num6 + num2 > parentSize.y && num2 != 0f)
			{
				float num10 = num6 + num2 - parentSize.y;
				float num11 = (float)padding.top / num2;
				float num12 = (float)padding.bottom / num2;
				padding.top -= Mathf.CeilToInt(num11 * num10);
				padding.bottom -= Mathf.CeilToInt(num12 * num10);
			}
			return padding;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00004584 File Offset: 0x00002784
		public static TextAnchor GetHorLayoutAnchor(this FObject fobject)
		{
			string text = "";
			text += fobject.PrimaryAxisAlignItems.ToString();
			text += " ";
			text += fobject.CounterAxisAlignItems.ToString();
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 1431224069U)
			{
				if (num <= 819073107U)
				{
					if (num != 77535530U)
					{
						if (num != 243532736U)
						{
							if (num == 819073107U)
							{
								if (text == "MAX MAX")
								{
									return TextAnchor.LowerRight;
								}
							}
						}
						else if (text == "SPACE_BETWEEN NONE")
						{
							return TextAnchor.UpperCenter;
						}
					}
					else if (text == "CENTER MAX")
					{
						return TextAnchor.LowerCenter;
					}
				}
				else if (num <= 1005784460U)
				{
					if (num != 849941584U)
					{
						if (num == 1005784460U)
						{
							if (text == "SPACE_BETWEEN MAX")
							{
								return TextAnchor.LowerCenter;
							}
						}
					}
					else if (text == "NONE CENTER")
					{
						return TextAnchor.MiddleLeft;
					}
				}
				else if (num != 1414303279U)
				{
					if (num == 1431224069U)
					{
						if (text == "NONE MAX")
						{
							return TextAnchor.LowerLeft;
						}
					}
				}
				else if (text == "SPACE_BETWEEN CENTER")
				{
					return TextAnchor.MiddleCenter;
				}
			}
			else if (num <= 2878838533U)
			{
				if (num <= 1484131741U)
				{
					if (num != 1437734339U)
					{
						if (num == 1484131741U)
						{
							if (text == "MAX NONE")
							{
								return TextAnchor.UpperRight;
							}
						}
					}
					else if (text == "CENTER BASELINE")
					{
						return TextAnchor.MiddleCenter;
					}
				}
				else if (num != 2353769836U)
				{
					if (num == 2878838533U)
					{
						if (text == "CENTER CENTER")
						{
							return TextAnchor.MiddleCenter;
						}
					}
				}
				else if (text == "MAX BASELINE")
				{
					return TextAnchor.MiddleRight;
				}
			}
			else if (num <= 3855249934U)
			{
				if (num != 3042798606U)
				{
					if (num == 3855249934U)
					{
						if (text == "MAX CENTER")
						{
							return TextAnchor.MiddleRight;
						}
					}
				}
				else if (text == "CENTER NONE")
				{
					return TextAnchor.UpperCenter;
				}
			}
			else if (num != 4029173975U)
			{
				if (num == 4133102734U)
				{
					if (text == "NONE BASELINE")
					{
						return TextAnchor.MiddleLeft;
					}
				}
			}
			else if (text == "NONE NONE")
			{
				return TextAnchor.UpperLeft;
			}
			DALogger.LogError(FcuLocKey.log_unknown_aligment.Localize(new object[]
			{
				text,
				fobject.Data.NameHierarchy
			}));
			return TextAnchor.UpperLeft;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00004824 File Offset: 0x00002A24
		public static TextAnchor GetVertLayoutAnchor(this FObject fobject)
		{
			string text = "";
			text += fobject.PrimaryAxisAlignItems.ToString();
			text += " ";
			text += fobject.CounterAxisAlignItems.ToString();
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 1414303279U)
			{
				if (num <= 819073107U)
				{
					if (num != 77535530U)
					{
						if (num != 243532736U)
						{
							if (num == 819073107U)
							{
								if (text == "MAX MAX")
								{
									return TextAnchor.LowerRight;
								}
							}
						}
						else if (text == "SPACE_BETWEEN NONE")
						{
							return TextAnchor.MiddleLeft;
						}
					}
					else if (text == "CENTER MAX")
					{
						return TextAnchor.MiddleRight;
					}
				}
				else if (num != 849941584U)
				{
					if (num != 1005784460U)
					{
						if (num == 1414303279U)
						{
							if (text == "SPACE_BETWEEN CENTER")
							{
								return TextAnchor.MiddleCenter;
							}
						}
					}
					else if (text == "SPACE_BETWEEN MAX")
					{
						return TextAnchor.MiddleRight;
					}
				}
				else if (text == "NONE CENTER")
				{
					return TextAnchor.UpperCenter;
				}
			}
			else if (num <= 2878838533U)
			{
				if (num != 1431224069U)
				{
					if (num != 1484131741U)
					{
						if (num == 2878838533U)
						{
							if (text == "CENTER CENTER")
							{
								return TextAnchor.MiddleCenter;
							}
						}
					}
					else if (text == "MAX NONE")
					{
						return TextAnchor.LowerLeft;
					}
				}
				else if (text == "NONE MAX")
				{
					return TextAnchor.UpperRight;
				}
			}
			else if (num != 3042798606U)
			{
				if (num != 3855249934U)
				{
					if (num == 4029173975U)
					{
						if (text == "NONE NONE")
						{
							return TextAnchor.UpperLeft;
						}
					}
				}
				else if (text == "MAX CENTER")
				{
					return TextAnchor.LowerCenter;
				}
			}
			else if (text == "CENTER NONE")
			{
				return TextAnchor.MiddleLeft;
			}
			DALogger.LogError(FcuLocKey.log_unknown_aligment.Localize(new object[]
			{
				text,
				fobject.Data.NameHierarchy
			}));
			return TextAnchor.UpperLeft;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00004A34 File Offset: 0x00002C34
		[return: TupleElementNames(new string[] { "alignItems", "justifyContent" })]
		public static ValueTuple<string, string> ToUITK(this TextAnchor textAnchor, bool isHorizontal = true)
		{
			ValueTuple<string, string> valueTuple;
			switch (textAnchor)
			{
			case TextAnchor.UpperLeft:
				valueTuple = new ValueTuple<string, string>("flex-start", "flex-start");
				break;
			case TextAnchor.UpperCenter:
				valueTuple = new ValueTuple<string, string>("flex-start", "center");
				break;
			case TextAnchor.UpperRight:
				valueTuple = new ValueTuple<string, string>("flex-start", "flex-end");
				break;
			case TextAnchor.MiddleLeft:
				valueTuple = new ValueTuple<string, string>("center", "flex-start");
				break;
			case TextAnchor.MiddleCenter:
				valueTuple = new ValueTuple<string, string>("center", "center");
				break;
			case TextAnchor.MiddleRight:
				valueTuple = new ValueTuple<string, string>("center", "flex-end");
				break;
			case TextAnchor.LowerLeft:
				valueTuple = new ValueTuple<string, string>("flex-end", "flex-start");
				break;
			case TextAnchor.LowerCenter:
				valueTuple = new ValueTuple<string, string>("flex-end", "center");
				break;
			case TextAnchor.LowerRight:
				valueTuple = new ValueTuple<string, string>("flex-end", "flex-end");
				break;
			default:
				throw new NotImplementedException();
			}
			ValueTuple<string, string> valueTuple2 = valueTuple;
			string item = valueTuple2.Item1;
			string item2 = valueTuple2.Item2;
			if (!isHorizontal)
			{
				return new ValueTuple<string, string>(item2, item);
			}
			return new ValueTuple<string, string>(item, item2);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00004B40 File Offset: 0x00002D40
		[return: TupleElementNames(new string[] { "childControlWidth", "childControlHeight" })]
		public static ValueTuple<bool, bool> GetChildControlByLayoutMode(this FObject fobject, LayoutMode layoutMode)
		{
			bool flag = false;
			bool flag2 = false;
			HashSet<float?> hashSet = new HashSet<float?>();
			HashSet<LayoutAlign> hashSet2 = new HashSet<LayoutAlign>();
			foreach (FObject fobject2 in fobject.Children)
			{
				hashSet.Add(fobject2.LayoutGrow);
				hashSet2.Add(fobject2.LayoutAlign);
			}
			if (hashSet.Count == 1)
			{
				float? num = hashSet.First<float?>();
				float num2 = (float)1;
				if ((num.GetValueOrDefault() == num2) & (num != null))
				{
					if (layoutMode == LayoutMode.VERTICAL)
					{
						flag2 = true;
					}
					else if (layoutMode == LayoutMode.HORIZONTAL)
					{
						flag = true;
					}
				}
			}
			if (hashSet2.Count == 1 && hashSet2.First<LayoutAlign>() == LayoutAlign.STRETCH)
			{
				if (layoutMode == LayoutMode.VERTICAL)
				{
					flag = true;
				}
				else if (layoutMode == LayoutMode.HORIZONTAL)
				{
					flag2 = true;
				}
			}
			return new ValueTuple<bool, bool>(flag, flag2);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00004C1C File Offset: 0x00002E1C
		public static float GetHorSpacing(this FObject fobject)
		{
			int num = 0;
			float num2;
			if (fobject.PrimaryAxisAlignItems == PrimaryAxisAlignItem.SPACE_BETWEEN)
			{
				if (fobject.Data.ChildIndexes.IsEmpty<int>())
				{
					num = 1;
					num2 = 0f;
				}
				else if (fobject.Data.ChildIndexes.Count == 1)
				{
					num = 2;
					num2 = 0f;
				}
				else
				{
					num = 3;
					int num3 = fobject.Data.ChildIndexes.Count - 1;
					float x = fobject.Data.FRect.size.x;
					float num4 = 0f;
					foreach (FObject fobject2 in fobject.Children)
					{
						num4 += fobject2.Data.FRect.size.x;
					}
					num2 = (x - num4) / (float)num3;
				}
			}
			else
			{
				num = 4;
				num2 = fobject.ItemSpacing.ToFloat();
			}
			FcuLogger.Debug(string.Format("{0} | {1} | {2} | {3}", new object[]
			{
				"GetHorSpacing",
				num,
				num2,
				fobject.Data.NameHierarchy
			}), FcuDebugSettingsFlags.LogDefault);
			return num2;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00004D74 File Offset: 0x00002F74
		public static float GetVertSpacing(this FObject fobject)
		{
			if (fobject.PrimaryAxisAlignItems != PrimaryAxisAlignItem.SPACE_BETWEEN)
			{
				return fobject.ItemSpacing.ToFloat();
			}
			if (fobject.Data.ChildIndexes.IsEmpty<int>())
			{
				return 0f;
			}
			if (fobject.Data.ChildIndexes.Count == 1)
			{
				return 0f;
			}
			int num = fobject.Data.ChildIndexes.Count - 1;
			float y = fobject.Data.FRect.size.y;
			float num2 = 0f;
			foreach (FObject fobject2 in fobject.Children)
			{
				num2 += fobject2.Data.FRect.size.y;
			}
			return (y - num2) / (float)num;
		}
	}
}
