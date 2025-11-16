using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using HexaSort.Configuration;
using UnityEngine;

namespace HexaSort.Tweening
{
    using CurveType = AnimationCurvesConfiguration.CurveType;

    public class Tweener
    {
        private readonly GameConfiguration _config;
        private readonly AnimationCurve _defaultCurve;

        public Tweener(GameConfiguration config)
        {
            _config = config;
            _defaultCurve = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 1.0f));

            ValidateCurves();
        }

        public async UniTask Tween(
            float delay, float duration, Action<float> act,
            CurveType curveType, CancellationToken ct)
        {
            var curve = _config.AnimationCurves.FirstOrDefault(c => c.Type == curveType)?.Curve;

            if (curve == null)
            {
                Debug.LogError($"Curve of type {curveType} not defined! Trying to use the first one.");
                curve = _config.AnimationCurves.FirstOrDefault()?.Curve;
            }

            if (curve == null)
            {
                Debug.LogError("Sorry, but there is no curves in config defined. Why? Don't answer, default one will be used.");
                curve = _defaultCurve;
            }

            if (duration < float.Epsilon)
            {
                act(curve.Evaluate(1.0f));

                return;
            }

            await TweenJob(delay, duration, act, curve, ct);
        }

        private async UniTask TweenJob(float delay, float duration, Action<float> act, AnimationCurve curve, CancellationToken ct)
        {
            var cancelled = await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: ct).SuppressCancellationThrow();

            if (cancelled) return;

            var startTime = Time.time;

            while (!ct.IsCancellationRequested && Time.time < startTime + duration)
            {
                var t = (Time.time - startTime) / duration;
                t = curve.Evaluate(t);
                act(t);

                await UniTask.Yield();
            }

            if (!ct.IsCancellationRequested)
            {
                act(curve.Evaluate(1.0f));
            }
        }

        private void ValidateCurves()
        {
            foreach (var curveData in _config.AnimationCurves)
            {
                if (curveData.Type != CurveType.Shake && curveData.Type != CurveType.ReturningBounce) continue;
                if (curveData.Curve?.keys == null) continue;
                if (curveData.Curve.keys.Length < 1) continue;

                ReportCurve(curveData);
            }
        }

        private static void ReportCurve(AnimationCurvesConfiguration curveData)
        {
            if (curveData.Curve.Evaluate(1.0f) != 0.0f)
            {
                Debug.LogWarning($"Curve {curveData.Type} should have last key with time 1.0 and value 0.0!");
            }
        }
    }
}