using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Logging;
using UnityEngine.Events;

namespace DA_Assets.FCU
{
	// Token: 0x0200032A RID: 810
	[Serializable]
	public class ProjectDownloader : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x060015F2 RID: 5618 RVA: 0x0008E25C File Offset: 0x0008C45C
		public void DownloadProject()
		{
			if (!this.monoBeh.IsJsonNetExists())
			{
				DALogger.LogError(FcuLocKey.log_cant_find_package.Localize(new object[] { "com.unity.nuget.newtonsoft-json" }));
				return;
			}
			this.monoBeh.AssetTools.StopImport(StopImportReason.Hidden);
			CancellationTokenSource cancellationTokenSource = this.monoBeh.CancellationTokenController.CreateNew(TokenType.Import);
			this.DownloadProjectAsync(cancellationTokenSource);
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x0008E2BC File Offset: 0x0008C4BC
		private async Task DownloadProjectAsync(CancellationTokenSource cts)
		{
			this.monoBeh.InspectorDrawer.SelectableDocument.Childs.Clear();
			if (!this.monoBeh.Authorizer.IsAuthed())
			{
				DALogger.LogError(FcuLocKey.log_need_auth.Localize(Array.Empty<object>()));
				UnityEvent<FigmaConverterUnity> onProjectDownloadFail = this.monoBeh.Events.OnProjectDownloadFail;
				if (onProjectDownloadFail != null)
				{
					onProjectDownloadFail.Invoke(this.monoBeh);
				}
			}
			else
			{
				UnityEvent<FigmaConverterUnity> onProjectDownloadStart = this.monoBeh.Events.OnProjectDownloadStart;
				if (onProjectDownloadStart != null)
				{
					onProjectDownloadStart.Invoke(this.monoBeh);
				}
				DARequest darequest = RequestCreator.CreateProjectRequest(this.monoBeh.RequestSender.GetRequestHeader(this.monoBeh.Authorizer.Token, null), this.monoBeh.Settings.MainSettings.ProjectUrl);
				DAResult<FigmaProject> daresult = await this.monoBeh.RequestSender.SendRequest<FigmaProject>(darequest);
				if (daresult.Success)
				{
					this.monoBeh.CurrentProject.FigmaProject = daresult.Object;
					this.monoBeh.CurrentProject.ProjectName = daresult.Object.Name;
					this.monoBeh.InspectorDrawer.FillSelectableFramesArray(this.monoBeh.CurrentProject.FigmaProject.Document, 2);
					DALogger.Log(FcuLocKey.log_project_downloaded.Localize(Array.Empty<object>()));
					UnityEvent<FigmaConverterUnity> onProjectDownloaded = this.monoBeh.Events.OnProjectDownloaded;
					if (onProjectDownloaded != null)
					{
						onProjectDownloaded.Invoke(this.monoBeh);
					}
				}
				else
				{
					int status = daresult.Error.Status;
					if (status != 403)
					{
						if (status != 404)
						{
							DALogger.LogError(FcuLocKey.log_unknown_error.Localize(new object[]
							{
								daresult.Error.Message,
								daresult.Error.Status,
								daresult.Error.Exception
							}));
						}
						else
						{
							DALogger.LogError(FcuLocKey.log_project_not_found.Localize(Array.Empty<object>()));
						}
					}
					else
					{
						DALogger.LogError(FcuLocKey.log_need_auth.Localize(Array.Empty<object>()));
					}
					UnityEvent<FigmaConverterUnity> onProjectDownloadFail2 = this.monoBeh.Events.OnProjectDownloadFail;
					if (onProjectDownloadFail2 != null)
					{
						onProjectDownloadFail2.Invoke(this.monoBeh);
					}
				}
			}
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x0008E300 File Offset: 0x0008C500
		public async Task<List<FObject>> DownloadAllNodes(List<string> selectedIds)
		{
			ProjectDownloader.<>c__DisplayClass2_0 CS$<>8__locals1 = new ProjectDownloader.<>c__DisplayClass2_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.nodeChunks = new List<List<FObject>>();
			List<List<string>> idChunks = selectedIds.Split<string>(FcuConfig.ChunkSizeGetNodes);
			foreach (List<string> list in idChunks)
			{
				ProjectDownloader.<>c__DisplayClass2_1 CS$<>8__locals2 = new ProjectDownloader.<>c__DisplayClass2_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				string text = string.Join(",", list);
				CS$<>8__locals2.projectRequest = RequestCreator.CreateNodeRequest(this.monoBeh.RequestSender.GetRequestHeader(this.monoBeh.Authorizer.Token, null), this.monoBeh.Settings.MainSettings.ProjectUrl, text);
				Task.Run(delegate
				{
					ProjectDownloader.<>c__DisplayClass2_1.<<DownloadAllNodes>b__0>d <<DownloadAllNodes>b__0>d;
					<<DownloadAllNodes>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<DownloadAllNodes>b__0>d.<>4__this = CS$<>8__locals2;
					<<DownloadAllNodes>b__0>d.<>1__state = -1;
					<<DownloadAllNodes>b__0>d.<>t__builder.Start<ProjectDownloader.<>c__DisplayClass2_1.<<DownloadAllNodes>b__0>d>(ref <<DownloadAllNodes>b__0>d);
					return <<DownloadAllNodes>b__0>d.<>t__builder.Task;
				});
			}
			int tempCount = -1;
			while (FcuLogger.WriteLogBeforeEqual(CS$<>8__locals1.nodeChunks, idChunks, FcuLocKey.log_getting_frames, CS$<>8__locals1.nodeChunks.CountAll<FObject>(), selectedIds.Count<string>(), ref tempCount))
			{
				await Task.Delay(1000);
			}
			return CS$<>8__locals1.nodeChunks.FromChunks<FObject>();
		}
	}
}
