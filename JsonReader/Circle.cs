using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonReader
{
	public class Circle:Shapes
	{
		[JsonProperty("center")]
		public string Center { get; set; }

		[JsonProperty("radius")]
		public double Radius { get; set; }
	}
}
