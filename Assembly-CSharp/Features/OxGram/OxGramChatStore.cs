using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Features.OxGram
{
	// Token: 0x02000E8F RID: 3727
	public sealed class OxGramChatStore : IDisposable
	{
		// Token: 0x06007011 RID: 28689 RVA: 0x002D53DC File Offset: 0x002D35DC
		public OxGramChatStore(string directory = null)
		{
			this._directory = directory ?? Path.Combine(Application.persistentDataPath, "CielGram");
		}

		// Token: 0x17001253 RID: 4691
		// (get) Token: 0x06007012 RID: 28690 RVA: 0x002D5409 File Offset: 0x002D3609
		private string HistoryPath
		{
			get
			{
				return Path.Combine(this._directory, "chat-history.json");
			}
		}

		// Token: 0x17001254 RID: 4692
		// (get) Token: 0x06007013 RID: 28691 RVA: 0x002D541B File Offset: 0x002D361B
		private string PhotosPath
		{
			get
			{
				return Path.Combine(this._directory, "Photos");
			}
		}

		// Token: 0x06007014 RID: 28692 RVA: 0x002D5430 File Offset: 0x002D3630
		public List<OxGramChat> Load()
		{
			List<OxGramChat> list = OxGramSeed.BuildChats();
			OxGramChatStore.SavedHistory savedHistory = OxGramChatStore.ReadHistory(this.HistoryPath) ?? OxGramChatStore.ReadHistory(this.HistoryPath + ".bak");
			if (savedHistory == null)
			{
				return list;
			}
			List<OxGramChat> list2 = new List<OxGramChat>();
			HashSet<string> hashSet = new HashSet<string>();
			using (List<OxGramChatStore.SavedChat>.Enumerator enumerator = savedHistory.chats.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					OxGramChatStore.SavedChat entry = enumerator.Current;
					if (entry != null && !string.IsNullOrEmpty(entry.id) && hashSet.Add(entry.id))
					{
						OxGramChat oxGramChat = list.Find((OxGramChat candidate) => candidate.id == entry.id);
						if (oxGramChat == null)
						{
							OxGramContacts.Contact contact = OxGramContacts.Find(entry.contactId);
							if (contact == null || entry.id != "dm:" + contact.id)
							{
								continue;
							}
							oxGramChat = OxGramContacts.Create(contact);
							string text;
							OxGramContacts.TryRename(oxGramChat, entry.title, out text);
						}
						if (entry.messages != null)
						{
							oxGramChat.messages.Clear();
							foreach (OxGramChatStore.SavedMessage savedMessage in entry.messages)
							{
								if (savedMessage != null)
								{
									oxGramChat.messages.Add(new OxGramMessage
									{
										sender = savedMessage.sender,
										kind = savedMessage.kind,
										authorName = ((!oxGramChat.isGroup && savedMessage.sender == OxGramSender.Other) ? oxGramChat.title : savedMessage.authorName),
										text = savedMessage.text,
										time = savedMessage.time,
										photoFileName = OxGramChatStore.SafePhotoFileName(savedMessage.photoFileName),
										photo = this.LoadPhoto(savedMessage.photoFileName, savedMessage.photoName),
										reactionTemplate = savedMessage.reactionTemplate,
										reactionAlias = savedMessage.reactionAlias
									});
								}
							}
						}
						list2.Add(oxGramChat);
					}
				}
			}
			using (List<OxGramChat>.Enumerator enumerator3 = list.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					OxGramChat group = enumerator3.Current;
					if (!list2.Exists((OxGramChat chat) => chat.id == group.id))
					{
						list2.Add(group);
					}
				}
			}
			return list2;
		}

		// Token: 0x06007015 RID: 28693 RVA: 0x002D5734 File Offset: 0x002D3934
		private static OxGramChatStore.SavedHistory ReadHistory(string path)
		{
			if (!File.Exists(path))
			{
				return null;
			}
			OxGramChatStore.SavedHistory savedHistory2;
			try
			{
				OxGramChatStore.SavedHistory savedHistory = JsonUtility.FromJson<OxGramChatStore.SavedHistory>(File.ReadAllText(path));
				savedHistory2 = ((savedHistory != null && savedHistory.version == 1 && savedHistory.chats != null) ? savedHistory : null);
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is ArgumentException)
			{
				Debug.LogWarning("[CielGram] Could not read chat history: " + ex.Message);
				savedHistory2 = null;
			}
			return savedHistory2;
		}

		// Token: 0x06007016 RID: 28694 RVA: 0x002D57CC File Offset: 0x002D39CC
		public bool Save(IList<OxGramChat> chats)
		{
			if (chats == null)
			{
				return false;
			}
			bool flag;
			try
			{
				Directory.CreateDirectory(this._directory);
				OxGramChatStore.SavedHistory savedHistory = new OxGramChatStore.SavedHistory();
				foreach (OxGramChat oxGramChat in chats)
				{
					if (oxGramChat != null && !string.IsNullOrEmpty(oxGramChat.id))
					{
						OxGramChatStore.SavedChat savedChat = new OxGramChatStore.SavedChat
						{
							id = oxGramChat.id,
							contactId = oxGramChat.contactId,
							title = oxGramChat.title
						};
						foreach (OxGramMessage oxGramMessage in oxGramChat.messages)
						{
							if (oxGramMessage != null)
							{
								this.ArchivePhoto(oxGramMessage);
								savedChat.messages.Add(new OxGramChatStore.SavedMessage
								{
									sender = oxGramMessage.sender,
									kind = oxGramMessage.kind,
									authorName = oxGramMessage.authorName,
									text = oxGramMessage.text,
									time = oxGramMessage.time,
									photoFileName = oxGramMessage.photoFileName,
									photoName = ((oxGramMessage.photo != null) ? oxGramMessage.photo.name : null),
									reactionTemplate = oxGramMessage.reactionTemplate,
									reactionAlias = oxGramMessage.reactionAlias
								});
							}
						}
						savedHistory.chats.Add(savedChat);
					}
				}
				string text = this.HistoryPath + ".tmp";
				File.WriteAllText(text, JsonUtility.ToJson(savedHistory));
				if (File.Exists(this.HistoryPath))
				{
					File.Replace(text, this.HistoryPath, this.HistoryPath + ".bak");
				}
				else
				{
					File.Move(text, this.HistoryPath);
				}
				flag = true;
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is ArgumentException || ex is UnityException)
			{
				Debug.LogWarning("[CielGram] Could not save chat history: " + ex.Message);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06007017 RID: 28695 RVA: 0x002D5A4C File Offset: 0x002D3C4C
		private void ArchivePhoto(OxGramMessage message)
		{
			if (message.kind != OxGramMessageKind.Photo || message.photo == null)
			{
				return;
			}
			string text = OxGramChatStore.SafePhotoFileName(message.photoFileName);
			if (text != null && File.Exists(Path.Combine(this.PhotosPath, text)))
			{
				return;
			}
			Directory.CreateDirectory(this.PhotosPath);
			Sprite photo = message.photo;
			Rect textureRect = photo.textureRect;
			Texture2D texture2D = new Texture2D(Mathf.RoundToInt(textureRect.width), Mathf.RoundToInt(textureRect.height), TextureFormat.RGBA32, false);
			try
			{
				texture2D.SetPixels(photo.texture.GetPixels(Mathf.RoundToInt(textureRect.x), Mathf.RoundToInt(textureRect.y), texture2D.width, texture2D.height));
				texture2D.Apply();
				string text2 = Guid.NewGuid().ToString("N") + ".png";
				File.WriteAllBytes(Path.Combine(this.PhotosPath, text2), texture2D.EncodeToPNG());
				message.photoFileName = text2;
			}
			finally
			{
				OxGramChatStore.DestroyGenerated(texture2D);
			}
		}

		// Token: 0x06007018 RID: 28696 RVA: 0x002D5B64 File Offset: 0x002D3D64
		private Sprite LoadPhoto(string fileName, string photoName)
		{
			fileName = OxGramChatStore.SafePhotoFileName(fileName);
			if (fileName == null)
			{
				return null;
			}
			Sprite sprite;
			if (this._loadedPhotos.TryGetValue(fileName, out sprite))
			{
				return sprite;
			}
			string text = Path.Combine(this.PhotosPath, fileName);
			if (!File.Exists(text))
			{
				return null;
			}
			Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false);
			try
			{
				if (texture2D.LoadImage(File.ReadAllBytes(text)))
				{
					Sprite sprite2 = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f), 100f);
					sprite2.name = photoName ?? string.Empty;
					this._loadedPhotos[fileName] = sprite2;
					return sprite2;
				}
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is UnityException)
			{
				Debug.LogWarning("[CielGram] Could not load a saved photo: " + ex.Message);
			}
			OxGramChatStore.DestroyGenerated(texture2D);
			return null;
		}

		// Token: 0x06007019 RID: 28697 RVA: 0x002D5C80 File Offset: 0x002D3E80
		private static string SafePhotoFileName(string value)
		{
			if (string.IsNullOrEmpty(value) || value.Length != 36 || !value.EndsWith(".png", StringComparison.Ordinal))
			{
				return null;
			}
			Guid guid;
			if (!Guid.TryParseExact(value.Substring(0, 32), "N", out guid))
			{
				return null;
			}
			return value;
		}

		// Token: 0x0600701A RID: 28698 RVA: 0x002D5CCC File Offset: 0x002D3ECC
		public void Dispose()
		{
			foreach (Sprite sprite in this._loadedPhotos.Values)
			{
				if (!(sprite == null))
				{
					OxGramChatStore.DestroyGenerated(sprite.texture);
					OxGramChatStore.DestroyGenerated(sprite);
				}
			}
			this._loadedPhotos.Clear();
		}

		// Token: 0x0600701B RID: 28699 RVA: 0x00027EE2 File Offset: 0x000260E2
		private static void DestroyGenerated(Object value)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(value);
				return;
			}
			Object.DestroyImmediate(value);
		}

		// Token: 0x040072D1 RID: 29393
		private const int Version = 1;

		// Token: 0x040072D2 RID: 29394
		private readonly string _directory;

		// Token: 0x040072D3 RID: 29395
		private readonly Dictionary<string, Sprite> _loadedPhotos = new Dictionary<string, Sprite>();

		// Token: 0x02000E90 RID: 3728
		[Serializable]
		private sealed class SavedHistory
		{
			// Token: 0x040072D4 RID: 29396
			public int version = 1;

			// Token: 0x040072D5 RID: 29397
			public List<OxGramChatStore.SavedChat> chats = new List<OxGramChatStore.SavedChat>();
		}

		// Token: 0x02000E91 RID: 3729
		[Serializable]
		private sealed class SavedChat
		{
			// Token: 0x040072D6 RID: 29398
			public string id;

			// Token: 0x040072D7 RID: 29399
			public string contactId;

			// Token: 0x040072D8 RID: 29400
			public string title;

			// Token: 0x040072D9 RID: 29401
			public List<OxGramChatStore.SavedMessage> messages = new List<OxGramChatStore.SavedMessage>();
		}

		// Token: 0x02000E92 RID: 3730
		[Serializable]
		private sealed class SavedMessage
		{
			// Token: 0x040072DA RID: 29402
			public OxGramSender sender;

			// Token: 0x040072DB RID: 29403
			public OxGramMessageKind kind;

			// Token: 0x040072DC RID: 29404
			public string authorName;

			// Token: 0x040072DD RID: 29405
			public string text;

			// Token: 0x040072DE RID: 29406
			public string time;

			// Token: 0x040072DF RID: 29407
			public string photoFileName;

			// Token: 0x040072E0 RID: 29408
			public string photoName;

			// Token: 0x040072E1 RID: 29409
			public string reactionTemplate;

			// Token: 0x040072E2 RID: 29410
			public string reactionAlias;
		}
	}
}
