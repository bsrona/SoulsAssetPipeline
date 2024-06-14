using Havoc.IO.Tagfile.Binary;
using Havoc.Objects;
using Havoc.Reflection;
using HKX2;
using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SoulsAssetPipeline.Animation
{
    public partial class HKX : SoulsFile<HKX>
    {
        private static object _lock_GenFakeFromTagFile = new object();
        public static HKX GenFakeFromTagFile(byte[] tagFile, byte[] compendium = null)
        {
            HKX meme = new HKX();
            lock (_lock_GenFakeFromTagFile)
            {
                IHkObject SelectFieldFromClass(IReadOnlyDictionary<HkField, IHkObject> havokClass, string fieldName)
                {
                    foreach (var kvp in havokClass as IReadOnlyDictionary<HkField, IHkObject>)
                    {
                        // namedVariants
                        if (kvp.Key.Name == fieldName)
                        {
                            return kvp.Value;
                        }
                    }
                    return null;
                }

                List<HKXObject> hkxObjectsFound = new List<HKXObject>();

                IHkObject obj = null;
                //try
                //{
                //    obj = HkBinaryTagfileReader.Read(tagFile, null);
                //}
                //catch
                //{
                //    obj = HkBinaryTagfileReader.Read(tagFile, compendium);
                //}
                obj = HkBinaryTagfileReader.Read(tagFile, compendium);

                if (obj.Type.Name == "hkRootLevelContainer")
                {
                    void DoVariant (IHkObject mainVariant)
                    {
                        var mainVariantClass = (SelectFieldFromClass(mainVariant.Value as IReadOnlyDictionary<HkField, IHkObject>, "variant") as HkPtr).Value;

                        var skeletons = (SelectFieldFromClass(mainVariantClass.Value as IReadOnlyDictionary<HkField, IHkObject>, "skeletons") as HkArray);
                        if (skeletons?.Value != null)
                        {
                            foreach (var skelPtr in skeletons.Value as IReadOnlyList<IHkObject>)
                            {
                                var skel = skelPtr.Value as HkClass;
                                if (skel == null)
                                    continue;
                                HKASkeleton hkaSkeleton = new HKASkeleton();
                                foreach (var kvp in skel.Value as IReadOnlyDictionary<HkField, IHkObject>)
                                {
                                    if (kvp.Key.Name == "name")
                                        hkaSkeleton.Name = new HKCString((kvp.Value as HkString).Value);
                                    else if (kvp.Key.Name == "parentIndices")
                                    {
                                        var arrData = new HKArrayData<HKShort>();
                                        arrData.Elements = new List<HKShort>();

                                        foreach (var arrEntry in kvp.Value.Value as IReadOnlyList<IHkObject>)
                                        {
                                            arrData.Elements.Add(new HKShort((arrEntry as HkInt16).Value));
                                        }

                                        hkaSkeleton.ParentIndices = new HKArray<HKShort>(arrData);
                                    }
                                    else if (kvp.Key.Name == "bones")
                                    {
                                        var arrData = new HKArrayData<Bone>();
                                        arrData.Elements = new List<Bone>();

                                        foreach (var arrEntry in kvp.Value.Value as IReadOnlyList<IHkObject>)
                                        {
                                            var boneName = SelectFieldFromClass(arrEntry.Value as IReadOnlyDictionary<HkField, IHkObject>, "name") as HkString;
                                            var newBone = new HKX.Bone();
                                            newBone.Name = new HKCString(boneName.Value);
                                            arrData.Elements.Add(newBone);
                                        }

                                        hkaSkeleton.Bones = new HKArray<Bone>(arrData);
                                    }
                                    else if (kvp.Key.Name == "referencePose")
                                    {
                                        var arrData = new HKArrayData<Transform>();
                                        arrData.Elements = new List<Transform>();

                                        foreach (var arrEntry in kvp.Value.Value as IReadOnlyList<IHkObject>)
                                        {
                                            var translation = ((SelectFieldFromClass(arrEntry.Value as IReadOnlyDictionary<HkField, IHkObject>, "translation") as IHkObject).Value as IReadOnlyList<IHkObject>).Cast<HkSingle>().ToList();
                                            var rotation = ((SelectFieldFromClass(arrEntry.Value as IReadOnlyDictionary<HkField, IHkObject>, "rotation") as IHkObject).Value as IReadOnlyList<IHkObject>).Cast<HkSingle>().ToList();
                                            var scale = ((SelectFieldFromClass(arrEntry.Value as IReadOnlyDictionary<HkField, IHkObject>, "scale") as IHkObject).Value as IReadOnlyList<IHkObject>).Cast<HkSingle>().ToList();
                                            Transform t = new Transform();
                                            t.Position = new HKVector4(new System.Numerics.Vector4(translation[0].Value, translation[1].Value, translation[2].Value, translation[3].Value));
                                            t.Rotation = new HKVector4(new System.Numerics.Vector4(rotation[0].Value, rotation[1].Value, rotation[2].Value, rotation[3].Value));
                                            t.Scale = new HKVector4(new System.Numerics.Vector4(scale[0].Value, scale[1].Value, scale[2].Value, scale[3].Value));
                                            arrData.Elements.Add(t);
                                        }

                                        hkaSkeleton.Transforms = new HKArray<Transform>(arrData);

                                    }

                                    
                                }

                                hkaSkeleton.ReferenceFloats = new HKArray<HKFloat>(new HKArrayData<HKFloat>()
                                {
                                    Elements = new List<HKFloat>() { }
                                });

                                hkxObjectsFound.Add(hkaSkeleton);
                            }
                        }

                        void DoAnim(HkClass anim)
                        {
                            if (anim.Type.Name == "hkaSplineCompressedAnimation")
                            {
                                HKASplineCompressedAnimation hkaSplineCompressedAnimation = new HKASplineCompressedAnimation();
                                hkaSplineCompressedAnimation.AnimationType = AnimationType.HK_SPLINE_COMPRESSED_ANIMATION;
                                foreach (var kvp in anim.Value as IReadOnlyDictionary<HkField, IHkObject>)
                                {
                                    if (kvp.Key.Name == "duration")
                                        hkaSplineCompressedAnimation.Duration = (kvp.Value as HkSingle).Value;
                                    else if (kvp.Key.Name == "numberOfTransformTracks")
                                        hkaSplineCompressedAnimation.TransformTrackCount = (kvp.Value as HkInt32).Value;
                                    //else if (kvp.Key.Name == "numberOfFloatTracks")
                                    //    hkaSplineCompressedAnimation.FloatTrackCount = (kvp.Value as HkInt32).Value;

                                    else if (kvp.Key.Name == "extractedMotion")
                                    {
                                        if (kvp.Value != null)
                                        {
                                            var refFrameClass = (kvp.Value.Value as HkClass);
                                            if (refFrameClass != null)
                                            {
                                                HKADefaultAnimatedReferenceFrame hkaDefaultAnimatedReferenceFrame = new HKADefaultAnimatedReferenceFrame();

                                                foreach (var k in refFrameClass.Value as IReadOnlyDictionary<HkField, IHkObject>)
                                                {
                                                    if (k.Key.Name == "up")
                                                    {
                                                        var floatList = (k.Value.Value as IReadOnlyList<IHkObject>).Select(x => ((HkSingle)x).Value).ToList();
                                                        hkaDefaultAnimatedReferenceFrame.Up = new System.Numerics.Vector4(floatList[0], floatList[1], floatList[2], floatList[3]);
                                                    }
                                                    else if (k.Key.Name == "forward")
                                                    {
                                                        var floatList = (k.Value.Value as IReadOnlyList<IHkObject>).Select(x => ((HkSingle)x).Value).ToList();
                                                        hkaDefaultAnimatedReferenceFrame.Forward = new System.Numerics.Vector4(floatList[0], floatList[1], floatList[2], floatList[3]);
                                                    }
                                                    else if (k.Key.Name == "duration")
                                                    {
                                                        hkaDefaultAnimatedReferenceFrame.Duration = (k.Value as HkSingle).Value;
                                                    }
                                                    else if (k.Key.Name == "referenceFrameSamples")
                                                    {
                                                        var arrData = new HKArrayData<HKVector4>();
                                                        arrData.Elements = new List<HKVector4>();
                                                        foreach (var refSampleArr in (k.Value.Value as IReadOnlyList<IHkObject>))
                                                        {
                                                            var refSample = (refSampleArr.Value as IReadOnlyList<IHkObject>).Select(x => ((HkSingle)x).Value).ToList();
                                                            arrData.Elements.Add(new HKVector4(new System.Numerics.Vector4(refSample[0], refSample[1], refSample[2], refSample[3])));
                                                        }
                                                        hkaDefaultAnimatedReferenceFrame.ReferenceFrameSamples = new HKArray<HKVector4>(arrData);
                                                    }
                                                }
                                                hkxObjectsFound.Add(hkaDefaultAnimatedReferenceFrame);
                                            }
                                        }
                                    }
                                    //else if (kvp.Key.Name == "annotationTracks")
                                    //{
                                    //    if (kvp.Value != null)
                                    //    {
                                    //        hkaSplineCompressedAnimation.
                                    //        foreach (var k in kvp.Value.Value as IReadOnlyList<HkClass>)
                                    //        {
                                    //            string trackName = (SelectFieldFromClass(k.Value as IReadOnlyDictionary<HkField, IHkObject>, "trackName") as HkString).Value;
                                    //        }
                                    //    }
                                    //}
                                    else if (kvp.Key.Name == "numFrames")
                                        hkaSplineCompressedAnimation.FrameCount = (kvp.Value as HkInt32).Value;
                                    else if (kvp.Key.Name == "numBlocks")
                                        hkaSplineCompressedAnimation.BlockCount = (kvp.Value as HkInt32).Value;
                                    else if (kvp.Key.Name == "maxFramesPerBlock")
                                        hkaSplineCompressedAnimation.FramesPerBlock = (kvp.Value as HkInt32).Value;
                                    else if (kvp.Key.Name == "maskAndQuantizationSize")
                                        hkaSplineCompressedAnimation.MaskAndQuantization = (uint)((kvp.Value as HkInt32).Value);
                                    else if (kvp.Key.Name == "blockDuration")
                                        hkaSplineCompressedAnimation.BlockDuration = (kvp.Value as HkSingle).Value;
                                    else if (kvp.Key.Name == "blockInverseDuration")
                                        hkaSplineCompressedAnimation.InverseBlockDuration = (kvp.Value as HkSingle).Value;
                                    else if (kvp.Key.Name == "frameDuration")
                                        hkaSplineCompressedAnimation.FrameDuration = (kvp.Value as HkSingle).Value;
                                    else if (kvp.Key.Name == "data")
                                    {
                                        var arrData = new HKArrayData<HKByte>();
                                        arrData.Elements = new List<HKByte>();
                                        foreach (var arrEntry in kvp.Value.Value as IReadOnlyList<IHkObject>)
                                        {
                                            arrData.Elements.Add(new HKByte((arrEntry as HkByte).Value));
                                        }

                                        hkaSplineCompressedAnimation.Data = new HKArray<HKByte>(arrData);

                                        hkaSplineCompressedAnimation.BlockOffsets = new HKArray<HKUInt>(new HKArrayData<HKUInt>()
                                        {
                                            Elements = new List<HKUInt>() { new HKUInt() { data = 0 } }
                                        });

                                        hkaSplineCompressedAnimation.FloatBlockOffsets = new HKArray<HKUInt>(new HKArrayData<HKUInt>()
                                        {
                                            Elements = new List<HKUInt>() { new HKUInt() { data = (uint)arrData.Elements.Count - 1 } }
                                        });

                                        hkaSplineCompressedAnimation.Endian = 0;

                                        hkaSplineCompressedAnimation.TransformOffsets = new HKArray<HKUInt>(new HKArrayData<HKUInt>()
                                        {
                                            Elements = new List<HKUInt>() { }
                                        });

                                        hkaSplineCompressedAnimation.FloatOffsets = new HKArray<HKUInt>(new HKArrayData<HKUInt>()
                                        {
                                            Elements = new List<HKUInt>() { }
                                        });
                                    }
                                }

                                hkxObjectsFound.Add(hkaSplineCompressedAnimation);
                            }
                            else if (anim.Type.Name == "hkaInterleavedUncompressedAnimation")
                            {
                                HKAInterleavedUncompressedAnimation hkaInterleavedUncompressedAnimation = new HKAInterleavedUncompressedAnimation();
                                hkaInterleavedUncompressedAnimation.AnimationType = AnimationType.HK_INTERLEAVED_ANIMATION;
                                foreach (var kvp in anim.Value as IReadOnlyDictionary<HkField, IHkObject>)
                                {
                                    if (kvp.Key.Name == "duration")
                                        hkaInterleavedUncompressedAnimation.Duration = (kvp.Value as HkSingle).Value;
                                    else if (kvp.Key.Name == "numberOfTransformTracks")
                                        hkaInterleavedUncompressedAnimation.TransformTrackCount = (kvp.Value as HkInt32).Value;
                                    else if (kvp.Key.Name == "numberOfFloatTracks")
                                        hkaInterleavedUncompressedAnimation.FloatTrackCount = (kvp.Value as HkInt32).Value;



                                    else if (kvp.Key.Name == "extractedMotion")
                                    {
                                        if (kvp.Value != null)
                                        {
                                            var refFrameClass = (kvp.Value.Value as HkClass);
                                            if (refFrameClass != null)
                                            {
                                                HKADefaultAnimatedReferenceFrame hkaDefaultAnimatedReferenceFrame = new HKADefaultAnimatedReferenceFrame();

                                                foreach (var k in refFrameClass.Value as IReadOnlyDictionary<HkField, IHkObject>)
                                                {
                                                    if (k.Key.Name == "up")
                                                    {
                                                        var floatList = (k.Value.Value as IReadOnlyList<IHkObject>).Select(x => ((HkSingle)x).Value).ToList();
                                                        hkaDefaultAnimatedReferenceFrame.Up = new System.Numerics.Vector4(floatList[0], floatList[1], floatList[2], floatList[3]);
                                                    }
                                                    else if (k.Key.Name == "forward")
                                                    {
                                                        var floatList = (k.Value.Value as IReadOnlyList<IHkObject>).Select(x => ((HkSingle)x).Value).ToList();
                                                        hkaDefaultAnimatedReferenceFrame.Forward = new System.Numerics.Vector4(floatList[0], floatList[1], floatList[2], floatList[3]);
                                                    }
                                                    else if (k.Key.Name == "duration")
                                                    {
                                                        hkaDefaultAnimatedReferenceFrame.Duration = (k.Value as HkSingle).Value;
                                                    }
                                                    else if (k.Key.Name == "referenceFrameSamples")
                                                    {
                                                        var arrData = new HKArrayData<HKVector4>();
                                                        arrData.Elements = new List<HKVector4>();
                                                        foreach (var refSampleArr in (k.Value.Value as IReadOnlyList<IHkObject>))
                                                        {
                                                            var refSample = (refSampleArr.Value as IReadOnlyList<IHkObject>).Select(x => ((HkSingle)x).Value).ToList();
                                                            arrData.Elements.Add(new HKVector4(new System.Numerics.Vector4(refSample[0], refSample[1], refSample[2], refSample[3])));
                                                        }
                                                        hkaDefaultAnimatedReferenceFrame.ReferenceFrameSamples = new HKArray<HKVector4>(arrData);
                                                    }
                                                }
                                                hkxObjectsFound.Add(hkaDefaultAnimatedReferenceFrame);
                                            }
                                        }
                                    }
                                    //else if (kvp.Key.Name == "annotationTracks")
                                    //{
                                    //    if (kvp.Value != null)
                                    //    {
                                    //        hkaSplineCompressedAnimation.
                                    //        foreach (var k in kvp.Value.Value as IReadOnlyList<HkClass>)
                                    //        {
                                    //            string trackName = (SelectFieldFromClass(k.Value as IReadOnlyDictionary<HkField, IHkObject>, "trackName") as HkString).Value;
                                    //        }
                                    //    }
                                    //}
                                    //else if (kvp.Key.Name == "numFrames")
                                    //    hkaInterleavedUncompressedAnimation.FrameCount = (kvp.Value as HkInt32).Value;
                                    //else if (kvp.Key.Name == "numBlocks")
                                    //    hkaInterleavedUncompressedAnimation.BlockCount = (kvp.Value as HkInt32).Value;
                                    //else if (kvp.Key.Name == "maxFramesPerBlock")
                                    //    hkaInterleavedUncompressedAnimation.FramesPerBlock = (kvp.Value as HkInt32).Value;
                                    //else if (kvp.Key.Name == "maskAndQuantizationSize")
                                    //    hkaInterleavedUncompressedAnimation.MaskAndQuantization = (uint)((kvp.Value as HkInt32).Value);
                                    //else if (kvp.Key.Name == "blockDuration")
                                    //    hkaInterleavedUncompressedAnimation.BlockDuration = (kvp.Value as HkSingle).Value;
                                    //else if (kvp.Key.Name == "blockInverseDuration")
                                    //    hkaInterleavedUncompressedAnimation.InverseBlockDuration = (kvp.Value as HkSingle).Value;
                                    //else if (kvp.Key.Name == "frameDuration")
                                    //    hkaInterleavedUncompressedAnimation.FrameDuration = (kvp.Value as HkSingle).Value;
                                    else if (kvp.Key.Name == "transforms")
                                    {
                                        var arrData = new HKArrayData<Transform>();
                                        arrData.Elements = new List<Transform>();
                                        foreach (var arrEntry in kvp.Value.Value as IReadOnlyList<IHkObject>)
                                        {
                                            var trClass = arrEntry as HkClass;
                                            var tr = new Transform();
                                            foreach (var k in trClass.Value as IReadOnlyDictionary<HkField, IHkObject>)
                                            {
                                                if (k.Key.Name == "translation")
                                                {
                                                    var floatList = (k.Value.Value as IReadOnlyList<IHkObject>).Select(x => ((HkSingle)x).Value).ToList();
                                                    tr.Position = new HKVector4(new System.Numerics.Vector4(floatList[0], floatList[1], floatList[2], floatList[3]));
                                                }
                                                else if (k.Key.Name == "rotation")
                                                {
                                                    var floatList = (k.Value.Value as IReadOnlyList<IHkObject>).Select(x => ((HkSingle)x).Value).ToList();
                                                    tr.Rotation = new HKVector4(new System.Numerics.Vector4(floatList[0], floatList[1], floatList[2], floatList[3]));
                                                }
                                                if (k.Key.Name == "scale")
                                                {
                                                    var floatList = (k.Value.Value as IReadOnlyList<IHkObject>).Select(x => ((HkSingle)x).Value).ToList();
                                                    tr.Scale = new HKVector4(new System.Numerics.Vector4(floatList[0], floatList[1], floatList[2], floatList[3]));
                                                }
                                            }
                                            arrData.Elements.Add(tr);
                                        }

                                        hkaInterleavedUncompressedAnimation.Transforms = new HKArray<Transform>(arrData);
                                    }
                                }

                                hkxObjectsFound.Add(hkaInterleavedUncompressedAnimation);
                            }
                            else
                            {
                                throw new NotImplementedException($"Animation type {anim.Type.Name} not implemented for TagFile read.");
                            }
                        }

                        var animations = (SelectFieldFromClass(mainVariantClass.Value as IReadOnlyDictionary<HkField, IHkObject>, "animations") as HkArray);
                        if (animations?.Value != null)
                        {
                            foreach (var animPtr in animations.Value as IReadOnlyList<IHkObject>)
                            {
                                var anim = animPtr.Value as HkClass;
                                DoAnim(anim);
                            }
                        }
                        //else if (animations.Type.Format == HkTypeFormat.Array)
                        //{
                        //    animations.Type.Fields.FirstOrDefault(x => x.Name == "m")
                        //}

                        var bindings = (SelectFieldFromClass(mainVariantClass.Value as IReadOnlyDictionary<HkField, IHkObject>, "bindings") as HkArray);
                        if (bindings?.Value != null)
                        {
                            foreach (var bindingPtr in bindings.Value as IReadOnlyList<IHkObject>)
                            {
                                var binding = bindingPtr.Value as HkClass;
                                HKAAnimationBinding hkaAnimationBinding = new HKAAnimationBinding();

                                hkaAnimationBinding.FloatTrackToFloatSlotIndices = new HKArray<HKShort>(new HKArrayData<HKShort>()
                                {
                                    Elements = new List<HKShort>() { }
                                });

                                hkaAnimationBinding.PartitionIndices = new HKArray<HKShort>(new HKArrayData<HKShort>()
                                {
                                    Elements = new List<HKShort>() { }
                                });

                                foreach (var kvp in binding.Value as IReadOnlyDictionary<HkField, IHkObject>)
                                {

                                    if (kvp.Key.Name == "originalSkeletonName")
                                        hkaAnimationBinding.OriginalSkeletonName = (kvp.Value as HkString).Value;
                                    else if (kvp.Key.Name == "transformTrackToBoneIndices")
                                    {
                                        var arrData = new HKArrayData<HKShort>();
                                        arrData.Elements = new List<HKShort>();
                                        foreach (var arrEntry in kvp.Value.Value as IReadOnlyList<IHkObject>)
                                        {
                                            arrData.Elements.Add(new HKShort((arrEntry as HkInt16).Value));
                                        }

                                        hkaAnimationBinding.TransformTrackToBoneIndices = new HKArray<HKShort>(arrData);
                                    }
                                    else if (kvp.Key.Name == "blendHint")
                                        hkaAnimationBinding.BlendHint = (AnimationBlendHint)((kvp.Value as HkSByte).Value);

                                }
                                hkxObjectsFound.Add(hkaAnimationBinding);
                            }
                        }
                    }

                    var namedVariants = SelectFieldFromClass(obj.Value as IReadOnlyDictionary<HkField, IHkObject>, "namedVariants") as HkArray;
                    //var mainVariant = namedVariants.Value[0];

                    foreach (var variant in namedVariants.Value)
                        DoVariant(variant);

                }


                meme.DataSection = new HKXSection();
                meme.DataSection.Objects = hkxObjectsFound;
            }
            return meme;
        }

		private static object _lock_Load = new object();
		public static IHavokObject Load(byte[] tagFile, byte[] compendium = null)
		{
			lock (_lock_Load)
			{
                Dictionary<HkClass, IHavokObject> objectDic = new Dictionary<HkClass, IHavokObject>();
				HkClass rootObject = HkBinaryTagfileReader.Read(tagFile, compendium) as HkClass;
                IHavokObject havokObject = CreateHKObject(rootObject, objectDic);
				return havokObject;
			}
		}

        static IHavokObject CreateHKObject(HkClass streamObject, Dictionary<HkClass, IHavokObject> objectDic)
        {
            IHavokObject hkObject = null;

            if (objectDic.TryGetValue(streamObject, out hkObject))
            {
                return hkObject;
            }

            HkType hkType = streamObject.Type;
            hkObject = CreateHKObject(hkType);

            if (hkObject != null)
            {
				objectDic.Add(streamObject, hkObject);

				IReadOnlyDictionary<HkField, IHkObject> hkFieldDic = streamObject.Value as IReadOnlyDictionary<HkField, IHkObject>;

				Type type = hkObject.GetType();
                FieldInfo[] fields = type.GetFields();
                foreach (var pair in hkFieldDic)
                {
                    HkField hkField = pair.Key;
                    FieldInfo field = FindFieldInfo(fields, hkField);
                    if (field == null)
                        continue;

                    IHkObject value = pair.Value;
                    SetHKField(hkObject, field, value, objectDic);
                }

				for (int i = 0; i < fields.Length; ++i)
				{
					FieldInfo field = fields[i];
					IHkObject value = FindHKField(field, hkFieldDic);
					if (value == null)
						continue;
					//SetHKField(hkObject, field, value, objectDic);
				}
			}

			return hkObject;
		}

		static readonly Type[] ConstructorParameters = new Type[0];
		static IHavokObject CreateHKObject(HkType hkType)
		{
			string hkTypeName = hkType.Name;
            hkTypeName = hkTypeName.Replace("::", "");
			Type type = Type.GetType($"HKX2.{hkTypeName}");
			ConstructorInfo constructorInfo = type.GetConstructor(ConstructorParameters);
			IHavokObject hkObject = constructorInfo.Invoke(null) as IHavokObject;
			return hkObject;
		}

		static IHkObject FindHKField(FieldInfo fieldInfo, IReadOnlyDictionary<HkField, IHkObject> hkFieldDic)
		{
			foreach(var pair in hkFieldDic)
			{
				HkField field = pair.Key;
				IHkObject value = pair.Value;

				if (fieldInfo.Name.Substring(2) != field.Name)
					continue;

				return value;
			}

			return null;
		}

		static FieldInfo FindFieldInfo(FieldInfo[] fields, HkField hkField)
        {
            FieldInfo fieldInfo = Array.Find(fields, e => e.Name.Substring(2) == hkField.Name);
            return fieldInfo;
		}

		static void SetHKField(IHavokObject hkObject, FieldInfo fieldInfo, IHkObject hkValueObject, Dictionary<HkClass, IHavokObject> objectDic)
		{
            object value = ConvertHkValue(hkValueObject, objectDic);

            if (hkValueObject.Type.Format == HkTypeFormat.Array)
            {
				List<object> elements = value as List<object>;
                if (elements != null)
                {
					value = Activator.CreateInstance(fieldInfo.FieldType);
					AddElements(value, elements);
				}
			}

			fieldInfo.SetValue(hkObject, value);
		}

        static object ConvertHkValue(IHkObject hkValue, Dictionary<HkClass, IHavokObject> objectDic)
        {
            object value = null;

		    HkType hkType = hkValue.Type;
			switch (hkType.Format)
			{
				case HkTypeFormat.Void:
				case HkTypeFormat.Opaque:
					break;

				case HkTypeFormat.Bool:
					{
						var valueObject = hkValue as HkBool;
                        value = valueObject.Value;
					}
                    break;

				case HkTypeFormat.String:
					{
                        var valueObject = hkValue as HkString;
						value = valueObject.Value;
					}
                    break;

				case HkTypeFormat.Int:
					{
                        int bitCount = hkType.BitCount;
                        bool isSigned = hkType.IsSigned;
						switch(bitCount)
						{
							case 8:
                                {
                                    if (isSigned)
                                    {
										var valueObject = hkValue as HkSByte;
										value = valueObject.Value;
									}
									else
                                    {
										var valueObject = hkValue as HkByte;
										value = valueObject.Value;
									}
								}
								break;

							case 16:
								{
									if(isSigned)
									{
										var valueObject = hkValue as HkInt16;
										value = valueObject.Value;
									}
									else
									{
										var valueObject = hkValue as HkUInt16;
										value = valueObject.Value;
									}
								}
								break;

							case 32:
								{
									if(isSigned)
									{
										var valueObject = hkValue as HkInt32;
										value = valueObject.Value;
									}
									else
									{
										var valueObject = hkValue as HkUInt32;
										value = valueObject.Value;
									}
								}
								break;

							case 64:
								{
									if(isSigned)
									{
										var valueObject = hkValue as HkInt64;
										value = valueObject.Value;
									}
									else
									{
										var valueObject = hkValue as HkUInt64;
										value = valueObject.Value;
									}
								}
								break;

							default:
								throw new InvalidDataException($"Unexpected bit count: {bitCount}");
						}
					}
                    break;

				case HkTypeFormat.FloatingPoint:
					{
						if (hkType.IsHalf)
                        {
                            var valueObject = hkValue as HkHalf;
                            value = new hknpHalf() { m_value = valueObject.Value };
                        }
                        else if (hkType.IsSingle)
						{
							var valueObject = hkValue as HkSingle;
							value = valueObject.Value;
						}
						else if(hkType.IsDouble)
						{
							var valueObject = hkValue as HkDouble;
							value = valueObject.Value;
						}
						else
						{
							throw new InvalidDataException("Unexpected floating point format");
						}
					}
                    break;

				case HkTypeFormat.Ptr:
					{
                        var valueObject = hkValue as HkPtr;
                        HkClass pointHKObject = valueObject.Value as HkClass;
                        if (pointHKObject != null)
                            value = CreateHKObject(pointHKObject, objectDic);
					}
                    break;

				case HkTypeFormat.Class:
					{
                        var valueObject = hkValue as HkClass;
						value = CreateHKObject(valueObject, objectDic);
					}
                    break;

				case HkTypeFormat.Array:
					{
						var valueObject = hkValue as HkArray;
						IReadOnlyList<IHkObject> elements = valueObject.Value;

                        if (elements != null)
                        {
							var array = new List<object>(elements.Count);

							for (int i = 0; i < elements.Count; ++i)
							{
								var element = elements[i];
								object elementObject = ConvertHkValue(element, objectDic);
								array.Add(elementObject);
							}

							if (hkType.Name == "hkVector4" || hkType.Name == "hkVector4f")
							{
								Vector4 result = new Vector4((float)array[0], (float)array[1], (float)array[2], (float)array[3]);
								value = result;
							}
							else if (hkType.Name == "hkQuaternion" || hkType.Name == "hkQuaternionf")
							{
								Quaternion result = new Quaternion((float)array[0], (float)array[1], (float)array[2], (float)array[3]);
								value = result;
							}
							else if(hkType.Name == "hkTransform" || hkType.Name == "hkTransformf")
							{
								Matrix4x4 result = new Matrix4x4
                                (
                                    (float)array[0], (float)array[1], (float)array[2], (float)array[3],
									(float)array[4], (float)array[5], (float)array[6], (float)array[7],
									(float)array[8], (float)array[9], (float)array[10], (float)array[11],
									(float)array[12], (float)array[13], (float)array[14], (float)array[15]
								);
								value = result;
							}
							else if(hkType.Name == "hkMatrix3" || hkType.Name == "hkMatrix3")
							{
								Matrix4x4 result = new Matrix4x4
								(
									(float)array[0], (float)array[1], (float)array[2], (float)array[3],
									(float)array[4], (float)array[5], (float)array[6], (float)array[7],
									(float)array[8], (float)array[9], (float)array[10], (float)array[11],
									0, 0, 0, 1
								);
								value = result;
							}
							else
							{
								value = array;
							}
						}
					}
                    break;

				default:
					throw new ArgumentOutOfRangeException(nameof(hkType.Format));
			}

            return value;
        }

        static void AddElements(object target, List<object> source)
        {
            MethodInfo method = target.GetType().GetMethod("Add");
            object[] parameters = new object[1];

            for (int i = 0; i < source.Count; ++i)
            {
                object element = source[i];
                parameters[0] = element;
                method.Invoke(target, parameters);
            }
        }
	}
}
