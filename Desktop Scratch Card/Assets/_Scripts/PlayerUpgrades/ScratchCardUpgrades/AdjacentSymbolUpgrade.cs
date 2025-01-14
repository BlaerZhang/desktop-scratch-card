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

        public int rewardItemCount = 2;

        // TODO: add number to the text
        private Tween AddRewardItemCount(int itemCount, TMP_Text itemCountText)
        {
            int finalCount = itemCount + rewardItemCount;

            return DOVirtual.Int(itemCount, finalCount, 0.2f,
                value => itemCountText.text = value.ToString());
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
                            winEffect.Join(AddRewardItemCount(Int32.Parse(currentItem.ItemCountText.text), currentItem.ItemCountText));

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
                            winEffect.Join(AddRewardItemCount(Int32.Parse(currentItem.ItemCountText.text), currentItem.ItemCountText));

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
                            winEffect.Join(AddRewardItemCount(Int32.Parse(currentItem.ItemCountText.text), currentItem.ItemCountText));

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
                            winEffect.Join(AddRewardItemCount(Int32.Parse(currentItem.ItemCountText.text), currentItem.ItemCountText));

                            hasWon = true;
                        }
                    }
                }
            }

            return hasWon;
        }
    }
}