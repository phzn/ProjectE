using System;
using System.Collections.Generic;
using System.IO;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.Score
{
	// Token: 0x02000C71 RID: 3185
	public static class NpcPhotoSessionArchive
	{
		// Token: 0x1400009C RID: 156
		// (add) Token: 0x06005C62 RID: 23650 RVA: 0x00244BFC File Offset: 0x00242DFC
		// (remove) Token: 0x06005C63 RID: 23651 RVA: 0x00244C30 File Offset: 0x00242E30
		public static event Action Changed;

		// Token: 0x17000F1E RID: 3870
		// (get) Token: 0x06005C64 RID: 23652 RVA: 0x00244C63 File Offset: 0x00242E63
		public static int Count
		{
			get
			{
				return NpcPhotoSessionArchive.Entries.Count;
			}
		}

		// Token: 0x17000F1F RID: 3871
		// (get) Token: 0x06005C65 RID: 23653 RVA: 0x00244C6F File Offset: 0x00242E6F
		public static string ExportDirectory
		{
			get
			{
				return PlayerMediaExport.ResolveSourceDirectory(PlayerMediaExport.RootDirectory, "NPC", "Photos");
			}
		}

		// Token: 0x17000F20 RID: 3872
		// (get) Token: 0x06005C66 RID: 23654 RVA: 0x00244C88 File Offset: 0x00242E88
		public static string ExportLocationLabel
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(NpcPhotoSessionArchive.ExportDirectory);
				return string.Concat(new string[]
				{
					directoryInfo.Parent.Parent.Name,
					" / ",
					directoryInfo.Parent.Name,
					" / ",
					directoryInfo.Name
				});
			}
		}

		// Token: 0x06005C67 RID: 23655 RVA: 0x00244CE5 File Offset: 0x00242EE5
		public static NpcPhotoSessionArchive.Entry Get(int index)
		{
			if (index < 0 || index >= NpcPhotoSessionArchive.Entries.Count)
			{
				return null;
			}
			return NpcPhotoSessionArchive.Entries[index];
		}

		// Token: 0x06005C68 RID: 23656 RVA: 0x00244D08 File Offset: 0x00242F08
		public static void Add(RenderTexture source, string sourceName, int captureCount)
		{
			if (source == null || !source.IsCreated())
			{
				return;
			}
			RenderTexture renderTexture = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB)
			{
				name = string.Format("NPC Photo Archive {0}", NpcPhotoSessionArchive._sequence + 1),
				antiAliasing = 1,
				filterMode = FilterMode.Bilinear,
				wrapMode = TextureWrapMode.Clamp,
				useMipMap = false,
				autoGenerateMips = false,
				useDynamicScale = false
			};
			try
			{
				if (!renderTexture.Create())
				{
					NpcPhotoSessionArchive.DestroyTexture(renderTexture);
				}
				else
				{
					Graphics.Blit(source, renderTexture);
					NpcPhotoSessionArchive.Entries.Add(new NpcPhotoSessionArchive.Entry
					{
						RuntimeTexture = renderTexture,
						Sequence = ++NpcPhotoSessionArchive._sequence,
						SourceName = (string.IsNullOrWhiteSpace(sourceName) ? "NPC" : sourceName),
						CaptureCount = Mathf.Max(1, captureCount),
						CapturedAt = DateTime.Now
					});
					Action changed = NpcPhotoSessionArchive.Changed;
					if (changed != null)
					{
						changed();
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[NpcPhotoSessionArchive] Copia dello scatto fallita: " + ex.Message);
				NpcPhotoSessionArchive.DestroyTexture(renderTexture);
			}
		}

		// Token: 0x06005C69 RID: 23657 RVA: 0x00244E34 File Offset: 0x00243034
		public static bool SaveToPc(int index, out string savedPath, out string error)
		{
			savedPath = null;
			error = null;
			NpcPhotoSessionArchive.Entry entry = NpcPhotoSessionArchive.Get(index);
			if (entry == null || entry.RuntimeTexture == null || !entry.RuntimeTexture.IsCreated())
			{
				error = "PHOTO NOT AVAILABLE";
				return false;
			}
			if (entry.IsSaved && File.Exists(entry.SavedPath))
			{
				savedPath = entry.SavedPath;
				return true;
			}
			RenderTexture active = RenderTexture.active;
			Texture2D texture2D = null;
			bool flag;
			try
			{
				texture2D = new Texture2D(entry.RuntimeTexture.width, entry.RuntimeTexture.height, TextureFormat.RGB24, false, false);
				RenderTexture.active = entry.RuntimeTexture;
				texture2D.ReadPixels(new Rect(0f, 0f, (float)entry.RuntimeTexture.width, (float)entry.RuntimeTexture.height), 0, 0, false);
				texture2D.Apply(false, false);
				string text = entry.CapturedAt.ToString("yyyyMMdd_HHmmss_fff");
				string text2 = string.Format("NPC_PHOTO_{0}_{1:000}.png", text, entry.Sequence);
				string text3 = PlayerMediaExport.WriteNpcPhoto(texture2D.EncodeToPNG(), text2);
				entry.SavedPath = text3;
				savedPath = text3;
				Action changed = NpcPhotoSessionArchive.Changed;
				if (changed != null)
				{
					changed();
				}
				Debug.Log("[NpcPhotoSessionArchive] Foto salvata sul PC: " + text3);
				flag = true;
			}
			catch (Exception ex)
			{
				error = ex.Message;
				Debug.LogError("[NpcPhotoSessionArchive] Save to PC fallito: " + ex.Message);
				flag = false;
			}
			finally
			{
				RenderTexture.active = active;
				if (texture2D != null)
				{
					Object.Destroy(texture2D);
				}
			}
			return flag;
		}

		// Token: 0x06005C6A RID: 23658 RVA: 0x00244FC8 File Offset: 0x002431C8
		public static void Clear()
		{
			for (int i = 0; i < NpcPhotoSessionArchive.Entries.Count; i++)
			{
				NpcPhotoSessionArchive.Entry entry = NpcPhotoSessionArchive.Entries[i];
				if (entry != null)
				{
					NpcPhotoSessionArchive.DestroyTexture(entry.RuntimeTexture);
				}
			}
			NpcPhotoSessionArchive.Entries.Clear();
			NpcPhotoSessionArchive._sequence = 0;
			Action changed = NpcPhotoSessionArchive.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x06005C6B RID: 23659 RVA: 0x00245023 File Offset: 0x00243223
		private static void DestroyTexture(RenderTexture texture)
		{
			if (texture == null)
			{
				return;
			}
			if (texture.IsCreated())
			{
				texture.Release();
			}
			if (Application.isPlaying)
			{
				Object.Destroy(texture);
				return;
			}
			Object.DestroyImmediate(texture);
		}

		// Token: 0x04005DA8 RID: 23976
		private static readonly List<NpcPhotoSessionArchive.Entry> Entries = new List<NpcPhotoSessionArchive.Entry>();

		// Token: 0x04005DA9 RID: 23977
		private static int _sequence;

		// Token: 0x02000C72 RID: 3186
		public sealed class Entry
		{
			// Token: 0x17000F21 RID: 3873
			// (get) Token: 0x06005C6D RID: 23661 RVA: 0x0024505D File Offset: 0x0024325D
			public Texture Texture
			{
				get
				{
					return this.RuntimeTexture;
				}
			}

			// Token: 0x17000F22 RID: 3874
			// (get) Token: 0x06005C6E RID: 23662 RVA: 0x00245065 File Offset: 0x00243265
			// (set) Token: 0x06005C6F RID: 23663 RVA: 0x0024506D File Offset: 0x0024326D
			public string SourceName { get; internal set; }

			// Token: 0x17000F23 RID: 3875
			// (get) Token: 0x06005C70 RID: 23664 RVA: 0x00245076 File Offset: 0x00243276
			// (set) Token: 0x06005C71 RID: 23665 RVA: 0x0024507E File Offset: 0x0024327E
			public int CaptureCount { get; internal set; }

			// Token: 0x17000F24 RID: 3876
			// (get) Token: 0x06005C72 RID: 23666 RVA: 0x00245087 File Offset: 0x00243287
			// (set) Token: 0x06005C73 RID: 23667 RVA: 0x0024508F File Offset: 0x0024328F
			public DateTime CapturedAt { get; internal set; }

			// Token: 0x17000F25 RID: 3877
			// (get) Token: 0x06005C74 RID: 23668 RVA: 0x00245098 File Offset: 0x00243298
			// (set) Token: 0x06005C75 RID: 23669 RVA: 0x002450A0 File Offset: 0x002432A0
			public string SavedPath { get; internal set; }

			// Token: 0x17000F26 RID: 3878
			// (get) Token: 0x06005C76 RID: 23670 RVA: 0x002450A9 File Offset: 0x002432A9
			public bool IsSaved
			{
				get
				{
					return !string.IsNullOrEmpty(this.SavedPath);
				}
			}

			// Token: 0x04005DAB RID: 23979
			internal RenderTexture RuntimeTexture;

			// Token: 0x04005DAC RID: 23980
			internal int Sequence;
		}
	}
}
