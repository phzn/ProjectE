using System;
using System.Collections.Generic;
using DA_Assets.DAI;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Logging;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x0200031E RID: 798
	[Serializable]
	public class CurrentProject : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x060015C2 RID: 5570 RVA: 0x0008D84C File Offset: 0x0008BA4C
		public string GetInstanceKey(string id)
		{
			foreach (KeyValuePair<string, FigmaComponent> keyValuePair in this.figmaProject.Components)
			{
				if (keyValuePair.Key == id)
				{
					return keyValuePair.Value.Key;
				}
			}
			return null;
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x0008D8C4 File Offset: 0x0008BAC4
		public bool TryGetById(string id, out FObject fobject)
		{
			foreach (FObject fobject2 in this.currentPage)
			{
				if (fobject2.Id == id)
				{
					fobject = fobject2;
					return true;
				}
			}
			fobject = default(FObject);
			return false;
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x0008D934 File Offset: 0x0008BB34
		public bool TryGetByIndex(int index, out FObject fobject)
		{
			if (index < 0)
			{
				fobject = default(FObject);
				return false;
			}
			fobject = this.currentPage[index];
			return true;
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x0008D956 File Offset: 0x0008BB56
		public bool TryGetParent(FObject fobject, out FObject parent)
		{
			if (fobject.Data.ParentIndex < 0)
			{
				parent = default(FObject);
				return false;
			}
			parent = this.currentPage[fobject.Data.ParentIndex];
			return true;
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x0008D990 File Offset: 0x0008BB90
		public FObject GetRootFrame(FObject fobject)
		{
			if (fobject.ContainsTag(FcuTag.Frame))
			{
				return fobject;
			}
			FObject fobject2;
			this.TryGetParent(fobject, out fobject2);
			return this.GetRootFrame(fobject2);
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x0008D9BC File Offset: 0x0008BBBC
		public bool HasLocalPrefab(SyncData fobject, out SyncHelper localPrefab)
		{
			foreach (SyncHelper syncHelper in this.localPrefabs)
			{
				if (syncHelper.Data.Hash == fobject.Hash)
				{
					localPrefab = syncHelper;
					return true;
				}
			}
			foreach (SyncHelper syncHelper2 in this.localPrefabs)
			{
				if (syncHelper2.Data.Id == fobject.Id && syncHelper2.Data.Names.FileName == fobject.Names.FileName)
				{
					localPrefab = syncHelper2;
					return true;
				}
			}
			localPrefab = null;
			return false;
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x0008DAA8 File Offset: 0x0008BCA8
		public void LoadLocalPrefabs()
		{
			DALogger.Log(FcuLocKey.log_search_local_prefabs.Localize(Array.Empty<object>()));
			this.localPrefabs = this.LoadAssetFromFolder<SyncHelper>(this.monoBeh.Settings.PrefabSettings.PrefabsPath, "t:Prefab");
			DALogger.Log(FcuLocKey.log_local_prefabs_found.Localize(new object[] { this.localPrefabs.Count }));
		}

		// Token: 0x060015C9 RID: 5577 RVA: 0x0008DB14 File Offset: 0x0008BD14
		public List<T> LoadAssetFromFolder<T>(string fontsPath, string customType = null) where T : Object
		{
			new List<string>();
			List<T> list = new List<T>();
			if (customType == null)
			{
				customType = "t:" + typeof(T).Name;
			}
			this.monoBeh.IsCancellationRequested(TokenType.Import);
			return list;
		}

		// Token: 0x060015CA RID: 5578 RVA: 0x0008DB5C File Offset: 0x0008BD5C
		public void SetRootFrames(List<FObject> fobjects)
		{
			DALogger.Log("SetRootFrames");
			for (int i = 0; i < fobjects.Count; i++)
			{
				if (!fobjects[i].ContainsTag(FcuTag.Page))
				{
					FObject rootFrame = this.monoBeh.CurrentProject.GetRootFrame(fobjects[i]);
					fobjects[i].Data.RootFrame = rootFrame.Data;
				}
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x060015CB RID: 5579 RVA: 0x0008DBC6 File Offset: 0x0008BDC6
		// (set) Token: 0x060015CC RID: 5580 RVA: 0x0008DBCE File Offset: 0x0008BDCE
		public List<SyncData> LastImportedFrames
		{
			get
			{
				return this.lastImportedFrames;
			}
			set
			{
				base.SetValue<List<SyncData>>(ref this.lastImportedFrames, value);
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x060015CD RID: 5581 RVA: 0x0008DBDD File Offset: 0x0008BDDD
		// (set) Token: 0x060015CE RID: 5582 RVA: 0x0008DBE5 File Offset: 0x0008BDE5
		public List<SyncHelper> LocalPrefabs
		{
			get
			{
				return this.localPrefabs;
			}
			set
			{
				base.SetValue<List<SyncHelper>>(ref this.localPrefabs, value);
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x060015CF RID: 5583 RVA: 0x0008DBF4 File Offset: 0x0008BDF4
		// (set) Token: 0x060015D0 RID: 5584 RVA: 0x0008DBFC File Offset: 0x0008BDFC
		public string ProjectName
		{
			get
			{
				return this.projectName;
			}
			set
			{
				this.projectName = value;
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x060015D1 RID: 5585 RVA: 0x0008DC05 File Offset: 0x0008BE05
		// (set) Token: 0x060015D2 RID: 5586 RVA: 0x0008DC0D File Offset: 0x0008BE0D
		public FigmaProject FigmaProject
		{
			get
			{
				return this.figmaProject;
			}
			set
			{
				this.figmaProject = value;
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x060015D3 RID: 5587 RVA: 0x0008DC16 File Offset: 0x0008BE16
		// (set) Token: 0x060015D4 RID: 5588 RVA: 0x0008DC1E File Offset: 0x0008BE1E
		public List<FObject> CurrentPage
		{
			get
			{
				return this.currentPage;
			}
			set
			{
				this.currentPage = value;
			}
		}

		// Token: 0x0400175A RID: 5978
		[SerializeField]
		private List<SyncData> lastImportedFrames = new List<SyncData>();

		// Token: 0x0400175B RID: 5979
		[SerializeField]
		private List<SyncHelper> localPrefabs = new List<SyncHelper>();

		// Token: 0x0400175C RID: 5980
		[SerializeField]
		private string projectName;

		// Token: 0x0400175D RID: 5981
		private FigmaProject figmaProject;

		// Token: 0x0400175E RID: 5982
		private List<FObject> currentPage = new List<FObject>();
	}
}
