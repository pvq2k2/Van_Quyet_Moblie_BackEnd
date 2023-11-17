using Van_Quyet_Moblie_BackEnd.Entities;

namespace Van_Quyet_Moblie_BackEnd.Helpers
{
    public class EmailTemplate
    {
        public static string MailTemplateString(string name, string email, string content, string link, string btnName)
        {
            return $@"
 <div style=""margin: 0; padding: 0"" bgcolor=""#FFFFFF"">
      <table
        width=""100%""
        height=""100%""
        style=""min-width: 348px""
        border=""0""
        cellspacing=""0""
        cellpadding=""0""
        lang=""en""
      >
        <tbody>
          <tr height=""32"" style=""height: 32px"">
            <td></td>
          </tr>
          <tr align=""center"">
            <td>
              <div><div></div></div>
              <table
                border=""0""
                cellspacing=""0""
                cellpadding=""0""
                style=""padding-bottom: 20px; max-width: 516px; min-width: 220px""
              >
                <tbody>
                  <tr>
                    <td width=""8"" style=""width: 8px""></td>
                    <td>
                      <div
                        style=""
                          border-style: solid;
                          border-width: thin;
                          border-color: #dadce0;
                          border-radius: 8px;
                          padding: 40px 20px;
                        ""
                        align=""center""
                        class=""m_-4601716730672635853mdv2rw""
                      >
                        <img
                          src=""https://res.cloudinary.com/dbdozmkxv/image/upload/v1700127886/root/logo_vertical_sfyaha.png""
                          width=""150""

                          aria-hidden=""true""
                          style=""margin-bottom: 16px""
                          class=""CToWUd""
                          data-bit=""iit""
                        />
                        <div
                          style=""
                            font-family: 'Google Sans', Roboto, RobotoDraft,
                              Helvetica, Arial, sans-serif;
                            border-bottom: thin solid #dadce0;
                            color: rgba(0, 0, 0, 0.87);
                            line-height: 32px;
                            padding-bottom: 24px;
                            text-align: center;
                            word-break: break-word;
                          ""
                        >
                          <div style=""font-size: 24px"">
                            Xin chào <b>{name}</b>
                          </div>
                          <table align=""center"" style=""margin-top: 8px"">
                            <tbody>
                              <tr style=""line-height: normal"">
                                <td>
                                  <span
                                    style=""
                                      font-family: 'Google Sans', Roboto,
                                        RobotoDraft, Helvetica, Arial,
                                        sans-serif;
                                      color: rgba(0, 0, 0, 0.87);
                                      font-size: 14px;
                                      line-height: 20px;
                                    ""
                                    >{email}</span>
                                </td>
                              </tr>
                            </tbody>
                          </table>
                        </div>
                        <div
                          style=""
                            font-family: Roboto-Regular, Helvetica, Arial,
                              sans-serif;
                            font-size: 14px;
                            color: rgba(0, 0, 0, 0.87);
                            line-height: 20px;
                            padding-top: 20px;
                            text-align: center;
                          ""
                        >
                        {content}
                          <div style=""padding-top: 32px; text-align: center"">
                            <a
                              href=""{link}""
                              style=""
                                font-family: 'Google Sans', Roboto, RobotoDraft,
                                  Helvetica, Arial, sans-serif;
                                line-height: 16px;
                                color: #ffffff;
                                font-weight: 400;
                                text-decoration: none;
                                font-size: 14px;
                                display: inline-block;
                                padding: 10px 24px;
                                background-color: #4184f3;
                                border-radius: 5px;
                                min-width: 90px;
                              ""
                              target=""_blank""
                              >{btnName}</a>
                          </div>
                        </div>
                      </div>
                      <div style=""text-align: left"">
                        <div
                          style=""
                            font-family: Roboto-Regular, Helvetica, Arial,
                              sans-serif;
                            color: rgba(0, 0, 0, 0.54);
                            font-size: 11px;
                            line-height: 18px;
                            padding-top: 12px;
                            text-align: center;
                          ""
                        >
                          <div style=""direction: ltr"">
                            © 2023 The Pizza House,
                            <a
                              class=""m_-4601716730672635853afal""
                              style=""
                                font-family: Roboto-Regular, Helvetica, Arial,
                                  sans-serif;
                                color: rgba(0, 0, 0, 0.54);
                                font-size: 11px;
                                line-height: 18px;
                                padding-top: 12px;
                                text-align: center;
                              ""
                              >Hanoi</a
                            >
                          </div>
                        </div>
                      </div>
                    </td>
                    <td width=""8"" style=""width: 8px""></td>
                  </tr>
                </tbody>
              </table>
            </td>
          </tr>
          <tr height=""32"" style=""height: 32px"">
            <td></td>
          </tr>
        </tbody>
      </table>
    </div>
";
        }
    }
}
