using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000A9 RID: 169
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	internal class JsonSchemaModelBuilder
	{
		// Token: 0x0600092C RID: 2348 RVA: 0x00025F71 File Offset: 0x00024171
		public JsonSchemaModel Build(JsonSchema schema)
		{
			this._nodes = new JsonSchemaNodeCollection();
			this._node = this.AddSchema(null, schema);
			this._nodeModels = new Dictionary<JsonSchemaNode, JsonSchemaModel>();
			return this.BuildNodeModel(this._node);
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x00025FA4 File Offset: 0x000241A4
		public JsonSchemaNode AddSchema(JsonSchemaNode existingNode, JsonSchema schema)
		{
			string text;
			if (existingNode != null)
			{
				if (existingNode.Schemas.Contains(schema))
				{
					return existingNode;
				}
				text = JsonSchemaNode.GetId(existingNode.Schemas.Union<JsonSchema>(new JsonSchema[] { schema }));
			}
			else
			{
				text = JsonSchemaNode.GetId(new JsonSchema[] { schema });
			}
			if (this._nodes.Contains(text))
			{
				return this._nodes[text];
			}
			JsonSchemaNode jsonSchemaNode = ((existingNode != null) ? existingNode.Combine(schema) : new JsonSchemaNode(schema));
			this._nodes.Add(jsonSchemaNode);
			this.AddProperties(schema.Properties, jsonSchemaNode.Properties);
			this.AddProperties(schema.PatternProperties, jsonSchemaNode.PatternProperties);
			if (schema.Items != null)
			{
				for (int i = 0; i < schema.Items.Count; i++)
				{
					this.AddItem(jsonSchemaNode, i, schema.Items[i]);
				}
			}
			if (schema.AdditionalItems != null)
			{
				this.AddAdditionalItems(jsonSchemaNode, schema.AdditionalItems);
			}
			if (schema.AdditionalProperties != null)
			{
				this.AddAdditionalProperties(jsonSchemaNode, schema.AdditionalProperties);
			}
			if (schema.Extends != null)
			{
				foreach (JsonSchema jsonSchema in schema.Extends)
				{
					jsonSchemaNode = this.AddSchema(jsonSchemaNode, jsonSchema);
				}
			}
			return jsonSchemaNode;
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x000260F8 File Offset: 0x000242F8
		public void AddProperties(IDictionary<string, JsonSchema> source, IDictionary<string, JsonSchemaNode> target)
		{
			if (source != null)
			{
				foreach (KeyValuePair<string, JsonSchema> keyValuePair in source)
				{
					this.AddProperty(target, keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x00026154 File Offset: 0x00024354
		public void AddProperty(IDictionary<string, JsonSchemaNode> target, string propertyName, JsonSchema schema)
		{
			JsonSchemaNode jsonSchemaNode;
			target.TryGetValue(propertyName, out jsonSchemaNode);
			target[propertyName] = this.AddSchema(jsonSchemaNode, schema);
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0002617C File Offset: 0x0002437C
		public void AddItem(JsonSchemaNode parentNode, int index, JsonSchema schema)
		{
			JsonSchemaNode jsonSchemaNode = ((parentNode.Items.Count > index) ? parentNode.Items[index] : null);
			JsonSchemaNode jsonSchemaNode2 = this.AddSchema(jsonSchemaNode, schema);
			if (parentNode.Items.Count <= index)
			{
				parentNode.Items.Add(jsonSchemaNode2);
				return;
			}
			parentNode.Items[index] = jsonSchemaNode2;
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x000261D8 File Offset: 0x000243D8
		public void AddAdditionalProperties(JsonSchemaNode parentNode, JsonSchema schema)
		{
			parentNode.AdditionalProperties = this.AddSchema(parentNode.AdditionalProperties, schema);
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x000261ED File Offset: 0x000243ED
		public void AddAdditionalItems(JsonSchemaNode parentNode, JsonSchema schema)
		{
			parentNode.AdditionalItems = this.AddSchema(parentNode.AdditionalItems, schema);
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x00026204 File Offset: 0x00024404
		private JsonSchemaModel BuildNodeModel(JsonSchemaNode node)
		{
			JsonSchemaModel jsonSchemaModel;
			if (this._nodeModels.TryGetValue(node, out jsonSchemaModel))
			{
				return jsonSchemaModel;
			}
			jsonSchemaModel = JsonSchemaModel.Create(node.Schemas);
			this._nodeModels[node] = jsonSchemaModel;
			foreach (KeyValuePair<string, JsonSchemaNode> keyValuePair in node.Properties)
			{
				if (jsonSchemaModel.Properties == null)
				{
					jsonSchemaModel.Properties = new Dictionary<string, JsonSchemaModel>();
				}
				jsonSchemaModel.Properties[keyValuePair.Key] = this.BuildNodeModel(keyValuePair.Value);
			}
			foreach (KeyValuePair<string, JsonSchemaNode> keyValuePair2 in node.PatternProperties)
			{
				if (jsonSchemaModel.PatternProperties == null)
				{
					jsonSchemaModel.PatternProperties = new Dictionary<string, JsonSchemaModel>();
				}
				jsonSchemaModel.PatternProperties[keyValuePair2.Key] = this.BuildNodeModel(keyValuePair2.Value);
			}
			foreach (JsonSchemaNode jsonSchemaNode in node.Items)
			{
				if (jsonSchemaModel.Items == null)
				{
					jsonSchemaModel.Items = new List<JsonSchemaModel>();
				}
				jsonSchemaModel.Items.Add(this.BuildNodeModel(jsonSchemaNode));
			}
			if (node.AdditionalProperties != null)
			{
				jsonSchemaModel.AdditionalProperties = this.BuildNodeModel(node.AdditionalProperties);
			}
			if (node.AdditionalItems != null)
			{
				jsonSchemaModel.AdditionalItems = this.BuildNodeModel(node.AdditionalItems);
			}
			return jsonSchemaModel;
		}

		// Token: 0x0400034F RID: 847
		private JsonSchemaNodeCollection _nodes = new JsonSchemaNodeCollection();

		// Token: 0x04000350 RID: 848
		private Dictionary<JsonSchemaNode, JsonSchemaModel> _nodeModels = new Dictionary<JsonSchemaNode, JsonSchemaModel>();

		// Token: 0x04000351 RID: 849
		private JsonSchemaNode _node;
	}
}
