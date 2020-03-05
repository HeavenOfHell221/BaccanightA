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

		private void ActiveObject(GameObject obj, Vector3 position, Quaternion rotation)
		{
			obj.transform.position = position;
			obj.transform.rotation = rotation;
			obj.SetActive(true);

			if (m_Pool.Contains(obj))
			{
				m_Pool.Remove(obj);
			}

			m_Pool.AddLast(obj);
		}

		public GameObject GetObject(Vector3 position, Quaternion rotation)
		{
			GameObject obj = null;
			foreach (GameObject o in m_Pool)
			{
				if (o == null)
				{
					break;
				}
				else if (!o.activeInHierarchy) // S'il y a un objet non actif, on le récupère et on n'agrandit pas la pool
				{
					obj = o;
					break;
				}
			}

			if (obj == null)
			{
				obj = SpawnObject();
			}

			ActiveObject(obj, position, rotation);
			return obj;
		}
	}

	#region Variables
	private Dictionary<GameObject, Pool> poolDictionary;
	#endregion

	private void Start()
	{
		poolDictionary = new Dictionary<GameObject, Pool>();
	}

	public GameObject SpawnFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		if (!poolDictionary.ContainsKey(prefab))
		{
			poolDictionary.Add(prefab, new Pool(prefab));
		}

		return poolDictionary[prefab].GetObject(position, rotation);
	}
}
