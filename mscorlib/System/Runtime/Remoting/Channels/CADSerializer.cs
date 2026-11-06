using System;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters.Binary;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005B0 RID: 1456
	internal class CADSerializer
	{
		// Token: 0x06003868 RID: 14440 RVA: 0x000CA650 File Offset: 0x000C8850
		internal static IMessage DeserializeMessage(MemoryStream mem, IMethodCallMessage msg)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.SurrogateSelector = null;
			mem.Position = 0L;
			if (msg == null)
			{
				return (IMessage)binaryFormatter.Deserialize(mem, null);
			}
			return (IMessage)binaryFormatter.DeserializeMethodResponse(mem, null, msg);
		}

		// Token: 0x06003869 RID: 14441 RVA: 0x000CA694 File Offset: 0x000C8894
		internal static MemoryStream SerializeMessage(IMessage msg)
		{
			MemoryStream memoryStream = new MemoryStream();
			new BinaryFormatter
			{
				SurrogateSelector = new RemotingSurrogateSelector()
			}.Serialize(memoryStream, msg);
			memoryStream.Position = 0L;
			return memoryStream;
		}

		// Token: 0x0600386A RID: 14442 RVA: 0x000CA6C8 File Offset: 0x000C88C8
		internal static object DeserializeObjectSafe(byte[] mem)
		{
			byte[] array = new byte[mem.Length];
			Array.Copy(mem, array, mem.Length);
			return CADSerializer.DeserializeObject(new MemoryStream(array));
		}

		// Token: 0x0600386B RID: 14443 RVA: 0x000CA6F4 File Offset: 0x000C88F4
		internal static MemoryStream SerializeObject(object obj)
		{
			MemoryStream memoryStream = new MemoryStream();
			new BinaryFormatter
			{
				SurrogateSelector = new RemotingSurrogateSelector()
			}.Serialize(memoryStream, obj);
			memoryStream.Position = 0L;
			return memoryStream;
		}

		// Token: 0x0600386C RID: 14444 RVA: 0x000CA727 File Offset: 0x000C8927
		internal static object DeserializeObject(MemoryStream mem)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.SurrogateSelector = null;
			mem.Position = 0L;
			return binaryFormatter.Deserialize(mem);
		}
	}
}
