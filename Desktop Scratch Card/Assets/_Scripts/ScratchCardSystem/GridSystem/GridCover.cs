using DG.Tweening;
using UnityEngine;

namespace _Scripts.ScratchCardSystem.GridSystem
{
    [RequireComponent(typeof(SpriteRenderer))][RequireComponent(typeof(BoxCollider2D))]
    public class GridCover : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _boxCollider2D;
        private SpriteRenderer _iconSpriteRenderer;
        private bool isRevealed = false;
        private bool isRevealing = false;
        private bool isMouseOver = false;  // 新增：追踪鼠标是否在当前格子上
        
        public Vector2Int grid;
        
        private static bool isMouseDown = false;

        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sortingOrder = 100;

            _iconSpriteRenderer = transform.Find("Cover Icon").GetComponent<SpriteRenderer>();

            _boxCollider2D = GetComponent<BoxCollider2D>();
            _boxCollider2D.isTrigger = true;
        }

        void Update()
        {
            // 更新鼠标按下状态
            if (Input.GetMouseButtonDown(0))
            {
                isMouseDown = true;
                // 如果鼠标在当前格子上且按下左键，立即reveal
                if (isMouseOver && !isRevealed && !isRevealing)
                {
                    RevealGrid();
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                isMouseDown = false;
            }
        }

        private void OnMouseEnter()
        {
            isMouseOver = true;
            
            if (isRevealing) return;

            if (!isRevealed)
            {
                if (isMouseDown)
                {
                    RevealGrid();
                }
                else
                {
                    _spriteRenderer.DOColor(Color.white, 0.1f);
                }
            }
            else
            {
                ScratchCardManager.onMouseOverRevealedItem?.Invoke(grid);
            }
        }

        private void OnMouseExit()
        {
            isMouseOver = false;
            
            if (isRevealing) return;

            if (!isRevealed) 
            {
                _spriteRenderer.DOColor(Color.gray, 0.1f);
            }
            else
            {
                ScratchCardManager.onMouseExitRevealedItem?.Invoke();
            }
        }

        private void RevealGrid()
        {
            isRevealing = true;

            _spriteRenderer.DOFade(0, 0.1f).OnComplete((() =>
            {
                isRevealed = true;
                isRevealing = false;
                ScratchCardManager.onCoverRevealStateChanged?.Invoke(grid, true);
                ScratchCardManager.onCoverRevealed?.Invoke(grid);
            }));

            _iconSpriteRenderer.DOFade(0, 0.1f);
        }
    }
}