using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Logging;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x020002B6 RID: 694
	[Serializable]
	public class AssetTools : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x0600139A RID: 5018 RVA: 0x0007EE58 File Offset: 0x0007D058
		public async Task ReselectFcu()
		{
			GameObject tempGo = MonoBehExtensions.CreateEmptyGameObject(null, null);
			await Task.Delay(100);
			tempGo.MakeGameObjectSelectedInHierarchy();
			await Task.Delay(100);
			this.SelectFcu();
			tempGo.Destroy();
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x0007EE9B File Offset: 0x0007D09B
		public void SelectFcu()
		{
			this.monoBeh.gameObject.MakeGameObjectSelectedInHierarchy();
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x0600139C RID: 5020 RVA: 0x0007EEAD File Offset: 0x0007D0AD
		// (set) Token: 0x0600139D RID: 5021 RVA: 0x0007EEBF File Offset: 0x0007D0BF
		public bool NeedShowRateMe
		{
			get
			{
				return !this.needShowRateMe && this.needShowRateMe;
			}
			set
			{
				this.needShowRateMe = value;
			}
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x0007EEC8 File Offset: 0x0007D0C8
		public void DestroyLastImportedFrames()
		{
			CancellationTokenSource cancellationTokenSource = this.monoBeh.CancellationTokenController.CreateNew(TokenType.Import);
			this.DestroyLastImportedFramesAsync(cancellationTokenSource);
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x0007EEF0 File Offset: 0x0007D0F0
		private async Task DestroyLastImportedFramesAsync(CancellationTokenSource cts)
		{
			foreach (SyncData syncData in this.monoBeh.CurrentProject.LastImportedFrames)
			{
				syncData.GameObject.Destroy();
			}
			this.monoBeh.CurrentProject.LastImportedFrames.Clear();
			await Task.Yield();
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x0007EF34 File Offset: 0x0007D134
		public static void CreateFcuOnScene()
		{
			GameObject gameObject = MonoBehExtensions.CreateEmptyGameObject(null, null);
			FigmaConverterUnity figmaConverterUnity;
			gameObject.TryAddComponent<FigmaConverterUnity>(out figmaConverterUnity, false);
			gameObject.name = string.Format(FcuConfig.CanvasGameObjectName, figmaConverterUnity.Guid);
			figmaConverterUnity.CanvasDrawer.AddCanvasComponent();
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x0007EF74 File Offset: 0x0007D174
		public void StopImport(StopImportReason reason)
		{
			this.monoBeh.CancellationTokenController.CancelAll();
			switch (reason)
			{
			case StopImportReason.Manual:
				DALogger.LogSuccess(FcuLocKey.log_import_stoped_manually.Localize(Array.Empty<object>()));
				return;
			case StopImportReason.Error:
				DALogger.Log(FcuLocKey.log_import_stoped_because_error.Localize(Array.Empty<object>()));
				return;
			case StopImportReason.TaskCanceled:
				DALogger.Log(FcuLocKey.log_import_task_canceled.Localize(Array.Empty<object>()));
				return;
			case StopImportReason.Hidden:
				break;
			case StopImportReason.End:
				DALogger.LogSuccess(FcuLocKey.log_import_complete.Localize(Array.Empty<object>()));
				break;
			default:
				return;
			}
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x0007EFFC File Offset: 0x0007D1FC
		internal void ShowRateMe()
		{
			int num = this.monoBeh.TagSetter.TagsCounter.Values.Sum();
			if (this.monoBeh.AssetTools.GetConsoleErrorCount() > 0 || num < 1)
			{
				this.needShowRateMe = false;
				return;
			}
			this.needShowRateMe = true;
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x0002AE8C File Offset: 0x0002908C
		public int GetConsoleErrorCount()
		{
			return 1;
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x0007F04C File Offset: 0x0007D24C
		public static int GetMaxFileNumber(string folderPath, string prefix, string extension)
		{
			string[] files = Directory.GetFiles(folderPath, prefix + "*." + extension, SearchOption.AllDirectories);
			int num = -1;
			string[] array = files;
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = AssetTools.ExtractFileNumber(Path.GetFileNameWithoutExtension(array[i]), prefix);
				if (num2 > num)
				{
					num = num2;
				}
			}
			return num;
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x0007F094 File Offset: 0x0007D294
		private static int ExtractFileNumber(string fileName, string prefix)
		{
			if (fileName == prefix)
			{
				return 0;
			}
			foreach (char c in new char[] { ' ', '-', '_' })
			{
				int num;
				if (fileName.StartsWith(prefix + c.ToString()) && int.TryParse(fileName.Substring(prefix.Length + 1), out num))
				{
					return num;
				}
			}
			return -1;
		}

		// Token: 0x04001572 RID: 5490
		[HideInInspector]
		[SerializeField]
		private bool needShowRateMe;
	}
}
