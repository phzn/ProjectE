using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Serialization.Binary;
using UnityEngine;

namespace Unity.Entities.Serialization
{
	// Token: 0x02000389 RID: 905
	internal class ManagedObjectBinaryReader : IContravariantBinaryAdapter<Object>, IBinaryAdapter, IBinaryAdapter<AnimationCurve>, IBinaryAdapter<UntypedUnityObjectRef>
	{
		// Token: 0x06001814 RID: 6164 RVA: 0x0005A1F4 File Offset: 0x000583F4
		public unsafe ManagedObjectBinaryReader(UnsafeAppendBuffer.Reader* stream, NativeArray<int> unityObjects)
		{
			this.m_Stream = stream;
			this.m_Params = new BinarySerializationParameters
			{
				UserDefinedAdapters = new List<IBinaryAdapter> { this },
				State = new BinarySerializationState()
			};
			this.m_UnityObjects = unityObjects;
			this.m_UnityObjectsArray = new List<Object>(this.m_UnityObjects.Length);
			Resources.InstanceIDToObjectList(unityObjects, this.m_UnityObjectsArray);
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x0005A265 File Offset: 0x00058465
		public void AddAdapter(IBinaryAdapter adapter)
		{
			this.m_Params.UserDefinedAdapters.Add(adapter);
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x0005A278 File Offset: 0x00058478
		public object ReadObject(Type type)
		{
			BinarySerializationParameters @params = this.m_Params;
			@params.SerializedType = type;
			return BinarySerialization.FromBinary<object>(this.m_Stream, @params);
		}

		// Token: 0x06001817 RID: 6167 RVA: 0x0005A2A0 File Offset: 0x000584A0
		void IContravariantBinaryAdapter<Object>.Serialize(IBinarySerializationContext context, Object value)
		{
			throw new InvalidOperationException("Serialize should never be invoked by ManagedObjectBinaryReader.");
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x0005A2AC File Offset: 0x000584AC
		unsafe object IContravariantBinaryAdapter<Object>.Deserialize(IBinaryDeserializationContext context)
		{
			int num = context.Reader->ReadNext<int>();
			if (num == -1)
			{
				return null;
			}
			if (!this.m_UnityObjects.IsCreated)
			{
				throw new ArgumentException("We are reading a UnityEngine.Object however no ObjectTable was provided to the ManagedObjectBinaryReader.");
			}
			if ((ulong)num >= (ulong)((long)this.m_UnityObjects.Length))
			{
				throw new ArgumentException("We are reading a UnityEngine.Object but the deserialized index is out of range for the given object table.");
			}
			return this.m_UnityObjectsArray[num];
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x0005A2A0 File Offset: 0x000584A0
		void IBinaryAdapter<AnimationCurve>.Serialize(in BinarySerializationContext<AnimationCurve> context, AnimationCurve value)
		{
			throw new InvalidOperationException("Serialize should never be invoked by ManagedObjectBinaryReader.");
		}

		// Token: 0x0600181A RID: 6170 RVA: 0x0005A310 File Offset: 0x00058510
		unsafe AnimationCurve IBinaryAdapter<AnimationCurve>.Deserialize(in BinaryDeserializationContext<AnimationCurve> context)
		{
			int num = context.Reader->ReadNext<int>();
			if (num >= 0)
			{
				WrapMode wrapMode = context.Reader->ReadNext<WrapMode>();
				WrapMode wrapMode2 = context.Reader->ReadNext<WrapMode>();
				AnimationCurve animationCurve = new AnimationCurve
				{
					preWrapMode = wrapMode,
					postWrapMode = wrapMode2
				};
				for (int i = 0; i < num; i++)
				{
					animationCurve.AddKey(context.Reader->ReadNext<SerializedKeyFrame>());
				}
				return animationCurve;
			}
			return null;
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x0005A2A0 File Offset: 0x000584A0
		public void Serialize(in BinarySerializationContext<UntypedUnityObjectRef> context, UntypedUnityObjectRef value)
		{
			throw new InvalidOperationException("Serialize should never be invoked by ManagedObjectBinaryReader.");
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x0005A384 File Offset: 0x00058584
		public unsafe UntypedUnityObjectRef Deserialize(in BinaryDeserializationContext<UntypedUnityObjectRef> context)
		{
			int num = context.Reader->ReadNext<int>();
			if (num == -1)
			{
				return default(UntypedUnityObjectRef);
			}
			if (!this.m_UnityObjects.IsCreated)
			{
				throw new ArgumentException("We are reading a UnityEngine.Object however no ObjectTable was provided to the ManagedObjectBinaryReader.");
			}
			if ((ulong)num >= (ulong)((long)this.m_UnityObjects.Length))
			{
				throw new ArgumentException("We are reading a UnityEngine.Object but the deserialized index is out of range for the given object table.");
			}
			return new UntypedUnityObjectRef
			{
				instanceId = this.m_UnityObjects[num]
			};
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x0005A403 File Offset: 0x00058603
		void IBinaryAdapter<UntypedUnityObjectRef>.Serialize(in BinarySerializationContext<UntypedUnityObjectRef> context, UntypedUnityObjectRef value)
		{
			this.Serialize(in context, value);
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x0005A40D File Offset: 0x0005860D
		UntypedUnityObjectRef IBinaryAdapter<UntypedUnityObjectRef>.Deserialize(in BinaryDeserializationContext<UntypedUnityObjectRef> context)
		{
			return this.Deserialize(in context);
		}

		// Token: 0x04000931 RID: 2353
		private unsafe readonly UnsafeAppendBuffer.Reader* m_Stream;

		// Token: 0x04000932 RID: 2354
		private readonly BinarySerializationParameters m_Params;

		// Token: 0x04000933 RID: 2355
		private readonly NativeArray<int> m_UnityObjects;

		// Token: 0x04000934 RID: 2356
		private readonly List<Object> m_UnityObjectsArray;
	}
}
