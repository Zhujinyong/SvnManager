
namespace Centa.SvnLog.Infrastructure.General.Exception
{
    /// <summary>
    /// 异常
    /// </summary>
    public class CustomException : System.Exception
    {
        private string _error;

        public CustomException(string message) : base(message)
        {
            _error = message;
        }

        public override string ToString()
        {
            return _error;
        }
    }
}
