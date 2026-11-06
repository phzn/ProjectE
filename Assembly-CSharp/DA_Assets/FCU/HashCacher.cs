using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x0200031F RID: 799
	[Serializable]
	public class HashCacher : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x060015D6 RID: 5590 RVA: 0x0008DC50 File Offset: 0x0008BE50
		public static string GetCachePath(string fileName)
		{
			return Path.Combine(FcuConfig.CachePath, fileName);
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x0008DC60 File Offset: 0x0008BE60
		public async Task WriteCache(List<FObject> fobjects)
		{
			await Task.Run(delegate
			{
				string text = this.monoBeh.Settings.MainSettings.ProjectUrl.RemoveInvalidCharsFromFileName('-') + ".txt";
				string text2 = Path.Combine(HashCacher.GetCachePath("HashCache"), text);
				HashCacher.FHash fhash = default(HashCacher.FHash);
				fhash.Hashes = fobjects.ConvertAll<HashCacher.FHashObject>((FObject fobject) => new HashCacher.FHashObject
				{
					Id = fobject.Data.Parent.Id + "_" + fobject.Data.Id,
					HashData = fobject.Data.HashData,
					HashDataTree = fobject.Data.HashDataTree
				}).ToArray();
				string text3 = JsonUtility.ToJson(fhash);
				Path.GetDirectoryName(text2).CreateFolderIfNotExists();
				File.WriteAllText(text2, text3);
			}, this.monoBeh.GetToken(TokenType.Import));
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x0008DCAC File Offset: 0x0008BEAC
		public async Task LoadHashCache(SyncHelper[] syncHelpers)
		{
			Action<SyncHelper> <>9__1;
			await Task.Run(delegate
			{
				IEnumerable<SyncHelper> syncHelpers2 = syncHelpers;
				Action<SyncHelper> action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate(SyncHelper syncHelper)
					{
						string text = this.monoBeh.Settings.MainSettings.ProjectUrl.RemoveInvalidCharsFromFileName('-') + ".txt";
						string text2 = Path.Combine(HashCacher.GetCachePath("HashCache"), text);
						if (File.Exists(text2))
						{
							foreach (HashCacher.FHashObject fhashObject in JsonUtility.FromJson<HashCacher.FHash>(File.ReadAllText(text2)).Hashes)
							{
								if (fhashObject.Id == syncHelper.Data.Parent.Id + "_" + syncHelper.Data.Id)
								{
									syncHelper.Data.HashData = fhashObject.HashData;
									syncHelper.Data.HashDataTree = fhashObject.HashDataTree;
									return;
								}
							}
						}
					});
				}
				Parallel.ForEach<SyncHelper>(syncHelpers2, action);
			}, this.monoBeh.GetToken(TokenType.Import));
		}

		// Token: 0x02000320 RID: 800
		[Serializable]
		private struct FHash
		{
			// Token: 0x0400175F RID: 5983
			public HashCacher.FHashObject[] Hashes;
		}

		// Token: 0x02000321 RID: 801
		[Serializable]
		private struct FHashObject
		{
			// Token: 0x04001760 RID: 5984
			public string Id;

			// Token: 0x04001761 RID: 5985
			public string HashData;

			// Token: 0x04001762 RID: 5986
			public string HashDataTree;
		}
	}
}
