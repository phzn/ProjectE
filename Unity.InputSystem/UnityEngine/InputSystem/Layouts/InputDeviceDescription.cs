using System;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.Layouts
{
	// Token: 0x02000112 RID: 274
	[Serializable]
	public struct InputDeviceDescription : IEquatable<InputDeviceDescription>
	{
		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06000FD4 RID: 4052 RVA: 0x0004D65A File Offset: 0x0004B85A
		// (set) Token: 0x06000FD5 RID: 4053 RVA: 0x0004D662 File Offset: 0x0004B862
		public string interfaceName
		{
			get
			{
				return this.m_InterfaceName;
			}
			set
			{
				this.m_InterfaceName = value;
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06000FD6 RID: 4054 RVA: 0x0004D66B File Offset: 0x0004B86B
		// (set) Token: 0x06000FD7 RID: 4055 RVA: 0x0004D673 File Offset: 0x0004B873
		public string deviceClass
		{
			get
			{
				return this.m_DeviceClass;
			}
			set
			{
				this.m_DeviceClass = value;
			}
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06000FD8 RID: 4056 RVA: 0x0004D67C File Offset: 0x0004B87C
		// (set) Token: 0x06000FD9 RID: 4057 RVA: 0x0004D684 File Offset: 0x0004B884
		public string manufacturer
		{
			get
			{
				return this.m_Manufacturer;
			}
			set
			{
				this.m_Manufacturer = value;
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06000FDA RID: 4058 RVA: 0x0004D68D File Offset: 0x0004B88D
		// (set) Token: 0x06000FDB RID: 4059 RVA: 0x0004D695 File Offset: 0x0004B895
		public string product
		{
			get
			{
				return this.m_Product;
			}
			set
			{
				this.m_Product = value;
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06000FDC RID: 4060 RVA: 0x0004D69E File Offset: 0x0004B89E
		// (set) Token: 0x06000FDD RID: 4061 RVA: 0x0004D6A6 File Offset: 0x0004B8A6
		public string serial
		{
			get
			{
				return this.m_Serial;
			}
			set
			{
				this.m_Serial = value;
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06000FDE RID: 4062 RVA: 0x0004D6AF File Offset: 0x0004B8AF
		// (set) Token: 0x06000FDF RID: 4063 RVA: 0x0004D6B7 File Offset: 0x0004B8B7
		public string version
		{
			get
			{
				return this.m_Version;
			}
			set
			{
				this.m_Version = value;
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06000FE0 RID: 4064 RVA: 0x0004D6C0 File Offset: 0x0004B8C0
		// (set) Token: 0x06000FE1 RID: 4065 RVA: 0x0004D6C8 File Offset: 0x0004B8C8
		public string capabilities
		{
			get
			{
				return this.m_Capabilities;
			}
			set
			{
				this.m_Capabilities = value;
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06000FE2 RID: 4066 RVA: 0x0004D6D4 File Offset: 0x0004B8D4
		public bool empty
		{
			get
			{
				return string.IsNullOrEmpty(this.m_InterfaceName) && string.IsNullOrEmpty(this.m_DeviceClass) && string.IsNullOrEmpty(this.m_Manufacturer) && string.IsNullOrEmpty(this.m_Product) && string.IsNullOrEmpty(this.m_Serial) && string.IsNullOrEmpty(this.m_Version) && string.IsNullOrEmpty(this.m_Capabilities);
			}
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0004D73C File Offset: 0x0004B93C
		public override string ToString()
		{
			bool flag = !string.IsNullOrEmpty(this.product);
			bool flag2 = !string.IsNullOrEmpty(this.manufacturer);
			bool flag3 = !string.IsNullOrEmpty(this.interfaceName);
			if (flag && flag2)
			{
				if (flag3)
				{
					return string.Concat(new string[] { this.manufacturer, " ", this.product, " (", this.interfaceName, ")" });
				}
				return this.manufacturer + " " + this.product;
			}
			else if (flag)
			{
				if (flag3)
				{
					return this.product + " (" + this.interfaceName + ")";
				}
				return this.product;
			}
			else if (!string.IsNullOrEmpty(this.deviceClass))
			{
				if (flag3)
				{
					return this.deviceClass + " (" + this.interfaceName + ")";
				}
				return this.deviceClass;
			}
			else if (!string.IsNullOrEmpty(this.capabilities))
			{
				string text = this.capabilities;
				if (this.capabilities.Length > 40)
				{
					text = text.Substring(0, 40) + "...";
				}
				if (flag3)
				{
					return text + " (" + this.interfaceName + ")";
				}
				return text;
			}
			else
			{
				if (flag3)
				{
					return this.interfaceName;
				}
				return "<Empty Device Description>";
			}
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x0004D894 File Offset: 0x0004BA94
		public bool Equals(InputDeviceDescription other)
		{
			return this.m_InterfaceName.InvariantEqualsIgnoreCase(other.m_InterfaceName) && this.m_DeviceClass.InvariantEqualsIgnoreCase(other.m_DeviceClass) && this.m_Manufacturer.InvariantEqualsIgnoreCase(other.m_Manufacturer) && this.m_Product.InvariantEqualsIgnoreCase(other.m_Product) && this.m_Serial.InvariantEqualsIgnoreCase(other.m_Serial) && this.m_Version.InvariantEqualsIgnoreCase(other.m_Version) && this.m_Capabilities.InvariantEqualsIgnoreCase(other.m_Capabilities);
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x0004D928 File Offset: 0x0004BB28
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is InputDeviceDescription)
			{
				InputDeviceDescription inputDeviceDescription = (InputDeviceDescription)obj;
				return this.Equals(inputDeviceDescription);
			}
			return false;
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x0004D954 File Offset: 0x0004BB54
		public override int GetHashCode()
		{
			return (((((((((((((this.m_InterfaceName != null) ? this.m_InterfaceName.GetHashCode() : 0) * 397) ^ ((this.m_DeviceClass != null) ? this.m_DeviceClass.GetHashCode() : 0)) * 397) ^ ((this.m_Manufacturer != null) ? this.m_Manufacturer.GetHashCode() : 0)) * 397) ^ ((this.m_Product != null) ? this.m_Product.GetHashCode() : 0)) * 397) ^ ((this.m_Serial != null) ? this.m_Serial.GetHashCode() : 0)) * 397) ^ ((this.m_Version != null) ? this.m_Version.GetHashCode() : 0)) * 397) ^ ((this.m_Capabilities != null) ? this.m_Capabilities.GetHashCode() : 0);
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x0004DA25 File Offset: 0x0004BC25
		public static bool operator ==(InputDeviceDescription left, InputDeviceDescription right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x0004DA2F File Offset: 0x0004BC2F
		public static bool operator !=(InputDeviceDescription left, InputDeviceDescription right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x0004DA3C File Offset: 0x0004BC3C
		public string ToJson()
		{
			return JsonUtility.ToJson(new InputDeviceDescription.DeviceDescriptionJson
			{
				@interface = this.interfaceName,
				type = this.deviceClass,
				product = this.product,
				manufacturer = this.manufacturer,
				serial = this.serial,
				version = this.version,
				capabilities = this.capabilities
			}, true);
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x0004DAB8 File Offset: 0x0004BCB8
		public static InputDeviceDescription FromJson(string json)
		{
			if (json == null)
			{
				throw new ArgumentNullException("json");
			}
			InputDeviceDescription.DeviceDescriptionJson deviceDescriptionJson = JsonUtility.FromJson<InputDeviceDescription.DeviceDescriptionJson>(json);
			return new InputDeviceDescription
			{
				interfaceName = deviceDescriptionJson.@interface,
				deviceClass = deviceDescriptionJson.type,
				product = deviceDescriptionJson.product,
				manufacturer = deviceDescriptionJson.manufacturer,
				serial = deviceDescriptionJson.serial,
				version = deviceDescriptionJson.version,
				capabilities = deviceDescriptionJson.capabilities
			};
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x0004DB40 File Offset: 0x0004BD40
		internal static bool ComparePropertyToDeviceDescriptor(string propertyName, JsonParser.JsonString propertyValue, string deviceDescriptor)
		{
			JsonParser jsonParser = new JsonParser(deviceDescriptor);
			if (!jsonParser.NavigateToProperty(propertyName))
			{
				return propertyValue.text.isEmpty;
			}
			return jsonParser.CurrentPropertyHasValueEqualTo(propertyValue);
		}

		// Token: 0x04000698 RID: 1688
		[SerializeField]
		private string m_InterfaceName;

		// Token: 0x04000699 RID: 1689
		[SerializeField]
		private string m_DeviceClass;

		// Token: 0x0400069A RID: 1690
		[SerializeField]
		private string m_Manufacturer;

		// Token: 0x0400069B RID: 1691
		[SerializeField]
		private string m_Product;

		// Token: 0x0400069C RID: 1692
		[SerializeField]
		private string m_Serial;

		// Token: 0x0400069D RID: 1693
		[SerializeField]
		private string m_Version;

		// Token: 0x0400069E RID: 1694
		[SerializeField]
		private string m_Capabilities;

		// Token: 0x02000237 RID: 567
		private struct DeviceDescriptionJson
		{
			// Token: 0x04000C35 RID: 3125
			public string @interface;

			// Token: 0x04000C36 RID: 3126
			public string type;

			// Token: 0x04000C37 RID: 3127
			public string product;

			// Token: 0x04000C38 RID: 3128
			public string serial;

			// Token: 0x04000C39 RID: 3129
			public string version;

			// Token: 0x04000C3A RID: 3130
			public string manufacturer;

			// Token: 0x04000C3B RID: 3131
			public string capabilities;
		}
	}
}
