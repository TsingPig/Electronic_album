using Michsky.MUIP;
using MVPFrameWork;
using UnityEngine.UI;

public interface ILoginView : IView
{
    Text TxtRegisterInputAccount { get; set; }

    Text TxtRegisterInputPassWord { get; set; }

    Text TxtRegisterInputSurePassWord { get; set; }

    Text TxtLoginInputAccount { get; set; }

    Text TxtLoginInputPassWord { get; set; }

    Toggle TglLoginChangePasswordState { get; set; }

    ButtonManager BtnLogin { get; set; }
}
