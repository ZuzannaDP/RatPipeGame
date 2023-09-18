using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    [SerializeField]
    private GameObject LevelButtonPrefab;
    public LevelData[] levels;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // for (int l = 0; l < levels.Length; l++) {
        //     GameObject newButton = Instantiate(LevelButtonPrefab);
        //     newButton.transform.SetParent(transform, false);
        // }
    }
}
