using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FancyScrollView
{
    public class GameObjectPool<T>
    {
        public string name { get; private set; }

        private Dictionary<T, List<Transform>> pool = new Dictionary<T, List<Transform>>();

        private Dictionary<T, Func<T, Transform>> poolFunc = new Dictionary<T, Func<T, Transform>>();

        private GameObject _pool;

        GameObject objpool
        {
            get
            {
                if(_pool == null)
                {
                    _pool = new GameObject(name);
                    _pool.SetActive(false);
                }
                return _pool;
            }
        }

        Func<T, Transform> DeFaultFunc;

        private GameObjectPool()
        {

        }

        public GameObjectPool(string poolname, Func<T, Transform> func)
        {
            name = poolname;
            DeFaultFunc = func;
        }


        public bool Contains(Transform target)
        {
            var en = pool.GetEnumerator();
            while (en.MoveNext())
            {
                var list = en.Current.Value;
                if (list.Contains(target))
                {
                    return true;
                }
            }

            return false;

        }

        public void SetFunc(T key, Func<T, Transform> passfunc)
        {
            if (poolFunc.ContainsKey(key))
            {
                poolFunc[key] += passfunc;
            }
            else
            {
                poolFunc[key] = passfunc;
            }
        }


        public void Clear()
        {
            poolFunc.Clear();

            var en = pool.GetEnumerator();
            while(en.MoveNext())
            {
                ListPool<Transform>.Release(en.Current.Value);
            }
            pool.Clear();

            if (objpool != null)
            {
                int cnt = objpool.transform.childCount;
                for (int i = cnt - 1; i >= 0; i--)
                {
                    GameObject.Destroy(objpool.transform.GetChild(i).gameObject);
                }
            }
        }

        private Transform TryCallFunc(T key)
        {
            Func<T, Transform> func;
            if (poolFunc.TryGetValue(key, out func))
            {
                if (func != null)
                {
                    return func(key);
                }
            }

            return DeFaultFunc(key);
        }

        public void RemoveKey(T key)
        {
            List<Transform> list = null;
            if (pool.TryGetValue(key, out list))
            {
                ListPool<Transform>.Release(list);
                pool.Remove(key);
            }
        }

        public void PreLoad(T key, int cnt)
        {
            if (cnt > 0)
            {
                List<Transform> list = null;
                if (!pool.TryGetValue(key,out list))
                {
                    list = ListPool<Transform>.Get();
                    pool.Add(key, list);
                }
 
                while (list.Count < cnt)
                {
                    Transform preloadobj = TryCallFunc(key);
                    if (preloadobj == null)
                        throw new ArgumentException("preload Is Null");

                    AddInstance(objpool, preloadobj);
                    preloadobj.gameObject.SetActive(false);
                    list.Add(preloadobj);
                }
            }
        }

        static Transform AddInstance(GameObject parent, Transform ins, bool worldPositionStays = false)
        {
            Transform transform = ins.transform;
            transform.SetParent(parent.transform, worldPositionStays);
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
            transform.gameObject.layer = parent.gameObject.layer;
            transform.tag = parent.tag;

            return ins;
        }

        public Transform Spawn(T key)
        {
            if (pool.ContainsKey(key))
            {
                List<Transform> list = pool[key];
                if (list.Count == 0)
                {
                    return this.TryCallFunc(key);
                }
                Transform obj = list[0];
                list.RemoveAt(0);

                obj.SetParent(null);
                return obj;
            }

            return this.TryCallFunc(key);
        }


        public Transform Spawn(T key, Transform Parent)
        {
            if (pool.ContainsKey(key))
            {
                List<Transform> list = pool[key];
                if (list.Count == 0)
                {
                    return this.TryCallFunc(key);
                }
                Transform obj = list[0];
                list.RemoveAt(0);

                obj.SetParent(Parent);
                return obj;
            }

            Transform created = this.TryCallFunc(key);
            if (created != null)
            {
                created.SetParent(Parent);
            }
            return created;
        }

        public bool DeSpawn(T key, Transform obj)
        {
            if (obj != null && objpool != null)
            {
                AddInstance(objpool, obj);

                if (pool.ContainsKey(key))
                {
                    pool[key].Add(obj);
                }
                else
                {
                    List<Transform> list = ListPool<Transform>.Get();
                    list.Add(obj);
                    pool[key] = list;
                }

                return true;
            }

            return false;
        }
    }
}


