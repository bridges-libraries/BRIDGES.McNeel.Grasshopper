using System;

using Euc3D = BRIDGES.Geometry.Euclidean3D;

using RH_Geo = Rhino.Geometry;

using BRIDGES.McNeel.Rhino.Extensions.Geometry.Euclidean3D;

using GH_Kernel = Grasshopper.Kernel;
using GH_Types = Grasshopper.Kernel.Types;

using BRIDGES.McNeel.Grasshopper.Display.Geometry.Euclidean3D;


namespace BRIDGES.McNeel.Grasshopper.Types.Geometry.Euclidean3D
{
    /// <summary>
    /// Class defining a grasshopper type for an <see cref="Euc3D.Segment"/>.
    /// </summary>
    public class Gh_Segment : GH_Types.GH_Goo<Euc3D.Segment>, GH_Kernel.IGH_PreviewData
    {
        #region Properties

        /// <summary>
        /// Volume containing the data.
        /// </summary>
        public RH_Geo.BoundingBox Boundingbox
        {
            get
            {
                this.Value.StartPoint.CastTo(out RH_Geo.Point3d rh_Start);
                this.Value.EndPoint.CastTo(out RH_Geo.Point3d rh_End);

                return new RH_Geo.BoundingBox(rh_Start, rh_End);
            }
        }

        /// <inheritdoc cref="GH_Kernel.IGH_PreviewData.ClippingBox"/>
        public RH_Geo.BoundingBox ClippingBox
        {
            get { return this.Boundingbox; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref= "Gh_Segment" /> class.
        /// </summary>
        public Gh_Segment() { }

        /// <summary>
        /// Initialises a new instance of <see cref= "Gh_Segment" /> class from another <see cref="Gh_Segment"/>..
        /// </summary>
        /// <param name="gh_Segment"> <see cref="Gh_Segment"/> to duplicate. </param>
        public Gh_Segment(Gh_Segment gh_Segment)
        {
            this.Value = gh_Segment.Value;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Gh_Segment"/> class from a <see cref="Euc3D.Segment"/>.
        /// </summary>
        /// <param name="line"> <see cref="Euc3D.Segment"/> for the grasshopper type.</param>
        public Gh_Segment(Euc3D.Segment line)
        {
            this.Value = line;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc cref="GH_Kernel.IGH_PreviewData.DrawViewportMeshes(GH_Kernel.GH_PreviewMeshArgs)"/>
        public void DrawViewportMeshes(GH_Kernel.GH_PreviewMeshArgs args)
        {
            /* Do Nothing */
        }

        /// <inheritdoc cref="GH_Kernel.IGH_PreviewData.DrawViewportWires(GH_Kernel.GH_PreviewWireArgs)"/>
        public void DrawViewportWires(GH_Kernel.GH_PreviewWireArgs args)
        {
            Draw.Wireframe.Segment(args.Pipeline, this.Value, false);
        }

        #endregion


        #region Override : GH_Goo<>

        // ---------- Properties ---------- //

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.IsValid"/>
        public override bool IsValid { get { return true; } }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.TypeDescription"/>
        public override string TypeDescription { get { return String.Format($"Grasshopper type containing a {typeof(Euc3D.Segment)}."); } }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.TypeName"/>
        public override string TypeName { get { return nameof(Gh_Segment); } }


        // ---------- Methods ---------- //

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.ToString"/>
        public override string ToString()
        {
            return $"Segment (S:{this.Value.StartPoint}, E:{this.Value.EndPoint})";
        }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.Duplicate"/>
        public override GH_Types.IGH_Goo Duplicate()
        {
            return new Gh_Segment(this);
        }


        /// <inheritdoc cref="GH_Types.GH_Goo{T}.CastFrom(object)"/>
        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }

            var type = source.GetType();

            // ----- BRIDGES Objects ----- //

            // Cast a Euc3D.Segment to a Gh_Segment
            if (typeof(Euc3D.Segment).IsAssignableFrom(type))
            {
                this.Value = (Euc3D.Segment)source;

                return true;
            }
            // Cast a Euc3D.Polyline to a Gh_Segment
            if(typeof(Euc3D.Polyline).IsAssignableFrom(type))
            {
                Euc3D.Polyline polyline = (Euc3D.Polyline)source;
                if(polyline.VertexCount == 2)
                {
                    this.Value = new Euc3D.Segment(polyline[0], polyline[2]);

                    return true;
                }
            }


            // ----- Rhino Objects ----- //

            // Cast a RH_Geo.Line to a Gh_Segment
            if (typeof(RH_Geo.Line).IsAssignableFrom(type))
            {
                ((RH_Geo.Line)source).CastTo(out Euc3D.Segment segment);
                this.Value = segment;

                return true;
            }
            // Cast a RH_Geo.LineCurve to a Gh_Segment
            if (typeof(RH_Geo.LineCurve).IsAssignableFrom(type))
            {
                ((RH_Geo.LineCurve)source).CastTo(out Euc3D.Segment segment);
                this.Value = segment;

                return true;
            }
            // Cast a RH_Geo.Polyline to a Gh_Segment
            if (typeof(RH_Geo.Polyline).IsAssignableFrom(type))
            {
                RH_Geo.Polyline Polyline = (RH_Geo.Polyline)source;

                if (Polyline.Count == 2)
                {
                    Polyline[0].CastTo(out Euc3D.Point start);
                    Polyline[1].CastTo(out Euc3D.Point end);

                    this.Value = new Euc3D.Segment(start, end);

                    return true;
                }
            }
            // Cast a RH_Geo.Polyline to a Gh_Segment
            if (typeof(RH_Geo.PolylineCurve).IsAssignableFrom(type))
            {
                RH_Geo.Polyline Polyline = ((RH_Geo.PolylineCurve)source).ToPolyline();

                if (Polyline.Count == 2)
                {
                    Polyline[0].CastTo(out Euc3D.Point start);
                    Polyline[1].CastTo(out Euc3D.Point end);

                    this.Value = new Euc3D.Segment(start, end);

                    return true;
                }
            }
            // Cast a RH_Geo.Curve to a Gh_Segment
            if (typeof(RH_Geo.Curve).IsAssignableFrom(type))
            {
                RH_Geo.Curve rh_Curve = (RH_Geo.Curve)source;

                if (rh_Curve.IsLinear())
                {
                    rh_Curve.PointAtStart.CastTo(out Euc3D.Point start);
                    rh_Curve.PointAtEnd.CastTo(out Euc3D.Point end);

                    this.Value = new Euc3D.Segment(start, end);

                    return true;
                }
            }


            // ----- BRIDGES.McNeel.Grasshopper Objects ----- //

            // Casts a Gh_Segment to a Gh_Segment
            if (typeof(Gh_Segment).IsAssignableFrom(type))
            {
                Euc3D.Segment segment = ((Gh_Segment)source).Value;

                this.Value = segment;

                return true;
            }
            // Cast a Gh_Polyline to a Gh_Segment
            if (typeof(Gh_Polyline).IsAssignableFrom(type))
            {
                Gh_Polyline gh_Polyine = (Gh_Polyline)source;

                if (gh_Polyine.Value.VertexCount == 2)
                {
                    this.Value = new Euc3D.Segment(gh_Polyine.Value[0], gh_Polyine.Value[1]);

                    return true;
                }
                
            }


            // ----- Grasshopper Objects ----- //

            // Cast a GH_Types.GH_Line to a Gh_Segment
            if (typeof(GH_Types.GH_Line).IsAssignableFrom(type))
            {
                GH_Types.GH_Line gh_Line = (GH_Types.GH_Line)source;

                gh_Line.Value.CastTo(out Euc3D.Segment segment);
                this.Value = segment;

                return true;
            }
            // Cast a GH_Types.GH_Curve to a Gh_Segment
            if (typeof(GH_Types.GH_Curve).IsAssignableFrom(type))
            {
                GH_Types.GH_Curve gh_Curve = (GH_Types.GH_Curve)source;

                if (gh_Curve.Value.IsLinear())
                {
                    gh_Curve.Value.PointAtStart.CastTo(out Euc3D.Point start);
                    gh_Curve.Value.PointAtEnd.CastTo(out Euc3D.Point end);

                    this.Value = new Euc3D.Segment(start, end);

                    return true;
                }
            }


            // ----- Otherwise ----- //

            return false;
        }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.CastTo{Q}(ref Q)"/>
        public override bool CastTo<T>(ref T target)
        {
            // ----- BRIDGES Objects ----- //

            // Casts a Gh_Segment to a Euc3D.Segment
            if (typeof(T).IsAssignableFrom(typeof(Euc3D.Segment)))
            {
                target = (T)(object)this.Value;
                return true;
            }
            // Casts a Gh_Segment to a Euc3D.Polyline
            if (typeof(T).IsAssignableFrom(typeof(Euc3D.Line)))
            {
                Euc3D.Polyline polyline = new Euc3D.Polyline(new Euc3D.Point[2] { this.Value.StartPoint, this.Value.EndPoint });

                target = (T)(object)polyline;
                return true;
            }


            // ----- Rhino Objects ----- //

            // Casts a Gh_Segment to a RH_Geo.Line
            if (typeof(T).IsAssignableFrom(typeof(RH_Geo.Line)))
            {
                this.Value.CastTo(out RH_Geo.Line rh_Line);

                target = (T)(object)rh_Line;
                return true;
            }
            // Casts a Gh_Segment to a RH_Geo.LineCurve
            if (typeof(T).IsAssignableFrom(typeof(RH_Geo.LineCurve)))
            {
                this.Value.CastTo(out RH_Geo.LineCurve rh_LineCurve);

                target = (T)(object)rh_LineCurve;

                return true;
            }
            // Casts a Gh_Segment to a RH_Geo.Polyline
            if (typeof(T).IsAssignableFrom(typeof(RH_Geo.Polyline)))
            {
                RH_Geo.Point3d[] vertices = new RH_Geo.Point3d[2];

                this.Value.StartPoint.CastTo(out vertices[0]);
                this.Value.EndPoint.CastTo(out vertices[1]);

                RH_Geo.Polyline rh_Polyline = new RH_Geo.Polyline(vertices);

                target = (T)(object)rh_Polyline;
                return true;
            }
            // Casts a Gh_Segment to a RH_Geo.PolylineCurve
            if (typeof(T).IsAssignableFrom(typeof(RH_Geo.PolylineCurve)))
            {
                RH_Geo.Point3d[] vertices = new RH_Geo.Point3d[2];

                this.Value.StartPoint.CastTo(out vertices[0]);
                this.Value.EndPoint.CastTo(out vertices[1]);

                RH_Geo.PolylineCurve rh_PolylineCurve = new RH_Geo.PolylineCurve(vertices);

                target = (T)(object)rh_PolylineCurve;

                return true;
            }
            // Casts a Gh_Segment to a RH_Geo.Curve
            if (typeof(T).IsAssignableFrom(typeof(RH_Geo.Curve)))
            {
                this.Value.CastTo(out RH_Geo.LineCurve rh_LineCurve);
                target = (T)(object)(RH_Geo.Curve)rh_LineCurve;

                return true;
            }


            // ----- BRIDGES.McNeel.Grasshopper Objects ----- //

            // Casts a Gh_Segment to a Gh_Segment
            if (typeof(T).IsAssignableFrom(typeof(Gh_Segment)))
            {
                target = (T)(object)this;

                return true;
            }
            // Casts a Gh_Segment to a Gh_Polyline
            if (typeof(T).IsAssignableFrom(typeof(Gh_Polyline)))
            {
                Euc3D.Polyline polyline = new Euc3D.Polyline( new Euc3D.Point[2] { this.Value.StartPoint, this.Value.EndPoint });

                Gh_Polyline gh_Polyline = new Gh_Polyline(polyline);
                target = (T)(object)gh_Polyline;

                return true;
            }


            // ----- Grasshopper Objects ----- //

            // Casts a Gh_Segment to a GH_Types.Gh_Line
            if (typeof(T).IsAssignableFrom(typeof(GH_Types.GH_Line)))
            {
                this.Value.CastTo(out RH_Geo.Line rh_Line);

                GH_Types.GH_Line gh_Line = new GH_Types.GH_Line(rh_Line);
                target = (T)(object)gh_Line;

                return true;
            }
            // Casts a Gh_Segment to a GH_Types.GH_Curve
            if (typeof(T).IsAssignableFrom(typeof(GH_Types.GH_Curve)))
            {
                this.Value.CastTo(out RH_Geo.LineCurve rh_LineCurve);

                GH_Types.GH_Curve gh_Curve = new GH_Types.GH_Curve(rh_LineCurve);
                target = (T)(object)gh_Curve;

                return true;
            }


            // ----- Otherwise ----- //

            return false;
        }

        #endregion
    }
}
