using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TsingPigSDK
{
    public class AutoGenerator : Editor
    {
        private const string fileName = "StrDef";

        [MenuItem("�ҵĹ���/�Զ�����AddressableConfig #W")]
        public static void AddressableAutoGenView()
        {
            Object selectedObject = Selection.activeObject;
            if(selectedObject != null && selectedObject is GameObject)
            {
                GameObject prefabObj = (GameObject)selectedObject;

                string objectName = prefabObj.name;

                GenerateCode(objectName);
            }
            else
            {
                Log.Warning("��ѡ��һ��Ԥ���������ɴ���");
            }
        }

        [MenuItem("�ҵĹ���/�Զ�����AddressableConfig(View��ͼ) #V")]
        public static void AddressableAutoGen()
        {
            Object selectedObject = Selection.activeObject;
            if(selectedObject != null && selectedObject is GameObject)
            {
                GameObject prefabObj = (GameObject)selectedObject;

                string objectName = prefabObj.name;

                GenerateCode(objectName, true);
            }
            else
            {
                Log.Warning("��ѡ��һ��Ԥ���������ɴ���");
            }
        }

        private static void GenerateCode(string objectName, bool isView = false)
        {
            string scriptPath = $"Assets/Script/Config/{fileName}.cs";

            string codeLine = $"    public const string {Split(objectName)}_DATA_PATH = \"{objectName}\";";

            if(!File.Exists(scriptPath))
            {
                using(StreamWriter writer = File.CreateText(scriptPath))
                {
                    writer.WriteLine($"public static class {fileName}");
                    writer.WriteLine("{");
                    writer.WriteLine(codeLine);
                    writer.WriteLine("}");
                    Log.Info($"�����ű�: {scriptPath}");
                }
            }
            else
            {
                string[] contexts = File.ReadAllLines(scriptPath);
                if(!contexts.Contains(codeLine))
                {
                    contexts[contexts.Length - 1] = codeLine + "\n}";
                    File.WriteAllLines(scriptPath, contexts);
                    Log.Info($"���Ӵ���:{codeLine} �� {scriptPath}");
                }
                else
                {
                    Log.Warning($"{scriptPath} �Ѿ�����{codeLine}");
                }
            }

            if(isView)
            {
                objectName = objectName.Substring(0, objectName.IndexOf("View"));

                string folderPath = $"Assets/Script/UI/{objectName}View";

                if(!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                // ���� INameObjPresenter.cs
                string presenterCode = $"using MVPFrameWork;\n" +
                    $"public interface I{objectName}Presenter : IPresenter {{}}";
                File.WriteAllText(Path.Combine(folderPath, $"I{objectName}Presenter.cs"), presenterCode);

                // ���� INameView.cs
                string viewCode = $"using MVPFrameWork;\n" +
                    $"public interface I{objectName}View : IView {{}}";
                File.WriteAllText(Path.Combine(folderPath, $"I{objectName}View.cs"), viewCode);

                // ���� NamePresenter.cs
                string presenterImplCode = $"using MVPFrameWork;\n" +
                    $"public class {objectName}Presenter : PresenterBase<I{objectName}View>, I{objectName}Presenter {{}}";

                File.WriteAllText(Path.Combine(folderPath, $"{objectName}Presenter.cs"), presenterImplCode);

                // ���� NameView.cs
                string viewImplCode = $"using MVPFrameWork;\n" +
                                      $"[ParentInfo(FindType.FindWithName, ConstDef.CANVAS)]\n" +
                                      $"public class {objectName}View : ViewBase<I{objectName}Presenter>, I{objectName}View {{ protected override void OnCreate() {{ throw new System.NotImplementedException(); }} }}";

                File.WriteAllText(Path.Combine(folderPath, $"{objectName}View.cs"), viewImplCode);
            }

            AssetDatabase.Refresh();
        }

        private static string Split(string name)
        {
            return string.Concat(name.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToUpper();
        }
    }
}