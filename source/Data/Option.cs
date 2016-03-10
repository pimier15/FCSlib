// Copyright (C) 2008-2016 Oliver Sturm <oliver@oliversturm.com>
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, see <http://www.gnu.org/licenses/>.
  

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace FCSlib.Data {
  public sealed class Option {
    private Option( ) { }

    public static Option<T> Some<T>(T value) {
      return new Option<T>(value);
    }

    public static readonly Option None = new Option( );
  }

  public static class OptionHelpers {
    public static Option<T> ToOption<T>(this T val) {
      return Option.Some(val);
    }

    public static Option<T> ToNotNullOption<T>(this T val) where T : class {
      return val != null ? val.ToOption( ) : Option.None;
    }
  }

  public sealed class Option<T> {
    private readonly T value;
    public T Value {
      get { return value; }
    }
    private readonly bool hasValue;
    public bool HasValue {
      get { return hasValue; }
    }
    public bool IsSome {
      get { return hasValue; }
    }
    public bool IsNone {
      get { return !hasValue; }
    }

    public Option(T value) {
      this.value = value;
      this.hasValue = true;
    }

    private Option( ) {
    }

    private static readonly Option<T> None = new Option<T>( );

    public static bool operator ==(Option<T> a, Option<T> b) {
      return a.HasValue == b.HasValue &&
        EqualityComparer<T>.Default.Equals(a.Value, b.Value);
    }
    public static bool operator !=(Option<T> a, Option<T> b) {
      return !(a == b);
    }

    public static implicit operator Option<T>(Option option) {
      return Option<T>.None;
    }

    public override int GetHashCode( ) {
      int hashCode = hasValue.GetHashCode( );
      if (hasValue)
        hashCode ^= value.GetHashCode( );
      return hashCode;
    }

    public override bool Equals(object obj) {
      return base.Equals(obj);
    }

    public Option<R> Bind<R>(Func<T, Option<R>> g) {
      if (IsNone)
        return Option.None;
      return g(Value);
    }
  }

}
