using Godot;
using CFEVect2 = Godot.Vector2;

namespace FuetEngine
{
    [Tool]
    public class CFESpriteFrame : Node
    {
        // Make sure you provide a parameterless constructor.
        public CFESpriteFrame()
        {
        }
        /// Should we use the same scale of X for the U tex coord?
        [Export]
        public bool m_bScaleXUEqually;

        /// Should we use the same scale of Y for the V tex coord?
        [Export]
        public bool m_bScaleYVEqually;

        /// Should this frame use projected world coordinates instead of UV coords? (U component)
        [Export]
        public bool m_bUWorldCoords;

        /// Should this frame use projected world coordinates instead of UV coords? (V component)
        [Export]
        public bool m_bVWorldCoords;

        /// Rectangle inside the sprite material defining this sprite frame.
        [Export]
        public CFEVect2 m_oUVIni = new CFEVect2();
        [Export]
        public CFEVect2 m_oUVEnd = new CFEVect2();

        /// Size in pixels (of virtual screen dimensions) of the frame.
        [Export]
        public CFEVect2 m_oSize = new CFEVect2();

        [Export]
        public CFEVect2 m_oScale = new CFEVect2();  // computed from m_oSize and m_oUV

        /// Time this frame should be rendered without blending.
        [Export]
        public float m_rDelay;

        /// The blending time between this and the next frame.
        [Export]
        public float m_rBlendTime;

        /// Total time between this and the next frame.
        [Export]
        public float m_rFrameTime;

        /// The moment in whole action this frame should be played.
        [Export]
        public float m_rStartTime;

        /// Pivot point of the sprite frame.
        // pivots go from 0,0 to 1,1 relative to the frame
        [Export]
        public CFEVect2 m_oPivot = new CFEVect2();

        /// Inverse of the material dimensions
        public CFEVect2 m_o1OverDims = new CFEVect2();

        /// The material used by this sprite frame.
        // public Material m_hMaterial = null;
        [Export]
        public Texture m_hMaterial = null;
        [Export]
        public string m_sMaterial = null;    // comes from material
    };
}
