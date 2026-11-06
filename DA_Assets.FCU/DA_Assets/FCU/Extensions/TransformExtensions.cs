using System;
using System.Collections.Generic;
using DA_Assets.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine;

namespace DA_Assets.FCU.Extensions
{
	// Token: 0x0200004F RID: 79
	public static class TransformExtensions
	{
		// Token: 0x060001E3 RID: 483 RVA: 0x000065FC File Offset: 0x000047FC
		public static List<Transform> GetAllParents(this Transform child)
		{
			List<Transform> list = new List<Transform>();
			Transform transform = child.parent;
			while (transform != null)
			{
				list.Add(transform);
				transform = transform.parent;
			}
			return list;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00006630 File Offset: 0x00004830
		public static float GetAngleFromMatrix(this FObject fobject)
		{
			if (fobject.RelativeTransform.IsEmpty<List<float?>>() || fobject.RelativeTransform.Count < 2 || fobject.RelativeTransform[1].Count < 2 || fobject.RelativeTransform[1][0] == null || fobject.RelativeTransform[1][1] == null)
			{
				return 0f;
			}
			float num = fobject.RelativeTransform[1][0].Value;
			float num2 = fobject.RelativeTransform[1][1].Value;
			float num3 = Mathf.Atan2(num, num2).Round(FcuConfig.Rounding.GetAngleFromMatrixDigits);
			num3 = -1f * num3;
			float num4 = num3 * 57.295776f;
			FcuLogger.Debug(string.Format("{0} | {1} | rotDeg: {2}\nb: {3}\na: {4}\nrotRad: {5}", new object[]
			{
				"GetAngleFromMatrix",
				fobject.Data.NameHierarchy,
				num4,
				num2,
				num,
				num3
			}), FcuDebugSettingsFlags.LogAngle);
			return num4;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000676C File Offset: 0x0000496C
		public static float GetAngleFromField(this FObject fobject)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = ((fobject.Rotation != null) ? fobject.Rotation.Value.Round(FcuConfig.Rounding.GetAngleFromMatrixDigits) : 0f);
			num3 = -1f * num3;
			float num4 = num3 * 57.295776f;
			FcuLogger.Debug(string.Format("{0} | {1} | hasValue: {2} | rotDeg: {3}\nb: {4}\na: {5}\nrotRad: {6}", new object[]
			{
				"GetAngleFromField",
				fobject.Data.NameHierarchy,
				fobject.Rotation != null,
				num4,
				num2,
				num,
				num3
			}), FcuDebugSettingsFlags.LogAngle);
			return num4;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000683C File Offset: 0x00004A3C
		public static float GetFigmaRotationAngle(this FObject fobject)
		{
			float num = fobject.GetAngleFromField();
			if (num == 0f)
			{
				num = fobject.GetAngleFromMatrix();
			}
			return num;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00006860 File Offset: 0x00004A60
		public static AnchorType GetFigmaAnchor(this FObject fobject)
		{
			string text = fobject.Constraints.Vertical + " " + fobject.Constraints.Horizontal;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 2811300000U)
			{
				if (num <= 1216664455U)
				{
					if (num <= 645222069U)
					{
						if (num != 404177299U)
						{
							if (num != 437930364U)
							{
								if (num == 645222069U)
								{
									if (text == "TOP LEFT")
									{
										return AnchorType.TopLeft;
									}
								}
							}
							else if (text == "BOTTOM SCALE")
							{
								return AnchorType.HorStretchBottom;
							}
						}
						else if (text == "BOTTOM CENTER")
						{
							return AnchorType.BottomCenter;
						}
					}
					else if (num != 757575642U)
					{
						if (num != 1197105300U)
						{
							if (num == 1216664455U)
							{
								if (text == "TOP_BOTTOM CENTER")
								{
									return AnchorType.VertStretchCenter;
								}
							}
						}
						else if (text == "BOTTOM RIGHT")
						{
							return AnchorType.BottomRight;
						}
					}
					else if (text == "SCALE LEFT")
					{
						return AnchorType.VertStretchLeft;
					}
				}
				else if (num <= 1979006936U)
				{
					if (num != 1451492718U)
					{
						if (num != 1519348980U)
						{
							if (num == 1979006936U)
							{
								if (text == "TOP_BOTTOM SCALE")
								{
									return AnchorType.VertStretchCenter;
								}
							}
						}
						else if (text == "TOP LEFT_RIGHT")
						{
							return AnchorType.HorStretchTop;
						}
					}
					else if (text == "TOP RIGHT")
					{
						return AnchorType.TopRight;
					}
				}
				else if (num != 2136135531U)
				{
					if (num != 2578779901U)
					{
						if (num == 2811300000U)
						{
							if (text == "TOP_BOTTOM RIGHT")
							{
								return AnchorType.VertStretchRight;
							}
						}
					}
					else if (text == "CENTER LEFT")
					{
						return AnchorType.MiddleLeft;
					}
				}
				else if (text == "BOTTOM LEFT")
				{
					return AnchorType.BottomLeft;
				}
			}
			else if (num <= 3256392731U)
			{
				if (num <= 3116131558U)
				{
					if (num != 2849903958U)
					{
						if (num != 2878838533U)
						{
							if (num == 3116131558U)
							{
								if (text == "BOTTOM LEFT_RIGHT")
								{
									return AnchorType.HorStretchBottom;
								}
							}
						}
						else if (text == "CENTER CENTER")
						{
							return AnchorType.MiddleCenter;
						}
					}
					else if (text == "CENTER RIGHT")
					{
						return AnchorType.MiddleRight;
					}
				}
				else if (num != 3148319182U)
				{
					if (num != 3213010315U)
					{
						if (num == 3256392731U)
						{
							if (text == "SCALE SCALE")
							{
								return AnchorType.StretchAll;
							}
						}
					}
					else if (text == "SCALE LEFT_RIGHT")
					{
						return AnchorType.HorStretchMiddle;
					}
				}
				else if (text == "TOP SCALE")
				{
					return AnchorType.HorStretchTop;
				}
			}
			else if (num <= 3627330055U)
			{
				if (num != 3453539826U)
				{
					if (num != 3616756893U)
					{
						if (num == 3627330055U)
						{
							if (text == "SCALE RIGHT")
							{
								return AnchorType.VertStretchRight;
							}
						}
					}
					else if (text == "TOP CENTER")
					{
						return AnchorType.TopCenter;
					}
				}
				else if (text == "TOP_BOTTOM LEFT_RIGHT")
				{
					return AnchorType.StretchAll;
				}
			}
			else if (num <= 3695284966U)
			{
				if (num != 3667871631U)
				{
					if (num == 3695284966U)
					{
						if (text == "CENTER SCALE")
						{
							return AnchorType.StretchAll;
						}
					}
				}
				else if (text == "TOP_BOTTOM LEFT")
				{
					return AnchorType.VertStretchLeft;
				}
			}
			else if (num != 3792401158U)
			{
				if (num == 3971853516U)
				{
					if (text == "CENTER LEFT_RIGHT")
					{
						return AnchorType.HorStretchMiddle;
					}
				}
			}
			else if (text == "SCALE CENTER")
			{
				return AnchorType.StretchAll;
			}
			return AnchorType.MiddleCenter;
		}
	}
}
