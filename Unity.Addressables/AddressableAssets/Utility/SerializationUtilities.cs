using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace UnityEngine.AddressableAssets.Utility
{
	// Token: 0x0200001E RID: 30
	internal static class SerializationUtilities
	{
		// Token: 0x0600018C RID: 396 RVA: 0x000070D9 File Offset: 0x000052D9
		internal static int ReadInt32FromByteArray(byte[] data, int offset)
		{
			return (int)data[offset] | ((int)data[offset + 1] << 8) | ((int)data[offset + 2] << 16) | ((int)data[offset + 3] << 24);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x000070F8 File Offset: 0x000052F8
		internal static int WriteInt32ToByteArray(byte[] data, int val, int offset)
		{
			data[offset] = (byte)(val & 255);
			data[offset + 1] = (byte)((val >> 8) & 255);
			data[offset + 2] = (byte)((val >> 16) & 255);
			data[offset + 3] = (byte)((val >> 24) & 255);
			return offset + 4;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00007138 File Offset: 0x00005338
		internal static object ReadObjectFromByteArray(byte[] keyData, int dataIndex)
		{
			try
			{
				SerializationUtilities.ObjectType objectType = (SerializationUtilities.ObjectType)keyData[dataIndex];
				dataIndex++;
				switch (objectType)
				{
				case SerializationUtilities.ObjectType.AsciiString:
				{
					int num = BitConverter.ToInt32(keyData, dataIndex);
					return Encoding.ASCII.GetString(keyData, dataIndex + 4, num);
				}
				case SerializationUtilities.ObjectType.UnicodeString:
				{
					int num2 = BitConverter.ToInt32(keyData, dataIndex);
					return Encoding.Unicode.GetString(keyData, dataIndex + 4, num2);
				}
				case SerializationUtilities.ObjectType.UInt16:
					return BitConverter.ToUInt16(keyData, dataIndex);
				case SerializationUtilities.ObjectType.UInt32:
					return BitConverter.ToUInt32(keyData, dataIndex);
				case SerializationUtilities.ObjectType.Int32:
					return BitConverter.ToInt32(keyData, dataIndex);
				case SerializationUtilities.ObjectType.Hash128:
					return Hash128.Parse(Encoding.ASCII.GetString(keyData, dataIndex + 1, (int)keyData[dataIndex]));
				case SerializationUtilities.ObjectType.Type:
					return Type.GetTypeFromCLSID(new Guid(Encoding.ASCII.GetString(keyData, dataIndex + 1, (int)keyData[dataIndex])));
				case SerializationUtilities.ObjectType.JsonObject:
				{
					int num3 = (int)keyData[dataIndex];
					dataIndex++;
					string @string = Encoding.ASCII.GetString(keyData, dataIndex, num3);
					dataIndex += num3;
					int num4 = (int)keyData[dataIndex];
					dataIndex++;
					string string2 = Encoding.ASCII.GetString(keyData, dataIndex, num4);
					dataIndex += num4;
					int num5 = BitConverter.ToInt32(keyData, dataIndex);
					dataIndex += 4;
					string string3 = Encoding.Unicode.GetString(keyData, dataIndex, num5);
					Type type = Assembly.Load(@string).GetType(string2);
					return JsonUtility.FromJson(string3, type);
				}
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			return null;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x000072CC File Offset: 0x000054CC
		internal static int WriteObjectToByteList(object obj, List<byte> buffer)
		{
			Type type = obj.GetType();
			if (type == typeof(string))
			{
				string text = obj as string;
				if (text == null)
				{
					text = string.Empty;
				}
				byte[] bytes = Encoding.Unicode.GetBytes(text);
				byte[] bytes2 = Encoding.ASCII.GetBytes(text);
				if (Encoding.Unicode.GetString(bytes) == Encoding.ASCII.GetString(bytes2))
				{
					buffer.Add(0);
					buffer.AddRange(BitConverter.GetBytes(bytes2.Length));
					buffer.AddRange(bytes2);
					return bytes2.Length + 5;
				}
				buffer.Add(1);
				buffer.AddRange(BitConverter.GetBytes(bytes.Length));
				buffer.AddRange(bytes);
				return bytes.Length + 5;
			}
			else
			{
				if (type == typeof(uint))
				{
					byte[] bytes3 = BitConverter.GetBytes((uint)obj);
					buffer.Add(3);
					buffer.AddRange(bytes3);
					return bytes3.Length + 1;
				}
				if (type == typeof(ushort))
				{
					byte[] bytes4 = BitConverter.GetBytes((ushort)obj);
					buffer.Add(2);
					buffer.AddRange(bytes4);
					return bytes4.Length + 1;
				}
				if (type == typeof(int))
				{
					byte[] bytes5 = BitConverter.GetBytes((int)obj);
					buffer.Add(4);
					buffer.AddRange(bytes5);
					return bytes5.Length + 1;
				}
				if (type == typeof(Hash128))
				{
					Hash128 hash = (Hash128)obj;
					byte[] bytes6 = Encoding.ASCII.GetBytes(hash.ToString());
					buffer.Add(5);
					buffer.Add((byte)bytes6.Length);
					buffer.AddRange(bytes6);
					return bytes6.Length + 2;
				}
				if (type == typeof(Type))
				{
					byte[] array = type.GUID.ToByteArray();
					buffer.Add(6);
					buffer.Add((byte)array.Length);
					buffer.AddRange(array);
					return array.Length + 2;
				}
				if (type.GetCustomAttributes(typeof(SerializableAttribute), true).Length == 0)
				{
					return 0;
				}
				int num = 0;
				buffer.Add(7);
				int num2 = num + 1;
				byte[] bytes7 = Encoding.ASCII.GetBytes(type.Assembly.FullName);
				buffer.Add((byte)bytes7.Length);
				int num3 = num2 + 1;
				buffer.AddRange(bytes7);
				int num4 = num3 + bytes7.Length;
				string text2 = type.FullName;
				if (text2 == null)
				{
					text2 = string.Empty;
				}
				byte[] bytes8 = Encoding.ASCII.GetBytes(text2);
				buffer.Add((byte)bytes8.Length);
				int num5 = num4 + 1;
				buffer.AddRange(bytes8);
				int num6 = num5 + bytes8.Length;
				byte[] bytes9 = Encoding.Unicode.GetBytes(JsonUtility.ToJson(obj));
				buffer.AddRange(BitConverter.GetBytes(bytes9.Length));
				int num7 = num6 + 4;
				buffer.AddRange(bytes9);
				return num7 + bytes9.Length;
			}
		}

		// Token: 0x02000052 RID: 82
		internal enum ObjectType
		{
			// Token: 0x0400010C RID: 268
			AsciiString,
			// Token: 0x0400010D RID: 269
			UnicodeString,
			// Token: 0x0400010E RID: 270
			UInt16,
			// Token: 0x0400010F RID: 271
			UInt32,
			// Token: 0x04000110 RID: 272
			Int32,
			// Token: 0x04000111 RID: 273
			Hash128,
			// Token: 0x04000112 RID: 274
			Type,
			// Token: 0x04000113 RID: 275
			JsonObject
		}
	}
}
