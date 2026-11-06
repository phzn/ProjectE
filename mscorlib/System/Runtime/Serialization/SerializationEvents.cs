using System;
using System.Collections.Generic;
using System.Reflection;

namespace System.Runtime.Serialization
{
	// Token: 0x02000656 RID: 1622
	internal sealed class SerializationEvents
	{
		// Token: 0x06003CA5 RID: 15525 RVA: 0x000D1B14 File Offset: 0x000CFD14
		internal SerializationEvents(Type t)
		{
			this._onSerializingMethods = this.GetMethodsWithAttribute(typeof(OnSerializingAttribute), t);
			this._onSerializedMethods = this.GetMethodsWithAttribute(typeof(OnSerializedAttribute), t);
			this._onDeserializingMethods = this.GetMethodsWithAttribute(typeof(OnDeserializingAttribute), t);
			this._onDeserializedMethods = this.GetMethodsWithAttribute(typeof(OnDeserializedAttribute), t);
		}

		// Token: 0x06003CA6 RID: 15526 RVA: 0x000D1B84 File Offset: 0x000CFD84
		private List<MethodInfo> GetMethodsWithAttribute(Type attribute, Type t)
		{
			List<MethodInfo> list = null;
			Type type = t;
			while (type != null && type != typeof(object))
			{
				foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					if (methodInfo.IsDefined(attribute, false))
					{
						if (list == null)
						{
							list = new List<MethodInfo>();
						}
						list.Add(methodInfo);
					}
				}
				type = type.BaseType;
			}
			if (list != null)
			{
				list.Reverse();
			}
			return list;
		}

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x06003CA7 RID: 15527 RVA: 0x000D1BFB File Offset: 0x000CFDFB
		internal bool HasOnSerializingEvents
		{
			get
			{
				return this._onSerializingMethods != null || this._onSerializedMethods != null;
			}
		}

		// Token: 0x06003CA8 RID: 15528 RVA: 0x000D1C10 File Offset: 0x000CFE10
		internal void InvokeOnSerializing(object obj, StreamingContext context)
		{
			SerializationEvents.InvokeOnDelegate(obj, context, this._onSerializingMethods);
		}

		// Token: 0x06003CA9 RID: 15529 RVA: 0x000D1C1F File Offset: 0x000CFE1F
		internal void InvokeOnDeserializing(object obj, StreamingContext context)
		{
			SerializationEvents.InvokeOnDelegate(obj, context, this._onDeserializingMethods);
		}

		// Token: 0x06003CAA RID: 15530 RVA: 0x000D1C2E File Offset: 0x000CFE2E
		internal void InvokeOnDeserialized(object obj, StreamingContext context)
		{
			SerializationEvents.InvokeOnDelegate(obj, context, this._onDeserializedMethods);
		}

		// Token: 0x06003CAB RID: 15531 RVA: 0x000D1C3D File Offset: 0x000CFE3D
		internal SerializationEventHandler AddOnSerialized(object obj, SerializationEventHandler handler)
		{
			return SerializationEvents.AddOnDelegate(obj, handler, this._onSerializedMethods);
		}

		// Token: 0x06003CAC RID: 15532 RVA: 0x000D1C4C File Offset: 0x000CFE4C
		internal SerializationEventHandler AddOnDeserialized(object obj, SerializationEventHandler handler)
		{
			return SerializationEvents.AddOnDelegate(obj, handler, this._onDeserializedMethods);
		}

		// Token: 0x06003CAD RID: 15533 RVA: 0x000D1C5B File Offset: 0x000CFE5B
		private static void InvokeOnDelegate(object obj, StreamingContext context, List<MethodInfo> methods)
		{
			SerializationEventHandler serializationEventHandler = SerializationEvents.AddOnDelegate(obj, null, methods);
			if (serializationEventHandler == null)
			{
				return;
			}
			serializationEventHandler(context);
		}

		// Token: 0x06003CAE RID: 15534 RVA: 0x000D1C70 File Offset: 0x000CFE70
		private static SerializationEventHandler AddOnDelegate(object obj, SerializationEventHandler handler, List<MethodInfo> methods)
		{
			if (methods != null)
			{
				foreach (MethodInfo methodInfo in methods)
				{
					SerializationEventHandler serializationEventHandler = (SerializationEventHandler)methodInfo.CreateDelegate(typeof(SerializationEventHandler), obj);
					handler = (SerializationEventHandler)Delegate.Combine(handler, serializationEventHandler);
				}
			}
			return handler;
		}

		// Token: 0x04002724 RID: 10020
		private readonly List<MethodInfo> _onSerializingMethods;

		// Token: 0x04002725 RID: 10021
		private readonly List<MethodInfo> _onSerializedMethods;

		// Token: 0x04002726 RID: 10022
		private readonly List<MethodInfo> _onDeserializingMethods;

		// Token: 0x04002727 RID: 10023
		private readonly List<MethodInfo> _onDeserializedMethods;
	}
}
