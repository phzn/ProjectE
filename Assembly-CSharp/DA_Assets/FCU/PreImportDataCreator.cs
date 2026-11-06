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
using UnityEngine.UI;

namespace DA_Assets.FCU
{
	// Token: 0x020002DB RID: 731
	[Serializable]
	public class PreImportDataCreator : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x06001457 RID: 5207 RVA: 0x000837D4 File Offset: 0x000819D4
		internal async Task<PreImportInput> GetPreImportData(List<FObject> fobjects, List<SyncHelper> syncHelpers)
		{
			SelectableObject<DiffInfo> selectableObject = await this.GetToImport(fobjects, syncHelpers);
			SelectableObject<DiffInfo> toImport = selectableObject;
			toImport.SetAllSelected(true);
			SelectableObject<SyncData> selectableObject2 = await this.GetToRemove(fobjects, syncHelpers);
			selectableObject2.SetAllSelected(false);
			DALogger.Log(string.Format("GetPreImportData | toImport: {0} | toRemove: {1}", toImport.Childs.Count, selectableObject2.Childs.Count));
			return new PreImportInput
			{
				ToImport = toImport,
				ToRemove = selectableObject2
			};
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x00083828 File Offset: 0x00081A28
		private async Task<SelectableObject<DiffInfo>> GetToImport(List<FObject> fobjects, List<SyncHelper> syncHelpers)
		{
			SelectableObject<DiffInfo> toImport = new SelectableObject<DiffInfo>
			{
				Object = new DiffInfo
				{
					Id = "1000023990958793079",
					Name = "1000023990958793079"
				}
			};
			Dictionary<string, DiffInfo> allObjects = new Dictionary<string, DiffInfo>();
			for (int i = 0; i < syncHelpers.Count; i++)
			{
				if (this.monoBeh.IsCancellationRequested(TokenType.Import))
				{
					return toImport;
				}
				if (i % 100 == 0)
				{
					await Task.Yield();
				}
				SyncHelper syncHelper = syncHelpers[i];
				if (syncHelper.Data.RootFrame == null)
				{
					DALogger.LogError("RootFrame is null for '" + syncHelper.gameObject.name + "', skip.");
				}
				else
				{
					allObjects[syncHelper.Data.Id] = new DiffInfo
					{
						Id = syncHelper.Data.Id,
						IsFrame = syncHelper.ContainsTag(FcuTag.Frame),
						RootFrame = syncHelper.Data.RootFrame,
						Name = syncHelper.gameObject.name,
						OldData = syncHelper.Data
					};
				}
			}
			for (int i = 0; i < fobjects.Count; i++)
			{
				if (this.monoBeh.IsCancellationRequested(TokenType.Import))
				{
					return toImport;
				}
				if (i % 100 == 0)
				{
					await Task.Yield();
				}
				FObject fobject = fobjects[i];
				SyncData data = fobject.Data;
				if (((data != null) ? data.RootFrame : null) == null)
				{
					DALogger.LogError("RootFrame is null for '" + fobject.Name + "', skip.");
				}
				else
				{
					DiffInfo diffInfo;
					if (allObjects.TryGetValue(fobject.Id, out diffInfo))
					{
						diffInfo.NewData = fobject;
					}
					else
					{
						diffInfo = new DiffInfo();
						diffInfo.Name = fobject.Name;
						diffInfo.IsFrame = fobject.ContainsTag(FcuTag.Frame);
						diffInfo.Id = fobject.Id;
						diffInfo.RootFrame = fobject.Data.RootFrame;
						diffInfo.IsNew = true;
						diffInfo.NewData = fobject;
					}
					allObjects[fobject.Id] = diffInfo;
				}
			}
			allObjects = allObjects.Where<KeyValuePair<string, DiffInfo>>((KeyValuePair<string, DiffInfo> x) => !x.Value.NewData.IsDefault<FObject>()).ToDictionary<KeyValuePair<string, DiffInfo>, string, DiffInfo>((KeyValuePair<string, DiffInfo> x) => x.Key, (KeyValuePair<string, DiffInfo> x) => x.Value);
			Dictionary<string, DiffInfo> dictionary = new Dictionary<string, DiffInfo>();
			foreach (KeyValuePair<string, DiffInfo> keyValuePair in allObjects)
			{
				if (this.monoBeh.IsCancellationRequested(TokenType.Import))
				{
					return toImport;
				}
				DiffInfo value = keyValuePair.Value;
				if (keyValuePair.Value.OldData != null && !keyValuePair.Value.NewData.IsDefault<FObject>())
				{
					if (value.OldData.HashData == value.NewData.Data.HashData)
					{
						value.HasFigmaDiff = false;
					}
					else
					{
						value.HasFigmaDiff = true;
					}
					RectTransform rectTransform;
					if (value.OldData.GameObject.TryGetComponentSafe<RectTransform>(out rectTransform))
					{
						PreImportDataCreator.<>c__DisplayClass3_0 CS$<>8__locals1;
						CS$<>8__locals1.oldSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
						CS$<>8__locals1.fobjectSize = value.NewData.Size;
						CS$<>8__locals1.hasBoundingSize = value.NewData.GetBoundingSize(out CS$<>8__locals1.bSize);
						CS$<>8__locals1.hasRenderSize = value.NewData.GetRenderSize(out CS$<>8__locals1.rSize);
						if (PreImportDataCreator.<GetToImport>g__IsEqual|3_4(CS$<>8__locals1.oldSize, CS$<>8__locals1.fobjectSize) || (CS$<>8__locals1.hasBoundingSize && PreImportDataCreator.<GetToImport>g__IsEqual|3_4(CS$<>8__locals1.oldSize, CS$<>8__locals1.bSize)) || (CS$<>8__locals1.hasRenderSize && PreImportDataCreator.<GetToImport>g__IsEqual|3_4(CS$<>8__locals1.oldSize, CS$<>8__locals1.rSize)))
						{
							value.Size = new TProp<Vector2, Vector2>(false, default(Vector2), default(Vector2));
						}
						else
						{
							value.Size = new TProp<Vector2, Vector2>(true, CS$<>8__locals1.oldSize, PreImportDataCreator.<GetToImport>g__GetDifferentSize|3_5(ref CS$<>8__locals1));
						}
					}
					Graphic graphic;
					if (value.OldData.GameObject.TryGetComponentSafe<Graphic>(out graphic))
					{
						if (!value.NewData.Fills.IsEmpty<Paint>() && graphic.color != value.NewData.Fills[0].Color)
						{
							value.Color = new TProp<Color, Color>(true, graphic.color, value.NewData.Fills[0].Color);
						}
						else
						{
							value.Color = new TProp<Color, Color>(false, default(Color), default(Color));
						}
					}
				}
				dictionary.Add(keyValuePair.Key, value);
			}
			allObjects = dictionary;
			Dictionary<string, SelectableObject<DiffInfo>> dictionary2 = new Dictionary<string, SelectableObject<DiffInfo>>();
			foreach (DiffInfo diffInfo2 in allObjects.Values)
			{
				if (this.monoBeh.IsCancellationRequested(TokenType.Import))
				{
					return toImport;
				}
				if (diffInfo2.IsFrame)
				{
					dictionary2.Add(diffInfo2.RootFrame.Id, new SelectableObject<DiffInfo>
					{
						Object = diffInfo2,
						Childs = new List<SelectableObject<DiffInfo>>()
					});
				}
			}
			foreach (DiffInfo diffInfo3 in allObjects.Values)
			{
				if (this.monoBeh.IsCancellationRequested(TokenType.Import))
				{
					return toImport;
				}
				if (!diffInfo3.IsFrame)
				{
					dictionary2[diffInfo3.RootFrame.Id].Childs.Add(new SelectableObject<DiffInfo>
					{
						Object = diffInfo3,
						Childs = new List<SelectableObject<DiffInfo>>()
					});
				}
			}
			toImport.Childs = dictionary2.Values.ToList<SelectableObject<DiffInfo>>();
			return toImport;
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x0008387C File Offset: 0x00081A7C
		private async Task<SelectableObject<SyncData>> GetToRemove(List<FObject> fobjects, List<SyncHelper> syncHelpers)
		{
			SelectableObject<SyncData> toRemove = new SelectableObject<SyncData>
			{
				Object = new SyncData
				{
					Id = "1000082127942688996"
				}
			};
			fobjects = fobjects.Where<FObject>(delegate(FObject x)
			{
				SyncData data = x.Data;
				return ((data != null) ? data.RootFrame : null) != null;
			}).ToList<FObject>();
			syncHelpers = syncHelpers.Where<SyncHelper>(delegate(SyncHelper x)
			{
				SyncData data2 = x.Data;
				return ((data2 != null) ? data2.RootFrame : null) != null;
			}).ToList<SyncHelper>();
			SelectableObject<SyncData>[] syncHelpersByFrame = (from x in syncHelpers
				group x by x.Data.RootFrame).Select<IGrouping<SyncData, SyncHelper>, SelectableObject<SyncData>>(delegate(IGrouping<SyncData, SyncHelper> g)
			{
				SelectableObject<SyncData> selectableObject2 = new SelectableObject<SyncData>();
				selectableObject2.Childs = g.Select<SyncHelper, SelectableObject<SyncData>>((SyncHelper x) => new SelectableObject<SyncData>
				{
					Object = x.Data
				}).ToList<SelectableObject<SyncData>>();
				selectableObject2.Object = g.First<SyncHelper>((SyncHelper x) => x.Data.RootFrame == x.Data).Data;
				return selectableObject2;
			}).ToArray<SelectableObject<SyncData>>();
			FrameGroup[] fobjectsByFrame = (from x in fobjects
				group x by x.Data.RootFrame).Select<IGrouping<SyncData, FObject>, FrameGroup>(delegate(IGrouping<SyncData, FObject> g)
			{
				FrameGroup frameGroup2 = default(FrameGroup);
				frameGroup2.Childs = g.Select<FObject, FObject>((FObject x) => x).ToList<FObject>();
				frameGroup2.RootFrame = g.First<FObject>();
				return frameGroup2;
			}).ToArray<FrameGroup>();
			int i = 0;
			while (i < syncHelpersByFrame.Length)
			{
				if (this.monoBeh.IsCancellationRequested(TokenType.Import))
				{
					return toRemove;
				}
				if (i % 100 == 0)
				{
					await Task.Yield();
				}
				SelectableObject<SyncData> syncGroup = syncHelpersByFrame[i];
				SelectableObject<SyncData> selectableObj = new SelectableObject<SyncData>();
				selectableObj.Object = syncGroup.Object;
				selectableObj.Childs = new List<SelectableObject<SyncData>>();
				int j = 0;
				int k;
				while (j < syncGroup.Childs.Count)
				{
					if (j % 100 == 0)
					{
						await Task.Yield();
					}
					SelectableObject<SyncData> selectableObject = syncGroup.Childs[j];
					if (!selectableObject.Object.Tags.Contains(FcuTag.Frame))
					{
						bool flag = true;
						foreach (FrameGroup frameGroup in fobjectsByFrame)
						{
							if (!(frameGroup.RootFrame.Id != syncGroup.Object.Id))
							{
								foreach (FObject fobject in frameGroup.Childs)
								{
									if (fobject.Id == selectableObject.Object.Id)
									{
										flag = false;
										break;
									}
								}
							}
						}
						if (flag)
						{
							selectableObj.Childs.Add(selectableObject);
						}
					}
					k = j++;
				}
				if (!selectableObj.Childs.IsEmpty<SelectableObject<SyncData>>())
				{
					toRemove.Childs.Add(selectableObj);
				}
				syncGroup = null;
				selectableObj = null;
				k = i++;
			}
			return toRemove;
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x000838CF File Offset: 0x00081ACF
		[CompilerGenerated]
		internal static Vector2 <GetToImport>g__Round|3_3(Vector2 vec)
		{
			return vec.Round(FcuConfig.Rounding.DiffCheckerSizeDigits);
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x000838E4 File Offset: 0x00081AE4
		[CompilerGenerated]
		internal static bool <GetToImport>g__IsEqual|3_4(Vector2 a, Vector2 b)
		{
			return PreImportDataCreator.<GetToImport>g__Round|3_3(a).Equals(PreImportDataCreator.<GetToImport>g__Round|3_3(b));
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x00083908 File Offset: 0x00081B08
		[CompilerGenerated]
		internal static Vector2 <GetToImport>g__GetDifferentSize|3_5(ref PreImportDataCreator.<>c__DisplayClass3_0 A_0)
		{
			if (!PreImportDataCreator.<GetToImport>g__IsEqual|3_4(A_0.oldSize, A_0.fobjectSize))
			{
				return A_0.fobjectSize;
			}
			if (A_0.hasBoundingSize && !PreImportDataCreator.<GetToImport>g__IsEqual|3_4(A_0.oldSize, A_0.bSize))
			{
				return A_0.bSize;
			}
			if (A_0.hasRenderSize && !PreImportDataCreator.<GetToImport>g__IsEqual|3_4(A_0.oldSize, A_0.rSize))
			{
				return A_0.rSize;
			}
			return A_0.fobjectSize;
		}

		// Token: 0x040015DB RID: 5595
		public const string TO_IMPORT_MENU_ID = "1000023990958793079";

		// Token: 0x040015DC RID: 5596
		public const string TO_REMOVE_MENU_ID = "1000082127942688996";
	}
}
