using System.Threading.Tasks;

namespace ControlV
{
    public interface IAction
    {
        string DisplayName { get; }
        string ResultText { get; }
        Task<bool> CanRun(string clipboardContent);
        Task Run(string clipboardContent);
    }
}