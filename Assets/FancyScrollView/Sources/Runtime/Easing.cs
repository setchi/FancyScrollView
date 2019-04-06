using System;
using UnityEngine;

namespace FancyScrollView
{
    public enum Easing
    {
        Linear,
        InBack,
        InBounce,
        InCirc,
        InCubic,
        InElastic,
        InExpo,
        InQuad,
        InQuart,
        InQuint,
        InSine,
        OutBack,
        OutBounce,
        OutCirc,
        OutCubic,
        OutElastic,
        OutExpo,
        OutQuad,
        OutQuart,
        OutQuint,
        OutSine,
        InOutBack,
        InOutBounce,
        InOutCirc,
        InOutCubic,
        InOutElastic,
        InOutExpo,
        InOutQuad,
        InOutQuart,
        InOutQuint,
        InOutSine,
    }

    public static class EasingFunction
    {
        public static Func<float, float> Get(Easing ease)
        {
            switch (ease)
            {
                case Easing.Linear: return EasingFunction.Linear;
                case Easing.InBack: return EasingFunction.InBack;
                case Easing.InBounce: return EasingFunction.InBounce;
                case Easing.InCirc: return EasingFunction.InCirc;
                case Easing.InCubic: return EasingFunction.InCubic;
                case Easing.InElastic: return EasingFunction.InElastic;
                case Easing.InExpo: return EasingFunction.InExpo;
                case Easing.InQuad: return EasingFunction.InQuad;
                case Easing.InQuart: return EasingFunction.InQuart;
                case Easing.InQuint: return EasingFunction.InQuint;
                case Easing.InSine: return EasingFunction.InSine;
                case Easing.OutBack: return EasingFunction.OutBack;
                case Easing.OutBounce: return EasingFunction.OutBounce;
                case Easing.OutCirc: return EasingFunction.OutCirc;
                case Easing.OutCubic: return EasingFunction.OutCubic;
                case Easing.OutElastic: return EasingFunction.OutElastic;
                case Easing.OutExpo: return EasingFunction.OutExpo;
                case Easing.OutQuad: return EasingFunction.OutQuad;
                case Easing.OutQuart: return EasingFunction.OutQuart;
                case Easing.OutQuint: return EasingFunction.OutQuint;
                case Easing.OutSine: return EasingFunction.OutSine;
                case Easing.InOutBack: return EasingFunction.InOutBack;
                case Easing.InOutBounce: return EasingFunction.InOutBounce;
                case Easing.InOutCirc: return EasingFunction.InOutCirc;
                case Easing.InOutCubic: return EasingFunction.InOutCubic;
                case Easing.InOutElastic: return EasingFunction.InOutElastic;
                case Easing.InOutExpo: return EasingFunction.InOutExpo;
                case Easing.InOutQuad: return EasingFunction.InOutQuad;
                case Easing.InOutQuart: return EasingFunction.InOutQuart;
                case Easing.InOutQuint: return EasingFunction.InOutQuint;
                case Easing.InOutSine: return EasingFunction.InOutSine;
                default: return EasingFunction.Linear;
            }
        }

        static float Linear(float t)
        {
            return t;
        }

        static float InBack(float t)
        {
            return t * t * t - t * Mathf.Sin(t * Mathf.PI);
        }

        static float OutBack(float t)
        {
            return 1f - InBack(1f - t);
        }

        static float InOutBack(float t)
        {
            return t < 0.5f
                ? 0.5f * InBack(t * 2f)
                : 0.5f * OutBack(t * 2f);
        }

        static float InBounce(float t)
        {
            return 1f - OutBounce(1f - t);
        }

        static float OutBounce(float t)
        {
            if (t < 4f / 11.0f)
            {
                return (121f * t * t) / 16.0f;
            }

            if (t < 8f / 11.0f)
            {
                return (363f / 40.0f * t * t) - (99f / 10.0f * t) + 17f / 5.0f;
            }

            if (t < 9f / 10.0f)
            {
                return (4356f / 361.0f * t * t) - (35442f / 1805.0f * t) + 16061f / 1805.0f;
            }

            return (54f / 5.0f * t * t) - (513f / 25.0f * t) + 268f / 25.0f;
        }

        static float InOutBounce(float t)
        {
            return t < 0.5f
                ? 0.5f * InBounce(t * 2f)
                : 0.5f * OutBounce(t * 2f - 1f) + 0.5f;
        }

        static float InCirc(float t)
        {
            return 1f - Mathf.Sqrt(1f - (t * t));
        }

        static float OutCirc(float t)
        {
            return Mathf.Sqrt((2f - t) * t);
        }

        static float InOutCirc(float t)
        {
            return t < 0.5f
                ? 0.5f * (1 - Mathf.Sqrt(1f - 4f * (t * t)))
                : 0.5f * (Mathf.Sqrt(-((2f * t) - 3f) * ((2f * t) - 1f)) + 1f);
        }

        static float InCubic(float t)
        {
            return t * t * t;
        }

        static float OutCubic(float t)
        {
            return InCubic(t - 1f) + 1f;
        }

        static float InOutCubic(float t)
        {
            return t < 0.5f
                ? 4f * t * t * t
                : 0.5f * InCubic(t * 2f - 2f) + 1f;
        }

        static float InElastic(float t)
        {
            return Mathf.Sin(13f * (Mathf.PI * 0.5f) * t) * Mathf.Pow(2f, 10f * (t - 1f));
        }

        static float OutElastic(float t)
        {
            return Mathf.Sin(-13f * (Mathf.PI * 0.5f) * (t + 1)) * Mathf.Pow(2f, -10f * t) + 1f;
        }

        static float InOutElastic(float t)
        {
            return t < 0.5f
                ? 0.5f * Mathf.Sin(13f * (Mathf.PI * 0.5f) * (2f * t)) * Mathf.Pow(2f, 10f * ((2f * t) - 1f))
                : 0.5f * (Mathf.Sin(-13f * (Mathf.PI * 0.5f) * ((2f * t - 1f) + 1f)) * Mathf.Pow(2f, -10f * (2f * t - 1f)) + 2f);
        }

        static float InExpo(float t)
        {
            return Mathf.Approximately(0.0f, t) ? t : Mathf.Pow(2f, 10f * (t - 1f));
        }

        static float OutExpo(float t)
        {
            return Mathf.Approximately(1.0f, t) ? t : 1f - Mathf.Pow(2f, -10f * t);
        }

        static float InOutExpo(float v)
        {
            return Mathf.Approximately(0.0f, v) || Mathf.Approximately(1.0f, v)
                ? v
                : v < 0.5f
                    ?  0.5f * Mathf.Pow(2f, (20f * v) - 10f)
                    : -0.5f * Mathf.Pow(2f, (-20f * v) + 10f) + 1f;
        }

        static float InQuad(float t)
        {
            return t * t;
        }

        static float OutQuad(float t)
        {
            return -(t * (t - 2f));
        }

        static float InOutQuad(float t)
        {
            return t < 0.5f
                ?  2f * t * t
                : -2f * t * t + 4f * t - 1f;
        }

        static float InQuart(float t)
        {
            return t * t * t * t;
        }

        static float OutQuart(float t)
        {
            var f = t - 1f;
            return f * f * f * (1f - t) + 1f;
        }

        static float InOutQuart(float t)
        {
            if (t < 0.5f)
            {
                return 8f * t * t * t * t;
            }

			float f = (t - 1);
			return -8 * f * f * f * f + 1;
        }

        static float InQuint(float t)
        {
            return t * t * t * t * t;
        }

        static float OutQuint(float t)
        {
            var f = t - 1f;
            return f * f * f * f * f + 1f;
        }

        static float InOutQuint(float t)
        {
            if (t < 0.5f)
            {
                return 16f * t * t * t * t * t;
            }

            var f = ((2f * t) - 2f);
            return 0.5f * f * f * f * f * f + 1f;
        }

        static float InSine(float t)
        {
            return Mathf.Sin((t - 1f) * (Mathf.PI * 0.5f)) + 1f;
        }

        static float OutSine(float t)
        {
            return Mathf.Sin(t * (Mathf.PI * 0.5f));
        }

        static float InOutSine(float t)
        {
            return 0.5f * (1f - Mathf.Cos(t * Mathf.PI));
        }
    }
}
