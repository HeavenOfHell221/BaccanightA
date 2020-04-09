using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : SingletonBehaviour<ObjectPooler>
{
	[System.Serializable]
	public class Pool
	{
		public GameObject m_PrefabToPool;
		public LinkedList<GameObject> m_Pool;

		public Pool(GameObject prefabToPool)
		{
			m_Pool = new LinkedList<GameObject>();
			m_PrefabToPool = prefabToPool;
		}

		public GameObject SpawnObject()
		{
            GameObject obj = Instantiate(m_PrefabToPool);
            obj.SetActive(false);
            return obj;
		}

		private void ActiveObject(GameObject obj, Vector3 position, Quaternion rotation, Transform parent)
		{
			obj.transform.position = position;
			obj.transform.rotation = rotation;
			obj.SetActive(true);
            obj.transform.SetParent(parent, true);

			if (m_Pool.Contains(obj))
			{
				m_Pool.Remove(obj);
			}

			m_Pool.AddLast(obj);
		}

		public GameObject GetObject(Vector3 position, Quaternion rotation)
		{
			GameObject objectRequested = null;
			foreach (GameObject obj in m_Pool)
			{
				if (obj == null)
				{
					break;
				}
				else if (!obj.activeInHierarchy) // S'il y a un objet non actif, on le récupère et on n'agrandit pas la pool
				{
					objectRequested = obj;
					break;
				}
			}

			if (objectRequested == null)
			{
				objectRequested = SpawnObject();
            }

            ActiveObject(objectRequested, position, rotation, Instance.transform);
			return objectRequested;
		}
	}

    #region Variables
	private Dictionary<GameObject, Pool> poolDictionary;
	#endregion

	private void Start()
	{
		poolDictionary = new Dictionary<GameObject, Pool>();
	}

	public GameObject SpawnFromPool(GameObject prefab, Vector3 position, Quaternion rotation = new Quaternion())
	{
		if (!poolDictionary.ContainsKey(prefab))
		{
			poolDictionary.Add(prefab, new Pool(prefab));
		}

		return poolDictionary[prefab].GetObject(position, rotation);
	}
}
