using System;
using System.Text;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities.Serialization;

namespace Unity.Entities.Content
{
	// Token: 0x02000477 RID: 1143
	internal struct RuntimeContentCatalog : IDisposable
	{
		// Token: 0x06001C29 RID: 7209 RVA: 0x00062600 File Offset: 0x00060800
		public void Initialize(int objectCapacity = 0, int fileCapacity = 0, int archiveCapacity = 0, int dependencyCapacity = 0, int sceneCapacity = 0)
		{
			this.ManagedStrings = new ManagedStringTable(objectCapacity + fileCapacity + archiveCapacity + sceneCapacity);
			this.SceneLocations = new UnsafeHashMap<UntypedWeakReferenceId, RuntimeContentCatalog.SceneLocation>(sceneCapacity, Allocator.Persistent);
			this.ObjectLocations = new UnsafeHashMap<UntypedWeakReferenceId, RuntimeContentCatalog.ObjectLocation>(objectCapacity, Allocator.Persistent);
			this.FileLocations = new UnsafeHashMap<ContentFileId, RuntimeContentCatalog.FileLocation>(fileCapacity, Allocator.Persistent);
			this.ArchiveLocations = new UnsafeHashMap<ContentArchiveId, RuntimeContentCatalog.ArchiveLocation>(archiveCapacity, Allocator.Persistent);
			this.FileDependencies = new UnsafeList<UnsafeList<ContentFileId>>(dependencyCapacity, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06001C2A RID: 7210 RVA: 0x0006267D File Offset: 0x0006087D
		public int FileDependencySetCount
		{
			get
			{
				return this.FileDependencies.Length;
			}
		}

		// Token: 0x06001C2B RID: 7211 RVA: 0x0006268A File Offset: 0x0006088A
		public NativeArray<ContentArchiveId> GetArchiveIds(AllocatorManager.AllocatorHandle alloc)
		{
			return this.ArchiveLocations.GetKeyArray(alloc);
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x00062698 File Offset: 0x00060898
		public NativeArray<UntypedWeakReferenceId> GetObjectIds(AllocatorManager.AllocatorHandle alloc)
		{
			return this.ObjectLocations.GetKeyArray(alloc);
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x000626A6 File Offset: 0x000608A6
		public NativeArray<UntypedWeakReferenceId> GetSceneIds(AllocatorManager.AllocatorHandle alloc)
		{
			return this.SceneLocations.GetKeyArray(alloc);
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x000626B4 File Offset: 0x000608B4
		public NativeArray<ContentFileId> GetFileIds(AllocatorManager.AllocatorHandle alloc)
		{
			return this.FileLocations.GetKeyArray(alloc);
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x000626C2 File Offset: 0x000608C2
		public int GetSceneLocationCount()
		{
			return this.SceneLocations.Count;
		}

		// Token: 0x06001C30 RID: 7216 RVA: 0x000626CF File Offset: 0x000608CF
		public int GetObjectLocationCount()
		{
			return this.ObjectLocations.Count;
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x000626DC File Offset: 0x000608DC
		public int GetFileLocationCount()
		{
			return this.FileLocations.Count;
		}

		// Token: 0x06001C32 RID: 7218 RVA: 0x000626E9 File Offset: 0x000608E9
		public int GetArchiveLocationCount()
		{
			return this.ArchiveLocations.Count;
		}

		// Token: 0x06001C33 RID: 7219 RVA: 0x000626F8 File Offset: 0x000608F8
		public bool LoadCatalogData(string catalogPath, Func<string, string> archivePathTransformFunc, Func<string, string> mountedFileNameTransformFunc)
		{
			BlobAssetReference<RuntimeContentCatalogData> blobAssetReference;
			if (!string.IsNullOrEmpty(catalogPath) && BlobAssetReference<RuntimeContentCatalogData>.TryRead(catalogPath, 1, out blobAssetReference))
			{
				this.LoadCatalogData(blobAssetReference.Value, archivePathTransformFunc, mountedFileNameTransformFunc);
				blobAssetReference.Dispose();
				return true;
			}
			return false;
		}

		// Token: 0x06001C34 RID: 7220 RVA: 0x00062734 File Offset: 0x00060934
		public void LoadCatalogData(ref RuntimeContentCatalogData catalogData, Func<string, string> archivePathTransformFunc, Func<string, string> mountedFileNameTransformFunc)
		{
			int length = this.FileDependencies.Length;
			this.AddFileDependencies(ref catalogData, length);
			this.AddArchiveLocations(ref catalogData, archivePathTransformFunc);
			this.AddFileLocations(ref catalogData, length, mountedFileNameTransformFunc);
			this.AddObjectLocations(ref catalogData);
			this.AddSceneLocations(ref catalogData);
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06001C35 RID: 7221 RVA: 0x00062774 File Offset: 0x00060974
		public bool IsCreated
		{
			get
			{
				return this.ObjectLocations.IsCreated;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06001C36 RID: 7222 RVA: 0x00062781 File Offset: 0x00060981
		public bool IsEmpty
		{
			get
			{
				return this.ObjectLocations.IsEmpty && this.SceneLocations.IsEmpty;
			}
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x0006279D File Offset: 0x0006099D
		public string GetStringValue(int index)
		{
			return this.ManagedStrings[index];
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x000627AC File Offset: 0x000609AC
		public void Dispose()
		{
			if (this.ObjectLocations.IsCreated)
			{
				this.ObjectLocations.Dispose();
			}
			if (this.SceneLocations.IsCreated)
			{
				this.SceneLocations.Dispose();
			}
			if (this.FileLocations.IsCreated)
			{
				this.FileLocations.Dispose();
			}
			if (this.ArchiveLocations.IsCreated)
			{
				this.ArchiveLocations.Dispose();
			}
			if (this.FileDependencies.IsCreated)
			{
				for (int i = 0; i < this.FileDependencies.Length; i++)
				{
					this.FileDependencies[i].Dispose();
				}
				this.FileDependencies.Dispose();
			}
			if (this.ManagedStrings.IsCreated)
			{
				this.ManagedStrings.Dispose();
			}
		}

		// Token: 0x06001C39 RID: 7225 RVA: 0x00062874 File Offset: 0x00060A74
		internal unsafe void AddFileDependencies(ref RuntimeContentCatalogData catalogData, int dependencyOffset)
		{
			if (!this.FileDependencies.IsCreated)
			{
				this.FileDependencies = new UnsafeList<UnsafeList<ContentFileId>>(catalogData.Dependencies.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			}
			this.FileDependencies.Resize(this.FileDependencies.Length + catalogData.Dependencies.Length, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < catalogData.Dependencies.Length; i++)
			{
				ref BlobArray<int> ptr = ref catalogData.Dependencies[i];
				int length = catalogData.Dependencies[i].Length;
				UnsafeList<ContentFileId> unsafeList = new UnsafeList<ContentFileId>(length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
				unsafeList.Resize(length, NativeArrayOptions.UninitializedMemory);
				for (int j = 0; j < length; j++)
				{
					unsafeList[j] = catalogData.Files[*ptr[j]].FileId;
				}
				this.FileDependencies[dependencyOffset + i] = unsafeList;
			}
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x00062960 File Offset: 0x00060B60
		internal void AddArchiveLocations(ref RuntimeContentCatalogData catalogData, Func<string, string> pathTransformFunc)
		{
			if (!this.ManagedStrings.IsCreated)
			{
				this.ManagedStrings = new ManagedStringTable(64);
			}
			if (!this.ArchiveLocations.IsCreated)
			{
				this.ArchiveLocations = new UnsafeHashMap<ContentArchiveId, RuntimeContentCatalog.ArchiveLocation>(catalogData.Archives.Length, Allocator.Persistent);
			}
			else
			{
				this.ArchiveLocations.Capacity = this.ArchiveLocations.Count + catalogData.Archives.Length;
			}
			for (int i = 0; i < catalogData.Archives.Length; i++)
			{
				ContentArchiveId archiveId = catalogData.Archives[i].ArchiveId;
				this.ArchiveLocations.TryAdd(archiveId, new RuntimeContentCatalog.ArchiveLocation
				{
					PathIndex = this.ManagedStrings.Add(pathTransformFunc(archiveId.ToString()))
				});
			}
		}

		// Token: 0x06001C3B RID: 7227 RVA: 0x00062A38 File Offset: 0x00060C38
		internal unsafe void AddFileLocations(ref RuntimeContentCatalogData catalogData, int dependencyOffset, Func<string, string> mountedFileNameTransformFunc)
		{
			if (!this.ManagedStrings.IsCreated)
			{
				this.ManagedStrings = new ManagedStringTable(64);
			}
			if (!this.FileLocations.IsCreated)
			{
				this.FileLocations = new UnsafeHashMap<ContentFileId, RuntimeContentCatalog.FileLocation>(catalogData.Files.Length, Allocator.Persistent);
			}
			else
			{
				this.FileLocations.Capacity = this.FileLocations.Count + catalogData.Files.Length;
			}
			for (int i = 0; i < catalogData.Files.Length; i++)
			{
				ContentFileLocation contentFileLocation = *catalogData.Files[i];
				this.FileLocations.TryAdd(contentFileLocation.FileId, new RuntimeContentCatalog.FileLocation
				{
					ArchiveId = catalogData.Archives[contentFileLocation.ArchiveIndex].ArchiveId,
					DependencyIndex = contentFileLocation.DependencyIndex + dependencyOffset,
					PathIndex = this.ManagedStrings.Add(mountedFileNameTransformFunc(contentFileLocation.FileId.ToString()))
				});
			}
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x00062B4C File Offset: 0x00060D4C
		internal unsafe void AddSceneLocations(ref RuntimeContentCatalogData catalogData)
		{
			if (!this.ManagedStrings.IsCreated)
			{
				this.ManagedStrings = new ManagedStringTable(64);
			}
			if (!this.SceneLocations.IsCreated)
			{
				this.SceneLocations = new UnsafeHashMap<UntypedWeakReferenceId, RuntimeContentCatalog.SceneLocation>(catalogData.Scenes.Length, Allocator.Persistent);
			}
			else
			{
				this.SceneLocations.Capacity = this.SceneLocations.Count + catalogData.Scenes.Length;
			}
			for (int i = 0; i < catalogData.Scenes.Length; i++)
			{
				ContentSceneLocation contentSceneLocation = *catalogData.Scenes[i];
				ContentFileId fileId = catalogData.Files[contentSceneLocation.FileIndex].FileId;
				this.SceneLocations.TryAdd(new UntypedWeakReferenceId
				{
					GlobalId = contentSceneLocation.SceneId.GlobalId,
					GenerationType = contentSceneLocation.SceneId.GenerationType
				}, new RuntimeContentCatalog.SceneLocation
				{
					FileId = fileId,
					PathIndex = this.ManagedStrings.Add(contentSceneLocation.SceneName.ToString())
				});
			}
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x00062C74 File Offset: 0x00060E74
		internal unsafe void AddObjectLocations(ref RuntimeContentCatalogData catalogData)
		{
			if (!this.ObjectLocations.IsCreated)
			{
				this.ObjectLocations = new UnsafeHashMap<UntypedWeakReferenceId, RuntimeContentCatalog.ObjectLocation>(catalogData.Objects.Length, Allocator.Persistent);
			}
			else
			{
				this.ObjectLocations.Capacity = this.ObjectLocations.Count + catalogData.Objects.Length;
			}
			ContentFileLocation* unsafePtr = (ContentFileLocation*)catalogData.Files.GetUnsafePtr();
			ContentObjectLocation* unsafePtr2 = (ContentObjectLocation*)catalogData.Objects.GetUnsafePtr();
			int length = catalogData.Objects.Length;
			for (int i = 0; i < length; i++)
			{
				ContentObjectLocation contentObjectLocation = unsafePtr2[i];
				this.ObjectLocations.TryAdd(new UntypedWeakReferenceId
				{
					GlobalId = contentObjectLocation.ObjectId.GlobalId,
					GenerationType = contentObjectLocation.ObjectId.GenerationType
				}, new RuntimeContentCatalog.ObjectLocation
				{
					FileId = unsafePtr[contentObjectLocation.FileIndex].FileId,
					LocalIdentifierInFile = contentObjectLocation.LocalIdentifierInFile
				});
			}
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x00062D88 File Offset: 0x00060F88
		public bool TryGetObjectLocation(UntypedWeakReferenceId objectId, out ContentFileId fileId, out long localIdentifierInFile)
		{
			RuntimeContentCatalog.ObjectLocation objectLocation;
			if (this.ObjectLocations.TryGetValue(objectId, out objectLocation))
			{
				fileId = objectLocation.FileId;
				localIdentifierInFile = objectLocation.LocalIdentifierInFile;
				return true;
			}
			fileId = default(ContentFileId);
			localIdentifierInFile = 0L;
			return false;
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x00062DC8 File Offset: 0x00060FC8
		public bool TryGetSceneLocation(UntypedWeakReferenceId sceneId, out ContentFileId fileId, out string sceneName)
		{
			RuntimeContentCatalog.SceneLocation sceneLocation;
			if (this.SceneLocations.TryGetValue(sceneId, out sceneLocation))
			{
				fileId = sceneLocation.FileId;
				sceneName = this.GetStringValue(sceneLocation.PathIndex);
				return true;
			}
			fileId = default(ContentFileId);
			sceneName = null;
			return false;
		}

		// Token: 0x06001C40 RID: 7232 RVA: 0x00062E0C File Offset: 0x0006100C
		public bool TryGetFileLocation(ContentFileId fileId, out string filePath, out UnsafeList<ContentFileId> fileDependencies, out ContentArchiveId archiveId, out int dependencyIndex)
		{
			int num;
			if (this.TryGetFileLocation(fileId, out num, out fileDependencies, out archiveId, out dependencyIndex))
			{
				filePath = this.GetStringValue(num);
				return true;
			}
			filePath = null;
			return false;
		}

		// Token: 0x06001C41 RID: 7233 RVA: 0x00062E38 File Offset: 0x00061038
		public bool TryGetFileLocation(ContentFileId fileId, out int filePathIndex, out UnsafeList<ContentFileId> fileDependencies, out ContentArchiveId archiveId, out int dependencyIndex)
		{
			RuntimeContentCatalog.FileLocation fileLocation;
			if (this.FileLocations.TryGetValue(fileId, out fileLocation))
			{
				filePathIndex = fileLocation.PathIndex;
				archiveId = fileLocation.ArchiveId;
				dependencyIndex = fileLocation.DependencyIndex;
				fileDependencies = this.FileDependencies[fileLocation.DependencyIndex];
				return true;
			}
			filePathIndex = -1;
			fileDependencies = default(UnsafeList<ContentFileId>);
			archiveId = default(ContentArchiveId);
			dependencyIndex = -1;
			return false;
		}

		// Token: 0x06001C42 RID: 7234 RVA: 0x00062EA4 File Offset: 0x000610A4
		public bool TryGetArchiveLocation(ContentArchiveId archiveId, out string archivePath)
		{
			RuntimeContentCatalog.ArchiveLocation archiveLocation;
			if (this.ArchiveLocations.TryGetValue(archiveId, out archiveLocation))
			{
				archivePath = this.GetStringValue(archiveLocation.PathIndex);
				return true;
			}
			archivePath = null;
			return false;
		}

		// Token: 0x06001C43 RID: 7235 RVA: 0x00062ED8 File Offset: 0x000610D8
		public bool TryGetArchiveLocation(ContentArchiveId archiveId, out int archivePathIndex)
		{
			RuntimeContentCatalog.ArchiveLocation archiveLocation;
			if (this.ArchiveLocations.TryGetValue(archiveId, out archiveLocation))
			{
				archivePathIndex = archiveLocation.PathIndex;
				return true;
			}
			archivePathIndex = -1;
			return false;
		}

		// Token: 0x06001C44 RID: 7236 RVA: 0x00062F04 File Offset: 0x00061104
		public void Print(Action<string> printFunc)
		{
			try
			{
				using (NativeArray<ContentArchiveId> archiveIds = this.GetArchiveIds(Allocator.Temp))
				{
					for (int i = 0; i < archiveIds.Length; i++)
					{
						ContentArchiveId contentArchiveId = archiveIds[i];
						string text;
						if (!this.TryGetArchiveLocation(contentArchiveId, out text))
						{
							printFunc(string.Format("Unable to find location for archive id {0}.", contentArchiveId));
						}
						else
						{
							printFunc(string.Format("Archives[{0}] = {1}, path = {2}", i, contentArchiveId, text));
						}
					}
				}
				using (NativeArray<ContentFileId> fileIds = this.GetFileIds(Allocator.Temp))
				{
					for (int j = 0; j < fileIds.Length; j++)
					{
						ContentFileId contentFileId = fileIds[j];
						string text2;
						UnsafeList<ContentFileId> unsafeList;
						ContentArchiveId contentArchiveId2;
						int num;
						if (!this.TryGetFileLocation(contentFileId, out text2, out unsafeList, out contentArchiveId2, out num))
						{
							printFunc(string.Format("Unable to find location for file id {0}", contentFileId));
						}
						else
						{
							printFunc(string.Format("Files[{0}] = {1}, ArchiveId = {2}, path = {3}, dependencyIndex = {4}", new object[] { j, contentFileId, contentArchiveId2, text2, num }));
						}
					}
				}
				using (NativeArray<UntypedWeakReferenceId> objectIds = this.GetObjectIds(Allocator.Temp))
				{
					for (int k = 0; k < objectIds.Length; k++)
					{
						UntypedWeakReferenceId untypedWeakReferenceId = objectIds[k];
						ContentFileId contentFileId2;
						long num2;
						if (!this.TryGetObjectLocation(untypedWeakReferenceId, out contentFileId2, out num2))
						{
							printFunc(string.Format("Unable to find location for object id {0}", untypedWeakReferenceId));
						}
						else
						{
							printFunc(string.Format("Objects[{0}] = {1}, FileId = {2}, IdInFile = {3}", new object[] { k, untypedWeakReferenceId, contentFileId2, num2 }));
						}
					}
				}
				for (int l = 0; l < this.FileDependencies.Length; l++)
				{
					printFunc(string.Format("Dependencies[{0}]:", l));
					StringBuilder stringBuilder = new StringBuilder(64 * this.FileDependencies[l].Length);
					stringBuilder.Append("ContentFileIds: [");
					for (int m = 0; m < this.FileDependencies[l].Length; m++)
					{
						StringBuilder stringBuilder2 = stringBuilder;
						Hash128 value = this.FileDependencies[l][m].Value;
						stringBuilder2.Append(value.ToString() + ", ");
					}
					stringBuilder.Remove(stringBuilder.Length - 2, 2);
					stringBuilder.Append("]");
					printFunc(stringBuilder.ToString());
				}
			}
			catch (Exception ex)
			{
				printFunc(ex.ToString());
			}
		}

		// Token: 0x040009F6 RID: 2550
		internal UnsafeHashMap<UntypedWeakReferenceId, RuntimeContentCatalog.SceneLocation> SceneLocations;

		// Token: 0x040009F7 RID: 2551
		internal UnsafeHashMap<UntypedWeakReferenceId, RuntimeContentCatalog.ObjectLocation> ObjectLocations;

		// Token: 0x040009F8 RID: 2552
		internal UnsafeHashMap<ContentFileId, RuntimeContentCatalog.FileLocation> FileLocations;

		// Token: 0x040009F9 RID: 2553
		internal UnsafeHashMap<ContentArchiveId, RuntimeContentCatalog.ArchiveLocation> ArchiveLocations;

		// Token: 0x040009FA RID: 2554
		internal UnsafeList<UnsafeList<ContentFileId>> FileDependencies;

		// Token: 0x040009FB RID: 2555
		internal ManagedStringTable ManagedStrings;

		// Token: 0x02000478 RID: 1144
		internal struct ObjectLocation
		{
			// Token: 0x040009FC RID: 2556
			public ContentFileId FileId;

			// Token: 0x040009FD RID: 2557
			public long LocalIdentifierInFile;
		}

		// Token: 0x02000479 RID: 1145
		internal struct SceneLocation
		{
			// Token: 0x040009FE RID: 2558
			public ContentFileId FileId;

			// Token: 0x040009FF RID: 2559
			public int PathIndex;
		}

		// Token: 0x0200047A RID: 1146
		internal struct FileLocation
		{
			// Token: 0x04000A00 RID: 2560
			public ContentArchiveId ArchiveId;

			// Token: 0x04000A01 RID: 2561
			public int DependencyIndex;

			// Token: 0x04000A02 RID: 2562
			public int PathIndex;
		}

		// Token: 0x0200047B RID: 1147
		internal struct ArchiveLocation
		{
			// Token: 0x04000A03 RID: 2563
			public int PathIndex;
		}
	}
}
