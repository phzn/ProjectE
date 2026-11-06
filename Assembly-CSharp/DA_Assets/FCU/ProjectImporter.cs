using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Logging;
using DA_Assets.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace DA_Assets.FCU
{
	// Token: 0x02000330 RID: 816
	[Serializable]
	public class ProjectImporter : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x060015FF RID: 5631 RVA: 0x0008EB1C File Offset: 0x0008CD1C
		private bool ValidateImportSettings(out string reason)
		{
			bool? flag = null;
			reason = "null";
			if (this.monoBeh.IsUITK())
			{
				if (this.monoBeh.UsingSVG())
				{
					reason = FcuLocKey.log_import_failed_incompatible.Localize(new object[]
					{
						string.Format("{0}.{1}", "UIFramework", UIFramework.UITK),
						string.Format("{0}.{1}", "ImageFormat", ImageFormat.SVG)
					});
					flag = new bool?(false);
				}
				else if (this.monoBeh.Settings.LocalizationSettings.LocalizationComponent == LocalizationComponent.I2Localization)
				{
					reason = FcuLocKey.log_import_failed_incompatible.Localize(new object[]
					{
						string.Format("{0}.{1}", "UIFramework", UIFramework.UITK),
						string.Format("{0}.{1}", "LocalizationComponent", this.monoBeh.Settings.LocalizationSettings.LocalizationComponent)
					});
					flag = new bool?(false);
				}
			}
			else if (this.monoBeh.UsingUIBlock2D())
			{
				if (!this.monoBeh.IsNova())
				{
					reason = FcuLocKey.log_import_failed_enable_required.Localize(new object[]
					{
						string.Format("{0}.{1}", "UIFramework", UIFramework.NOVA),
						string.Format("{0}.{1}", "ImageComponent", ImageComponent.UIBlock2D)
					});
					flag = new bool?(false);
				}
				else if (this.monoBeh.UsingSVG())
				{
					reason = FcuLocKey.log_import_failed_incompatible.Localize(new object[]
					{
						string.Format("{0}.{1}", "ImageComponent", ImageComponent.UIBlock2D),
						string.Format("{0}.{1}", "ImageFormat", ImageFormat.SVG)
					});
					flag = new bool?(false);
				}
			}
			else if (this.monoBeh.UsingSvgImage())
			{
				if (!this.monoBeh.IsUGUI())
				{
					reason = FcuLocKey.log_import_failed_enable_required.Localize(new object[]
					{
						string.Format("{0}.{1}", "UIFramework", UIFramework.UGUI),
						string.Format("{0}.{1}", "ImageComponent", ImageComponent.SvgImage)
					});
					flag = new bool?(false);
				}
				else if (!this.monoBeh.UsingSVG())
				{
					reason = FcuLocKey.log_import_failed_enable_required.Localize(new object[]
					{
						string.Format("{0}.{1}", "ImageFormat", ImageFormat.SVG),
						string.Format("{0}.{1}", "ImageComponent", ImageComponent.SvgImage)
					});
					flag = new bool?(false);
				}
			}
			else if (!this.monoBeh.UsingSvgImage() && this.monoBeh.UsingSVG())
			{
				reason = FcuLocKey.log_import_failed_unsupported.Localize(new object[]
				{
					string.Format("{0}.{1}", "ImageComponent", this.monoBeh.Settings.ImageSpritesSettings.ImageComponent),
					string.Format("{0}.{1}", "ImageFormat", ImageFormat.SVG)
				});
				flag = new bool?(false);
			}
			return flag.ToBoolNullTrue();
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x0008EE20 File Offset: 0x0008D020
		public void StartImport()
		{
			this.monoBeh.AssetTools.StopImport(StopImportReason.Hidden);
			if (!this.monoBeh.IsJsonNetExists())
			{
				DALogger.LogError(FcuLocKey.log_cant_find_package.Localize(new object[] { "com.unity.nuget.newtonsoft-json" }));
				return;
			}
			if (this.monoBeh.InspectorDrawer.SelectableDocument.IsProjectEmpty())
			{
				DALogger.LogError(FcuLocKey.log_project_empty.Localize(Array.Empty<object>()));
				return;
			}
			string text;
			if (!this.ValidateImportSettings(out text))
			{
				Debug.LogError(text);
				return;
			}
			this.monoBeh.CancellationTokenController.CreateNew(TokenType.Import);
			this.StartImportAsync();
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x0008EEB8 File Offset: 0x0008D0B8
		private async Task StartImportAsync()
		{
			try
			{
				List<string> selectedFrameIds = this.GetSelectedFrameIds();
				if (selectedFrameIds.Count < 1)
				{
					DALogger.Log(FcuLocKey.log_nothing_to_import.Localize(Array.Empty<object>()));
				}
				else
				{
					UnityEvent<FigmaConverterUnity> onImportStart = this.monoBeh.Events.OnImportStart;
					if (onImportStart != null)
					{
						onImportStart.Invoke(this.monoBeh);
					}
					SceneBackuper.TryBackupActiveScene();
					FObject virtualPage = default(FObject);
					this.monoBeh.FolderCreator.CreateAll();
					this.monoBeh.CurrentProject.LastImportedFrames.Clear();
					this._currPage = this.monoBeh.CurrentProject.CurrentPage;
					this._currPage.Clear();
					List<FObject> list = await this.monoBeh.ProjectDownloader.DownloadAllNodes(selectedFrameIds);
					virtualPage = new FObject
					{
						Id = FcuConfig.PARENT_ID,
						Name = this.monoBeh.CurrentProject.ProjectName,
						Children = list,
						Data = new SyncData
						{
							GameObject = this.monoBeh.gameObject,
							RectGameObject = this.monoBeh.gameObject,
							Names = new FNames
							{
								ObjectName = FcuTag.Page.ToString()
							},
							Tags = new List<FcuTag> { FcuTag.Page }
						}
					};
					this.monoBeh.NameSetter.ClearNames();
					await this.monoBeh.TagSetter.SetTags(virtualPage);
					await this.ConvertTreeToListAsync(virtualPage, this._currPage);
					await this.monoBeh.ImageTypeSetter.SetImageTypes(this._currPage);
					await this.monoBeh.ImageTypeSetter.SetInsideDownloadableFlag(virtualPage);
					await this.monoBeh.HashGenerator.SetHashes(this._currPage);
					await this.monoBeh.SpriteDownloader.SetScalesAndMaxSpriteSizes(this._currPage);
					await this.monoBeh.NameSetter.SetNames(this._currPage, FcuNameType.File);
					await this.monoBeh.NameSetter.SetNames(this._currPage, FcuNameType.UssClass);
					this.monoBeh.CurrentProject.SetRootFrames(this._currPage);
					if (this.monoBeh.IsUGUI() || this.monoBeh.IsNova())
					{
						this.monoBeh.CurrentProject.LoadLocalPrefabs();
						await this.TryShowPreImportWindow();
						this.monoBeh.CanvasDrawer.GameObjectDrawer.Draw(virtualPage);
					}
					await this.monoBeh.HashCacher.WriteCache(this._currPage);
					this.monoBeh.CurrentProject.SetRootFrames(this._currPage);
					this.monoBeh.TagSetter.CountTags(this._currPage);
					await this.monoBeh.DuplicateFinder.SetDuplicateFlags(this._currPage);
					await this.monoBeh.SpritePathSetter.SetSpritePaths(this._currPage);
					await this.monoBeh.SpriteDownloader.CreateImageChunks(this._currPage);
					if (!this.monoBeh.IsCancellationRequested(TokenType.Import))
					{
						await this.monoBeh.SpriteDownloader.DownloadSprites(this._currPage);
						await this.monoBeh.SpriteGenerator.GenerateSprites(this._currPage);
						await this.monoBeh.SpriteColorizer.ColorizeSprites(this._currPage);
						if (!this.monoBeh.IsCancellationRequested(TokenType.Import))
						{
							await this.monoBeh.FontDownloader.DownloadFonts(this._currPage);
							if (!this.monoBeh.IsCancellationRequested(TokenType.Import))
							{
								await this.monoBeh.SpriteSlicer.SliceSprites(this._currPage);
								if (this.monoBeh.IsUGUI())
								{
									this.monoBeh.CanvasDrawer.AddCanvasComponent();
									await this.monoBeh.TransformSetter.SetTransformPos(this._currPage);
									await this.monoBeh.TransformSetter.MoveUguiTransforms(this._currPage);
									await this.monoBeh.TransformSetter.RestoreParents(this._currPage);
									await this.monoBeh.TransformSetter.SetStretchAllIfNeeded(this._currPage);
									await this.monoBeh.TransformSetter.SetSiblingIndex(this._currPage);
									if (this.monoBeh.IsCancellationRequested(TokenType.Import))
									{
										return;
									}
									await this.monoBeh.CanvasDrawer.DrawToCanvas(this._currPage);
									if (this.monoBeh.UsingSpriteRenderer())
									{
										await this.monoBeh.CanvasDrawer.FixSpriteRenderers(this._currPage);
									}
									else if (this.monoBeh.UsingJoshPui())
									{
										await this.monoBeh.CanvasDrawer.FixJoshPui();
									}
									else if (this.monoBeh.UsingDttPui())
									{
										await this.monoBeh.CanvasDrawer.FixDttImages(this._currPage);
									}
								}
								else if (this.monoBeh.IsUITK())
								{
									await this.monoBeh.NameSetter.Set_UITK_Names(this._currPage);
								}
								else
								{
									this.monoBeh.IsNova();
								}
								this.ClearAfterImport();
								SceneBackuper.MakeActiveSceneDirty();
								if (!this.monoBeh.IsCancellationRequested(TokenType.Import))
								{
									UnityEvent<FigmaConverterUnity> onImportComplete = this.monoBeh.Events.OnImportComplete;
									if (onImportComplete != null)
									{
										onImportComplete.Invoke(this.monoBeh);
									}
									this.monoBeh.AssetTools.ShowRateMe();
									this.monoBeh.AssetTools.StopImport(StopImportReason.End);
									virtualPage = default(FObject);
								}
							}
						}
					}
				}
			}
			catch (TaskCanceledException ex)
			{
				Debug.LogException(ex);
				this.monoBeh.AssetTools.StopImport(StopImportReason.TaskCanceled);
			}
			catch (Exception ex2)
			{
				Debug.LogException(ex2);
				this.monoBeh.AssetTools.StopImport(StopImportReason.Error);
			}
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x0008EEFC File Offset: 0x0008D0FC
		private Task TryShowPreImportWindow()
		{
			ProjectImporter.<TryShowPreImportWindow>d__4 <TryShowPreImportWindow>d__;
			<TryShowPreImportWindow>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<TryShowPreImportWindow>d__.<>4__this = this;
			<TryShowPreImportWindow>d__.<>1__state = -1;
			<TryShowPreImportWindow>d__.<>t__builder.Start<ProjectImporter.<TryShowPreImportWindow>d__4>(ref <TryShowPreImportWindow>d__);
			return <TryShowPreImportWindow>d__.<>t__builder.Task;
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x0008EF40 File Offset: 0x0008D140
		private List<string> GetSelectedFrameIds()
		{
			return (from si in this.monoBeh.InspectorDrawer.SelectableDocument.Childs.SelectMany<SelectableFObject, SelectableFObject>((SelectableFObject si) => si.Childs)
				where si.Selected
				select si.Id).ToList<string>();
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x0008EFD4 File Offset: 0x0008D1D4
		public async Task ConvertTreeToListAsync(FObject parent, List<FObject> fobjects)
		{
			DALogger.Log("ConvertTreeToListAsync");
			await Task.Run(delegate
			{
				this.ConvertTreeToList(parent, fobjects, 0);
			}, this.monoBeh.GetToken(TokenType.Import));
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x0008F028 File Offset: 0x0008D228
		private void ConvertTreeToList(FObject parent, List<FObject> fobjects, int depth = 0)
		{
			foreach (FObject fobject in parent.Children)
			{
				if (fobject.Data.IsEmpty || fobject.ContainsTag(FcuTag.Ignore))
				{
					fobject.SetFlagToAllChilds(delegate(FObject x)
					{
						x.Data.IsEmpty = true;
					});
				}
				else
				{
					fobject.Data.HierarchyLevel = depth + 1;
					int num = fobjects.IndexOf(parent);
					fobject.Data.ParentIndex = num;
					fobjects.Add(fobject);
					if (!fobject.Data.Parent.ContainsTag(FcuTag.Page))
					{
						fobjects[fobject.Data.ParentIndex].Data.ChildIndexes.Add(fobjects.Count - 1);
					}
					if (fobject.Data.ForceImage)
					{
						fobject.SetFlagToAllChilds(delegate(FObject x)
						{
							x.Data.IsEmpty = true;
						});
					}
					else if (!fobject.Children.IsEmpty<FObject>())
					{
						this.ConvertTreeToList(fobject, fobjects, depth + 1);
					}
				}
			}
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x0008F188 File Offset: 0x0008D388
		private void ClearAfterImport()
		{
			this.monoBeh.CanvasDrawer.GameObjectDrawer.ClearTempRectFrames();
			if (!this.monoBeh.IsDebug())
			{
				Parallel.ForEach<SyncHelper>(this.monoBeh.SyncHelpers.GetAllSyncHelpers(), delegate(SyncHelper syncHelper)
				{
					ObjectCleaner.ClearByAttribute<ClearAttribute>(syncHelper.Data);
				});
				this.monoBeh.ImageTypeSetter.ClearAllIds();
				this.monoBeh.CanvasDrawer.ButtonDrawer.Buttons.Clear();
			}
		}

		// Token: 0x0400178C RID: 6028
		private List<FObject> _currPage;
	}
}
