using System;
using DA_Assets.DAI;

namespace DA_Assets.FCU
{
	// Token: 0x020002C4 RID: 708
	[Serializable]
	public class FcuEventHandlers : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x060013D8 RID: 5080 RVA: 0x0007F946 File Offset: 0x0007DB46
		public static void CreateFcu_OnClick()
		{
			AssetTools.CreateFcuOnScene();
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x0007F94D File Offset: 0x0007DB4D
		public void Auth_OnClick()
		{
			this.monoBeh.Authorizer.Auth();
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x0007F95F File Offset: 0x0007DB5F
		public void DownloadProject_OnClick()
		{
			this.monoBeh.ProjectDownloader.DownloadProject();
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x0007F971 File Offset: 0x0007DB71
		public void ImportSelectedFrames_OnClick()
		{
			this.monoBeh.ProjectImporter.StartImport();
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x0007F983 File Offset: 0x0007DB83
		public void GenerateScripts_OnClick()
		{
			this.monoBeh.ScriptGenerator.GenerateScripts();
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x0007F995 File Offset: 0x0007DB95
		public void SerializeObjects_OnClick()
		{
			this.monoBeh.ScriptGenerator.Serialize();
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x0007F9A7 File Offset: 0x0007DBA7
		public void CreatePrefabs_OnClick()
		{
			this.monoBeh.PrefabCreator.CreatePrefabs();
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x0007F9B9 File Offset: 0x0007DBB9
		public void UpdatePrefabs_OnClick()
		{
			this.monoBeh.PrefabUpdater.UpdatePrefabs();
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x0007F9CB File Offset: 0x0007DBCB
		public void DestroyLastImportedFrames_OnClick()
		{
			this.monoBeh.AssetTools.DestroyLastImportedFrames();
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x0007F9DD File Offset: 0x0007DBDD
		public void DestroySyncHelpers_OnClick()
		{
			this.monoBeh.SyncHelpers.DestroySyncHelpers();
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x0007F9EF File Offset: 0x0007DBEF
		public void SetFcuToSyncHelpers_OnClick()
		{
			this.monoBeh.SyncHelpers.SetFcuToAllSyncHelpers();
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x0007FA01 File Offset: 0x0007DC01
		public void OptimizeSyncHelpers_OnClick()
		{
			this.monoBeh.SyncHelpers.OptimizeSyncHelpers();
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x0007FA13 File Offset: 0x0007DC13
		public void StopImport_OnClick()
		{
			this.monoBeh.AssetTools.StopImport(StopImportReason.Manual);
		}
	}
}
