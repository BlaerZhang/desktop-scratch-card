using System.Collections.Generic;
using _Scripts.ScratchCardSystem;
using _Scripts.ScratchCardSystem.GridSystem;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.PlayerUpgrades.ScratchCardUpgrades
{
    public class AdjacentSymbolUpgrade : ScratchCardUpgrade
    {
        public GridItemType mainType;
        public List<GridItemType> adjacentRewardTypes = new List<GridItemType>();

        // TODO: add number to the text
        private void AddRewardItemCount()
        {

        }

        public override bool CheckWin(ScratchCard card, out Sequence winEffect)
        {
            winEffect = DOTween.Sequence();
            winEffect.SetLink(card.gameObject);

            bool hasWon = false;

            var gridItems = card.gridData.items;
            int rows = gridItems.GetLength(0);
            int columns = gridItems.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var currentItem = gridItems[i, j];

                    if (!currentItem.type.Equals(mainType)) continue;

                    if (j < columns - 2)
                    {
                        var rightItem = gridItems[i, j + 1];
                        if (adjacentRewardTypes.Contains(rightItem.type))
                        {
                            // effect
                            winEffect.Append(currentItem.transform.DOShakeScale(winEffectDuration));
                            winEffect.Join(rightItem.transform.DOShakeScale(winEffectDuration));
                            hasWon = true;
                        }
                    }
                    if (i < rows - 2)
                    {
                        var downItem = gridItems[i + 1, j];
                        if (adjacentRewardTypes.Contains(downItem.type))
                        {
                            // effect
                            winEffect.Append(currentItem.transform.DOShakeScale(winEffectDuration));
                            winEffect.Join(downItem.transform.DOShakeScale(winEffectDuration));
                            hasWon = true;
                        }
                    }
                    if (j > 0)
                    {
                        var leftItem = gridItems[i, j - 1];
                        if (adjacentRewardTypes.Contains(leftItem.type))
                        {
                            // effect
                            winEffect.Append(currentItem.transform.DOShakeScale(winEffectDuration));
                            winEffect.Join(leftItem.transform.DOShakeScale(winEffectDuration));
                            hasWon = true;
                        }
                    }
                    if (i > 0)
                    {
                        var upItem = gridItems[i - 1, j];
                        if (adjacentRewardTypes.Contains(upItem.type))
                        {
                            // effect
                            winEffect.Append(currentItem.transform.DOShakeScale(winEffectDuration));
                            winEffect.Join(upItem.transform.DOShakeScale(winEffectDuration));
                            hasWon = true;
                        }
                    }
                }
            }

            return hasWon;
        }
    }
}