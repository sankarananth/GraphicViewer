using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonReader
{
	public class Triangle:Shapes
	{
		[JsonProperty("a")]
		public string PointA { get; set; }

		[JsonProperty("b")]
		public string PointB { get; set; }

		[JsonProperty("c")]
		public string PointC { get; set; }
	}
}
