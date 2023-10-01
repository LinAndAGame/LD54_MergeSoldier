using System.Threading.Tasks;

namespace MyGameUtility {
    public interface ICustomPoolFunctions<T> {
        T       CreateFunc();
        Task<T> AsyncCreateFunc();
        void    GetAction(T      cache);
        void    ReleaseAction(T  cache);
        void    DestroyAction(T  cache);
    }
}