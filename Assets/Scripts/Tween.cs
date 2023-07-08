using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween : MonoBehaviour
{
    private static Tween _ins;

    public static void MoveTo(RectTransform rectTransform, Vector2 from, Vector2 to, float duration, System.Action onComplete = null)
    {
        _ins._tweenDatas.Add(new RectTransformTweenData
        {
            Transform = rectTransform,
            RectTransform = rectTransform,
            From = from,
            To = to,
            Duration = duration,
            OnComplete = onComplete,
            UseCurve = false
        });
    }
    public static void MoveTo(RectTransform rectTransform, Vector2 from, Vector2 to, float duration, AnimationCurve curve, System.Action onComplete = null)
    {
        _ins._tweenDatas.Add(new RectTransformTweenData
        {
            Transform = rectTransform,
            RectTransform = rectTransform,
            From = from,
            To = to,
            Duration = duration,
            OnComplete = onComplete,
            UseCurve = true,
            Curve=curve,
        });
    }


    private List<ITweenData> _tweenDatas;


    void Awake()
    {
        _ins = this;
        _tweenDatas = new List<ITweenData>();
    }

    void Update()
    {
        int length = _tweenDatas.Count;
        for (int i = 0; i < length; i++)
        {
            _tweenDatas[i].Update(Time.deltaTime);
            if (_tweenDatas[i].IsFinished)
            {
                _tweenDatas.RemoveAt(i);
                i--;
                length--;
            }
        }
    }

    

    public interface ITweenData
    {
        bool IsFinished { get; }

        // event System.Action OnComplete;

        void Update(float t);
    }
    
    public struct RectTransformTweenData : ITweenData
    {
        public Transform Transform;
        public RectTransform RectTransform;
        public Vector2 From;
        public Vector2 To;
        public float Duration;
        public float Time;

        public System.Action OnComplete;

        public bool UseCurve;
        public AnimationCurve Curve;

        public bool IsFinished { get; private set; }

        public void Update(float t)
        {
            Time += t;

            if (UseCurve)
                RectTransform.anchoredPosition = Vector2.Lerp(From, To, Curve.Evaluate(Time / Duration));
            else
                RectTransform.anchoredPosition = Vector2.Lerp(From, To, Time / Duration);

            IsFinished = Time >= Duration;

            if (IsFinished)
                OnComplete?.Invoke();
        }
    }
}
