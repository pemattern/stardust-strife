#ifndef LIGHTING_CEL_SHADED_INCLUDED
#define LIGHTING_CEL_SHADED_INCLUDED

#ifndef SHADERGRAPH_PREVIEW
struct EdgeConstants
{
    float edgeDiffuse;
    float edgeSpecular;
    float edgeSpecularOffset;
    float edgeDistanceAttenuation;
    float edgeDistanceAttenuationMin; // New
    float edgeShadowAttenuation;
    float edgeShadowAttenuationMin; // New
    float edgeRim;
    float edgeRimOffset;
};

struct SurfaceVariables
{
    float3 normal;
    float3 view;
    float smoothness;
    float shininess;
    float rimThreshold;
    EdgeConstants ec;
};

float3 CalculateCelShading(Light l, SurfaceVariables s)
{
    float shadowAttenuationSmoothstepped = smoothstep(s.ec.edgeShadowAttenuationMin, s.ec.edgeShadowAttenuation, l.shadowAttenuation);
    float distanceAttenuationSmoothstepped = smoothstep(s.ec.edgeDistanceAttenuationMin, s.ec.edgeDistanceAttenuation, l.distanceAttenuation);

    float attenuation = shadowAttenuationSmoothstepped * distanceAttenuationSmoothstepped;

    float diffuse = saturate(dot(s.normal, l.direction));
    diffuse *= attenuation;

    float3 h = SafeNormalize(l.direction + s. view);
    float specular = saturate(dot(s.normal, h));
    specular = pow(specular, s.shininess);
    specular *= diffuse * s.smoothness;

    float rim = 1 - dot(s.view, s.normal);
    rim *= pow(diffuse, s.rimThreshold);

    diffuse = smoothstep(0.0f, s.ec.edgeDiffuse, diffuse);
    specular = s.smoothness * smoothstep((1- s.smoothness) * s.ec.edgeSpecular + s.ec.edgeSpecularOffset, s.ec.edgeSpecular + s.ec.edgeSpecularOffset, specular);
    rim = s.smoothness * smoothstep(s.ec.edgeRim - 0.5f * s.ec.edgeRimOffset, s.ec.edgeRim + 0.5f * s.ec.edgeRimOffset, rim);

    return l.color * (diffuse + max(specular, rim));
}
#endif

void LightingCelShader_float(float3 Normal, float3 View, float3 Position, float Smoothness, float RimThreshold, float EdgeDiffuse, float EdgeSpecular, float EdgeSpecularOffset, float EdgeDistanceAttenuation, float EdgeShadowAttenuation, float EdgeRim, float EdgeRimOffset, float EdgeDistanceAttenuationMin, float EdgeShadowAttenuationMin, out float3 Color)
{
#if defined(SHADERGRAPH_PREVIEW)
    Color = float3(1f, 1f, 1f);
#else
    EdgeConstants ec;
    ec.edgeDiffuse = EdgeDiffuse;
    ec.edgeSpecular = EdgeSpecular;
    ec.edgeSpecularOffset = EdgeSpecularOffset;
    ec.edgeDistanceAttenuation = EdgeDistanceAttenuation;
    ec.edgeDistanceAttenuationMin = EdgeDistanceAttenuationMin;
    ec.edgeShadowAttenuation = EdgeShadowAttenuation;
    ec.edgeShadowAttenuationMin = EdgeShadowAttenuationMin;
    ec.edgeRim = EdgeRim;
    ec.edgeRimOffset = EdgeRimOffset;

    SurfaceVariables s;
    s.normal = normalize(Normal);
    s.view = SafeNormalize(View);
    s.smoothness = Smoothness;
    s.shininess = exp2(10 * Smoothness + 1);
    s.rimThreshold = RimThreshold;
    s.ec = ec;

#if SHADOWS_SCREEN
    float4 clipPos = TransformWorldToHClip(Position);
    float4 shadowCoord = ComputeScreenPos(clipPos);
#else
    float4 shadowCoord = TransformWorldToShadowCoord(Position);
#endif
    Light light = GetMainLight(shadowCoord);
    Color = CalculateCelShading(light, s);

    int pixelLightCount = GetAdditionalLightsCount();
    for (int i = 0; i < pixelLightCount; i++)
    {
        light = GetAdditionalLight(i, Position, 1);
        Color += CalculateCelShading(light, s);
    }

#endif
}
#endif