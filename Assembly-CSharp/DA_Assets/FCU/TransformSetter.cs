using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x020002F5 RID: 757
	[Serializable]
	public class TransformSetter : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x060014F1 RID: 5361 RVA: 0x0008861C File Offset: 0x0008681C
		public float GetAbsoluteAngle(FObject fobject)
		{
			float num = 0f;
			FObject fobject2 = fobject;
			do
			{
				num += fobject2.GetFigmaRotationAngle();
			}
			while (this.monoBeh.CurrentProject.TryGetParent(fobject2, out fobject2));
			return num;
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x00088650 File Offset: 0x00086850
		public async Task SetTransformPos(List<FObject> fobjects)
		{
			foreach (FObject fobject in fobjects)
			{
				SyncData data = fobject.Data;
				if (!(((data != null) ? data.RectGameObject : null) == null))
				{
					RectTransform component = fobject.Data.RectGameObject.GetComponent<RectTransform>();
					component.SetSmartAnchor(AnchorType.BottomLeft);
					component.SetSmartPivot(PivotType.TopLeft);
					fobject.Data.FRect = this.GetGlobalRect(fobject);
					component.sizeDelta = fobject.Data.FRect.size;
					component.anchoredPosition = fobject.Data.FRect.position;
					component.SetSmartPivot(PivotType.MiddleCenter);
					component.SetRotation(fobject.Data.FRect.absoluteAngle);
					if (fobject.ContainsTag(FcuTag.Frame))
					{
						component.SetSmartAnchor(AnchorType.TopLeft);
					}
					else if (!fobject.Data.Parent.ContainsTag(FcuTag.AutoLayoutGroup))
					{
						component.SetSmartAnchor(fobject.GetFigmaAnchor());
					}
					component.SetSmartPivot(this.monoBeh.Settings.MainSettings.PivotType);
				}
			}
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x0008869C File Offset: 0x0008689C
		public FRect GetGlobalRect(FObject fobject)
		{
			FRect frect = default(FRect);
			Vector2 vector = default(Vector2);
			Vector2 vector2 = default(Vector2);
			Vector2 vector3;
			fobject.GetBoundingSize(out vector3);
			Vector2 vector4;
			fobject.GetBoundingPosition(out vector4);
			Vector2 vector5;
			fobject.GetRenderSize(out vector5);
			Vector2 vector6;
			fobject.GetRenderPosition(out vector6);
			Vector2 vector7;
			fobject.TryGetLocalPosition(out vector7);
			frect.angle = ((fobject.Data.FcuImageType == FcuImageType.Downloadable) ? 0f : fobject.GetFigmaRotationAngle());
			frect.absoluteAngle = ((fobject.Data.FcuImageType == FcuImageType.Downloadable) ? 0f : this.GetAbsoluteAngle(fobject));
			bool flag = this.monoBeh.IsUGUI() || this.monoBeh.IsNova();
			float num = 1f;
			BigInteger bigInteger;
			int num2;
			if (!fobject.IsSvgExtension() && fobject.IsDownloadableType() && fobject.Data.SpritePath.TryParseSpriteName(out num, out bigInteger))
			{
				num2 = 1;
				vector = vector6;
				vector2 = new Vector2((float)fobject.Data.SpriteSize.x / num, (float)fobject.Data.SpriteSize.y / num);
			}
			else if (frect.absoluteAngle != 0f)
			{
				num2 = 2;
				vector2 = fobject.Size;
				Vector2 vector8 = TransformSetter.CalculateOffset(vector2.x, vector2.y, frect.absoluteAngle);
				vector = new Vector2(vector4.x + vector8.x, vector4.y + vector8.y);
			}
			else
			{
				num2 = 3;
				vector2 = vector3;
				vector = vector4;
			}
			float num3;
			if (fobject.TryFixSizeWithStroke(vector2.y, out num3))
			{
				vector2.y = num3;
			}
			FcuLogger.Debug(string.Format("{0} | {1} | state: {2} | {3} | {4} | {5}", new object[]
			{
				"GetGlobalRect",
				fobject.Data.NameHierarchy,
				num2,
				vector2,
				vector,
				frect.absoluteAngle
			}), FcuDebugSettingsFlags.LogTransform);
			frect.size = vector2;
			frect.position = new Vector2(vector.x, flag ? (-vector.y) : vector.y);
			List<Vector2> list = new List<Vector2>();
			foreach (int num4 in fobject.Data.ChildIndexes)
			{
				FObject fobject2;
				if (this.monoBeh.CurrentProject.TryGetByIndex(num4, out fobject2))
				{
					list.Add(this.GetGlobalRect(fobject2).size);
				}
			}
			frect.padding = TransformSetter.GetPadding(fobject).AdjustPadding(vector2, list.ToArray());
			return frect;
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x00088960 File Offset: 0x00086B60
		private static RectOffsetCustom GetPadding(FObject fobject)
		{
			return new RectOffsetCustom
			{
				bottom = (int)fobject.PaddingBottom.ToFloat().Round(FcuConfig.Rounding.GetPaddingDigits),
				top = (int)fobject.PaddingTop.ToFloat().Round(FcuConfig.Rounding.GetPaddingDigits),
				left = (int)fobject.PaddingLeft.ToFloat().Round(FcuConfig.Rounding.GetPaddingDigits),
				right = (int)fobject.PaddingRight.ToFloat().Round(FcuConfig.Rounding.GetPaddingDigits)
			};
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x00088A04 File Offset: 0x00086C04
		public static Vector2 CalculateOffset(float width, float height, float angleDegrees)
		{
			float num = 0.017453292f * angleDegrees;
			float num2 = width / 2f;
			float num3 = height / 2f;
			float num4 = Mathf.Abs(num2 * Mathf.Cos(num)) + Mathf.Abs(num3 * Mathf.Sin(num));
			float num5 = Mathf.Abs(num2 * Mathf.Sin(num)) + Mathf.Abs(num3 * Mathf.Cos(num));
			float num6 = num4 - num2;
			float num7 = num5 - num3;
			return new Vector2(num6, num7);
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x00088A70 File Offset: 0x00086C70
		public async Task RestoreParentsRect(List<FObject> fobjects)
		{
			foreach (FObject fobject in fobjects)
			{
				SyncData data = fobject.Data;
				if (!(((data != null) ? data.RectGameObject : null) == null) && !fobject.ContainsTag(FcuTag.Frame))
				{
					if (fobject.Data.RectGameObject != null)
					{
						fobject.Data.RectGameObject.transform.SetParent(fobject.Data.ParentTransformRect);
					}
					else
					{
						fobject.Data.RectGameObject.transform.SetParent(this.monoBeh.transform);
					}
				}
			}
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x00088ABC File Offset: 0x00086CBC
		public async Task RestoreParents(List<FObject> fobjects)
		{
			foreach (FObject fobject in fobjects)
			{
				SyncData data = fobject.Data;
				if (!(((data != null) ? data.GameObject : null) == null))
				{
					if (fobject.Data.GameObject != null)
					{
						fobject.Data.GameObject.transform.SetParent(fobject.Data.ParentTransform);
					}
					else
					{
						fobject.Data.GameObject.transform.SetParent(this.monoBeh.transform);
					}
				}
			}
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x00088B08 File Offset: 0x00086D08
		internal async Task MoveUguiTransforms(List<FObject> currPage)
		{
			foreach (FObject fobject in currPage)
			{
				if (!(fobject.Data.GameObject == null) && !(fobject.Data.RectGameObject == null))
				{
					RectTransform rectTransform;
					fobject.Data.GameObject.TryAddComponent<RectTransform>(out rectTransform, false);
					RectTransform rectTransform2;
					fobject.Data.RectGameObject.TryGetComponentSafe<RectTransform>(out rectTransform2);
					rectTransform.CopyFrom(rectTransform2);
				}
			}
			await Task.Yield();
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x00088B4C File Offset: 0x00086D4C
		internal void MoveNovaTransforms(List<FObject> currPage)
		{
			Transform transform = MonoBehExtensions.CreateEmptyGameObject("tempParent", this.monoBeh.transform).transform;
			foreach (FObject fobject in currPage)
			{
				if (!(fobject.Data.GameObject == null) && !(fobject.Data.RectGameObject == null))
				{
					RectTransform rectTransform;
					fobject.Data.RectGameObject.TryGetComponentSafe<RectTransform>(out rectTransform);
					fobject.Data.UguiTransformData = UguiTransformData.Create(rectTransform);
				}
			}
			transform.gameObject.Destroy();
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x00088C08 File Offset: 0x00086E08
		public async Task SetNovaAnchors(List<FObject> fobjects)
		{
			int total = fobjects.Count;
			int processed = 0;
			Action <>9__3;
			foreach (FrameGroup frameGroup in (from x in fobjects
				group x by x.Data.RootFrame).Select<IGrouping<SyncData, FObject>, FrameGroup>(delegate(IGrouping<SyncData, FObject> g)
			{
				FrameGroup frameGroup2 = default(FrameGroup);
				frameGroup2.Childs = g.Select<FObject, FObject>((FObject x) => x).ToList<FObject>();
				frameGroup2.RootFrame = g.First<FObject>();
				return frameGroup2;
			}))
			{
				if (!(frameGroup.RootFrame.Data.RectGameObject == null))
				{
					List<FObject> childs = frameGroup.Childs;
					Action action;
					if ((action = <>9__3) == null)
					{
						action = (<>9__3 = delegate
						{
							int processed2 = processed;
							processed = processed2 + 1;
						});
					}
					this.SetNovaAnchorsRoutine(childs, action);
				}
			}
			int tempCount = -1;
			while (FcuLogger.WriteLogBeforeEqual(ref processed, ref total, FcuLocKey.log_set_anchors.Localize(new object[] { processed, total }), ref tempCount))
			{
				await Task.Delay(1000);
			}
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x00088C54 File Offset: 0x00086E54
		private async Task SetNovaAnchorsRoutine(List<FObject> fobjects, Action onProcess)
		{
			await Task.Yield();
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x00088C90 File Offset: 0x00086E90
		internal async Task RestoreNovaFramePositions(List<FObject> fobjects)
		{
			IEnumerable<FrameGroup> enumerable = (from x in fobjects
				group x by x.Data.RootFrame).Select<IGrouping<SyncData, FObject>, FrameGroup>(delegate(IGrouping<SyncData, FObject> g)
			{
				FrameGroup frameGroup2 = default(FrameGroup);
				frameGroup2.Childs = g.Select<FObject, FObject>((FObject x) => x).ToList<FObject>();
				frameGroup2.RootFrame = g.First<FObject>();
				return frameGroup2;
			});
			foreach (FrameGroup frameGroup in enumerable)
			{
				if (!(frameGroup.RootFrame.Data.GameObject == null))
				{
					await Task.Delay(100);
				}
			}
			IEnumerator<FrameGroup> enumerator = null;
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x00088CD4 File Offset: 0x00086ED4
		private void SetFigmaRotation(FObject fobject, GameObject target)
		{
			target.GetComponent<Transform>().SetRotation(fobject.Data.FRect.absoluteAngle);
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x00088D00 File Offset: 0x00086F00
		internal async Task SetSiblingIndex(List<FObject> fobjects)
		{
			foreach (FObject fobject in fobjects)
			{
				if (!(fobject.Data.GameObject == null))
				{
					fobject.Data.GameObject.transform.SetSiblingIndex(fobject.Data.SiblingIndex);
				}
			}
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x00088D44 File Offset: 0x00086F44
		internal async Task SetStretchAllIfNeeded(List<FObject> fobjects)
		{
			if (this.monoBeh.Settings.MainSettings.PositioningMode == PositioningMode.GameView)
			{
				IEnumerable<FObject> frames = fobjects.Where<FObject>((FObject x) => x.ContainsTag(FcuTag.Frame));
				await Task.Yield();
				var frameSizeGroups = from x in frames
					group x by x.Size into @group
					select new
					{
						Size = @group.Key,
						Count = @group.Count<FObject>()
					};
				await Task.Yield();
				var anon = frameSizeGroups.OrderByDescending(x => x.Count).FirstOrDefault();
				if (anon.Size.x > 0f && anon.Size.y > 0f)
				{
					this.monoBeh.DelegateHolder.SetGameViewSize(anon.Size);
				}
				foreach (FObject fobject in frames)
				{
					if (!(fobject.Data.GameObject == null))
					{
						RectTransform component = fobject.Data.GameObject.GetComponent<RectTransform>();
						component.SetSmartAnchor(AnchorType.StretchAll);
						component.offsetMin = new Vector2(0f, 0f);
						component.offsetMax = new Vector2(0f, 0f);
						component.localScale = Vector3.one;
					}
				}
				frames = null;
				frameSizeGroups = null;
			}
		}
	}
}
