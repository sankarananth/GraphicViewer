using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonReader
{
	public class Line:Shapes
	{
		[JsonProperty("a")]
		public string PointA { get; set; }

		[JsonProperty("b")]
		public string PointB { get; set; }
	}
}
