using System;

using Euc3D = BRIDGES.Geometry.Euclidean3D;
using He = BRIDGES.DataStructures.PolyhedralMeshes.HalfedgeMesh;
using Fv = BRIDGES.DataStructures.PolyhedralMeshes.FaceVertexMesh;

using RH_Geo = Rhino.Geometry;

using BRIDGES.McNeel.Rhino.Extensions.Geometry.Euclidean3D;

using GH_Kernel = Grasshopper.Kernel;
using GH_Types = Grasshopper.Kernel.Types;

using BRIDGES.McNeel.Grasshopper.Display.Geometry.Euclidean3D;


namespace BRIDGES.McNeel.Grasshopper.Types.Geometry.Euclidean3D
{
    /// <summary>
    /// Class defining a grasshopper type for an <see cref="He.Mesh{TPosition}"/>.
    /// </summary>
    public class Gh_HeMesh : GH_Types.GH_Goo<He.Mesh<Euc3D.Point>>, GH_Kernel.IGH_PreviewData
    {
        #region Properties

        /// <summary>
        /// Volume containing the data.
        /// </summary>
        public RH_Geo.BoundingBox Boundingbox
        {
            get
            {
                double minX = 0d, maxX = 0d, minY = 0d, maxY = 0d, minZ = 0d, maxZ = 0d;
                foreach (He.Vertex<Euc3D.Point> vertices in Value.GetVertices())
                {
                    if (vertices.Position.X < minX) { minX = vertices.Position.X; }
                    else if (maxX < vertices.Position.X) { minX = vertices.Position.X; }

                    if (vertices.Position.Y < minY) { minY = vertices.Position.Y; }
                    else if (maxY < vertices.Position.Y) { maxY = vertices.Position.Y; }

                    if (vertices.Position.Z < minZ) { minZ = vertices.Position.Z; }
                    else if (maxZ < vertices.Position.Z) { maxZ = vertices.Position.Z; }
                }

                return new RH_Geo.BoundingBox(minX, maxX, minY, maxY, minZ, maxZ);
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
        /// Initialises a new instance of <see cref= "Gh_HeMesh" /> class.
        /// </summary>
        public Gh_HeMesh() { }

        /// <summary>
        /// Initialises a new instance of <see cref= "Gh_HeMesh" /> class from another <see cref="Gh_HeMesh"/>..
        /// </summary>
        /// <param name="gh_HeMesh"> <see cref="Gh_HeMesh"/> to duplicate. </param>
        public Gh_HeMesh(Gh_HeMesh gh_HeMesh)
        {
            this.Value = (He.Mesh<Euc3D.Point>)gh_HeMesh.Value.Clone() ;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Gh_HeMesh"/> class from a <see cref="He.Mesh{TPosition}"/>.
        /// </summary>
        /// <param name="heMesh"> <see cref="He.Mesh{TPosition}"/> to duplicate.</param>
        public Gh_HeMesh(He.Mesh<Euc3D.Point> heMesh)
        {
            this.Value = (He.Mesh<Euc3D.Point>)heMesh.Clone();
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
            Draw.Wireframe.Mesh(args.Pipeline, this.Value, false);
        }

        #endregion


        #region Override : GH_Goo<>

        /********** Properties **********/

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.IsValid"/>
        public override bool IsValid { get { return true; } }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.TypeDescription"/>
        public override string TypeDescription { get { return String.Format($"Grasshopper type containing a {typeof(He.Mesh<Euc3D.Point>)}."); } }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.TypeName"/>
        public override string TypeName { get { return nameof(Gh_HeMesh); } }


        /********** Methods **********/

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.ToString"/>
        public override string ToString()
        {
            return $"HeMesh (V:{Value.VertexCount}, H:{Value.HalfedgeCount}, F:{Value.FaceCount})";
        }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.Duplicate"/>
        public override GH_Types.IGH_Goo Duplicate()
        {
            return new Gh_HeMesh(this);
        }


        /// <inheritdoc cref="GH_Types.GH_Goo{T}.CastFrom(object)"/>
        public override bool CastFrom(object source)
        {
            if (source == null) { return false; }

            var type = source.GetType();

            /******************** BRIDGES Objects ********************/

            // Cast a He.Mesh<Euc3D.Point> to a Gh_HeMesh
            if (typeof(He.Mesh<Euc3D.Point>).IsAssignableFrom(type))
            {
                Value = (He.Mesh<Euc3D.Point>)source;
                return true;
            }
            // Cast a Fv.Mesh<Euc3D.Point> to a Gh_HeMesh
            if (typeof(Fv.Mesh<Euc3D.Point>).IsAssignableFrom(type))
            {
                Fv.Mesh<Euc3D.Point> fvMesh = (Fv.Mesh<Euc3D.Point>)source;
                Value = fvMesh.ToHalfedgeMesh();
                return true;
            }

            /******************** Rhino Objects ********************/

            // Cast a RH_Geo.Mesh to a Gh_HeMesh
            if (typeof(RH_Geo.Mesh).IsAssignableFrom(type))
            {
                RH_Geo.Mesh rh_Mesh = (RH_Geo.Mesh)source;

                rh_Mesh.ConvertTo(out He.Mesh<Euc3D.Point> mesh);
                Value = mesh;

                return true;
            }


            /******************** BRIDGES.McNeel.Grasshopper Objects ********************/

            // Cast a Gh_FvMesh to a Gh_HeMesh
            if (typeof(Gh_FvMesh).IsAssignableFrom(type))
            {
                Fv.Mesh<Euc3D.Point> fvMesh = ((Gh_FvMesh)source).Value;
                Value = fvMesh.ToHalfedgeMesh();

                return true;
            }


            /******************** Grasshopper Objects ********************/

            // Cast a GH_Types.GH_Mesh to a Gh_HeMesh
            if (typeof(GH_Types.GH_Mesh).IsAssignableFrom(type))
            {
                RH_Geo.Mesh rh_Mesh = ((GH_Types.GH_Mesh)source).Value;

                rh_Mesh.ConvertTo(out He.Mesh<Euc3D.Point> mesh);
                Value = mesh;

                return true;
            }


            /******************** Otherwise ********************/

            return false;
        }

        /// <inheritdoc cref="GH_Types.GH_Goo{T}.CastTo{Q}(ref Q)"/>
        public override bool CastTo<T>(ref T target)
        {
            /******************** BRIDGES Objects ********************/

            // Casts a Gh_HeMesh to a He.HeMesh<Euc3D.Point>
            if (typeof(T).IsAssignableFrom(typeof(He.Mesh<Euc3D.Point>)))
            {
                object mesh = this.Value;
                target = (T)mesh;
                return true;
            }
            // Casts a Gh_HeMesh to a Fv.FvMesh<Euc3D.Point>
            if (typeof(T).IsAssignableFrom(typeof(Fv.Mesh<Euc3D.Point>)))
            {
                He.Mesh<Euc3D.Point> heMesh = this.Value;
                Fv.Mesh<Euc3D.Point> fvMesh = heMesh.ToFaceVertexMesh();
                target = (T)(object)fvMesh;
                return true;
            }

            /******************** Rhino Objects ********************/

            // Casts a Gh_HeMesh to a RH_Geo.Mesh
            if (typeof(T).IsAssignableFrom(typeof(RH_Geo.Plane)))
            {
                this.Value.ConvertTo(out RH_Geo.Mesh rh_Mesh);

                target = (T)(object)rh_Mesh;
                return true;
            }


            /******************** BRIDGES.McNeel.Grasshopper Objects ********************/

            // Casts a Gh_HeMesh to a Gh_FvMesh
            if (typeof(T).IsAssignableFrom(typeof(Gh_FvMesh)))
            {
                He.Mesh<Euc3D.Point> heMesh = this.Value;
                Fv.Mesh<Euc3D.Point> fvMesh = heMesh.ToFaceVertexMesh();

                Gh_FvMesh gh_FvMesh = new Gh_FvMesh(fvMesh);
                target = (T)(object)gh_FvMesh;

                return true;
            }


            /******************** Grasshopper Objects ********************/

            // Casts a Gh_HeMesh to a GH_Types.GH_Mesh
            if (typeof(T).IsAssignableFrom(typeof(GH_Types.GH_Mesh)))
            {
                this.Value.ConvertTo(out RH_Geo.Mesh rh_Mesh);

                GH_Types.GH_Mesh gh_Mesh = new GH_Types.GH_Mesh(rh_Mesh);
                target = (T)(object)gh_Mesh;

                return true;
            }


            /******************** Otherwise ********************/

            return false;
        }

        #endregion
    }
}