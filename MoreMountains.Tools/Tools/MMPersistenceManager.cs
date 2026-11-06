using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoreMountains.Tools
{
	// Token: 0x02000139 RID: 313
	public class MMPersistenceManager : MMPersistentSingleton<MMPersistenceManager>, MMEventListener<MMGameEvent>, MMEventListenerBase
	{
		// Token: 0x060006F6 RID: 1782 RVA: 0x000221ED File Offset: 0x000203ED
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		protected static void InitializeStatics()
		{
			MMPersistentSingleton<MMPersistenceManager>._instance = null;
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x000221F5 File Offset: 0x000203F5
		protected override void Awake()
		{
			base.Awake();
			this.SceneDatas = new DictionaryStringSceneData();
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x00022208 File Offset: 0x00020408
		public virtual void SaveToMemory()
		{
			this.ComputeCurrentSceneName();
			this.SceneDatas.Remove(this._currentSceneName);
			MMPersistenceSceneData mmpersistenceSceneData = new MMPersistenceSceneData();
			mmpersistenceSceneData.ObjectDatas = new DictionaryStringString();
			foreach (IMMPersistent immpersistent in this.FindAllPersistentObjects())
			{
				if (immpersistent.ShouldBeSaved())
				{
					mmpersistenceSceneData.ObjectDatas.Add(immpersistent.GetGuid(), immpersistent.OnSave());
				}
			}
			this.SceneDatas.Add(this._currentSceneName, mmpersistenceSceneData);
			MMPersistenceEvent.Trigger(MMPersistenceEventType.DataSavedToMemory, this.PersistenceID);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x00022294 File Offset: 0x00020494
		public virtual void LoadFromMemory()
		{
			this.ComputeCurrentSceneName();
			MMPersistenceSceneData mmpersistenceSceneData;
			if (!this.SceneDatas.TryGetValue(this._currentSceneName, out mmpersistenceSceneData))
			{
				return;
			}
			if (mmpersistenceSceneData.ObjectDatas == null)
			{
				return;
			}
			foreach (IMMPersistent immpersistent in this.FindAllPersistentObjects())
			{
				string text;
				if (mmpersistenceSceneData.ObjectDatas.TryGetValue(immpersistent.GetGuid(), out text))
				{
					immpersistent.OnLoad(mmpersistenceSceneData.ObjectDatas[immpersistent.GetGuid()]);
				}
			}
			MMPersistenceEvent.Trigger(MMPersistenceEventType.DataLoadedFromMemory, this.PersistenceID);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00022318 File Offset: 0x00020518
		public virtual void SaveFromMemoryToFile()
		{
			MMSaveLoadManager.Save(new MMPersistenceManagerData
			{
				PersistenceID = this.PersistenceID,
				SaveDate = DateTime.Now.ToString(),
				SceneDatas = this.SceneDatas
			}, this.DetermineSaveName(), MMPersistenceManager._saveFolderName);
			MMPersistenceEvent.Trigger(MMPersistenceEventType.DataSavedFromMemoryToFile, this.PersistenceID);
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00022374 File Offset: 0x00020574
		public virtual void LoadFromFileToMemory()
		{
			MMPersistenceManagerData mmpersistenceManagerData = (MMPersistenceManagerData)MMSaveLoadManager.Load(typeof(MMPersistenceManagerData), this.DetermineSaveName(), MMPersistenceManager._saveFolderName);
			if (mmpersistenceManagerData != null && mmpersistenceManagerData.SceneDatas != null)
			{
				this.SceneDatas = new DictionaryStringSceneData();
				this.SceneDatas = mmpersistenceManagerData.SceneDatas;
			}
			MMPersistenceEvent.Trigger(MMPersistenceEventType.DataLoadedFromFileToMemory, this.PersistenceID);
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x000223CF File Offset: 0x000205CF
		public virtual void Save()
		{
			this.SaveToMemory();
			if (this.SaveToFileOnSaveEvents)
			{
				this.SaveFromMemoryToFile();
			}
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x000223E5 File Offset: 0x000205E5
		public virtual void Load()
		{
			if (this.LoadFromFileOnLoadEvents)
			{
				this.LoadFromFileToMemory();
			}
			this.LoadFromMemory();
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x000223FC File Offset: 0x000205FC
		public virtual void DeletePersistencyMemoryForScene(string sceneName)
		{
			MMPersistenceSceneData mmpersistenceSceneData;
			if (!this.SceneDatas.TryGetValue(this._currentSceneName, out mmpersistenceSceneData))
			{
				return;
			}
			this.SceneDatas.Remove(sceneName);
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0002242C File Offset: 0x0002062C
		public virtual void ResetPersistence()
		{
			this.DeletePersistenceMemory();
			this.DeletePersistenceFile();
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0002243A File Offset: 0x0002063A
		public virtual void DeletePersistenceMemory()
		{
			this.SceneDatas = new DictionaryStringSceneData();
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x00022447 File Offset: 0x00020647
		public virtual void DeletePersistenceFile()
		{
			MMSaveLoadManager.DeleteSave(this.DetermineSaveName(), MMPersistenceManager._saveFolderName);
			Debug.LogFormat("Persistence save file deleted", Array.Empty<object>());
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x00022468 File Offset: 0x00020668
		protected virtual IMMPersistent[] FindAllPersistentObjects()
		{
			return Object.FindObjectsOfType<MonoBehaviour>(true).OfType<IMMPersistent>().ToArray<IMMPersistent>();
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0002247C File Offset: 0x0002067C
		protected virtual void ComputeCurrentSceneName()
		{
			this._currentSceneName = SceneManager.GetActiveScene().name;
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0002249C File Offset: 0x0002069C
		protected virtual string DetermineSaveName()
		{
			return base.gameObject.name + "_" + this.PersistenceID + MMPersistenceManager._saveFileExtension;
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x000224C0 File Offset: 0x000206C0
		public virtual void OnMMEvent(MMGameEvent gameEvent)
		{
			if (gameEvent.EventName == "Save" && this.ListenForSaveEvents)
			{
				this.Save();
			}
			if (gameEvent.EventName == "Load" && this.ListenForLoadEvents)
			{
				this.Load();
			}
			if (gameEvent.EventName == "SaveToMemory" && this.ListenForSaveToMemoryEvents)
			{
				this.SaveToMemory();
			}
			if (gameEvent.EventName == "LoadFromMemory" && this.ListenForLoadFromMemoryEvents)
			{
				this.LoadFromMemory();
			}
			if (gameEvent.EventName == "SaveToFile" && this.ListenForSaveToFileEvents)
			{
				this.SaveFromMemoryToFile();
			}
			if (gameEvent.EventName == "LoadFromFile" && this.ListenForLoadFromFileEvents)
			{
				this.LoadFromFileToMemory();
			}
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0002258D File Offset: 0x0002078D
		protected virtual void OnEnable()
		{
			this.MMEventStartListening<MMGameEvent>();
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00022595 File Offset: 0x00020795
		protected virtual void OnDisable()
		{
			this.MMEventStopListening<MMGameEvent>();
		}

		// Token: 0x04000893 RID: 2195
		[Header("Persistence")]
		[Tooltip("A persistence ID used to identify the data associated to this manager. Usually you'll want to leave this to its default value.")]
		public string PersistenceID = "MMPersistency";

		// Token: 0x04000894 RID: 2196
		[Header("Events")]
		[Tooltip("whether or not this manager should listen for save events. If you set this to false, you'll have to call SaveToMemory or SaveFromMemoryToFile manually")]
		public bool ListenForSaveEvents = true;

		// Token: 0x04000895 RID: 2197
		[Tooltip("whether or not this manager should listen for load events. If you set this to false, you'll have to call LoadFromMemory or LoadFromFileToMemory manually")]
		public bool ListenForLoadEvents = true;

		// Token: 0x04000896 RID: 2198
		[Tooltip("whether or not this manager should listen for save to memory events. If you set this to false, you'll have to call SaveToMemory manually")]
		public bool ListenForSaveToMemoryEvents = true;

		// Token: 0x04000897 RID: 2199
		[Tooltip("whether or not this manager should listen for load from memory events. If you set this to false, you'll have to call LoadFromMemory manually")]
		public bool ListenForLoadFromMemoryEvents = true;

		// Token: 0x04000898 RID: 2200
		[Tooltip("whether or not this manager should listen for save to file events. If you set this to false, you'll have to call SaveFromMemoryToFile manually")]
		public bool ListenForSaveToFileEvents = true;

		// Token: 0x04000899 RID: 2201
		[Tooltip("whether or not this manager should listen for load from file events. If you set this to false, you'll have to call LoadFromFileToMemory manually")]
		public bool ListenForLoadFromFileEvents = true;

		// Token: 0x0400089A RID: 2202
		[Tooltip("whether or not this manager should save data to file on save events")]
		public bool SaveToFileOnSaveEvents = true;

		// Token: 0x0400089B RID: 2203
		[Tooltip("whether or not this manager should load data from file on load events")]
		public bool LoadFromFileOnLoadEvents = true;

		// Token: 0x0400089C RID: 2204
		[Header("Debug Buttons (Only at Runtime)")]
		[MMInspectorButton("SaveToMemory")]
		public bool SaveToMemoryButton;

		// Token: 0x0400089D RID: 2205
		[MMInspectorButton("LoadFromMemory")]
		public bool LoadFromMemoryButton;

		// Token: 0x0400089E RID: 2206
		[MMInspectorButton("SaveFromMemoryToFile")]
		public bool SaveToFileButton;

		// Token: 0x0400089F RID: 2207
		[MMInspectorButton("LoadFromFileToMemory")]
		public bool LoadFromFileButton;

		// Token: 0x040008A0 RID: 2208
		[MMInspectorButton("DeletePersistenceFile")]
		public bool DeletePersistenceFileButton;

		// Token: 0x040008A1 RID: 2209
		public DictionaryStringSceneData SceneDatas;

		// Token: 0x040008A2 RID: 2210
		public static string _resourceItemPath = "Persistence/";

		// Token: 0x040008A3 RID: 2211
		public static string _saveFolderName = "MMTools/";

		// Token: 0x040008A4 RID: 2212
		public static string _saveFileExtension = ".persistence";

		// Token: 0x040008A5 RID: 2213
		protected string _currentSceneName;
	}
}
