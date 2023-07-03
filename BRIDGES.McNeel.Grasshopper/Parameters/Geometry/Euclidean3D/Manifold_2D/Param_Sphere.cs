﻿using System;

using BRIDGES.McNeel.Grasshopper.Types.Geometry.Euclidean3D;

using RH_Geo = Rhino.Geometry;

using GH_Kernel = Grasshopper.Kernel;

using BRIDGES.McNeel.Grasshopper.Display;
using BRIDGES.McNeel.Grasshopper.Display.Geometry.Euclidean3D;
using System.Collections.Generic;


namespace BRIDGES.McNeel.Grasshopper.Parameters.Geometry.Euclidean3D
{
    /// <summary>
    /// A <see cref="Gh_Sphere"/> grasshopper parameter.
    /// </summary>
    public class Param_Sphere : GH_Kernel.GH_Param<Gh_Sphere>, GH_Kernel.IGH_PreviewObject
    {
        #region Properties

        /// <inheritdoc cref="GH_Kernel.IGH_PreviewObject.Hidden"/>
        public bool Hidden { get; set; }

        /// <inheritdoc cref="GH_Kernel.IGH_PreviewObject.IsPreviewCapable"/>
        public bool IsPreviewCapable
        {
            get { return true; }
        }

        /// <inheritdoc cref="GH_Kernel.IGH_PreviewObject.ClippingBox"/>
        public RH_Geo.BoundingBox ClippingBox
        {
            get { return this.Preview_ComputeClippingBox(); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="Param_Sphere"/>.
        /// </summary>
        /// <param name="category"> The name of the grasshopper library.</param>
        /// <param name="subCategory"> The name of the section containing the parameter.</param>
        public Param_Sphere(string category, string subCategory)
          : base("Sphere", "Sphere", "Contains a collection of spheres in a three-dimensional euclidean space.", category, subCategory, GH_Kernel.GH_ParamAccess.item)
        {
            this.Hidden = false;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc cref="GH_Kernel.IGH_PreviewObject.DrawViewportMeshes(GH_Kernel.IGH_PreviewArgs)"/>
        public void DrawViewportMeshes(GH_Kernel.IGH_PreviewArgs args)
        {
            /* Do Nothing */
        }

        /// <inheritdoc cref="GH_Kernel.IGH_PreviewObject.DrawViewportWires(GH_Kernel.IGH_PreviewArgs)"/>
        public void DrawViewportWires(GH_Kernel.IGH_PreviewArgs args)
        {
            // Evaluates whether the parameter is selected
            bool isSelected = this.Attributes.GetTopLevel.Selected;

            // Launch display for each point stored in the parameter.
            int dataCount = base.m_data.DataCount;

            if (dataCount != 0)
            {
                for (int i_B = 0; i_B < m_data.Branches.Count; i_B++)
                {
                    List<Gh_Sphere> branch = m_data.Branches[i_B];
                    for (int i_I = 0; i_I < branch.Count; i_I++)
                    {
                        var item = branch[i_I];
                        if (item != null)
                        {
                            Draw.Wireframe.Sphere(args.Display, item.Value, isSelected);
                        }
                    }
                }
            }
        }

        #endregion


        #region Override : GH_DocumentObject

        /// <inheritdoc cref="GH_Kernel.GH_DocumentObject.ComponentGuid"/>
        public override Guid ComponentGuid
        {
            get { return new Guid("{D245F134-2FA8-4CDE-8C93-C37968C73F45}"); }
        }

        /// <inheritdoc cref="GH_Kernel.GH_DocumentObject.Exposure"/>
        public override GH_Kernel.GH_Exposure Exposure
        {
            get { return GH_Kernel.GH_Exposure.tertiary; }
        }

        /// <inheritdoc cref="GH_Kernel.GH_DocumentObject.Icon"/>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

                // The image must be an embedded resource to be accessed in this way
                System.IO.Stream stream = assembly.GetManifestResourceStream("BRIDGES.McNeel.Grasshopper.Resources.Geometry.Euclidean3D." + "Param_Sphere.png");

                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(stream);

                return bmp;
            }
        }


        /// <inheritdoc cref="GH_Kernel.GH_DocumentObject.CreateAttributes()"/>
        public override void CreateAttributes()
        {
            m_attributes = new ParameterAttributes(this);
        }

        #endregion

        #region Override : GH_ActiveObject

        /// <inheritdoc cref="GH_Kernel.GH_ActiveObject.Locked"/>
        public override bool Locked
        {
            get { return base.Locked; }
            set
            {
                if (base.Locked != value)
                {
                    base.Locked = value;
                }
            }
        }

        #endregion
    }
}
