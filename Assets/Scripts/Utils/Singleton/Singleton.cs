using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//单例
public class Singleton<T> where T : Singleton<T>, new() {
	public static T Instance {
		get {
			if (instance == null) {
				instance = new T();
			}
			return instance;
		}
	}

	private static T instance;
}

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T> {
	public static T Instance {
		get {
			return instance;
		}
	}

	public static T InstanceNotNull {
		get {
			CreateInstanceIfNotExits();
			return instance;
		}
	}

	private static T instance;
	protected static Func<T> createInstanceHandler;

	protected virtual void Awake() {
		instance = (T)this;
	}

	public static void CreateInstanceIfNotExits() {
		if (instance != null) {
			return;
		}
		if (createInstanceHandler != null) {
			instance = createInstanceHandler();
			return;
		}
		GameObject obj = new GameObject(typeof(T).Name + "_Instance");
		instance = obj.AddComponent(typeof(T)) as T;
	}

	protected virtual void OnDestroy() {
		if (instance == this) instance = null;
	}
}
