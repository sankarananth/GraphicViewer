using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JsonReader
{
	public partial class Form1 : Form
	{

		public Form1()
		{
			InitializeComponent();
			CenterToScreen();
			BackColor = Color.DarkBlue;
		}

		private void ReadJson_Click(object sender, EventArgs e)
		{
			int size = -1;
			OpenFileDialog openFileDialog1 = new OpenFileDialog();
			openFileDialog1.Filter = "Json files (*.json) | *.json";
			openFileDialog1.Multiselect = false;
			DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
			if (result == DialogResult.OK) // Test result.
			{
				string file = openFileDialog1.FileName;

				try
				{
					string text = File.ReadAllText(file);
					size = text.Length;
					ReadJsonContent(file);
				}
				catch (IOException)
				{
				}
			}
			Console.WriteLine(size); // <-- Shows file size in debugging mode.
			Console.WriteLine(result); // <-- For debugging use.
		}
		private void ReadJsonContent(string path)
		{
			try
			{
				using (StreamReader r = new StreamReader(path))
				{
					string json = r.ReadToEnd();
					var shapes = JsonConvert.DeserializeObject<List<Shapes>>(json, new ShapesConverter());
					shapes.ForEach(shape =>
					{
						DrawShape(shape);
					});

					if (shapes.Count == 0)
						MessageBox.Show("No shape in the read json file !");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void DrawShape<T>(T shape) where T : Shapes
		{
			switch (shape.Type)
			{
				case ShapesEnum.Line:
					Line line = JsonConvert.DeserializeObject<Line>(JsonConvert.SerializeObject(shape));
					var colorLine = GetColor(line.Color.Split(';'));
					Pen pen = new Pen(colorLine);
					pen.Width = 2;
					System.Drawing.Graphics lineGraphics;
					lineGraphics = this.CreateGraphics();
					line.PointA = line.PointA.Replace(',', '.');
					line.PointB = line.PointB.Replace(',', '.');
					var pointA=line.PointA.Split(';');
					var pointB = line.PointB.Split(';');
					Point topLeft = new Point()
					{
						X = (this.ClientRectangle.Width) / 2,
						Y = (this.ClientRectangle.Height) / 2
					};
					lineGraphics.DrawLine(pen, topLeft.X-Convert.ToInt32(float.Parse(pointA[0])), topLeft.Y - Convert.ToInt32(float.Parse(pointA[1])), topLeft.X - Convert.ToInt32(float.Parse(pointB[0])), topLeft.Y - Convert.ToInt32(float.Parse(pointB[1])));
					break;
				case ShapesEnum.Circle:
					Circle circle = JsonConvert.DeserializeObject<Circle>(JsonConvert.SerializeObject(shape));
					var colorCircle = GetColor(circle.Color.Split(';'));
					System.Drawing.Pen myPen = new System.Drawing.Pen(colorCircle);
					System.Drawing.Graphics formGraphics;
					formGraphics = this.CreateGraphics();
					var centerPoints = circle.Center.Split(';');
					DrawCircle(formGraphics, myPen, float.Parse(centerPoints[0]), float.Parse(centerPoints[1]), float.Parse(circle.Radius.ToString()));
					if(circle.Filled)
					{
						Brush brush = new SolidBrush(colorCircle);
						FillCircle(formGraphics, brush, float.Parse(centerPoints[0]), float.Parse(centerPoints[1]), float.Parse(circle.Radius.ToString()));
					}
						
					myPen.Dispose();
					formGraphics.Dispose();
					break;
				case ShapesEnum.Triangle:
					System.Drawing.Graphics triangleGraphics;
					triangleGraphics = this.CreateGraphics();
					Triangle triangle = JsonConvert.DeserializeObject<Triangle>(JsonConvert.SerializeObject(shape));
					triangle.PointA = triangle.PointA.Replace(',', '.');
					triangle.PointB = triangle.PointB.Replace(',', '.');
					triangle.PointC = triangle.PointC.Replace(',', '.');
					var tpointA = triangle.PointA.Split(';');
					var tpointB = triangle.PointB.Split(';');
					var tpointC = triangle.PointC.Split(';');
					Color triangleColor = GetColor(triangle.Color.Split(';'));
					 myPen = new System.Drawing.Pen(triangleColor);
					Brush triangleBrush = new SolidBrush(triangleColor);
					Point firstPoint = new Point
					{
						X=Convert.ToInt32(float.Parse(tpointA[0])),
						Y= Convert.ToInt32(float.Parse(tpointA[1]))
					};
					Point secondPoint = new Point
					{
						X = Convert.ToInt32(float.Parse(tpointB[0])),
						Y = Convert.ToInt32(float.Parse(tpointB[1]))
					};
					Point thirdPoint = new Point
					{
						X = Convert.ToInt32(float.Parse(tpointC[0])),
						Y = Convert.ToInt32(float.Parse(tpointC[1]))
					};
					Point topLeftTraingle = new Point()
					{
						X = (this.ClientRectangle.Width - 100) / 2,
						Y = (this.ClientRectangle.Height - 100) / 2
					};
					if (triangle.Filled)
						triangleGraphics.FillPolygon(triangleBrush, new Point[] { new Point(topLeftTraingle.X - firstPoint.X, topLeftTraingle.Y - firstPoint.Y), new Point(topLeftTraingle.X - secondPoint.X, topLeftTraingle.Y - secondPoint.Y), new Point(topLeftTraingle.X - thirdPoint.X, topLeftTraingle.Y - thirdPoint.Y) });
					else
						triangleGraphics.DrawPolygon(myPen, new Point[] { new Point(topLeftTraingle.X - firstPoint.X, topLeftTraingle.Y - firstPoint.Y), new Point(topLeftTraingle.X - secondPoint.X, topLeftTraingle.Y - secondPoint.Y), new Point(topLeftTraingle.X - thirdPoint.X, topLeftTraingle.Y - thirdPoint.Y) });
					break;
			}

		}
		public void DrawCircle(Graphics g, Pen pen,
								  float centerX, float centerY, float radius)
		{

			int circleDiameter = Convert.ToInt32(radius) * 2;
			Point CenterPoint = new Point()
			{
				X = this.ClientRectangle.Width / 2,
				Y = this.ClientRectangle.Height / 2
			};
			Point topLeft = new Point()
			{
				X = (this.ClientRectangle.Width - circleDiameter) / 2,
				Y = (this.ClientRectangle.Height - circleDiameter) / 2
			};
			Point topRight = new Point()
			{
				X = (this.ClientRectangle.Width + circleDiameter) / 2,
				Y = (this.ClientRectangle.Height - circleDiameter) / 2
			};
			Point bottomLeft = new Point()
			{
				X = (this.ClientRectangle.Width - circleDiameter) / 2,
				Y = (this.ClientRectangle.Height + circleDiameter) / 2
			};
			Point bottomRight = new Point()
			{
				X = (this.ClientRectangle.Width + circleDiameter) / 2,
				Y = (this.ClientRectangle.Height + circleDiameter) / 2
			};

			g.DrawEllipse(pen, topLeft.X, topLeft.Y, circleDiameter, circleDiameter);
		}
		public void FillCircle(Graphics g, Brush brush,
									 float centerX, float centerY, float radius)
		{
			int circleDiameter = Convert.ToInt32(radius) * 2;
			Point CenterPoint = new Point()
			{
				X = this.ClientRectangle.Width / 2,
				Y = this.ClientRectangle.Height / 2
			};
			Point topLeft = new Point()
			{
				X = (this.ClientRectangle.Width - circleDiameter) / 2,
				Y = (this.ClientRectangle.Height - circleDiameter) / 2
			};
			Point topRight = new Point()
			{
				X = (this.ClientRectangle.Width + circleDiameter) / 2,
				Y = (this.ClientRectangle.Height - circleDiameter) / 2
			};
			Point bottomLeft = new Point()
			{
				X = (this.ClientRectangle.Width - circleDiameter) / 2,
				Y = (this.ClientRectangle.Height + circleDiameter) / 2
			};
			Point bottomRight = new Point()
			{
				X = (this.ClientRectangle.Width + circleDiameter) / 2,
				Y = (this.ClientRectangle.Height + circleDiameter) / 2
			};

			g.FillEllipse(brush, topLeft.X, topLeft.Y, circleDiameter, circleDiameter);
		}
		public Color GetColor(string[] colorArray)
		{
			var a = int.Parse(colorArray[0]);
			var r = int.Parse(colorArray[1]);
			var b = int.Parse(colorArray[2]);
			var g = int.Parse(colorArray[3]);

			return Color.FromArgb(a, r, b, g);
		}
	}


}

