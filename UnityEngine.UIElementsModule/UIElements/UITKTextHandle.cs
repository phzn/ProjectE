using System;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements.Experimental;

namespace UnityEngine.UIElements
{
	// Token: 0x0200036C RID: 876
	internal class UITKTextHandle : TextHandle
	{
		// Token: 0x06001D1C RID: 7452 RVA: 0x000712FB File Offset: 0x0006F4FB
		public UITKTextHandle(TextElement te)
		{
			this.m_TextElement = te;
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06001D1D RID: 7453 RVA: 0x00071328 File Offset: 0x0006F528
		// (set) Token: 0x06001D1E RID: 7454 RVA: 0x00071330 File Offset: 0x0006F530
		public Vector2 MeasuredSizes { get; set; }

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06001D1F RID: 7455 RVA: 0x00071339 File Offset: 0x0006F539
		// (set) Token: 0x06001D20 RID: 7456 RVA: 0x00071341 File Offset: 0x0006F541
		public Vector2 RoundedSizes { get; set; }

		// Token: 0x06001D21 RID: 7457 RVA: 0x0007134C File Offset: 0x0006F54C
		public float ComputeTextWidth(string textToMeasure, bool wordWrap, float width, float height)
		{
			this.ConvertUssToTextGenerationSettings(TextHandle.s_LayoutSettings);
			TextHandle.s_LayoutSettings.text = textToMeasure;
			TextHandle.s_LayoutSettings.screenRect = new Rect(0f, 0f, width, height);
			TextHandle.s_LayoutSettings.wordWrap = wordWrap;
			return base.ComputeTextWidth(TextHandle.s_LayoutSettings);
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x000713A8 File Offset: 0x0006F5A8
		public float ComputeTextHeight(string textToMeasure, float width, float height)
		{
			this.ConvertUssToTextGenerationSettings(TextHandle.s_LayoutSettings);
			TextHandle.s_LayoutSettings.text = textToMeasure;
			TextHandle.s_LayoutSettings.screenRect = new Rect(0f, 0f, width, height);
			return base.ComputeTextHeight(TextHandle.s_LayoutSettings);
		}

		// Token: 0x06001D23 RID: 7459 RVA: 0x000713F8 File Offset: 0x0006F5F8
		public TextInfo Update()
		{
			this.ConvertUssToTextGenerationSettings(this.textGenerationSettings);
			Vector2 vector = this.m_TextElement.contentRect.size;
			bool flag = Mathf.Abs(vector.x - this.RoundedSizes.x) < 0.01f && Mathf.Abs(vector.y - this.RoundedSizes.y) < 0.01f;
			if (flag)
			{
				vector = this.MeasuredSizes;
			}
			else
			{
				this.RoundedSizes = vector;
				this.MeasuredSizes = vector;
			}
			this.textGenerationSettings.screenRect = new Rect(Vector2.zero, vector);
			base.Update(this.textGenerationSettings);
			this.HandleATag();
			this.HandleLinkTag();
			return base.textInfo;
		}

		// Token: 0x06001D24 RID: 7460 RVA: 0x000714C4 File Offset: 0x0006F6C4
		private void ATagOnPointerUp(PointerUpEvent pue)
		{
			Vector3 vector = pue.localPosition - new Vector3(this.m_TextElement.contentRect.min.x, this.m_TextElement.contentRect.min.y);
			int num = base.FindIntersectingLink(vector, true);
			bool flag = num < 0;
			if (!flag)
			{
				LinkInfo linkInfo = base.textInfo.linkInfo[num];
				bool flag2 = linkInfo.hashCode == 2535353;
				if (flag2)
				{
					bool flag3 = linkInfo.linkId != null && linkInfo.linkIdLength > 0;
					if (flag3)
					{
						string linkId = linkInfo.GetLinkId();
						bool flag4 = Uri.IsWellFormedUriString(linkId, UriKind.Absolute);
						if (flag4)
						{
							Application.OpenURL(linkId);
						}
					}
				}
			}
		}

		// Token: 0x06001D25 RID: 7461 RVA: 0x0007158A File Offset: 0x0006F78A
		private void ATagOnPointerOver(PointerOverEvent _)
		{
			this.isOverridingCursor = false;
		}

		// Token: 0x06001D26 RID: 7462 RVA: 0x00071594 File Offset: 0x0006F794
		private void ATagOnPointerMove(PointerMoveEvent pme)
		{
			Vector3 vector = pme.localPosition - new Vector3(this.m_TextElement.contentRect.min.x, this.m_TextElement.contentRect.min.y);
			int num = base.FindIntersectingLink(vector, true);
			BaseVisualElementPanel baseVisualElementPanel = this.m_TextElement.panel as BaseVisualElementPanel;
			ICursorManager cursorManager = ((baseVisualElementPanel != null) ? baseVisualElementPanel.cursorManager : null);
			bool flag = num >= 0;
			if (flag)
			{
				LinkInfo linkInfo = base.textInfo.linkInfo[num];
				bool flag2 = linkInfo.hashCode == 2535353;
				if (flag2)
				{
					bool flag3 = !this.isOverridingCursor;
					if (flag3)
					{
						this.isOverridingCursor = true;
						if (cursorManager != null)
						{
							cursorManager.SetCursor(new Cursor
							{
								defaultCursorId = 4
							});
						}
					}
					return;
				}
			}
			bool flag4 = this.isOverridingCursor;
			if (flag4)
			{
				if (cursorManager != null)
				{
					cursorManager.SetCursor(this.m_TextElement.computedStyle.cursor);
				}
				this.isOverridingCursor = false;
			}
		}

		// Token: 0x06001D27 RID: 7463 RVA: 0x0007158A File Offset: 0x0006F78A
		private void ATagOnPointerOut(PointerOutEvent _)
		{
			this.isOverridingCursor = false;
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x000716AC File Offset: 0x0006F8AC
		internal void LinkTagOnPointerDown(PointerDownEvent pde)
		{
			Vector3 vector = pde.localPosition - new Vector3(this.m_TextElement.contentRect.min.x, this.m_TextElement.contentRect.min.y);
			int num = base.FindIntersectingLink(vector, true);
			bool flag = num < 0;
			if (!flag)
			{
				LinkInfo linkInfo = base.textInfo.linkInfo[num];
				bool flag2 = linkInfo.hashCode != 2535353;
				if (flag2)
				{
					bool flag3 = linkInfo.linkId != null && linkInfo.linkIdLength > 0;
					if (flag3)
					{
						using (PointerDownLinkTagEvent pooled = PointerDownLinkTagEvent.GetPooled(pde, linkInfo.GetLinkId(), linkInfo.GetLinkText(base.textInfo)))
						{
							pooled.target = this.m_TextElement;
							this.m_TextElement.SendEvent(pooled);
						}
					}
				}
			}
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x000717B4 File Offset: 0x0006F9B4
		internal void LinkTagOnPointerUp(PointerUpEvent pue)
		{
			Vector3 vector = pue.localPosition - new Vector3(this.m_TextElement.contentRect.min.x, this.m_TextElement.contentRect.min.y);
			int num = base.FindIntersectingLink(vector, true);
			bool flag = num < 0;
			if (!flag)
			{
				LinkInfo linkInfo = base.textInfo.linkInfo[num];
				bool flag2 = linkInfo.hashCode != 2535353;
				if (flag2)
				{
					bool flag3 = linkInfo.linkId != null && linkInfo.linkIdLength > 0;
					if (flag3)
					{
						using (PointerUpLinkTagEvent pooled = PointerUpLinkTagEvent.GetPooled(pue, linkInfo.GetLinkId(), linkInfo.GetLinkText(base.textInfo)))
						{
							pooled.target = this.m_TextElement;
							this.m_TextElement.SendEvent(pooled);
						}
					}
				}
			}
		}

		// Token: 0x06001D2A RID: 7466 RVA: 0x000718BC File Offset: 0x0006FABC
		internal void LinkTagOnPointerMove(PointerMoveEvent pme)
		{
			Vector3 vector = pme.localPosition - new Vector3(this.m_TextElement.contentRect.min.x, this.m_TextElement.contentRect.min.y);
			int num = base.FindIntersectingLink(vector, true);
			bool flag = num >= 0;
			if (flag)
			{
				LinkInfo linkInfo = base.textInfo.linkInfo[num];
				bool flag2 = linkInfo.hashCode != 2535353;
				if (flag2)
				{
					bool flag3 = this.currentLinkIDHash == -1;
					if (flag3)
					{
						this.currentLinkIDHash = linkInfo.hashCode;
						using (PointerOverLinkTagEvent pooled = PointerOverLinkTagEvent.GetPooled(pme, linkInfo.GetLinkId(), linkInfo.GetLinkText(base.textInfo)))
						{
							pooled.target = this.m_TextElement;
							this.m_TextElement.SendEvent(pooled);
						}
						return;
					}
					bool flag4 = this.currentLinkIDHash == linkInfo.hashCode;
					if (flag4)
					{
						using (PointerMoveLinkTagEvent pooled2 = PointerMoveLinkTagEvent.GetPooled(pme, linkInfo.GetLinkId(), linkInfo.GetLinkText(base.textInfo)))
						{
							pooled2.target = this.m_TextElement;
							this.m_TextElement.SendEvent(pooled2);
						}
						return;
					}
				}
			}
			bool flag5 = this.currentLinkIDHash != -1;
			if (flag5)
			{
				this.currentLinkIDHash = -1;
				using (PointerOutLinkTagEvent pooled3 = PointerOutLinkTagEvent.GetPooled(pme, string.Empty))
				{
					pooled3.target = this.m_TextElement;
					this.m_TextElement.SendEvent(pooled3);
				}
			}
		}

		// Token: 0x06001D2B RID: 7467 RVA: 0x00071A9C File Offset: 0x0006FC9C
		private void LinkTagOnPointerOut(PointerOutEvent poe)
		{
			bool flag = this.currentLinkIDHash != -1;
			if (flag)
			{
				using (PointerOutLinkTagEvent pooled = PointerOutLinkTagEvent.GetPooled(poe, string.Empty))
				{
					pooled.target = this.m_TextElement;
					this.m_TextElement.SendEvent(pooled);
				}
				this.currentLinkIDHash = -1;
			}
		}

		// Token: 0x06001D2C RID: 7468 RVA: 0x00071B08 File Offset: 0x0006FD08
		private void HandleLinkTag()
		{
			for (int i = 0; i < base.textInfo.linkCount; i++)
			{
				LinkInfo linkInfo = base.textInfo.linkInfo[i];
				bool flag = linkInfo.hashCode != 2535353;
				if (flag)
				{
					this.m_TextElement.RegisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.LinkTagOnPointerDown), TrickleDown.TrickleDown);
					this.m_TextElement.RegisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.LinkTagOnPointerUp), TrickleDown.TrickleDown);
					this.m_TextElement.RegisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.LinkTagOnPointerMove), TrickleDown.TrickleDown);
					this.m_TextElement.RegisterCallback<PointerOutEvent>(new EventCallback<PointerOutEvent>(this.LinkTagOnPointerOut), TrickleDown.TrickleDown);
					this.hasLinkTag = true;
					return;
				}
			}
			bool flag2 = this.hasLinkTag;
			if (flag2)
			{
				this.hasLinkTag = false;
				this.m_TextElement.UnregisterCallback<PointerDownEvent>(new EventCallback<PointerDownEvent>(this.LinkTagOnPointerDown), TrickleDown.TrickleDown);
				this.m_TextElement.UnregisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.LinkTagOnPointerUp), TrickleDown.TrickleDown);
				this.m_TextElement.UnregisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.LinkTagOnPointerMove), TrickleDown.TrickleDown);
				this.m_TextElement.UnregisterCallback<PointerOutEvent>(new EventCallback<PointerOutEvent>(this.LinkTagOnPointerOut), TrickleDown.TrickleDown);
				return;
			}
		}

		// Token: 0x06001D2D RID: 7469 RVA: 0x00071C48 File Offset: 0x0006FE48
		private void HandleATag()
		{
			for (int i = 0; i < base.textInfo.linkCount; i++)
			{
				LinkInfo linkInfo = base.textInfo.linkInfo[i];
				bool flag = linkInfo.hashCode == 2535353;
				if (flag)
				{
					this.m_TextElement.RegisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.ATagOnPointerUp), TrickleDown.TrickleDown);
					bool flag2 = this.m_TextElement.panel.contextType == ContextType.Editor;
					if (flag2)
					{
						this.m_TextElement.RegisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.ATagOnPointerMove), TrickleDown.TrickleDown);
						this.m_TextElement.RegisterCallback<PointerOverEvent>(new EventCallback<PointerOverEvent>(this.ATagOnPointerOver), TrickleDown.TrickleDown);
						this.m_TextElement.RegisterCallback<PointerOutEvent>(new EventCallback<PointerOutEvent>(this.ATagOnPointerOut), TrickleDown.TrickleDown);
					}
					this.hasATag = true;
					return;
				}
			}
			bool flag3 = this.hasATag;
			if (flag3)
			{
				this.hasATag = false;
				this.m_TextElement.UnregisterCallback<PointerUpEvent>(new EventCallback<PointerUpEvent>(this.ATagOnPointerUp), TrickleDown.TrickleDown);
				bool flag4 = this.m_TextElement.panel.contextType == ContextType.Editor;
				if (flag4)
				{
					this.m_TextElement.UnregisterCallback<PointerMoveEvent>(new EventCallback<PointerMoveEvent>(this.ATagOnPointerMove), TrickleDown.TrickleDown);
					this.m_TextElement.UnregisterCallback<PointerOverEvent>(new EventCallback<PointerOverEvent>(this.ATagOnPointerOver), TrickleDown.TrickleDown);
					this.m_TextElement.UnregisterCallback<PointerOutEvent>(new EventCallback<PointerOutEvent>(this.ATagOnPointerOut), TrickleDown.TrickleDown);
				}
				return;
			}
		}

		// Token: 0x06001D2E RID: 7470 RVA: 0x00071DC4 File Offset: 0x0006FFC4
		private unsafe TextOverflowMode GetTextOverflowMode()
		{
			ComputedStyle computedStyle = *this.m_TextElement.computedStyle;
			bool flag = computedStyle.textOverflow == TextOverflow.Clip;
			TextOverflowMode textOverflowMode;
			if (flag)
			{
				textOverflowMode = TextOverflowMode.Masking;
			}
			else
			{
				bool flag2 = computedStyle.textOverflow != TextOverflow.Ellipsis;
				if (flag2)
				{
					textOverflowMode = TextOverflowMode.Overflow;
				}
				else
				{
					bool flag3 = !this.TextLibraryCanElide();
					if (flag3)
					{
						textOverflowMode = TextOverflowMode.Masking;
					}
					else
					{
						bool flag4 = computedStyle.overflow == OverflowInternal.Hidden;
						if (flag4)
						{
							textOverflowMode = TextOverflowMode.Ellipsis;
						}
						else
						{
							textOverflowMode = TextOverflowMode.Overflow;
						}
					}
				}
			}
			return textOverflowMode;
		}

		// Token: 0x06001D2F RID: 7471 RVA: 0x00071E38 File Offset: 0x00070038
		internal unsafe void ConvertUssToTextGenerationSettings(TextGenerationSettings tgs)
		{
			ComputedStyle computedStyle = *this.m_TextElement.computedStyle;
			tgs.textSettings = TextUtilities.GetTextSettingsFrom(this.m_TextElement);
			bool flag = tgs.textSettings == null;
			if (!flag)
			{
				tgs.fontAsset = TextUtilities.GetFontAsset(this.m_TextElement);
				bool flag2 = tgs.fontAsset == null;
				if (!flag2)
				{
					tgs.material = tgs.fontAsset.material;
					tgs.screenRect = new Rect(0f, 0f, this.m_TextElement.contentRect.width, this.m_TextElement.contentRect.height);
					tgs.extraPadding = this.GetTextEffectPadding(tgs.fontAsset);
					tgs.text = ((this.m_TextElement.isElided && !this.TextLibraryCanElide()) ? this.m_TextElement.elidedText : this.m_TextElement.renderedText);
					tgs.fontSize = ((computedStyle.fontSize.value > 0f) ? computedStyle.fontSize.value : ((float)tgs.fontAsset.faceInfo.pointSize));
					tgs.fontStyle = TextGeneratorUtilities.LegacyStyleToNewStyle(computedStyle.unityFontStyleAndWeight);
					tgs.textAlignment = TextGeneratorUtilities.LegacyAlignmentToNewAlignment(computedStyle.unityTextAlign);
					tgs.wordWrap = computedStyle.whiteSpace == WhiteSpace.Normal;
					tgs.wordWrappingRatio = 0.4f;
					tgs.richText = this.m_TextElement.enableRichText;
					tgs.overflowMode = this.GetTextOverflowMode();
					tgs.characterSpacing = computedStyle.letterSpacing.value;
					tgs.wordSpacing = computedStyle.wordSpacing.value;
					tgs.paragraphSpacing = computedStyle.unityParagraphSpacing.value;
					tgs.color = computedStyle.color;
					tgs.shouldConvertToLinearSpace = false;
					tgs.isRightToLeft = this.m_TextElement.localLanguageDirection == LanguageDirection.RTL;
					tgs.parseControlCharacters = this.m_TextElement.parseEscapeSequences;
					tgs.inverseYAxis = true;
				}
			}
		}

		// Token: 0x06001D30 RID: 7472 RVA: 0x00072058 File Offset: 0x00070258
		internal bool TextLibraryCanElide()
		{
			return this.m_TextElement.computedStyle.unityTextOverflowPosition == TextOverflowPosition.End;
		}

		// Token: 0x06001D31 RID: 7473 RVA: 0x00072080 File Offset: 0x00070280
		internal unsafe float GetTextEffectPadding(FontAsset fontAsset)
		{
			ComputedStyle computedStyle = *this.m_TextElement.computedStyle;
			float num = computedStyle.unityTextOutlineWidth / 2f;
			float num2 = Mathf.Abs(computedStyle.textShadow.offset.x);
			float num3 = Mathf.Abs(computedStyle.textShadow.offset.y);
			float num4 = Mathf.Abs(computedStyle.textShadow.blurRadius);
			bool flag = num <= 0f && num2 <= 0f && num3 <= 0f && num4 <= 0f;
			float num5;
			if (flag)
			{
				num5 = UITKTextHandle.k_MinPadding;
			}
			else
			{
				float num6 = Mathf.Max(num2 + num4, num);
				float num7 = Mathf.Max(num3 + num4, num);
				float num8 = Mathf.Max(num6, num7) + UITKTextHandle.k_MinPadding;
				float num9 = TextUtilities.ConvertPixelUnitsToTextCoreRelativeUnits(this.m_TextElement, fontAsset);
				int num10 = fontAsset.atlasPadding + 1;
				num5 = Mathf.Min(num8 * num9 * (float)num10, (float)num10);
			}
			return num5;
		}

		// Token: 0x04000C4C RID: 3148
		private TextElement m_TextElement;

		// Token: 0x04000C4D RID: 3149
		internal bool isOverridingCursor = false;

		// Token: 0x04000C4E RID: 3150
		internal int currentLinkIDHash = -1;

		// Token: 0x04000C4F RID: 3151
		internal bool hasLinkTag = false;

		// Token: 0x04000C50 RID: 3152
		internal bool hasATag = false;

		// Token: 0x04000C51 RID: 3153
		internal static readonly float k_MinPadding = 6f;
	}
}
