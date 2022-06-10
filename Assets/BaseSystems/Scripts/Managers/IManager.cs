
namespace Managers
{
    public interface IManager
    {
        void Initialize();
        void TransferData<T>(T data);
    }
}