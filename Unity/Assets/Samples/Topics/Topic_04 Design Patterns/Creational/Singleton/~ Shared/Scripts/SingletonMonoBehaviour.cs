﻿using UnityEngine;

namespace RMC.BestPractices.DesignPatterns.CreationalPatterns.SingletonPattern.Shared
{
	public abstract class SingletonMonobehavior<T> : MonoBehaviour where T : MonoBehaviour
	{
		//  Properties ------------------------------------------
		public static bool IsInstantiated
		{
			get
			{
				return _Instance != null;
			}
		}

		public static T Instance
		{
			get
			{
				if (!IsInstantiated)
				{
					Instantiate();
				}
				return _Instance;
			}
			set
			{
				_Instance = value;
			}

		}

		//  Fields -------------------------------------------------
		private static T _Instance;
		public delegate void OnInstantiateCompletedDelegate(T instance);
		public static OnInstantiateCompletedDelegate OnInstantiateCompleted;

		//  Instantiation ------------------------------------------

		public static T Instantiate()
		{
			if (!IsInstantiated)
			{
				_Instance = GameObject.FindFirstObjectByType<T>();

				if (_Instance == null)
				{
					GameObject go = new GameObject();
					_Instance = go.AddComponent<T>();
					go.name = _Instance.GetType().FullName;
					DontDestroyOnLoad(go);
				}

				// Notify child scope
				(_Instance as SingletonMonobehavior<T>).InstantiateCompleted();

				// Notify observing scope(s)
				if (OnInstantiateCompleted != null)
				{
					OnInstantiateCompleted(_Instance);
				}
			}
			return _Instance;
		}


		//  Unity Methods ------------------------------------------
		public static void Destroy()
		{

			if (IsInstantiated)
			{
				if (Application.isPlaying)
				{
					Destroy(_Instance.gameObject);
				}
				else
				{
					DestroyImmediate(_Instance.gameObject);
				}

				_Instance = null;
			}
		}

		//  Other Methods ------------------------------------------
		public virtual void InstantiateCompleted()
		{
			// Override, if desired
		}

	}
}