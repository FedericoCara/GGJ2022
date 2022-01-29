using UnityEngine;

namespace Mimic{
	
	/// <summary>
	/// Be aware this will not prevent a non singleton constructor such as `T myT = new T();`
	/// To prevent that, add `protected T () {}` to your singleton class.
	/// </summary>
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
		
		protected static T instance;

	    private static object _lock = new object();

	    private static bool applicationIsQuitting = false;

	    public static T Instance {
	        get {
	            if (applicationIsQuitting) {
	                Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed on application quit." + " Won't create again.");
                    //return instance;
                    applicationIsQuitting = false;
                }

	            lock (_lock) {
					if (instance == null){
						T[] instances = FindObjectsOfType<T> ();

						if (instances.Length == 0) {
							GameObject singletonGO = new GameObject(typeof(T).Name);
							instance = singletonGO.AddComponent<T>();
						} else {
							instance = instances [0];
							if (instances.Length > 1) {
								Debug.LogError ("[Singleton] Something went really wrong " + " - there should never be more than 1 singleton instance!");
							}
						}
	                }
					return instance;
	            }
	        }
			protected set => instance = value;
	    }

	    /// <summary>
	    /// When Unity quits, it destroys objects in a random order.
	    /// In principle, a Singleton is only destroyed when application quits.
	    /// If any script calls Instance after it have been destroyed,  it will create a buggy ghost object that will stay on the Editor scene even after stopping playing the Application.
	    /// This was made to be sure we're not creating that buggy ghost object.
	    /// </summary>
	    public void OnApplicationQuit() {
	        applicationIsQuitting = true;
	    }
	}
}