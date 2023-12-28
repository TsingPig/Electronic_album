using UnityEngine;

namespace TsingPigSDK
{
    public static class Texture2DExtensions
    {
        public static Texture2D RandomGenerate(this Texture2D texture)
        {
            if(texture == null)
            {
                Debug.LogError("Texture2D is null.");
                return null;
            }

            int width = texture.width;
            int height = texture.height;

            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    // 生成随机颜色
                    Color randomColor = new Color(Random.value, Random.value, Random.value, 1.0f);

                    texture.SetPixel(x, y, randomColor);
                }
            }
            texture.Apply();
            return texture;
        }
    }
}