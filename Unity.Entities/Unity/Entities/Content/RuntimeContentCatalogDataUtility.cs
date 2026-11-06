using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Collections;
using Unity.Entities.Serialization;
using Unity.Mathematics;

namespace Unity.Entities.Content
{
	// Token: 0x02000486 RID: 1158
	internal static class RuntimeContentCatalogDataUtility
	{
		// Token: 0x06001C56 RID: 7254 RVA: 0x00063384 File Offset: 0x00061584
		public unsafe static void Print(ref RuntimeContentCatalogData data, Action<string> printFunc)
		{
			for (int i = 0; i < data.Archives.Length; i++)
			{
				printFunc(string.Format("Archives[{0}] = ArchiveId:{1}", i, data.Archives[i].ArchiveId));
			}
			for (int j = 0; j < data.Files.Length; j++)
			{
				printFunc(string.Format("Files[{0}] = FileId:{1}, ArchiveIndex:{2}, DependencyIndex:{3}", new object[]
				{
					j,
					data.Files[j].FileId,
					data.Files[j].ArchiveIndex,
					data.Files[j].DependencyIndex
				}));
			}
			for (int k = 0; k < data.Objects.Length; k++)
			{
				printFunc(string.Format("Objects[{0}] = ObjectId:{1}, FileIndex:{2}, LocalIdentifierInFile:{3}", new object[]
				{
					k,
					data.Objects[k].ObjectId,
					data.Objects[k].FileIndex,
					data.Objects[k].LocalIdentifierInFile
				}));
			}
			for (int l = 0; l < data.Scenes.Length; l++)
			{
				printFunc(string.Format("Scenes[{0}] = SceneId:{1}, FileIndex:{2}, SceneName:{3}", new object[]
				{
					l,
					data.Scenes[l].SceneId,
					data.Scenes[l].FileIndex,
					data.Scenes[l].SceneName
				}));
			}
			for (int m = 0; m < data.Dependencies.Length; m++)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int n = 0; n < data.Dependencies[m].Length; n++)
				{
					stringBuilder.Append(string.Format("{0},", *data.Dependencies[m][n]));
				}
				printFunc(string.Format("Dependencies[{0}] = {1}", m, stringBuilder));
			}
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x000635E0 File Offset: 0x000617E0
		private static int GetDependencyIndex(IEnumerable<ContentFileId> files, ref Dictionary<uint2, int> depMap, ref List<List<ContentFileId>> depList)
		{
			xxHash3.StreamingState streamingState = new xxHash3.StreamingState(true, 0UL);
			foreach (ContentFileId contentFileId in files)
			{
				streamingState.Update<ContentFileId>(in contentFileId);
			}
			uint2 @uint = streamingState.DigestHash64();
			int count;
			if (!depMap.TryGetValue(@uint, out count))
			{
				depMap.Add(@uint, count = depList.Count);
				depList.Add(new List<ContentFileId>(files));
			}
			return count;
		}

		// Token: 0x06001C58 RID: 7256 RVA: 0x00063668 File Offset: 0x00061868
		public unsafe static void Create(IRuntimeCatalogDataSource dataSource, BlobBuilder blobBuilder, ref RuntimeContentCatalogData blobDataRoot, Func<UntypedWeakReferenceId, UntypedWeakReferenceId> idRemapFunc)
		{
			IEnumerable<ContentArchiveId> archiveIds = dataSource.GetArchiveIds();
			int num = archiveIds.Count<ContentArchiveId>();
			BlobBuilderArray<ContentArchiveLocation> blobBuilderArray = blobBuilder.Allocate<ContentArchiveLocation>(ref blobDataRoot.Archives, num);
			List<ValueTuple<ContentFileId, int>> list = new List<ValueTuple<ContentFileId, int>>();
			Dictionary<ContentFileId, int> dictionary = new Dictionary<ContentFileId, int>();
			List<ValueTuple<UntypedWeakReferenceId, long, int>> list2 = new List<ValueTuple<UntypedWeakReferenceId, long, int>>();
			List<ValueTuple<UntypedWeakReferenceId, string, int>> list3 = new List<ValueTuple<UntypedWeakReferenceId, string, int>>();
			int num2 = 0;
			foreach (ContentArchiveId contentArchiveId in archiveIds)
			{
				*blobBuilderArray[num2] = new ContentArchiveLocation
				{
					ArchiveId = contentArchiveId
				};
				foreach (ContentFileId contentFileId in dataSource.GetFileIds(contentArchiveId))
				{
					int count = list.Count;
					dictionary.Add(contentFileId, count);
					list.Add(new ValueTuple<ContentFileId, int>(contentFileId, num2));
					foreach (ValueTuple<UntypedWeakReferenceId, long> valueTuple in dataSource.GetObjects(contentFileId))
					{
						list2.Add(new ValueTuple<UntypedWeakReferenceId, long, int>(valueTuple.Item1, valueTuple.Item2, count));
					}
					foreach (ValueTuple<UntypedWeakReferenceId, string> valueTuple2 in dataSource.GetScenes(contentFileId))
					{
						list3.Add(new ValueTuple<UntypedWeakReferenceId, string, int>(valueTuple2.Item1, valueTuple2.Item2, count));
					}
				}
				num2++;
			}
			Dictionary<uint2, int> dictionary2 = new Dictionary<uint2, int>();
			List<List<ContentFileId>> list4 = new List<List<ContentFileId>>();
			BlobBuilderArray<ContentFileLocation> blobBuilderArray2 = blobBuilder.Allocate<ContentFileLocation>(ref blobDataRoot.Files, list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				ValueTuple<ContentFileId, int> valueTuple3 = list[i];
				*blobBuilderArray2[i] = new ContentFileLocation
				{
					FileId = valueTuple3.Item1,
					ArchiveIndex = valueTuple3.Item2,
					DependencyIndex = RuntimeContentCatalogDataUtility.GetDependencyIndex(dataSource.GetDependencies(valueTuple3.Item1), ref dictionary2, ref list4)
				};
			}
			BlobBuilderArray<ContentObjectLocation> blobBuilderArray3 = blobBuilder.Allocate<ContentObjectLocation>(ref blobDataRoot.Objects, list2.Count);
			for (int j = 0; j < list2.Count; j++)
			{
				ValueTuple<UntypedWeakReferenceId, long, int> valueTuple4 = list2[j];
				*blobBuilderArray3[j] = new ContentObjectLocation
				{
					ObjectId = new UnsafeUntypedWeakReferenceId(idRemapFunc(valueTuple4.Item1)),
					LocalIdentifierInFile = valueTuple4.Item2,
					FileIndex = valueTuple4.Item3
				};
			}
			BlobBuilderArray<ContentSceneLocation> blobBuilderArray4 = blobBuilder.Allocate<ContentSceneLocation>(ref blobDataRoot.Scenes, list3.Count);
			for (int k = 0; k < list3.Count; k++)
			{
				ValueTuple<UntypedWeakReferenceId, string, int> valueTuple5 = list3[k];
				*blobBuilderArray4[k] = new ContentSceneLocation
				{
					SceneId = new UnsafeUntypedWeakReferenceId(idRemapFunc(valueTuple5.Item1)),
					SceneName = valueTuple5.Item2,
					FileIndex = valueTuple5.Item3
				};
			}
			BlobBuilderArray<BlobArray<int>> blobBuilderArray5 = blobBuilder.Allocate<BlobArray<int>>(ref blobDataRoot.Dependencies, list4.Count);
			for (int l = 0; l < list4.Count; l++)
			{
				BlobBuilderArray<int> blobBuilderArray6 = blobBuilder.Allocate<int>(blobBuilderArray5[l], list4[l].Count);
				for (int m = 0; m < list4[l].Count; m++)
				{
					*blobBuilderArray6[m] = dictionary[list4[l][m]];
				}
			}
		}
	}
}
