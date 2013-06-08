using UnityEngine;

///
/// Singleton base class.
/// Derive this class to make it Singleton.
/// 
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
   protected static T instance;
 
   /**
      Returns the instance of this singleton.
   */
   public static T Instance
   {
      get
      {
         if(instance == null)
         {
            instance = (T) FindObjectOfType(typeof(T));
 
            if (instance == null)
            {
				GameObject obj = new GameObject(typeof(T).ToString());
                instance = obj.AddComponent<T>();				
               //Debug.LogError("An instance of " + typeof(T) + 
               //   " is needed in the scene, but there is none.");
            }
         }
 
         return instance;
      }
   }
}