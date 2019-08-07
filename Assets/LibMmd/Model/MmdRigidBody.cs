using UnityEngine;

namespace LibMMD.Model
{
    public class MmdRigidBody
    {
        /// <summary>
        /// collider类型
        /// </summary>
        public enum RigidBodyShape : byte
        {
            RigidShapeSphere = 0x00,
            RigidShapeBox = 0x01,
            RigidShapeCapsule = 0x02
        }

        /// <summary>
        /// rigidbody模式
        /// </summary>
        public enum RigidBodyType : byte
        {
            RigidTypeKinematic = 0x00,
            RigidTypePhysics = 0x01,
            RigidTypePhysicsStrict = 0x02,
            RigidTypePhysicsGhost = 0x03
        }
        /// <summary>
        /// 碰撞体名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 碰撞体英文名
        /// </summary>
        public string NameEn { get; set; }
        /// <summary>
        /// 依赖骨骼的序号
        /// </summary>
        public int AssociatedBoneIndex { get; set; }
        /// <summary>
        /// 发生碰撞的组?
        /// </summary>
        public int CollisionGroup { get; set; }
        /// <summary>
        /// 碰撞层
        /// </summary>
        public ushort CollisionMask { get; set; }
        /// <summary>
        /// collider类型
        /// </summary>
        public RigidBodyShape Shape { get; set; }
        /// <summary>
        /// 维度?
        /// </summary>
        public Vector3 Dimemsions { get; set; }
        /// <summary>
        /// position坐标
        /// </summary>
        public Vector3 Position { get; set; }
        /// <summary>
        /// rotation(等下,不应该是四元数吗)
        /// </summary>
        public Vector3 Rotation { get; set; }
        /// <summary>
        /// 忘了,一大块吧
        /// </summary>
        public float Mass { get; set; }
        /// <summary>
        /// damp?
        /// </summary>
        public float TranslateDamp { get; set; }
        /// <summary>
        /// 旋转的damp?
        /// </summary>
        public float RotateDamp { get; set; }
        /// <summary>
        /// 复位
        /// </summary>
        public float Restitution { get; set; }
        /// <summary>
        /// 摩擦力
        /// </summary>
        public float Friction { get; set; }
        /// <summary>
        /// rigidbody碰撞模式
        /// </summary>
        public RigidBodyType Type { get; set; }
    }
}