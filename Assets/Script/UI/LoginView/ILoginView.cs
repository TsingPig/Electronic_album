using Michsky.MUIP;
using MVPFrameWork;
using TMPro;
using UnityEngine.UI;

public interface ILoginView : IView
{
    TMP_Text TxtRegisterInputAccount { get; set; }

    TMP_InputField TxtRegisterInputPassWord { get; set; }

    TMP_InputField TxtRegisterInputSurePassWord { get; set; }

    TMP_Text TxtLoginInputAccount { get; set; }

    TMP_InputField TxtLoginInputPassWord { get; set; }

    Toggle TglLoginChangePasswordState { get; set; }

    ButtonManager BtnLogin { get; set; }

    ButtonManager BtnRegister { get; set; }
}
