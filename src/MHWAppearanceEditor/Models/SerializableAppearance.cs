using Cirilla.Core.Enums;
using Cirilla.Core.Interfaces;
using Cirilla.Core.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MHWAppearanceEditor.Models
{
    public class SerializableAppearance
    {
        private static ILogger log = Log.ForContext<SerializableAppearance>();

        public Gender? Gender { get; set; }

        #region Types

        public sbyte? EyeWidth { get; set; }
        public sbyte? EyeHeight { get; set; }
        public byte? SkinColorX { get; set; }
        public byte? SkinColorY { get; set; }
        public byte? Age { get; set; }
        public sbyte? NoseHeight { get; set; }
        public sbyte? MouthHeight { get; set; }

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
        public float? Makeup1Luminescent { get; set; }
        public int? Makeup1Type { get; set; }

        public Color? Makeup2Color { get; set; }
        public float? Makeup2PosX { get; set; }
        public float? Makeup2PosY { get; set; }
        public float? Makeup2SizeX { get; set; }
        public float? Makeup2SizeY { get; set; }
        public float? Makeup2Glossy { get; set; }
        public float? Makeup2Metallic { get; set; }
        public float? Makeup2Luminescent { get; set; }
        public int? Makeup2Type { get; set; }

        public Color? Makeup3Color { get; set; }
        public float? Makeup3PosX { get; set; }
        public float? Makeup3PosY { get; set; }
        public float? Makeup3SizeX { get; set; }
        public float? Makeup3SizeY { get; set; }
        public float? Makeup3Glossy { get; set; }
        public float? Makeup3Metallic { get; set; }
        public float? Makeup3Luminescent { get; set; }
        public int? Makeup3Type { get; set; }

        #endregion

        public SerializableAppearance()
        {

        }

        public SerializableAppearance(ICharacterAppearanceProperties objWithAppearanceProperties)
        {
            Type thisType = GetType();
            Type saveSlotAppearanceType = typeof(ICharacterAppearanceProperties);

            // Foreach property in this class
            foreach (var prop in thisType.GetProperties())
            {
                // Check if the saveSlot also has this property
                var saveSlotProp = saveSlotAppearanceType.GetProperty(prop.Name);

                // And if it has the same property then set __this__ classes property to the same value
                if (saveSlotProp != null)
                {
                    prop.SetValue(this, saveSlotProp.GetValue(objWithAppearanceProperties));
                }
            }
        }

        public SerializableAppearance(SerializableAppearanceCompat compat)
        {
            Type destType = GetType();
            Type sourceType = typeof(SerializableAppearanceCompat);

            // Foreach property in this class
            foreach (var prop in destType.GetProperties())
            {
                // Check if the saveSlot also has this property
                var sourceProp = sourceType.GetProperty(prop.Name);

                // And if it has the same property then set __this__ classes property to the same value
                if (sourceProp != null)
                {
                    var value = sourceProp.GetValue(compat);

                    if (prop.PropertyType == typeof(sbyte?) && sourceProp.PropertyType == typeof(byte?))
                    {
                        byte b = (byte)value;
                        sbyte? newValue;

                        if (b > sbyte.MaxValue)
                        {
                            newValue = (sbyte)(b - 256);
                        }
                        else
                        {
                            newValue = (sbyte)b;
                        }

                        log.Information($"CompatLoader changed '{prop.Name}' from '{value}' to '{newValue}'.");
                        prop.SetValue(this, newValue);
                    }
                    else
                    {
                        prop.SetValue(this, value);
                    }
                }
            }
        }

        public void ApplyToSaveSlot(SaveSlot saveSlot)
        {
            Type thisType = GetType();
            Type saveSlotAppearanceType = typeof(ICharacterAppearanceProperties);

            // Foreach property in this class
            foreach (var prop in thisType.GetProperties())
            {
                // Check if the saveSlot also has this property
                var saveSlotProp = saveSlotAppearanceType.GetProperty(prop.Name);

                // And if it has the same property then set __saveSlot__ property to the same value
                if (saveSlotProp != null)
                {
                    var value = prop.GetValue(this);

                    // Don't set if null (since all fields are optional, and we dont want to set the Gender to Male (zero) when it is not included in the JSON)
                    if (value != null)
                        saveSlotProp.SetValue(saveSlot, value);
                }
            }
        }

        public Dictionary<string, object> ToDictionary()
        {
            return GetType().GetProperties().ToDictionary(prop => prop.Name, prop => prop.GetValue(this)!);
        }
    }
}
