using Cirilla.Core.Enums;
using System.Drawing;

namespace MHWAppearanceEditor.Models
{
    public class SerializableAppearanceCompat
    {
        public Gender? Gender { get; set; }

        #region Types

        public byte? EyeWidth { get; set; }
        public byte? EyeHeight { get; set; }
        public byte? SkinColorX { get; set; }
        public byte? SkinColorY { get; set; }
        public byte? Age { get; set; }
        public byte? NoseHeight { get; set; }
        public byte? MouthHeight { get; set; }

        public byte? BrowType { get; set; }
        public byte? FaceType { get; set; }
        public byte? EyeType { get; set; }
        public byte? NoseType { get; set; }
        public byte? MouthType { get; set; }
        public byte? EyebrowType { get; set; }
        public EyelashLength? EyelashLength { get; set; }
        public byte? FacialHairType { get; set; }

        public short? HairType { get; set; }
        public byte? ClothingType { get; set; }
        public byte? Voice { get; set; }
        public int? Expression { get; set; }

        #endregion

        #region Colors

        public Color? LeftEyeColor { get; set; }
        public Color? RightEyeColor { get; set; }
        public Color? EyebrowColor { get; set; }
        public Color? FacialHairColor { get; set; }

        public Color? HairColor { get; set; }
        public Color? ClothingColor { get; set; }

        #endregion

        #region Makeup

        public Color? Makeup1Color { get; set; }
        public float? Makeup1PosX { get; set; }
        public float? Makeup1PosY { get; set; }
        public float? Makeup1SizeX { get; set; }
        public float? Makeup1SizeY { get; set; }
        public float? Makeup1Glossy { get; set; }
        public float? Makeup1Metallic { get; set; }
        public int? Makeup1Type { get; set; }

        public Color? Makeup2Color { get; set; }
        public float? Makeup2PosX { get; set; }
        public float? Makeup2PosY { get; set; }
        public float? Makeup2SizeX { get; set; }
        public float? Makeup2SizeY { get; set; }
        public float? Makeup2Glossy { get; set; }
        public float? Makeup2Metallic { get; set; }
        public int? Makeup2Type { get; set; }

        public Color? Makeup3Color { get; set; }
        public float? Makeup3PosX { get; set; }
        public float? Makeup3PosY { get; set; }
        public float? Makeup3SizeX { get; set; }
        public float? Makeup3SizeY { get; set; }
        public float? Makeup3Glossy { get; set; }
        public float? Makeup3Metallic { get; set; }
        public int? Makeup3Type { get; set; }

        #endregion
    }
}
