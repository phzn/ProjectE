using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

namespace UnityEngine.ResourceManagement.Util
{
	// Token: 0x02000027 RID: 39
	[Serializable]
	public struct ObjectInitializationData
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00004FB5 File Offset: 0x000031B5
		public string Id
		{
			get
			{
				return this.m_Id;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00004FBD File Offset: 0x000031BD
		public SerializedType ObjectType
		{
			get
			{
				return this.m_ObjectType;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00004FC5 File Offset: 0x000031C5
		public string Data
		{
			get
			{
				return this.m_Data;
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00004FCD File Offset: 0x000031CD
		public override string ToString()
		{
			return string.Format("ObjectInitializationData: id={0}, type={1}", this.m_Id, this.m_ObjectType);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00004FEC File Offset: 0x000031EC
		public TObject CreateInstance<TObject>(string idOverride = null)
		{
			TObject tobject;
			try
			{
				Type value = this.m_ObjectType.Value;
				if (value == null)
				{
					tobject = default(TObject);
					tobject = tobject;
				}
				else
				{
					object obj = Activator.CreateInstance(value, true);
					IInitializableObject initializableObject = obj as IInitializableObject;
					if (initializableObject != null && !initializableObject.Initialize((idOverride == null) ? this.m_Id : idOverride, this.m_Data))
					{
						tobject = default(TObject);
					}
					else
					{
						tobject = (TObject)((object)obj);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				tobject = default(TObject);
			}
			return tobject;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005080 File Offset: 0x00003280
		public AsyncOperationHandle GetAsyncInitHandle(ResourceManager rm, string idOverride = null)
		{
			AsyncOperationHandle asyncOperationHandle;
			try
			{
				Type value = this.m_ObjectType.Value;
				if (value == null)
				{
					asyncOperationHandle = default(AsyncOperationHandle);
					asyncOperationHandle = asyncOperationHandle;
				}
				else
				{
					IInitializableObject initializableObject = Activator.CreateInstance(value, true) as IInitializableObject;
					if (initializableObject != null)
					{
						asyncOperationHandle = initializableObject.InitializeAsync(rm, (idOverride == null) ? this.m_Id : idOverride, this.m_Data);
					}
					else
					{
						asyncOperationHandle = default(AsyncOperationHandle);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				asyncOperationHandle = default(AsyncOperationHandle);
			}
			return asyncOperationHandle;
		}

		// Token: 0x0400005E RID: 94
		[FormerlySerializedAs("m_id")]
		[SerializeField]
		private string m_Id;

		// Token: 0x0400005F RID: 95
		[FormerlySerializedAs("m_objectType")]
		[SerializeField]
		private SerializedType m_ObjectType;

		// Token: 0x04000060 RID: 96
		[FormerlySerializedAs("m_data")]
		[SerializeField]
		private string m_Data;
	}
}
