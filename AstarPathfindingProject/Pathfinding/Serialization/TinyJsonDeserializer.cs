using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Pathfinding.Util;
using Pathfinding.WindowsStore;
using UnityEngine;

namespace Pathfinding.Serialization
{
	// Token: 0x02000253 RID: 595
	public class TinyJsonDeserializer
	{
		// Token: 0x06000ED2 RID: 3794 RVA: 0x0005B149 File Offset: 0x00059349
		public static object Deserialize(string text, Type type, object populate = null, GameObject contextRoot = null)
		{
			return new TinyJsonDeserializer
			{
				reader = new StringReader(text),
				fullTextDebug = text,
				contextRoot = contextRoot
			}.Deserialize(type, populate);
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x0005B174 File Offset: 0x00059374
		private object Deserialize(Type tp, object populate = null)
		{
			Type type = WindowsStoreCompatibility.GetTypeInfo(tp);
			if (type.IsEnum)
			{
				return Enum.Parse(tp, this.EatField());
			}
			if (this.TryEat('n'))
			{
				this.Eat("ull");
				this.TryEat(',');
				return null;
			}
			if (object.Equals(tp, typeof(float)))
			{
				return float.Parse(this.EatField(), TinyJsonDeserializer.numberFormat);
			}
			if (object.Equals(tp, typeof(int)))
			{
				return int.Parse(this.EatField(), TinyJsonDeserializer.numberFormat);
			}
			if (object.Equals(tp, typeof(uint)))
			{
				return uint.Parse(this.EatField(), TinyJsonDeserializer.numberFormat);
			}
			if (object.Equals(tp, typeof(bool)))
			{
				return bool.Parse(this.EatField());
			}
			if (object.Equals(tp, typeof(string)))
			{
				return this.EatField();
			}
			if (object.Equals(tp, typeof(Version)))
			{
				return new Version(this.EatField());
			}
			if (object.Equals(tp, typeof(Vector2)))
			{
				this.Eat("{");
				Vector2 vector = default(Vector2);
				this.EatField();
				vector.x = float.Parse(this.EatField(), TinyJsonDeserializer.numberFormat);
				this.EatField();
				vector.y = float.Parse(this.EatField(), TinyJsonDeserializer.numberFormat);
				this.Eat("}");
				return vector;
			}
			if (object.Equals(tp, typeof(Vector3)))
			{
				this.Eat("{");
				Vector3 vector2 = default(Vector3);
				this.EatField();
				vector2.x = float.Parse(this.EatField(), TinyJsonDeserializer.numberFormat);
				this.EatField();
				vector2.y = float.Parse(this.EatField(), TinyJsonDeserializer.numberFormat);
				this.EatField();
				vector2.z = float.Parse(this.EatField(), TinyJsonDeserializer.numberFormat);
				this.Eat("}");
				return vector2;
			}
			if (object.Equals(tp, typeof(Guid)))
			{
				this.Eat("{");
				this.EatField();
				Guid guid = Guid.Parse(this.EatField());
				this.Eat("}");
				return guid;
			}
			if (object.Equals(tp, typeof(LayerMask)))
			{
				this.Eat("{");
				this.EatField();
				LayerMask layerMask = int.Parse(this.EatField());
				this.Eat("}");
				return layerMask;
			}
			if (tp.IsGenericType && object.Equals(tp.GetGenericTypeDefinition(), typeof(List<>)))
			{
				IList list = (IList)Activator.CreateInstance(tp);
				Type type2 = tp.GetGenericArguments()[0];
				this.Eat("[");
				while (!this.TryEat(']'))
				{
					list.Add(this.Deserialize(type2, null));
					this.TryEat(',');
				}
				return list;
			}
			if (type.IsArray)
			{
				List<object> list2 = new List<object>();
				this.Eat("[");
				while (!this.TryEat(']'))
				{
					list2.Add(this.Deserialize(tp.GetElementType(), null));
					this.TryEat(',');
				}
				Array array = Array.CreateInstance(tp.GetElementType(), list2.Count);
				list2.ToArray().CopyTo(array, 0);
				return array;
			}
			if (typeof(Object).IsAssignableFrom(tp))
			{
				return this.DeserializeUnityObject();
			}
			this.Eat("{");
			if (type.GetCustomAttributes(typeof(JsonDynamicTypeAttribute), true).Length != 0)
			{
				string text = this.EatField();
				if (text != "@type")
				{
					throw new Exception("Expected field '@type' but found '" + text + "'\n\nWhen trying to deserialize: " + this.fullTextDebug);
				}
				string text2 = this.EatField();
				JsonDynamicTypeAliasAttribute[] array2 = type.GetCustomAttributes(typeof(JsonDynamicTypeAliasAttribute), true) as JsonDynamicTypeAliasAttribute[];
				string text3 = text2.Split(',', StringSplitOptions.None)[0];
				Type type3 = null;
				foreach (JsonDynamicTypeAliasAttribute jsonDynamicTypeAliasAttribute in array2)
				{
					if (jsonDynamicTypeAliasAttribute.alias == text3)
					{
						type3 = jsonDynamicTypeAliasAttribute.type;
					}
				}
				if (type3 == null)
				{
					type3 = Type.GetType(text2);
				}
				Type type4 = type3;
				if (type4 == null)
				{
					throw new Exception("Could not find a type with the name '" + text2 + "'\n\nWhen trying to deserialize: " + this.fullTextDebug);
				}
				tp = type4;
				type = WindowsStoreCompatibility.GetTypeInfo(tp);
			}
			object obj = populate ?? Activator.CreateInstance(tp);
			while (!this.TryEat('}'))
			{
				string text4 = this.EatField();
				Type type5 = tp;
				FieldInfo fieldInfo = null;
				while (fieldInfo == null && type5 != null)
				{
					fieldInfo = type5.GetField(text4, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					type5 = type5.BaseType;
				}
				if (fieldInfo == null)
				{
					PropertyInfo propertyInfo = null;
					type5 = tp;
					while (propertyInfo == null && type5 != null)
					{
						propertyInfo = type5.GetProperty(text4, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						type5 = type5.BaseType;
					}
					if (propertyInfo == null)
					{
						this.SkipFieldData();
					}
					else
					{
						propertyInfo.SetValue(obj, this.Deserialize(propertyInfo.PropertyType, null));
					}
				}
				else
				{
					fieldInfo.SetValue(obj, this.Deserialize(fieldInfo.FieldType, null));
				}
				this.TryEat(',');
			}
			return obj;
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x0005B6DB File Offset: 0x000598DB
		private Object DeserializeUnityObject()
		{
			this.Eat("{");
			Object @object = this.DeserializeUnityObjectInner();
			this.Eat("}");
			return @object;
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x0005B6FC File Offset: 0x000598FC
		private Object DeserializeUnityObjectInner()
		{
			string text = this.EatField();
			if (text == "InstanceID")
			{
				this.EatField();
				text = this.EatField();
			}
			if (text != "Name")
			{
				throw new Exception("Expected 'Name' field");
			}
			string text2 = this.EatField();
			if (text2 == null)
			{
				return null;
			}
			if (this.EatField() != "Type")
			{
				throw new Exception("Expected 'Type' field");
			}
			string text3 = this.EatField();
			if (text3.IndexOf(',') != -1)
			{
				text3 = text3.Substring(0, text3.IndexOf(','));
			}
			Type type = WindowsStoreCompatibility.GetTypeInfo(typeof(AstarPath)).Assembly.GetType(text3);
			type = type ?? WindowsStoreCompatibility.GetTypeInfo(typeof(Transform)).Assembly.GetType(text3);
			if (object.Equals(type, null))
			{
				Debug.LogError("Could not find type '" + text3 + "'. Cannot deserialize Unity reference");
				return null;
			}
			this.EatWhitespace();
			if ((ushort)this.reader.Peek() == 34)
			{
				if (this.EatField() != "GUID")
				{
					throw new Exception("Expected 'GUID' field");
				}
				string text4 = this.EatField();
				UnityReferenceHelper[] array;
				int i;
				if (this.contextRoot != null)
				{
					array = this.contextRoot.GetComponentsInChildren<UnityReferenceHelper>(true);
					i = 0;
					while (i < array.Length)
					{
						UnityReferenceHelper unityReferenceHelper = array[i];
						if (unityReferenceHelper.GetGUID() == text4)
						{
							if (object.Equals(type, typeof(GameObject)))
							{
								return unityReferenceHelper.gameObject;
							}
							return unityReferenceHelper.GetComponent(type);
						}
						else
						{
							i++;
						}
					}
				}
				array = UnityCompatibility.FindObjectsByTypeUnsortedWithInactive<UnityReferenceHelper>();
				i = 0;
				while (i < array.Length)
				{
					UnityReferenceHelper unityReferenceHelper2 = array[i];
					if (unityReferenceHelper2.GetGUID() == text4)
					{
						if (object.Equals(type, typeof(GameObject)))
						{
							return unityReferenceHelper2.gameObject;
						}
						return unityReferenceHelper2.GetComponent(type);
					}
					else
					{
						i++;
					}
				}
			}
			if (!string.IsNullOrEmpty(text2))
			{
				Object[] array2 = Resources.LoadAll(text2, type);
				for (int j = 0; j < array2.Length; j++)
				{
					if (array2[j].name == text2 || array2.Length == 1)
					{
						return array2[j];
					}
				}
			}
			return null;
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x0005B928 File Offset: 0x00059B28
		private void EatWhitespace()
		{
			while (char.IsWhiteSpace((char)this.reader.Peek()))
			{
				this.reader.Read();
			}
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x0005B94C File Offset: 0x00059B4C
		private void Eat(string s)
		{
			this.EatWhitespace();
			for (int i = 0; i < s.Length; i++)
			{
				char c = (char)this.reader.Read();
				if (c != s[i])
				{
					throw new Exception(string.Concat(new string[]
					{
						"Expected '",
						s[i].ToString(),
						"' found '",
						c.ToString(),
						"'\n\n...",
						this.reader.ReadLine(),
						"\n\nWhen trying to deserialize: ",
						this.fullTextDebug
					}));
				}
			}
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x0005B9F0 File Offset: 0x00059BF0
		private string EatUntil(string c, bool inString)
		{
			this.builder.Length = 0;
			bool flag = false;
			for (;;)
			{
				int num = this.reader.Peek();
				if (!flag && (ushort)num == 34)
				{
					inString = !inString;
				}
				char c2 = (char)num;
				if (num == -1)
				{
					break;
				}
				if (!flag && c2 == '\\')
				{
					flag = true;
					this.reader.Read();
				}
				else
				{
					if (!inString && c.IndexOf(c2) != -1)
					{
						goto IL_0088;
					}
					this.builder.Append(c2);
					this.reader.Read();
					flag = false;
				}
			}
			throw new Exception("Unexpected EOF\n\nWhen trying to deserialize: " + this.fullTextDebug);
			IL_0088:
			return this.builder.ToString();
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x0005BA90 File Offset: 0x00059C90
		private bool TryEat(char c)
		{
			this.EatWhitespace();
			if ((char)this.reader.Peek() == c)
			{
				this.reader.Read();
				return true;
			}
			return false;
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x0005BAB6 File Offset: 0x00059CB6
		private string EatField()
		{
			string text = this.EatUntil("\",}]", this.TryEat('"'));
			this.TryEat('"');
			this.TryEat(':');
			this.TryEat(',');
			return text;
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x0005BAE8 File Offset: 0x00059CE8
		private void SkipFieldData()
		{
			int num = 0;
			for (;;)
			{
				this.EatUntil(",{}[]", false);
				char c = (char)this.reader.Peek();
				if (c <= '[')
				{
					if (c != ',')
					{
						if (c != '[')
						{
							break;
						}
						goto IL_003E;
					}
					else if (num == 0)
					{
						goto Block_8;
					}
				}
				else
				{
					if (c != ']')
					{
						if (c == '{')
						{
							goto IL_003E;
						}
						if (c != '}')
						{
							break;
						}
					}
					num--;
					if (num < 0)
					{
						return;
					}
				}
				IL_0068:
				this.reader.Read();
				continue;
				IL_003E:
				num++;
				goto IL_0068;
			}
			goto IL_005D;
			Block_8:
			this.reader.Read();
			return;
			IL_005D:
			throw new Exception("Should not reach this part");
		}

		// Token: 0x04000B1D RID: 2845
		private TextReader reader;

		// Token: 0x04000B1E RID: 2846
		private string fullTextDebug;

		// Token: 0x04000B1F RID: 2847
		private GameObject contextRoot;

		// Token: 0x04000B20 RID: 2848
		private static readonly NumberFormatInfo numberFormat = NumberFormatInfo.InvariantInfo;

		// Token: 0x04000B21 RID: 2849
		private StringBuilder builder = new StringBuilder();
	}
}
