using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace GameStartStudio
{
    // 1.AssestBundlemanager
    // 2.ObjectManager
    // 3.ResourceManager
    // 4.Pool
    public class ResourceManager : MonoSingleton<ResourceManager>
    {
        // Synchronous resource loading
        public T Load<T>(string path) where T : Object
        {
            T resource = Resources.Load<T>(path);
            return resource is GameObject ? Instantiate(resource) : resource;
        }

        // Asynchronous resource loading
        public void LoadAsync<T>(string path, UnityAction<T> callback) where T : Object
        {
            StartCoroutine(LoadAsyncInternal(path, callback));
        }

        // Asynchronous resource loading
        private IEnumerator LoadAsyncInternal<T>(string path, UnityAction<T>  callback) where T :Object
        {
            ResourceRequest resourceRequest = Resources.LoadAsync<T>(path);
            yield return resourceRequest;

            if (resourceRequest.asset is GameObject)
            {
                callback(Instantiate(resourceRequest.asset) as T);
            }
            else
            {
                callback(resourceRequest.asset as T);
            }
        }
    }
}


