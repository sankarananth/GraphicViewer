using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonReader
{
	public abstract class Shapes
	{
		[JsonProperty("type")]
		public ShapesEnum Type { get; set; }

		[JsonProperty("color")]
		public string Color { get; set; }

		[JsonProperty("filled")]
		public bool Filled { get; set; }
	}
}
