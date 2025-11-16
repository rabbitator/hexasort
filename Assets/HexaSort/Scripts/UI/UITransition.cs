using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using HexaSort.Configuration;
using HexaSort.Tweening;
using HexaSort.UI.Views;
using UnityEngine;

namespace HexaSort.UI
{
    using CurveType = AnimationCurvesConfiguration.CurveType;

    public class UITransition
    {
        private struct TransformData
        {
            public Vector3 AnchoredPosition;
            public Vector3 LocalScale;
            public Quaternion LocalRotation;
        }

        private class ActData
        {
            public float Delay;
            public float Duration;
            public CurveType Curve;
            public Action<float> Act;
        }

        private readonly Tweener _tweener;
        private readonly Vector3 _deltaPos = new(0.0f, 600.0f, 0.0f);
        private readonly Vector3 _deltaRot = new(0.0f, 0.0f, 45.0f);

        private LobbyView _lobbyView;
        private GameplayView _gameplayView;
        private SettingsView _settingsView;

        private TransformData _logoNormalTrs;
        private TransformData _buttonPlayNormalTrs;
        private TransformData _buttonMenuNormalTrs;
        private TransformData _buttonSettingsNormalTrs;

        private TransformData _logoStartTrs;
        private TransformData _buttonPlayStartTrs;
        private TransformData _buttonMenuStartTrs;
        private TransformData _buttonSettingsStartTrs;

        private float _duration = 0.4f;
        private CancellationTokenSource _cts = new();

        public UITransition(Tweener tweener)
        {
            _tweener = tweener;
        }

        public async UniTask InitializeAsync(LobbyView lobbyView, GameplayView gameplayView, SettingsView settingsView)
        {
            _lobbyView = lobbyView;
            _gameplayView = gameplayView;
            _settingsView = settingsView;

            GetTransformData(_lobbyView.LogoRoot, out _logoNormalTrs);
            GetTransformData(_lobbyView.PlayButtonRoot, out _buttonPlayNormalTrs);
            GetTransformData(_gameplayView.MenuButtonRoot, out _buttonMenuNormalTrs);
            GetTransformData(_gameplayView.SettingsButtonRoot, out _buttonSettingsNormalTrs);
        }

        public void ShowLobby(bool immediately = false)
        {
            var duration = immediately ? 0.0f : _duration;

            var acts = new List<ActData>
            {
                new()
                {
                    Delay = 0.0f,
                    Duration = duration,
                    Curve = CurveType.SimpleBounce,
                    Act = ApplyLobbyPositions
                },

                new()
                {
                    Delay = 0.0f,
                    Duration = duration,
                    Curve = CurveType.ReturningBounce,
                    Act = ApplyRotations
                }
            };

            MakeTransition(acts);
        }

        public void ShowGameplay(bool immediately = false)
        {
            var duration = immediately ? 0.0f : _duration;

            var acts = new List<ActData>
            {
                new()
                {
                    Delay = 0.0f,
                    Duration = duration,
                    Curve = CurveType.SimpleBounce,
                    Act = ApplyGameplayPositions
                },

                new()
                {
                    Delay = 0.0f,
                    Duration = duration,
                    Curve = CurveType.ReturningBounce,
                    Act = ApplyRotations
                }
            };

            MakeTransition(acts);
        }

        public void ShakePlayButton()
        {
            var acts = new List<ActData>
            {
                new()
                {
                    Delay = 0.0f,
                    Duration = _duration,
                    Curve = CurveType.Shake,
                    Act = ApplyPlayButtonShake
                }
            };

            MakeTransition(acts);
        }

        private void MakeTransition(List<ActData> acts)
        {
            if (!GotViews()) return;

            _cts.Cancel();
            _cts = new CancellationTokenSource();

            UpdateStartTransforms();
            acts.ForEach(data => _tweener.Tween(data.Delay, data.Duration, data.Act, data.Curve, _cts.Token).Forget());
        }

        private void UpdateStartTransforms()
        {
            GetTransformData(_lobbyView.LogoRoot, out _logoStartTrs);
            GetTransformData(_lobbyView.PlayButtonRoot, out _buttonPlayStartTrs);
            GetTransformData(_gameplayView.MenuButtonRoot, out _buttonMenuStartTrs);
            GetTransformData(_gameplayView.SettingsButtonRoot, out _buttonSettingsStartTrs);
        }

        private void ApplyLobbyPositions(float t)
        {
            SetAnchoredPosition(
                _lobbyView.LogoRoot,
                _logoStartTrs.AnchoredPosition,
                _logoNormalTrs.AnchoredPosition,
                t);

            SetAnchoredPosition(
                _lobbyView.PlayButtonRoot,
                _buttonPlayStartTrs.AnchoredPosition,
                _buttonPlayNormalTrs.AnchoredPosition,
                t);

            SetAnchoredPosition(
                _gameplayView.MenuButtonRoot,
                _buttonMenuStartTrs.AnchoredPosition,
                _buttonMenuNormalTrs.AnchoredPosition + _deltaPos,
                t);

            SetAnchoredPosition(
                _gameplayView.SettingsButtonRoot,
                _buttonSettingsStartTrs.AnchoredPosition,
                _buttonSettingsNormalTrs.AnchoredPosition + _deltaPos,
                t);
        }

        private void ApplyGameplayPositions(float t)
        {
            // Positions
            SetAnchoredPosition(
                _lobbyView.LogoRoot,
                _logoStartTrs.AnchoredPosition,
                _logoNormalTrs.AnchoredPosition + _deltaPos,
                t);

            SetAnchoredPosition(
                _lobbyView.PlayButtonRoot,
                _buttonPlayStartTrs.AnchoredPosition,
                _buttonPlayNormalTrs.AnchoredPosition - _deltaPos,
                t);

            SetAnchoredPosition(
                _gameplayView.MenuButtonRoot,
                _buttonMenuStartTrs.AnchoredPosition,
                _buttonMenuNormalTrs.AnchoredPosition,
                t);

            SetAnchoredPosition(
                _gameplayView.SettingsButtonRoot,
                _buttonSettingsStartTrs.AnchoredPosition,
                _buttonSettingsNormalTrs.AnchoredPosition,
                t);
        }

        private void ApplyRotations(float t)
        {
            // Rotations
            SetAnchoredRotation(
                _gameplayView.MenuButtonRoot,
                _buttonMenuStartTrs.LocalRotation,
                _buttonMenuStartTrs.LocalRotation * Quaternion.Euler(_deltaRot),
                t);

            SetAnchoredRotation(
                _gameplayView.SettingsButtonRoot,
                _buttonSettingsStartTrs.LocalRotation,
                _buttonSettingsStartTrs.LocalRotation * Quaternion.Euler(_deltaRot),
                t);
        }

        private void ApplyPlayButtonShake(float t)
        {
            var delta = -new Vector3(50.0f, 0.0f, 0.0f);
            SetAnchoredPosition(
                _lobbyView.PlayButtonRoot,
                _buttonPlayNormalTrs.AnchoredPosition,
                _buttonPlayNormalTrs.AnchoredPosition + delta,
                t);
        }

        private void SetAnchoredPosition(RectTransform rt, Vector3 start, Vector3 target, float t)
        {
            rt.anchoredPosition = Vector3.LerpUnclamped(start, target, t);
        }

        private void SetAnchoredRotation(RectTransform rt, Quaternion start, Quaternion target, float t)
        {
            rt.localRotation = Quaternion.LerpUnclamped(start, target, t);
        }

        private void GetTransformData(RectTransform target, out TransformData data)
        {
            data = new TransformData
            {
                AnchoredPosition = target.anchoredPosition,
                LocalScale = target.localScale,
                LocalRotation = target.localRotation
            };
        }

        private bool GotViews()
        {
            return _lobbyView && _gameplayView && _settingsView;
        }
    }
}