using System.Collections;
using UnityEngine;

public static class TransformExtensions
{
    public static IEnumerator MoveToPosition(this Transform transform, Vector3 endPosition, float durationSeconds)
    {
        var startPosition = transform.position;
        var elapsedTime = 0f;

        while (elapsedTime < durationSeconds)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / durationSeconds);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
    }
}
