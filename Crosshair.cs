using Newtonsoft.Json;
using Windows.UI;

namespace MurbongCrosshair
{
    public class Crosshair
    {
        public Color Color { get; set; }
        public Color Colors { get => Color.FromArgb(Alpha, Color.R, Color.G, Color.B); }
        public byte Alpha { get; set; }
        public float Thickness { get; set; }
        public int Size { get; set; }
        public int Gap { get; set; }
        public int Outline { get; set; }
        public bool EnableOutline { get => Outline > 0; }
        public bool EnableDot { get; set; }

        public bool DotIsCircle { get; set; }

        public bool EnableTShape { get; set; }
        public void ImportSetting(string json)
        {
            try
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
                    EnableTShape = obj.EnableTShape;
                    DotIsCircle = obj.DotIsCircle;
                }
                else
                {
                    SetDefault();
                }
            }
            catch
            {
                SetDefault();
            }

        }

        private void SetDefault()
        {
            Alpha = 255;
            Color = Color.FromArgb(255, 0, 255, 0);
            Thickness = 3;
            Size = 10;
            Gap = 1;
            Outline = 0;
            EnableDot = false;
            EnableTShape = false;
            DotIsCircle = false;
        }
        public string ExportSetting()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
