using Spotlight.Core;
using Spotlight.Core.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;

namespace Rage
{
    internal static unsafe class Render
    {
        public static void SpotLight(Vector3 position, Vector3 direction, eLightFlags flags, float intensity, float range, float volumeIntensity, float volumeSize, float falloff, float innerAngle, float outerAngle, Color color, ulong shadowId = 0)
        {
            CLightDrawData* drawData = CLightDrawData.New(eLightType.SPOT_LIGHT, flags, position, color, intensity);
            NativeVector3 dir = direction;
            NativeVector3 perp = Utility.GetPerpendicular(dir).ToNormalized();
            drawData->Range = range;
            if (volumeIntensity > 0)
            {
                drawData->VolumeIntensity = volumeIntensity;
                drawData->VolumeExponent = 70.0f;
                drawData->VolumeSize = volumeSize;
            }
            drawData->FalloffExponent = falloff;
            GameFunctions.SetLightDrawDataDirection(drawData, &dir, &perp);
            GameFunctions.SetLightDrawDataAngles(drawData, innerAngle, outerAngle);
            if (shadowId != 0)
            {
                drawData->ShadowRenderId = shadowId;
                drawData->ShadowUnkValue = GameFunctions.GetValueForLightDrawDataShadowUnkValue(drawData);
            }
        }

        public static void RangeLight(Vector3 position, eLightFlags flags, float intensity, float range, float volumeIntensity, float volumeSize, float falloff, Color color, ulong shadowId = 0)
        {
            CLightDrawData* drawData = CLightDrawData.New(eLightType.RANGE_2, flags, position, color, intensity);
            if (volumeIntensity > 0)
            {
                drawData->VolumeIntensity = volumeIntensity;
                drawData->VolumeExponent = 70.0f;
                drawData->VolumeSize = volumeSize;
            }
            drawData->FalloffExponent = falloff;
            if (shadowId != 0)
            {
                drawData->ShadowRenderId = shadowId;
                drawData->ShadowUnkValue = GameFunctions.GetValueForLightDrawDataShadowUnkValue(drawData);
            }
        }

        public static void Corona(Vector3 position, Vector3 direction, float size, float intensity, float innerAngle, float outerAngle, Color color)
        {
            int colorRaw = (color.A << 24 | color.R << 16 | color.G << 8 | color.B << 0) & -1;
            GameMemory.Coronas->Draw(position, size, colorRaw, intensity, 100.0f, direction, innerAngle, outerAngle, 3);
        }

        public static ulong GenerateShadowId()
        {
            ulong a = unchecked((ulong)AppDomain.CurrentDomain.Id) << 32;
            ulong b = unchecked((ulong)MathHelper.GetRandomInteger(10000000, 99999999));
            return a | b;
        }
    }
}
