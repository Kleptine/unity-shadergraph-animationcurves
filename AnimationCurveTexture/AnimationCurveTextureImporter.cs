using System;
using System.Linq;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

// To create a new curve texture:
//  1. Create an empty file with the 'curve_texture' extension. 
//  2. In Unity, select the file, and tweak the AnimationCurve in the inspector. Hit 'apply' when done.
//  3. The file will now act like a Texture2D in Unity. It contains the curve, baked into a R8 texture.
//
// To use the baked curve:
//   - In shader graph, create a new "SampleCurveTexture" node. It's included in this folder.

namespace Editor.CustomImporters.AnimationCurveTexture
{
    /// <summary>Generates a Texture2D asset from the given AnimationCurve. The 'file' that is imported can just be empty.</summary>
    [ScriptedImporter(1, "curve_texture")]
    public class AnimationCurveTextureImporter : ScriptedImporter
    {
        /// <summary>A simplified version of <see cref="TextureFormat" />. Only the formats that make sense.</summary>
        /// <remarks>
        ///     There's really no reason to use any format other than R8, unless you need very fine-grained resolution on the
        ///     Y-Axis.
        /// </remarks>
        public enum BakeFormat
        {
            RInt8, // 8 bits (integer)
            RInt16, // 16 bit (integer)
            RFloat16, // 16 bits (floating point)
            RFloat32 // 32 bits (floating point)
        }

        [Tooltip("The curve to bake. This curve must be in the range [0,1] for both the X and Y axis.")]
        public AnimationCurve Curve;

        /// <remarks>When sampled, the values are linearly interpolated between, so you don't need much resolution.</remarks>
        [Tooltip("The resolution of the final baked texture.")]
        public int Resolution = 32;

        [Tooltip("The texture format to bake. Generally you should just use RInt8, unless you need more resolution.")]
        public BakeFormat Format = BakeFormat.RInt8;

        /// <inheritdoc />
        public override void OnImportAsset(AssetImportContext ctx)
        {
            // Generate the texture as an R8. 
            // The texture doesn't use mips, and is linear, for simplicity.
            Texture2D texture = new Texture2D(Resolution, 1, ToTextureFormat(Format), false, true);
            texture.SetPixels(
                Enumerable.Range(0, Resolution)
                          .Select((_, i) => Curve.Evaluate((float) i / Resolution))
                          .Select(f => new Color(f, f, f))
                          .ToArray()
            );

            ctx.AddObjectToAsset("CurveTexture", texture);
            ctx.SetMainObject(texture);
        }

        private static TextureFormat ToTextureFormat(BakeFormat format)
        {
            switch (format)
            {
                case BakeFormat.RInt8:
                    return TextureFormat.R8;
                case BakeFormat.RInt16:
                    return TextureFormat.R16;
                case BakeFormat.RFloat16:
                    return TextureFormat.RHalf;
                case BakeFormat.RFloat32:
                    return TextureFormat.RFloat;
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }
    }
}
