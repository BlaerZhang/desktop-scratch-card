using System.Collections.Generic;
using _Scripts.ItemCountGenerator;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.GridSystem
{
    public class GridGenerator : MonoBehaviour
    {
        public Canvas textCanvas;
        public TextMeshProUGUI itemCountTextPrefab;

        private GridItemSO _gridItemSo;

        private ScratchCard scratchCardObject;
        private GameObject itemParentObject;
        private GameObject coverParentObject;
        private Canvas textCanvasObject;

        private int _rows;
        private int _columns;
        private Vector2 _gridGapLength;
        private Vector2 _startPoint;
        private GridData _gridData;
        private int[,] _itemCounts;

        public void Initialize(Vector2Int dimension, Vector2 gridGapLength, Vector2 startPoint, GridItemSO gridItemSo, GridData gridData, int[,] itemCounts)
        {
            // basic frame of the scratch card
            scratchCardObject = new GameObject("Scratch Card").AddComponent<ScratchCard>();
            textCanvasObject = Instantiate(textCanvas, scratchCardObject.transform);
            itemParentObject = new GameObject("Grid Items")
            {
                transform =
                {
                    parent = scratchCardObject.transform
                }
            };
            coverParentObject = new GameObject("Covers")
            {
                transform =
                {
                    parent = scratchCardObject.transform
                }
            };

            // fetch data
            _rows = dimension.x;
            _columns = dimension.y;
            _gridGapLength = gridGapLength;
            _startPoint = startPoint;
            _gridItemSo = gridItemSo;
            _gridData = gridData;
            _itemCounts = itemCounts;
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

        private GridItemData FetchGridItem(GridItemType type, int level)
        {
            return _gridItemSo.itemPool[type].itemLevelData[level];
        }

        private void DistributeItemCount(int row, int column, Vector2 position, GridItemType itemType, int itemCount)
        {
            TextMeshProUGUI itemCountText = Instantiate(itemCountTextPrefab, Camera.main.WorldToScreenPoint(position-new Vector2(0, 0.25f)), Quaternion.identity, textCanvasObject.transform);
            itemCountText.text = itemCount.ToString();
            itemCountText.fontSize = 36;
            itemCountText.alignment = TextAlignmentOptions.Center;

            itemCountText.GetComponent<ItemCountText>().Grid = new Vector2Int(row, column);

            itemCountText.DOFade(0, 0);

            // fill the reward list of the scratch card
            if (itemCount > 0)
            {
                scratchCardObject.AddReward(itemType, itemCount);
            }
        }

        /// <summary>
        /// generate a random grid in specific location
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public void GenerateRandomGrid(int row, int column)
        {
            GameObject itemObject = new GameObject("Item" + row + "_" + column)
            {
                transform =
                {
                    // set position
                    parent = itemParentObject.transform,
                    position = new Vector2(_startPoint.x + column * (1 + _gridGapLength.x), _startPoint.y - row * (1 + _gridGapLength.y) ),
                    localScale = Vector3.one
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

            // distribute item count
            int currentItemCount = _itemCounts[row, column];
            DistributeItemCount(row, column, itemObject.transform.position, itemType, currentItemCount);
        }

        /// <summary>
        /// generate a grid of specific type and level in specific location
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="type"></param>
        /// <param name="level"></param>
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

        public ScratchCard GenerateAllGrids()
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

            return scratchCardObject;
        }
    }
}
