namespace Application.Common.Email;

public static class EmailTemplate
{
    public static string BuildTemplate(
        string name,
        string email,
        string content,
        string? actionHtml = null
    )
    {
        return $@"
<div style=""margin: 0; padding: 0"" bgcolor=""#FFFFFF"">
  <table width=""100%"" height=""100%"" style=""min-width: 348px"" border=""0"" cellspacing=""0"" cellpadding=""0"" lang=""en"">
    <tbody>
      <tr height=""32""><td></td></tr>
      <tr align=""center"">
        <td>
          <table border=""0"" cellspacing=""0"" cellpadding=""0"" style=""padding-bottom: 20px; max-width: 516px; min-width: 220px"">
            <tbody>
              <tr>
                <td width=""8""></td>
                <td>
                  <div style=""border-style: solid; border-width: thin; border-color: #dadce0; border-radius: 8px; padding: 40px 20px;"" align=""center"">
                    <img src=""https://res.cloudinary.com/dbdozmkxv/image/upload/v1700127886/root/logo_vertical_sfyaha.png"" width=""150"" aria-hidden=""true"" style=""margin-bottom: 16px"" />
                    <div style=""font-family: 'Google Sans', Roboto, RobotoDraft, Helvetica, Arial, sans-serif; border-bottom: thin solid #dadce0; color: rgba(0, 0, 0, 0.87); line-height: 32px; padding-bottom: 24px; text-align: center;"">
                      <div style=""font-size: 24px;"">Xin chào <b>{name}</b></div>
                      <table align=""center"" style=""margin-top: 8px"">
                        <tr><td><span style=""font-family: 'Google Sans', Roboto, RobotoDraft, Helvetica, Arial, sans-serif; color: rgba(0, 0, 0, 0.87); font-size: 14px; line-height: 20px;"">{email}</span></td></tr>
                      </table>
                    </div>
                    <div style=""font-family: Roboto-Regular, Helvetica, Arial, sans-serif; font-size: 14px; color: rgba(0, 0, 0, 0.87); line-height: 20px; padding-top: 20px; text-align: center;"">
                      {content}
                      {actionHtml}
                    </div>
                  </div>
                  <div style=""text-align: center; font-family: Roboto-Regular, Helvetica, Arial, sans-serif; color: rgba(0, 0, 0, 0.54); font-size: 11px; line-height: 18px; padding-top: 12px;"">
                    <div>© {DateTime.Now.Year} Văn Quyết Mobile, <a style=""color: rgba(0, 0, 0, 0.54);"">Hanoi</a></div>
                  </div>
                </td>
                <td width=""8""></td>
              </tr>
            </tbody>
          </table>
        </td>
      </tr>
      <tr height=""32""><td></td></tr>
    </tbody>
  </table>
</div>";
    }

    private static string ButtonHtml(string link, string btnName)
    {
        return $@"<div style=""padding-top: 32px; text-align: center"">
    <a href=""{link}""
       style=""font-family: 'Google Sans', Roboto, RobotoDraft, Helvetica, Arial, sans-serif;
              line-height: 16px; color: #ffffff; font-weight: 400;
              text-decoration: none; font-size: 14px; display: inline-block;
              padding: 10px 24px; background-color: #4184f3; border-radius: 5px;
              min-width: 90px;"" target=""_blank"">{btnName}</a>
</div>";
    }

    private static string OtpHtml(string otp)
    {
        return $@"<div style=""padding-top: 32px; text-align: center;"">
    <div style=""font-size: 32px; letter-spacing: 5px; font-weight: bold; color: #202124;"">
        {otp}
    </div>
    <div style=""font-size: 12px; margin-top: 8px; color: #5f6368;"">
        Mã OTP có hiệu lực trong 10 phút.
    </div>
</div>";
    }

    public static string WelcomeEmail(string name, string email)
    {
        var content = "Cảm ơn bạn đã đăng ký tài khoản. Chúc bạn có trải nghiệm tuyệt vời cùng chúng tôi!";
        return BuildTemplate(name, email, content);
    }

    public static string ForgotPasswordEmail(string name, string email, string resetLink)
    {
        var content = "Chúng tôi đã nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn. " +
                         "Nếu bạn là người thực hiện yêu cầu này, vui lòng nhấn nút bên dưới để tiếp tục. " +
                         "Nếu không, bạn có thể bỏ qua email này.";
        var action = ButtonHtml(resetLink, "Đặt lại mật khẩu");
        return BuildTemplate(name, email, content, action);
    }

    public static string OtpEmail(string name, string email, string otpCode)
    {
        var content = "Mã OTP của bạn để xác thực là:";
        var action = OtpHtml(otpCode);
        return BuildTemplate(name, email, content, action);
    }

    public static string SecurityAlertEmail(string name, string email, string deviceInfo)
    {
        var content = $@"Chúng tôi phát hiện một lần đăng nhập bất thường từ thiết bị: <br/><b>{deviceInfo}</b>.<br/>
                         Nếu không phải bạn, hãy đổi mật khẩu ngay.";
        return BuildTemplate(name, email, content);
    }
}
