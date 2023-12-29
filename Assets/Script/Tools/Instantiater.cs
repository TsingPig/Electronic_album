using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public static class Instantiater
{
    private static Dictionary<string, List<GameObject>> _objectPools = new Dictionary<string, List<GameObject>>();

    public static void Release()
    {
        foreach(var objectList in _objectPools.Values)
        {
            foreach(var obj in objectList)
            {
                Addressables.ReleaseInstance(obj);
            }
        }
        _objectPools.Clear();
    }

    public static async Task<GameObject> InstantiateAsync(string addressablePath, Transform parent)
    {
        List<GameObject> objectPool;

        if(_objectPools.TryGetValue(addressablePath, out objectPool) && objectPool.Count > 0)
        {
            GameObject obj = objectPool[0];
            objectPool.RemoveAt(0);
            obj.SetActive(true);
            return obj;
        }
        else
        {
            AsyncOperationHandle<IList<IResourceLocation>> handlers = Addressables.LoadResourceLocationsAsync(addressablePath);
            await handlers.Task;

            IList<IResourceLocation> results = handlers.Result;

            if(results.Count > 0)
            {
                AsyncOperationHandle<GameObject> handler = Addressables.InstantiateAsync(results[0], parent);
                await handler.Task;
                GameObject instantiatedObject = handler.Result;
                if(instantiatedObject != null)
                {
                    if(!_objectPools.ContainsKey(addressablePath))
                    {
                        _objectPools[addressablePath] = new List<GameObject>();
                    }
                    _objectPools[addressablePath].Add(instantiatedObject);
                    return instantiatedObject;
                }
                else
                {
                    Debug.LogError($"{addressablePath}GameObject加载错误");
                    return null;
                }
            }
            else
            {
                Debug.LogError($"{addressablePath}找不到资源位置");
                return null;
            }
        }
    }

    public static void DeactivateObject(GameObject obj)
    {
        // 将对象设置为非激活状态，并放回对象池
        obj.SetActive(false);

        // 将对象放回对象池
        foreach(var objectList in _objectPools.Values)
        {
            if(objectList.Contains(obj))
            {
                return;
            }
        }

        Debug.LogWarning("Trying to deactivate an object not managed by the Instantiater.");
    }
}

//using System.Collections.Generic;
//using System.Threading.Tasks;
//using UnityEngine.AddressableAssets;
//using UnityEngine.ResourceManagement.AsyncOperations;
//using UnityEngine.ResourceManagement.ResourceLocations;
//using UnityEngine;

//public static class Instantiater
//{
//    public static AsyncOperationHandle<IList<IResourceLocation>> handlers;

//    public static Dictionary<string, GameObject> _dicObjCache = new Dictionary<string, GameObject> { };

//    public static void Release()
//    {
//        Addressables.Release(handlers);
//    }

//    /// <summary>
//    /// 异步实例化GameObject
//    /// </summary>
//    /// <param name="addressablePath">物体的Addressable路径</param>
//    /// <param name="parent"></param>
//    /// <returns></returns>
//    public static async Task<GameObject> InstantiateAsync(string addressablePath, Transform parent)
//    {
//        GameObject gameObjectToInstantiate;
//        //if(_dicObjCache.ContainsKey(addressablePath))
//        //{
//        //    gameObjectToInstantiate = _dicObjCache[addressablePath];
//        //}
//        //else
//        //{
//        handlers = Addressables.LoadResourceLocationsAsync(addressablePath);
//        await handlers.Task;
//        IList<IResourceLocation> results = handlers.Result;
//        gameObjectToInstantiate = results[0].Data as GameObject;

//        Debug.Log(gameObjectToInstantiate);
//        //}
//        if(gameObjectToInstantiate == null)
//        {
//            Debug.LogError($"{addressablePath}加载错误");
//            return null;
//        }
//        await Addressables.InstantiateAsync(gameObjectToInstantiate, parent).Task;
//        return gameObjectToInstantiate;
//    }
//}

//using System.Collections.Generic;
//using System.Threading.Tasks;
//using UnityEngine;
//using UnityEngine.AddressableAssets;
//using UnityEngine.ResourceManagement.AsyncOperations;
//using UnityEngine.ResourceManagement.ResourceLocations;

//public static class Instantiater
//{
//    public static async Task<GameObject> InstantiateAsync(string path, Transform parent)
//    {
//        AsyncOperationHandle<IList<IResourceLocation>> handlers = Addressables.LoadResourceLocationsAsync(path);
//        await handlers.Task;
//        IList<IResourceLocation> results = handlers.Result;
//        GameObject obj = results[0].Data as GameObject;
//        await Addressables.InstantiateAsync(obj, parent).Task;
//        return obj;
//    }

//    public static async Task<GameObject> InstantiateAsync(GameObject obj, Transform parent)
//    {
//        await Addressables.InstantiateAsync(obj, parent).Task;
//        return obj;
//    }
//}