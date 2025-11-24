using System.Collections.Generic;
using DG.Tweening;
using SceneManagement.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace SceneManagement.Loading
{
    public class ScrollLoading : LoadingScreen
    {
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");

        [FoldoutGroup("Customisation"), SerializeField]
        private List<ColorTemplate> _colourTemplates = new List<ColorTemplate>();

        [FoldoutGroup("Scrolling Settings")] [SerializeField]
        private Image _logoImage;

        [FoldoutGroup("Scrolling Settings")] [SerializeField]
        private List<Image> _scrollingImages = new List<Image>();

        [FoldoutGroup("Scrolling Settings")] [SerializeField]
        private Vector2 _scrollDirection = new Vector2(1, 0);

        [FoldoutGroup("Scrolling Settings")] [SerializeField]
        private float _scrollSpeed = 0.1f;

        [FoldoutGroup("Background Settings")] [SerializeField]
        private Image _backgroundImage;

        private bool _isScrolling;
        private Vector2 _currentOffset;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _currentOffset = _scrollingImages[0].material.GetTextureOffset(MainTex);
        }

        private void Update()
        {
            if (!_isScrolling) return;

            ScrollImages();
        }

        private void ScrollImages()
        {
            _currentOffset += _scrollDirection * (_scrollSpeed * Time.deltaTime);

            foreach (var image in _scrollingImages)
            {
                image.material.SetTextureOffset(MainTex, _currentOffset);
            }
        }

        public ColorTemplate GetRandomTemplate() => _colourTemplates[Random.Range(0, _colourTemplates.Count)];

        public void SetColours()
        {
            ColorTemplate colorTemplate = GetRandomTemplate();
            _scrollingImages.ForEach(i => i.color = colorTemplate.highlightColour);
            _backgroundImage.sprite = colorTemplate.backgroundSprite;
        }

        public void ShowVisuals()
        {
            SetColours();

            _backgroundImage.raycastTarget = true;
            _backgroundImage.DOFade(0, 0).OnComplete(() => _backgroundImage.DOFade(1, 0.5f));
            _logoImage.DOFade(1, 0.5f);
            foreach (var image in _scrollingImages)
            {
                image.DOFade(0, 0).OnComplete(() => image.DOFade(1, 0.5f));
            }
        }

        public void HideVisuals()
        {
            _backgroundImage.raycastTarget = false;
            _backgroundImage.DOFade(1, 0).OnComplete(() => _backgroundImage.DOFade(0, 0.5f));
            _logoImage.DOFade(0, 0.5f);

            foreach (var image in _scrollingImages)
            {
                image.DOFade(1, 0).OnComplete(() => image.DOFade(0, 0.5f));
            }
        }

        public override void StartLoading()
        {
            base.StartLoading();
            ShowVisuals();
            _isScrolling = true;
        }

        public override void FinishLoading()
        {
            base.FinishLoading();
            HideVisuals();
            _isScrolling = false;
        }

        private void OnDisable()
        {
            foreach (var image in _scrollingImages)
            {
                image.material.SetTextureOffset(MainTex, Vector2.zero);
            }
        }
    }
}