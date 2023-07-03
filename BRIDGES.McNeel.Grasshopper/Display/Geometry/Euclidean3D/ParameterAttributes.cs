using System;
using System.Drawing;

using GH_Ker = Grasshopper.Kernel;
using GH_Gui = Grasshopper.GUI.Canvas;
using Rhino.UI;

namespace BRIDGES.McNeel.Grasshopper.Display.Geometry.Euclidean3D
{
    /// <summary>
    /// Class controling the display behaviour of <see cref="GH_Ker.GH_Param{T}"/>
    /// </summary>
    internal class ParameterAttributes : GH_Ker.Attributes.GH_FloatingParamAttributes
    {
        #region Constructors

        /// <summary>
        /// Class defining the attributes of an <see cref="GH_Ker.IGH_Param"/>
        /// </summary>
        /// <param name="param"> Parameter whose attribute to modify. </param>
        public ParameterAttributes(GH_Ker.IGH_Param param)
            : base(param)
        {
            /* Do Nothing */
        }

        #endregion

        #region Other Methods

        protected override void Render(GH_Gui.GH_Canvas canvas, Graphics graphics, GH_Gui.GH_CanvasChannel channel)
        {
            if (channel == GH_Gui.GH_CanvasChannel.Objects)
            {
                // Cache the existing style.
                GH_Gui.GH_PaletteStyle style_Normal_Standard = GH_Gui.GH_Skin.palette_normal_standard;
                GH_Gui.GH_PaletteStyle style_Hidden_Standard = GH_Gui.GH_Skin.palette_hidden_standard;
                GH_Gui.GH_PaletteStyle style_Locked_Standard = GH_Gui.GH_Skin.palette_locked_standard;

                // Swap out palette for normal, unselected components.
                GH_Gui.GH_Skin.palette_normal_standard = new GH_Gui.GH_PaletteStyle(ColourPalette.LightBlue, Color.Black, Color.Black);
                GH_Gui.GH_Skin.palette_hidden_standard = new GH_Gui.GH_PaletteStyle(ColourPalette.Blue, Color.Black, Color.Black);
                GH_Gui.GH_Skin.palette_locked_standard = new GH_Gui.GH_PaletteStyle(Color.SlateGray, Color.Black, Color.Black);

                base.Render(canvas, graphics, channel);

                // Put the original style back.
                GH_Gui.GH_Skin.palette_normal_standard = style_Normal_Standard;
                GH_Gui.GH_Skin.palette_hidden_standard = style_Hidden_Standard;
                GH_Gui.GH_Skin.palette_locked_standard = style_Locked_Standard;
            }
            else
            {
                base.Render(canvas, graphics, channel);
            }
        }

        #endregion
    }
}
