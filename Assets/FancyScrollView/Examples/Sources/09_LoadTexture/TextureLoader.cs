/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FancyScrollView.Example09
{
    static class TextureLoader
    {
        public static void Load(string url, Action<(string Url, Texture Texture)> onSuccess) =>
            Loader.Instance.Load(url, onSuccess);

        class Loader : MonoBehaviour
        {
            readonly Dictionary<string, Texture> cache = new Dictionary<string, Texture>();

            static Loader instance;

            public static Loader Instance => instance ??
                (instance = FindObjectOfType<Loader>() ??
                    new GameObject(typeof(TextureLoader).Name).AddComponent<Loader>());

            public void Load(string url, Action<(string Url, Texture Texture)> onSuccess)
            {
                if (cache.TryGetValue(url, out var cachedTexture))
                {
                    onSuccess((url, cachedTexture));
                    return;
                }

                StartCoroutine(DownloadTexture(url, result =>
                {
                    cache[result.Url] = result.Texture;
                    onSuccess(result);
                }));
            }

            IEnumerator DownloadTexture(string url, Action<(string Url, Texture Texture)> onSuccess)
            {
                using (var request = UnityWebRequestTexture.GetTexture(url))
                {
                    yield return request.SendWebRequest();

                    if (request.isNetworkError)
                    {
                        Debug.LogErrorFormat("Error: {0}", request.error);
                        yield break;
                    }

                    onSuccess((
                        url,
                        ((DownloadHandlerTexture) request.downloadHandler).texture
                    ));
                }
            }

            void OnDestroy()
            {
                foreach (var kv in cache)
                {
                    Destroy(kv.Value);
                }

                instance = null;
            }
        }
    }
}
