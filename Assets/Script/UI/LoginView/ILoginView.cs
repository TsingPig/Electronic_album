using Michsky.MUIP;
using MVPFrameWork;
using TMPro;
using UnityEngine.UI;

public interface ILoginView : IView
{
    TMP_InputField InptRegisterInputAccount { get; set; }

    TMP_InputField InptRegisterInputPassWord { get; set; }

    TMP_InputField InptRegisterInputSurePassWord { get; set; }

    TMP_InputField InptLoginInputAccount { get; set; }

    TMP_InputField InptLoginInputPassWord { get; set; }

    Toggle TglLoginChangePasswordState { get; set; }

    ButtonManager BtnLogin { get; set; }

    ButtonManager BtnRegister { get; set; }
    
    ButtonManager BtnLoginSuper { get; set; }

    ButtonManager BtnRegisterInputAccountClear { get; set; }

    ButtonManager BtnRegisterInputPassWordClear { get; set; }

    ButtonManager BtnRegisterInputSurePassWordClear { get; set; }

    ButtonManager BtnLoginInputAccountClear { get; set; }

    ButtonManager BtnLoginInputPassWordClear { get; set; }

    TMP_InputField InptLoginSuperInputAccount { get; set; }

    TMP_InputField InptLoginSuperInputPassword { get; set; }
}