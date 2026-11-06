using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.Collections;
using UnityEngine;

namespace Unity.Entities.Content
{
	// Token: 0x02000465 RID: 1125
	public class ContentDownloadService : IDisposable
	{
		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06001BD5 RID: 7125 RVA: 0x00061704 File Offset: 0x0005F904
		// (set) Token: 0x06001BD6 RID: 7126 RVA: 0x0006170C File Offset: 0x0005F90C
		public string CacheRoot { get; protected set; }

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06001BD7 RID: 7127 RVA: 0x00061715 File Offset: 0x0005F915
		// (set) Token: 0x06001BD8 RID: 7128 RVA: 0x0006171D File Offset: 0x0005F91D
		public string Name { get; protected set; }

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06001BD9 RID: 7129 RVA: 0x00061726 File Offset: 0x0005F926
		// (set) Token: 0x06001BDA RID: 7130 RVA: 0x0006172E File Offset: 0x0005F92E
		public int Priority { get; set; }

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06001BDB RID: 7131 RVA: 0x00061737 File Offset: 0x0005F937
		public long TotalDownloadedBytes
		{
			get
			{
				return this.activeDownloadedBytes + this.completeDownloadedBytes;
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06001BDC RID: 7132 RVA: 0x00061746 File Offset: 0x0005F946
		// (set) Token: 0x06001BDD RID: 7133 RVA: 0x0006174E File Offset: 0x0005F94E
		public long TotalBytes { get; private set; }

		// Token: 0x06001BDE RID: 7134 RVA: 0x00061758 File Offset: 0x0005F958
		public ContentDownloadService(string name, string cacheDir, int priority = 1, int maxActiveDownloads = 5, Func<ContentDownloadService.DownloadOperation> createDownloadOpFunc = null)
		{
			this.Name = name;
			this.Priority = priority;
			this.maxActive = maxActiveDownloads;
			this.CacheRoot = cacheDir;
			this.activeDownloads = new LinkedList<ContentDownloadService.DownloadOperation>();
			this.downloadStates = new Dictionary<RemoteContentLocation, ContentDownloadService.DownloadStatus>();
			Func<ContentDownloadService.DownloadOperation> func;
			if (createDownloadOpFunc != null)
			{
				func = createDownloadOpFunc;
			}
			else
			{
				func = () => new DownloadOperationUnityWebRequest();
			}
			this.createOpFunc = func;
			Directory.CreateDirectory(cacheDir);
		}

		// Token: 0x06001BDF RID: 7135 RVA: 0x00002287 File Offset: 0x00000487
		public void Dispose()
		{
		}

		// Token: 0x06001BE0 RID: 7136 RVA: 0x00002287 File Offset: 0x00000487
		public void OnAddedToDeliveryService(ContentDeliveryService contentDeliveryService)
		{
		}

		// Token: 0x06001BE1 RID: 7137 RVA: 0x000617DC File Offset: 0x0005F9DC
		public void CancelDownload(RemoteContentLocation loc)
		{
			ContentDownloadService.DownloadStatus downloadStatus;
			if (this.downloadStates.TryGetValue(loc, out downloadStatus))
			{
				downloadStatus.DownloadState = ContentDownloadService.State.Cancelled;
				this.downloadStates[loc] = downloadStatus;
			}
			LinkedListNode<ContentDownloadService.DownloadOperation> next;
			for (LinkedListNode<ContentDownloadService.DownloadOperation> linkedListNode = this.activeDownloads.First; linkedListNode != null; linkedListNode = next)
			{
				ContentDownloadService.DownloadOperation value = linkedListNode.Value;
				next = linkedListNode.Next;
				if (value.Location.Equals(loc))
				{
					value.Cancel();
					this.activeDownloads.Remove(linkedListNode);
				}
			}
		}

		// Token: 0x06001BE2 RID: 7138 RVA: 0x00061850 File Offset: 0x0005FA50
		public ContentDownloadService.DownloadStatus GetDownloadStatus(in RemoteContentLocation loc)
		{
			ContentDownloadService.DownloadStatus downloadStatus;
			string text;
			if (!this.downloadStates.TryGetValue(loc, out downloadStatus) && this.GetLocalCacheFilePath(loc, out text))
			{
				downloadStatus = new ContentDownloadService.DownloadStatus
				{
					BytesDownloaded = loc.Size,
					LocalPath = text,
					DownloadState = ContentDownloadService.State.Complete
				};
				this.downloadStates[loc] = downloadStatus;
			}
			return downloadStatus;
		}

		// Token: 0x06001BE3 RID: 7139 RVA: 0x000618C1 File Offset: 0x0005FAC1
		public bool CanDownload(RemoteContentLocation location)
		{
			return location.Type == RemoteContentLocation.LocationType.RemoteURL;
		}

		// Token: 0x06001BE4 RID: 7140 RVA: 0x000618CC File Offset: 0x0005FACC
		public string ComputeCachePath(RemoteContentLocation loc)
		{
			if (!loc.Hash.IsValid)
			{
				return string.Empty;
			}
			string text = loc.Hash.ToString();
			StringBuilder stringBuilder = new StringBuilder(this.CacheRoot, this.CacheRoot.Length + text.Length + 4);
			stringBuilder.Append(Path.DirectorySeparatorChar);
			stringBuilder.Append(text[0]);
			stringBuilder.Append(text[1]);
			stringBuilder.Append(Path.DirectorySeparatorChar);
			stringBuilder.Append(text);
			return stringBuilder.ToString();
		}

		// Token: 0x06001BE5 RID: 7141 RVA: 0x00061960 File Offset: 0x0005FB60
		public bool GetLocalCacheFilePath(RemoteContentLocation loc, out string path)
		{
			string text;
			path = (text = this.ComputeCachePath(loc));
			return File.Exists(text);
		}

		// Token: 0x06001BE6 RID: 7142 RVA: 0x00061980 File Offset: 0x0005FB80
		public void ClearDownloadProgress()
		{
			this.TotalBytes = (this.activeDownloadedBytes = (this.completeDownloadedBytes = 0L));
		}

		// Token: 0x06001BE7 RID: 7143 RVA: 0x000619A8 File Offset: 0x0005FBA8
		public void GetDownloadProgress(RemoteContentLocation loc, ref long contentSize, ref long downloadedBytes)
		{
			contentSize += loc.Size;
			ContentDownloadService.DownloadStatus downloadStatus;
			if (this.downloadStates.TryGetValue(loc, out downloadStatus))
			{
				downloadedBytes += downloadStatus.BytesDownloaded;
			}
		}

		// Token: 0x06001BE8 RID: 7144 RVA: 0x000619DC File Offset: 0x0005FBDC
		public ContentDownloadService.DownloadStatus DownloadContent(in RemoteContentLocation loc)
		{
			ContentDownloadService.DownloadStatus downloadStatus;
			if (this.downloadStates.TryGetValue(loc, out downloadStatus) && (downloadStatus.DownloadState == ContentDownloadService.State.Cancelled || downloadStatus.DownloadState == ContentDownloadService.State.Failed))
			{
				this.downloadStates.Remove(loc);
			}
			ContentDownloadService.DownloadStatus downloadStatus2;
			if (!this.downloadStates.TryGetValue(loc, out downloadStatus2))
			{
				this.TotalBytes += loc.Size;
				Hash128 hash = loc.Hash;
				string text2;
				if (!hash.IsValid)
				{
					ContentDownloadService.DownloadOperation downloadOperation = this.createOpFunc();
					string text = Path.Combine(Application.persistentDataPath, Guid.NewGuid().ToString());
					downloadOperation.Init(loc, text + ".tmpdownload", text);
					this.activeDownloads.AddLast(downloadOperation);
					downloadStatus2.DownloadState = ContentDownloadService.State.Queued;
				}
				else if (this.GetLocalCacheFilePath(loc, out text2))
				{
					File.SetLastAccessTime(text2, DateTime.Now);
					downloadStatus2.DownloadState = ContentDownloadService.State.Complete;
					downloadStatus2.BytesDownloaded = loc.Size;
					downloadStatus2.LocalPath = text2;
				}
				else
				{
					ContentDownloadService.DownloadOperation downloadOperation2 = this.createOpFunc();
					downloadOperation2.Init(loc, text2 + ".tmpdownload", text2);
					this.activeDownloads.AddLast(downloadOperation2);
					downloadStatus2.DownloadState = ContentDownloadService.State.Queued;
				}
				this.downloadStates[loc] = downloadStatus2;
				this.Process();
			}
			return this.downloadStates[loc];
		}

		// Token: 0x06001BE9 RID: 7145 RVA: 0x00061B64 File Offset: 0x0005FD64
		public void Process()
		{
			this.activeDownloadedBytes = 0L;
			int num = 0;
			LinkedListNode<ContentDownloadService.DownloadOperation> next;
			for (LinkedListNode<ContentDownloadService.DownloadOperation> linkedListNode = this.activeDownloads.First; linkedListNode != null; linkedListNode = next)
			{
				ContentDownloadService.DownloadOperation value = linkedListNode.Value;
				next = linkedListNode.Next;
				ContentDownloadService.DownloadStatus downloadStatus = this.downloadStates[value.Location];
				if (!value.IsStarted && num < this.maxActive)
				{
					value.Start(ref downloadStatus);
					num++;
				}
				if (value.IsStarted)
				{
					long num2 = 0L;
					if (value.Process(ref downloadStatus, ref num2))
					{
						this.activeDownloads.Remove(linkedListNode);
						this.completeDownloadedBytes += num2;
					}
					else
					{
						num++;
						this.activeDownloadedBytes += num2;
					}
				}
				this.downloadStates[value.Location] = downloadStatus;
			}
		}

		// Token: 0x040009BA RID: 2490
		private LinkedList<ContentDownloadService.DownloadOperation> activeDownloads;

		// Token: 0x040009BB RID: 2491
		private Dictionary<RemoteContentLocation, ContentDownloadService.DownloadStatus> downloadStates;

		// Token: 0x040009BC RID: 2492
		private int maxActive = 5;

		// Token: 0x040009BD RID: 2493
		private Func<ContentDownloadService.DownloadOperation> createOpFunc;

		// Token: 0x040009BE RID: 2494
		private long activeDownloadedBytes;

		// Token: 0x040009BF RID: 2495
		private long completeDownloadedBytes;

		// Token: 0x02000466 RID: 1126
		public enum State
		{
			// Token: 0x040009C5 RID: 2501
			None,
			// Token: 0x040009C6 RID: 2502
			Queued,
			// Token: 0x040009C7 RID: 2503
			Downloading,
			// Token: 0x040009C8 RID: 2504
			Complete,
			// Token: 0x040009C9 RID: 2505
			Cancelled,
			// Token: 0x040009CA RID: 2506
			Failed
		}

		// Token: 0x02000467 RID: 1127
		public struct DownloadStatus
		{
			// Token: 0x040009CB RID: 2507
			public ContentDownloadService.State DownloadState;

			// Token: 0x040009CC RID: 2508
			public long BytesDownloaded;

			// Token: 0x040009CD RID: 2509
			public FixedString512Bytes LocalPath;
		}

		// Token: 0x02000468 RID: 1128
		public abstract class DownloadOperation
		{
			// Token: 0x17000237 RID: 567
			// (get) Token: 0x06001BEA RID: 7146 RVA: 0x00061C29 File Offset: 0x0005FE29
			// (set) Token: 0x06001BEB RID: 7147 RVA: 0x00061C31 File Offset: 0x0005FE31
			public bool IsCancelled { get; private set; }

			// Token: 0x17000238 RID: 568
			// (get) Token: 0x06001BEC RID: 7148 RVA: 0x00061C3A File Offset: 0x0005FE3A
			// (set) Token: 0x06001BED RID: 7149 RVA: 0x00061C42 File Offset: 0x0005FE42
			public RemoteContentLocation Location { get; private set; }

			// Token: 0x17000239 RID: 569
			// (get) Token: 0x06001BEE RID: 7150 RVA: 0x00061C4B File Offset: 0x0005FE4B
			// (set) Token: 0x06001BEF RID: 7151 RVA: 0x00061C53 File Offset: 0x0005FE53
			public bool IsStarted { get; private set; }

			// Token: 0x06001BF0 RID: 7152 RVA: 0x00061C5C File Offset: 0x0005FE5C
			public void Init(RemoteContentLocation loc, string tmpPath, string finalPath)
			{
				this.Location = loc;
				this.tempDownloadPath = tmpPath;
				this.finalDownloadPath = finalPath;
			}

			// Token: 0x06001BF1 RID: 7153 RVA: 0x00061C74 File Offset: 0x0005FE74
			public bool Process(ref ContentDownloadService.DownloadStatus status, ref long downloadedBytes)
			{
				if (this.IsCancelled)
				{
					status.DownloadState = ContentDownloadService.State.Cancelled;
					return true;
				}
				string text = null;
				if (this.ProcessDownload(ref downloadedBytes, ref text))
				{
					if (File.Exists(this.finalDownloadPath))
					{
						File.Delete(this.finalDownloadPath);
					}
					if (string.IsNullOrEmpty(text))
					{
						Directory.CreateDirectory(Path.GetDirectoryName(this.finalDownloadPath));
						File.Move(this.tempDownloadPath, this.finalDownloadPath);
						status.BytesDownloaded = downloadedBytes;
						status.LocalPath = this.finalDownloadPath;
						status.DownloadState = ContentDownloadService.State.Complete;
					}
					else
					{
						if (File.Exists(this.tempDownloadPath))
						{
							File.Delete(this.tempDownloadPath);
						}
						status.DownloadState = ContentDownloadService.State.Failed;
					}
					return true;
				}
				return false;
			}

			// Token: 0x06001BF2 RID: 7154
			protected abstract void StartDownload(string remotePath, string localTmpPath);

			// Token: 0x06001BF3 RID: 7155
			protected abstract bool ProcessDownload(ref long downloadedBytes, ref string error);

			// Token: 0x06001BF4 RID: 7156
			protected abstract void CancelDownload();

			// Token: 0x06001BF5 RID: 7157 RVA: 0x00061D28 File Offset: 0x0005FF28
			public void Start(ref ContentDownloadService.DownloadStatus status)
			{
				if (this.IsCancelled)
				{
					status.DownloadState = ContentDownloadService.State.Cancelled;
					return;
				}
				status.DownloadState = ContentDownloadService.State.Downloading;
				this.StartDownload(this.Location.Path.ToString(), this.tempDownloadPath);
				this.IsStarted = true;
			}

			// Token: 0x06001BF6 RID: 7158 RVA: 0x00061D78 File Offset: 0x0005FF78
			public void Cancel()
			{
				this.IsCancelled = true;
				this.CancelDownload();
			}

			// Token: 0x040009CE RID: 2510
			private string tempDownloadPath;

			// Token: 0x040009CF RID: 2511
			private string finalDownloadPath;
		}
	}
}
