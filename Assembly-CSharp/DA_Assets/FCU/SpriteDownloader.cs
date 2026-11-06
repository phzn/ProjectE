using System;
using System.Collections.Concurrent;
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
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

namespace DA_Assets.FCU
{
	// Token: 0x02000339 RID: 825
	[Serializable]
	public class SpriteDownloader : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x0600161E RID: 5662 RVA: 0x00090C3C File Offset: 0x0008EE3C
		public async Task CreateImageChunks(List<FObject> fobjects)
		{
			await Task.Run(delegate
			{
				List<FObject> list = (from x in fobjects
					where x.Data.NeedDownload
					group x by x.Data.Hash into g
					select g.First<FObject>()).ToList<FObject>();
				this.idFormatChunks = this.GetIdFormatChunks(list);
			});
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x00090C88 File Offset: 0x0008EE88
		public async Task DownloadSprites(List<FObject> fobjects)
		{
			SpriteDownloader.<>c__DisplayClass8_0 CS$<>8__locals1 = new SpriteDownloader.<>c__DisplayClass8_0();
			CS$<>8__locals1.<>4__this = this;
			DALogger.Log("Download Sprites");
			List<FObject> list = (from x in fobjects
				where x.Data.NeedDownload
				group x by x.Data.Hash into g
				select g.First<FObject>()).ToList<FObject>();
			if (list.IsEmpty<FObject>())
			{
				DALogger.Log("DownloadSprites no need");
			}
			else
			{
				CS$<>8__locals1.totalCount = list.Count;
				CS$<>8__locals1.downloadedCount = 0;
				CS$<>8__locals1.lastLoggedCount = -1;
				CS$<>8__locals1.downloadLogTokenSource = new CancellationTokenSource();
				Dictionary<SpriteDownloader.ImageFormatScaleKey, List<List<SpriteDownloader.SpriteData>>> dictionary = await this.GetSpriteLinks(list);
				CS$<>8__locals1.semaphore = new SemaphoreSlim(this._maxConcurrentDownloads);
				List<Task> tasks = new List<Task>();
				CS$<>8__locals1.failedObjects = new ConcurrentBag<FObject>();
				Task.Run(delegate
				{
					SpriteDownloader.<>c__DisplayClass8_0.<<DownloadSprites>b__3>d <<DownloadSprites>b__3>d;
					<<DownloadSprites>b__3>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<DownloadSprites>b__3>d.<>4__this = CS$<>8__locals1;
					<<DownloadSprites>b__3>d.<>1__state = -1;
					<<DownloadSprites>b__3>d.<>t__builder.Start<SpriteDownloader.<>c__DisplayClass8_0.<<DownloadSprites>b__3>d>(ref <<DownloadSprites>b__3>d);
					return <<DownloadSprites>b__3>d.<>t__builder.Task;
				}, CS$<>8__locals1.downloadLogTokenSource.Token);
				DALogger.Log(FcuLocKey.log_start_download_images.Localize(Array.Empty<object>()));
				foreach (KeyValuePair<SpriteDownloader.ImageFormatScaleKey, List<List<SpriteDownloader.SpriteData>>> keyValuePair in dictionary)
				{
					foreach (List<SpriteDownloader.SpriteData> list2 in keyValuePair.Value)
					{
						using (List<SpriteDownloader.SpriteData>.Enumerator enumerator3 = list2.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								SpriteDownloader.<>c__DisplayClass8_1 CS$<>8__locals2 = new SpriteDownloader.<>c__DisplayClass8_1();
								CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
								CS$<>8__locals2.idFormatLink = enumerator3.Current;
								await CS$<>8__locals2.CS$<>8__locals1.semaphore.WaitAsync();
								tasks.Add(Task.Run(delegate
								{
									SpriteDownloader.<>c__DisplayClass8_1.<<DownloadSprites>b__4>d <<DownloadSprites>b__4>d;
									<<DownloadSprites>b__4>d.<>t__builder = AsyncTaskMethodBuilder.Create();
									<<DownloadSprites>b__4>d.<>4__this = CS$<>8__locals2;
									<<DownloadSprites>b__4>d.<>1__state = -1;
									<<DownloadSprites>b__4>d.<>t__builder.Start<SpriteDownloader.<>c__DisplayClass8_1.<<DownloadSprites>b__4>d>(ref <<DownloadSprites>b__4>d);
									return <<DownloadSprites>b__4>d.<>t__builder.Task;
								}));
								CS$<>8__locals2 = null;
							}
						}
						List<SpriteDownloader.SpriteData>.Enumerator enumerator3 = default(List<SpriteDownloader.SpriteData>.Enumerator);
					}
					List<List<SpriteDownloader.SpriteData>>.Enumerator enumerator2 = default(List<List<SpriteDownloader.SpriteData>>.Enumerator);
				}
				Dictionary<SpriteDownloader.ImageFormatScaleKey, List<List<SpriteDownloader.SpriteData>>>.Enumerator enumerator = default(Dictionary<SpriteDownloader.ImageFormatScaleKey, List<List<SpriteDownloader.SpriteData>>>.Enumerator);
				await Task.WhenAll(tasks);
				CS$<>8__locals1.downloadLogTokenSource.Cancel();
				DALogger.Log(FcuLocKey.log_downloading_images.Localize(new object[] { CS$<>8__locals1.downloadedCount, CS$<>8__locals1.totalCount }));
				this.LogFailedDownloads(CS$<>8__locals1.failedObjects);
			}
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x00090CD4 File Offset: 0x0008EED4
		public async Task<bool> DownloadSprite(SpriteDownloader.SpriteData idFormatLink, int maxDownloadAttempts)
		{
			bool flag;
			try
			{
				if (idFormatLink.Link.IsEmpty())
				{
					flag = false;
				}
				else
				{
					DARequest request = new DARequest
					{
						RequestType = RequestType.GetFile,
						Query = idFormatLink.Link
					};
					DAResult<byte[]> daresult = default(DAResult<byte[]>);
					int attempts = 0;
					using (new CancellationTokenSource(TimeSpan.FromSeconds(30.0)))
					{
						while (attempts < maxDownloadAttempts && daresult.Object == null)
						{
							int num = attempts;
							attempts = num + 1;
							daresult = await this.monoBeh.RequestSender.SendRequest<byte[]>(request);
						}
					}
					CancellationTokenSource cts = null;
					if (daresult.Error.Status == 909)
					{
						DALogger.LogError(FcuLocKey.log_ssl_error.Localize(new object[]
						{
							daresult.Error.Message,
							daresult.Error.Status
						}));
						UnityEvent<FigmaConverterUnity> onImportFail = this.monoBeh.Events.OnImportFail;
						if (onImportFail != null)
						{
							onImportFail.Invoke(this.monoBeh);
						}
						this.monoBeh.AssetTools.StopImport(StopImportReason.Error);
					}
					if (daresult.Object == null)
					{
						throw new NullReferenceException();
					}
					File.WriteAllBytes(idFormatLink.FObject.Data.SpritePath, daresult.Object);
					flag = true;
				}
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x00090D28 File Offset: 0x0008EF28
		public async Task<Dictionary<SpriteDownloader.ImageFormatScaleKey, List<List<SpriteDownloader.SpriteData>>>> GetSpriteLinks(List<FObject> fobjects)
		{
			SpriteDownloader.<>c__DisplayClass10_0 CS$<>8__locals1 = new SpriteDownloader.<>c__DisplayClass10_0();
			CS$<>8__locals1.<>4__this = this;
			Dictionary<SpriteDownloader.ImageFormatScaleKey, List<List<SpriteDownloader.SpriteData>>> idFormatLinkChunks = new Dictionary<SpriteDownloader.ImageFormatScaleKey, List<List<SpriteDownloader.SpriteData>>>();
			CS$<>8__locals1.totalLinks = fobjects.Count;
			CS$<>8__locals1.obtainedLinks = 0;
			CS$<>8__locals1.lastLoggedLinks = -1;
			CS$<>8__locals1.linkLogTokenSource = new CancellationTokenSource();
			Task.Run(delegate
			{
				SpriteDownloader.<>c__DisplayClass10_0.<<GetSpriteLinks>b__0>d <<GetSpriteLinks>b__0>d;
				<<GetSpriteLinks>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<GetSpriteLinks>b__0>d.<>4__this = CS$<>8__locals1;
				<<GetSpriteLinks>b__0>d.<>1__state = -1;
				<<GetSpriteLinks>b__0>d.<>t__builder.Start<SpriteDownloader.<>c__DisplayClass10_0.<<GetSpriteLinks>b__0>d>(ref <<GetSpriteLinks>b__0>d);
				return <<GetSpriteLinks>b__0>d.<>t__builder.Task;
			}, CS$<>8__locals1.linkLogTokenSource.Token);
			foreach (KeyValuePair<SpriteDownloader.ImageFormatScaleKey, List<List<SpriteDownloader.SpriteData>>> idFormatChunk in this.idFormatChunks)
			{
				if (this.monoBeh.IsCancellationRequested(TokenType.Import))
				{
					break;
				}
				foreach (List<SpriteDownloader.SpriteData> chunk in idFormatChunk.Value)
				{
					IEnumerable<string> enumerable = chunk.Select<SpriteDownloader.SpriteData, string>((SpriteDownloader.SpriteData x) => x.FObject.Id);
					DARequest darequest = RequestCreator.CreateImageLinksRequest(this.monoBeh.Settings.MainSettings.ProjectUrl, idFormatChunk.Key.ImageFormat.ToLower(), idFormatChunk.Key.Scale, enumerable, this.monoBeh.RequestSender.GetRequestHeader(this.monoBeh.Authorizer.Token, null));
					DAResult<SpriteDownloader.FigmaImageRequest> daresult = await this.monoBeh.RequestSender.SendRequest<SpriteDownloader.FigmaImageRequest>(darequest);
					if (daresult.Success && daresult.Object.images.IsEmpty<KeyValuePair<string, string>>())
					{
						Debug.LogError("result.Success && result.Object.images.IsEmpty()");
					}
					else if (daresult.Success)
					{
						if (!idFormatLinkChunks.ContainsKey(idFormatChunk.Key))
						{
							idFormatLinkChunks[idFormatChunk.Key] = new List<List<SpriteDownloader.SpriteData>>();
						}
						List<SpriteDownloader.SpriteData> list = new List<SpriteDownloader.SpriteData>();
						foreach (SpriteDownloader.SpriteData spriteData in chunk)
						{
							string text;
							daresult.Object.images.TryGetValue(spriteData.FObject.Id, out text);
							if (!this.monoBeh.Settings.MainSettings.Https)
							{
								text = text.Replace("https://", "http://");
							}
							list.Add(new SpriteDownloader.SpriteData
							{
								FObject = spriteData.FObject,
								Format = spriteData.Format,
								Link = (text ?? string.Empty)
							});
							Interlocked.Increment(ref CS$<>8__locals1.obtainedLinks);
						}
						idFormatLinkChunks[idFormatChunk.Key].Add(list);
					}
					else
					{
						Debug.LogError(daresult.Error.Message);
					}
					chunk = null;
				}
				List<List<SpriteDownloader.SpriteData>>.Enumerator enumerator2 = default(List<List<SpriteDownloader.SpriteData>>.Enumerator);
				idFormatChunk = default(KeyValuePair<SpriteDownloader.ImageFormatScaleKey, List<List<SpriteDownloader.SpriteData>>>);
			}
			Dictionary<SpriteDownloader.ImageFormatScaleKey, List<List<SpriteDownloader.SpriteData>>>.Enumerator enumerator = default(Dictionary<SpriteDownloader.ImageFormatScaleKey, List<List<SpriteDownloader.SpriteData>>>.Enumerator);
			CS$<>8__locals1.linkLogTokenSource.Cancel();
			DALogger.Log(FcuLocKey.log_getting_links.Localize(new object[] { CS$<>8__locals1.obtainedLinks, CS$<>8__locals1.totalLinks }));
			return idFormatLinkChunks;
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x00090D74 File Offset: 0x0008EF74
		public async Task SetScalesAndMaxSpriteSizes(List<FObject> fobjects)
		{
			Action<FObject> <>9__2;
			await Task.Run(delegate
			{
				IEnumerable<FObject> enumerable = fobjects.Where<FObject>((FObject x) => x.IsSprite());
				Action<FObject> action;
				if ((action = <>9__2) == null)
				{
					action = (<>9__2 = delegate(FObject fobject)
					{
						fobject.Data.MaxSpriteSize = this.GetMaxSpriteSize(fobject);
						fobject.Data.Scale = this.GetMaxAllowedScale(fobject.Data.MaxSpriteSize, this.monoBeh.Settings.ImageSpritesSettings.MaxSpriteSize, 0.25f, 4f);
					});
				}
				Parallel.ForEach<FObject>(enumerable, action);
			});
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x00090DC0 File Offset: 0x0008EFC0
		public Dictionary<SpriteDownloader.ImageFormatScaleKey, List<List<SpriteDownloader.SpriteData>>> GetIdFormatChunks(List<FObject> fobjects)
		{
			Dictionary<SpriteDownloader.ImageFormatScaleKey, List<List<SpriteDownloader.SpriteData>>> dictionary = new Dictionary<SpriteDownloader.ImageFormatScaleKey, List<List<SpriteDownloader.SpriteData>>>();
			Dictionary<SpriteDownloader.ImageFormatScaleKey, List<FObject>> dictionary2 = new Dictionary<SpriteDownloader.ImageFormatScaleKey, List<FObject>>();
			foreach (FObject fobject in fobjects.Where<FObject>((FObject x) => x.IsSprite()))
			{
				SpriteDownloader.ImageFormatScaleKey imageFormatScaleKey = new SpriteDownloader.ImageFormatScaleKey
				{
					ImageFormat = fobject.Data.ImageFormat,
					Scale = fobject.Data.Scale
				};
				if (!dictionary2.ContainsKey(imageFormatScaleKey))
				{
					dictionary2[imageFormatScaleKey] = new List<FObject>();
				}
				dictionary2[imageFormatScaleKey].Add(fobject);
			}
			foreach (KeyValuePair<SpriteDownloader.ImageFormatScaleKey, List<FObject>> keyValuePair in dictionary2)
			{
				SpriteDownloader.ImageFormatScaleKey key = keyValuePair.Key;
				List<FObject> value = keyValuePair.Value;
				List<List<SpriteDownloader.SpriteData>> list = new List<List<SpriteDownloader.SpriteData>>();
				List<SpriteDownloader.SpriteData> list2 = new List<SpriteDownloader.SpriteData>();
				float num = 0f;
				foreach (FObject fobject2 in value)
				{
					float num2 = fobject2.Data.MaxSpriteSize.x * fobject2.Data.MaxSpriteSize.y * fobject2.Data.Scale;
					if (num + num2 > this._maxChunkSize || list2.Count > this._maxSpritesCount)
					{
						if (list2.Count > 0)
						{
							list.Add(list2);
						}
						list2 = new List<SpriteDownloader.SpriteData>();
						num = 0f;
					}
					list2.Add(new SpriteDownloader.SpriteData
					{
						FObject = fobject2,
						Format = key.ImageFormat.ToString(),
						Scale = key.Scale
					});
					num += num2;
				}
				if (list2.Count > 0)
				{
					list.Add(list2);
				}
				dictionary[key] = list;
			}
			return dictionary;
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x00091028 File Offset: 0x0008F228
		private void LogFailedDownloads(ConcurrentBag<FObject> failedObjects)
		{
			if (failedObjects.Count<FObject>() > 0)
			{
				foreach (List<string> list in failedObjects.Select<FObject, string>((FObject x) => x.Data.NameHierarchy).Split<string>(this._errorLogSplitLimit))
				{
					string text = string.Join("\n", list);
					DALogger.LogError(FcuLocKey.log_malformed_url.Localize(new object[] { list.Count, text }));
				}
			}
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x000910DC File Offset: 0x0008F2DC
		private Vector2 GetMaxSpriteSize(FObject fobject)
		{
			Vector2 vector;
			bool boundingSize = fobject.GetBoundingSize(out vector);
			Vector2 vector2;
			bool renderSize = fobject.GetRenderSize(out vector2);
			float num;
			float num2;
			if (boundingSize && renderSize)
			{
				num = Mathf.Max(vector.x, vector2.x);
				num2 = Mathf.Max(vector.y, vector2.y);
			}
			else if (renderSize)
			{
				num = vector2.x;
				num2 = vector2.y;
			}
			else if (boundingSize)
			{
				num = vector.x;
				num2 = vector.y;
			}
			else
			{
				num = fobject.Size.x;
				num2 = fobject.Size.y;
			}
			return new Vector2(num, num2);
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x00091174 File Offset: 0x0008F374
		public float GetMaxAllowedScale(Vector2 imageSize, Vector2 maxSpriteSize, float minScale = 0.25f, float maxScale = 4f)
		{
			if (this.monoBeh.UsingSVG())
			{
				return this.monoBeh.Settings.ImageSpritesSettings.ImageScale;
			}
			float num = Mathf.Min(maxSpriteSize.x, (float)this.monoBeh.Settings.ImageSpritesSettings.MaxSpriteSize.x);
			float num2 = Mathf.Min(maxSpriteSize.y, (float)this.monoBeh.Settings.ImageSpritesSettings.MaxSpriteSize.y);
			float num3 = num / imageSize.x;
			float num4 = num2 / imageSize.y;
			float num5 = Mathf.Min(num3, num4);
			num5 = Mathf.Max(1f, num5);
			return Mathf.Min((float)Math.Round((double)Mathf.Clamp(num5, minScale, maxScale), FcuConfig.Rounding.GetMaxAllowedScaleDigits), this.monoBeh.Settings.ImageSpritesSettings.ImageScale);
		}

		// Token: 0x040017B2 RID: 6066
		private int _maxConcurrentDownloads = 100;

		// Token: 0x040017B3 RID: 6067
		private int _maxDownloadAttempts = 3;

		// Token: 0x040017B4 RID: 6068
		private float _maxChunkSize = 24000000f;

		// Token: 0x040017B5 RID: 6069
		private int _errorLogSplitLimit = 50;

		// Token: 0x040017B6 RID: 6070
		private int _logDelayMs = 1000;

		// Token: 0x040017B7 RID: 6071
		private int _maxSpritesCount = 100;

		// Token: 0x040017B8 RID: 6072
		private Dictionary<SpriteDownloader.ImageFormatScaleKey, List<List<SpriteDownloader.SpriteData>>> idFormatChunks;

		// Token: 0x0200033A RID: 826
		public struct FigmaImageRequest
		{
			// Token: 0x040017B9 RID: 6073
			[JsonProperty("err")]
			public string error;

			// Token: 0x040017BA RID: 6074
			[JsonProperty("images")]
			public Dictionary<string, string> images;
		}

		// Token: 0x0200033B RID: 827
		public struct ImageFormatScaleKey
		{
			// Token: 0x1700042F RID: 1071
			// (get) Token: 0x06001628 RID: 5672 RVA: 0x0009128F File Offset: 0x0008F48F
			// (set) Token: 0x06001629 RID: 5673 RVA: 0x00091297 File Offset: 0x0008F497
			public ImageFormat ImageFormat { readonly get; set; }

			// Token: 0x17000430 RID: 1072
			// (get) Token: 0x0600162A RID: 5674 RVA: 0x000912A0 File Offset: 0x0008F4A0
			// (set) Token: 0x0600162B RID: 5675 RVA: 0x000912A8 File Offset: 0x0008F4A8
			public float Scale { readonly get; set; }

			// Token: 0x0600162C RID: 5676 RVA: 0x000912B4 File Offset: 0x0008F4B4
			public override bool Equals(object obj)
			{
				if (!(obj is SpriteDownloader.ImageFormatScaleKey))
				{
					return false;
				}
				SpriteDownloader.ImageFormatScaleKey imageFormatScaleKey = (SpriteDownloader.ImageFormatScaleKey)obj;
				return this.ImageFormat == imageFormatScaleKey.ImageFormat && this.Scale.Equals(imageFormatScaleKey.Scale);
			}

			// Token: 0x0600162D RID: 5677 RVA: 0x000912F8 File Offset: 0x0008F4F8
			public override int GetHashCode()
			{
				return (17 * 23 + this.ImageFormat.GetHashCode()) * 23 + this.Scale.GetHashCode();
			}
		}

		// Token: 0x0200033C RID: 828
		public struct SpriteData
		{
			// Token: 0x17000431 RID: 1073
			// (get) Token: 0x0600162E RID: 5678 RVA: 0x00091331 File Offset: 0x0008F531
			// (set) Token: 0x0600162F RID: 5679 RVA: 0x00091339 File Offset: 0x0008F539
			public FObject FObject { readonly get; set; }

			// Token: 0x17000432 RID: 1074
			// (get) Token: 0x06001630 RID: 5680 RVA: 0x00091342 File Offset: 0x0008F542
			// (set) Token: 0x06001631 RID: 5681 RVA: 0x0009134A File Offset: 0x0008F54A
			public string Format { readonly get; set; }

			// Token: 0x17000433 RID: 1075
			// (get) Token: 0x06001632 RID: 5682 RVA: 0x00091353 File Offset: 0x0008F553
			// (set) Token: 0x06001633 RID: 5683 RVA: 0x0009135B File Offset: 0x0008F55B
			public string Link { readonly get; set; }

			// Token: 0x17000434 RID: 1076
			// (get) Token: 0x06001634 RID: 5684 RVA: 0x00091364 File Offset: 0x0008F564
			// (set) Token: 0x06001635 RID: 5685 RVA: 0x0009136C File Offset: 0x0008F56C
			public float Scale { readonly get; set; }
		}
	}
}
