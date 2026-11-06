using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Content;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.SceneManagement;

namespace Unity.Loading
{
	// Token: 0x02000009 RID: 9
	[NativeHeader("Modules/ContentLoad/Public/ContentLoadFrontend.h")]
	[StaticAccessor("GetContentLoadFrontend()", StaticAccessorType.Dot)]
	public static class ContentLoadInterface
	{
		// Token: 0x06000019 RID: 25 RVA: 0x0000236C File Offset: 0x0000056C
		[NativeThrows]
		internal unsafe static ContentFile LoadContentFileAsync(ContentNamespace nameSpace, string filename, void* dependencies, int dependencyCount, JobHandle dependentFence, bool useUnsafe = false)
		{
			ContentFile contentFile;
			ContentLoadInterface.LoadContentFileAsync_Injected(ref nameSpace, filename, dependencies, dependencyCount, ref dependentFence, useUnsafe, out contentFile);
			return contentFile;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000238A File Offset: 0x0000058A
		[NativeThrows]
		internal static void ContentFile_UnloadAsync(ContentFile handle)
		{
			ContentLoadInterface.ContentFile_UnloadAsync_Injected(ref handle);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002393 File Offset: 0x00000593
		internal static Object ContentFile_GetObject(ContentFile handle, ulong localIdentifierInFile)
		{
			return ContentLoadInterface.ContentFile_GetObject_Injected(ref handle, localIdentifierInFile);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000239D File Offset: 0x0000059D
		internal static Object[] ContentFile_GetObjects(ContentFile handle)
		{
			return ContentLoadInterface.ContentFile_GetObjects_Injected(ref handle);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000023A6 File Offset: 0x000005A6
		internal static LoadingStatus ContentFile_GetLoadingStatus(ContentFile handle)
		{
			return ContentLoadInterface.ContentFile_GetLoadingStatus_Injected(ref handle);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000023AF File Offset: 0x000005AF
		internal static bool ContentFile_IsHandleValid(ContentFile handle)
		{
			return ContentLoadInterface.ContentFile_IsHandleValid_Injected(ref handle);
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001F RID: 31
		// (set) Token: 0x06000020 RID: 32
		internal static extern float IntegrationTimeMS
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000023B8 File Offset: 0x000005B8
		internal static bool WaitForLoadCompletion(ContentFile handle, int timeoutMs)
		{
			return ContentLoadInterface.WaitForLoadCompletion_Injected(ref handle, timeoutMs);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000023C2 File Offset: 0x000005C2
		internal static bool WaitForUnloadCompletion(ContentFile handle, int timeoutMs)
		{
			return ContentLoadInterface.WaitForUnloadCompletion_Injected(ref handle, timeoutMs);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000023CC File Offset: 0x000005CC
		internal static bool ContentFile_IsUnloadComplete(ContentFile handle)
		{
			return ContentLoadInterface.ContentFile_IsUnloadComplete_Injected(ref handle);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000023D8 File Offset: 0x000005D8
		[NativeThrows]
		internal unsafe static ContentSceneFile LoadSceneAsync(ContentNamespace nameSpace, string filename, string sceneName, ContentSceneParameters sceneParams, ContentFile* dependencies, int dependencyCount, JobHandle dependentFence)
		{
			ContentSceneFile contentSceneFile;
			ContentLoadInterface.LoadSceneAsync_Injected(ref nameSpace, filename, sceneName, ref sceneParams, dependencies, dependencyCount, ref dependentFence, out contentSceneFile);
			return contentSceneFile;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000023FC File Offset: 0x000005FC
		internal static Scene ContentSceneFile_GetScene(ContentSceneFile handle)
		{
			Scene scene;
			ContentLoadInterface.ContentSceneFile_GetScene_Injected(ref handle, out scene);
			return scene;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002413 File Offset: 0x00000613
		internal static SceneLoadingStatus ContentSceneFile_GetStatus(ContentSceneFile handle)
		{
			return ContentLoadInterface.ContentSceneFile_GetStatus_Injected(ref handle);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x0000241C File Offset: 0x0000061C
		[NativeThrows]
		internal static void ContentSceneFile_IntegrateAtEndOfFrame(ContentSceneFile handle)
		{
			ContentLoadInterface.ContentSceneFile_IntegrateAtEndOfFrame_Injected(ref handle);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002425 File Offset: 0x00000625
		internal static bool ContentSceneFile_UnloadAtEndOfFrame(ContentSceneFile handle)
		{
			return ContentLoadInterface.ContentSceneFile_UnloadAtEndOfFrame_Injected(ref handle);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000242E File Offset: 0x0000062E
		internal static bool ContentSceneFile_IsHandleValid(ContentSceneFile handle)
		{
			return ContentLoadInterface.ContentSceneFile_IsHandleValid_Injected(ref handle);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002437 File Offset: 0x00000637
		internal static bool ContentSceneFile_WaitForCompletion(ContentSceneFile handle, int timeoutMs)
		{
			return ContentLoadInterface.ContentSceneFile_WaitForCompletion_Injected(ref handle, timeoutMs);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002444 File Offset: 0x00000644
		public unsafe static ContentSceneFile LoadSceneAsync(ContentNamespace nameSpace, string filename, string sceneName, ContentSceneParameters sceneParams, NativeArray<ContentFile> dependencies, JobHandle dependentFence = default(JobHandle))
		{
			return ContentLoadInterface.LoadSceneAsync(nameSpace, filename, sceneName, sceneParams, (ContentFile*)dependencies.m_Buffer, dependencies.Length, dependentFence);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002470 File Offset: 0x00000670
		public static ContentFile LoadContentFileAsync(ContentNamespace nameSpace, string filename, NativeArray<ContentFile> dependencies, JobHandle dependentFence = default(JobHandle))
		{
			return ContentLoadInterface.LoadContentFileAsync(nameSpace, filename, dependencies.m_Buffer, dependencies.Length, dependentFence, false);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002499 File Offset: 0x00000699
		public static ContentFile[] GetContentFiles(ContentNamespace nameSpace)
		{
			return ContentLoadInterface.GetContentFiles_Injected(ref nameSpace);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000024A2 File Offset: 0x000006A2
		public static ContentSceneFile[] GetSceneFiles(ContentNamespace nameSpace)
		{
			return ContentLoadInterface.GetSceneFiles_Injected(ref nameSpace);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000024AC File Offset: 0x000006AC
		public static float GetIntegrationTimeMS()
		{
			return ContentLoadInterface.IntegrationTimeMS;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000024C4 File Offset: 0x000006C4
		public static void SetIntegrationTimeMS(float integrationTimeMS)
		{
			bool flag = integrationTimeMS <= 0f;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("integrationTimeMS", "integrationTimeMS was out of range. Must be greater than zero.");
			}
			ContentLoadInterface.IntegrationTimeMS = integrationTimeMS;
		}

		// Token: 0x06000031 RID: 49
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void LoadContentFileAsync_Injected(ref ContentNamespace nameSpace, string filename, void* dependencies, int dependencyCount, ref JobHandle dependentFence, bool useUnsafe = false, out ContentFile ret);

		// Token: 0x06000032 RID: 50
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ContentFile_UnloadAsync_Injected(ref ContentFile handle);

		// Token: 0x06000033 RID: 51
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Object ContentFile_GetObject_Injected(ref ContentFile handle, ulong localIdentifierInFile);

		// Token: 0x06000034 RID: 52
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Object[] ContentFile_GetObjects_Injected(ref ContentFile handle);

		// Token: 0x06000035 RID: 53
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern LoadingStatus ContentFile_GetLoadingStatus_Injected(ref ContentFile handle);

		// Token: 0x06000036 RID: 54
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ContentFile_IsHandleValid_Injected(ref ContentFile handle);

		// Token: 0x06000037 RID: 55
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool WaitForLoadCompletion_Injected(ref ContentFile handle, int timeoutMs);

		// Token: 0x06000038 RID: 56
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool WaitForUnloadCompletion_Injected(ref ContentFile handle, int timeoutMs);

		// Token: 0x06000039 RID: 57
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ContentFile_IsUnloadComplete_Injected(ref ContentFile handle);

		// Token: 0x0600003A RID: 58
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void LoadSceneAsync_Injected(ref ContentNamespace nameSpace, string filename, string sceneName, ref ContentSceneParameters sceneParams, ContentFile* dependencies, int dependencyCount, ref JobHandle dependentFence, out ContentSceneFile ret);

		// Token: 0x0600003B RID: 59
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ContentSceneFile_GetScene_Injected(ref ContentSceneFile handle, out Scene ret);

		// Token: 0x0600003C RID: 60
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern SceneLoadingStatus ContentSceneFile_GetStatus_Injected(ref ContentSceneFile handle);

		// Token: 0x0600003D RID: 61
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ContentSceneFile_IntegrateAtEndOfFrame_Injected(ref ContentSceneFile handle);

		// Token: 0x0600003E RID: 62
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ContentSceneFile_UnloadAtEndOfFrame_Injected(ref ContentSceneFile handle);

		// Token: 0x0600003F RID: 63
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ContentSceneFile_IsHandleValid_Injected(ref ContentSceneFile handle);

		// Token: 0x06000040 RID: 64
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ContentSceneFile_WaitForCompletion_Injected(ref ContentSceneFile handle, int timeoutMs);

		// Token: 0x06000041 RID: 65
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ContentFile[] GetContentFiles_Injected(ref ContentNamespace nameSpace);

		// Token: 0x06000042 RID: 66
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ContentSceneFile[] GetSceneFiles_Injected(ref ContentNamespace nameSpace);
	}
}
