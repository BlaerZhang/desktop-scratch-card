using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace _Scripts.GridSystem
{
    public class GridGenerator
    {
        private GridItemSO _gridItemSo;

        private GameObject itemParentObject = new GameObject("GridItemParent");
        private GameObject coverParentObject = new GameObject("CoverParent");

        private int _rows;
        private int _columns;
        private Vector2 _gridGapLength;
        private Vector2 _startPoint;
        private GridData _gridData;

        public GridGenerator(int rows, int columns, Vector2 gridGapLength, Vector2 startPoint, GridItemSO gridItemSo, GridData gridData)
        {
            _rows = rows;
            _columns = columns;
            _gridGapLength = gridGapLength;
            _startPoint = startPoint;
            _gridItemSo = gridItemSo;
            _gridData = gridData;
        }

        private void GenerateCover(int row, int column)
        {
            GameObject cover = new GameObject("cover_" + row + "_" + column)
            {
                transform =
                {
                    // set position
                    parent = coverParentObject.transform,
                    position = new Vector2(_startPoint.x + column * (1 + _gridGapLength.x), _startPoint.y - row * (1 + _gridGapLength.y))
                }
            };
            var spriteRenderer = cover.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>("DefaultAssets/Textures/Square");
            spriteRenderer.color = Color.gray;

            spriteRenderer.sortingOrder = 999;

            var gridCover = cover.AddComponent<GridCover>();
            gridCover.grid = new Vector2Int(row, column);

            // generate cover BG
            // GameObject bg = new GameObject("MergerBG")
            // {
            //     transform =
            //     {
            //         parent = cover.transform,
            //         position = cover.transform.position
            //     }
            // };
            // var bgSpriteRenderer = bg.AddComponent<SpriteRenderer>();
            // bgSpriteRenderer.sprite = Resources.Load<Sprite>("DefaultAssets/Textures/Square");
            // bgSpriteRenderer.color = Color.gray;
            // bgSpriteRenderer.sortingOrder = -101;

            // _gridData.covers[row, column] = mergerGridCover;
        }

        public GridItemData FetchGridItem(GridItemType type, int level)
        {
            return _gridItemSo.itemPool[type].itemLevelData[level];
        }

        public void GenerateRandomGrid(int row, int column)
        {
            GameObject itemObject = new GameObject("Item" + row + "_" + column)
            {
                transform =
                {
                    // set position
                    parent = itemParentObject.transform,
                    position = new Vector2(_startPoint.x + column * (1 + _gridGapLength.x), _startPoint.y - row * (1 + _gridGapLength.y) ),
                    localScale = Vector3.one * 0.6f
                }
            };

            // randomize icon item
            var itemType = Utils.CalculateMultiProbability(_gridItemSo.itemPool);
            int randItemLevelDataIndex = Utils.CalculateMultiProbability(_gridItemSo.itemPool[itemType].itemLevelData);
            var itemData = FetchGridItem(itemType, randItemLevelDataIndex);

            var gridItem = itemObject.AddComponent<GridItem>();
            gridItem.Initialize(itemType, itemData);

            _gridData.items[row, column] = gridItem;

            // SetItemData(gridItem, randItemData);

            // set sprite
            SpriteRenderer sr = itemObject.AddComponent<SpriteRenderer>();
            sr.sprite = itemData.image;
        }

        public void GenerateSingleGrid(int row, int column, GridItemType type, int level)
        {
            GameObject itemObject = new GameObject("Item" + row + "_" + column)
            {
                transform =
                {
                    // set position
                    parent = itemParentObject.transform,
                    position = new Vector2(_startPoint.x + column, _startPoint.y - row),
                    localScale = Vector3.one * 0.6f
                }
            };

            var itemType = type;
            var itemData = FetchGridItem(itemType, level);

            var gridItem = itemObject.AddComponent<GridItem>();
            gridItem.Initialize(type, itemData);

            _gridData.items[row, column] = gridItem;

            // SetItemData(gridItem, randItemData);

            // set sprite
            SpriteRenderer sr = itemObject.AddComponent<SpriteRenderer>();
            sr.sprite = itemData.image;
        }

        public void GenerateAllGrids()
        {
            _gridData.items = new GridItem[_rows, _columns];
            // _gridData.covers = new MergerGridCover[_rows, _columns];
            _gridData.revealedGrids = new List<Vector2Int>();

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    GenerateRandomGrid(i, j);
                    // add cover
                    GenerateCover(i, j);
                }
            }
        }
    }
}
