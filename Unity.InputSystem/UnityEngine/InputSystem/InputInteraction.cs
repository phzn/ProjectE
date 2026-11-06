using System;
using System.ComponentModel;
using System.Reflection;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem
{
	// Token: 0x0200001A RID: 26
	internal static class InputInteraction
	{
		// Token: 0x0600012F RID: 303 RVA: 0x0000333C File Offset: 0x0000153C
		public static Type GetValueType(Type interactionType)
		{
			if (interactionType == null)
			{
				throw new ArgumentNullException("interactionType");
			}
			return TypeHelpers.GetGenericTypeArgumentFromHierarchy(interactionType, typeof(IInputInteraction<>), 0);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00003364 File Offset: 0x00001564
		public static string GetDisplayName(string interaction)
		{
			if (string.IsNullOrEmpty(interaction))
			{
				throw new ArgumentNullException("interaction");
			}
			Type type = InputInteraction.s_Interactions.LookupTypeRegistration(interaction);
			if (type == null)
			{
				return interaction;
			}
			return InputInteraction.GetDisplayName(type);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x000033A4 File Offset: 0x000015A4
		public static string GetDisplayName(Type interactionType)
		{
			if (interactionType == null)
			{
				throw new ArgumentNullException("interactionType");
			}
			DisplayNameAttribute customAttribute = interactionType.GetCustomAttribute<DisplayNameAttribute>();
			if (customAttribute != null)
			{
				return customAttribute.DisplayName;
			}
			if (interactionType.Name.EndsWith("Interaction"))
			{
				return interactionType.Name.Substring(0, interactionType.Name.Length - "Interaction".Length);
			}
			return interactionType.Name;
		}

		// Token: 0x0400007F RID: 127
		public static TypeTable s_Interactions;
	}
}
