using System.Collections.Generic;
using System.Linq;
using _Scripts.GridSystem;
using UnityEngine;

public class Utils
{
    public static int CalculateMultiProbability(List<GridItemData> probabilityList)
    {
        var sortedDistribution = probabilityList.OrderBy(x => x.probability);

        float rand = Random.value;
        float accumulatedProbability = 0;
        float totalProbability = probabilityList.Sum(item => item.probability);

        foreach (var d in sortedDistribution)
        {
            float normalizedProbability = Normalize(d.probability, 0, totalProbability);
            accumulatedProbability += normalizedProbability;
            if (rand <= accumulatedProbability)
            {
                // Debug.Log($"accumulatedProbability: {accumulatedProbability}");
                // return int.Parse(d.id);
                return d.level;
            }
        }

        return default;
    }

    public static GridItemType CalculateMultiProbability(Dictionary<GridItemType, GridTypeData> probabilityDict)
    {
        var sortedDistribution = probabilityDict.OrderBy(x => x.Value.typeProbability);

        float rand = Random.value;
        float accumulatedProbability = 0;
        float totalProbability = probabilityDict.Values.Sum(data => data.typeProbability);

        foreach (var d in sortedDistribution)
        {
            float normalizedProbability = Normalize(d.Value.typeProbability, 0, totalProbability);
            accumulatedProbability += normalizedProbability;
            if (rand <= accumulatedProbability)
            {
                // Debug.Log($"accumulatedProbability: {accumulatedProbability}");
                return d.Key;
            }
        }

        return default;
    }

    public static float Normalize(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }
}
