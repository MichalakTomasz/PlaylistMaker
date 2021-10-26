using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PlaylistMaker.Wrappers
{
    public class WrapperBase<TBase> : ValidationBase, IValidation, INotifyPropertyChanged
    {
        public TBase Model { get; set; }

        public WrapperBase(TBase model)
            => Model = model;

        public WrapperBase() { }

        protected void SetValue<T>(T value, [CallerMemberName]string propertyName = default)
        {
            Validate(value, propertyName);
            var property = typeof(TBase).GetProperty(propertyName);
            property.SetValue(Model, value);
            NotyfyPropertyChanged();
        }

        protected T GetValue<T>([CallerMemberName]string propertyName = default)
            => (T)typeof(TBase).GetProperty(propertyName).GetValue(Model);
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        public void NotyfyPropertyChanged([CallerMemberName]string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
