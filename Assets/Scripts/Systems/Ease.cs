using System;

public static class Ease
{
    public static Func<float, float> Linear = t => Clamp01(t);
    public static Func<float, float> QuadIn = t => { t = Clamp01(t); return t * t; };
    public static Func<float, float> QuadOut = t => { t = Clamp01(t); return 1 - (t - 1) * (t - 1); };
    public static Func<float, float> CubeIn = t => { t = Clamp01(t); return t * t * t; };
    public static Func<float, float> CubeOut = t => { t = Clamp01(t); return 1 - (t - 1) * (t - 1) * (t - 1); };
    public static Func<float, float> CircleIn = t => { t = Clamp01(t); return -MathF.Sqrt(1 - t * t) + 1; };
    public static Func<float, float> CircleOut = t => { t = Clamp01(t); return MathF.Sqrt(1 - (t - 1) * (t - 1)); };
    public static Func<float, float> OvershootIn = t => { t = Clamp01(t); return 2 * t * t; };
    public static Func<float, float> OvershootOut = t => { t = Clamp01(t); return 1 - 2 * (t - 1) * (t - 1); };
    public static Func<float, float> Smooth = t => { t = Clamp01(t); return t * t * (3 - 2 * t); };
    public static Func<float, float> InverseParabola = t => { t = Clamp01(t); return -4 * (t - 0.5f) * (t - 0.5f) + 1; };
    public static Func<float, float> Triangle010 = t => { t = Clamp01(t); return -MathF.Abs((4 * t) % 4 - 2) * 0.5f + 1; };

    public static Func<float, float> CircleInOutLinear = t => Mix(CircleIn, CircleOut, Linear, t);
    public static Func<float, float> CircleOutInLinear = t => Mix(CircleOut, CircleIn, Linear, t);
    public static Func<float, float> CircleInOutSmooth = t => Mix(CircleIn, CircleOut, Smooth, t);
    public static Func<float, float> CircleOutInSmooth = t => Mix(CircleOut, CircleIn, Smooth, t);

    public static Func<float, float> OvershotInOutLinear = t => Mix(OvershootIn, OvershootOut, Linear, t);
    public static Func<float, float> OvershotInOutSmooth = t => Mix(OvershootIn, OvershootOut, Smooth, t);

    public static float Mix(Func<float, float> easeInFunction, Func<float, float> easeOutFunction, Func<float, float> interpolationFunction, float t)
    {
        t = interpolationFunction(t);
        return easeInFunction(t) * (1 - t) + easeOutFunction(t) * t;
    }

    private static float Clamp01(float t)
    {
        if (t < 0f) return 0f;
        if (t > 1f) return 1f;
        return t;
    }
}