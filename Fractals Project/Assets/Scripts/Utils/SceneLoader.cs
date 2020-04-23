using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour  {
   
   public void Load(int i) {
      SceneManager.LoadScene(i);
   }
}
