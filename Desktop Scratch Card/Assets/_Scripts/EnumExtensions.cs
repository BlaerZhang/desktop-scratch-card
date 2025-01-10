using System;
using System.Linq;

public static class EnumExtensions
{
    public static T[] GetRandomUniqueValues<T>(int count) where T : Enum
    {
        // 首先获取枚举的所有值
        T[] allValues = (T[])Enum.GetValues(typeof(T));
        
        // 验证请求数量是否合理
        if (count > allValues.Length)
        {
            throw new ArgumentException($"请求的数量({count})超过了枚举中的总项数({allValues.Length})");
        }
        
        // 创建随机数生成器
        System.Random random = new System.Random();
        
        // 复制一份数组，避免修改原始数据
        T[] shuffled = new T[allValues.Length];
        allValues.CopyTo(shuffled, 0);
        
        // 使用Fisher-Yates洗牌算法的变体
        // 只需要处理到count个元素即可
        for (int i = 0; i < count; i++)
        {
            // 在[i, length-1]范围内随机选择一个位置
            int randomIndex = random.Next(i, shuffled.Length);
            
            // 交换当前位置和随机位置的元素
            (shuffled[i], shuffled[randomIndex]) = (shuffled[randomIndex], shuffled[i]);
        }
        
        // 返回前count个元素
        return shuffled.Take(count).ToArray();
    }
}