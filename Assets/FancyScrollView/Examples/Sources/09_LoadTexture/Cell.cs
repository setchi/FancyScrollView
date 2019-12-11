﻿/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2019 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using EasingCore;

namespace FancyScrollView.Example09
{
    public class Cell : FancyScrollViewCell<ItemData>
    {
        readonly Func<float, float> alphaEasing = Easing.Get(Ease.OutQuint);

        [SerializeField] Text title = default;
        [SerializeField] Text description = default;
        [SerializeField] RawImage image = default;
        [SerializeField] Image background = default;
        [SerializeField] CanvasGroup canvasGroup = default;

        public override void UpdateContent(ItemData itemData)
        {
            image.texture = null;

            TextureLoader.Load(itemData.Url, result =>
            {
                if (image == null || result.Url != itemData.Url)
                {
                    return;
                }

                image.texture = result.Texture;
            });

            title.text = itemData.Title;
            description.text = itemData.Description;

            UpdateSibling();
        }

        void UpdateSibling()
        {
            var isLast = Index <= transform.parent.Cast<Transform>()
                .Min(x => x.GetComponent<Cell>().Index);
            if (isLast)
            {
                transform.SetAsLastSibling();
            }

            var isFirst = Index >= transform.parent.Cast<Transform>()
                .Max(x => x.GetComponent<Cell>().Index);
            if (isFirst)
            {
                transform.SetAsFirstSibling();
            }
        }

        public override void UpdatePosition(float t)
        {
            const float PopAngle = -15;
            const float SlideAngle = 25;

            t = 1f - t;

            var key = 1f / 4f;
            var popTime = key * 3f;
            var pop = Mathf.Min(popTime, t) / popTime;
            var slide = (Mathf.Max(popTime, t) - popTime) / key;

            transform.localRotation = t < popTime
                ? Quaternion.Euler(0, 0, Mathf.Lerp(PopAngle, 0, pop))
                : Quaternion.Euler(0, 0, Mathf.Lerp(0, SlideAngle, slide));

            transform.localPosition = Vector3.Lerp(Vector3.zero, Vector3.left * 500, slide);

            canvasGroup.alpha = alphaEasing(1f - slide);

            background.color = Color.Lerp(Color.gray, Color.white, pop);
        }
    }
}
