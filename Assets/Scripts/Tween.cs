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

    public static void FloatTween(float from, float to, float duration, System.Action<float> onUpdate, System.Action onComplete = null)
    {
        _ins._tweenDatas.Add(new FloatTweenData
        {
            From = from,
            To = to,
            Duration = duration,
            OnUpdate = onUpdate,
            OnComplete = onComplete,
            UseCurve = false
        });
    }

    public static void FloatTween(float from, float to, float duration, System.Action<float> onUpdate, AnimationCurve curve, System.Action onComplete = null)
    {
        _ins._tweenDatas.Add(new FloatTweenData
        {
            From = from,
            To = to,
            Duration = duration,
            OnUpdate = onUpdate,
            OnComplete = onComplete,
            UseCurve = true,
            Curve = curve,
        });
    }

    public static void ColorTween(Color from, Color to, float duration, System.Action<Color> onUpdate, System.Action onComplete = null)
    {
        _ins._tweenDatas.Add(new ColorTweenData
        {
            From = from,
            To = to,
            Duration = duration,
            OnUpdate = onUpdate,
            OnComplete = onComplete,
            UseCurve = false
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

    public struct FloatTweenData : ITweenData
    {
        public float From;
        public float To;
        public float Duration;
        public float Time;

        public System.Action<float> OnUpdate;
        public System.Action OnComplete;

        public bool UseCurve;
        public AnimationCurve Curve;

        public bool IsFinished { get; private set; }

        public void Update(float t)
        {
            Time += t;

            if (UseCurve)
                OnUpdate?.Invoke(Mathf.Lerp(From, To, Curve.Evaluate(Time / Duration)));
            else
                OnUpdate?.Invoke(Mathf.Lerp(From, To, Time / Duration));

            IsFinished = Time >= Duration;

            if (IsFinished)
                OnComplete?.Invoke();
        }
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

    public struct ColorTweenData : ITweenData
    {
        public Color From;
        public Color To;
        public float Duration;
        public float Time;

        public System.Action<Color> OnUpdate;
        public System.Action OnComplete;

        public bool UseCurve;
        public AnimationCurve Curve;

        public bool IsFinished { get; private set; }

        public void Update(float t)
        {
            Time += t;

            if (UseCurve)
                OnUpdate?.Invoke(Color.Lerp(From, To, Curve.Evaluate(Time / Duration)));
            else
                OnUpdate?.Invoke(Color.Lerp(From, To, Time / Duration));

            IsFinished = Time >= Duration;

            if (IsFinished)
                OnComplete?.Invoke();
        }
    }
}
