using System;
using System.Collections.Generic;
using UnityEngine;

namespace LibMMD.Model
{
    public class Bone
    {
        
        public class IkLink//OYM：不知道
        {
            public int LinkIndex { get; set; }
            public bool HasLimit { get; set; }
            public Vector3 LoLimit { get; set; }
            public Vector3 HiLimit { get; set; }

            public static IkLink CopyOf(IkLink ikLink)//OYM：一个静态方法,返回属性
            {
                return new IkLink
                {
                    LinkIndex = ikLink.LinkIndex,
                    HasLimit = ikLink.HasLimit,
                    LoLimit = ikLink.LoLimit,
                    HiLimit = ikLink.HiLimit
                };
            }
        }
        /// <summary>
        /// 子骨骼
        /// </summary>
        public class ChildBone
        {
            /// <summary>
            /// 这是个bool?
            /// </summary>
            public bool ChildUseId { get; set; }
            /// <summary>
            /// 偏移量
            /// </summary>
            public Vector3 Offset { get; set; }
            /// <summary>
            /// 序列号
            /// </summary>
            public int Index { get; set; }

            public static ChildBone CopyOf(ChildBone childBone)//OYM：一个方法,返回信息
            {
                return new ChildBone
                {
                    ChildUseId = childBone.ChildUseId,
                    Offset = childBone.Offset,
                    Index = childBone.Index
                };
            }
        }


        public class AppendBone//不清楚+1,悬挂的骨骼?
        {
            public int Index { get; set; }
            public float Ratio { get; set; }

            public static AppendBone CopyOf(AppendBone appendBone)
            {
                return new AppendBone
                {
                    Index = appendBone.Index,
                    Ratio = appendBone.Ratio
                };
            }
        }

        public class IkInfo //IK信息(莫非是通过挪IK来完成的?)
        {
            public int IkTargetIndex { get; set; }
            public int CcdIterateLimit { get; set; }
            public float CcdAngleLimit { get; set; }
            public IkLink[] IkLinks { get; set; }

            public static IkInfo CopyOf(IkInfo ikInfo) //OYM：返回blablabla,对了这里还有一个IKLink的数组
            {
                var ikLinksCopy = new IkLink[ikInfo.IkLinks.Length];
                ikInfo.IkLinks.CopyTo(ikLinksCopy, 0);
                return new IkInfo
                {
                    IkTargetIndex = ikInfo.IkTargetIndex,
                    CcdIterateLimit = ikInfo.CcdIterateLimit,
                    CcdAngleLimit = ikInfo.CcdAngleLimit,
                    IkLinks = ikLinksCopy
                };
            }
        }

        public class LocalAxis //OYM：本地平面
        {
            public Vector3 AxisX { get; set; }
            public Vector3 AxisY { get; set; }
            public Vector3 AxisZ { get; set; }

            public static LocalAxis CopyOf(LocalAxis localAxis)
            {
                return new LocalAxis
                {
                    AxisX = localAxis.AxisX,
                    AxisY = localAxis.AxisY,
                    AxisZ = localAxis.AxisZ
                };
            }
        }

        public Bone() //OYM：骨骼类的构造方法
        {
            ChildBoneVal = new ChildBone();//OYM：初始化
            AppendBoneVal = new AppendBone();//OYM：初始化
            LocalAxisVal = new LocalAxis();//OYM：初始化
        }
        /// <summary>
        /// 骨骼名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 骨骼英文名
        /// </summary>
        public string NameEn { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position { get; set; }
        /// <summary>
        /// 父序号
        /// </summary>
        public int ParentIndex { get; set; }
        /// <summary>
        /// 不清楚
        /// </summary>
        public int TransformLevel { get; set; }
        /// <summary>
        /// 是否可旋转?
        /// </summary>
        public bool Rotatable { get; set; }
        /// <summary>
        /// 是否可移动
        /// </summary>
        public bool Movable { get; set; }
        /// <summary>
        /// 是否可显示
        /// </summary>
        public bool Visible { get; set; }
        /// <summary>
        /// 是否可操作
        /// </summary>
        public bool Controllable { get; set; }
        /// <summary>
        /// 有IK
        /// </summary>
        public bool HasIk { get; set; }
        /// <summary>
        /// rotate相关,具体不明
        /// </summary>
        public bool AppendRotate { get; set; }
        /// <summary>
        /// 等下,这个translate是翻译还是转变?
        /// </summary>
        public bool AppendTranslate { get; set; }
        /// <summary>
        /// 旋转固定轴
        /// </summary>
        public bool RotAxisFixed { get; set; }
        /// <summary>
        /// 使用本地坐标?
        /// </summary>
        public bool UseLocalAxis { get; set; }
        /// <summary>
        /// 物理某个参数,看到再回来补
        /// </summary>
        public bool PostPhysics { get; set; }
        /// <summary>
        /// 接收的位置?
        /// </summary>
        public bool ReceiveTransform { get; set; }
        /// <summary>
        /// 子骨骼
        /// </summary>
        public ChildBone ChildBoneVal { get; set; }
        /// <summary>
        /// 这个应该是父骨骼了
        /// </summary>
        public AppendBone AppendBoneVal { get; set; }
        /// <summary>
        /// 旋转平面
        /// </summary>
        public Vector3 RotAxis { get; set; }
        /// <summary>
        /// 不知道
        /// </summary>
        public LocalAxis LocalAxisVal { get; set; }
        /// <summary>
        /// 不知道+1
        /// </summary>
        public int ExportKey { get; set; }
        /// <summary>
        /// 不知道+2
        /// </summary>
        public IkInfo IkInfoVal { get; set; }

        public static Bone CopyOf(Bone bone)
        {
            return new Bone
            {
                //OYM：这是什么骚操作
                Name = bone.Name,
                NameEn = bone.NameEn,
                Position = bone.Position,
                ParentIndex = bone.ParentIndex,
                TransformLevel = bone.TransformLevel,
                Rotatable = bone.Rotatable,
                Movable = bone.Movable,
                Visible = bone.Visible,
                Controllable = bone.Controllable,
                HasIk = bone.HasIk,
                AppendRotate = bone.AppendRotate,
                AppendTranslate = bone.AppendTranslate,
                RotAxisFixed =  bone.RotAxisFixed,
                UseLocalAxis = bone.UseLocalAxis,
                PostPhysics = bone.PostPhysics,
                ReceiveTransform = bone.ReceiveTransform,
                ChildBoneVal = ChildBone.CopyOf(bone.ChildBoneVal),
                AppendBoneVal = AppendBone.CopyOf(bone.AppendBoneVal),
                RotAxis = bone.RotAxis,
                LocalAxisVal = LocalAxis.CopyOf(bone.LocalAxisVal),
                ExportKey = bone.ExportKey,
                IkInfoVal = IkInfo.CopyOf(bone.IkInfoVal),
            };
        }
        
        
    }
}