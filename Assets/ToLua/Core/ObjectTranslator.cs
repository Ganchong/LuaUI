﻿/*
Copyright (c) 2015-2017 topameng(topameng@qq.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace LuaInterface
{
    public class ObjectTranslator
	{   /** 延迟GC对象 */   
        private class DelayGC
        {
            public DelayGC(int id, UnityEngine.Object obj, float time)
            {
                this.id = id;
                this.time = time;
                this.obj = obj;
            }

            public int id;
            public UnityEngine.Object obj;
            public float time;
        }

        private class CompareObject : IEqualityComparer<object>
        {
            public new bool Equals(object x, object y)
            {
                return object.ReferenceEquals(x, y);                
            }

            public int GetHashCode(object obj)
            {
                return RuntimeHelpers.GetHashCode(obj);                
            }
        }

        public bool LogGC { get; set; }
		/** object对象在对象池中的索引 */
        public readonly Dictionary<object, int> objectsBackMap = new Dictionary<object, int>(new CompareObject());
		/** object对象池 */
        public readonly LuaObjectPool objects = new LuaObjectPool();
		/** 延迟gc列表，目前只在GameObject.Destroy(go,float t)传递了参数t时才使用 */
        private List<DelayGC> gcList = new List<DelayGC>();

#if !MULTI_STATE
        private static ObjectTranslator _translator = null;
#endif

        public ObjectTranslator()
        {
			/** 是否在RemoveObject时，打印信息 */
            LogGC = false;
#if !MULTI_STATE
            _translator = this;
#endif
        }
		/** 缓存一个object，返回在LuaObject中的索引 */
        public int AddObject(object obj)
        {
            int index = objects.Add(obj);

            if (!TypeChecker.IsValueType(obj.GetType()))
            {
                objectsBackMap[obj] = index;
            }

            return index;
        }

        public static ObjectTranslator Get(IntPtr L)
        {
#if !MULTI_STATE
                return _translator;
#else
                return LuaState.GetTranslator(L);
#endif
        }

        //fixed 枚举唯一性问题（对象唯一，没有实现__eq操作符）
        void RemoveObject(object o, int udata)
        {
            int index = -1;
            
            if (objectsBackMap.TryGetValue(o, out index) && index == udata)
            {
                objectsBackMap.Remove(o);
            }
        }

        //lua gc一个对象(lua 库不再引用，但不代表c#没使用)
		/** 根据索引，移除一个object缓存 */
        public void RemoveObject(int udata)
        {            
            //只有lua gc才能移除
            object o = objects.Remove(udata);

            if (o != null)
            {
                if (!TypeChecker.IsValueType(o.GetType()))
                {
                    RemoveObject(o, udata);
                }

                if (LogGC)
                {
                    Debugger.Log("gc object {0}, id {1}", o, udata);
                }
            }
        }
		/** 根据索引获得一个object缓存 */
        public object GetObject(int udata)
        {
            return objects.TryGetValue(udata);         
        }

        //预删除，但不移除一个lua对象(移除id只能由gc完成)
        public void Destroy(int udata)
        {            
            object o = objects.Destroy(udata);

            if (o != null)
            {
                if (!TypeChecker.IsValueType(o.GetType()))
                {
                    RemoveObject(o, udata);
                }

                if (LogGC)
                {
                    Debugger.Log("destroy object {0}, id {1}", o, udata);
                }
            }
        }

        //Unity Object 延迟删除
        public void DelayDestroy(int id, float time)
        {
            UnityEngine.Object obj = (UnityEngine.Object)GetObject(id);

            if (obj != null)
            {
                gcList.Add(new DelayGC(id, obj, time));
            }            
        }

        public bool Getudata(object o, out int index)
        {
            index = -1;
            return objectsBackMap.TryGetValue(o, out index);
        }

        public void Destroyudata(int udata)
        {
            objects.Destroy(udata);
        }

        public void SetBack(int index, object o)
        {
            objects.Replace(index, o);            
        }

        bool RemoveFromGCList(int id)
        {
            int index = gcList.FindIndex((p) => { return p.id == id; });

            if (index >= 0)
            {
                gcList.RemoveAt(index);
                return true;                       
            }

            return false;
        }
        
        //延迟删除处理
        void DestroyUnityObject(int udata, UnityEngine.Object obj)
        {
            object o = objects.TryGetValue(udata);

            if (object.ReferenceEquals(o, obj))
            {
                RemoveObject(o, udata);
                //一定不能Remove, 因为GC还可能再来一次
                objects.Destroy(udata);     

                if (LogGC)
                {
                    Debugger.Log("destroy object {0}, id {1}", o, udata);
                }
            }

            UnityEngine.Object.Destroy(obj);
        }

        public void Collect()
        {
            if (gcList.Count == 0)
            {
                return;
            }

            float delta = Time.deltaTime;

            for (int i = gcList.Count - 1; i >= 0; i--)
            {
                float time = gcList[i].time - delta;

                if (time <= 0)
                {
                    DestroyUnityObject(gcList[i].id, gcList[i].obj);                    
                    gcList.RemoveAt(i);
                }
                else
                {
                    gcList[i].time = time;
                }
            }
        }

        public void Dispose()
        {
            objectsBackMap.Clear();
            objects.Clear();     
            
#if !MULTI_STATE
            _translator = null;
#endif
        }
    }
}