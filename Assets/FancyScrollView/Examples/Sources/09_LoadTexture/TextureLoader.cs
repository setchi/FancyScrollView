/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2019 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FancyScrollView.Example09
{
    public static class TextureLoader
    {
        public static void Load(string url, Action<(string Url, Texture Texture)> onSuccess) =>
            InternalLoader.Instance.Load(url, onSuccess);

        class InternalLoader : MonoBehaviour
        {
            readonly Dictionary<string, Texture> cache = new Dictionary<string, Texture>();

            static InternalLoader instance;

            public static InternalLoader Instance => instance ??
                (instance = FindObjectOfType<InternalLoader>() ??
                    new GameObject(typeof(TextureLoader).Name).AddComponent<InternalLoader>());

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
