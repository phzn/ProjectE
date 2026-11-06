using System;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;

namespace UnityEngine.UIElements
{
	// Token: 0x0200036F RID: 879
	public class TextElement : BindableElement, ITextElement, INotifyValueChanged<string>, ITextEdition, ITextElementExperimentalFeatures, IExperimentalFeatures, ITextSelection
	{
		// Token: 0x06001D3C RID: 7484 RVA: 0x0007266C File Offset: 0x0007086C
		public TextElement()
		{
			base.requireMeasureFunction = true;
			base.tabIndex = -1;
			this.uitkTextHandle = new UITKTextHandle(this);
			base.AddToClassList(TextElement.ussClassName);
			base.generateVisualContent = (Action<MeshGenerationContext>)Delegate.Combine(base.generateVisualContent, new Action<MeshGenerationContext>(this.OnGenerateVisualContent));
			base.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.OnGeometryChanged), TrickleDown.NoTrickleDown);
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06001D3D RID: 7485 RVA: 0x0007277B File Offset: 0x0007097B
		// (set) Token: 0x06001D3E RID: 7486 RVA: 0x00072783 File Offset: 0x00070983
		internal UITKTextHandle uitkTextHandle { get; set; }

		// Token: 0x06001D3F RID: 7487 RVA: 0x0007278C File Offset: 0x0007098C
		private void OnGeometryChanged(GeometryChangedEvent e)
		{
			this.UpdateVisibleText();
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06001D40 RID: 7488 RVA: 0x00072796 File Offset: 0x00070996
		// (set) Token: 0x06001D41 RID: 7489 RVA: 0x0007279E File Offset: 0x0007099E
		public virtual string text
		{
			get
			{
				return ((INotifyValueChanged<string>)this).value;
			}
			set
			{
				((INotifyValueChanged<string>)this).value = value;
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06001D42 RID: 7490 RVA: 0x000727A8 File Offset: 0x000709A8
		// (set) Token: 0x06001D43 RID: 7491 RVA: 0x000727B0 File Offset: 0x000709B0
		public bool enableRichText
		{
			get
			{
				return this.m_EnableRichText;
			}
			set
			{
				bool flag = this.m_EnableRichText == value;
				if (!flag)
				{
					this.m_EnableRichText = value;
					base.MarkDirtyRepaint();
				}
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06001D44 RID: 7492 RVA: 0x000727DB File Offset: 0x000709DB
		// (set) Token: 0x06001D45 RID: 7493 RVA: 0x000727E4 File Offset: 0x000709E4
		public bool parseEscapeSequences
		{
			get
			{
				return this.m_ParseEscapeSequences;
			}
			set
			{
				bool flag = this.m_ParseEscapeSequences == value;
				if (!flag)
				{
					this.m_ParseEscapeSequences = value;
					base.MarkDirtyRepaint();
				}
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06001D46 RID: 7494 RVA: 0x0007280F File Offset: 0x00070A0F
		// (set) Token: 0x06001D47 RID: 7495 RVA: 0x00072818 File Offset: 0x00070A18
		public bool displayTooltipWhenElided
		{
			get
			{
				return this.m_DisplayTooltipWhenElided;
			}
			set
			{
				bool flag = this.m_DisplayTooltipWhenElided != value;
				if (flag)
				{
					this.m_DisplayTooltipWhenElided = value;
					this.UpdateVisibleText();
					base.MarkDirtyRepaint();
				}
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06001D48 RID: 7496 RVA: 0x0007284D File Offset: 0x00070A4D
		// (set) Token: 0x06001D49 RID: 7497 RVA: 0x00072855 File Offset: 0x00070A55
		public bool isElided { get; private set; }

		// Token: 0x06001D4A RID: 7498 RVA: 0x00072860 File Offset: 0x00070A60
		internal void OnGenerateVisualContent(MeshGenerationContext mgc)
		{
			this.UpdateVisibleText();
			mgc.Text(this);
			bool flag = this.ShouldElide() && this.uitkTextHandle.TextLibraryCanElide();
			if (flag)
			{
				this.isElided = this.uitkTextHandle.IsElided();
			}
			this.UpdateTooltip();
			bool flag2 = this.selection.HasSelection() && this.selectingManipulator.HasFocus();
			if (flag2)
			{
				this.DrawHighlighting(mgc);
			}
			else
			{
				bool flag3 = !this.edition.isReadOnly && this.selection.isSelectable && this.selectingManipulator.RevealCursor();
				if (flag3)
				{
					this.DrawCaret(mgc);
				}
			}
		}

		// Token: 0x06001D4B RID: 7499 RVA: 0x00072910 File Offset: 0x00070B10
		internal string ElideText(string drawText, string ellipsisText, float width, TextOverflowPosition textOverflowPosition)
		{
			float num = base.resolvedStyle.paddingRight;
			bool flag = float.IsNaN(num);
			if (flag)
			{
				num = 0f;
			}
			float num2 = Mathf.Clamp(num, 1f / base.scaledPixelsPerPoint, 1f);
			Vector2 vector = this.MeasureTextSize(drawText, 0f, VisualElement.MeasureMode.Undefined, 0f, VisualElement.MeasureMode.Undefined);
			bool flag2 = vector.x <= width + num2 || string.IsNullOrEmpty(ellipsisText);
			string text;
			if (flag2)
			{
				text = drawText;
			}
			else
			{
				string text2 = ((drawText.Length > 1) ? ellipsisText : drawText);
				Vector2 vector2 = this.MeasureTextSize(text2, 0f, VisualElement.MeasureMode.Undefined, 0f, VisualElement.MeasureMode.Undefined);
				bool flag3 = vector2.x >= width;
				if (flag3)
				{
					text = text2;
				}
				else
				{
					int num3 = drawText.Length - 1;
					int num4 = -1;
					string text3 = drawText;
					int i = ((textOverflowPosition == TextOverflowPosition.Start) ? 1 : 0);
					int num5 = ((textOverflowPosition == TextOverflowPosition.Start || textOverflowPosition == TextOverflowPosition.Middle) ? num3 : (num3 - 1));
					int num6 = (i + num5) / 2;
					while (i <= num5)
					{
						bool flag4 = textOverflowPosition == TextOverflowPosition.Start;
						if (flag4)
						{
							text3 = ellipsisText + drawText.Substring(num6, num3 - (num6 - 1));
						}
						else
						{
							bool flag5 = textOverflowPosition == TextOverflowPosition.End;
							if (flag5)
							{
								text3 = drawText.Substring(0, num6) + ellipsisText;
							}
							else
							{
								bool flag6 = textOverflowPosition == TextOverflowPosition.Middle;
								if (flag6)
								{
									text3 = ((num6 - 1 <= 0) ? "" : drawText.Substring(0, num6 - 1)) + ellipsisText + ((num3 - (num6 - 1) <= 0) ? "" : drawText.Substring(num3 - (num6 - 1)));
								}
							}
						}
						vector = this.MeasureTextSize(text3, 0f, VisualElement.MeasureMode.Undefined, 0f, VisualElement.MeasureMode.Undefined);
						bool flag7 = Math.Abs(vector.x - width) < 1E-30f;
						if (flag7)
						{
							return text3;
						}
						bool flag8 = textOverflowPosition == TextOverflowPosition.Start;
						if (flag8)
						{
							bool flag9 = vector.x > width;
							if (flag9)
							{
								bool flag10 = num4 == num6 - 1;
								if (flag10)
								{
									return ellipsisText + drawText.Substring(num4, num3 - (num4 - 1));
								}
								i = num6 + 1;
							}
							else
							{
								num5 = num6 - 1;
								num4 = num6;
							}
						}
						else
						{
							bool flag11 = textOverflowPosition == TextOverflowPosition.End || textOverflowPosition == TextOverflowPosition.Middle;
							if (flag11)
							{
								bool flag12 = vector.x > width;
								if (flag12)
								{
									bool flag13 = num4 == num6 - 1;
									if (flag13)
									{
										bool flag14 = textOverflowPosition == TextOverflowPosition.End;
										if (flag14)
										{
											return drawText.Substring(0, num4) + ellipsisText;
										}
										return drawText.Substring(0, Mathf.Max(num4 - 1, 0)) + ellipsisText + drawText.Substring(num3 - Mathf.Max(num4 - 1, 0));
									}
									else
									{
										num5 = num6 - 1;
									}
								}
								else
								{
									i = num6 + 1;
									num4 = num6;
								}
							}
						}
						num6 = (i + num5) / 2;
					}
					text = text3;
				}
			}
			return text;
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x00072BE8 File Offset: 0x00070DE8
		private void UpdateTooltip()
		{
			bool flag = this.displayTooltipWhenElided && this.isElided;
			bool flag2 = flag;
			if (flag2)
			{
				base.tooltip = this.text;
				this.m_WasElided = true;
			}
			else
			{
				bool wasElided = this.m_WasElided;
				if (wasElided)
				{
					base.tooltip = null;
					this.m_WasElided = false;
				}
			}
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x00072C40 File Offset: 0x00070E40
		private void UpdateVisibleText()
		{
			bool flag = this.ShouldElide();
			bool flag2 = flag && this.uitkTextHandle.TextLibraryCanElide();
			if (!flag2)
			{
				bool flag3 = flag;
				if (flag3)
				{
					this.elidedText = this.ElideText(this.text, TextElement.k_EllipsisText, base.contentRect.width, base.computedStyle.unityTextOverflowPosition);
					this.isElided = flag && this.elidedText != this.text;
				}
				else
				{
					this.isElided = false;
				}
			}
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x00072CD0 File Offset: 0x00070ED0
		private bool ShouldElide()
		{
			return base.computedStyle.textOverflow == TextOverflow.Ellipsis && base.computedStyle.overflow == OverflowInternal.Hidden;
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06001D4F RID: 7503 RVA: 0x00072D01 File Offset: 0x00070F01
		internal bool hasFocus
		{
			get
			{
				bool flag;
				if (base.elementPanel != null)
				{
					FocusController focusController = base.elementPanel.focusController;
					flag = ((focusController != null) ? focusController.GetLeafFocusedElement() : null) == this;
				}
				else
				{
					flag = false;
				}
				return flag;
			}
		}

		// Token: 0x06001D50 RID: 7504 RVA: 0x00072D28 File Offset: 0x00070F28
		public Vector2 MeasureTextSize(string textToMeasure, float width, VisualElement.MeasureMode widthMode, float height, VisualElement.MeasureMode heightMode)
		{
			return TextUtilities.MeasureVisualElementTextSize(this, textToMeasure, width, widthMode, height, heightMode);
		}

		// Token: 0x06001D51 RID: 7505 RVA: 0x00072D48 File Offset: 0x00070F48
		protected internal override Vector2 DoMeasure(float desiredWidth, VisualElement.MeasureMode widthMode, float desiredHeight, VisualElement.MeasureMode heightMode)
		{
			return this.MeasureTextSize(this.renderedText, desiredWidth, widthMode, desiredHeight, heightMode);
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06001D52 RID: 7506 RVA: 0x00072D6B File Offset: 0x00070F6B
		// (set) Token: 0x06001D53 RID: 7507 RVA: 0x00072D7C File Offset: 0x00070F7C
		string INotifyValueChanged<string>.value
		{
			get
			{
				return this.m_Text ?? string.Empty;
			}
			set
			{
				bool flag = this.m_Text != value;
				if (flag)
				{
					bool flag2 = base.panel != null;
					if (flag2)
					{
						using (ChangeEvent<string> pooled = ChangeEvent<string>.GetPooled(this.text, value))
						{
							pooled.target = this;
							((INotifyValueChanged<string>)this).SetValueWithoutNotify(value);
							this.SendEvent(pooled);
						}
					}
					else
					{
						((INotifyValueChanged<string>)this).SetValueWithoutNotify(value);
					}
				}
			}
		}

		// Token: 0x06001D54 RID: 7508 RVA: 0x00072DFC File Offset: 0x00070FFC
		void INotifyValueChanged<string>.SetValueWithoutNotify(string newValue)
		{
			newValue = ((ITextEdition)this).CullString(newValue);
			bool flag = this.m_Text != newValue;
			if (flag)
			{
				this.renderedText = newValue;
				this.m_Text = newValue;
				base.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Repaint);
				bool flag2 = !string.IsNullOrEmpty(base.viewDataKey);
				if (flag2)
				{
					base.SaveViewData();
				}
			}
			bool flag3 = this.editingManipulator != null;
			if (flag3)
			{
				this.editingManipulator.editingUtilities.text = newValue;
			}
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06001D55 RID: 7509 RVA: 0x0002E015 File Offset: 0x0002C215
		internal ITextEdition edition
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06001D56 RID: 7510 RVA: 0x00072E78 File Offset: 0x00071078
		// (set) Token: 0x06001D57 RID: 7511 RVA: 0x00072E80 File Offset: 0x00071080
		bool ITextEdition.multiline
		{
			get
			{
				return this.m_Multiline;
			}
			set
			{
				bool flag = value != this.m_Multiline;
				if (flag)
				{
					bool flag2 = !this.edition.isReadOnly;
					if (flag2)
					{
						this.editingManipulator.editingUtilities.multiline = value;
					}
					this.m_Multiline = value;
				}
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06001D58 RID: 7512 RVA: 0x00072ECA File Offset: 0x000710CA
		TouchScreenKeyboard ITextEdition.touchScreenKeyboard
		{
			get
			{
				return this.m_TouchScreenKeyboard;
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06001D59 RID: 7513 RVA: 0x00072ED2 File Offset: 0x000710D2
		// (set) Token: 0x06001D5A RID: 7514 RVA: 0x00072EDA File Offset: 0x000710DA
		TouchScreenKeyboardType ITextEdition.keyboardType
		{
			get
			{
				return this.m_KeyboardType;
			}
			set
			{
				this.m_KeyboardType = value;
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06001D5B RID: 7515 RVA: 0x00072EE4 File Offset: 0x000710E4
		// (set) Token: 0x06001D5C RID: 7516 RVA: 0x00072F28 File Offset: 0x00071128
		bool ITextEdition.hideMobileInput
		{
			get
			{
				RuntimePlatform platform = Application.platform;
				RuntimePlatform runtimePlatform = platform;
				if (runtimePlatform <= RuntimePlatform.Android)
				{
					if (runtimePlatform != RuntimePlatform.IPhonePlayer && runtimePlatform != RuntimePlatform.Android)
					{
						goto IL_0032;
					}
				}
				else if (runtimePlatform != RuntimePlatform.WebGLPlayer && runtimePlatform != RuntimePlatform.tvOS)
				{
					goto IL_0032;
				}
				return this.m_HideMobileInput;
				IL_0032:
				return true;
			}
			set
			{
				RuntimePlatform platform = Application.platform;
				RuntimePlatform runtimePlatform = platform;
				if (runtimePlatform <= RuntimePlatform.Android)
				{
					if (runtimePlatform != RuntimePlatform.IPhonePlayer && runtimePlatform != RuntimePlatform.Android)
					{
						goto IL_0032;
					}
				}
				else if (runtimePlatform != RuntimePlatform.WebGLPlayer && runtimePlatform != RuntimePlatform.tvOS)
				{
					goto IL_0032;
				}
				this.m_HideMobileInput = value;
				return;
				IL_0032:
				this.m_HideMobileInput = true;
			}
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06001D5D RID: 7517 RVA: 0x00072F70 File Offset: 0x00071170
		// (set) Token: 0x06001D5E RID: 7518 RVA: 0x00072F88 File Offset: 0x00071188
		bool ITextEdition.isReadOnly
		{
			get
			{
				return this.m_IsReadOnly || !base.enabledInHierarchy;
			}
			set
			{
				bool flag = value == this.m_IsReadOnly;
				if (!flag)
				{
					this.editingManipulator = (value ? null : new TextEditingManipulator(this));
					this.m_IsReadOnly = value;
				}
			}
		}

		// Token: 0x06001D5F RID: 7519 RVA: 0x00072FC0 File Offset: 0x000711C0
		private void ProcessMenuCommand(string command)
		{
			using (ExecuteCommandEvent pooled = CommandEventBase<ExecuteCommandEvent>.GetPooled(command))
			{
				pooled.target = this;
				this.SendEvent(pooled);
			}
		}

		// Token: 0x06001D60 RID: 7520 RVA: 0x00073004 File Offset: 0x00071204
		private void Cut(DropdownMenuAction a)
		{
			this.ProcessMenuCommand("Cut");
		}

		// Token: 0x06001D61 RID: 7521 RVA: 0x00073013 File Offset: 0x00071213
		private void Copy(DropdownMenuAction a)
		{
			this.ProcessMenuCommand("Copy");
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x00073022 File Offset: 0x00071222
		private void Paste(DropdownMenuAction a)
		{
			this.ProcessMenuCommand("Paste");
		}

		// Token: 0x06001D63 RID: 7523 RVA: 0x00073034 File Offset: 0x00071234
		private void BuildContextualMenu(ContextualMenuPopulateEvent evt)
		{
			bool flag = ((evt != null) ? evt.target : null) is TextElement;
			if (flag)
			{
				bool flag2 = !this.edition.isReadOnly;
				if (flag2)
				{
					evt.menu.AppendAction("Cut", new Action<DropdownMenuAction>(this.Cut), new Func<DropdownMenuAction, DropdownMenuAction.Status>(this.CutActionStatus), null);
					evt.menu.AppendAction("Copy", new Action<DropdownMenuAction>(this.Copy), new Func<DropdownMenuAction, DropdownMenuAction.Status>(this.CopyActionStatus), null);
					evt.menu.AppendAction("Paste", new Action<DropdownMenuAction>(this.Paste), new Func<DropdownMenuAction, DropdownMenuAction.Status>(this.PasteActionStatus), null);
				}
				else
				{
					evt.menu.AppendAction("Copy", new Action<DropdownMenuAction>(this.Copy), new Func<DropdownMenuAction, DropdownMenuAction.Status>(this.CopyActionStatus), null);
				}
			}
		}

		// Token: 0x06001D64 RID: 7524 RVA: 0x00073120 File Offset: 0x00071320
		private DropdownMenuAction.Status CutActionStatus(DropdownMenuAction a)
		{
			return (base.enabledInHierarchy && this.selection.HasSelection() && !this.edition.isPassword) ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled;
		}

		// Token: 0x06001D65 RID: 7525 RVA: 0x00073158 File Offset: 0x00071358
		private DropdownMenuAction.Status CopyActionStatus(DropdownMenuAction a)
		{
			return ((!base.enabledInHierarchy || this.selection.HasSelection()) && !this.edition.isPassword) ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled;
		}

		// Token: 0x06001D66 RID: 7526 RVA: 0x00073190 File Offset: 0x00071390
		private DropdownMenuAction.Status PasteActionStatus(DropdownMenuAction a)
		{
			bool flag = this.editingManipulator.editingUtilities.CanPaste();
			return base.enabledInHierarchy ? (flag ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled) : DropdownMenuAction.Status.Hidden;
		}

		// Token: 0x06001D67 RID: 7527 RVA: 0x000731C8 File Offset: 0x000713C8
		[EventInterest(new Type[]
		{
			typeof(ContextualMenuPopulateEvent),
			typeof(FocusInEvent),
			typeof(FocusOutEvent),
			typeof(KeyDownEvent),
			typeof(KeyUpEvent),
			typeof(FocusEvent),
			typeof(BlurEvent),
			typeof(ValidateCommandEvent),
			typeof(ExecuteCommandEvent),
			typeof(PointerDownEvent),
			typeof(PointerUpEvent),
			typeof(PointerMoveEvent),
			typeof(NavigationMoveEvent),
			typeof(NavigationSubmitEvent),
			typeof(NavigationCancelEvent)
		})]
		protected override void ExecuteDefaultActionAtTarget(EventBase evt)
		{
			bool isSelectable = this.selection.isSelectable;
			if (isSelectable)
			{
				TextEditingManipulator textEditingManipulator = this.editingManipulator;
				bool flag = textEditingManipulator != null && textEditingManipulator.editingUtilities.TouchScreenKeyboardShouldBeUsed();
				bool flag2 = !flag || (flag && this.edition.hideMobileInput);
				if (flag2)
				{
					TextSelectingManipulator selectingManipulator = this.selectingManipulator;
					if (selectingManipulator != null)
					{
						selectingManipulator.ExecuteDefaultActionAtTarget(evt);
					}
				}
				bool flag3 = !this.edition.isReadOnly;
				if (flag3)
				{
					TextEditingManipulator textEditingManipulator2 = this.editingManipulator;
					if (textEditingManipulator2 != null)
					{
						textEditingManipulator2.ExecuteDefaultActionAtTarget(evt);
					}
				}
				BaseVisualElementPanel elementPanel = base.elementPanel;
				if (elementPanel != null)
				{
					ContextualMenuManager contextualMenuManager = elementPanel.contextualMenuManager;
					if (contextualMenuManager != null)
					{
						contextualMenuManager.DisplayMenuIfEventMatches(evt, this);
					}
				}
				long? num = ((evt != null) ? new long?(evt.eventTypeId) : null);
				long num2 = EventBase<ContextualMenuPopulateEvent>.TypeId();
				bool flag4 = (num.GetValueOrDefault() == num2) & (num != null);
				if (flag4)
				{
					ContextualMenuPopulateEvent contextualMenuPopulateEvent = evt as ContextualMenuPopulateEvent;
					int count = contextualMenuPopulateEvent.menu.MenuItems().Count;
					this.BuildContextualMenu(contextualMenuPopulateEvent);
					bool flag5 = count > 0 && contextualMenuPopulateEvent.menu.MenuItems().Count > count;
					if (flag5)
					{
						contextualMenuPopulateEvent.menu.InsertSeparator(null, count);
					}
				}
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06001D68 RID: 7528 RVA: 0x0007330B File Offset: 0x0007150B
		// (set) Token: 0x06001D69 RID: 7529 RVA: 0x00073313 File Offset: 0x00071513
		int ITextEdition.maxLength
		{
			get
			{
				return this.m_MaxLength;
			}
			set
			{
				this.m_MaxLength = value;
				this.text = this.edition.CullString(this.text);
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06001D6A RID: 7530 RVA: 0x00073335 File Offset: 0x00071535
		// (set) Token: 0x06001D6B RID: 7531 RVA: 0x0007333D File Offset: 0x0007153D
		bool ITextEdition.isDelayed { get; set; }

		// Token: 0x06001D6C RID: 7532 RVA: 0x00073348 File Offset: 0x00071548
		void ITextEdition.ResetValueAndText()
		{
			this.m_OriginalText = (this.text = null);
		}

		// Token: 0x06001D6D RID: 7533 RVA: 0x00073367 File Offset: 0x00071567
		void ITextEdition.SaveValueAndText()
		{
			this.m_OriginalText = this.text;
		}

		// Token: 0x06001D6E RID: 7534 RVA: 0x00073376 File Offset: 0x00071576
		void ITextEdition.RestoreValueAndText()
		{
			this.text = this.m_OriginalText;
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06001D6F RID: 7535 RVA: 0x00073386 File Offset: 0x00071586
		// (set) Token: 0x06001D70 RID: 7536 RVA: 0x0007338E File Offset: 0x0007158E
		Func<char, bool> ITextEdition.AcceptCharacter { get; set; }

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06001D71 RID: 7537 RVA: 0x00073397 File Offset: 0x00071597
		// (set) Token: 0x06001D72 RID: 7538 RVA: 0x0007339F File Offset: 0x0007159F
		Action<bool> ITextEdition.UpdateScrollOffset { get; set; }

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06001D73 RID: 7539 RVA: 0x000733A8 File Offset: 0x000715A8
		// (set) Token: 0x06001D74 RID: 7540 RVA: 0x000733B0 File Offset: 0x000715B0
		Action ITextEdition.UpdateValueFromText { get; set; }

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06001D75 RID: 7541 RVA: 0x000733B9 File Offset: 0x000715B9
		// (set) Token: 0x06001D76 RID: 7542 RVA: 0x000733C1 File Offset: 0x000715C1
		Action ITextEdition.UpdateTextFromValue { get; set; }

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06001D77 RID: 7543 RVA: 0x000733CA File Offset: 0x000715CA
		// (set) Token: 0x06001D78 RID: 7544 RVA: 0x000733D2 File Offset: 0x000715D2
		Action ITextEdition.MoveFocusToCompositeRoot { get; set; }

		// Token: 0x06001D79 RID: 7545 RVA: 0x000733DC File Offset: 0x000715DC
		void ITextEdition.UpdateText(string value)
		{
			bool flag = this.m_TouchScreenKeyboard != null && this.m_TouchScreenKeyboard.text != value;
			if (flag)
			{
				this.m_TouchScreenKeyboard.text = value;
			}
			bool flag2 = this.text != value;
			if (flag2)
			{
				using (InputEvent pooled = InputEvent.GetPooled(this.text, value))
				{
					pooled.target = base.parent;
					((INotifyValueChanged<string>)this).SetValueWithoutNotify(value);
					VisualElement parent = base.parent;
					if (parent != null)
					{
						parent.SendEvent(pooled);
					}
				}
			}
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x00073480 File Offset: 0x00071680
		string ITextEdition.CullString(string s)
		{
			int maxLength = this.edition.maxLength;
			bool flag = maxLength >= 0 && s != null && s.Length > maxLength;
			string text;
			if (flag)
			{
				text = s.Substring(0, maxLength);
			}
			else
			{
				text = s;
			}
			return text;
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06001D7B RID: 7547 RVA: 0x000734C1 File Offset: 0x000716C1
		// (set) Token: 0x06001D7C RID: 7548 RVA: 0x000734CC File Offset: 0x000716CC
		char ITextEdition.maskChar
		{
			get
			{
				return this.m_MaskChar;
			}
			set
			{
				bool flag = this.m_MaskChar != value;
				if (flag)
				{
					this.m_MaskChar = value;
					bool isPassword = this.edition.isPassword;
					if (isPassword)
					{
						base.IncrementVersion(VersionChangeType.Repaint);
					}
				}
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06001D7D RID: 7549 RVA: 0x00073510 File Offset: 0x00071710
		private char effectiveMaskChar
		{
			get
			{
				return this.edition.isPassword ? this.m_MaskChar : '\0';
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06001D7E RID: 7550 RVA: 0x00073528 File Offset: 0x00071728
		// (set) Token: 0x06001D7F RID: 7551 RVA: 0x00073530 File Offset: 0x00071730
		bool ITextEdition.isPassword
		{
			get
			{
				return this.m_IsPassword;
			}
			set
			{
				bool flag = this.m_IsPassword != value;
				if (flag)
				{
					this.m_IsPassword = value;
					base.IncrementVersion(VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06001D80 RID: 7552 RVA: 0x00073563 File Offset: 0x00071763
		// (set) Token: 0x06001D81 RID: 7553 RVA: 0x0007356B File Offset: 0x0007176B
		bool ITextEdition.autoCorrection
		{
			get
			{
				return this.m_AutoCorrection;
			}
			set
			{
				this.m_AutoCorrection = value;
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06001D82 RID: 7554 RVA: 0x00073574 File Offset: 0x00071774
		// (set) Token: 0x06001D83 RID: 7555 RVA: 0x000735D7 File Offset: 0x000717D7
		internal string renderedText
		{
			get
			{
				bool flag = this.effectiveMaskChar > '\0';
				string text;
				if (flag)
				{
					text = "".PadLeft(this.text.Length, this.effectiveMaskChar) + "\u200b";
				}
				else
				{
					text = (string.IsNullOrEmpty(this.m_RenderedText) ? "\u200b" : this.m_RenderedText);
				}
				return text;
			}
			set
			{
				this.m_RenderedText = value + "\u200b";
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06001D84 RID: 7556 RVA: 0x000735EA File Offset: 0x000717EA
		internal string originalText
		{
			get
			{
				return this.m_OriginalText;
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06001D85 RID: 7557 RVA: 0x0002E015 File Offset: 0x0002C215
		public new ITextElementExperimentalFeatures experimental
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06001D86 RID: 7558 RVA: 0x000735F2 File Offset: 0x000717F2
		void ITextElementExperimentalFeatures.SetRenderedText(string renderedText)
		{
			this.renderedText = renderedText;
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06001D87 RID: 7559 RVA: 0x0002E015 File Offset: 0x0002C215
		public ITextSelection selection
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06001D88 RID: 7560 RVA: 0x000735FD File Offset: 0x000717FD
		// (set) Token: 0x06001D89 RID: 7561 RVA: 0x00073610 File Offset: 0x00071810
		bool ITextSelection.isSelectable
		{
			get
			{
				return this.m_IsSelectable && base.focusable;
			}
			set
			{
				bool flag = value == this.m_IsSelectable;
				if (!flag)
				{
					base.focusable = value;
					this.m_IsSelectable = value;
				}
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06001D8A RID: 7562 RVA: 0x0007363C File Offset: 0x0007183C
		// (set) Token: 0x06001D8B RID: 7563 RVA: 0x0007365C File Offset: 0x0007185C
		int ITextSelection.cursorIndex
		{
			get
			{
				return this.selection.isSelectable ? this.selectingManipulator.cursorIndex : (-1);
			}
			set
			{
				bool isSelectable = this.selection.isSelectable;
				if (isSelectable)
				{
					this.selectingManipulator.cursorIndex = value;
				}
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06001D8C RID: 7564 RVA: 0x00073686 File Offset: 0x00071886
		// (set) Token: 0x06001D8D RID: 7565 RVA: 0x000736A4 File Offset: 0x000718A4
		int ITextSelection.selectIndex
		{
			get
			{
				return this.selection.isSelectable ? this.selectingManipulator.selectIndex : (-1);
			}
			set
			{
				bool isSelectable = this.selection.isSelectable;
				if (isSelectable)
				{
					this.selectingManipulator.selectIndex = value;
				}
			}
		}

		// Token: 0x06001D8E RID: 7566 RVA: 0x000736D0 File Offset: 0x000718D0
		void ITextSelection.SelectAll()
		{
			bool isSelectable = this.selection.isSelectable;
			if (isSelectable)
			{
				this.selectingManipulator.m_SelectingUtilities.SelectAll();
			}
		}

		// Token: 0x06001D8F RID: 7567 RVA: 0x00073700 File Offset: 0x00071900
		void ITextSelection.SelectNone()
		{
			bool isSelectable = this.selection.isSelectable;
			if (isSelectable)
			{
				this.selectingManipulator.m_SelectingUtilities.SelectNone();
			}
		}

		// Token: 0x06001D90 RID: 7568 RVA: 0x00073730 File Offset: 0x00071930
		void ITextSelection.SelectRange(int cursorIndex, int selectionIndex)
		{
			bool isSelectable = this.selection.isSelectable;
			if (isSelectable)
			{
				this.selectingManipulator.m_SelectingUtilities.cursorIndex = cursorIndex;
				this.selectingManipulator.m_SelectingUtilities.selectIndex = selectionIndex;
			}
		}

		// Token: 0x06001D91 RID: 7569 RVA: 0x00073774 File Offset: 0x00071974
		bool ITextSelection.HasSelection()
		{
			return this.selection.isSelectable && this.selectingManipulator.HasSelection();
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06001D92 RID: 7570 RVA: 0x000737A1 File Offset: 0x000719A1
		// (set) Token: 0x06001D93 RID: 7571 RVA: 0x000737A9 File Offset: 0x000719A9
		bool ITextSelection.doubleClickSelectsWord { get; set; } = true;

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06001D94 RID: 7572 RVA: 0x000737B2 File Offset: 0x000719B2
		// (set) Token: 0x06001D95 RID: 7573 RVA: 0x000737BA File Offset: 0x000719BA
		bool ITextSelection.tripleClickSelectsLine { get; set; } = true;

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06001D96 RID: 7574 RVA: 0x000737C3 File Offset: 0x000719C3
		// (set) Token: 0x06001D97 RID: 7575 RVA: 0x000737CB File Offset: 0x000719CB
		bool ITextSelection.selectAllOnFocus { get; set; } = false;

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06001D98 RID: 7576 RVA: 0x000737D4 File Offset: 0x000719D4
		// (set) Token: 0x06001D99 RID: 7577 RVA: 0x000737DC File Offset: 0x000719DC
		bool ITextSelection.selectAllOnMouseUp { get; set; } = false;

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06001D9A RID: 7578 RVA: 0x000737E8 File Offset: 0x000719E8
		Vector2 ITextSelection.cursorPosition
		{
			get
			{
				return this.uitkTextHandle.GetCursorPositionFromStringIndexUsingLineHeight(this.selection.cursorIndex, false, true) + base.contentRect.min;
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06001D9B RID: 7579 RVA: 0x00073820 File Offset: 0x00071A20
		float ITextSelection.lineHeightAtCursorPosition
		{
			get
			{
				return this.uitkTextHandle.GetLineHeightFromCharacterIndex(this.selection.cursorIndex);
			}
		}

		// Token: 0x06001D9C RID: 7580 RVA: 0x00073838 File Offset: 0x00071A38
		void ITextSelection.MoveTextEnd()
		{
			bool isSelectable = this.selection.isSelectable;
			if (isSelectable)
			{
				this.selectingManipulator.m_SelectingUtilities.MoveTextEnd();
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06001D9D RID: 7581 RVA: 0x00073866 File Offset: 0x00071A66
		// (set) Token: 0x06001D9E RID: 7582 RVA: 0x00073870 File Offset: 0x00071A70
		Color ITextSelection.selectionColor
		{
			get
			{
				return this.m_SelectionColor;
			}
			set
			{
				bool flag = this.m_SelectionColor == value;
				if (!flag)
				{
					this.m_SelectionColor = value;
					base.MarkDirtyRepaint();
				}
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06001D9F RID: 7583 RVA: 0x0007389E File Offset: 0x00071A9E
		// (set) Token: 0x06001DA0 RID: 7584 RVA: 0x000738A8 File Offset: 0x00071AA8
		Color ITextSelection.cursorColor
		{
			get
			{
				return this.m_CursorColor;
			}
			set
			{
				bool flag = this.m_CursorColor == value;
				if (!flag)
				{
					this.m_CursorColor = value;
					base.MarkDirtyRepaint();
				}
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06001DA1 RID: 7585 RVA: 0x000738D6 File Offset: 0x00071AD6
		// (set) Token: 0x06001DA2 RID: 7586 RVA: 0x000738E3 File Offset: 0x00071AE3
		private Color cursorColor
		{
			get
			{
				return this.selection.cursorColor;
			}
			set
			{
				this.selection.cursorColor = value;
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06001DA3 RID: 7587 RVA: 0x000738F2 File Offset: 0x00071AF2
		// (set) Token: 0x06001DA4 RID: 7588 RVA: 0x000738FC File Offset: 0x00071AFC
		float ITextSelection.cursorWidth
		{
			get
			{
				return this.m_CursorWidth;
			}
			set
			{
				bool flag = Mathf.Approximately(this.m_CursorWidth, value);
				if (!flag)
				{
					this.m_CursorWidth = value;
					base.MarkDirtyRepaint();
				}
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06001DA5 RID: 7589 RVA: 0x0007392C File Offset: 0x00071B2C
		internal TextSelectingManipulator selectingManipulator
		{
			get
			{
				TextSelectingManipulator textSelectingManipulator;
				if ((textSelectingManipulator = this.m_SelectingManipulator) == null)
				{
					textSelectingManipulator = (this.m_SelectingManipulator = new TextSelectingManipulator(this));
				}
				return textSelectingManipulator;
			}
		}

		// Token: 0x06001DA6 RID: 7590 RVA: 0x00073954 File Offset: 0x00071B54
		private void DrawHighlighting(MeshGenerationContext mgc)
		{
			Color color = ((base.panel.contextType == ContextType.Editor) ? UIElementsUtility.editorPlayModeTintColor : Color.white);
			int num = Math.Min(this.selection.cursorIndex, this.selection.selectIndex);
			int num2 = Math.Max(this.selection.cursorIndex, this.selection.selectIndex);
			Vector2 vector = this.uitkTextHandle.GetCursorPositionFromStringIndexUsingLineHeight(num, false, true);
			Vector2 vector2 = this.uitkTextHandle.GetCursorPositionFromStringIndexUsingLineHeight(num2, false, true);
			int lineNumber = this.uitkTextHandle.GetLineNumber(num);
			int lineNumber2 = this.uitkTextHandle.GetLineNumber(num2);
			float lineHeight = this.uitkTextHandle.GetLineHeight(lineNumber);
			Vector2 min = base.contentRect.min;
			bool flag = this.m_TouchScreenKeyboard != null && this.m_HideMobileInput;
			if (flag)
			{
				TextInfo textInfo = this.uitkTextHandle.textInfo;
				int num3 = ((this.selection.selectIndex < this.selection.cursorIndex) ? textInfo.textElementInfo[this.selection.selectIndex].index : textInfo.textElementInfo[this.selection.cursorIndex].index);
				int num4 = ((this.selection.selectIndex < this.selection.cursorIndex) ? (this.selection.cursorIndex - num3) : (this.selection.selectIndex - num3));
				this.m_TouchScreenKeyboard.selection = new RangeInt(num3, num4);
			}
			bool flag2 = lineNumber == lineNumber2;
			if (flag2)
			{
				vector += min;
				vector2 += min;
				mgc.Rectangle(new MeshGenerationContextUtils.RectangleParams
				{
					rect = new Rect(vector.x, vector.y - lineHeight, vector2.x - vector.x, lineHeight),
					color = this.selection.selectionColor,
					playmodeTintColor = color
				});
			}
			else
			{
				for (int i = lineNumber; i <= lineNumber2; i++)
				{
					bool flag3 = i == lineNumber;
					if (flag3)
					{
						int num5 = this.GetLastCharacterAt(i);
						vector2 = this.uitkTextHandle.GetCursorPositionFromStringIndexUsingLineHeight(num5, true, true);
					}
					else
					{
						bool flag4 = i == lineNumber2;
						if (flag4)
						{
							int num6 = this.uitkTextHandle.textInfo.lineInfo[i].firstCharacterIndex;
							vector = this.uitkTextHandle.GetCursorPositionFromStringIndexUsingLineHeight(num6, false, true);
							vector2 = this.uitkTextHandle.GetCursorPositionFromStringIndexUsingLineHeight(num2, true, true);
						}
						else
						{
							bool flag5 = i != lineNumber && i != lineNumber2;
							if (flag5)
							{
								int num6 = this.uitkTextHandle.textInfo.lineInfo[i].firstCharacterIndex;
								vector = this.uitkTextHandle.GetCursorPositionFromStringIndexUsingLineHeight(num6, false, true);
								int num5 = this.GetLastCharacterAt(i);
								vector2 = this.uitkTextHandle.GetCursorPositionFromStringIndexUsingLineHeight(num5, true, true);
							}
						}
					}
					vector += min;
					vector2 += min;
					mgc.Rectangle(new MeshGenerationContextUtils.RectangleParams
					{
						rect = new Rect(vector.x, vector.y - lineHeight, vector2.x - vector.x, lineHeight),
						color = this.selection.selectionColor,
						playmodeTintColor = color
					});
				}
			}
		}

		// Token: 0x06001DA7 RID: 7591 RVA: 0x00073CC8 File Offset: 0x00071EC8
		internal void DrawCaret(MeshGenerationContext mgc)
		{
			Color color = ((base.panel.contextType == ContextType.Editor) ? UIElementsUtility.editorPlayModeTintColor : Color.white);
			float characterHeightFromIndex = this.uitkTextHandle.GetCharacterHeightFromIndex(this.selection.cursorIndex);
			float num = AlignmentUtils.CeilToPixelGrid(this.selection.cursorWidth, base.scaledPixelsPerPoint, -0.02f);
			mgc.Rectangle(new MeshGenerationContextUtils.RectangleParams
			{
				rect = new Rect(this.selection.cursorPosition.x, this.selection.cursorPosition.y - characterHeightFromIndex, num, characterHeightFromIndex),
				color = this.selection.cursorColor,
				playmodeTintColor = color
			});
		}

		// Token: 0x06001DA8 RID: 7592 RVA: 0x00073D80 File Offset: 0x00071F80
		private int GetLastCharacterAt(int lineIndex)
		{
			int num = this.uitkTextHandle.textInfo.lineInfo[lineIndex].lastCharacterIndex;
			int firstCharacterIndex = this.uitkTextHandle.textInfo.lineInfo[lineIndex].firstCharacterIndex;
			TextElementInfo textElementInfo = this.uitkTextHandle.textInfo.textElementInfo[num];
			while (textElementInfo.character == '\n' || textElementInfo.character == '\r')
			{
				bool flag = num > firstCharacterIndex;
				if (!flag)
				{
					break;
				}
				num--;
				textElementInfo = this.uitkTextHandle.textInfo.textElementInfo[num];
			}
			return num;
		}

		// Token: 0x04000C52 RID: 3154
		public static readonly string ussClassName = "unity-text-element";

		// Token: 0x04000C54 RID: 3156
		private string m_Text = string.Empty;

		// Token: 0x04000C55 RID: 3157
		private bool m_EnableRichText = true;

		// Token: 0x04000C56 RID: 3158
		private bool m_ParseEscapeSequences = true;

		// Token: 0x04000C57 RID: 3159
		private bool m_DisplayTooltipWhenElided = true;

		// Token: 0x04000C59 RID: 3161
		internal static readonly string k_EllipsisText = "...";

		// Token: 0x04000C5A RID: 3162
		internal string elidedText;

		// Token: 0x04000C5B RID: 3163
		private bool m_WasElided;

		// Token: 0x04000C5C RID: 3164
		internal TextEditingManipulator editingManipulator;

		// Token: 0x04000C5D RID: 3165
		private bool m_Multiline;

		// Token: 0x04000C5E RID: 3166
		internal TouchScreenKeyboard m_TouchScreenKeyboard;

		// Token: 0x04000C5F RID: 3167
		internal TouchScreenKeyboardType m_KeyboardType = TouchScreenKeyboardType.Default;

		// Token: 0x04000C60 RID: 3168
		private bool m_HideMobileInput;

		// Token: 0x04000C61 RID: 3169
		private bool m_IsReadOnly = true;

		// Token: 0x04000C62 RID: 3170
		private int m_MaxLength = -1;

		// Token: 0x04000C69 RID: 3177
		private string m_RenderedText;

		// Token: 0x04000C6A RID: 3178
		private string m_OriginalText;

		// Token: 0x04000C6B RID: 3179
		private char m_MaskChar;

		// Token: 0x04000C6C RID: 3180
		private bool m_IsPassword;

		// Token: 0x04000C6D RID: 3181
		private bool m_AutoCorrection;

		// Token: 0x04000C6E RID: 3182
		private TextSelectingManipulator m_SelectingManipulator;

		// Token: 0x04000C6F RID: 3183
		private bool m_IsSelectable;

		// Token: 0x04000C74 RID: 3188
		private Color m_SelectionColor = new Color(0.239f, 0.502f, 0.875f, 0.65f);

		// Token: 0x04000C75 RID: 3189
		private Color m_CursorColor = new Color(0.706f, 0.706f, 0.706f, 1f);

		// Token: 0x04000C76 RID: 3190
		private float m_CursorWidth = 1f;

		// Token: 0x02000370 RID: 880
		public new class UxmlFactory : UxmlFactory<TextElement, TextElement.UxmlTraits>
		{
		}

		// Token: 0x02000371 RID: 881
		public new class UxmlTraits : BindableElement.UxmlTraits
		{
			// Token: 0x170006F5 RID: 1781
			// (get) Token: 0x06001DAB RID: 7595 RVA: 0x00073E50 File Offset: 0x00072050
			public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
			{
				get
				{
					yield break;
				}
			}

			// Token: 0x06001DAC RID: 7596 RVA: 0x00073E70 File Offset: 0x00072070
			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				TextElement textElement = (TextElement)ve;
				textElement.text = this.m_Text.GetValueFromBag(bag, cc);
				textElement.enableRichText = this.m_EnableRichText.GetValueFromBag(bag, cc);
				textElement.parseEscapeSequences = this.m_ParseEscapeSequences.GetValueFromBag(bag, cc);
				textElement.displayTooltipWhenElided = this.m_DisplayTooltipWhenElided.GetValueFromBag(bag, cc);
			}

			// Token: 0x04000C77 RID: 3191
			private UxmlStringAttributeDescription m_Text = new UxmlStringAttributeDescription
			{
				name = "text"
			};

			// Token: 0x04000C78 RID: 3192
			private UxmlBoolAttributeDescription m_EnableRichText = new UxmlBoolAttributeDescription
			{
				name = "enable-rich-text",
				defaultValue = true
			};

			// Token: 0x04000C79 RID: 3193
			private UxmlBoolAttributeDescription m_ParseEscapeSequences = new UxmlBoolAttributeDescription
			{
				name = "parse-escape-sequences",
				defaultValue = false
			};

			// Token: 0x04000C7A RID: 3194
			private UxmlBoolAttributeDescription m_DisplayTooltipWhenElided = new UxmlBoolAttributeDescription
			{
				name = "display-tooltip-when-elided"
			};
		}
	}
}
