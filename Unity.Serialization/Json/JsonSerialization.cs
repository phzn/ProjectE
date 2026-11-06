using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Properties;

namespace Unity.Serialization.Json
{
	// Token: 0x0200002D RID: 45
	public static class JsonSerialization
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x00003650 File Offset: 0x00001850
		private static SerializedObjectReaderConfiguration GetDefaultConfigurationForString(string json, JsonSerializationParameters parameters = default(JsonSerializationParameters))
		{
			SerializedObjectReaderConfiguration @default = SerializedObjectReaderConfiguration.Default;
			@default.UseReadAsync = false;
			@default.ValidationType = (parameters.DisableValidation ? JsonValidationType.None : (parameters.Simplified ? JsonValidationType.Simple : JsonValidationType.Standard));
			@default.BlockBufferSize = math.max(json.Length * 2, 16);
			@default.TokenBufferSize = math.max(json.Length / 2, 16);
			@default.OutputBufferSize = math.max(json.Length * 2, 16);
			@default.StripStringEscapeCharacters = parameters.StringEscapeHandling;
			return @default;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000036DC File Offset: 0x000018DC
		private static SerializedObjectReaderConfiguration GetDefaultConfigurationForFile(FileInfo file, JsonSerializationParameters parameters = default(JsonSerializationParameters))
		{
			SerializedObjectReaderConfiguration @default = SerializedObjectReaderConfiguration.Default;
			if (!file.Exists)
			{
				throw new FileNotFoundException();
			}
			@default.UseReadAsync = file.Length > 524288L;
			@default.ValidationType = (parameters.DisableValidation ? JsonValidationType.None : (parameters.Simplified ? JsonValidationType.Simple : JsonValidationType.Standard));
			@default.BlockBufferSize = math.min((int)file.Length, 524288);
			@default.OutputBufferSize = math.min((int)file.Length, 1048576);
			@default.StripStringEscapeCharacters = parameters.StringEscapeHandling;
			return @default;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003774 File Offset: 0x00001974
		private static SerializedObjectReaderConfiguration GetDefaultConfigurationForStream(Stream stream, JsonSerializationParameters parameters = default(JsonSerializationParameters))
		{
			SerializedObjectReaderConfiguration @default = SerializedObjectReaderConfiguration.Default;
			@default.UseReadAsync = stream.Length > 524288L;
			@default.ValidationType = (parameters.Simplified ? JsonValidationType.Simple : JsonValidationType.Standard);
			@default.BlockBufferSize = math.min((int)stream.Length, 524288);
			@default.OutputBufferSize = math.min((int)stream.Length, 1048576);
			@default.StripStringEscapeCharacters = parameters.StringEscapeHandling;
			return @default;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000037F0 File Offset: 0x000019F0
		public static T FromJson<T>(string json, JsonSerializationParameters parameters = default(JsonSerializationParameters))
		{
			T t;
			DeserializationResult deserializationResult;
			if (!JsonSerialization.TryFromJson<T>(json, out t, out deserializationResult, parameters))
			{
				deserializationResult.Throw();
			}
			return t;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003812 File Offset: 0x00001A12
		public static bool TryFromJson<T>(string json, out T container, out DeserializationResult result, JsonSerializationParameters parameters = default(JsonSerializationParameters))
		{
			container = default(T);
			return JsonSerialization.TryFromJsonOverride<T>(json, ref container, out result, parameters);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003824 File Offset: 0x00001A24
		public static void FromJsonOverride<T>(string json, ref T container, JsonSerializationParameters parameters = default(JsonSerializationParameters))
		{
			DeserializationResult deserializationResult;
			if (!JsonSerialization.TryFromJsonOverride<T>(json, ref container, out deserializationResult, parameters))
			{
				deserializationResult.Throw();
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00003844 File Offset: 0x00001A44
		public unsafe static bool TryFromJsonOverride<T>(string json, ref T container, out DeserializationResult result, JsonSerializationParameters parameters = default(JsonSerializationParameters))
		{
			if (json == null)
			{
				throw new ArgumentNullException("json");
			}
			char* ptr = json;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			SerializedObjectReader serializedObjectReader = new SerializedObjectReader(ptr, json.Length, JsonSerialization.GetDefaultConfigurationForString(json, parameters), Allocator.TempJob)
			{
				RequiresExplicitExceptionHandling = true
			};
			checked
			{
				SerializedValueView* ptr2 = stackalloc SerializedValueView[unchecked((UIntPtr)1) * (UIntPtr)sizeof(SerializedValueView)];
				new JsonSerialization.ReadJob
				{
					Reader = serializedObjectReader,
					View = ptr2
				}.Run<JsonSerialization.ReadJob>();
				try
				{
					serializedObjectReader.CheckAndThrowInvalidJsonException();
				}
				catch (Exception ex)
				{
					result = JsonSerialization.CreateResult(new List<DeserializationEvent>
					{
						new DeserializationEvent(EventType.Exception, ex)
					});
					return false;
				}
				bool flag = JsonSerialization.TryFromJson<T>(*ptr2, ref container, out result, parameters);
				serializedObjectReader.Dispose();
				return flag;
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00003914 File Offset: 0x00001B14
		public static T FromJson<T>(FileInfo file, JsonSerializationParameters parameters = default(JsonSerializationParameters))
		{
			T t;
			DeserializationResult deserializationResult;
			if (!JsonSerialization.TryFromJson<T>(file, out t, out deserializationResult, parameters))
			{
				deserializationResult.Throw();
			}
			return t;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00003936 File Offset: 0x00001B36
		public static bool TryFromJson<T>(FileInfo file, out T container, out DeserializationResult result, JsonSerializationParameters parameters = default(JsonSerializationParameters))
		{
			container = default(T);
			return JsonSerialization.TryFromJsonOverride<T>(file, ref container, out result, parameters);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00003948 File Offset: 0x00001B48
		public static void FromJsonOverride<T>(FileInfo file, ref T container, JsonSerializationParameters parameters = default(JsonSerializationParameters))
		{
			DeserializationResult deserializationResult;
			if (!JsonSerialization.TryFromJsonOverride<T>(file, ref container, out deserializationResult, parameters))
			{
				deserializationResult.Throw();
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00003968 File Offset: 0x00001B68
		public unsafe static bool TryFromJsonOverride<T>(FileInfo file, ref T container, out DeserializationResult result, JsonSerializationParameters parameters = default(JsonSerializationParameters))
		{
			checked
			{
				bool flag;
				using (SerializedObjectReader serializedObjectReader = new SerializedObjectReader(file.FullName, JsonSerialization.GetDefaultConfigurationForFile(file, parameters), Allocator.TempJob)
				{
					RequiresExplicitExceptionHandling = true
				})
				{
					SerializedValueView* ptr = stackalloc SerializedValueView[unchecked((UIntPtr)1) * (UIntPtr)sizeof(SerializedValueView)];
					new JsonSerialization.ReadJob
					{
						Reader = serializedObjectReader,
						View = ptr
					}.Run<JsonSerialization.ReadJob>();
					try
					{
						serializedObjectReader.CheckAndThrowInvalidJsonException();
					}
					catch (Exception ex)
					{
						result = JsonSerialization.CreateResult(new List<DeserializationEvent>
						{
							new DeserializationEvent(EventType.Exception, ex)
						});
						return false;
					}
					flag = JsonSerialization.TryFromJson<T>(*ptr, ref container, out result, parameters);
				}
				return flag;
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00003A2C File Offset: 0x00001C2C
		public static T FromJson<T>(Stream stream, JsonSerializationParameters parameters = default(JsonSerializationParameters))
		{
			T t;
			DeserializationResult deserializationResult;
			if (!JsonSerialization.TryFromJson<T>(stream, out t, out deserializationResult, parameters))
			{
				deserializationResult.Throw();
			}
			return t;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00003A50 File Offset: 0x00001C50
		public static bool TryFromJson<T>(Stream stream, out T container, out DeserializationResult result, JsonSerializationParameters parameters = default(JsonSerializationParameters))
		{
			container = default(T);
			return JsonSerialization.TryFromJsonOverride<T>(stream, ref container, out result, default(JsonSerializationParameters));
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00003A78 File Offset: 0x00001C78
		public static void FromJsonOverride<T>(Stream stream, ref T container, JsonSerializationParameters parameters = default(JsonSerializationParameters))
		{
			DeserializationResult deserializationResult;
			if (!JsonSerialization.TryFromJsonOverride<T>(stream, ref container, out deserializationResult, parameters))
			{
				deserializationResult.Throw();
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00003A98 File Offset: 0x00001C98
		public unsafe static bool TryFromJsonOverride<T>(Stream stream, ref T container, out DeserializationResult result, JsonSerializationParameters parameters = default(JsonSerializationParameters))
		{
			checked
			{
				bool flag;
				using (SerializedObjectReader serializedObjectReader = new SerializedObjectReader(stream, JsonSerialization.GetDefaultConfigurationForStream(stream, parameters), Allocator.TempJob, true)
				{
					RequiresExplicitExceptionHandling = true
				})
				{
					SerializedValueView* ptr = stackalloc SerializedValueView[unchecked((UIntPtr)1) * (UIntPtr)sizeof(SerializedValueView)];
					new JsonSerialization.ReadJob
					{
						Reader = serializedObjectReader,
						View = ptr
					}.Run<JsonSerialization.ReadJob>();
					try
					{
						serializedObjectReader.CheckAndThrowInvalidJsonException();
					}
					catch (Exception ex)
					{
						result = JsonSerialization.CreateResult(new List<DeserializationEvent>
						{
							new DeserializationEvent(EventType.Exception, ex)
						});
						return false;
					}
					flag = JsonSerialization.TryFromJson<T>(*ptr, ref container, out result, parameters);
				}
				return flag;
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00003B58 File Offset: 0x00001D58
		public static T FromJson<T>(SerializedValueView view, JsonSerializationParameters parameters = default(JsonSerializationParameters))
		{
			T t = default(T);
			DeserializationResult deserializationResult;
			if (!JsonSerialization.TryFromJson<T>(view, ref t, out deserializationResult, parameters))
			{
				deserializationResult.Throw();
			}
			return t;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00003B84 File Offset: 0x00001D84
		public static void FromJsonOverride<T>(SerializedValueView view, ref T container, JsonSerializationParameters parameters = default(JsonSerializationParameters))
		{
			DeserializationResult deserializationResult;
			if (!JsonSerialization.TryFromJsonOverride<T>(view, ref container, out deserializationResult, parameters))
			{
				deserializationResult.Throw();
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00003BA4 File Offset: 0x00001DA4
		public static bool TryFromJson<T>(SerializedValueView view, ref T container, out DeserializationResult result, JsonSerializationParameters parameters = default(JsonSerializationParameters))
		{
			SerializedReferences serializedReferences = null;
			JsonSerializationState jsonSerializationState = parameters.State ?? (parameters.RequiresThreadSafety ? new JsonSerializationState() : JsonSerialization.GetSharedState());
			if (!parameters.DisableSerializedReferences)
			{
				serializedReferences = jsonSerializationState.GetSerializedReferences();
			}
			JsonPropertyReader jsonPropertyReader = jsonSerializationState.GetJsonPropertyReader();
			List<DeserializationEvent> deserializationEvents = jsonSerializationState.GetDeserializationEvents();
			jsonPropertyReader.SetView(view.AsUnsafe());
			jsonPropertyReader.SetSerializedType(parameters.SerializedType);
			jsonPropertyReader.SetDisableRootAdapters(parameters.DisableRootAdapters);
			jsonPropertyReader.SetGlobalAdapters(JsonSerialization.GetGlobalAdapters());
			jsonPropertyReader.SetUserDefinedAdapters(parameters.UserDefinedAdapters);
			jsonPropertyReader.SetGlobalMigrations(JsonSerialization.GetGlobalMigrations());
			jsonPropertyReader.SetUserDefinedMigrations(parameters.UserDefinedMigrations);
			jsonPropertyReader.SetUserData(parameters.UserData);
			jsonPropertyReader.SetEvents(deserializationEvents);
			jsonPropertyReader.SetSerializedReferences(serializedReferences);
			PropertyWrapper<T> propertyWrapper = new PropertyWrapper<T>(container);
			try
			{
				using (jsonPropertyReader.Lock())
				{
					PropertyContainer.Accept<PropertyWrapper<T>>(jsonPropertyReader, ref propertyWrapper, default(VisitParameters));
				}
			}
			catch (Exception ex)
			{
				deserializationEvents.Add(new DeserializationEvent(EventType.Exception, ex));
			}
			container = propertyWrapper.Value;
			result = JsonSerialization.CreateResult(deserializationEvents);
			if (parameters.State == null && serializedReferences != null)
			{
				serializedReferences.Clear();
			}
			return result.DidSucceed();
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00003CFC File Offset: 0x00001EFC
		public static bool TryFromJsonOverride<T>(SerializedValueView view, ref T container, out DeserializationResult result, JsonSerializationParameters parameters = default(JsonSerializationParameters))
		{
			return JsonSerialization.TryFromJson<T>(view, ref container, out result, parameters);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00003D08 File Offset: 0x00001F08
		private static DeserializationResult CreateResult(List<DeserializationEvent> events)
		{
			if (events.Count <= 0)
			{
				return default(DeserializationResult);
			}
			return new DeserializationResult(events.ToList<DeserializationEvent>());
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00003D33 File Offset: 0x00001F33
		public static void ToJson<T>(FileInfo file, T container, JsonSerializationParameters parameters = default(JsonSerializationParameters))
		{
			File.WriteAllText(file.FullName, JsonSerialization.ToJson<T>(container, parameters));
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00003D48 File Offset: 0x00001F48
		public static string ToJson<T>(T value, JsonSerializationParameters parameters = default(JsonSerializationParameters))
		{
			string text;
			using (JsonWriter jsonWriter = new JsonWriter(parameters.InitialCapacity, Allocator.Temp, new JsonWriterOptions
			{
				Minified = parameters.Minified,
				Simplified = parameters.Simplified,
				Indent = parameters.Indent,
				StringEscapeHandling = parameters.StringEscapeHandling
			}))
			{
				JsonSerialization.ToJson<T>(jsonWriter, value, parameters);
				text = jsonWriter.ToString();
			}
			return text;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00003DDC File Offset: 0x00001FDC
		public static void ToJson<T>(JsonWriter writer, T value, JsonSerializationParameters parameters = default(JsonSerializationParameters))
		{
			PropertyWrapper<T> propertyWrapper = new PropertyWrapper<T>(value);
			SerializedReferences serializedReferences = null;
			JsonSerializationState jsonSerializationState = parameters.State ?? (parameters.RequiresThreadSafety ? new JsonSerializationState() : JsonSerialization.GetSharedState());
			if (!parameters.DisableSerializedReferences)
			{
				serializedReferences = jsonSerializationState.GetSerializedReferences();
				SerializedReferenceVisitor serializedReferenceVisitor = jsonSerializationState.GetSerializedReferenceVisitor();
				serializedReferenceVisitor.SetSerializedReference(serializedReferences);
				PropertyContainer.Accept<PropertyWrapper<T>>(serializedReferenceVisitor, ref propertyWrapper, default(VisitParameters));
			}
			JsonPropertyWriter jsonPropertyWriter = jsonSerializationState.GetJsonPropertyWriter();
			jsonPropertyWriter.SetWriter(writer);
			jsonPropertyWriter.SetSerializedType(parameters.SerializedType);
			jsonPropertyWriter.SetDisableRootAdapters(parameters.DisableRootAdapters);
			jsonPropertyWriter.SetGlobalAdapters(JsonSerialization.GetGlobalAdapters());
			jsonPropertyWriter.SetUserDefinedAdapters(parameters.UserDefinedAdapters);
			jsonPropertyWriter.SetGlobalMigrations(JsonSerialization.GetGlobalMigrations());
			jsonPropertyWriter.SetUserDefinedMigration(parameters.UserDefinedMigrations);
			jsonPropertyWriter.SetSerializedReferences(serializedReferences);
			using (jsonPropertyWriter.Lock())
			{
				PropertyContainer.Accept<PropertyWrapper<T>>(jsonPropertyWriter, ref propertyWrapper, default(VisitParameters));
			}
			if (parameters.State == null && serializedReferences != null)
			{
				serializedReferences.Clear();
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00003EF0 File Offset: 0x000020F0
		private static JsonSerializationState GetSharedState()
		{
			if (JsonSerialization.s_SharedState.IsLocked)
			{
				return new JsonSerializationState();
			}
			JsonSerialization.s_SharedState.ClearSerializedReferences();
			JsonSerialization.s_SharedState.ClearDeserializationEvents();
			return JsonSerialization.s_SharedState;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00003F1D File Offset: 0x0000211D
		public static void AddGlobalAdapter(IJsonAdapter adapter)
		{
			if (JsonSerialization.s_Adapters.Contains(adapter))
			{
				throw new ArgumentException("IJsonAdapter has already been registered.");
			}
			JsonSerialization.s_Adapters.Add(adapter);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00003F42 File Offset: 0x00002142
		public static void RemoveGlobalAdapter(IJsonAdapter adapter)
		{
			if (!JsonSerialization.s_Adapters.Contains(adapter))
			{
				throw new ArgumentException("IJsonAdapter has not been registered.");
			}
			JsonSerialization.s_Adapters.Remove(adapter);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00003F68 File Offset: 0x00002168
		public static void AddGlobalMigration(IJsonMigration migration)
		{
			if (JsonSerialization.s_Migrations.Contains(migration))
			{
				throw new ArgumentException("IJsonMigration has already been registered.");
			}
			JsonSerialization.s_Migrations.Add(migration);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00003F8D File Offset: 0x0000218D
		public static void RemoveGlobalMigration(IJsonMigration migration)
		{
			if (!JsonSerialization.s_Migrations.Contains(migration))
			{
				throw new ArgumentException("IJsonMigration has not been registered.");
			}
			JsonSerialization.s_Migrations.Add(migration);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00003FB2 File Offset: 0x000021B2
		private static List<IJsonAdapter> GetGlobalAdapters()
		{
			return JsonSerialization.s_Adapters;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00003FB9 File Offset: 0x000021B9
		private static List<IJsonMigration> GetGlobalMigrations()
		{
			return JsonSerialization.s_Migrations;
		}

		// Token: 0x04000047 RID: 71
		private static readonly List<IJsonAdapter> s_Adapters = new List<IJsonAdapter>();

		// Token: 0x04000048 RID: 72
		private static readonly List<IJsonMigration> s_Migrations = new List<IJsonMigration>();

		// Token: 0x04000049 RID: 73
		private static readonly JsonSerializationState s_SharedState = new JsonSerializationState();

		// Token: 0x0200002E RID: 46
		[BurstCompile(CompileSynchronously = true)]
		private struct ReadJob : IJob
		{
			// Token: 0x060000CF RID: 207 RVA: 0x00003FE0 File Offset: 0x000021E0
			public unsafe void Execute()
			{
				SerializedValueView serializedValueView;
				this.Reader.Read(out serializedValueView, NodeType.Any);
				*this.View = serializedValueView;
			}

			// Token: 0x0400004A RID: 74
			public SerializedObjectReader Reader;

			// Token: 0x0400004B RID: 75
			[NativeDisableUnsafePtrRestriction]
			public unsafe SerializedValueView* View;
		}
	}
}
