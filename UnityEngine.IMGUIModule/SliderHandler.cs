using System;

namespace UnityEngine
{
	// Token: 0x0200003B RID: 59
	internal struct SliderHandler
	{
		// Token: 0x06000424 RID: 1060 RVA: 0x0000FA10 File Offset: 0x0000DC10
		public SliderHandler(Rect position, float currentValue, float size, float start, float end, GUIStyle slider, GUIStyle thumb, bool horiz, int id, GUIStyle thumbExtent = null)
		{
			this.position = position;
			this.currentValue = currentValue;
			this.size = size;
			this.start = start;
			this.end = end;
			this.slider = slider;
			this.thumb = thumb;
			this.thumbExtent = thumbExtent;
			this.horiz = horiz;
			this.id = id;
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0000FA6C File Offset: 0x0000DC6C
		public float Handle()
		{
			bool flag = this.slider == null || this.thumb == null;
			float num;
			if (flag)
			{
				num = this.currentValue;
			}
			else
			{
				EventType eventType = this.CurrentEventType();
				EventType eventType2 = eventType;
				switch (eventType2)
				{
				case EventType.MouseDown:
					return this.OnMouseDown();
				case EventType.MouseUp:
					return this.OnMouseUp();
				case EventType.MouseMove:
					break;
				case EventType.MouseDrag:
					return this.OnMouseDrag();
				default:
					if (eventType2 == EventType.Repaint)
					{
						return this.OnRepaint();
					}
					break;
				}
				num = this.currentValue;
			}
			return num;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0000FAF0 File Offset: 0x0000DCF0
		private float OnMouseDown()
		{
			Rect rect = this.ThumbSelectionRect();
			bool flag = GUIUtility.HitTest(rect, this.CurrentEvent());
			Rect zero = Rect.zero;
			zero.xMin = Math.Min(this.position.xMin, rect.xMin);
			zero.xMax = Math.Max(this.position.xMax, rect.xMax);
			zero.yMin = Math.Min(this.position.yMin, rect.yMin);
			zero.yMax = Math.Max(this.position.yMax, rect.yMax);
			bool flag2 = this.IsEmptySlider() || (!GUIUtility.HitTest(zero, this.CurrentEvent()) && !flag);
			float num;
			if (flag2)
			{
				num = this.currentValue;
			}
			else
			{
				GUI.scrollTroughSide = 0;
				GUIUtility.hotControl = this.id;
				this.CurrentEvent().Use();
				bool flag3 = flag;
				if (flag3)
				{
					this.StartDraggingWithValue(this.ClampedCurrentValue());
					num = this.currentValue;
				}
				else
				{
					GUI.changed = true;
					bool flag4 = this.SupportsPageMovements();
					if (flag4)
					{
						this.SliderState().isDragging = false;
						GUI.nextScrollStepTime = SystemClock.now.AddMilliseconds(250.0);
						GUI.scrollTroughSide = this.CurrentScrollTroughSide();
						num = this.PageMovementValue();
					}
					else
					{
						float num2 = this.ValueForCurrentMousePosition();
						this.StartDraggingWithValue(num2);
						num = this.Clamp(num2);
					}
				}
			}
			return num;
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0000FC84 File Offset: 0x0000DE84
		private float OnMouseDrag()
		{
			bool flag = GUIUtility.hotControl != this.id;
			float num;
			if (flag)
			{
				num = this.currentValue;
			}
			else
			{
				SliderState sliderState = this.SliderState();
				bool flag2 = !sliderState.isDragging;
				if (flag2)
				{
					num = this.currentValue;
				}
				else
				{
					GUI.changed = true;
					this.CurrentEvent().Use();
					float num2 = this.MousePosition() - sliderState.dragStartPos;
					float num3 = sliderState.dragStartValue + num2 / this.ValuesPerPixel();
					num = this.Clamp(num3);
				}
			}
			return num;
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0000FD10 File Offset: 0x0000DF10
		private float OnMouseUp()
		{
			bool flag = GUIUtility.hotControl == this.id;
			if (flag)
			{
				this.CurrentEvent().Use();
				GUIUtility.hotControl = 0;
			}
			return this.currentValue;
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0000FD50 File Offset: 0x0000DF50
		private float OnRepaint()
		{
			bool flag = GUIUtility.HitTest(this.position, this.CurrentEvent());
			this.slider.Draw(this.position, GUIContent.none, this.id, false, flag);
			bool flag2 = this.currentValue >= Mathf.Min(this.start, this.end) && this.currentValue <= Mathf.Max(this.start, this.end);
			if (flag2)
			{
				bool flag3 = this.thumbExtent != null;
				if (flag3)
				{
					this.thumbExtent.Draw(this.ThumbExtRect(), GUIContent.none, this.id, false, flag);
				}
				this.thumb.Draw(this.ThumbRect(), GUIContent.none, this.id, false, flag);
			}
			bool flag4 = GUIUtility.hotControl != this.id || !flag || this.IsEmptySlider();
			float num;
			if (flag4)
			{
				num = this.currentValue;
			}
			else
			{
				Rect rect = this.ThumbRect();
				bool flag5 = this.horiz;
				if (flag5)
				{
					rect.y = this.position.y;
					rect.height = this.position.height;
				}
				else
				{
					rect.x = this.position.x;
					rect.width = this.position.width;
				}
				bool flag6 = GUIUtility.HitTest(rect, this.CurrentEvent());
				if (flag6)
				{
					bool flag7 = GUI.scrollTroughSide != 0;
					if (flag7)
					{
						GUIUtility.hotControl = 0;
					}
					num = this.currentValue;
				}
				else
				{
					GUI.InternalRepaintEditorWindow();
					bool flag8 = SystemClock.now < GUI.nextScrollStepTime;
					if (flag8)
					{
						num = this.currentValue;
					}
					else
					{
						bool flag9 = this.CurrentScrollTroughSide() != GUI.scrollTroughSide;
						if (flag9)
						{
							num = this.currentValue;
						}
						else
						{
							GUI.nextScrollStepTime = SystemClock.now.AddMilliseconds(30.0);
							bool flag10 = this.SupportsPageMovements();
							if (flag10)
							{
								this.SliderState().isDragging = false;
								GUI.changed = true;
								num = this.PageMovementValue();
							}
							else
							{
								num = this.ClampedCurrentValue();
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000FF88 File Offset: 0x0000E188
		private EventType CurrentEventType()
		{
			return this.CurrentEvent().GetTypeForControl(this.id);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000FFAC File Offset: 0x0000E1AC
		private int CurrentScrollTroughSide()
		{
			float num = (this.horiz ? this.CurrentEvent().mousePosition.x : this.CurrentEvent().mousePosition.y);
			float num2 = (this.horiz ? this.ThumbRect().x : this.ThumbRect().y);
			return (num > num2) ? 1 : (-1);
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00010018 File Offset: 0x0000E218
		private bool IsEmptySlider()
		{
			return this.start == this.end;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00010038 File Offset: 0x0000E238
		private bool SupportsPageMovements()
		{
			return this.size != 0f && GUI.usePageScrollbars;
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00010060 File Offset: 0x0000E260
		private float PageMovementValue()
		{
			float num = this.currentValue;
			int num2 = ((this.start > this.end) ? (-1) : 1);
			bool flag = this.MousePosition() > this.PageUpMovementBound();
			if (flag)
			{
				num += this.size * (float)num2 * 0.9f;
			}
			else
			{
				num -= this.size * (float)num2 * 0.9f;
			}
			return this.Clamp(num);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x000100CC File Offset: 0x0000E2CC
		private float PageUpMovementBound()
		{
			bool flag = this.horiz;
			float num;
			if (flag)
			{
				num = this.ThumbRect().xMax - this.position.x;
			}
			else
			{
				num = this.ThumbRect().yMax - this.position.y;
			}
			return num;
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00010128 File Offset: 0x0000E328
		private Event CurrentEvent()
		{
			return Event.current;
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00010140 File Offset: 0x0000E340
		private float ValueForCurrentMousePosition()
		{
			bool flag = this.horiz;
			float num;
			if (flag)
			{
				num = (this.MousePosition() - this.ThumbRect().width * 0.5f) / this.ValuesPerPixel() + this.start - this.size * 0.5f;
			}
			else
			{
				num = (this.MousePosition() - this.ThumbRect().height * 0.5f) / this.ValuesPerPixel() + this.start - this.size * 0.5f;
			}
			return num;
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x000101CC File Offset: 0x0000E3CC
		private float Clamp(float value)
		{
			return Mathf.Clamp(value, this.MinValue(), this.MaxValue());
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x000101F0 File Offset: 0x0000E3F0
		private Rect ThumbSelectionRect()
		{
			return this.ThumbRect();
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0001020C File Offset: 0x0000E40C
		private void StartDraggingWithValue(float dragStartValue)
		{
			SliderState sliderState = this.SliderState();
			sliderState.dragStartPos = this.MousePosition();
			sliderState.dragStartValue = dragStartValue;
			sliderState.isDragging = true;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0001023C File Offset: 0x0000E43C
		private SliderState SliderState()
		{
			return (SliderState)GUIUtility.GetStateObject(typeof(SliderState), this.id);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00010268 File Offset: 0x0000E468
		private Rect ThumbExtRect()
		{
			return new Rect(0f, 0f, this.thumbExtent.fixedWidth, this.thumbExtent.fixedHeight)
			{
				center = this.ThumbRect().center
			};
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x000102BC File Offset: 0x0000E4BC
		private Rect ThumbRect()
		{
			return this.horiz ? this.HorizontalThumbRect() : this.VerticalThumbRect();
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x000102E4 File Offset: 0x0000E4E4
		private Rect VerticalThumbRect()
		{
			Rect rect = this.thumb.margin.Remove(this.slider.padding.Remove(this.position));
			float num = ((this.thumb.fixedWidth != 0f) ? this.thumb.fixedWidth : rect.width);
			float num2 = this.ThumbSize();
			float num3 = this.ValuesPerPixel();
			bool flag = this.start < this.end;
			Rect rect2;
			if (flag)
			{
				rect2 = new Rect(rect.x, (this.ClampedCurrentValue() - this.start) * num3 + rect.y, num, this.size * num3 + num2);
			}
			else
			{
				rect2 = new Rect(rect.x, (this.ClampedCurrentValue() + this.size - this.start) * num3 + rect.y, num, this.size * -num3 + num2);
			}
			return rect2;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x000103D4 File Offset: 0x0000E5D4
		private Rect HorizontalThumbRect()
		{
			Rect rect = this.thumb.margin.Remove(this.slider.padding.Remove(this.position));
			float num = ((this.thumb.fixedHeight != 0f) ? this.thumb.fixedHeight : rect.height);
			float num2 = this.ThumbSize();
			float num3 = this.ValuesPerPixel();
			bool flag = this.start < this.end;
			Rect rect2;
			if (flag)
			{
				rect2 = new Rect((this.ClampedCurrentValue() - this.start) * num3 + rect.x, rect.y, this.size * num3 + num2, num);
			}
			else
			{
				rect2 = new Rect((this.ClampedCurrentValue() + this.size - this.start) * num3 + rect.x, rect.y, this.size * -num3 + num2, num);
			}
			return rect2;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x000104C0 File Offset: 0x0000E6C0
		private float ClampedCurrentValue()
		{
			return this.Clamp(this.currentValue);
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x000104E0 File Offset: 0x0000E6E0
		private float MousePosition()
		{
			bool flag = this.horiz;
			float num;
			if (flag)
			{
				num = this.CurrentEvent().mousePosition.x - this.position.x;
			}
			else
			{
				num = this.CurrentEvent().mousePosition.y - this.position.y;
			}
			return num;
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00010540 File Offset: 0x0000E740
		private float ValuesPerPixel()
		{
			float num = ((this.end == this.start) ? 1f : (this.end - this.start));
			bool flag = this.horiz;
			float num2;
			if (flag)
			{
				num2 = (this.position.width - (float)this.slider.padding.horizontal - this.ThumbSize()) / num;
			}
			else
			{
				num2 = (this.position.height - (float)this.slider.padding.vertical - this.ThumbSize()) / num;
			}
			return num2;
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x000105D4 File Offset: 0x0000E7D4
		private float ThumbSize()
		{
			bool flag = this.horiz;
			float num;
			if (flag)
			{
				num = ((this.thumb.fixedWidth != 0f) ? this.thumb.fixedWidth : ((float)this.thumb.padding.horizontal));
			}
			else
			{
				num = ((this.thumb.fixedHeight != 0f) ? this.thumb.fixedHeight : ((float)this.thumb.padding.vertical));
			}
			return num;
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00010654 File Offset: 0x0000E854
		private float MaxValue()
		{
			return Mathf.Max(this.start, this.end) - this.size;
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00010680 File Offset: 0x0000E880
		private float MinValue()
		{
			return Mathf.Min(this.start, this.end);
		}

		// Token: 0x04000136 RID: 310
		private readonly Rect position;

		// Token: 0x04000137 RID: 311
		private readonly float currentValue;

		// Token: 0x04000138 RID: 312
		private readonly float size;

		// Token: 0x04000139 RID: 313
		private readonly float start;

		// Token: 0x0400013A RID: 314
		private readonly float end;

		// Token: 0x0400013B RID: 315
		private readonly GUIStyle slider;

		// Token: 0x0400013C RID: 316
		private readonly GUIStyle thumb;

		// Token: 0x0400013D RID: 317
		private readonly GUIStyle thumbExtent;

		// Token: 0x0400013E RID: 318
		private readonly bool horiz;

		// Token: 0x0400013F RID: 319
		private readonly int id;
	}
}
