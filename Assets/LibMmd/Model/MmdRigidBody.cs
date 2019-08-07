using UnityEngine;

namespace LibMMD.Model
{
    public class MmdRigidBody
    {
        /// <summary>
        /// collider����
        /// </summary>
        public enum RigidBodyShape : byte
        {
            RigidShapeSphere = 0x00,
            RigidShapeBox = 0x01,
            RigidShapeCapsule = 0x02
        }

        /// <summary>
        /// rigidbodyģʽ
        /// </summary>
        public enum RigidBodyType : byte
        {
            RigidTypeKinematic = 0x00,
            RigidTypePhysics = 0x01,
            RigidTypePhysicsStrict = 0x02,
            RigidTypePhysicsGhost = 0x03
        }
        /// <summary>
        /// ��ײ������
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ��ײ��Ӣ����
        /// </summary>
        public string NameEn { get; set; }
        /// <summary>
        /// �������������
        /// </summary>
        public int AssociatedBoneIndex { get; set; }
        /// <summary>
        /// ������ײ����?
        /// </summary>
        public int CollisionGroup { get; set; }
        /// <summary>
        /// ��ײ��
        /// </summary>
        public ushort CollisionMask { get; set; }
        /// <summary>
        /// collider����
        /// </summary>
        public RigidBodyShape Shape { get; set; }
        /// <summary>
        /// ά��?
        /// </summary>
        public Vector3 Dimemsions { get; set; }
        /// <summary>
        /// position����
        /// </summary>
        public Vector3 Position { get; set; }
        /// <summary>
        /// rotation(����,��Ӧ������Ԫ����)
        /// </summary>
        public Vector3 Rotation { get; set; }
        /// <summary>
        /// ����,һ����
        /// </summary>
        public float Mass { get; set; }
        /// <summary>
        /// damp?
        /// </summary>
        public float TranslateDamp { get; set; }
        /// <summary>
        /// ��ת��damp?
        /// </summary>
        public float RotateDamp { get; set; }
        /// <summary>
        /// ��λ
        /// </summary>
        public float Restitution { get; set; }
        /// <summary>
        /// Ħ����
        /// </summary>
        public float Friction { get; set; }
        /// <summary>
        /// rigidbody��ײģʽ
        /// </summary>
        public RigidBodyType Type { get; set; }
    }
}