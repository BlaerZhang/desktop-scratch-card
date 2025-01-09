using System.Collections.Generic;
using _Scripts.GridSystem;
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

        // TODO: bug fix: number tends to accumualtes at the last grid
        private int[,] DistributeCount(Vector2Int dimension, int total)
        {
            int row = dimension.x;
            int column = dimension.y;
            int totalGrid = row * column;
            int[,] gridCount = new int[row, column];
            float[] randomWeights = new float[9];
            float weightSum = 0;

            for(int i = 0; i < totalGrid; i++)
            {
                randomWeights[i] = Random.Range(0f, 1f);
                weightSum += randomWeights[i];
            }

            int allocated = 0;
            for(int i = 0; i < row; i++)
            {
                for(int j = 0; j < column; j++)
                {
                    int index = i * column + j;
                    if(index == totalGrid - 1)
                    {
                        gridCount[i,j] = total - allocated;
                    }
                    else
                    {
                        gridCount[i,j] = Mathf.FloorToInt((randomWeights[index] / weightSum) * total);
                        allocated += gridCount[i,j];
                    }
                }
            }

            return gridCount;
        }
    }
}