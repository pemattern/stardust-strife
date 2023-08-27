using UnityEngine;
using System;
using System.Threading.Tasks;

public static class CameraShake
{
    public static async void Shake(this Camera camera, float duration, float speed, float magnitude, Func<float, float> easingFunction)
    {
        float startTime = Time.time;
        float endTime = Time.time + duration;
        float t = 0f;

        while (Time.time < endTime)
        {
            t = easingFunction(ProgressNormalized(startTime, endTime, Time.time));
            Vector3 direction = UnityEngine.Random.onUnitSphere;
            await MoveTo(camera.transform, direction, speed, magnitude, t);
        }
        t = easingFunction(ProgressNormalized(startTime, endTime, Time.time));
        await MoveTo(camera.transform, Vector3.zero, speed, magnitude, t);
    }

    private static async Task MoveTo(Transform transform, Vector3 targetPosition, float speed, float magnitude, float t)
    {
        
        float tShake = 0f;
        Vector3 startingPosition = transform.localPosition;
        targetPosition *= magnitude * t;
        speed *= t;
        
        while (tShake < 1f)
        {
            tShake = ProgressNormalized(startingPosition, targetPosition, transform.localPosition);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, speed * Time.deltaTime);
            await Task.Yield();
        }
    }

    private static float ProgressNormalized(float starting, float target, float current)
    {
        float fullRange = Mathf.Abs(target - starting);
        float progress = Mathf.Abs(target - current);

        if (progress >= fullRange) return 1f;

        return progress / fullRange;
    }

    private static float ProgressNormalized(Vector3 starting, Vector3 target, Vector3 current)
    {
        float fullRange = (target - starting).magnitude;
        float progress = (target - current).magnitude;

        if (progress >= fullRange) return 1f;

        return progress / fullRange;
    }
}