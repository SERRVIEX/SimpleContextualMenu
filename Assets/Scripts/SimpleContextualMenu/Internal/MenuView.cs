namespace SimpleContextualMenu.Internal
{
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(VerticalLayoutGroup))]
    [RequireComponent(typeof(ContentSizeFitter))]
    public sealed class MenuView : MonoBehaviour
    {
        [field: SerializeField] public RectTransform RectTransform { get; private set; }
        [SerializeField] private Image _background;
        [SerializeField] private VerticalLayoutGroup _layoutGroup;
        [SerializeField] private ContentSizeFitter _sizeFitter;

        // Constructors

        private MenuView() { }

        // Methods

        private void OnValidate()
        {
            if (RectTransform == null)
                RectTransform = GetComponent<RectTransform>();

            RectTransform.SetAnchor(Anchor.TopLeft);
            RectTransform.SetPivot(Pivot.TopLeft);

            if (_background == null)
                _background = GetComponent<Image>();

            if (_layoutGroup == null)
                _layoutGroup = GetComponent<VerticalLayoutGroup>();

            if(_sizeFitter == null)
                _sizeFitter = GetComponent<ContentSizeFitter>();

            _sizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            _sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
    }
}