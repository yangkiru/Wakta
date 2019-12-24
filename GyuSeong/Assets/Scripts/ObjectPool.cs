using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private Queue<GameObject> objs;

    public ObjectPool()
    {
        objs = new Queue<GameObject>();
    }

    public void EnqueueObjectPool(GameObject obj, bool isRemoveChild = false)
    {
        if (isRemoveChild)
        {
            int count = obj.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                obj.transform.GetChild(0).SetParent(transform);
            }
        }
        obj.transform.SetParent(transform);
        obj.SetActive(false);
        objs.Enqueue(obj);
    }

    public GameObject DequeueObjectPool()
    {
        try
        {
            GameObject temp = objs.Dequeue();
            if (!temp.activeInHierarchy)
                return temp;
            else return DequeueObjectPool();
        }
        catch
        {
            return null;
        }
    }

    public void Clear()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.activeSelf)
                EnqueueObjectPool(child);
        }
    }

    public int GetCount()
    {
        return objs.Count;
    }
}
