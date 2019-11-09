using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace FancyScrollView
{
    [RequireComponent(typeof(Dropdown))]
    public class ScenesDropdown : MonoBehaviour
    {
        readonly string[] scenes =
        {
            "01_Basic",
            "02_FocusOn",
            "03_InfiniteScroll",
            "04_Metaball",
            "05_Voronoi",
            "06_LoopTabBar",
            "07_ScrollRect",
            "08_GridView",
        };

        [SerializeField] int defaultScene = default;

        Dropdown dropdown;

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
