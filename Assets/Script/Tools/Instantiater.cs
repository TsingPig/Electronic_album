using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace TsingPigSDK
{
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
                foreach(var obj in objectPool)
                {
                    if(!obj.activeSelf)
                    {
                        obj.SetActive(true);
                        return obj;
                    }
                }
            }
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
                    Debug.LogError($"{addressablePath}GameObject���ش���");
                    return null;
                }
            }
            else
            {
                Debug.LogError($"{addressablePath}�Ҳ�����Դλ��");
                return null;
            }
        }

        public static void DeactivateObjectPool(string addressablePath)
        {
            if(_objectPools.ContainsKey(addressablePath))
            {
                foreach(var obj in _objectPools[addressablePath])
                {
                    DeactivateObject(obj);
                }
            }
            else
            {
                Debug.LogWarning($"����ɾ��{addressablePath}����Ч��");
            }
        }

        private static void DeactivateObject(GameObject obj)
        {
            obj.SetActive(false);
        }

        public static void DeactivateObjectByIndex(string addressablePath, int idx)
        {
            if(_objectPools.ContainsKey(addressablePath))
            {
                foreach(var obj in _objectPools[addressablePath])
                {
                    if(obj.activeSelf) 
                    {
                        if(idx == 0)
                        {
                            DeactivateObject(obj);
                            break;
                        }
                        idx--;
                    } 
                }
            }
            else
            {
                // Debug.LogWarning($"����ɾ��{addressablePath}����Ч��");
            }
        }
    }
}