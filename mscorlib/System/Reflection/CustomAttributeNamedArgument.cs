using System;
using System.Globalization;
using Internal.Runtime.Augments;

namespace System.Reflection
{
	/// <summary>Represents a named argument of a custom attribute in the reflection-only context.</summary>
	// Token: 0x020008DD RID: 2269
	public struct CustomAttributeNamedArgument
	{
		// Token: 0x06004B8F RID: 19343 RVA: 0x000F0A35 File Offset: 0x000EEC35
		internal CustomAttributeNamedArgument(Type attributeType, string memberName, bool isField, CustomAttributeTypedArgument typedValue)
		{
			this.IsField = isField;
			this.MemberName = memberName;
			this.TypedValue = typedValue;
			this._attributeType = attributeType;
			this._lazyMemberInfo = null;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> class, which represents the specified field or property of the custom attribute, and specifies the value of the field or property.</summary>
		/// <param name="memberInfo">A field or property of the custom attribute. The new <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> object represents this member and its value.</param>
		/// <param name="value">The value of the field or property of the custom attribute.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="memberInfo" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="memberInfo" /> is not a field or property of the custom attribute.</exception>
		// Token: 0x06004B90 RID: 19344 RVA: 0x000F0A60 File Offset: 0x000EEC60
		public CustomAttributeNamedArgument(MemberInfo memberInfo, object value)
		{
			if (memberInfo == null)
			{
				throw new ArgumentNullException("memberInfo");
			}
			FieldInfo fieldInfo = memberInfo as FieldInfo;
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			Type type;
			if (fieldInfo != null)
			{
				type = fieldInfo.FieldType;
			}
			else
			{
				if (!(propertyInfo != null))
				{
					throw new ArgumentException("The member must be either a field or a property.");
				}
				type = propertyInfo.PropertyType;
			}
			this._lazyMemberInfo = memberInfo;
			this._attributeType = memberInfo.DeclaringType;
			if (value is CustomAttributeTypedArgument)
			{
				CustomAttributeTypedArgument customAttributeTypedArgument = (CustomAttributeTypedArgument)value;
				this.TypedValue = customAttributeTypedArgument;
			}
			else
			{
				this.TypedValue = new CustomAttributeTypedArgument(type, value);
			}
			this.IsField = fieldInfo != null;
			this.MemberName = memberInfo.Name;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> class, which represents the specified field or property of the custom attribute, and specifies a <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> object that describes the type and value of the field or property.</summary>
		/// <param name="memberInfo">A field or property of the custom attribute. The new <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> object represents this member and its value.</param>
		/// <param name="typedArgument">An object that describes the type and value of the field or property.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="memberInfo" /> is <see langword="null" />.</exception>
		// Token: 0x06004B91 RID: 19345 RVA: 0x000F0B14 File Offset: 0x000EED14
		public CustomAttributeNamedArgument(MemberInfo memberInfo, CustomAttributeTypedArgument typedArgument)
		{
			if (memberInfo == null)
			{
				throw new ArgumentNullException("memberInfo");
			}
			this._lazyMemberInfo = memberInfo;
			this._attributeType = memberInfo.DeclaringType;
			this.TypedValue = typedArgument;
			this.IsField = memberInfo is FieldInfo;
			this.MemberName = memberInfo.Name;
		}

		/// <summary>Gets a <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> structure that can be used to obtain the type and value of the current named argument.</summary>
		/// <returns>A structure that can be used to obtain the type and value of the current named argument.</returns>
		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x06004B92 RID: 19346 RVA: 0x000F0B6C File Offset: 0x000EED6C
		public readonly CustomAttributeTypedArgument TypedValue { get; }

		/// <summary>Gets a value that indicates whether the named argument is a field.</summary>
		/// <returns>
		///   <see langword="true" /> if the named argument is a field; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x06004B93 RID: 19347 RVA: 0x000F0B74 File Offset: 0x000EED74
		public readonly bool IsField { get; }

		/// <summary>Gets the name of the attribute member that would be used to set the named argument.</summary>
		/// <returns>The name of the attribute member that would be used to set the named argument.</returns>
		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x06004B94 RID: 19348 RVA: 0x000F0B7C File Offset: 0x000EED7C
		public readonly string MemberName { get; }

		/// <summary>Gets the attribute member that would be used to set the named argument.</summary>
		/// <returns>The attribute member that would be used to set the named argument.</returns>
		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x06004B95 RID: 19349 RVA: 0x000F0B84 File Offset: 0x000EED84
		public MemberInfo MemberInfo
		{
			get
			{
				MemberInfo memberInfo = this._lazyMemberInfo;
				if (memberInfo == null)
				{
					if (this.IsField)
					{
						memberInfo = this._attributeType.GetField(this.MemberName, BindingFlags.Instance | BindingFlags.Public);
					}
					else
					{
						memberInfo = this._attributeType.GetProperty(this.MemberName, BindingFlags.Instance | BindingFlags.Public);
					}
					if (memberInfo == null)
					{
						throw RuntimeAugments.Callbacks.CreateMissingMetadataException(this._attributeType);
					}
					this._lazyMemberInfo = memberInfo;
				}
				return memberInfo;
			}
		}

		/// <summary>Returns a value that indicates whether this instance is equal to a specified object.</summary>
		/// <param name="obj">An object to compare with this instance, or <see langword="null" />.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="obj" /> equals the type and value of this instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004B96 RID: 19350 RVA: 0x000F0BF9 File Offset: 0x000EEDF9
		public override bool Equals(object obj)
		{
			return obj == this;
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		// Token: 0x06004B97 RID: 19351 RVA: 0x000F0C09 File Offset: 0x000EEE09
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>Tests whether two <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> structures are equivalent.</summary>
		/// <param name="left">The structure to the left of the equality operator.</param>
		/// <param name="right">The structure to the right of the equality operator.</param>
		/// <returns>
		///   <see langword="true" /> if the two <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> structures are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004B98 RID: 19352 RVA: 0x000F0C1B File Offset: 0x000EEE1B
		public static bool operator ==(CustomAttributeNamedArgument left, CustomAttributeNamedArgument right)
		{
			return left.Equals(right);
		}

		/// <summary>Tests whether two <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> structures are different.</summary>
		/// <param name="left">The structure to the left of the inequality operator.</param>
		/// <param name="right">The structure to the right of the inequality operator.</param>
		/// <returns>
		///   <see langword="true" /> if the two <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> structures are different; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004B99 RID: 19353 RVA: 0x000F0C30 File Offset: 0x000EEE30
		public static bool operator !=(CustomAttributeNamedArgument left, CustomAttributeNamedArgument right)
		{
			return !left.Equals(right);
		}

		/// <summary>Returns a string that consists of the argument name, the equal sign, and a string representation of the argument value.</summary>
		/// <returns>A string that consists of the argument name, the equal sign, and a string representation of the argument value.</returns>
		// Token: 0x06004B9A RID: 19354 RVA: 0x000F0C48 File Offset: 0x000EEE48
		public override string ToString()
		{
			if (this._attributeType == null)
			{
				return base.ToString();
			}
			string text;
			try
			{
				bool flag = this._lazyMemberInfo == null || (this.IsField ? ((FieldInfo)this._lazyMemberInfo).FieldType : ((PropertyInfo)this._lazyMemberInfo).PropertyType) != typeof(object);
				text = string.Format(CultureInfo.CurrentCulture, "{0} = {1}", this.MemberName, this.TypedValue.ToString(flag));
			}
			catch (MissingMetadataException)
			{
				text = base.ToString();
			}
			return text;
		}

		// Token: 0x04002F6A RID: 12138
		private readonly Type _attributeType;

		// Token: 0x04002F6B RID: 12139
		private volatile MemberInfo _lazyMemberInfo;
	}
}
