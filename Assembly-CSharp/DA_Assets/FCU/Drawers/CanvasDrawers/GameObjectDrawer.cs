using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Logging;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003AD RID: 941
	[Serializable]
	public class GameObjectDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x06001859 RID: 6233 RVA: 0x0009AB4C File Offset: 0x00098D4C
		public void Draw(FObject parent)
		{
			this._tempFrames = MonoBehExtensions.CreateEmptyGameObject(null, null);
			this._tempFrames.transform.SetParent(this.monoBeh.transform);
			DALogger.Log(FcuLocKey.log_instantiate_game_objects.Localize(Array.Empty<object>()));
			this.DrawFObject(parent);
		}

		// Token: 0x0600185A RID: 6234 RVA: 0x0009AB9C File Offset: 0x00098D9C
		public void DrawFObject(FObject parent)
		{
			for (int i = 0; i < parent.Children.Count; i++)
			{
				if (this.monoBeh.IsCancellationRequested(TokenType.Import))
				{
					return;
				}
				FObject fobject = parent.Children[i];
				if (fobject.Data.IsEmpty)
				{
					FcuLogger.Debug("InstantiateGameObjects | continue | " + fobject.Data.NameHierarchy, FcuDebugSettingsFlags.LogDefault);
				}
				else
				{
					SyncHelper syncHelper;
					bool flag = this.monoBeh.SyncHelpers.IsExistsOnCurrentCanvas(fobject, out syncHelper);
					SyncHelper syncHelper2;
					if (flag)
					{
						FcuLogger.Debug("InstantiateGameObjects | 1 | " + fobject.Data.NameHierarchy, FcuDebugSettingsFlags.LogGameObjectDrawer);
					}
					else if (this.monoBeh.CurrentProject.HasLocalPrefab(fobject.Data, out syncHelper2))
					{
						FcuLogger.Debug("InstantiateGameObjects | 2 | " + fobject.Data.NameHierarchy, FcuDebugSettingsFlags.LogGameObjectDrawer);
						int num = 0;
						this.monoBeh.SyncHelpers.SetFcuToAllChilds(syncHelper.gameObject, ref num);
						this.SetFigmaIds(fobject, syncHelper);
						if (fobject.ContainsTag(FcuTag.Frame))
						{
							syncHelper.transform.SetParent(this.monoBeh.transform, false);
							goto IL_025A;
						}
						goto IL_025A;
					}
					else
					{
						FcuLogger.Debug("InstantiateGameObjects | 3 | " + fobject.Data.NameHierarchy, FcuDebugSettingsFlags.LogGameObjectDrawer);
						syncHelper = MonoBehExtensions.CreateEmptyGameObject(null, null).AddComponent<SyncHelper>();
					}
					fobject.SetData(syncHelper, this.monoBeh);
					fobject.Data.GameObject.name = fobject.Data.Names.ObjectName;
					if (!flag)
					{
						UnityEvent<FigmaConverterUnity, FObject> onObjectInstantiate = this.monoBeh.Events.OnObjectInstantiate;
						if (onObjectInstantiate != null)
						{
							onObjectInstantiate.Invoke(this.monoBeh, fobject);
						}
					}
					if (this.monoBeh.IsUGUI())
					{
						RectTransform rectTransform;
						fobject.Data.GameObject.TryAddComponent<RectTransform>(out rectTransform, false);
					}
					fobject.Data.GameObject.transform.SetParent(this._tempFrames.transform);
					this.AddRectGameObject(fobject);
					int num2;
					if (fobject.ContainsTag(FcuTag.Blur))
					{
						num2 = LayerTools.AddLayer(FcuConfig.BlurredObjectTag);
					}
					else
					{
						num2 = this.monoBeh.Settings.MainSettings.GameObjectLayer;
					}
					fobject.Data.GameObject.layer = num2;
					fobject.Data.SiblingIndex = GameObjectDrawer.GetSiblingIndex(fobject, parent.Children);
					this.SetParent(fobject, parent);
					this.SetParentRect(fobject, parent);
					if (!fobject.Children.IsEmpty<FObject>())
					{
						this.DrawFObject(fobject);
					}
				}
				IL_025A:;
			}
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x0009AE19 File Offset: 0x00099019
		public static int GetSiblingIndex(FObject fobject, List<FObject> fobjects)
		{
			return fobjects.Select<FObject, string>((FObject x) => x.Id).ToList<string>().IndexOf(fobject.Id);
		}

		// Token: 0x0600185C RID: 6236 RVA: 0x0009AE54 File Offset: 0x00099054
		private void AddRectGameObject(FObject fobject)
		{
			GameObject gameObject = MonoBehExtensions.CreateEmptyGameObject(null, null);
			gameObject.name = fobject.Data.GameObject.name + " | RectTransform";
			gameObject.transform.SetParent(this._tempFrames.transform);
			fobject.Data.RectGameObject = gameObject;
			RectTransform rectTransform;
			fobject.Data.RectGameObject.TryAddComponent<RectTransform>(out rectTransform, false);
			Image image;
			fobject.Data.RectGameObject.TryAddComponent<Image>(out image, false);
			image.color = this.monoBeh.GraphicHelpers.GetRectTransformColor(fobject);
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x0009AEF3 File Offset: 0x000990F3
		private void SetParent(FObject fobject, FObject parent)
		{
			if (!fobject.Data.GameObject.transform.parent.IsPartOfAnyPrefab())
			{
				fobject.Data.ParentTransform = parent.Data.GameObject.transform;
			}
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x0009AF2F File Offset: 0x0009912F
		private void SetParentRect(FObject fobject, FObject parent)
		{
			if (!fobject.Data.RectGameObject.transform.parent.IsPartOfAnyPrefab())
			{
				fobject.Data.ParentTransformRect = parent.Data.RectGameObject.transform;
			}
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x0009AF6C File Offset: 0x0009916C
		private void SetFigmaIds(FObject rootFObject, SyncHelper rootSyncObject)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			foreach (int num in rootFObject.Data.ChildIndexes)
			{
				FObject fobject;
				if (this.monoBeh.CurrentProject.TryGetByIndex(num, out fobject))
				{
					dictionary.Add(fobject.Id, fobject.Data.Hash);
				}
			}
			foreach (SyncHelper syncHelper in rootSyncObject.GetComponentsInChildren<SyncHelper>(true))
			{
				string text = null;
				foreach (KeyValuePair<string, int> keyValuePair in dictionary)
				{
					if (keyValuePair.Value == syncHelper.Data.Hash)
					{
						text = keyValuePair.Key;
						break;
					}
				}
				if (text != null)
				{
					dictionary.Remove(text);
					syncHelper.Data.Id = text;
					FObject fobject2;
					if (this.monoBeh.CurrentProject.TryGetById(text, out fobject2))
					{
						this.SetFigmaIds(fobject2, syncHelper);
					}
				}
			}
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x0009B0B4 File Offset: 0x000992B4
		public async Task DestroyMissing(IEnumerable<SyncData> diffCheckResult)
		{
			foreach (SyncData syncData in diffCheckResult)
			{
				try
				{
					FcuLogger.Debug("DestroyMissing | " + syncData.NameHierarchy, FcuDebugSettingsFlags.LogDefault);
					syncData.GameObject.Destroy();
				}
				catch (Exception ex)
				{
					Debug.LogWarning(ex);
				}
				await Task.Yield();
			}
			IEnumerator<SyncData> enumerator = null;
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x0009B0F8 File Offset: 0x000992F8
		public async Task DestroyMissing(List<FObject> fobjects)
		{
			IEnumerable<SyncHelper> allSyncHelpers = this.monoBeh.SyncHelpers.GetAllSyncHelpers();
			ConcurrentBag<SyncHelper> toDestroy = new ConcurrentBag<SyncHelper>();
			Parallel.ForEach<SyncHelper>(allSyncHelpers, delegate(SyncHelper syncHelper)
			{
				bool flag = false;
				foreach (FObject fobject in fobjects)
				{
					if (syncHelper.Data.Id == fobject.Data.Id)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					FcuLogger.Debug("DestroyMissing | " + syncHelper.Data.NameHierarchy, FcuDebugSettingsFlags.LogDefault);
					toDestroy.Add(syncHelper);
				}
			});
			foreach (SyncHelper syncHelper2 in toDestroy)
			{
				try
				{
					syncHelper2.gameObject.Destroy();
				}
				catch
				{
				}
				await Task.Yield();
			}
			IEnumerator<SyncHelper> enumerator = null;
		}

		// Token: 0x06001862 RID: 6242 RVA: 0x0009B143 File Offset: 0x00099343
		public void ClearTempRectFrames()
		{
			this._tempFrames.Destroy();
		}

		// Token: 0x04001992 RID: 6546
		private GameObject _tempFrames;
	}
}
