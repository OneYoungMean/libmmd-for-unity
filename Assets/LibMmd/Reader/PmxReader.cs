using System;
using System.IO;
using System.Text;
using LibMMD.Material;
using LibMMD.Model;
using LibMMD.Util;
using UnityEngine;

namespace LibMMD.Reader
{
    public class PmxReader : ModelReader
    {
        /// <summary>
        /// 读取mmd
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public override MmdModel Read(BinaryReader reader,ModelReadConfig config)
        {
            //config虽然现在是空的,但是等下会被赋值,作者只是想顺便传出去一下
            var pmxHeader = ReadMeta(reader);//OYM：读取
            if (!"PMX ".Equals(pmxHeader.Magic) || Math.Abs(pmxHeader.Version - 2.0f) > 0.0001f || pmxHeader.FileFlagSize != 8)//OYM：验证文件
            {
                throw new MmdFileParseException("File is not a PMX 2.0 file");
            }

            var model = new MmdModel();
            var pmxConfig = ReadPmxConfig(reader, model);//OYM：底层读取,另外config在这里获取
            ReadModelNameAndDescription(reader, model, pmxConfig);//OYM：读取文件信息
            ReadVertices(reader, model, pmxConfig);//OYM：读取顶点
            ReadTriangles(reader, model, pmxConfig);//OYM：读取三角形
            var textureList = ReadTextureList(reader, pmxConfig);//OYM：获取所有材质的路径的一个类
            ReadParts(reader, config, model, pmxConfig, textureList);//OYM：获取材质shader的类
            ReadBones(reader, model, pmxConfig);//OYM：获取骨骼
            ReadMorphs(reader, model, pmxConfig);//OYM：表情包~
            ReadEntries(reader, pmxConfig);//OYM：没用,但是还是要把字节取出来丢掉
            ReadRigidBodies(reader, model, pmxConfig);//OYM：获取碰撞
            ReadConstraints(reader, model, pmxConfig);//OYM：获取约束
            model.Normalize();//OYM：检查顶点
            return model;
        }
        /// <summary>
        /// 在节点上约束位置啥的,也不知道具体怎么起作用
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="model"></param>
        /// <param name="pmxConfig"></param>
        private static void ReadConstraints(BinaryReader reader, MmdModel model, PmxConfig pmxConfig)
        {
            var constraintNum = reader.ReadInt32();
            model.Constraints = new Constraint[constraintNum];
            for (var i = 0; i < constraintNum; ++i)
            {
                var constraint = new Constraint
                {
                    Name = MmdReaderUtil.ReadSizedString(reader, pmxConfig.Encoding),
                    NameEn = MmdReaderUtil.ReadSizedString(reader, pmxConfig.Encoding)
                };
                var dofType = reader.ReadByte();
                if (dofType == 0)
                {
                    constraint.AssociatedRigidBodyIndex[0] =
                        MmdReaderUtil.ReadIndex(reader, pmxConfig.RigidBodyIndexSize);
                    constraint.AssociatedRigidBodyIndex[1] =
                        MmdReaderUtil.ReadIndex(reader, pmxConfig.RigidBodyIndexSize);
                    constraint.Position = MmdReaderUtil.ReadVector3(reader);
                    constraint.Rotation = MmdReaderUtil.ReadVector3(reader);
                    constraint.PositionLowLimit = MmdReaderUtil.ReadVector3(reader);
                    constraint.PositionHiLimit = MmdReaderUtil.ReadVector3(reader);
                    constraint.RotationLowLimit = MmdReaderUtil.ReadVector3(reader);
                    constraint.RotationHiLimit = MmdReaderUtil.ReadVector3(reader);
                    constraint.SpringTranslate = MmdReaderUtil.ReadVector3(reader);
                    constraint.SpringRotate = MmdReaderUtil.ReadVector3(reader);
                }
                else
                {
                    throw new MmdFileParseException("Only 6DOF spring joints are supported.");
                }

                model.Constraints[i] = constraint;
            }
        }

        private static PmxConfig ReadPmxConfig(BinaryReader reader, MmdModel model)
        {
            //https://www.cnblogs.com/tanding/archive/2012/07/02/2572702.html
            //OYM：下面一摞方法都是读取一个字节,从0-256到-128-128的方法都有.

            var pmxConfig = new PmxConfig();
            pmxConfig.Utf8Encoding = reader.ReadByte() != 0;//OYM：是否是UTF8
            pmxConfig.ExtraUvNumber = reader.ReadSByte();//OYM：UV数量
            pmxConfig.VertexIndexSize = reader.ReadSByte();//OYM：不知道
            pmxConfig.TextureIndexSize = reader.ReadSByte();//OYM：不知道
            pmxConfig.MaterialIndexSize = reader.ReadSByte();//OYM：不知道,下面都不知道,懒得写了
            pmxConfig.BoneIndexSize = reader.ReadSByte();
            pmxConfig.MorphIndexSize = reader.ReadSByte();
            pmxConfig.RigidBodyIndexSize = reader.ReadSByte();

            model.ExtraUvNumber = pmxConfig.ExtraUvNumber;
            pmxConfig.Encoding = pmxConfig.Utf8Encoding ? Encoding.UTF8 : Encoding.Unicode;
            return pmxConfig;
        }
        /// <summary>
        /// 看上去很多要讲的内容...实际上也没啥
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="model"></param>
        /// <param name="pmxConfig"></param>
        private static void ReadRigidBodies(BinaryReader reader, MmdModel model, PmxConfig pmxConfig)
        {
            //OYM：获取rigidbody,然后blablabla,完毕
            var rigidBodyNum = reader.ReadInt32();
            model.Rigidbodies = new MmdRigidBody[rigidBodyNum];
            for (var i = 0; i < rigidBodyNum; ++i)
            {
                var rigidBody = new MmdRigidBody
                {
                    Name = MmdReaderUtil.ReadSizedString(reader, pmxConfig.Encoding),
                    NameEn = MmdReaderUtil.ReadSizedString(reader, pmxConfig.Encoding),
                    AssociatedBoneIndex = MmdReaderUtil.ReadIndex(reader, pmxConfig.BoneIndexSize),
                    CollisionGroup = reader.ReadByte(),
                    CollisionMask = reader.ReadUInt16(),
                    Shape = (MmdRigidBody.RigidBodyShape) reader.ReadByte(),
                    Dimemsions = MmdReaderUtil.ReadVector3(reader),
                    Position = MmdReaderUtil.ReadVector3(reader),
                    Rotation = MmdReaderUtil.ReadVector3(reader),
                    Mass = reader.ReadSingle(),
                    TranslateDamp = reader.ReadSingle(),
                    RotateDamp = reader.ReadSingle(),
                    Restitution = reader.ReadSingle(),
                    Friction = reader.ReadSingle(),
                    Type = (MmdRigidBody.RigidBodyType) reader.ReadByte()
                };
                model.Rigidbodies[i] = rigidBody;
            }
        }

        //unused data
        /// <summary>
        /// 作者写了一个没有用的标签
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="pmxConfig"></param>
        private static void ReadEntries(BinaryReader reader, PmxConfig pmxConfig)
        {
            //OYM：注意,这是个void方法...作者把所有的数据都丢了23333
            var entryItemNum = reader.ReadInt32();
            for (var i = 0; i < entryItemNum; ++i)
            {
                MmdReaderUtil.ReadSizedString(reader, pmxConfig.Encoding); //entryItemName
                MmdReaderUtil.ReadSizedString(reader, pmxConfig.Encoding); //entryItemNameEn
                reader.ReadByte(); //isSpecial
                var elementNum = reader.ReadInt32();
                for (var j = 0; j < elementNum; ++j)
                {
                    var isMorph = reader.ReadByte() == 1;
                    if (isMorph)
                    {
                        MmdReaderUtil.ReadIndex(reader, pmxConfig.MorphIndexSize); //morphIndex
                        
                    }
                    else
                    {
                        MmdReaderUtil.ReadIndex(reader, pmxConfig.BoneIndexSize); //boneIndex
                    }
                }
            }
        }
        /// <summary>
        /// 读取变形
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="model"></param>
        /// <param name="pmxConfig"></param>
        private static void ReadMorphs(BinaryReader reader, MmdModel model, PmxConfig pmxConfig)
        {
            var morphNum = reader.ReadInt32();//OYM：表情包数量
            int? baseMorphIndex = null;
            model.Morphs = new Morph[morphNum];
            for (var i = 0; i < morphNum; ++i)
            {
                var morph = new Morph
                {
                    Name = MmdReaderUtil.ReadSizedString(reader, pmxConfig.Encoding),
                    NameEn = MmdReaderUtil.ReadSizedString(reader, pmxConfig.Encoding),
                    Category = (Morph.MorphCategory) reader.ReadByte()
                };

                if (morph.Category == Morph.MorphCategory.MorphCatSystem)
                {
                    baseMorphIndex = i;//OYM：这个就相当于是一个基类吧
                }

                morph.Type = (Morph.MorphType)reader.ReadByte();//OYM：变形方式
                var morphDataNum = reader.ReadInt32();
                morph.MorphDatas = new Morph.MorphData[morphDataNum];//OYM：获取表情变形的数据
                switch (morph.Type)
                {
                    case Morph.MorphType.MorphTypeGroup://OYM：不知道干啥用的
                        for (var j = 0; j < morphDataNum; ++j)
                        {
                            var morphData =
                                new Morph.GroupMorph
                                {
                                    MorphIndex = MmdReaderUtil.ReadIndex(reader, pmxConfig.MorphIndexSize),
                                    MorphRate = reader.ReadSingle()
                                };
                            morph.MorphDatas[j] = morphData;
                        }
                        break;
                    case Morph.MorphType.MorphTypeVertex://OYM：喜闻乐见的顶点变形
                        for (var j = 0; j < morphDataNum; ++j)
                        {
                            var morphData =
                                new Morph.VertexMorph
                                {
                                    VertexIndex = MmdReaderUtil.ReadIndex(reader, pmxConfig.VertexIndexSize),//OYM：顶点的序号
                                    Offset = MmdReaderUtil.ReadVector3(reader)//OYM：偏移量
                                };
                            morph.MorphDatas[j] = morphData;
                        }
                        break;
                    case Morph.MorphType.MorphTypeBone://OYM：骨骼变形
                        for (var j = 0; j < morphDataNum; ++j)
                        {
                            var morphData =
                                new Morph.BoneMorph
                                {
                                    BoneIndex = MmdReaderUtil.ReadIndex(reader, pmxConfig.BoneIndexSize),//OYM：骨骼序号
                                    Translation = MmdReaderUtil.ReadVector3(reader),//OYM：移动方向
                                    Rotation = MmdReaderUtil.ReadQuaternion(reader)//OYM：旋转方向
                                };
                            morph.MorphDatas[j] = morphData;
                        }

                        break;
                    //OYM：下面一堆case都是执行靠后面那个方法
                    case Morph.MorphType.MorphTypeUv:
                    case Morph.MorphType.MorphTypeExtUv1:
                    case Morph.MorphType.MorphTypeExtUv2:
                    case Morph.MorphType.MorphTypeExtUv3:
                    case Morph.MorphType.MorphTypeExtUv4:
                        for (var j = 0; j < morphDataNum; ++j)
                        {
                            //OYM：获取信息就完事了
                            var morphData =
                                new Morph.UvMorph
                                {
                                    VertexIndex = MmdReaderUtil.ReadIndex(reader, pmxConfig.VertexIndexSize),
                                    Offset = MmdReaderUtil.ReadVector4(reader)
                                };
                            morph.MorphDatas[j] = morphData;
                        }

                        break;
                    case Morph.MorphType.MorphTypeMaterial:
                        for (var j = 0; j < morphDataNum; j++)
                        {
                            var morphData = new Morph.MaterialMorph();
                            var mmIndex = MmdReaderUtil.ReadIndex(reader, pmxConfig.MaterialIndexSize);
                            //OYM：补一个,作者居然还顺便修复了一个bug23333
                            if (mmIndex < model.Parts.Length && mmIndex > 0) //TODO mmdlib的代码里是和bone数比较。确认这个逻辑
                            {
                                morphData.MaterialIndex = mmIndex;
                                morphData.Global = false;
                            }
                            else
                            {
                                morphData.MaterialIndex = 0;
                                morphData.Global = true;
                            }
                            morphData.Method = (Morph.MaterialMorph.MaterialMorphMethod) reader.ReadByte();
                            morphData.Diffuse = MmdReaderUtil.ReadColor(reader, true);
                            morphData.Specular = MmdReaderUtil.ReadColor(reader, false);
                            morphData.Shiness = reader.ReadSingle();
                            morphData.Ambient = MmdReaderUtil.ReadColor(reader, false);
                            morphData.EdgeColor = MmdReaderUtil.ReadColor(reader, true);
                            morphData.EdgeSize = reader.ReadSingle();
                            morphData.Texture = MmdReaderUtil.ReadVector4(reader);
                            morphData.SubTexture = MmdReaderUtil.ReadVector4(reader);
                            morphData.ToonTexture = MmdReaderUtil.ReadVector4(reader);
                            morph.MorphDatas[j] = morphData;
                        }
                        break;
                    default:
                        throw new MmdFileParseException("invalid morph type " + morph.Type);
                }
                if (baseMorphIndex != null)
                {
                    //TODO rectify system-reserved category
                    //OYM：不清楚作者要干啥
                }

                model.Morphs[i] = morph;
            }
        }
        /// <summary>
        /// 读取骨骼数目
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="model"></param>
        /// <param name="pmxConfig"></param>
        private static void ReadBones(BinaryReader reader, MmdModel model, PmxConfig pmxConfig)
        {
            var boneNum = reader.ReadInt32();//OYM：获取骨骼数目
            model.Bones = new Bone[boneNum];
            for (var i = 0; i < boneNum; ++i)
            {
                var bone = new Bone
                {
                    Name = MmdReaderUtil.ReadSizedString(reader, pmxConfig.Encoding),
                    NameEn = MmdReaderUtil.ReadSizedString(reader, pmxConfig.Encoding),
                    Position = MmdReaderUtil.ReadVector3(reader)
                };
                var parentIndex = MmdReaderUtil.ReadIndex(reader, pmxConfig.BoneIndexSize);//OYM：获取父骨骼的序号
                if (parentIndex < boneNum && parentIndex >= 0)
                {
                    bone.ParentIndex = parentIndex;
                }
                else
                {
                    bone.ParentIndex = -1;
                }
                bone.TransformLevel = reader.ReadInt32();//OYM：不知道
                //下面一排位运算,跳过
                var flag = reader.ReadUInt16();
                bone.ChildBoneVal.ChildUseId = (flag & PmxBoneFlags.PmxBoneChildUseId) != 0;
                bone.Rotatable = (flag & PmxBoneFlags.PmxBoneRotatable) != 0;
                bone.Movable = (flag & PmxBoneFlags.PmxBoneMovable) != 0;
                bone.Visible = (flag & PmxBoneFlags.PmxBoneVisible) != 0;
                bone.Controllable = (flag & PmxBoneFlags.PmxBoneControllable) != 0;
                bone.HasIk = (flag & PmxBoneFlags.PmxBoneHasIk) != 0;
                bone.AppendRotate = (flag & PmxBoneFlags.PmxBoneAcquireRotate) != 0;
                bone.AppendTranslate = (flag & PmxBoneFlags.PmxBoneAcquireTranslate) != 0;
                bone.RotAxisFixed = (flag & PmxBoneFlags.PmxBoneRotAxisFixed) != 0;
                bone.UseLocalAxis = (flag & PmxBoneFlags.PmxBoneUseLocalAxis) != 0;
                bone.PostPhysics = (flag & PmxBoneFlags.PmxBonePostPhysics) != 0;
                bone.ReceiveTransform = (flag & PmxBoneFlags.PmxBoneReceiveTransform) != 0;
                if (bone.ChildBoneVal.ChildUseId)
                {
                    bone.ChildBoneVal.Index = MmdReaderUtil.ReadIndex(reader, pmxConfig.BoneIndexSize);
                }
                else
                {
                    bone.ChildBoneVal.Offset = MmdReaderUtil.ReadVector3(reader);
                }
                if (bone.RotAxisFixed)//OYM：旋转角固定?
                {
                    bone.RotAxis = MmdReaderUtil.ReadVector3(reader);
                }
                if (bone.AppendRotate || bone.AppendTranslate)//OYM：看不懂,告辞
                {
                    bone.AppendBoneVal.Index = MmdReaderUtil.ReadIndex(reader, pmxConfig.BoneIndexSize);
                    bone.AppendBoneVal.Ratio = reader.ReadSingle();
                }
                if (bone.UseLocalAxis)//OYM：使用本地坐标
                {
                    Vector3 localX = MmdReaderUtil.ReadVector3(reader);
                    Vector3 localZ = MmdReaderUtil.ReadVector3(reader);
                    Vector3 localY = Vector3.Cross(localX, localZ);//OYM：差积出来你也太懒了吧
                    localZ = Vector3.Cross(localX, localY);//OYM：再差积一次防止坐标轴反了?
                    localX.Normalize();
                    localY.Normalize();
                    localZ.Normalize();
                    bone.LocalAxisVal.AxisX = localX;
                    bone.LocalAxisVal.AxisY = localY;
                    bone.LocalAxisVal.AxisZ = localZ;
                }
                if (bone.ReceiveTransform)
                {
                    bone.ExportKey = reader.ReadInt32();
                }
                if (bone.HasIk)
                {
                    ReadBoneIk(reader, bone, pmxConfig.BoneIndexSize);//OYM：来看看别人IK怎么描述位置的
                }

                model.Bones[i] = bone;
            }
        }
        /// <summary>
        /// 读取模型骨骼的IK
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="bone"></param>
        /// <param name="boneIndexSize"></param>
        private static void ReadBoneIk(BinaryReader reader, Bone bone, int boneIndexSize)
        {
            bone.IkInfoVal = new Bone.IkInfo();
            bone.IkInfoVal.IkTargetIndex = MmdReaderUtil.ReadIndex(reader, boneIndexSize);//OYM：IK目标
            bone.IkInfoVal.CcdIterateLimit = reader.ReadInt32();//OYM：我想起来了,这是IK解算的术语
            bone.IkInfoVal.CcdAngleLimit = reader.ReadSingle();//OYM：同上
            var ikLinkNum = reader.ReadInt32();
            bone.IkInfoVal.IkLinks = new Bone.IkLink[ikLinkNum];
            for (var j = 0; j < ikLinkNum; ++j)
            {
                var link = new Bone.IkLink();
                link.LinkIndex = MmdReaderUtil.ReadIndex(reader, boneIndexSize);//OYM：IK的link?
                link.HasLimit = reader.ReadByte() != 0;//OYM：有限制
                if (link.HasLimit)
                {
                    link.LoLimit = MmdReaderUtil.ReadVector3(reader);
                    link.HiLimit = MmdReaderUtil.ReadVector3(reader);
                }
                bone.IkInfoVal.IkLinks[j] = link;//OYM：反正看不懂,等下找下在哪调用的看看好了
            }
        }
        /// <summary>
        /// 这个是负责生成mmd的shader的(抽象比喻
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="config"></param>
        /// <param name="model"></param>
        /// <param name="pmxConfig"></param>
        /// <param name="textureList"></param>
        private static void ReadParts(BinaryReader reader, ModelReadConfig config, MmdModel model, PmxConfig pmxConfig, MmdTexture[] textureList)
        {
            var partNum = reader.ReadInt32();
            var partBaseShift = 0;
            model.Parts = new Part[partNum];
            for (var i = 0; i < partNum; i++)
            {
                var part = new Part();
                var material = ReadMaterial(reader, config, pmxConfig.Encoding, pmxConfig.TextureIndexSize, textureList);
                part.Material = material;
                var partTriangleIndexNum = reader.ReadInt32();//OYM：读出三角形数量,
                if (partTriangleIndexNum % 3 != 0)
                {
                    throw new MmdFileParseException("part" + i + " triangle index count " + partTriangleIndexNum +
                                                   " is not multiple of 3");
                }
                part.BaseShift = partBaseShift;//OYM：这里不一定为零,是一个累加的计算
                part.TriangleIndexNum = partTriangleIndexNum;
                partBaseShift += partTriangleIndexNum;
                model.Parts[i] = part;
            }
        }
        /// <summary>
        /// 读取材质
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="pmxConfig"></param>
        /// <returns></returns>
        private static MmdTexture[] ReadTextureList(BinaryReader reader, PmxConfig pmxConfig)
        {
            var textureNum = reader.ReadInt32();//OYM：材质数目
            var textureList = new MmdTexture[textureNum];
            for (var i = 0; i < textureNum; ++i)
            {
                var texturePathEncoding = pmxConfig.Utf8Encoding ? Encoding.UTF8 : Encoding.Unicode;
                var texturePath = MmdReaderUtil.ReadSizedString(reader, texturePathEncoding);//OYM：获取材质路径
                textureList[i] = new MmdTexture(texturePath);
            }
            return textureList;
        }
        /// <summary>
        /// 读取三角形数目
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="model"></param>
        /// <param name="pmxConfig"></param>
        private static void ReadTriangles(BinaryReader reader, MmdModel model, PmxConfig pmxConfig)
        {
            var triangleIndexCount = reader.ReadInt32();
            model.TriangleIndexes = new int[triangleIndexCount];
            if (triangleIndexCount % 3 != 0)//OYM：如果不是三的整数倍就报错(这样子就代表肯定有个三角形的数据丢失了)
            {
                throw new MmdFileParseException("triangle index count " + triangleIndexCount + " is not multiple of 3");
            }
            for (var i = 0; i < triangleIndexCount; ++i)
            {
                model.TriangleIndexes[i] = MmdReaderUtil.ReadIndex(reader, pmxConfig.VertexIndexSize);//OYM：读取三角形,注意三角形是根据顶点的序号数目生成的
            }
        }
        /// <summary>
        /// 一个读取顶点数目的方法
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="model"></param>
        /// <param name="pmxConfig"></param>
        private static void ReadVertices(BinaryReader reader, MmdModel model, PmxConfig pmxConfig)
        {
            //OYM：不啰嗦了
            var vertexNum = reader.ReadInt32();
            model.Vertices = new Vertex[vertexNum];
            for (uint i = 0; i < vertexNum; ++i)
            {
                var vertex = ReadVertex(reader, pmxConfig);//OYM：对一个顶点进行数据提取
                model.Vertices[i] = vertex;//OYM：赋值
            }
        }

        private static void ReadModelNameAndDescription(BinaryReader reader, MmdModel model, PmxConfig pmxConfig)
        {
            //OYM：一堆读取信息,没什么好看的
            model.Name = MmdReaderUtil.ReadSizedString(reader, pmxConfig.Encoding);
            model.NameEn = MmdReaderUtil.ReadSizedString(reader, pmxConfig.Encoding);
            model.Description = MmdReaderUtil.ReadSizedString(reader, pmxConfig.Encoding);
            model.DescriptionEn = MmdReaderUtil.ReadSizedString(reader, pmxConfig.Encoding);
        }
        /// <summary>
        /// 关键部分
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="pmxConfig"></param>
        /// <returns></returns>
        private static Vertex ReadVertex(BinaryReader reader, PmxConfig pmxConfig)
        {

            var pv = ReadVertexBasic(reader);
            //new一个顶点,赋值

            var vertex = new Vertex
            {
                Coordinate = pv.Coordinate,
                Normal = pv.Normal,
                UvCoordinate = pv.UvCoordinate
            };
            //OYM：如果有额外的uv(在底层设置那里)
            //OYM：也不知道到底有什么用...
            if (pmxConfig.ExtraUvNumber > 0)
            {
                var extraUv = new Vector4[pmxConfig.ExtraUvNumber];
                for (var ei = 0; ei < pmxConfig.ExtraUvNumber; ++ei)
                {
                    extraUv[ei] = MmdReaderUtil.ReadVector4(reader);
                }
                vertex.ExtraUvCoordinate = extraUv;
            }
            
            var op = new SkinningOperator();
            var skinningType = (SkinningOperator.SkinningType)reader.ReadByte();//OYM：蒙皮种类
            op.Type = skinningType;//OYM：看不懂就完事了

            switch (skinningType)//OYM：应该是权重没跑了
            {
                case SkinningOperator.SkinningType.SkinningBdef1:
                    var bdef1 = new SkinningOperator.Bdef1();//OYM：注意这里用了父类可以为任意形式子类的方法,很模糊但是还是要好好学
                    bdef1.BoneId = MmdReaderUtil.ReadIndex(reader, pmxConfig.BoneIndexSize);
                    op.Param = bdef1;
                    break;
                case SkinningOperator.SkinningType.SkinningBdef2:
                    var bdef2 = new SkinningOperator.Bdef2();
                    bdef2.BoneId[0] = MmdReaderUtil.ReadIndex(reader, pmxConfig.BoneIndexSize);
                    bdef2.BoneId[1] = MmdReaderUtil.ReadIndex(reader, pmxConfig.BoneIndexSize);
                    bdef2.BoneWeight = reader.ReadSingle();
                    op.Param = bdef2;
                    break;
                case SkinningOperator.SkinningType.SkinningBdef4:
                    var bdef4 = new SkinningOperator.Bdef4();
                    for (var j = 0; j < 4; ++j)
                    {
                        bdef4.BoneId[j] = MmdReaderUtil.ReadIndex(reader, pmxConfig.BoneIndexSize);
                    }
                    for (var j = 0; j < 4; ++j)
                    {
                        bdef4.BoneWeight[j] = reader.ReadSingle();
                    }
                    op.Param = bdef4;
                    break;
                case SkinningOperator.SkinningType.SkinningSdef://OYM：这个猜不准,到时候再看
                    var sdef = new SkinningOperator.Sdef();
                    sdef.BoneId[0] = MmdReaderUtil.ReadIndex(reader, pmxConfig.BoneIndexSize);
                    sdef.BoneId[1] = MmdReaderUtil.ReadIndex(reader, pmxConfig.BoneIndexSize);
                    sdef.BoneWeight = reader.ReadSingle();
                    sdef.C = MmdReaderUtil.ReadVector3(reader);
                    sdef.R0 = MmdReaderUtil.ReadVector3(reader);
                    sdef.R1 = MmdReaderUtil.ReadVector3(reader);
                    op.Param = sdef;
                    break;
                default:
                    throw new MmdFileParseException("invalid skinning type: " + skinningType);
            }
            vertex.SkinningOperator = op;//OYM：顶点的蒙皮操作
            vertex.EdgeScale = reader.ReadSingle();//OYM：这个实在猜不准,但是考虑到就一个单精度,猜一个倍率好了
            return vertex;
        }
        /// <summary>
        /// 获取这个material各种杂七杂八的属性
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="config"></param>
        /// <param name="encoding"></param>
        /// <param name="textureIndexSize"></param>
        /// <param name="textureList"></param>
        /// <returns></returns>
        private static MmdMaterial ReadMaterial(BinaryReader reader, ModelReadConfig config, Encoding encoding,
            int textureIndexSize, MmdTexture[] textureList)
        {
            //OYM：别想着优化这一坨代码,人家按顺序来的
            var material = new MmdMaterial();
            material.Name = MmdReaderUtil.ReadSizedString(reader, encoding);
            material.NameEn = MmdReaderUtil.ReadSizedString(reader, encoding);
            material.DiffuseColor = MmdReaderUtil.ReadColor(reader, true);
            material.SpecularColor = MmdReaderUtil.ReadColor(reader, false);
            material.Shiness = reader.ReadSingle();
            material.AmbientColor = MmdReaderUtil.ReadColor(reader, false);
            var drawFlag = reader.ReadByte();
            //OYM：下面一排位运算,谁有兴趣去看一眼?
            material.DrawDoubleFace = (drawFlag & PmxMaterialDrawFlags.PmxMaterialDrawDoubleFace) != 0;
            material.DrawGroundShadow = (drawFlag & PmxMaterialDrawFlags.PmxMaterialDrawGroundShadow) != 0;
            material.CastSelfShadow = (drawFlag & PmxMaterialDrawFlags.PmxMaterialCastSelfShadow) != 0;
            material.DrawSelfShadow = (drawFlag & PmxMaterialDrawFlags.PmxMaterialDrawSelfShadow) != 0;
            material.DrawEdge = (drawFlag & PmxMaterialDrawFlags.PmxMaterialDrawEdge) != 0;
            material.EdgeColor = MmdReaderUtil.ReadColor(reader, true);
            material.EdgeSize = reader.ReadSingle();
            var textureIndex = MmdReaderUtil.ReadIndex(reader, textureIndexSize);//OYM：后面是选择相应的着色器
            if (textureIndex < textureList.Length && textureIndex >= 0)
            {
                material.Texture = textureList[textureIndex];
            }
            var subTextureIndex = MmdReaderUtil.ReadIndex(reader, textureIndexSize);
            if (subTextureIndex < textureList.Length && subTextureIndex >= 0)
            {
                material.SubTexture = textureList[subTextureIndex];
            }
            material.SubTextureType = (MmdMaterial.SubTextureTypeEnum) reader.ReadByte();
            var useGlobalToon = reader.ReadByte() != 0;
            if (useGlobalToon)
            {
                int globalToonIndex = reader.ReadByte();
                material.Toon = MmdTextureUtil.GetGlobalToon(globalToonIndex, config.GlobalToonPath);
            }
            else
            {
                var toonIndex = MmdReaderUtil.ReadIndex(reader, textureIndexSize);
                if (toonIndex < textureList.Length && toonIndex >= 0)
                {
                    material.Toon = textureList[toonIndex];
                }
            }
            material.MetaInfo = MmdReaderUtil.ReadSizedString(reader, encoding);
            return material;
        }

        private static PmxMeta ReadMeta(BinaryReader reader)
        {
            PmxMeta ret;
            ret.Magic = MmdReaderUtil.ReadStringFixedLength(reader, 4, Encoding.ASCII);
            ret.Version = reader.ReadSingle();
            ret.FileFlagSize = reader.ReadByte();
            return ret;
        }
        /// <summary>
        /// 传出来一个顶点的基础信息
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static PmxVertexBasic ReadVertexBasic(BinaryReader reader)
        {
            
            PmxVertexBasic ret;
            ret.Coordinate = MmdReaderUtil.ReadVector3(reader);
            ret.Normal = MmdReaderUtil.ReadVector3(reader);
            ret.UvCoordinate = MmdReaderUtil.ReadVector2(reader);
            return ret;
        }


        private struct PmxMeta
        {
            public string Magic;
            public float Version;
            public byte FileFlagSize;
        }
        /// <summary>
        /// 顶点的基础信息
        /// </summary>
        private struct PmxVertexBasic
        {
            /// <summary>
            /// 坐标
            /// </summary>
            public Vector3 Coordinate;
            /// <summary>
            /// 法线朝向
            /// </summary>
            public Vector3 Normal;
            /// <summary>
            /// UV坐标
            /// </summary>
            public Vector2 UvCoordinate;
        }

        private class PmxConfig
        {
            public bool Utf8Encoding { get; set; }
            public Encoding Encoding { get; set; }
            public int ExtraUvNumber { get; set; }
            public int VertexIndexSize { get; set; }
            public int TextureIndexSize { get; set; }
            public int MaterialIndexSize{ get; set; }
            public int BoneIndexSize { get; set; }
            public int MorphIndexSize  { get; set; }
            public int RigidBodyIndexSize { get; set; }
        }
        /// <summary>
        /// 位运算用的比较符,看不懂,告辞
        /// </summary>
        private abstract class PmxMaterialDrawFlags
        {
            public const byte PmxMaterialDrawDoubleFace = 0x01;
            public const byte PmxMaterialDrawGroundShadow = 0x02;
            public const byte PmxMaterialCastSelfShadow = 0x04;
            public const byte PmxMaterialDrawSelfShadow = 0x08;
            public const byte PmxMaterialDrawEdge = 0x10;
        }

        private abstract class PmxBoneFlags
        {
            public const ushort PmxBoneChildUseId = 0x0001;
            public const ushort PmxBoneRotatable = 0x0002;
            public const ushort PmxBoneMovable = 0x0004;
            public const ushort PmxBoneVisible = 0x0008;
            public const ushort PmxBoneControllable = 0x0010;
            public const ushort PmxBoneHasIk = 0x0020;
            public const ushort PmxBoneAcquireRotate = 0x0100;
            public const ushort PmxBoneAcquireTranslate = 0x0200;
            public const ushort PmxBoneRotAxisFixed = 0x0400;
            public const ushort PmxBoneUseLocalAxis = 0x0800;
            public const ushort PmxBonePostPhysics = 0x1000;
            public const ushort PmxBoneReceiveTransform = 0x2000;
        }
    }
}