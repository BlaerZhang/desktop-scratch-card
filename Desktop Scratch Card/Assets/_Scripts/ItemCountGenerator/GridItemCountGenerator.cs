using System.Collections.Generic;
using _Scripts.ScratchCardSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.ItemCountGenerator
{
    public class GridItemCountGenerator : SerializedMonoBehaviour
    {
        public Dictionary<int, float> countProbability = new Dictionary<int, float>();

        public int[,] GenerateGridItemCount(Vector2Int dimension)
        {
            int totalCount = GenerateTotalItemCount();

            // randomly distribute the total count to each grid
            return DistributeCount(dimension, totalCount);
        }

        private int GenerateTotalItemCount()
        {
            // generate the total count
            return Utils.CalculateMultiProbability(countProbability);
        }

        public int[] SplitRandomly(int total, int n, int minValue = 1)
        {
            // 参数验证
            if (n <= 0)
            {
                Debug.LogError("份数必须大于0");
                return null;
            }
            if (total < 0)
            {
                Debug.LogError("总数不能为负数");
                return null;
            }
            if (minValue < 0)
            {
                Debug.LogError("最小值不能为负数");
                return null;
            }
            if (minValue * n > total)
            {
                Debug.LogError($"最小值 {minValue} 乘以份数 {n} 超过了总数 {total}");
                return null;
            }

            // 特殊情况处理
            if (n == 1)
            {
                return new int[] { total };
            }

            int[] result = new int[n];
            int remainingTotal = total;

            // 首先确保每份都至少得到最小值
            if (minValue > 0)
            {
                for (int i = 0; i < n; i++)
                {
                    result[i] = minValue;
                    remainingTotal -= minValue;
                }
            }

            // 使用两步法来分配剩余的数值
            // 第一步：随机分配
            int[] tempPoints = new int[n-1];
            for (int i = 0; i < n-1; i++)
            {
                // 为了避免某些份额为0，我们限制随机范围
                int maxPoint = remainingTotal - (n - i - 1);
                if (maxPoint > 0)
                {
                    tempPoints[i] = Random.Range(0, maxPoint);
                }
                else
                {
                    tempPoints[i] = 0;
                }
            }

            // 对分割点进行排序
            System.Array.Sort(tempPoints);

            // 第二步：根据分割点计算每份的大小
            int previousPoint = 0;
            for (int i = 0; i < n-1; i++)
            {
                result[i] += tempPoints[i] - previousPoint;
                previousPoint = tempPoints[i];
            }
            // 处理最后一份
            result[n-1] += remainingTotal - previousPoint;

            // 随机打乱结果数组的顺序，使得每个位置获得每个数值的概率相等
            for (int i = result.Length - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                (result[i], result[randomIndex]) = (result[randomIndex], result[i]);
            }

            return result;
        }

        private int[,] DistributeCount(Vector2Int dimension, int totalCount)
        {
            int totalGrids = dimension.x * dimension.y;
            // the random portion the total number will be split into
            int n = Random.Range(1, totalCount + 1);

            int[] splitCounts = SplitRandomly(totalCount, n);

            int[,] gridCount = new int[dimension.x, dimension.y];
            // 创建可用位置的列表
            List<Vector2Int> availablePositions = new List<Vector2Int>();
            for (int i = 0; i < dimension.x; i++)
            {
                for (int j = 0; j < dimension.y; j++)
                {
                    availablePositions.Add(new Vector2Int(i, j));
                }
            }

            // 随机分配每个数字
            foreach (int number in splitCounts)
            {
                if (availablePositions.Count == 0) break;

                // 随机选择一个位置
                int randomIndex = Random.Range(0, availablePositions.Count);
                Vector2Int position = availablePositions[randomIndex];

                // 放置数字并移除已使用的位置
                gridCount[position.x, position.y] = number;
                availablePositions.RemoveAt(randomIndex);
            }

            return gridCount;
        }
    }
}