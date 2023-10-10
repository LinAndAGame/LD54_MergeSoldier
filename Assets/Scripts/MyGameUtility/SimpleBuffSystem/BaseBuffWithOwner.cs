namespace MyGameUtility {
    public class BaseBuffWithOwner<T> : BaseBuff {
        protected T Owner;

        public BaseBuffWithOwner(T owner) {
            Owner = owner;
        }
    }
}