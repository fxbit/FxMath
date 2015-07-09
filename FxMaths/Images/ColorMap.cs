using FxMaths.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxMaths.Images
{
    public enum ColorMapDefaults
    {
        Jet,
        Gray,
        Hot,
        Bones,
        Sepia,
        Copper,
        HSV,
        DeepBlue,
        RGB332
    }

    public class ColorMap
    {
        byte[][] colorMap;
        
        #region Get/Set

        /// <summary>
        /// Set/Get internal values.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public byte this[int id, int color]
        {
            get
            {
                return this.colorMap[id][color];
            }

            set
            {
                this.colorMap[id][color] = value;
            }
        }

        #endregion
        

        #region Constructions

        public ColorMap()
        {
            colorMap = new byte[256][];
            for (int i = 0; i < 256; i++)
            {
                colorMap[i] = new byte[3];
                colorMap[i][0] = (byte)i;
                colorMap[i][1] = (byte)i;
                colorMap[i][2] = (byte)i;
            }
        }



        public ColorMap(ColorMapDefaults map)
        {
            colorMap = new byte[256][];
            switch (map)
            {
                case ColorMapDefaults.Jet:
                    {
                        #region Jet
#if false
                        float[]r_range = { 0, 0.376f, 0.627f, 0.878f, 1 };
                        float[]r_points = { 0, 0.016f, 1, 0.984f, 0.500f };
                        float[]g_range = { 0, 0.125f, 0.376f, 0.627f, 0.878f, 1 };
                        float[]g_points = { 0, 0.016f, 1, 0.984f, 0, 0 };
                        float[]b_range = { 0, 0.125f, 0.376f, 0.627f, 1 };
                        float[]b_points = { 0.516f, 1, 0.984f, 0, 0 };
                        FxVectorF r = FxVectorF.LinSpace(r_points, r_range, 256);
                        FxVectorF g = FxVectorF.LinSpace(g_points, g_range, 256);
                        FxVectorF b = FxVectorF.LinSpace(b_points, b_range, 256);

#else
                        float[] r_points = { 0, 0, 0, 1, 1, 0.5f };
                        float[] g_points = { 0, 0, 1, 1, 0, 0 };
                        float[] b_points = { 0.5f, 1, 1, 0, 0, 0 };
                        FxVectorF r = FxVectorF.LinSpace(r_points, 256);
                        FxVectorF g = FxVectorF.LinSpace(g_points, 256);
                        FxVectorF b = FxVectorF.LinSpace(b_points, 256);
#endif

                        for (int i = 0; i < 256; i++)
                        {
                            colorMap[i] = new byte[3];
                            colorMap[i][0] = (byte)(r[i] * 255);
                            colorMap[i][1] = (byte)(g[i] * 255);
                            colorMap[i][2] = (byte)(b[i] * 255);
                        }
                        #endregion
                    }
                    break;
                case ColorMapDefaults.Gray:
                    {
                        #region Gray
                        for (int i = 0; i < 256; i++)
                        {
                            colorMap[i] = new byte[3];
                            colorMap[i][0] = (byte)i;
                            colorMap[i][1] = (byte)i;
                            colorMap[i][2] = (byte)i;
                        }
                        #endregion
                    }
                    break;
                case ColorMapDefaults.Hot:
                    {
                        #region Hot
                        float[] r_points = { 0, 1, 1, 1 };
                        float[] g_points = { 0, 0, 1, 1 };
                        float[] b_points = { 0, 0, 0, 1 };
                        FxVectorF r = FxVectorF.LinSpace(r_points, 256);
                        FxVectorF g = FxVectorF.LinSpace(g_points, 256);
                        FxVectorF b = FxVectorF.LinSpace(b_points, 256);

                        for (int i = 0; i < 256; i++)
                        {
                            colorMap[i] = new byte[3];
                            colorMap[i][0] = (byte)(r[i] * 255);
                            colorMap[i][1] = (byte)(g[i] * 255);
                            colorMap[i][2] = (byte)(b[i] * 255);
                        }
                        #endregion
                    }
                    break;
                case ColorMapDefaults.Bones:
                    {
                        #region Bones
                        float[] r_range = { 0, 0.753f, 1 };
                        float[] r_points = { 0, 0.661f, 1 };
                        float[] g_range = { 0, 0.376f, 0.753f, 1 };
                        float[] g_points = { 0, 0.331f, 0.784f, 1 };
                        float[] b_range = { 0, 0.376f, 1 };
                        float[] b_points = { 0.001f, 0.454f, 1 };
                        FxVectorF r = FxVectorF.LinSpace(r_points, r_range, 256);
                        FxVectorF g = FxVectorF.LinSpace(g_points, g_range, 256);
                        FxVectorF b = FxVectorF.LinSpace(b_points, b_range, 256);

                        for (int i = 0; i < 256; i++)
                        {
                            colorMap[i] = new byte[3];
                            colorMap[i][0] = (byte)(r[i] * 255);
                            colorMap[i][1] = (byte)(g[i] * 255);
                            colorMap[i][2] = (byte)(b[i] * 255);
                        }
                        #endregion
                    }
                    break;
                case ColorMapDefaults.Copper:
                    {
                        #region Copper
                        float[] r_range = { 0, 0.804f, 1 };
                        float[] r_points = { 0, 1, 1 };
                        float[] g_range = { 0, 1 };
                        float[] g_points = { 0, 0.781f };
                        float[] b_range = { 0, 1 };
                        float[] b_points = { 0, 0.497f };
                        FxVectorF r = FxVectorF.LinSpace(r_points, r_range, 256);
                        FxVectorF g = FxVectorF.LinSpace(g_points, g_range, 256);
                        FxVectorF b = FxVectorF.LinSpace(b_points, b_range, 256);

                        for (int i = 0; i < 256; i++)
                        {
                            colorMap[i] = new byte[3];
                            colorMap[i][0] = (byte)(r[i] * 255);
                            colorMap[i][1] = (byte)(g[i] * 255);
                            colorMap[i][2] = (byte)(b[i] * 255);
                        }
                        #endregion
                    }
                    break;
                case ColorMapDefaults.Sepia:
                    {
                        #region Sepia
                        float[] r_points = { 0, 0.753f, 1 };
                        float[] g_points = { 0, 0.376f, 0.753f, 1 };
                        float[] b_points = { 0, 0.376f, 1 };
                        FxVectorF r = FxVectorF.LinSpace(r_points, 256);
                        FxVectorF g = FxVectorF.LinSpace(g_points, 256);
                        FxVectorF b = FxVectorF.LinSpace(b_points, 256);

                        for (int i = 0; i < 256; i++)
                        {
                            colorMap[i] = new byte[3];
                            colorMap[i][0] = (byte)(r[i] * 255);
                            colorMap[i][1] = (byte)(g[i] * 255);
                            colorMap[i][2] = (byte)(b[i] * 255);
                        }
                        #endregion
                    }
                    break;
                case ColorMapDefaults.HSV:
                    {
                        #region HSV
                        float[] r_range = { 0f, 0.169f, 0.173f, 0.337f, 0.341f, 0.671f, 0.675f, 0.839f, 0.843f, 1 };
                        float[] r_points = { 1f, 0.992f, 0.969f, 0f, 0f, 0.008f, 0.031f, 1f, 1f, 1 };
                        float[] g_range = { 0f, 0.169f, 0.173f, 0.506f, 0.671f, 0.675f, 1 };
                        float[] g_points = { 0f, 1f, 1f, 0.977f, 0f, 0f, 0 };
                        float[] b_range = { 0f, 0.337f, 0.341f, 0.506f, 0.839f, 0.843f, 1 };
                        float[] b_points = { 0f, 0.016f, 0.039f, 1f, 0.984f, 0.961f, 0.023f };
                        FxVectorF r = FxVectorF.LinSpace(r_points, r_range, 256);
                        FxVectorF g = FxVectorF.LinSpace(g_points, g_range, 256);
                        FxVectorF b = FxVectorF.LinSpace(b_points, b_range, 256);

                        for (int i = 0; i < 256; i++)
                        {
                            colorMap[i] = new byte[3];
                            colorMap[i][0] = (byte)(r[i] * 255);
                            colorMap[i][1] = (byte)(g[i] * 255);
                            colorMap[i][2] = (byte)(b[i] * 255);
                        }
                        #endregion
                    }
                    break;
                case ColorMapDefaults.DeepBlue:
                    {
                        #region DeepBlue
                        float[] r_range = { 0, 0.75f, 1 };
                        float[] r_points = { 0, 0, 1 };
                        float[] g_range = { 0, 0.375f, 0.75f, 1 };
                        float[] g_points = { 0, 0, 1, 1 };
                        float[] b_range = { 0, 0.375f, 1 };
                        float[] b_points = { 0, 1, 1 };
                        FxVectorF r = FxVectorF.LinSpace(r_points, r_range, 256);
                        FxVectorF g = FxVectorF.LinSpace(g_points, g_range, 256);
                        FxVectorF b = FxVectorF.LinSpace(b_points, b_range, 256);

                        for (int i = 0; i < 256; i++)
                        {
                            colorMap[i] = new byte[3];
                            colorMap[i][0] = (byte)(r[i] * 255);
                            colorMap[i][1] = (byte)(g[i] * 255);
                            colorMap[i][2] = (byte)(b[i] * 255);
                        }
                        #endregion
                    }
                    break;

                case ColorMapDefaults.RGB332:
                    {
                        #region RGB332
                        for (int i = 0; i < 256; i++)
                        {
                            colorMap[i] = new byte[3];
                            colorMap[i][0] = (byte)(i & 0xE0);
                            colorMap[i][1] = (byte)((i & 0x1C) << 3);
                            colorMap[i][2] = (byte)((i & 0x03) << 6);
                        }
                        #endregion
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion



        private static Dictionary<ColorMapDefaults, ColorMap> cacheColorMap = new Dictionary<ColorMapDefaults, ColorMap>();
        public static ColorMap GetColorMap(ColorMapDefaults colorMap)
        {
            if (cacheColorMap.ContainsKey(colorMap))
                return cacheColorMap[colorMap];

            var value = new ColorMap(colorMap);
            cacheColorMap.Add(colorMap, value);
            return value;
        }
    }
}
