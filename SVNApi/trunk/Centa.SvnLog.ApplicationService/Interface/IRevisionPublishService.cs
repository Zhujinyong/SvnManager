using Centa.SvnLog.Common;
using Centa.SvnLog.Infrastructure.General.Page;
using Centa.SvnLog.Infrastructure.Model;
using System.Collections.Generic;

namespace Centa.SvnLog.ApplicationService.Interface
{
    /// <summary>
    /// svn发布到jenkins
    /// </summary>
    public interface IRevisionPublishService
    {
        List<dynamic> GetRevisionPublishList(string logID);

        ExcuteMessage AddRevisionPublish(SvnRevisionPublishModel model);
    }
}
