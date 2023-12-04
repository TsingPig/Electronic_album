using Michsky.MUIP;
using MVPFrameWork;
using TMPro;
using UnityEngine.UI;

public interface ILoginView : IView
{
    TMP_InputField TxtRegisterInputAccount { get; set; }

    TMP_InputField TxtRegisterInputPassWord { get; set; }

    TMP_InputField TxtRegisterInputSurePassWord { get; set; }

    TMP_InputField TxtLoginInputAccount { get; set; }

    TMP_InputField TxtLoginInputPassWord { get; set; }

    Toggle TglLoginChangePasswordState { get; set; }

    ButtonManager BtnLogin { get; set; }

    ButtonManager BtnRegister { get; set; }

    ButtonManager BtnRegisterInputAccountClear { get; set; }

    ButtonManager BtnRegisterInputPassWordClear { get; set; }

    ButtonManager BtnRegisterInputSurePassWordClear { get; set; }

    ButtonManager BtnLoginInputAccountClear { get; set; }

    ButtonManager BtnLoginInputPassWordClear { get; set; }
}
