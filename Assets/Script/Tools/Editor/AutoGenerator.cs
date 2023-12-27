using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TsingPigSDK
{

    public class AutoGenerator : Editor
    {
        private const string fileName = "StrDef";

        [MenuItem("�ҵĹ���/Prefab������ #W")]
        public static void AddressableAutoGen()
        {
            Object selectedObject = Selection.activeObject;
            if (selectedObject != null && selectedObject is GameObject)
            {
                GameObject prefabObj = (GameObject)selectedObject;

                string objectName = prefabObj.name;

                string codeLine = $"    public const string {Split(objectName)}_DATA_PATH = \"{objectName}\";";

                string scriptPath = $"Assets/Script/Config/{fileName}.cs";

                if (!File.Exists(scriptPath))
                {
                    using (StreamWriter writer = File.CreateText(scriptPath))
                    {
                        writer.WriteLine($"public static class {fileName}");
                        writer.WriteLine("{");
                        writer.WriteLine(codeLine);
                        writer.WriteLine("}");
                        Log.Info($"�����ű�: {scriptPath}");
                        AssetDatabase.Refresh();

                        return;
                    }
                }
                string[] contexts=File.ReadAllLines(scriptPath);

                if (contexts.Contains(codeLine))
                {
                    Log.Warning($"{scriptPath} �Ѿ�����{codeLine}");
                    return;
                }

                contexts[contexts.Length - 1] = codeLine+"\n}";
                File.WriteAllLines(scriptPath, contexts);
                Log.Info($"���Ӵ���:{codeLine} �� {scriptPath}");
                AssetDatabase.Refresh();
            }
            else
            {
                Log.Warning("��ѡ��һ��Ԥ���������ɴ���");
            }
        }
        
        private static string Split(string name)
        {
           return string.Concat(name.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToUpper();
        }
    }
   
}
