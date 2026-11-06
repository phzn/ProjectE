using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Logging;

namespace DA_Assets.FCU
{
	// Token: 0x020002ED RID: 749
	[Serializable]
	public class TagSetter : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x060014C2 RID: 5314 RVA: 0x000867FA File Offset: 0x000849FA
		// (set) Token: 0x060014C3 RID: 5315 RVA: 0x00086802 File Offset: 0x00084A02
		public Dictionary<FcuTag, int> TagsCounter { get; set; } = new Dictionary<FcuTag, int>();

		// Token: 0x060014C4 RID: 5316 RVA: 0x0008680C File Offset: 0x00084A0C
		public Task SetTags(FObject page)
		{
			TagSetter.<SetTags>d__10 <SetTags>d__;
			<SetTags>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SetTags>d__.<>4__this = this;
			<SetTags>d__.page = page;
			<SetTags>d__.<>1__state = -1;
			<SetTags>d__.<>t__builder.Start<TagSetter.<SetTags>d__10>(ref <SetTags>d__);
			return <SetTags>d__.<>t__builder.Task;
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x00086858 File Offset: 0x00084A58
		private async Task SetTagsAsync(FObject root, TagSetter.TagAlgorithm tagAlgorithm, CancellationToken importToken)
		{
			this._outstandingTasks = 0;
			this._currentConcurrency = 0;
			this._processedElements = 0;
			this._tcs = new TaskCompletionSource<bool>();
			CancellationTokenSource logCTS = new CancellationTokenSource();
			this.LogTagging(logCTS.Token, importToken);
			Interlocked.Increment(ref this._outstandingTasks);
			this.TryProcessNode(root, tagAlgorithm, importToken);
			await this._tcs.Task;
			logCTS.Cancel();
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x000868B4 File Offset: 0x00084AB4
		private async Task LogTagging(CancellationToken logToken, CancellationToken importToken)
		{
			DALogger.Log(FcuLocKey.log_tagging.Localize(new object[] { this._processedElements }));
			try
			{
				while (!logToken.IsCancellationRequested && !importToken.IsCancellationRequested)
				{
					await Task.Delay(1000, logToken);
					DALogger.Log(FcuLocKey.log_tagging.Localize(new object[] { this._processedElements }));
				}
			}
			catch (TaskCanceledException)
			{
			}
			finally
			{
				DALogger.Log(FcuLocKey.log_tagging.Localize(new object[] { this._processedElements }));
			}
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x00086908 File Offset: 0x00084B08
		private void TryProcessNode(FObject node, TagSetter.TagAlgorithm tagAlgorithm, CancellationToken importToken)
		{
			if (this.TryIncrementConcurrency())
			{
				ThreadPool.QueueUserWorkItem(delegate(object state)
				{
					try
					{
						this.ProcessNode(node, tagAlgorithm, importToken);
					}
					finally
					{
						Interlocked.Decrement(ref this._currentConcurrency);
						if (Interlocked.Decrement(ref this._outstandingTasks) == 0)
						{
							this._tcs.TrySetResult(true);
						}
					}
				});
				return;
			}
			Task.Run(delegate
			{
				try
				{
					this.ProcessNode(node, tagAlgorithm, importToken);
				}
				finally
				{
					if (Interlocked.Decrement(ref this._outstandingTasks) == 0)
					{
						this._tcs.TrySetResult(true);
					}
				}
			}, importToken);
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x0008696C File Offset: 0x00084B6C
		private void ProcessNode(FObject parent, TagSetter.TagAlgorithm tagAlgorithm, CancellationToken importToken)
		{
			if (importToken.IsCancellationRequested)
			{
				return;
			}
			if (parent.ContainsTag(FcuTag.Frame))
			{
				parent.Data.Hierarchy = new List<FcuHierarchy>
				{
					new FcuHierarchy
					{
						Index = -1,
						Name = parent.Data.Names.ObjectName,
						Guid = parent.Data.Names.UitkGuid
					}
				};
			}
			Interlocked.Increment(ref this._processedElements);
			if (parent.Children == null)
			{
				return;
			}
			if (parent.Children.Count > FcuConfig.ChildParsingLimit && tagAlgorithm != TagSetter.TagAlgorithm.Figma)
			{
				return;
			}
			for (int i = 0; i < parent.Children.Count; i++)
			{
				FObject fobject = parent.Children[i];
				bool flag = false;
				switch (tagAlgorithm)
				{
				case TagSetter.TagAlgorithm.Figma:
					this.SetTagsByFigma(ref fobject, ref parent, i);
					parent.Children[i] = fobject;
					flag = !fobject.Children.IsEmpty<FObject>();
					break;
				case TagSetter.TagAlgorithm.Smart:
					this.SetSmartTags(ref fobject, ref parent);
					flag = fobject.HasVisibleProperty<FObject>((FObject x) => x.Children);
					break;
				case TagSetter.TagAlgorithm.Ignore:
					this.SetIgnoredObjects(ref fobject, ref parent);
					flag = fobject.HasVisibleProperty<FObject>((FObject x) => x.Children);
					break;
				}
				if (flag)
				{
					Interlocked.Increment(ref this._outstandingTasks);
					this.TryProcessNode(fobject, tagAlgorithm, importToken);
				}
			}
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x00086B38 File Offset: 0x00084D38
		private bool TryIncrementConcurrency()
		{
			for (;;)
			{
				int currentConcurrency = this._currentConcurrency;
				if (currentConcurrency >= 10)
				{
					break;
				}
				if (Interlocked.CompareExchange(ref this._currentConcurrency, currentConcurrency + 1, currentConcurrency) == currentConcurrency)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x00086B68 File Offset: 0x00084D68
		private void SetTagsByFigma(ref FObject child, ref FObject parent, int index)
		{
			child.Data = new SyncData
			{
				Id = child.Id,
				ProjectId = this.monoBeh.Settings.MainSettings.ProjectUrl,
				ChildIndexes = new List<int>(),
				Parent = parent,
				Graphic = this.monoBeh.GraphicHelpers.GetGraphic(child)
			};
			if (child.Name.IsScrollContent())
			{
				child.Data.ForceContainer = true;
			}
			else if (child.Name.IsScrollViewport())
			{
				child.Data.ForceContainer = true;
			}
			this.monoBeh.NameSetter.SetNames(child);
			List<FcuTag> list;
			if (this.TryGetManualTags(child, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					child.AddTag(list[i]);
					FcuLogger.Debug(string.Format("GetManualTag {0} | {1} | {2}", i, child.Name, list[i]), FcuDebugSettingsFlags.LogSetTag);
				}
				if (list.Contains(FcuTag.Ignore))
				{
					child.Data.IsEmpty = true;
				}
				else if (list.Contains(FcuTag.Image))
				{
					child.Data.ForceImage = true;
				}
				else if (list.Contains(FcuTag.Container))
				{
					child.Data.ForceContainer = true;
				}
			}
			child.Data.IsEmpty = this.IsEmpty(child);
			if (child.ContainsTag(FcuTag.Background))
			{
				child.AddTag(FcuTag.Image);
			}
			if (parent.ContainsTag(FcuTag.Page))
			{
				child.AddTag(FcuTag.Frame);
			}
			NodeType type = child.Type;
			if ((child.LayoutWrap == LayoutWrap.WRAP || child.LayoutMode == LayoutMode.HORIZONTAL || child.LayoutMode == LayoutMode.VERTICAL) && child.HasVisibleProperty<FObject>((FObject x) => x.Children))
			{
				child.AddTag(FcuTag.AutoLayoutGroup);
			}
			if (child.PreserveRatio.ToBoolNullFalse())
			{
				child.AddTag(FcuTag.AspectRatioFitter);
			}
			if (child.IsAnyMask())
			{
				child.AddTag(FcuTag.Mask);
			}
			if (child.Name.ToLower() == "button")
			{
				child.AddTag(FcuTag.Button);
			}
			if (child.Type == NodeType.TEXT)
			{
				child.AddTag(FcuTag.Text);
				if (!child.Style.IsDefault<Style>() && (child.Style.TextAutoResize == TextAutoResize.WIDTH_AND_HEIGHT || child.Style.TextAutoResize == TextAutoResize.HEIGHT))
				{
					child.AddTag(FcuTag.ContentSizeFitter);
				}
			}
			else if (child.Type == NodeType.VECTOR)
			{
				child.AddTag(FcuTag.Image);
			}
			else if (child.HasVisibleProperty<Paint>((FObject x) => x.Fills) || child.HasVisibleProperty<Paint>((FObject x) => x.Strokes))
			{
				child.AddTag(FcuTag.Image);
			}
			if (!child.Effects.IsEmpty<Effect>())
			{
				Effect[] array = child.Effects.Where<Effect>((Effect x) => x.IsShadowType()).ToArray<Effect>();
				if (this.monoBeh.IsUGUI() && this.monoBeh.UsingTrueShadow() && !this.monoBeh.UsingSpriteRenderer())
				{
					if (array.Length != 0)
					{
						child.AddTag(FcuTag.Shadow);
					}
				}
				else if (this.monoBeh.IsUITK())
				{
					if (array.Length != 0)
					{
					}
				}
				else if (this.monoBeh.IsNova() && array.Length != 0)
				{
					child.AddTag(FcuTag.Shadow);
				}
				if (this.monoBeh.IsNova())
				{
					foreach (Effect effect in child.Effects)
					{
						if (effect.Type == EffectType.BACKGROUND_BLUR)
						{
							child.AddTag(FcuTag.Blur);
						}
					}
				}
			}
			child.Data.IsOverlappedByStroke = this.IsOverlappedByStroke(child);
			if (child.Opacity != null)
			{
				float? opacity = child.Opacity;
				float num = (float)1;
				if (!((opacity.GetValueOrDefault() == num) & (opacity != null)))
				{
					child.AddTag(FcuTag.CanvasGroup);
				}
			}
			child.Data.Hierarchy.AddRange(parent.Data.Hierarchy);
			int newIndex = this.GetNewIndex(parent, index);
			child.Data.Hierarchy.Add(new FcuHierarchy
			{
				Index = newIndex,
				Name = child.Data.Names.ObjectName,
				Guid = child.Data.Names.UitkGuid
			});
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x00087104 File Offset: 0x00085304
		private bool SetSmartTags(ref FObject child, ref FObject parent)
		{
			string text = "SetSmartTags";
			if (this.Is9slice(child))
			{
				child.AddTag(FcuTag.Slice9);
				child.AddTag(FcuTag.Image);
				child.Data.ForceImage = true;
				return true;
			}
			bool flag = (parent.Name.IsInputTextArea() || parent.ContainsTag(FcuTag.InputField) || parent.ContainsTag(FcuTag.PasswordField)) && child.ContainsTag(FcuTag.Text);
			if (child.Data.IsEmpty && !flag)
			{
				child.Data.TagReasons.Add("IsEmpty");
				FcuLogger.Debug(string.Format("{0} | {1} | {2}", text, child.Data.TagReasons, child.Data.NameHierarchy), FcuDebugSettingsFlags.LogSetTag);
				return true;
			}
			if (child.Data.ForceImage)
			{
				child.Data.TagReasons.Add("ForceImage");
				FcuLogger.Debug(string.Format("{0} | {1} | {2}", text, child.Data.TagReasons, child.Data.NameHierarchy), FcuDebugSettingsFlags.LogSetTag);
				return true;
			}
			if (child.IsRootSprite(parent))
			{
				child.AddTag(FcuTag.Image);
				child.Data.ForceImage = true;
				child.Data.TagReasons.Add("IsRootSprite");
				FcuLogger.Debug(string.Format("{0} | {1} | {2}", text, child.Data.TagReasons, child.Data.NameHierarchy), FcuDebugSettingsFlags.LogSetTag);
				return true;
			}
			if (!this.monoBeh.Settings.MainSettings.RawImport)
			{
				bool flag2 = child.ContainsCustomButtonTags();
				if (flag2 && child.ContainsTag(FcuTag.Image) && !child.IsDownloadableType() && !child.IsGenerativeType() && this.monoBeh.Settings.ButtonSettings.TransitionType == ButtonTransitionType.SpriteSwapForAll)
				{
					child.Data.ForceImage = true;
					child.Data.TagReasons.Add("SpriteSwapForAll");
					return true;
				}
				bool flag3 = this.ContainsIcon(child);
				bool flag4 = this.CanBeSingleImage(child);
				FcuLogger.Debug(string.Format("{0} | singleImage: {1} | {2}", text, flag4, child.Data.NameHierarchy), FcuDebugSettingsFlags.LogSetTag);
				if (flag3)
				{
					child.Data.ForceContainer = true;
					child.AddTag(FcuTag.Container);
					child.Data.TagReasons.Add("ContainsIcon");
					FcuLogger.Debug(string.Format("{0} | {1} | {2}", text, child.Data.TagReasons, child.Data.NameHierarchy), FcuDebugSettingsFlags.LogSetTag);
				}
				else
				{
					if (flag4 && flag2)
					{
						child.Data.ForceImage = true;
						child.AddTag(FcuTag.Image);
						child.RemoveNotDownloadableTags();
						child.Data.TagReasons.Add("ContainsCustomButtonTags");
						FcuLogger.Debug(string.Format("{0} | {1} | {2}", text, child.Data.TagReasons, child.Data.NameHierarchy), FcuDebugSettingsFlags.LogSetTag);
						return true;
					}
					if (flag4)
					{
						child.Data.ForceImage = true;
						child.AddTag(FcuTag.Image);
						child.RemoveNotDownloadableTags();
						child.Data.TagReasons.Add("singleImage");
						FcuLogger.Debug(string.Format("{0} | {1} | {2}", text, child.Data.TagReasons, child.Data.NameHierarchy), FcuDebugSettingsFlags.LogSetTag);
						return true;
					}
					if (child.Type == NodeType.BOOLEAN_OPERATION)
					{
						child.Data.ForceImage = true;
						child.AddTag(FcuTag.Image);
						child.Data.TagReasons.Add("BOOLEAN_OPERATION");
						return true;
					}
					FcuLogger.Debug(text + " | else | " + child.Data.NameHierarchy, FcuDebugSettingsFlags.LogSetTag);
				}
			}
			if (child.HasVisibleProperty<FObject>((FObject x) => x.Children))
			{
				child.Data.TagReasons.Add("children not empty");
				FcuLogger.Debug(string.Format("{0} | {1} | {2}", text, child.Data.TagReasons, child.Data.NameHierarchy), FcuDebugSettingsFlags.LogSetTag);
				child.AddTag(FcuTag.Container);
			}
			return false;
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x00087580 File Offset: 0x00085780
		private bool IsOverlappedByStroke(FObject fobject)
		{
			bool flag = false;
			if (fobject.HasVisibleProperty<Paint>((FObject x) => x.Fills) && fobject.HasVisibleProperty<Paint>((FObject x) => x.Strokes) && !fobject.ContainsTag(FcuTag.Shadow))
			{
				if (fobject.IndividualStrokeWeights.IsDefault<IndividualStrokeWeights>())
				{
					float num = fobject.StrokeWeight * 2f;
					if (num >= fobject.Size.y)
					{
						flag = true;
					}
					else if (num >= fobject.Size.x)
					{
						flag = true;
					}
				}
				else
				{
					float num2 = fobject.IndividualStrokeWeights.Top + fobject.IndividualStrokeWeights.Bottom;
					float num3 = fobject.IndividualStrokeWeights.Left + fobject.IndividualStrokeWeights.Right;
					if (num2 >= fobject.Size.y)
					{
						flag = true;
					}
					else if (num3 >= fobject.Size.x)
					{
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x000876D0 File Offset: 0x000858D0
		private int GetNewIndex(FObject parent, int figmaIndex)
		{
			int num = 0;
			for (int i = 0; i < figmaIndex; i++)
			{
				FObject fobject = parent.Children[i];
				if (fobject.Data == null)
				{
					break;
				}
				if (!fobject.Data.IsEmpty)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x00087718 File Offset: 0x00085918
		private bool Is9slice(FObject fobject)
		{
			if (fobject.Children.IsEmpty<FObject>())
			{
				return false;
			}
			if (fobject.Children.Count != 9)
			{
				return false;
			}
			bool figmaAnchor = fobject.Children[0].GetFigmaAnchor() != AnchorType.TopLeft;
			AnchorType figmaAnchor2 = fobject.Children[1].GetFigmaAnchor();
			AnchorType figmaAnchor3 = fobject.Children[2].GetFigmaAnchor();
			AnchorType figmaAnchor4 = fobject.Children[3].GetFigmaAnchor();
			AnchorType figmaAnchor5 = fobject.Children[4].GetFigmaAnchor();
			AnchorType figmaAnchor6 = fobject.Children[5].GetFigmaAnchor();
			AnchorType figmaAnchor7 = fobject.Children[6].GetFigmaAnchor();
			AnchorType figmaAnchor8 = fobject.Children[7].GetFigmaAnchor();
			AnchorType figmaAnchor9 = fobject.Children[8].GetFigmaAnchor();
			return !figmaAnchor && figmaAnchor2 == AnchorType.HorStretchTop && figmaAnchor3 == AnchorType.TopRight && figmaAnchor4 == AnchorType.VertStretchLeft && figmaAnchor5 == AnchorType.StretchAll && figmaAnchor6 == AnchorType.VertStretchRight && figmaAnchor7 == AnchorType.BottomLeft && figmaAnchor8 == AnchorType.HorStretchBottom && figmaAnchor9 == AnchorType.BottomRight;
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x00087824 File Offset: 0x00085A24
		private void SetIgnoredObjects(ref FObject child, ref FObject parent)
		{
			if (child.Data.IsEmpty)
			{
				child.SetFlagToAllChilds(delegate(FObject x)
				{
					x.Data.IsEmpty = true;
				});
				return;
			}
			if (child.Data.ForceImage)
			{
				child.SetFlagToAllChilds(delegate(FObject x)
				{
					x.Data.IsEmpty = true;
				});
				return;
			}
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x000878A4 File Offset: 0x00085AA4
		internal bool TryGetManualTags(FObject fobject, out List<FcuTag> tags)
		{
			tags = new List<FcuTag>();
			if (!fobject.Name.Contains(FcuConfig.RealTagSeparator))
			{
				return false;
			}
			foreach (FcuTag fcuTag in from FcuTag x in Enum.GetValues(typeof(FcuTag))
				where x > FcuTag.None
				select x)
			{
				foreach (string text in this.GetTagParts(fobject.Name))
				{
					if (this.FindManualTag(text, fcuTag))
					{
						tags.Add(fcuTag);
					}
				}
			}
			return tags.Count > 0;
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x00087978 File Offset: 0x00085B78
		private string[] GetTagParts(string name)
		{
			string[] array = name.ToLower().Replace(" ", "").Split(FcuConfig.RealTagSeparator, StringSplitOptions.None);
			if (array.Length != 0)
			{
				return array[0].Split(",", StringSplitOptions.None);
			}
			return new string[0];
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x000879C0 File Offset: 0x00085BC0
		private bool FindManualTag(string tagPart, FcuTag fcuTag)
		{
			string text = fcuTag.GetTagConfig().FigmaTag.ToLower();
			if (text.IsEmpty())
			{
				return false;
			}
			string text2 = Regex.Replace(tagPart, "[^a-z]", "");
			if (text2 == text)
			{
				FcuLogger.Debug("FindManualTag | tagPart: " + text2 + " | tag: " + text, FcuDebugSettingsFlags.LogSetTag);
				return true;
			}
			return false;
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x00087A20 File Offset: 0x00085C20
		private bool ContainsIcon(FObject fobject)
		{
			if (fobject.Children.IsEmpty<FObject>())
			{
				return false;
			}
			foreach (FObject fobject2 in fobject.Children)
			{
				if (fobject2.Name.ToLower().Contains("icon"))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x00087A9C File Offset: 0x00085C9C
		private bool CanBeSingleImage(FObject fobject)
		{
			if (fobject.Children.IsEmpty<FObject>())
			{
				FcuLogger.Debug("Reason: 1 - Object has no children. | " + fobject.Data.NameHierarchy, FcuDebugSettingsFlags.LogDefault);
				return false;
			}
			int num = 0;
			this.CanBeSingleImageRecursive(fobject, fobject, ref num);
			return num == 0;
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x00087AE8 File Offset: 0x00085CE8
		private void CanBeSingleImageRecursive(FObject fobject, FObject parent, ref int count)
		{
			if (!this.CanBeInsideSingleImage(fobject, parent))
			{
				count++;
				return;
			}
			if (fobject.Children.IsEmpty<FObject>())
			{
				return;
			}
			foreach (FObject fobject2 in fobject.Children)
			{
				this.CanBeSingleImageRecursive(fobject2, parent, ref count);
			}
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x00087B60 File Offset: 0x00085D60
		private bool CanBeInsideSingleImage(FObject fobject, FObject parent)
		{
			if (fobject.Data.ForceContainer)
			{
				int num = 2;
				FcuLogger.Debug(string.Format("Reason: {0} - ForceContainer is true. | Parent: {1}, Current: {2}", num, parent.Data.NameHierarchy, fobject.Data.NameHierarchy), FcuDebugSettingsFlags.LogDefault);
				return false;
			}
			if (fobject.Data.ForceImage)
			{
				int num = 3;
				FcuLogger.Debug(string.Format("Reason: {0} - ForceImage is true. | Parent: {1}, Current: {2}", num, parent.Data.NameHierarchy, fobject.Data.NameHierarchy), FcuDebugSettingsFlags.LogDefault);
				return false;
			}
			foreach (FcuTag fcuTag in fobject.Data.Tags)
			{
				if (!fcuTag.GetTagConfig().CanBeInsideSingleImage)
				{
					int num = 4;
					FcuLogger.Debug(string.Format("Reason: {0} - Tag {1} cannot be inside a single image. | Parent: {2}, Current: {3}", new object[]
					{
						num,
						fcuTag,
						parent.Data.NameHierarchy,
						fobject.Data.NameHierarchy
					}), FcuDebugSettingsFlags.LogDefault);
					return false;
				}
			}
			return true;
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x00087C98 File Offset: 0x00085E98
		private bool IsEmpty(FObject fobject)
		{
			int num = 0;
			this.IsEmptyRecursive(fobject, ref num);
			return num == 0;
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x00087CB4 File Offset: 0x00085EB4
		private void IsEmptyRecursive(FObject fobject, ref int count)
		{
			if (count > 0)
			{
				return;
			}
			float? opacity = fobject.Opacity;
			float num = 0f;
			if ((opacity.GetValueOrDefault() == num) & (opacity != null))
			{
				return;
			}
			if (!fobject.IsVisible())
			{
				return;
			}
			if (fobject.ContainsTag(FcuTag.Ignore))
			{
				return;
			}
			if (fobject.IsZeroSize() && fobject.Type != NodeType.LINE)
			{
				return;
			}
			bool flag;
			if (!fobject.Fills.IsEmpty<Paint>())
			{
				flag = fobject.Fills.Any<Paint>((Paint x) => x.IsVisible());
			}
			else
			{
				flag = false;
			}
			bool flag2;
			if (!fobject.Strokes.IsEmpty<Paint>())
			{
				flag2 = fobject.Strokes.Any<Paint>((Paint x) => x.IsVisible());
			}
			else
			{
				flag2 = false;
			}
			bool flag3 = flag2;
			bool flag4;
			if (!fobject.Effects.IsEmpty<Effect>())
			{
				flag4 = fobject.Effects.Any<Effect>((Effect x) => x.IsVisible());
			}
			else
			{
				flag4 = false;
			}
			bool flag5 = flag4;
			if (flag || flag3 || flag5 || fobject.IsObjectMask())
			{
				count++;
				return;
			}
			if (!fobject.HasVisibleProperty<FObject>((FObject x) => x.Children))
			{
				return;
			}
			foreach (FObject fobject2 in fobject.Children)
			{
				this.IsEmptyRecursive(fobject2, ref count);
			}
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x00087E70 File Offset: 0x00086070
		public void CountTags(List<FObject> fobjects)
		{
			ConcurrentDictionary<FcuTag, ConcurrentBag<bool>> tagsCounter = new ConcurrentDictionary<FcuTag, ConcurrentBag<bool>>();
			foreach (object obj in Enum.GetValues(typeof(FcuTag)))
			{
				FcuTag fcuTag = (FcuTag)obj;
				tagsCounter.TryAdd(fcuTag, new ConcurrentBag<bool>());
			}
			Parallel.ForEach<FObject>(fobjects, delegate(FObject fobject)
			{
				if (fobject.Data.GameObject == null)
				{
					return;
				}
				foreach (FcuTag fcuTag2 in fobject.Data.Tags)
				{
					tagsCounter[fcuTag2].Add(true);
				}
			});
			Dictionary<FcuTag, int> dictionary = tagsCounter.ToDictionary<KeyValuePair<FcuTag, ConcurrentBag<bool>>, FcuTag, int>((KeyValuePair<FcuTag, ConcurrentBag<bool>> kvp) => kvp.Key, (KeyValuePair<FcuTag, ConcurrentBag<bool>> kvp) => kvp.Value.Count);
			this.TagsCounter = dictionary;
		}

		// Token: 0x0400165F RID: 5727
		private const int MAX_THREAD_COUNT = 10;

		// Token: 0x04001660 RID: 5728
		private const int LOG_DELAY_MS = 1000;

		// Token: 0x04001661 RID: 5729
		private int _outstandingTasks;

		// Token: 0x04001662 RID: 5730
		private int _currentConcurrency;

		// Token: 0x04001663 RID: 5731
		private int _processedElements;

		// Token: 0x04001664 RID: 5732
		private TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();

		// Token: 0x020002EE RID: 750
		private enum TagAlgorithm
		{
			// Token: 0x04001667 RID: 5735
			Figma,
			// Token: 0x04001668 RID: 5736
			Smart,
			// Token: 0x04001669 RID: 5737
			Ignore
		}
	}
}
