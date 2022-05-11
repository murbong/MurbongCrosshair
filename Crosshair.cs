using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace MurbongCrosshair
{
    public class Crosshair
    {
        public Color Color { get; set; }
        public Color Colors { get => Color.FromArgb(Alpha, Color.R, Color.G, Color.B); }
        public byte Alpha { get; set; }
        public int Thickness { get; set; }
        public int Size { get; set; }
        public int Gap { get; set; }
        public int Outline { get; set; }
        public bool EnableOutline { get => Outline > 0; }
        public bool EnableDot { get; set; }
        public void ImportSetting(string json)
        {
            if (json != null)
            {
                var obj = JsonConvert.DeserializeObject<Crosshair>(json);
                Alpha = obj.Alpha;
                Color = obj.Color;
                Thickness = obj.Thickness;
                Size = obj.Size;
                Gap = obj.Gap;
                Outline = obj.Outline;
                EnableDot = obj.EnableDot;
            }

        }
        public string ExportSetting()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
