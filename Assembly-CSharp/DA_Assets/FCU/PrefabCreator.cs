using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Logging;
using DA_Assets.Tools;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x020002D6 RID: 726
	[Serializable]
	public class PrefabCreator : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x0600143C RID: 5180 RVA: 0x00082A3E File Offset: 0x00080C3E
		public void CreatePrefabs()
		{
			this.CreatePrefabsAsync();
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x00082A48 File Offset: 0x00080C48
		private async Task CreatePrefabsAsync()
		{
			CancellationToken token = this.monoBeh.CancellationTokenController.CreateNew(TokenType.Prefab).Token;
			try
			{
				DALogger.Log(FcuLocKey.log_start_creating_prefabs.Localize(Array.Empty<object>()));
				await Task.Delay(100);
				if (!SceneBackuper.TryBackupActiveScene())
				{
					DALogger.LogError(FcuLocKey.log_cant_execute_because_no_backup.Localize(Array.Empty<object>()));
				}
				else
				{
					SceneBackuper.MakeActiveSceneDirty();
					this.pstructs = new List<PrefabStruct>();
					this.toDestroy = new List<GameObject>();
					this.createdPrefabs = new Dictionary<int, SyncHelper>();
					this.syncHelpers = this.monoBeh.SyncHelpers.GetAllSyncHelpers();
					this.monoBeh.SyncHelpers.RestoreRootFrames(this.syncHelpers);
					for (int i = 0; i < this.syncHelpers.Length; i++)
					{
						SyncHelper syncHelper = this.syncHelpers[i];
						if (!syncHelper.IsPartOfAnyPrefab())
						{
							PrefabStruct prefabStruct = this.CreatePrefabStruct(syncHelper, i);
							prefabStruct.Name = syncHelper.name;
							this.pstructs.Add(prefabStruct);
						}
					}
					int num = 1;
					Dictionary<int, int> dictionary = new Dictionary<int, int>();
					for (int j = 0; j < this.pstructs.Count; j++)
					{
						PrefabStruct prefabStruct = this.pstructs[j];
						int num2;
						if (dictionary.TryGetValue(prefabStruct.Hash, out num2))
						{
							prefabStruct.PrefabNumber = num2;
						}
						else
						{
							prefabStruct.PrefabNumber = num;
							dictionary[prefabStruct.Hash] = num;
							num++;
						}
						this.pstructs[j] = prefabStruct;
					}
					int prefabCount = 0;
					for (int k = 0; k < this.pstructs.Count; k++)
					{
						PrefabStruct prefabStruct = this.pstructs[k];
						SyncHelper syncHelper2;
						if (!this.createdPrefabs.TryGetValue(prefabStruct.PrefabNumber, out syncHelper2))
						{
							SyncHelper syncHelper = this.syncHelpers[prefabStruct.Current];
							if (this.CanBePrefab(syncHelper.Data))
							{
								this.CreatePrefab(k);
								prefabCount++;
							}
						}
					}
					for (int l = 0; l < this.pstructs.Count; l++)
					{
						PrefabStruct prefabStruct = this.pstructs[l];
						SyncHelper syncHelper3;
						if (this.createdPrefabs.TryGetValue(prefabStruct.PrefabNumber, out syncHelper3))
						{
							SyncHelper syncHelper = this.syncHelpers[prefabStruct.Current];
							if (this.CanBePrefab(syncHelper.Data))
							{
								this.InstantiatePrefab(l, syncHelper3);
							}
						}
						else
						{
							Debug.Log("missing prefab");
						}
					}
					for (int m = 0; m < this.pstructs.Count; m++)
					{
						PrefabStruct prefabStruct = this.pstructs[m];
						SyncHelper syncHelper = this.syncHelpers[prefabStruct.Current];
						syncHelper.transform.SetParentEx(this.GetParentTransform(prefabStruct));
					}
					for (int n = 0; n < this.pstructs.Count; n++)
					{
						PrefabStruct prefabStruct = this.pstructs[n];
						SyncHelper syncHelper = this.syncHelpers[prefabStruct.Current];
						syncHelper.transform.SetSiblingIndex(prefabStruct.SiblingIndex);
					}
					foreach (GameObject gameObject in this.toDestroy)
					{
						gameObject.Destroy();
					}
					new HashSet<SyncHelper>();
					for (int num3 = 0; num3 < this.pstructs.Count; num3++)
					{
						PrefabStruct prefabStruct = this.pstructs[num3];
						SyncHelper syncHelper4;
						if (this.createdPrefabs.TryGetValue(prefabStruct.PrefabNumber, out syncHelper4))
						{
							SyncHelper syncHelper = this.syncHelpers[prefabStruct.Current];
						}
					}
					for (int num4 = 0; num4 < this.pstructs.Count; num4++)
					{
						PrefabStruct prefabStruct = this.pstructs[num4];
						SyncHelper syncHelper = this.syncHelpers[prefabStruct.Current];
						try
						{
							if (syncHelper.Data.Tags.Contains(FcuTag.Text))
							{
								syncHelper.gameObject.SetText(prefabStruct.Text);
							}
						}
						catch
						{
						}
					}
					await this.monoBeh.SyncHelpers.SetFcuToAllSyncHelpersAsync(null);
					this.monoBeh.SyncHelpers.RestoreRootFrames(this.syncHelpers);
					DALogger.Log(FcuLocKey.log_prefabs_created.Localize(new object[] { prefabCount }));
				}
			}
			catch (OperationCanceledException)
			{
				Debug.Log("The task was stopped.");
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x0000D91D File Offset: 0x0000BB1D
		private static bool IsInsideAddedGameObject(GameObject gameObject)
		{
			return false;
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x00082A8B File Offset: 0x00080C8B
		private Transform GetParentTransform(PrefabStruct ps)
		{
			if (ps.Parent != -1)
			{
				return this.syncHelpers[ps.Parent].transform;
			}
			return this.monoBeh.transform;
		}

		// Token: 0x06001440 RID: 5184 RVA: 0x00082AB8 File Offset: 0x00080CB8
		private PrefabStruct CreatePrefabStruct(SyncHelper syncHelper, int i)
		{
			PrefabStruct prefabStruct = default(PrefabStruct);
			prefabStruct.Hash = syncHelper.Data.Hash;
			prefabStruct.Id = syncHelper.Data.Id;
			prefabStruct.Current = i;
			SyncHelper component = syncHelper.transform.parent.GetComponent<SyncHelper>();
			int num = Array.IndexOf<SyncHelper>(this.syncHelpers, component);
			prefabStruct.Parent = num;
			List<SyncHelper> list = syncHelper.GetComponentsInChildren<SyncHelper>(true).ToList<SyncHelper>();
			list.RemoveAt(0);
			prefabStruct.Childs = list.Select<SyncHelper, int>((SyncHelper x) => Array.IndexOf<SyncHelper>(this.syncHelpers, x)).ToList<int>();
			if (this.monoBeh.IsUGUI())
			{
				prefabStruct.UguiTransformData = UguiTransformData.Create(syncHelper.GetComponent<RectTransform>());
			}
			prefabStruct.SiblingIndex = syncHelper.transform.GetSiblingIndex();
			prefabStruct.PrefabNumber = 0;
			if (syncHelper.ContainsTag(FcuTag.Text))
			{
				prefabStruct.Text = syncHelper.gameObject.GetText();
			}
			return prefabStruct;
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x00082BAC File Offset: 0x00080DAC
		private void CreatePrefab(int i)
		{
			PrefabStruct prefabStruct = this.pstructs[i];
			this.RemoveParent(prefabStruct);
			string prefabPath = this.GetPrefabPath(prefabStruct);
			SyncHelper syncHelper;
			Exception ex;
			if (this.syncHelpers[prefabStruct.Current].gameObject.SaveAsPrefabAsset<SyncHelper>(prefabPath, out syncHelper, out ex))
			{
				this.createdPrefabs.TryAddValue(prefabStruct.PrefabNumber, syncHelper);
			}
			else
			{
				DALogger.LogException(ex);
			}
			this.RestoreParent(prefabStruct);
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x00082C18 File Offset: 0x00080E18
		private void RestoreParent(PrefabStruct ps)
		{
			this.syncHelpers[ps.Current].transform.SetParentEx(this.GetParentTransform(ps));
			foreach (int num in ps.Childs)
			{
				this.syncHelpers[num].transform.SetParentEx(this.syncHelpers[ps.Current].transform);
			}
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x00082CAC File Offset: 0x00080EAC
		private void RemoveParent(PrefabStruct ps)
		{
			this.syncHelpers[ps.Current].transform.SetParentEx(null);
			foreach (int num in ps.Childs)
			{
				this.syncHelpers[num].transform.SetParentEx(null);
			}
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x00082D28 File Offset: 0x00080F28
		private void InstantiatePrefab(int i, SyncHelper savedPrefab)
		{
			PrefabStruct prefabStruct = this.pstructs[i];
			SyncHelper syncHelper = this.syncHelpers[prefabStruct.Current];
			SyncHelper syncHelper2 = null;
			syncHelper2.name = syncHelper.name;
			syncHelper2.transform.SetParentEx(this.GetParentTransform(prefabStruct));
			syncHelper2.transform.SetSiblingIndex(prefabStruct.SiblingIndex);
			if (this.monoBeh.IsUGUI())
			{
				prefabStruct.UguiTransformData.ApplyTo(syncHelper2.GetComponent<RectTransform>());
			}
			else
			{
				this.monoBeh.IsNova();
			}
			syncHelper2.Data.Id = prefabStruct.Id;
			this.syncHelpers[prefabStruct.Current] = syncHelper2;
			this.toDestroy.Add(syncHelper.gameObject);
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x00082DE8 File Offset: 0x00080FE8
		private string GetPrefabName(SyncHelper sh)
		{
			string text;
			if (this.monoBeh.Settings.PrefabSettings.TextPrefabNameType != TextPrefabNameType.Figma && sh.Data.Tags.Contains(FcuTag.Text))
			{
				text = string.Format("{0} {1}", sh.Data.Names.HumanizedTextPrefabName.Trim(), sh.Data.Hash);
			}
			else
			{
				text = string.Format("{0} {1}", sh.gameObject.name.Trim(), sh.Data.Hash);
			}
			return text;
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x00082E84 File Offset: 0x00081084
		private string GetPrefabPath(PrefabStruct ps)
		{
			SyncHelper syncHelper = this.syncHelpers[ps.Current];
			if (syncHelper.Data.RootFrame == null)
			{
				SyncData rootFrame = this.monoBeh.SyncHelpers.GetRootFrame(syncHelper.Data);
				syncHelper.Data.RootFrame = rootFrame;
			}
			string text = Path.Combine(this.monoBeh.Settings.PrefabSettings.PrefabsPath, syncHelper.Data.RootFrame.Names.FileName);
			string text2 = PrefabCreator.<GetPrefabPath>g__GetSubDir|14_0(syncHelper.Data);
			Path.Combine(text, text2).CreateFolderIfNotExists();
			string prefabName = this.GetPrefabName(syncHelper);
			return Path.Combine(text, text2, prefabName + ".prefab").GetPathRelativeToProjectDirectory();
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x00082F35 File Offset: 0x00081135
		private bool CanBePrefab(SyncData sd)
		{
			return !sd.Tags.Contains(FcuTag.Container) || sd.Tags.Count != 1;
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x00082F7C File Offset: 0x0008117C
		[CompilerGenerated]
		internal static string <GetPrefabPath>g__GetSubDir|14_0(SyncData sd)
		{
			string text;
			try
			{
				if (sd.Tags.Contains(FcuTag.Text))
				{
					text = "Texts";
				}
				else if (sd.Tags.Contains(FcuTag.Image))
				{
					text = "Images";
				}
				else if (sd.Tags.Contains(FcuTag.Button))
				{
					text = "Buttons";
				}
				else if (sd.Tags.Contains(FcuTag.InputField))
				{
					text = "InputFields";
				}
				else
				{
					text = "Other";
				}
			}
			catch
			{
				text = "Other";
			}
			return text;
		}

		// Token: 0x040015CF RID: 5583
		private List<GameObject> toDestroy;

		// Token: 0x040015D0 RID: 5584
		[SerializeField]
		private List<PrefabStruct> pstructs;

		// Token: 0x040015D1 RID: 5585
		private Dictionary<int, SyncHelper> createdPrefabs = new Dictionary<int, SyncHelper>();

		// Token: 0x040015D2 RID: 5586
		[SerializeField]
		private SyncHelper[] syncHelpers;
	}
}
