using TsingPigSDK;
using UnityEngine.UI;

public class TestPanel : BasePanel
{
    public override void OnEntry()
    {
        UIManager.Instance.GetOrAddComponentInChilden
           <Button>("Exit").onClick.AddListener(() =>
           {
               UIManager.Instance.Pop();
           });
        UIManager.Instance.GetOrAddComponentInChilden
           <Button>("Test").onClick.AddListener(() =>
           {
               Log.Info("Test");
           });
        UIManager.Instance.GetOrAddComponentInChilden
           <Text>("Text").text = "按下Esc关闭，按下A再次打开";
    }

}
