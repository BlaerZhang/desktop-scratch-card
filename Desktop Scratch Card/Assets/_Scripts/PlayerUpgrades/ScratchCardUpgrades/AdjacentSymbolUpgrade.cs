using System;
using System.Collections.Generic;
using _Scripts.ScratchCardSystem;
using _Scripts.ScratchCardSystem.GridSystem;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Scripts.PlayerUpgrades.ScratchCardUpgrades
{
    public class AdjacentSymbolUpgrade : ScratchCardUpgrade
    {
        public GridItemType mainType;
        public List<GridItemType> adjacentRewardTypes = new List<GridItemType>();

        public int rewardItemCount = 1;

        public override bool CheckWin(ScratchCard card, out Sequence winEffect)
        {
            winEffect = DOTween.Sequence();
            winEffect.SetLink(card.gameObject);

            bool hasWon = false;
            var gridItems = card.gridData.items;
            int rows = gridItems.GetLength(0);
            int columns = gridItems.GetLength(1);

            // 按照矩阵顺序处理每个mainType符号
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var currentItem = gridItems[i, j];
                    if (!currentItem.type.Equals(mainType)) continue;

                    int initialItemCount = Int32.Parse(currentItem.itemCountText.text);
                    int finalItemCount = Int32.Parse(currentItem.itemCountText.text);

                    // 顺时针方向数组：右、下、左、上
                    (int di, int dj)[] directions = new[]
                    {
                        (0, 1),   // 右
                        (1, 0),   // 下
                        (0, -1),  // 左
                        (-1, 0)   // 上
                    };

                    // 为当前符号创建一个子序列处理其周围的符号
                    Sequence itemSequence = DOTween.Sequence();
                    bool hasAdjacentMatch = false;

                    // 检查四个方向
                    foreach (var (di, dj) in directions)
                    {
                        int newI = i + di;
                        int newJ = j + dj;

                        // 检查边界
                        if (newI >= 0 && newI < rows && newJ >= 0 && newJ < columns)
                        {
                            var adjacentItem = gridItems[newI, newJ];
                            if (adjacentRewardTypes.Contains(adjacentItem.type))
                            {
                                hasAdjacentMatch = true;

                                // 创建这对符号的动画序列
                                Sequence pairSequence = DOTween.Sequence();

                                // 同时抖动当前符号和相邻符号
                                pairSequence.Append(DOTween.Sequence()
                                    .Join(currentItem.transform.DOScale(Vector3.one * symbolEffectScale, winEffectDuration).SetLoops(2, LoopType.Yoyo))
                                    .Join(adjacentItem.transform.DOScale(Vector3.one * symbolEffectScale, winEffectDuration).SetLoops(2, LoopType.Yoyo)));

                                // 更新计数
                                finalItemCount += rewardItemCount;
                                currentItem.itemCount = finalItemCount;
                                pairSequence.Append(AddRewardItemCount(initialItemCount, finalItemCount, currentItem.itemCountText));
                                initialItemCount = finalItemCount;

                                // 添加到当前符号的序列中
                                itemSequence.Append(pairSequence);
                            }
                        }
                    }

                    // 如果当前符号有匹配的相邻符号，将其序列添加到主序列
                    if (hasAdjacentMatch)
                    {
                        winEffect.Append(itemSequence);
                        hasWon = true;
                    }
                }
            }

            return hasWon;
        }

        private Tween AddRewardItemCount(int initialCount, int finalCount, TMP_Text itemCountText)
        {
            return DOVirtual.Int(initialCount, finalCount, .5f,
                value => itemCountText.text = value.ToString());
        }
    }
}