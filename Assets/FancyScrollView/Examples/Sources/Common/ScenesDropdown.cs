using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace FancyScrollView
{
    [RequireComponent(typeof(Dropdown))]
    public class ScenesDropdown : MonoBehaviour
    {
        [SerializeField] int defaultScene = default;

        Dropdown dropdown;

        readonly string[] scenes = {
            "01_Basic",
            "02_FocusOn",
            "03_InfiniteScroll",
            "04_Metaball",
            "05_Voronoi",
            "06_LoopTabBar",
        };

        void Awake() => dropdown = GetComponent<Dropdown>();

        void Start()
        {
            dropdown.AddOptions(scenes.Select(x => new Dropdown.OptionData(x)).ToList());
            dropdown.value = defaultScene;
            dropdown.onValueChanged.AddListener(value =>
                SceneManager.LoadScene(scenes[value], LoadSceneMode.Single));
        }
    }
}
