using System.Collections;
using System.Collections.Generic;
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

        readonly string[] scenes = new[] {
            "01_Basic",
            "02_FocusOn",
            "03_InfiniteScroll",
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
