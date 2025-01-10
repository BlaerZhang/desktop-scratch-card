using System;
using _Scripts.GridSystem;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace _Scripts.ItemCountGenerator
{
    public class ItemCountText : MonoBehaviour
    {
        public Vector2Int Grid { private get; set; }

        private void OnEnable()
        {
            GridManager.onCoverRevealed += Show;
        }

        private void OnDisable()
        {
            GridManager.onCoverRevealed -= Show;
        }

        private void Show(Vector2Int revealedGrid)
        {
            if (revealedGrid.Equals(Grid))
            {
                GetComponent<TextMeshProUGUI>().DOFade(1, 0.1f);
            }
        }

    }
}