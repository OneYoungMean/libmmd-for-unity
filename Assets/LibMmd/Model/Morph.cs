using System.Collections.Generic;
using LibMMD.Util;
using UnityEngine;

namespace LibMMD.Model
{
    /// <summary>
    /// 姑且称为表情吧
    /// </summary>
    public class Morph
    {
        /// <summary>
        /// 种类
        /// </summary>
        public enum MorphCategory : byte
        {
            /// <summary>
            /// 全身
            /// </summary>
            MorphCatSystem = 0x00,
            /// <summary>
            /// 眉毛
            /// </summary>
            MorphCatEyebrow = 0x01,
            /// <summary>
            /// 眼
            /// </summary>
            MorphCatEye = 0x02,
            /// <summary>
            /// 嘴
            /// </summary>
            MorphCatMouth = 0x03,
            /// <summary>
            /// 其他
            /// </summary>
            MorphCatOther = 0x04
        }
        /// <summary>
        /// 变形方式
        /// </summary>
        public enum MorphType : byte
        {
            /// <summary>
            /// 不知道
            /// </summary>
            MorphTypeGroup = 0x00,
            /// <summary>
            /// 顶点变形
            /// </summary>
            MorphTypeVertex = 0x01,
            /// <summary>
            /// 骨骼变形
            /// </summary>
            MorphTypeBone = 0x02,
            //OYM：后面一系列都是uv,这里就不重复阐述了
            MorphTypeUv = 0x03,
            MorphTypeExtUv1 = 0x04,
            MorphTypeExtUv2 = 0x05,
            MorphTypeExtUv3 = 0x06,
            MorphTypeExtUv4 = 0x07,
            /// <summary>
            /// 材料变形(罕见)
            /// </summary>
            MorphTypeMaterial = 0x08
        }
        /// <summary>
        /// 所有表情的集合
        /// </summary>
        public abstract class MorphData 
        {
            //OYM：啊我懂了,原来抽象类是这么用的
        }

        public class GroupMorph : MorphData
        {
            public int MorphIndex { get; set; }
            public float MorphRate { get; set; }
        }

        public class VertexMorph : MorphData
        {
            public int VertexIndex { get; set; }
            public Vector3 Offset { get; set; }
        }

        public class BoneMorph : MorphData
        {
            public int BoneIndex { get; set; }
            public Vector3 Translation { get; set; }
            public Quaternion Rotation { get; set; }
        }

        public class UvMorph : MorphData
        {
            public int VertexIndex { get; set; }
            public Vector4 Offset { get; set; }
        }

        public class MaterialMorph : MorphData
        {
            public enum MaterialMorphMethod : byte {
                MorphMatMul = 0x00,
                MorphMatAdd = 0x01
            }

            public int MaterialIndex { get; set; }
            public bool Global { get; set; }
            public MaterialMorphMethod Method { get; set; }
            public Color Diffuse { get; set; }
            public Color Specular { get; set; }
            public Color Ambient { get; set; }
            public float Shiness { get; set; }
            public Color EdgeColor { get; set; }
            public float EdgeSize { get; set; }
            public Vector4 Texture { get; set; }
            public Vector4 SubTexture { get; set; }
            public Vector4 ToonTexture { get; set; }
        }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 英文名
        /// </summary>
        public string NameEn { get; set; }
        public MorphCategory Category { get; set; }
        public MorphType Type { get; set; }
        public MorphData[] MorphDatas { get; set; }
    }
}