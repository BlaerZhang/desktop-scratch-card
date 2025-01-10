using _Scripts.GridSystem;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))][RequireComponent(typeof(BoxCollider2D))]
public class GridCover : MonoBehaviour
{
     private SpriteRenderer _spriteRenderer;
    // private BoxCollider2D _boxCollider2D;
    private bool isRevealed = false;
    private bool isRevealing = false;

    // private SpriteRenderer _mergerBG;

    public Vector2Int grid;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        // _boxCollider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer.sortingOrder = 100;

        // _mergerBG = transform.Find("MergerBG").GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter()
    {
        if (isRevealing) return;

        if (!isRevealed) _spriteRenderer.DOColor(Color.white, 0.1f);
        else
        {
            //Check cluster and display
            ScratchCardManager.onMouseOverRevealedItem?.Invoke(grid);
        }
    }

    private void OnMouseExit()
    {
        if (isRevealing) return;

        if (!isRevealed) _spriteRenderer.DOColor(Color.gray, 0.1f);
        else
        {
            //Hide cluster
            ScratchCardManager.onMouseExitRevealedItem?.Invoke();
        }
    }

    private void OnMouseDown()
    {
        // if (IconManager.isIconMoving) return;
        if (isRevealing) return;

        if (isRevealed) ScratchCardManager.onMouseDownRevealedItem?.Invoke(grid);
        else RevealGrid();
    }

    private void RevealGrid()
    {
        // print(grid + "revealed");
        isRevealing = true;

        //Generate Icon

        _spriteRenderer.DOFade(0, 0.1f).OnComplete((() =>
        {
            isRevealed = true;
            isRevealing = false;
            ScratchCardManager.onCoverRevealStateChanged?.Invoke(grid, true);
            ScratchCardManager.onCoverRevealed?.Invoke(grid);
        }));
    }

    // public void ResetCover()
    // {
    //     isRevealed = false;
    //     _spriteRenderer.DOFade(1, 0);
    //     _spriteRenderer.DOColor(Color.gray, 0);
    // }
}
