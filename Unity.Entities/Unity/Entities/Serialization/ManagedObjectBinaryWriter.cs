using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Serialization.Binary;
using UnityEngine;

namespace Unity.Entities.Serialization
{
	// Token: 0x02000388 RID: 904
	internal class ManagedObjectBinaryWriter : IContravariantBinaryAdapter<Object>, IBinaryAdapter, IBinaryAdapter<AnimationCurve>, IBinaryAdapter<UntypedUnityObjectRef>
	{
		// Token: 0x0600180A RID: 6154 RVA: 0x00059FB8 File Offset: 0x000581B8
		public unsafe ManagedObjectBinaryWriter(UnsafeAppendBuffer* stream, UnityObjectRefMap unityObjectRefs)
		{
			this.m_Stream = stream;
			this.m_Params = new BinarySerializationParameters
			{
				UserDefinedAdapters = new List<IBinaryAdapter> { this },
				State = new BinarySerializationState()
			};
			this.m_UnityObjectRefs = unityObjectRefs;
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x0005A007 File Offset: 0x00058207
		public void AddAdapter(IBinaryAdapter adapter)
		{
			this.m_Params.UserDefinedAdapters.Add(adapter);
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x0005A01C File Offset: 0x0005821C
		public void WriteObject(object obj)
		{
			BinarySerializationParameters @params = this.m_Params;
			@params.SerializedType = ((obj != null) ? obj.GetType() : null);
			BinarySerialization.ToBinary<object>(this.m_Stream, obj, @params);
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x0005A050 File Offset: 0x00058250
		unsafe void IBinaryAdapter<UntypedUnityObjectRef>.Serialize(in BinarySerializationContext<UntypedUnityObjectRef> context, UntypedUnityObjectRef value)
		{
			int num = -1;
			if (value.instanceId != 0 && this.m_UnityObjectRefs.IsCreated && !this.m_UnityObjectRefs.InstanceIDMap.TryGetValue(value.instanceId, out num))
			{
				num = this.m_UnityObjectRefs.InstanceIDs.Length;
				this.m_UnityObjectRefs.InstanceIDMap.Add(value.instanceId, num);
				this.m_UnityObjectRefs.InstanceIDs.Add(in value.instanceId);
			}
			context.Writer->Add<int>(num);
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x0005A0D9 File Offset: 0x000582D9
		public UntypedUnityObjectRef Deserialize(in BinaryDeserializationContext<UntypedUnityObjectRef> context)
		{
			throw new InvalidOperationException("Deserialize should never be invoked by ManagedObjectBinaryWriter");
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x0005A0E8 File Offset: 0x000582E8
		unsafe void IContravariantBinaryAdapter<Object>.Serialize(IBinarySerializationContext context, Object value)
		{
			int num = -1;
			if (value != null)
			{
				int instanceID = value.GetInstanceID();
				if (instanceID != 0 && this.m_UnityObjectRefs.IsCreated && !this.m_UnityObjectRefs.InstanceIDMap.TryGetValue(instanceID, out num))
				{
					num = this.m_UnityObjectRefs.InstanceIDs.Length;
					this.m_UnityObjectRefs.InstanceIDMap.Add(instanceID, num);
					this.m_UnityObjectRefs.InstanceIDs.Add(in instanceID);
				}
			}
			context.Writer->Add<int>(num);
		}

		// Token: 0x06001810 RID: 6160 RVA: 0x0005A0D9 File Offset: 0x000582D9
		object IContravariantBinaryAdapter<Object>.Deserialize(IBinaryDeserializationContext context)
		{
			throw new InvalidOperationException("Deserialize should never be invoked by ManagedObjectBinaryWriter");
		}

		// Token: 0x06001811 RID: 6161 RVA: 0x0005A170 File Offset: 0x00058370
		unsafe void IBinaryAdapter<AnimationCurve>.Serialize(in BinarySerializationContext<AnimationCurve> context, AnimationCurve value)
		{
			if (value != null)
			{
				context.Writer->Add<int>(value.length);
				context.Writer->Add<WrapMode>(value.preWrapMode);
				context.Writer->Add<WrapMode>(value.postWrapMode);
				int i = 0;
				int length = value.length;
				while (i < length)
				{
					context.Writer->Add<SerializedKeyFrame>(value[i]);
					i++;
				}
				return;
			}
			context.Writer->Add<int>(-1);
		}

		// Token: 0x06001812 RID: 6162 RVA: 0x0005A0D9 File Offset: 0x000582D9
		AnimationCurve IBinaryAdapter<AnimationCurve>.Deserialize(in BinaryDeserializationContext<AnimationCurve> context)
		{
			throw new InvalidOperationException("Deserialize should never be invoked by ManagedObjectBinaryWriter");
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x0005A1EA File Offset: 0x000583EA
		UntypedUnityObjectRef IBinaryAdapter<UntypedUnityObjectRef>.Deserialize(in BinaryDeserializationContext<UntypedUnityObjectRef> context)
		{
			return this.Deserialize(in context);
		}

		// Token: 0x0400092E RID: 2350
		private unsafe readonly UnsafeAppendBuffer* m_Stream;

		// Token: 0x0400092F RID: 2351
		private readonly BinarySerializationParameters m_Params;

		// Token: 0x04000930 RID: 2352
		private UnityObjectRefMap m_UnityObjectRefs;
	}
}
