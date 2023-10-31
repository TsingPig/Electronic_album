using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVPFrameWork.ResourceModule
{
    public interface IResourceLoader
    {
        T LoadAsset<T>(string assetPath) where T : UnityEngine.Object;

        void LoadAssetAsync<T>(string assetPath, Action<T> callback, Action<float> onProgress = null) where T : UnityEngine.Object;

        void Unload(string assetPath);

        void UnloadAll();
    }
}