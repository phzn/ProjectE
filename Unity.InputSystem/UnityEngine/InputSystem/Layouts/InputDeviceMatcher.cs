using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.Layouts
{
	// Token: 0x02000113 RID: 275
	public struct InputDeviceMatcher : IEquatable<InputDeviceMatcher>
	{
		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06000FEC RID: 4076 RVA: 0x0004DB7E File Offset: 0x0004BD7E
		public bool empty
		{
			get
			{
				return this.m_Patterns == null;
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06000FED RID: 4077 RVA: 0x0004DB89 File Offset: 0x0004BD89
		public IEnumerable<KeyValuePair<string, object>> patterns
		{
			get
			{
				if (this.m_Patterns == null)
				{
					yield break;
				}
				int count = this.m_Patterns.Length;
				int num;
				for (int i = 0; i < count; i = num)
				{
					yield return new KeyValuePair<string, object>(this.m_Patterns[i].Key.ToString(), this.m_Patterns[i].Value);
					num = i + 1;
				}
				yield break;
			}
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x0004DB9E File Offset: 0x0004BD9E
		public InputDeviceMatcher WithInterface(string pattern, bool supportRegex = true)
		{
			return this.With(InputDeviceMatcher.kInterfaceKey, pattern, supportRegex);
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x0004DBAD File Offset: 0x0004BDAD
		public InputDeviceMatcher WithDeviceClass(string pattern, bool supportRegex = true)
		{
			return this.With(InputDeviceMatcher.kDeviceClassKey, pattern, supportRegex);
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x0004DBBC File Offset: 0x0004BDBC
		public InputDeviceMatcher WithManufacturer(string pattern, bool supportRegex = true)
		{
			return this.With(InputDeviceMatcher.kManufacturerKey, pattern, supportRegex);
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x0004DBCB File Offset: 0x0004BDCB
		public InputDeviceMatcher WithManufacturerContains(string noRegExPattern)
		{
			return this.With(InputDeviceMatcher.kManufacturerContainsKey, noRegExPattern, false);
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0004DBDA File Offset: 0x0004BDDA
		public InputDeviceMatcher WithProduct(string pattern, bool supportRegex = true)
		{
			return this.With(InputDeviceMatcher.kProductKey, pattern, supportRegex);
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x0004DBE9 File Offset: 0x0004BDE9
		public InputDeviceMatcher WithVersion(string pattern, bool supportRegex = true)
		{
			return this.With(InputDeviceMatcher.kVersionKey, pattern, supportRegex);
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0004DBF8 File Offset: 0x0004BDF8
		public InputDeviceMatcher WithCapability<TValue>(string path, TValue value)
		{
			return this.With(new InternedString(path), value, true);
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0004DC10 File Offset: 0x0004BE10
		private InputDeviceMatcher With(InternedString key, object value, bool supportRegex = true)
		{
			if (supportRegex)
			{
				string text = value as string;
				if (text != null)
				{
					double num;
					if (!text.All<char>((char ch) => char.IsLetterOrDigit(ch) || char.IsWhiteSpace(ch)) && !double.TryParse(text, out num))
					{
						value = new Regex(text, RegexOptions.IgnoreCase);
					}
				}
			}
			InputDeviceMatcher inputDeviceMatcher = this;
			ArrayHelpers.Append<KeyValuePair<InternedString, object>>(ref inputDeviceMatcher.m_Patterns, new KeyValuePair<InternedString, object>(key, value));
			return inputDeviceMatcher;
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x0004DC88 File Offset: 0x0004BE88
		public float MatchPercentage(InputDeviceDescription deviceDescription)
		{
			if (this.empty)
			{
				return 0f;
			}
			int num = this.m_Patterns.Length;
			for (int i = 0; i < num; i++)
			{
				InternedString key = this.m_Patterns[i].Key;
				object value = this.m_Patterns[i].Value;
				if (key == InputDeviceMatcher.kInterfaceKey)
				{
					if (string.IsNullOrEmpty(deviceDescription.interfaceName) || !InputDeviceMatcher.MatchSingleProperty(value, deviceDescription.interfaceName))
					{
						return 0f;
					}
				}
				else if (key == InputDeviceMatcher.kDeviceClassKey)
				{
					if (string.IsNullOrEmpty(deviceDescription.deviceClass) || !InputDeviceMatcher.MatchSingleProperty(value, deviceDescription.deviceClass))
					{
						return 0f;
					}
				}
				else if (key == InputDeviceMatcher.kManufacturerKey)
				{
					if (string.IsNullOrEmpty(deviceDescription.manufacturer) || !InputDeviceMatcher.MatchSingleProperty(value, deviceDescription.manufacturer))
					{
						return 0f;
					}
				}
				else if (key == InputDeviceMatcher.kManufacturerContainsKey)
				{
					if (string.IsNullOrEmpty(deviceDescription.manufacturer) || !InputDeviceMatcher.MatchSinglePropertyContains(value, deviceDescription.manufacturer))
					{
						return 0f;
					}
				}
				else if (key == InputDeviceMatcher.kProductKey)
				{
					if (string.IsNullOrEmpty(deviceDescription.product) || !InputDeviceMatcher.MatchSingleProperty(value, deviceDescription.product))
					{
						return 0f;
					}
				}
				else if (key == InputDeviceMatcher.kVersionKey)
				{
					if (string.IsNullOrEmpty(deviceDescription.version) || !InputDeviceMatcher.MatchSingleProperty(value, deviceDescription.version))
					{
						return 0f;
					}
				}
				else
				{
					if (string.IsNullOrEmpty(deviceDescription.capabilities))
					{
						return 0f;
					}
					JsonParser jsonParser = new JsonParser(deviceDescription.capabilities);
					if (!jsonParser.NavigateToProperty(key.ToString()) || !jsonParser.CurrentPropertyHasValueEqualTo(new JsonParser.JsonValue
					{
						type = JsonParser.JsonValueType.Any,
						anyValue = value
					}))
					{
						return 0f;
					}
				}
			}
			int numPropertiesIn = InputDeviceMatcher.GetNumPropertiesIn(deviceDescription);
			float num2 = 1f / (float)numPropertiesIn;
			return (float)num * num2;
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x0004DE98 File Offset: 0x0004C098
		private static bool MatchSingleProperty(object pattern, string value)
		{
			string text = pattern as string;
			if (text != null)
			{
				return string.Compare(text, value, StringComparison.OrdinalIgnoreCase) == 0;
			}
			Regex regex = pattern as Regex;
			return regex != null && regex.IsMatch(value);
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0004DED0 File Offset: 0x0004C0D0
		private static bool MatchSinglePropertyContains(object pattern, string value)
		{
			string text = pattern as string;
			return text != null && value.Contains(text, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x0004DEF4 File Offset: 0x0004C0F4
		private static int GetNumPropertiesIn(InputDeviceDescription description)
		{
			int num = 0;
			if (!string.IsNullOrEmpty(description.interfaceName))
			{
				num++;
			}
			if (!string.IsNullOrEmpty(description.deviceClass))
			{
				num++;
			}
			if (!string.IsNullOrEmpty(description.manufacturer))
			{
				num++;
			}
			if (!string.IsNullOrEmpty(description.product))
			{
				num++;
			}
			if (!string.IsNullOrEmpty(description.version))
			{
				num++;
			}
			if (!string.IsNullOrEmpty(description.capabilities))
			{
				num++;
			}
			return num;
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x0004DF70 File Offset: 0x0004C170
		public static InputDeviceMatcher FromDeviceDescription(InputDeviceDescription deviceDescription)
		{
			InputDeviceMatcher inputDeviceMatcher = default(InputDeviceMatcher);
			if (!string.IsNullOrEmpty(deviceDescription.interfaceName))
			{
				inputDeviceMatcher = inputDeviceMatcher.WithInterface(deviceDescription.interfaceName, false);
			}
			if (!string.IsNullOrEmpty(deviceDescription.deviceClass))
			{
				inputDeviceMatcher = inputDeviceMatcher.WithDeviceClass(deviceDescription.deviceClass, false);
			}
			if (!string.IsNullOrEmpty(deviceDescription.manufacturer))
			{
				inputDeviceMatcher = inputDeviceMatcher.WithManufacturer(deviceDescription.manufacturer, false);
			}
			if (!string.IsNullOrEmpty(deviceDescription.product))
			{
				inputDeviceMatcher = inputDeviceMatcher.WithProduct(deviceDescription.product, false);
			}
			if (!string.IsNullOrEmpty(deviceDescription.version))
			{
				inputDeviceMatcher = inputDeviceMatcher.WithVersion(deviceDescription.version, false);
			}
			return inputDeviceMatcher;
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x0004E01C File Offset: 0x0004C21C
		public override string ToString()
		{
			if (this.empty)
			{
				return "<empty>";
			}
			string text = string.Empty;
			foreach (KeyValuePair<InternedString, object> keyValuePair in this.m_Patterns)
			{
				if (text.Length > 0)
				{
					text += string.Format(",{0}={1}", keyValuePair.Key, keyValuePair.Value);
				}
				else
				{
					text += string.Format("{0}={1}", keyValuePair.Key, keyValuePair.Value);
				}
			}
			return text;
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x0004E0B0 File Offset: 0x0004C2B0
		public bool Equals(InputDeviceMatcher other)
		{
			if (this.m_Patterns == other.m_Patterns)
			{
				return true;
			}
			if (this.m_Patterns == null || other.m_Patterns == null)
			{
				return false;
			}
			if (this.m_Patterns.Length != other.m_Patterns.Length)
			{
				return false;
			}
			for (int i = 0; i < this.m_Patterns.Length; i++)
			{
				KeyValuePair<InternedString, object> keyValuePair = this.m_Patterns[i];
				bool flag = false;
				int j = 0;
				while (j < this.m_Patterns.Length)
				{
					KeyValuePair<InternedString, object> keyValuePair2 = other.m_Patterns[j];
					if (!(keyValuePair.Key != keyValuePair2.Key))
					{
						if (!keyValuePair.Value.Equals(keyValuePair2.Value))
						{
							return false;
						}
						flag = true;
						break;
					}
					else
					{
						j++;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x0004E16C File Offset: 0x0004C36C
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is InputDeviceMatcher)
			{
				InputDeviceMatcher inputDeviceMatcher = (InputDeviceMatcher)obj;
				return this.Equals(inputDeviceMatcher);
			}
			return false;
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x0004E196 File Offset: 0x0004C396
		public static bool operator ==(InputDeviceMatcher left, InputDeviceMatcher right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x0004E1A0 File Offset: 0x0004C3A0
		public static bool operator !=(InputDeviceMatcher left, InputDeviceMatcher right)
		{
			return !(left == right);
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x0004E1AC File Offset: 0x0004C3AC
		public override int GetHashCode()
		{
			if (this.m_Patterns == null)
			{
				return 0;
			}
			return this.m_Patterns.GetHashCode();
		}

		// Token: 0x0400069F RID: 1695
		private KeyValuePair<InternedString, object>[] m_Patterns;

		// Token: 0x040006A0 RID: 1696
		private static readonly InternedString kInterfaceKey = new InternedString("interface");

		// Token: 0x040006A1 RID: 1697
		private static readonly InternedString kDeviceClassKey = new InternedString("deviceClass");

		// Token: 0x040006A2 RID: 1698
		private static readonly InternedString kManufacturerKey = new InternedString("manufacturer");

		// Token: 0x040006A3 RID: 1699
		private static readonly InternedString kManufacturerContainsKey = new InternedString("manufacturerContains");

		// Token: 0x040006A4 RID: 1700
		private static readonly InternedString kProductKey = new InternedString("product");

		// Token: 0x040006A5 RID: 1701
		private static readonly InternedString kVersionKey = new InternedString("version");

		// Token: 0x02000238 RID: 568
		[Serializable]
		internal struct MatcherJson
		{
			// Token: 0x060015F2 RID: 5618 RVA: 0x00065218 File Offset: 0x00063418
			public static InputDeviceMatcher.MatcherJson FromMatcher(InputDeviceMatcher matcher)
			{
				if (matcher.empty)
				{
					return default(InputDeviceMatcher.MatcherJson);
				}
				InputDeviceMatcher.MatcherJson matcherJson = default(InputDeviceMatcher.MatcherJson);
				foreach (KeyValuePair<InternedString, object> keyValuePair in matcher.m_Patterns)
				{
					InternedString key = keyValuePair.Key;
					string text = keyValuePair.Value.ToString();
					if (key == InputDeviceMatcher.kInterfaceKey)
					{
						if (matcherJson.@interface == null)
						{
							matcherJson.@interface = text;
						}
						else
						{
							ArrayHelpers.Append<string>(ref matcherJson.interfaces, text);
						}
					}
					else if (key == InputDeviceMatcher.kDeviceClassKey)
					{
						if (matcherJson.deviceClass == null)
						{
							matcherJson.deviceClass = text;
						}
						else
						{
							ArrayHelpers.Append<string>(ref matcherJson.deviceClasses, text);
						}
					}
					else if (key == InputDeviceMatcher.kManufacturerKey)
					{
						if (matcherJson.manufacturer == null)
						{
							matcherJson.manufacturer = text;
						}
						else
						{
							ArrayHelpers.Append<string>(ref matcherJson.manufacturers, text);
						}
					}
					else if (key == InputDeviceMatcher.kProductKey)
					{
						if (matcherJson.product == null)
						{
							matcherJson.product = text;
						}
						else
						{
							ArrayHelpers.Append<string>(ref matcherJson.products, text);
						}
					}
					else if (key == InputDeviceMatcher.kVersionKey)
					{
						if (matcherJson.version == null)
						{
							matcherJson.version = text;
						}
						else
						{
							ArrayHelpers.Append<string>(ref matcherJson.versions, text);
						}
					}
					else
					{
						ArrayHelpers.Append<InputDeviceMatcher.MatcherJson.Capability>(ref matcherJson.capabilities, new InputDeviceMatcher.MatcherJson.Capability
						{
							path = key,
							value = text
						});
					}
				}
				return matcherJson;
			}

			// Token: 0x060015F3 RID: 5619 RVA: 0x000653B8 File Offset: 0x000635B8
			public InputDeviceMatcher ToMatcher()
			{
				InputDeviceMatcher inputDeviceMatcher = default(InputDeviceMatcher);
				if (!string.IsNullOrEmpty(this.@interface))
				{
					inputDeviceMatcher = inputDeviceMatcher.WithInterface(this.@interface, true);
				}
				if (this.interfaces != null)
				{
					foreach (string text in this.interfaces)
					{
						inputDeviceMatcher = inputDeviceMatcher.WithInterface(text, true);
					}
				}
				if (!string.IsNullOrEmpty(this.deviceClass))
				{
					inputDeviceMatcher = inputDeviceMatcher.WithDeviceClass(this.deviceClass, true);
				}
				if (this.deviceClasses != null)
				{
					foreach (string text2 in this.deviceClasses)
					{
						inputDeviceMatcher = inputDeviceMatcher.WithDeviceClass(text2, true);
					}
				}
				if (!string.IsNullOrEmpty(this.manufacturer))
				{
					inputDeviceMatcher = inputDeviceMatcher.WithManufacturer(this.manufacturer, true);
				}
				if (this.manufacturers != null)
				{
					foreach (string text3 in this.manufacturers)
					{
						inputDeviceMatcher = inputDeviceMatcher.WithManufacturer(text3, true);
					}
				}
				if (!string.IsNullOrEmpty(this.manufacturerContains))
				{
					inputDeviceMatcher = inputDeviceMatcher.WithManufacturerContains(this.manufacturerContains);
				}
				if (!string.IsNullOrEmpty(this.product))
				{
					inputDeviceMatcher = inputDeviceMatcher.WithProduct(this.product, true);
				}
				if (this.products != null)
				{
					foreach (string text4 in this.products)
					{
						inputDeviceMatcher = inputDeviceMatcher.WithProduct(text4, true);
					}
				}
				if (!string.IsNullOrEmpty(this.version))
				{
					inputDeviceMatcher = inputDeviceMatcher.WithVersion(this.version, true);
				}
				if (this.versions != null)
				{
					foreach (string text5 in this.versions)
					{
						inputDeviceMatcher = inputDeviceMatcher.WithVersion(text5, true);
					}
				}
				if (this.capabilities != null)
				{
					foreach (InputDeviceMatcher.MatcherJson.Capability capability in this.capabilities)
					{
						inputDeviceMatcher = inputDeviceMatcher.WithCapability<string>(capability.path, capability.value);
					}
				}
				return inputDeviceMatcher;
			}

			// Token: 0x04000C3C RID: 3132
			public string @interface;

			// Token: 0x04000C3D RID: 3133
			public string[] interfaces;

			// Token: 0x04000C3E RID: 3134
			public string deviceClass;

			// Token: 0x04000C3F RID: 3135
			public string[] deviceClasses;

			// Token: 0x04000C40 RID: 3136
			public string manufacturer;

			// Token: 0x04000C41 RID: 3137
			public string manufacturerContains;

			// Token: 0x04000C42 RID: 3138
			public string[] manufacturers;

			// Token: 0x04000C43 RID: 3139
			public string product;

			// Token: 0x04000C44 RID: 3140
			public string[] products;

			// Token: 0x04000C45 RID: 3141
			public string version;

			// Token: 0x04000C46 RID: 3142
			public string[] versions;

			// Token: 0x04000C47 RID: 3143
			public InputDeviceMatcher.MatcherJson.Capability[] capabilities;

			// Token: 0x0200028B RID: 651
			public struct Capability
			{
				// Token: 0x04000D3F RID: 3391
				public string path;

				// Token: 0x04000D40 RID: 3392
				public string value;
			}
		}
	}
}
