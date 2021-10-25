﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Playlist.Wrappers
{
    public class WrapperBase<TBase> : ValidationBase, IValidation
    {
        public TBase Model { get; set; }

        public WrapperBase(TBase model)
            => Model = model;

        public WrapperBase() { }

        public void SetModel(TBase model) => Model = model;
        protected void SetValue<T>(T value, [CallerMemberName]string propertyName = default)
        {
            Validate(value, propertyName);
            var property = typeof(TBase).GetProperty(propertyName);
            property.SetValue(Model, value);
        }

        protected T GetValue<T>([CallerMemberName]string propertyName = default)
            => (T)typeof(TBase).GetProperty(propertyName).GetValue(Model);
    }
}
